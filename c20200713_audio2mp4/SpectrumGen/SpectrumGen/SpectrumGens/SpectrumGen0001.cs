using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;
using Charlotte.SpectrumGens.Engines;

namespace Charlotte.SpectrumGens
{
	public class SpectrumGen0001
	{
		private static void WriteRow(CsvFileWriter writer, SpectrumGraph graph)
		{
			foreach (double level in graph.Spectra)
				writer.WriteCell(level.ToString("F9"));

			writer.EndRow();
		}

		private void Main_Go(string rMP3File, string rJpgFile, string wDir)
		{
			Console.WriteLine("< " + rMP3File); // test
			Console.WriteLine("< " + rJpgFile); // test
			Console.WriteLine("> " + wDir); // test

			if (File.Exists(rMP3File) == false)
				throw new Exception("no rMP3File: " + rMP3File);

			if (File.Exists(rJpgFile) == false)
				throw new Exception("no rImgFile: " + rJpgFile);

			FileTools.Delete(wDir);
			FileTools.CreateDir(wDir);

			string wCsvFile = Path.Combine(wDir, "Spectrum.csv");
			string wCsvFile_L = Path.Combine(wDir, "Spectrum_L.csv");
			string wCsvFile_R = Path.Combine(wDir, "Spectrum_R.csv");
			string wWavFile = Path.Combine(wDir, "Wave.wav");
			string wJpgFile = Path.Combine(wDir, "Jacket.jpg");

			Console.WriteLine("*1"); // test
			MP3Conv.MP3FileToWavFile(rMP3File, wWavFile);
			Console.WriteLine("*2"); // test
			WaveData wavDat = new WaveData(wWavFile);
			Console.WriteLine("*3"); // test

			using (CsvFileWriter writer = new CsvFileWriter(wCsvFile))
			using (CsvFileWriter writer_L = new CsvFileWriter(wCsvFile_L))
			using (CsvFileWriter writer_R = new CsvFileWriter(wCsvFile_R))
			{
				for (int frame = 0; ; frame++)
				{
					int wavPartPos = (int)((frame * 1.0 / Consts.FPS + Consts.AUDIO_DELAY_SEC) * wavDat.WavHz);

					if (wavDat.Length <= wavPartPos)
						break;

					if (frame % (Consts.FPS * 10) == 0)
						Console.WriteLine("frame: " + frame); // test

					wavDat.SetWavPart(wavPartPos);

					SpectrumGraph graph = new SpectrumGraph(hz => wavDat.GetSpectrum(hz));
					SpectrumGraph graph_L = new SpectrumGraph(hz => wavDat.GetSpectrum_L(hz));
					SpectrumGraph graph_R = new SpectrumGraph(hz => wavDat.GetSpectrum_R(hz));

					WriteRow(writer, graph);
					WriteRow(writer_L, graph_L);
					WriteRow(writer_R, graph_R);
				}
			}
			Console.WriteLine("*4"); // test

			File.Copy(rJpgFile, wJpgFile);

			Console.WriteLine("done"); // test
		}

		public void Main01()
		{
			Main_Go(
				@"C:\wb2\20200710_動画よっしーさんからのテスト用データ\2-04 Rock'n Rouge.mp3",
				@"C:\wb2\20200710_動画よっしーさんからのテスト用データ\2-04 Rock'n Rouge.jpg",
				@"C:\temp\a1001"
				);

			Main_Go(
				@"C:\wb2\20200710_動画よっしーさんからのテスト用データ\悪女.mp3",
				@"C:\wb2\20200710_動画よっしーさんからのテスト用データ\悪女.jpg",
				@"C:\temp\a1002"
				);
		}
	}
}
