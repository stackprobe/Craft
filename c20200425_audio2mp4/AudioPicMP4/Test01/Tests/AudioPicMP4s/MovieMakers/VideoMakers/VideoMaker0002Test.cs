using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.AudioPicMP4s.MovieMakers;
using Charlotte.AudioPicMP4s.MovieMakers.VideoMakers;
using Charlotte.AudioPicMP4s.MovieMakers.VideoMakers.VideoImageMakers.Backgrounds01;
using Charlotte.AudioPicMP4s.MovieMakers.VideoMakers.VideoImageMakers.Foregrounds;
using Charlotte.AudioPicMP4s.MovieMakers.VideoMakers.VideoImageMakers.Options;

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
				new Background0001(),
				new VIMFadeIn(10, 20),
				new Foreground0001(),
				new VIMFadeOut(10, 10)
				));
		}
	}
}
