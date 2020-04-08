using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using Charlotte.Tests.AudioPicMp4s;

namespace Charlotte
{
	class Program
	{
		public const string APP_IDENT = "{5fac1e8a-3300-4834-aad4-70ba3523df82}";
		public const string APP_TITLE = "AudioPicMp4";

		static void Main(string[] args)
		{
			ProcMain.CUIMain(new Program().Main2, APP_IDENT, APP_TITLE);

			//if (ProcMain.CUIError)
			{
				Console.WriteLine("Press ENTER.");
				Console.ReadLine();
			}
		}

		private void Main2(ArgsReader ar)
		{
			//new AudioPicMp40001Test().Test01(); // -- 0001
		}
	}
}
