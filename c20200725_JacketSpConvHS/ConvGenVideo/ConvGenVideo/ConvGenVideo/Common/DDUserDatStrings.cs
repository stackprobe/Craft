using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Tools;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public static class DDUserDatStrings
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
			if (File.Exists(DDConsts.UserDatStringsFile) == false)
				return;

			IEnumerable<string> lines = File.ReadAllLines(DDConsts.UserDatStringsFile, StringTools.ENCODING_SJIS).Select(line => line.Trim()).Where(line => line != "" && line[0] != ';');

			foreach (string line in lines)
			{
				int p = line.IndexOf('=');

				if (p == -1)
					throw new DDError();

				string name = line.Substring(0, p).Trim();
				string value = line.Substring(p + 1).Trim();

				Name2Value.Add(name, value);
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static string GetValue(string name, string defval)
		{
			if (Name2Value.ContainsKey(name) == false)
				return defval;

			return Name2Value[name];
		}

		// Accessor >

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static string Version
		{
			get { return GetValue("Version", "0.00"); }
		}

		// < Accessor
	}
}
