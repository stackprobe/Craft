using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Charlotte.Tools;
using Charlotte.SpectrumGens;

namespace Charlotte
{
	class Program
	{
		public const string APP_IDENT = "{17249ae5-b553-4981-a494-0444dda0c85d}";
		public const string APP_TITLE = "SpectrumGen";

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
			new SpectrumGen0001().Main01();
		}
	}
}
