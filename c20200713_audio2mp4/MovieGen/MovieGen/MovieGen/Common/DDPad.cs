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
	public static class DDPad
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public const int PAD_MAX = 16;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public const int PAD_BUTTON_MAX = 32;

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static int[] ButtonStatus = new int[PAD_MAX * PAD_BUTTON_MAX]; // [padId * PAD_BUTTON_MAX + btnId]
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static uint[] PadStatus = new uint[PAD_MAX];

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static int PadCount = -1; // -1 == 未取得

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static int GetPadCount()
		{
			if (PadCount == -1)
			{
				PadCount = DX.GetJoypadNum();

				if (PadCount < 0 || PAD_MAX < PadCount)
					throw new DDError();
			}
			return PadCount;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static int PadId2InputType(int padId)
		{
			return padId + 1;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void EachFrame()
		{
			for (int padId = 0; padId < GetPadCount(); padId++)
			{
				uint status;

				if (DDEngine.WindowIsActive)
					status = (uint)DX.GetJoypadInputState(PadId2InputType(padId));
				else
					status = 0u;

				if (status != 0u)
				{
					for (int btnId = 0; btnId < PAD_BUTTON_MAX; btnId++)
						DDUtils.UpdateInput(ref ButtonStatus[padId * PAD_BUTTON_MAX + btnId], (status & (1u << btnId)) != 0u);
				}
				else
				{
					for (int btnId = 0; btnId < PAD_BUTTON_MAX; btnId++)
						DDUtils.UpdateInput(ref ButtonStatus[padId * PAD_BUTTON_MAX + btnId], false);
				}

				if (DDGround.PrimaryPadId == -1 && 10 < DDEngine.ProcFrame && PadStatus[padId] == 0u && status != 0u) // 最初にボタンを押下したパッドを PrimaryPadId にセット
					DDGround.PrimaryPadId = padId;

				PadStatus[padId] = status;
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static int GetInput(int padId, int btnId)
		{
			if (padId == -1) // ? 未割り当て
				padId = 0;

			if (btnId == -1) // ? 割り当てナシ
				return 0;

			return 1 <= DDEngine.FreezeInputFrame ? 0 : ButtonStatus[padId * PAD_BUTTON_MAX + btnId];
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static bool IsPound(int padId, int btnId)
		{
			return DDUtils.IsPound(GetInput(padId, btnId));
		}
	}
}
