using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Charlotte.Tools;
using Charlotte.MP4Gens;

namespace Charlotte
{
	class Program
	{
		public const string APP_IDENT = "{be2af3f0-69b8-4927-9b35-3a2b23584a09}";
		public const string APP_TITLE = "MP4Gen";

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
			new MP4Gen0001().Main01();
		}
	}
}
