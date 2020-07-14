using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public static class DDKey
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private const int KEY_MAX = 256;

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static int[] KeyStatus = new int[KEY_MAX];
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static byte[] StatusMap = new byte[KEY_MAX];

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void INIT()
		{
			DDSystem.Pin(StatusMap);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void EachFrame()
		{
			if (DDEngine.WindowIsActive)
			{
				if (DX.GetHitKeyStateAll(StatusMap) != 0) // ? 失敗
					throw new DDError();

				for (int keyId = 0; keyId < 256; keyId++)
					DDUtils.UpdateInput(ref KeyStatus[keyId], StatusMap[keyId] != 0);
			}
			else
			{
				for (int keyId = 0; keyId < 256; keyId++)
					DDUtils.UpdateInput(ref KeyStatus[keyId], false);
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static int GetInput(int keyId)
		{
			// keyId == DX.KEY_INPUT_RETURN etc.

			return 1 <= DDEngine.FreezeInputFrame ? 0 : KeyStatus[keyId];
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static bool IsPound(int keyId)
		{
			return DDUtils.IsPound(GetInput(keyId));
		}
	}
}
