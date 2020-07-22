using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Tools;
using Charlotte.Utils;
using System.Threading;
using System.Drawing.Imaging;

namespace Charlotte
{
	public class ConvFileMain
	{
		public string InputDir;
		public string OutputDir;
		public string PresumeAudioRelFile;

		// <---- prm

		public string AudioFile;
		private string JacketFile;
		private string MovieFile;

		public void Perform()
		{
			this.AudioFile = Path.Combine(this.InputDir, this.PresumeAudioRelFile);

			{
				string localFile = Path.GetFileName(this.AudioFile);

				if (localFile.StartsWith("."))
					throw new Exception("隠しファイルは除外する。");
			}

			{
				string ext = Path.GetExtension(this.AudioFile);

				if (Consts.AUDIO_EXTS.Any(v => StringTools.EqualsIgnoreCase(v, ext)) == false)
					throw new Exception("音楽ファイルではない。");
			}

			{
				string file = this.TryGetJacketFile();

				if (file == null)
					throw new Exception("ジャケット画像が見つからない。");

				this.JacketFile = file;
			}

			{
				string file = Path.Combine(this.OutputDir, this.PresumeAudioRelFile);

				file = FileUtils.EraseExt(file) + Consts.MOVIE_EXT;

				string dir = Path.GetDirectoryName(file);

				// memo: dirの親と同名のファイルが存在するとディレクトリを作成出来ない。
				// -> そのファイルを削除して良いか判断出来ないので、あえて削除せず、失敗させる。

				FileTools.CreateDir(dir);
				FileTools.Delete(file);
				File.WriteAllBytes(file, BinTools.EMPTY); // テスト作成

				if (File.Exists(file) == false)
					throw new Exception("出力先の動画ファイルを作成出来ません。");

				FileTools.Delete(file); // テスト削除

				if (File.Exists(file))
					throw new Exception("出力先の動画ファイルをクリア出来ません。");

				this.MovieFile = file;
			}

			this.Conv();
		}

		private string TryGetJacketFile()
		{
			string fileBase = FileUtils.EraseExt(this.AudioFile);

			foreach (string ext in Consts.JACKET_EXTS)
			{
				string file = fileBase + ext;

				if (File.Exists(file))
					return file;
			}
			return null;
		}

		private string WavFile;
		private string MasterWavFile;
		private string SpectrumFile;
		private string SpectrumFile_L;
		private string SpectrumFile_R;
		private string VideoBmpDir;
		private string VideoJpgDir;

		private void Conv()
		{
			using (WorkingDir wd = new WorkingDir())
			{
				this.WavFile = wd.MakePath() + ".wav";
				this.MasterWavFile = wd.MakePath() + ".wav";
				this.SpectrumFile = wd.MakePath() + ".csv";
				this.SpectrumFile_L = wd.MakePath() + ".csv";
				this.SpectrumFile_R = wd.MakePath() + ".csv";
				this.VideoBmpDir = wd.MakePath();
				this.VideoJpgDir = wd.MakePath();

				// TODO キャンセルのチェック, Cancelled の throw

				this.MakeWavFile();
				this.MasteringWavFile();
				this.MakeSpectrumFile();
				this.MakeVideoBmp();
				this.MakeVideoJpg();
				this.MakeMovieFile();
			}
		}

		private void MakeWavFile()
		{
			using (WorkingDir wd = new WorkingDir())
			{
				string audioExt = Path.GetExtension(this.AudioFile);

#if true // .wav にもサラウンドとかある様なのでステレオにする。
				File.Copy(this.AudioFile, wd.GetPath("1" + audioExt));

				ProcessTools.Batch(new string[]
				{
					Consts.FFMPEG_FILE + " -i 1" + audioExt + " -ac 2 2.wav",
				},
				wd.GetPath(".")
				);
#else
				if (StringTools.EqualsIgnoreCase(audioExt, ".wav"))
				{
					File.Copy(this.AudioFile, wd.GetPath("2.wav"));
				}
				else
				{
					File.Copy(this.AudioFile, wd.GetPath("1" + audioExt));

					ProcessTools.Batch(new string[]
					{
						Consts.FFMPEG_FILE + " -i 1" + audioExt + " 2.wav",
					},
					wd.GetPath(".")
					);
				}
#endif
				File.Move(wd.GetPath("2.wav"), this.WavFile);
			}
		}

		private void MasteringWavFile()
		{
			using (WorkingDir wd = new WorkingDir())
			{
				File.Copy(this.WavFile, wd.GetPath("1.wav"));

				ProcessTools.Batch(new string[]
				{
					Consts.MASTER_FILE + " /E " + Consts.EV_STOP_MASTER + " 1.wav 2.wav 3.txt",
				},
				wd.GetPath(".")
				);

				// TODO log 3.txt as report

				if (File.Exists(wd.GetPath("2.wav"))) // ? 音量調整した。
				{
					// noop
				}
				else // ? 音量調整しなかった。
				{
					File.Copy(wd.GetPath("1.wav"), wd.GetPath("2.wav")); // そのままコピー
				}

				File.Move(wd.GetPath("2.wav"), this.MasterWavFile);
			}
		}

		private void MakeSpectrumFile()
		{
			WaveData wavDat = new WaveData(this.MasterWavFile);

			using (CsvFileWriter writer = new CsvFileWriter(this.SpectrumFile))
			//using (CsvFileWriter writer_L = new CsvFileWriter(this.SpectrumFile_L)) // 不使用
			//using (CsvFileWriter writer_R = new CsvFileWriter(this.SpectrumFile_R)) // 不使用
			{
				for (int frame = 0; ; frame++)
				{
					int wavPartPos = (int)((frame * 1.0 / Consts.FPS + Consts.AUDIO_DELAY_SEC) * wavDat.WavHz);

					if (wavDat.Length <= wavPartPos)
						break;

					wavDat.SetWavPart(wavPartPos);

					SpectrumGraph graph = new SpectrumGraph(hz => wavDat.GetSpectrum(hz));
					//SpectrumGraph graph_L = new SpectrumGraph(hz => wavDat.GetSpectrum_L(hz)); // 不使用
					//SpectrumGraph graph_R = new SpectrumGraph(hz => wavDat.GetSpectrum_R(hz)); // 不使用

					CsvUtils.WriteRow(writer, graph);
					//CsvUtils.WriteRow(writer_L, graph_L); // 不使用
					//CsvUtils.WriteRow(writer_R, graph_R); // 不使用
				}
			}
		}

		private void MakeVideoBmp()
		{
			try
			{
				Ground.I.EvCancellable_N.Set();
				Ground.I.EvMessage_StartGenVideo.Set();

				Thread.Sleep(5000); // 予告期間

				Ground.I.EvMessage_GenVideoRunning.Set();

				using (WorkingDir wd = new WorkingDir())
				{
					string wdJacketFile = wd.GetPath("2" + Path.GetExtension(this.JacketFile));

					File.Copy(this.SpectrumFile, wd.GetPath("1.csv"));
					File.Copy(this.JacketFile, wdJacketFile);

					FileTools.CreateDir(wd.GetPath("3"));

					ProcessTools.Batch(new string[]
					{
						"START /WAIT " + Ground.I.ConvGenVideoFile + " CS-ConvGenVideo 1.csv " + Path.GetFileName(wdJacketFile) + " 3 4.flg",
					},
					wd.GetPath(".")
					);

					if (File.Exists(wd.GetPath("4.flg")) == false)
						throw new Exception("映像データ生成プロセスが正常に動作しなかったようです。");

					FileTools.MoveDir(wd.GetPath("3"), this.VideoBmpDir);
				}
			}
			finally
			{
				Ground.I.EvCancellable_Y.Set();
				Ground.I.EvMessage_Normal.Set();
			}
		}

		private void MakeVideoJpg()
		{
			FileTools.CreateDir(this.VideoJpgDir);

			int frame;

			for (frame = 0; ; frame++)
			{
				string rFile = Path.Combine(this.VideoBmpDir, string.Format("{0}.bmp", frame));
				string wFile = Path.Combine(this.VideoJpgDir, string.Format("{0}.jpg", frame));

				if (File.Exists(rFile) == false)
					break;

				new Canvas2(rFile).Save(wFile, ImageFormat.Jpeg, Consts.JPEG_QUALITY);
			}
			if (frame < 1)
				throw new Exception("映像データが空です。");
		}

		private void MakeMovieFile()
		{
			using (WorkingDir wd = new WorkingDir())
			{
				FileTools.CopyDir(this.VideoJpgDir, wd.GetPath("1"));

				ProcessTools.Batch(new string[]
				{
					Consts.FFMPEG_FILE + " -r " + Consts.FPS + " -i %%d.jpg ..\\2.mp4",
				},
				wd.GetPath("1")
				);

				if (File.Exists(wd.GetPath("2.mp4")) == false)
					throw new Exception("映像ファイルの生成に失敗しました。");

				File.Copy(this.MasterWavFile, wd.GetPath("3.wav"));

				ProcessTools.Batch(new string[]
				{
					Consts.FFMPEG_FILE + " -i 2.mp4 -i 3.wav -map 0:0 -map 1:0 -vcodec copy 4.mp4",
				},
				wd.GetPath(".")
				);

				if (File.Exists(wd.GetPath("4.mp4")) == false)
					throw new Exception("動画ファイルの生成に失敗しました。");

				ProcessTools.Batch(new string[]
				{
					"ECHO Y|CACLS 4.mp4 /P Users:F Guest:F",
				},
				wd.GetPath(".")
				);

				if (File.Exists(this.MovieFile)) // 2bs
					throw null;

				File.Move(wd.GetPath("4.mp4"), this.MovieFile);

				if (File.Exists(this.MovieFile) == false)
					throw new Exception("動画ファイルの書き出しに失敗しました。");
			}
		}
	}
}
