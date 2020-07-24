﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Tools;
using Charlotte.Utils;

namespace Charlotte
{
	public class ConvMain
	{
		public void Perform(string inputDir, string outputDir, string successfulFile)
		{
			using (LogWriter stat = new LogWriter(FileUtils.EraseExt(ProcMain.SelfFile) + "_01.log"))
			using (LogWriter info = new LogWriter(FileUtils.EraseExt(ProcMain.SelfFile) + "_02.log"))
			{
				Ground.I.Logger = new Logger(stat, info);
				try
				{
					Ground.I.Logger.Stat("ConvMain 開始");
					this.Perform_02(inputDir, outputDir);
					Ground.I.Logger.Stat("ConvMain 完了");
					File.WriteAllBytes(successfulFile, BinTools.EMPTY); // 正常終了フラグ_作成
					Ground.I.Logger.Stat("ConvMain 完了_2");
				}
				catch (Exception e)
				{
					Ground.I.Logger.Stat("ConvMain 異常終了：" + e);
				}
				finally
				{
					Ground.I.Logger = null;
				}
			}
		}

		private void Perform_02(string inputDir, string outputDir)
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
					file = FileTools.MakeFullPath(file);

					if (file != StringTools.ENCODING_SJIS.GetString(StringTools.ENCODING_SJIS.GetBytes(file)))
						throw new Exception("Bad ConvGenVideo.exe");

					Ground.I.ConvGenVideoFile = file;
				}
			}

			Ground.I.Logger.Stat("<1 " + inputDir);
			Ground.I.Logger.Stat(">1 " + outputDir);

			inputDir = FileTools.MakeFullPath(inputDir);
			outputDir = FileTools.MakeFullPath(outputDir);

			Ground.I.Logger.Stat("<2 " + inputDir);
			Ground.I.Logger.Stat(">2 " + outputDir);

			// memo: 出力フォルダが入力フォルダと同じ場合がある。

			if (Directory.Exists(inputDir) == false)
				throw new ArgumentException("no inputDir: " + inputDir);

			if (Directory.Exists(outputDir) == false)
				throw new ArgumentException("no outputDir: " + outputDir);

			string[] files = Directory.GetFiles(inputDir, "*", SearchOption.AllDirectories);

			files = files.Select(v => FileTools.ChangeRoot(v, inputDir)).ToArray();

			foreach (string file in files)
			{
				try
				{
					Ground.I.Logger.Stat("ファイル：" + file);

					new ConvFileMain()
					{
						InputDir = inputDir,
						OutputDir = outputDir,
						PresumeAudioRelFile = file,
					}
					.Perform();

					Ground.I.Logger.Stat("成功");
				}
				catch (Cancelled)
				{
					Ground.I.Logger.Stat("中止");
					break;
				}
				catch (Exception e)
				{
					Ground.I.Logger.Stat("失敗：" + e);
				}
			}
		}
	}
}
