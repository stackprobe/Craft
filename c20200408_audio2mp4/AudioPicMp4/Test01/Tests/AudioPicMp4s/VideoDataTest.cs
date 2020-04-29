using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.AudioPicMP4s;
using Charlotte.Tools;
using System.IO;
using Charlotte.AudioPicMP4s.Effects;

namespace Charlotte.Tests.AudioPicMP4s
{
	public class VideoDataTest
	{
		public void Test01()
		{
			using (WorkingDir wd = new WorkingDir())
			{
				string imgDir = wd.GetPath("img");

				FileTools.CreateDir(imgDir);

				PictureData picture = new PictureData(new Canvas2(@"C:\wb2\20191204_ジャケット的な\北へ.jpg"), 1920, 1080);
				WaveData wave = new WaveData(@"C:\var\mp4\mp4\ddd.wav");

				VideoData video = new VideoData(picture, (wave.Length * VideoData.FPS) / wave.WavHz, imgDir);
				IEffect effect = new SpectrumEffect01(wave);

				video.MakeImages(new VideoData.FadeInOutInfo()
				{
					StartMargin = 2 * VideoData.FPS,
					EndMargin = -1,
					FadeInOutSpan = 20,
				},
				effect,
				new VideoData.FadeInOutInfo()
				{
					StartMargin = -1,
					EndMargin = 10,
					FadeInOutSpan = 10,
				});

				ProcessTools.Batch(new string[]
				{
					@"C:\app\ffmpeg-4.1.3-win64-shared\bin\ffmpeg.exe -r 20 -i %%d.jpg -i C:\var\mp4\mp4\ddd.wav ..\out.mp4",
				},
				imgDir
				);

				File.Copy(wd.GetPath("out.mp4"), @"C:\temp\1.mp4", true);
			}
		}
	}
}
