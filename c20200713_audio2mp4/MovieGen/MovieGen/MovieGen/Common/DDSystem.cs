using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public static class DDSystem
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void PinOn<T>(T data, Action<IntPtr> routine)
		{
			GCHandle pinnedData = GCHandle.Alloc(data, GCHandleType.Pinned);
			try
			{
				routine(pinnedData.AddrOfPinnedObject());
			}
			finally
			{
				pinnedData.Free();
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Pin<T>(T data)
		{
			GCHandle h = GCHandle.Alloc(data, GCHandleType.Pinned);

			DDMain.Finalizers.Add(() =>
			{
				h.Free();
			});
		}
	}
}
