using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;
using System.Drawing.Imaging;

namespace Charlotte.AudioPicMP4s.MovieMakers
{
	public class MovieMaker0002
	{
		private WorkingDir WD;
		private FFmpeg FFmpeg;

		public void MakeMovie(string mediaFile, string destMP4File, bool masterFlag, IVideoMaker videoMaker)
		{
			mediaFile = FileTools.MakeFullPath(mediaFile);
			destMP4File = FileTools.MakeFullPath(destMP4File);

			//masterFlag

			if (videoMaker == null)
				throw new Exception("videoMaker is null");

			FileTools.Delete(destMP4File);

			if (File.Exists(mediaFile) == false)
				throw new Exception("no mediaFile: " + mediaFile);

			// ----

			this.WD = new WorkingDir();
			try
			{
				this.FFmpeg = new FFmpeg();
				try
				{
					this.FFmpeg.Audio = new FFmpegMedia();
					try
					{
						this.MakeMovie_Main(mediaFile, destMP4File, masterFlag, videoMaker);
					}
					finally
					{
						this.FFmpeg.Audio.Dispose();
						this.FFmpeg.Audio = null;
					}
				}
				finally
				{
					this.FFmpeg.Dispose();
					this.FFmpeg = null;
				}
			}
			finally
			{
				this.WD.Dispose();
				this.WD = null;
			}
		}

		private const int JPEG_QUALITY = 90;

		private void MakeMovie_Main(string mediaFile, string destMP4File, bool masterFlag, IVideoMaker videoMaker)
		{
			WaveData wave;

			{
				string wavFile = this.WD.GetPath("audio.wav");
				string masterWavFile = this.WD.GetPath("audio2.wav");

				FFmpegConv.MakeWavFile(mediaFile, wavFile);

				if (masterFlag && MasterUtils.Mastering(wavFile, masterWavFile))
				{
					wave = new WaveData(masterWavFile);
					this.FFmpeg.Audio.PutAudioFile(masterWavFile);
				}
				else
				{
					wave = new WaveData(wavFile);
					this.FFmpeg.Audio.PutAudioFile(mediaFile);
				}
			}

			int frame = 0;

			using (WorkingDir mvWd = new WorkingDir())
			{
				videoMaker.MakeVideo(wave, mvWd, image =>
				{
					image.Save(this.FFmpeg.GetImageFile(frame), ImageFormat.Jpeg, JPEG_QUALITY);
					frame++;

					GC.Collect();
				});
			}

			this.FFmpeg.MakeMovie();

			ProcessTools.Batch(new string[]
			{
				string.Format(@"ECHO Y|CACLS {0} /P Users:F Guest:F", this.FFmpeg.GetMovieFile()),
			});

			File.Move(this.FFmpeg.GetMovieFile(), destMP4File);
		}
	}
}
