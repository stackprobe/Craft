using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Tools;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public static class DDMouse
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static int _rot;

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static int Rot
		{
			get
			{
				return 1 <= DDEngine.FreezeInputFrame ? 0 : _rot;
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public class Button
		{
			public int Status = 0;

			public int GetInput()
			{
				return 1 <= DDEngine.FreezeInputFrame ? 0 : this.Status;
			}

			public bool IsPound()
			{
				return DDUtils.IsPound(this.GetInput());
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static Button L = new Button();
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static Button R = new Button();
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static Button M = new Button();

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void EachFrame()
		{
			uint status;

			if (DDEngine.WindowIsActive)
			{
				_rot = DX.GetMouseWheelRotVol();
				status = (uint)DX.GetMouseInput();
			}
			else
			{
				_rot = 0;
				status = 0u;
			}
			_rot = IntTools.ToRange(_rot, -IntTools.IMAX, IntTools.IMAX);

			DDUtils.UpdateInput(ref L.Status, (status & (uint)DX.MOUSE_INPUT_LEFT) != 0u);
			DDUtils.UpdateInput(ref R.Status, (status & (uint)DX.MOUSE_INPUT_RIGHT) != 0u);
			DDUtils.UpdateInput(ref M.Status, (status & (uint)DX.MOUSE_INPUT_MIDDLE) != 0u);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static int X = (int)(DDConsts.Screen_W / 2.0);
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static int Y = (int)(DDConsts.Screen_H / 2.0);

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void UpdatePos()
		{
			if (DX.GetMousePoint(out X, out Y) != 0) // ? 失敗
				throw new DDError();

			if (DDGround.RealScreenDraw_W != -1)
			{
				X -= DDGround.RealScreenDraw_L;
				X *= DDConsts.Screen_W;
				X /= DDGround.RealScreenDraw_W;
				Y -= DDGround.RealScreenDraw_T;
				Y *= DDConsts.Screen_H;
				Y /= DDGround.RealScreenDraw_H;
			}
			else
			{
				X *= DDConsts.Screen_W;
				X /= DDGround.RealScreen_W;
				Y *= DDConsts.Screen_H;
				Y /= DDGround.RealScreen_H;
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void ApplyPos()
		{
			int mx = X;
			int my = Y;

			if (DDGround.RealScreenDraw_W != -1)
			{
				mx *= DDGround.RealScreenDraw_W;
				mx /= DDConsts.Screen_W;
				mx += DDGround.RealScreenDraw_L;
				my *= DDGround.RealScreenDraw_H;
				my /= DDConsts.Screen_H;
				my += DDGround.RealScreenDraw_T;
			}
			else
			{
				mx *= DDGround.RealScreen_W;
				mx /= DDConsts.Screen_W;
				my *= DDGround.RealScreen_H;
				my /= DDConsts.Screen_H;
			}
			if (DX.SetMousePoint(mx, my) != 0) // ? 失敗
				throw new DDError();
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static int MoveX;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static int MoveY;

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static int UM_LastFrame = -IntTools.IMAX;

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void UpdateMove()
		{
			const int centerX = (int)(DDConsts.Screen_W / 2.0);
			const int centerY = (int)(DDConsts.Screen_H / 2.0);

			if (DDEngine.ProcFrame <= UM_LastFrame) // ? 2回以上更新した。
				throw new DDError();

			UpdatePos();

			MoveX = X - centerX;
			MoveY = Y - centerY;

			X = centerX;
			Y = centerY;

			ApplyPos();

			if (UM_LastFrame + 1 < DDEngine.ProcFrame) // ? 1フレーム以上更新しなかった。
			{
				MoveX = 0;
				MoveY = 0;
			}
			UM_LastFrame = DDEngine.ProcFrame;
		}
	}
}
