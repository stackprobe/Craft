using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;
using System.Drawing.Imaging;

namespace Charlotte.MP4Gens
{
	public class MP4Gen0001
	{
		private void Main_Go_2(string audioDir, string videoDir, string wDir)
		{
			if (Directory.Exists(audioDir) == false)
				throw null;

			if (Directory.Exists(videoDir) == false)
				throw null;

			string midVideoDir = Path.Combine(wDir, "video");

			FileTools.Delete(wDir);
			FileTools.CreateDir(wDir);
			FileTools.CreateDir(midVideoDir);

			string audioWavFile = Path.Combine(audioDir, "Wave.wav");

			if (File.Exists(audioWavFile) == false)
				throw null;

			int frame;

			Console.WriteLine("*1"); // test

			for (frame = 0; ; frame++)
			{
				string rFile = Path.Combine(videoDir, string.Format("{0}.bmp", frame));
				string wFile = Path.Combine(midVideoDir, string.Format("{0}.jpg", frame));

				if (File.Exists(rFile) == false)
					break;

				if (frame % 100 == 0)
					Console.WriteLine("frame: " + frame); // test

				new Canvas2(rFile).Save(wFile, ImageFormat.Jpeg, 90);
			}
			if (frame < 1)
				throw null;

			Console.WriteLine("*2"); // test

			ProcessTools.Batch(new string[]
			{
				Consts.FFMPEG_FILE + " -r 20 -i %%d.jpg ..\\video.mp4",
			},
			midVideoDir
			);

			if (File.Exists(Path.Combine(wDir, "video.mp4")) == false)
				throw null;

			Console.WriteLine("*3"); // test

			ProcessTools.Batch(new string[]
			{
				Consts.FFMPEG_FILE + " -i video.mp4 -i " + audioWavFile + " -map 0:0 -map 1:0 -vcodec copy movie.mp4",
			},
			wDir
			);

			if (File.Exists(Path.Combine(wDir, "movie.mp4")) == false)
				throw null;

			Console.WriteLine("*4"); // test
		}

		private void Main_Go(string audioDir, string videoRootDir, string wRootDir, string title)
		{
			FileTools.Delete(wRootDir);
			FileTools.CreateDir(wRootDir);

			// ----

			foreach (string videoDir in Directory.GetDirectories(videoRootDir))
			{
				string videoLocalDir = Path.GetFileName(videoDir);
				string wDir = Path.Combine(wRootDir, videoLocalDir);

				Main_Go_2(audioDir, videoDir, wDir);

				File.Move(Path.Combine(wDir, "movie.mp4"), Path.Combine(wRootDir, title + "_" + videoLocalDir + ".mp4"));
			}
		}

		public void Main01()
		{
			Main_Go(
				@"C:\temp\a1001",
				@"C:\temp\a2001",
				@"C:\temp\a3001",
				"Rock7n-Rouge"
				);

			Main_Go(
				@"C:\temp\a1002",
				@"C:\temp\a2002",
				@"C:\temp\a3002",
				"悪女"
				);
		}
	}
}
