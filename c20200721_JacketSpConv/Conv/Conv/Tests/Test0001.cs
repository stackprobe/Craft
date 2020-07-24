using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.Tests
{
	public class Test0001
	{
		private void Test01_Main(string inputDir, string outputDir, string successfulFile)
		{
			FileTools.CreateDir(outputDir);
			FileTools.Delete(successfulFile);

			new ConvMain().Perform(
				inputDir,
				outputDir,
				successfulFile
				);
		}

		public void Test00()
		{
			Test01_Main(
				@"C:\wb2\20200710_動画よっしーさんからのテスト用データ",
				@"C:\temp\Test00_out",
				@"C:\temp\Test00_out.flg"
				);
		}

		public void Test01()
		{
			Test01_Main(
				@"C:\wb2\20200723_JacketSpConv_テスト入力データ\TestData0001",
				@"C:\temp\Test01_out",
				@"C:\temp\Test01_out.flg"
				);
		}

		public void Test02()
		{
			Test01_Main(
				@"C:\wb2\20200723_JacketSpConv_テスト入力データ\TestData0002",
				@"C:\temp\Test02_out",
				@"C:\temp\Test02_out.flg"
				);
		}
	}
}
