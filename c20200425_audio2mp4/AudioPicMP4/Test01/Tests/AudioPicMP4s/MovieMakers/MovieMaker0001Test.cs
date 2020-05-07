using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.AudioPicMP4s.MovieMakers;

namespace Charlotte.Tests.AudioPicMP4s.MovieMakers
{
	public class MovieMaker0001Test
	{
		public void Test01()
		{
			Test01_a(@"C:\var\mp4\mp4\ddd.wav", @"C:\wb2\20191204_ジャケット的な\北へ.jpg", @"C:\temp\1.mp4");
			Test01_a(@"C:\wb2\20200423_動画テストデータ\新田美恵子のMINELVAカメラのCF.wav", @"C:\wb2\20200423_動画テストデータ\新田美恵子のMINELVAカメラのCF.jpg", @"C:\temp\2.mp4");
			Test01_a(@"C:\var\mp4\mp4\hbn.wav", @"C:\wb2\20191204_ジャケット的な\灰羽連盟.jpg", @"C:\temp\3.mp4");
			Test01_a(@"C:\wb2\20200423_動画テストデータ\まちぶせ.wav", @"C:\wb2\20200423_動画テストデータ\まちぶせ.jpg", @"C:\temp\4.mp4");
		}

		public void Test01_a(string mediaFile, string imgFile, string mp4File)
		{
			MovieMaker0001 mm = new MovieMaker0001()
			{
				DiscJacketFile = imgFile,
				SourceMediaFile = mediaFile,
				DestMP4File = mp4File,
			};

			mm.Perform();
		}
	}
}
