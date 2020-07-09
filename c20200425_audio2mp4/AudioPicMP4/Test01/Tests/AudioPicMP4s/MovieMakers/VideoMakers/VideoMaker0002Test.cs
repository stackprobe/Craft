using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.AudioPicMP4s.MovieMakers;
using Charlotte.AudioPicMP4s.MovieMakers.VideoMakers;
using Charlotte.AudioPicMP4s.MovieMakers.VideoMakers.VideoImageMakers.Backgrounds;
using Charlotte.AudioPicMP4s.MovieMakers.VideoMakers.VideoImageMakers.Foregrounds;
using Charlotte.AudioPicMP4s.MovieMakers.VideoMakers.VideoImageMakers.Options;
using Charlotte.AudioPicMP4s;

namespace Charlotte.Tests.AudioPicMP4s.MovieMakers.VideoMakers
{
	public class VideoMaker0002Test
	{
		public void Test01()
		{
			Test01_a(@"C:\var\mp4\mp4\ddd.wav", @"C:\wb2\20191204_ジャケット的な\北へ.jpg", @"C:\temp\1.mp4");
			Test01_a(@"C:\wb2\20200423_動画テストデータ\新田美恵子のMINELVAカメラのCF.wav", @"C:\wb2\20200423_動画テストデータ\新田美恵子のMINELVAカメラのCF.jpg", @"C:\temp\2.mp4");
			Test01_a(@"C:\var\mp4\mp4\hbn.wav", @"C:\wb2\20191204_ジャケット的な\灰羽連盟.jpg", @"C:\temp\3.mp4");
			Test01_a(@"C:\wb2\20200423_動画テストデータ\まちぶせ.wav", @"C:\wb2\20200423_動画テストデータ\まちぶせ.jpg", @"C:\temp\4.mp4");
		}

		private void Test01_a(string mediaFile, string imgFile, string mp4File)
		{
			new MovieMaker0002().MakeMovie(mediaFile, mp4File, false, new VideoMaker0002(
				new Background0001(imgFile),
				new VIMFadeIn(AudioPicMP4Props.FPS * 2, 20),
				new VIMCurtain(0.1),
				new Foreground0001(),
				new VIMFadeOut(10, 10)
				));
		}

		public void Test02()
		{
			new MovieMaker0002().MakeMovie(
				@"C:\wb2\20200423_動画テストデータ\新田美恵子のMINELVAカメラのCF.wav",
				@"C:\temp\2-1.mp4",
				false,
				new VideoMaker0002(
					new Background0001(@"C:\wb2\20200423_動画テストデータ\新田美恵子のMINELVAカメラのCF.jpg", 1.0, 0.5),
					new VIMFadeIn(10, 20),
					new VIMCurtain(0.25),
					new Foreground0001(),
					new VIMFadeOut(10, 10)
				));
		}

		public void Test03()
		{
			Test03_a(
				@"C:\temp\3-1.mp4",
				new VideoMaker0002(
					new Background0002(@"C:\wb2\20200423_動画テストデータ\新田美恵子のMINELVAカメラのCF.jpg", true),
					new VIMFadeIn(10, 10),
					new VIMCurtain(0.25),
					new Foreground0001(),
					new VIMFadeOut(10, 10)
				));
		}

		public void Test03_2()
		{
			Test03_a(
				@"C:\temp\3-1_2.mp4",
				new VideoMaker0002(
					new Background0002(@"C:\wb2\20200423_動画テストデータ\新田美恵子のMINELVAカメラのCF.jpg", true),
					new VIMFadeIn(10, 10),
					new VIMCurtain(0.25),
					new Foreground0002(),
					new VIMFadeOut(10, 10)
				));
		}

		private void Test03_a(string mp4File, VideoMaker0002 vm)
		{
			new MovieMaker0002().MakeMovie(
				@"C:\wb2\20200423_動画テストデータ\新田美恵子のMINELVAカメラのCF.wav",
				mp4File,
				false,
				vm
				);
		}

		public void Test04_01()
		{
			new MovieMaker0002().MakeMovie(
				@"C:\wb2\20200423_動画テストデータ\まちぶせ.wav",
				@"C:\temp\荒井由実 - まちぶせ (A).mp4",
				false,
				new VideoMaker0002(
					new Background0001(@"C:\wb2\20200423_動画テストデータ\まちぶせ.jpg"),
					new VIMFadeIn(10, 10),
					new VIMCurtain(0.25),
					new Foreground0001(),
					new VIMFadeOut(10, 10)
					)
				);
		}

		public void Test04_02()
		{
			new MovieMaker0002().MakeMovie(
				@"C:\wb2\20200423_動画テストデータ\まちぶせ.wav",
				@"C:\temp\荒井由実 - まちぶせ (B).mp4",
				false,
				new VideoMaker0002(
					new Background0002(@"C:\wb2\20200423_動画テストデータ\まちぶせ_02.png", false),
					new VIMFadeIn(10, 10),
					new VIMCurtain(0.25),
					new Foreground0002(),
					new VIMFadeOut(10, 10)
					)
				);
		}
	}
}
