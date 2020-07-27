using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using System.Drawing;
using Charlotte.Tools;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public static class DDSubScreenUtils
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static List<DDSubScreen> SubScreens = new List<DDSubScreen>();

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Add(DDSubScreen subScreen)
		{
			SubScreens.Add(subScreen);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Remove(DDSubScreen subScreen)
		{
			if (DDUtils.FastDesertElement(SubScreens, i => i == subScreen) == null) // ? Already removed
				throw new DDError();
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void UnloadAll()
		{
			foreach (DDSubScreen subScreen in SubScreens)
				subScreen.Unload();
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static int CurrDrawScreenHandle = DX.DX_SCREEN_BACK;

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void ChangeDrawScreen(int handle)
		{
			if (DX.SetDrawScreen(handle) != 0) // ? 失敗
				throw new DDError();

			CurrDrawScreenHandle = handle;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void ChangeDrawScreen(DDSubScreen subScreen)
		{
			ChangeDrawScreen(subScreen.GetHandle());
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void RestoreDrawScreen()
		{
			ChangeDrawScreen(DDGround.MainScreen != null ? DDGround.MainScreen.GetHandle() : DX.DX_SCREEN_BACK);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static Size GetDrawScreenSize() // ret: 描画領域のサイズ？
		{
			int w;
			int h;
			int cbd;

			if (DX.GetScreenState(out w, out h, out cbd) != 0)
				throw new DDError();

			if (w < 1 || IntTools.IMAX < w)
				throw new DDError("w: " + w);

			if (h < 1 || IntTools.IMAX < h)
				throw new DDError("h: " + h);

			return new Size(w, h);
		}
	}
}
