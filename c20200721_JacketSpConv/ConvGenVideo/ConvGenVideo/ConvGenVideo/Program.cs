using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Charlotte.Tools;
using System.IO;

namespace Charlotte
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main()
		{
			BootDir = Directory.GetCurrentDirectory();

			ProcMain.GUIMain(() => new MainWin(), APP_IDENT, APP_TITLE);
		}

		public static string BootDir;

		public const string APP_IDENT = "{4ac26037-d5f7-4001-80c1-e50fb08d10e8}";
		public const string APP_TITLE = "ConvGenVideo";
	}
}
