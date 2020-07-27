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
	public static class DDFontRegister
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static WorkingDir WD;

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void INIT()
		{
			WD = new WorkingDir();

			DDMain.Finalizers.Add(() =>
			{
				UnloadAll();

				WD.Dispose();
				WD = null;
			});
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Add(string file)
		{
			Add(DDResource.Load(file), Path.GetFileName(file));
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Add(byte[] fileData, string localFile)
		{
			string dir = WD.MakePath();
			string file = Path.Combine(dir, localFile);

			FileTools.CreateDir(dir);
			File.WriteAllBytes(file, fileData);

			if (DDWin32.AddFontResourceEx(file, DDWin32.FR_PRIVATE, IntPtr.Zero) == 0) // ? 失敗
				throw new DDError();

			FontFiles.Add(file);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static List<string> FontFiles = new List<string>();

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static void Unload(string file)
		{
			if (DDWin32.RemoveFontResourceEx(file, DDWin32.FR_PRIVATE, IntPtr.Zero) == 0) // ? 失敗
				throw new DDError();
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static void UnloadAll()
		{
			foreach (string file in FontFiles)
				Unload(file);
		}
	}
}
