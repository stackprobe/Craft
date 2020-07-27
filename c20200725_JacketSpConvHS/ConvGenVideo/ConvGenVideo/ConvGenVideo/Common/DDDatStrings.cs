using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public static class DDDatStrings
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static Dictionary<string, string> Name2Value = DictionaryTools.Create<string>();

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void INIT()
		{
			string[] lines = FileTools.TextToLines(StringTools.ENCODING_SJIS.GetString(DDResource.Load("DatStrings.txt")));

			foreach (string line in lines)
			{
				int p = line.IndexOf('=');

				if (p == -1)
					throw new DDError();

				string name = line.Substring(0, p);
				string value = line.Substring(p + 1);

				Name2Value.Add(name, value);
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static string GetValue(string name)
		{
			if (Name2Value.ContainsKey(name) == false)
				throw new DDError(name);

			return Name2Value[name];
		}

		// Accessor >

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static string Title
		{
			get { return GetValue("Title"); }
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static string Author
		{
			get { return GetValue("Author"); }
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static string Copyright
		{
			get { return GetValue("Copyright"); }
		}

		// < Accessor
	}
}
