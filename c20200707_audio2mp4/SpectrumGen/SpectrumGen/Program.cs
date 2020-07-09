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
		public const string APP_IDENT = "{6f82d885-20ca-406b-9738-89fd83dec3f6}";
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
