using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public static class DDConsts
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public const string ConfigFile = "Config.conf";
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public const string SaveDataFile = "SaveData.dat";
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public const string ResourceFile = "Resource.dat";

#if false // 使わないはず。
		private const string RESOURCE_DIR_01 = @".\Resource";
		private const string RESOURCE_DIR_02 = @"..\..\..\..\Resource";

		private static string P_ResourceDir = null;

		public static string ResourceDir
		{
			get
			{
				if (P_ResourceDir == null)
				{
					if (Directory.Exists(RESOURCE_DIR_01))
					{
						P_ResourceDir = RESOURCE_DIR_01;
					}
					else
					{
						P_ResourceDir = RESOURCE_DIR_02;
					}
				}
				return P_ResourceDir;
			}
		}
#endif

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public const string UserDatStringsFile = "Properties.dat";

		// app > @ Screen_WH

		public const int Screen_W = 800;
		public const int Screen_H = 600;

		// < app

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public const int Screen_W_Min = 100;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public const int Screen_H_Min = 100;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public const int Screen_W_Max = 4000;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public const int Screen_H_Max = 3000;

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public const double DefaultVolume = 0.45;
	}
}
