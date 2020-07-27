using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Charlotte.Utils
{
	public static class FileUtils
	{
		public static string EraseExt(string strPath)
		{
			return Path.Combine(Path.GetDirectoryName(strPath), Path.GetFileNameWithoutExtension(strPath));
		}
	}
}
