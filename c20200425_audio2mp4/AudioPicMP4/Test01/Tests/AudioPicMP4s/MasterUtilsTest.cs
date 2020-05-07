using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.AudioPicMP4s;
using System.IO;
using Charlotte.Tools;

namespace Charlotte.Tests.AudioPicMP4s
{
	public class MasterUtilsTest
	{
		public void Test01()
		{
			Test01_a(
				@"C:\wb2\20200423_動画テストデータ\まちぶせ.wav", // 要処理
				@"C:\temp\1.wav",
				@"C:\temp\1.txt"
				);
			Test01_a(
				@"C:\wb2\20200423_動画テストデータ\新田美恵子のMINELVAカメラのCF.wav", // 処理不要
				@"C:\temp\2.wav",
				@"C:\temp\2.txt"
				);
		}

		private void Test01_a(string rFile, string wFile, string reportFile)
		{
			Console.WriteLine("rFile: " + rFile);
			Console.WriteLine("wFile: " + wFile);
			Console.WriteLine("reportFile: " + reportFile);

			bool ret = MasterUtils.Mastering(rFile, wFile, lines => File.WriteAllLines(reportFile, lines, StringTools.ENCODING_SJIS));

			Console.WriteLine("====> " + ret);
		}

		public void Test02()
		{
			DebugTools.MustThrow(() =>
			{
				// .wavファイルではない。
				Test01_a(
					@"C:\wb2\20200423_動画テストデータ\新田美恵子のMINELVAカメラのCF.jpg",
					@"C:\temp\1.wav",
					@"C:\temp\1.txt"
					);

				// ** たまたま偶然処理出来てしまうということもあるかもしれない。
			});

			DebugTools.MustThrow(() =>
			{
				// そんなファイル存在しない。
				Test01_a(
					@"C:\temp\not_exists_mkpd-147809964481328986302236643611578493541.mp4",
					@"C:\temp\2.wav",
					@"C:\temp\2.txt"
					);
			});
		}
	}
}
