﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Tools;

namespace Charlotte
{
	public class Ground : IDisposable
	{
		public static Ground I;

		public string ConvExeFile;

		// 保存データ >

		// -- 実行中ここに保持しない。

		public string InputDir;
		public string OutputDir;
		public bool OutputToInputDir;
		public bool OutputOverwriteMode;

		// -- 実行中ここに保持する。

		public int ConvThreadCount = Consts.CONV_THREAD_COUNT_DEF;

		// --

		// < 保存データ

		private string SaveDataFile = Path.Combine(ProcMain.SelfDir, Path.GetFileNameWithoutExtension(ProcMain.SelfFile) + ".dat");

		public bool Load()
		{
			if (File.Exists(this.SaveDataFile) == false)
				return false;

			string[] lines = File.ReadAllLines(this.SaveDataFile, Encoding.UTF8);
			int c = 0;

			// 保存データ >

			this.InputDir = lines[c++];
			this.OutputDir = lines[c++];
			this.OutputToInputDir = int.Parse(lines[c++]) != 0;
			this.OutputOverwriteMode = int.Parse(lines[c++]) != 0;
			this.ConvThreadCount = int.Parse(lines[c++]);

			// < 保存データ

			return true;
		}

		public void Save()
		{
			List<string> lines = new List<string>();

			// 保存データ >

			lines.Add(this.InputDir);
			lines.Add(this.OutputDir);
			lines.Add("" + (this.OutputToInputDir ? 1 : 0));
			lines.Add("" + (this.OutputOverwriteMode ? 1 : 0));
			lines.Add("" + this.ConvThreadCount);

			// < 保存データ

			File.WriteAllLines(this.SaveDataFile, lines, Encoding.UTF8);
		}

		// ---- 受信

		public NamedEventUnit EvCancellable_Y = new NamedEventUnit(Consts.IPC_IDENT + "_CY");
		public NamedEventUnit EvCancellable_N = new NamedEventUnit(Consts.IPC_IDENT + "_CN");
		public NamedEventUnit EvMessage_Normal = new NamedEventUnit(Consts.IPC_IDENT + "_MN");
		public NamedEventUnit EvMessage_StartGenVideo = new NamedEventUnit(Consts.IPC_IDENT + "_MS");
		public NamedEventUnit EvMessage_GenVideoRunning = new NamedEventUnit(Consts.IPC_IDENT + "_MR");
		public NamedEventUnit EvMessage_UserCancelled = new NamedEventUnit(Consts.IPC_IDENT + "_UC");
		public FileCommunicator CmProgressRate = new FileCommunicator(Consts.IPC_IDENT + "_PR");
		public FileCommunicator CmReport = new FileCommunicator(Consts.IPC_IDENT + "_RP");

		// ---- 送信

		public NamedEventUnit EvStop_Conv = new NamedEventUnit(Consts.IPC_IDENT + "_SC");
		public NamedEventUnit EvStop_Master = new NamedEventUnit(Consts.IPC_IDENT + "_SM");

		// ----

		public void Dispose()
		{
			if (this.CmProgressRate != null)
			{
				ExceptionDam.Section(eDam =>
				{
					eDam.Dispose(ref this.EvCancellable_Y);
					eDam.Dispose(ref this.EvCancellable_N);
					eDam.Dispose(ref this.EvMessage_Normal);
					eDam.Dispose(ref this.EvMessage_StartGenVideo);
					eDam.Dispose(ref this.EvMessage_GenVideoRunning);
					eDam.Dispose(ref this.EvMessage_UserCancelled);
					eDam.Dispose(ref this.CmProgressRate);
					eDam.Dispose(ref this.CmReport);
					eDam.Dispose(ref this.EvStop_Conv);
					eDam.Dispose(ref this.EvStop_Master);
				});
			}
		}
	}
}
