﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Charlotte.Tools;
using Charlotte.Tests;

namespace Charlotte
{
	class Program
	{
		public const string APP_IDENT = "{53957f5c-18d7-413b-a914-8a8be0b282de}";
		public const string APP_TITLE = "Conv";

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
			Ground.I = new Ground();
			try
			{
#if DEBUG
				//new Test0001().Test00();
				new Test0001().Test01();
				//new Test0001().Test02();
#else
				this.Main3(ar);
#endif
			}
			finally
			{
				Ground.I.Dispose();
				Ground.I = null;
			}
		}

		private void Main3(ArgsReader ar)
		{
			try
			{
				if (ar.NextArg() != "CS-Conv")
				{
					throw new Exception("不正なコールサイン");
				}

				string inputDir = StringTools.LiteDecode(ar.NextArg());
				string outputDir = StringTools.LiteDecode(ar.NextArg());
				bool outputOverwriteMode = int.Parse(ar.NextArg()) != 0;
				string successfulFile = ar.NextArg();

				new ConvMain().Perform(inputDir, outputDir, outputOverwriteMode, successfulFile);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}
