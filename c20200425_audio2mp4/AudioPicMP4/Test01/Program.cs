using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using Charlotte.Tests.AudioPicMP4s.MovieMakers;

namespace Charlotte
{
	class Program
	{
		public const string APP_IDENT = "{5fac1e8a-3300-4834-aad4-70ba3523df82}";
		public const string APP_TITLE = "AudioPicMP4";

		static void Main(string[] args)
		{
			ProcMain.CUIMain(new Program().Main2, APP_IDENT, APP_TITLE);

#if DEBUG
			//if (ProcMain.CUIError)
			{
				Console.WriteLine("Press ENTER.");
				Console.ReadLine();
			}
#endif
		}

		private void Main2(ArgsReader ar)
		{
			new Spectrum0001Test().Test01();
		}
	}
}
