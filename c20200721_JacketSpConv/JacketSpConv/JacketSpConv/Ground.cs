using System;
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

		public string InputDir;
		public string OutputDir;
		public bool OutputToInputDir;
		public bool OutputOverwriteMode;

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

			// < 保存データ

			File.WriteAllLines(this.SaveDataFile, lines, Encoding.UTF8);
		}

		// ---- 受信

		public NamedEventUnit EvCancellable_Y = new NamedEventUnit(Consts.IPC_IDENT + "_CY");
		public NamedEventUnit EvCancellable_N = new NamedEventUnit(Consts.IPC_IDENT + "_CN");
		public NamedEventUnit EvMessage_Normal = new NamedEventUnit(Consts.IPC_IDENT + "_MN");
		public NamedEventUnit EvMessage_StartGenVideo = new NamedEventUnit(Consts.IPC_IDENT + "_MS");
		public NamedEventUnit EvMessage_GenVideoRunning = new NamedEventUnit(Consts.IPC_IDENT + "_MR");
		public FileCommunicator CmProgressRate = new FileCommunicator(Consts.IPC_IDENT + "_PR");

		// ---- 送信

		public NamedEventUnit EvStop_Conv = new NamedEventUnit(Consts.IPC_IDENT + "_SC");
		public NamedEventUnit EvStop_Master = new NamedEventUnit(Consts.IPC_IDENT + "_SM");

		// ----

		public void Dispose()
		{
			if (this.CmProgressRate != null)
			{
				this.CmProgressRate.Dispose();
				this.CmProgressRate = null;

				this.EvCancellable_Y.Dispose();
				this.EvCancellable_N.Dispose();
				this.EvMessage_Normal.Dispose();
				this.EvMessage_StartGenVideo.Dispose();
				this.EvMessage_GenVideoRunning.Dispose();
				this.EvStop_Conv.Dispose();
				this.EvStop_Master.Dispose();
			}
		}
	}
}
