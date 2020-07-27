using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using DxLibDLL;
using Charlotte.Tools;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public static class DDJammer
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static byte[] Encode(byte[] data)
		{
			data = ZipTools.Compress(data);
			MaskGZData(data);
			return data;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static byte[] Decode(byte[] data)
		{
			MaskGZData(data);
			byte[] ret = ZipTools.Decompress(data);
			//MaskGZData(data); // 復元
			return ret;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static void MaskGZData(byte[] data)
		{
			// app > @ MaskGZData

			// $_git:secretBegin
//
			/////////////////////
			//
				/////////////////////////
				/////////////////////////
			//
//
			// $_git:secretEnd

			// < app
		}
	}
}
