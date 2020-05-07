using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Charlotte.AudioPicMP4s
{
	public static class Utils
	{
		public static bool IsEmptyFile(string file)
		{
			return File.Exists(file) == false || new FileInfo(file).Length == 0;
		}
	}
}
