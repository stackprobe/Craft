using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Tools;

namespace Charlotte
{
	public static class CommonUtils
	{
		// sync > @ AntiWindowsDefenderSmartScreen

		public static void AntiWindowsDefenderSmartScreen()
		{
			WriteLog("awdss_1");

			if (Is初回起動())
			{
				WriteLog("awdss_2");

				foreach (string exeFile in Directory.GetFiles(BootTools.SelfDir, "*.exe", SearchOption.TopDirectoryOnly))
				{
					try
					{
						WriteLog("awdss_exeFile: " + exeFile);

						if (exeFile.ToLower() == BootTools.SelfFile.ToLower())
						{
							WriteLog("awdss_self_noop");
						}
						else
						{
							byte[] exeData = File.ReadAllBytes(exeFile);
							File.Delete(exeFile);
							File.WriteAllBytes(exeFile, exeData);
						}
						WriteLog("awdss_OK");
					}
					catch (Exception e)
					{
						WriteLog(e);
					}
				}
				WriteLog("awdss_3");
			}
			WriteLog("awdss_4");
		}

		// < sync

		private static class BootTools // AntiWindowsDefenderSmartScreen() 専用
		{
			public static string SelfDir
			{
				get
				{
					return ProcMain.SelfDir;
				}
			}

			public static string SelfFile
			{
				get
				{
					return ProcMain.SelfFile;
				}
			}
		}

		public static bool Is初回起動()
		{
			string sigFile = ProcMain.SelfFile + ".awdss.sig";

			if (File.Exists(sigFile))
				return false;

			File.WriteAllBytes(sigFile, BinTools.EMPTY);
			return true;
		}

		private static bool LogWrote = false;

		public static void WriteLog(object message)
		{
			try
			{
				using (StreamWriter writer = new StreamWriter(ProcMain.SelfFile + ".log", LogWrote, Encoding.UTF8))
				{
					writer.WriteLine("[" + DateTime.Now + "] " + message);
				}
				LogWrote = true;
			}
			catch
			{ }
		}
	}
}
