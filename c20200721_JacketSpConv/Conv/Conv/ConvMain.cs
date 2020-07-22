using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Tools;

namespace Charlotte
{
	public class ConvMain
	{
		public void Perform(string inputDir, string outputDir)
		{
			// 環境のチェック
			{
				if (File.Exists(Consts.FFMPEG_FILE) == false)
					throw new Exception("no ffmpeg.exe");

				if (File.Exists(Consts.MASTER_FILE) == false)
					throw new Exception("no Master.exe");

				if (File.Exists(Consts.wavCsv_FILE) == false)
					throw new Exception("no wavCsv.exe");

				{
					string file = @".\ConvGenVideo.exe";

					if (File.Exists(file) == false)
					{
						file = @"..\..\..\..\ConvGenVideo\ConvGenVideo\ConvGenVideo\bin\Release\ConvGenVideo.exe";

						if (File.Exists(file) == false)
							throw new Exception("no ConvGenVideo.exe");
					}
					Ground.I.ConvGenVideoFile = file;
				}
			}

			inputDir = FileTools.MakeFullPath(inputDir);
			outputDir = FileTools.MakeFullPath(outputDir);

			// memo: 出力フォルダが入力フォルダと同じ場合がある。

			if (Directory.Exists(inputDir) == false)
				throw new ArgumentException("no inputDir: " + inputDir);

			if (Directory.Exists(outputDir) == false)
				throw new ArgumentException("no outputDir: " + outputDir);

			string[] files = Directory.GetFiles(inputDir, "*", SearchOption.AllDirectories);

			files = files.Select(v => FileTools.ChangeRoot(v, inputDir)).ToArray();

			foreach (string file in files)
			{
				// TODO log

				try
				{
					new ConvFileMain()
					{
						InputDir = inputDir,
						OutputDir = outputDir,
						PresumeAudioRelFile = file,
					}
					.Perform();

					// TODO log
				}
				catch (Cancelled)
				{
					// TODO log

					break;
				}
				catch (Exception e)
				{
					// TODO log
				}
			}
		}
	}
}
