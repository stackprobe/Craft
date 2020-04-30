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
			//Test01_a(@"C:\var\mp4\mp4\ddd.wav", @"C:\wb2\20191204_ジャケット的な\北へ.jpg", @"C:\temp\1.mp4");
			Test01_a(@"C:\wb2\20200423_動画テストデータ\新田美恵子のMINELVAカメラのCF.wav", @"C:\wb2\20200423_動画テストデータ\新田美恵子のMINELVAカメラのCF.jpg", @"C:\temp\1.mp4");
		}

		public void Test01_a(string wavFile, string imgFile, string mp4File)
		{
			using (WorkingDir wd = new WorkingDir())
			{
				string imgDir = wd.GetPath("img");

				FileTools.CreateDir(imgDir);

				PictureData picture = new PictureData(new Canvas2(imgFile), 1920, 1080);
				WaveData wave = new WaveData(wavFile);

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

				File.Copy(wavFile, wd.GetPath("audio.wav"));

				ProcessTools.Batch(new string[]
				{
					@"C:\app\ffmpeg-4.1.3-win64-shared\bin\ffmpeg.exe -r 20 -i %%d.jpg -i ..\audio.wav ..\out.mp4",
				},
				imgDir
				);

				File.Copy(wd.GetPath("out.mp4"), mp4File, true);
			}
		}
	}
}
