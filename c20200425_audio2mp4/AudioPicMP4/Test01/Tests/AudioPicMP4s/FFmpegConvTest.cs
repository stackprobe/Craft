using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.AudioPicMP4s;
using Charlotte.Tools;

namespace Charlotte.Tests.AudioPicMP4s
{
	public class FFmpegConvTest
	{
		public void Test01()
		{
			Test01_a(
				@"C:\wb2\20200423_動画テストデータ\まちぶせ.wav",
				@"C:\temp\1.wav"
				);
			Test01_a(
				@"C:\var\mp4\mp4\ddd.mp4",
				@"C:\temp\2.wav"
				);
			Test01_a(
				@"C:\var\mp4\mp4_2\僕は君の涙.mp4",
				@"C:\temp\3.wav"
				);
			Test01_a(
				@"C:\var\mp4\mp4_2\僕は君の涙.ogv",
				@"C:\temp\4.wav"
				);
			Test01_a(
				@"C:\var\mp4\ogv_test\ハンドル握って.mp3",
				@"C:\temp\5.wav"
				);
			Test01_a(
				@"C:\var\mp4\ogv_test\マミさんのテーマ.mp3",
				@"C:\temp\6.wav"
				);
		}

		private void Test01_a(string rFile, string wFile)
		{
			Console.WriteLine("rFile: " + rFile);
			Console.WriteLine("wFile: " + wFile);

			FFmpegConv.MakeWavFile(rFile, wFile);

			Console.WriteLine("done");
		}

		public void Test02()
		{
			DebugTools.MustThrow(() =>
			{
				// 動画ファイルではない。
				Test01_a(
					@"C:\wb2\20200423_動画テストデータ\新田美恵子のMINELVAカメラのCF.jpg",
					@"C:\temp\1.wav"
					);
			});

			DebugTools.MustThrow(() =>
			{
				// 動画ファイルだけど、映像のみ。
				Test01_a(
					@"S:\_rosetta\Hatena\20191031\Movie.mp4",
					@"C:\temp\2.wav"
					);
			});

			DebugTools.MustThrow(() =>
			{
				// そんなファイル存在しない。
				Test01_a(
					@"C:\temp\not_exists_mkpd-147809964481328986302236643611578493541.mp4",
					@"C:\temp\3.wav"
					);
			});
		}
	}
}
