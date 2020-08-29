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
	public static class DDEngine
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static long FrameStartTime;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static long HzChaserTime;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static int FrameProcessingMillis;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static int FrameProcessingMillis_Worst;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static int FrameProcessingMillis_WorstFrame;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static int ProcFrame;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static int FreezeInputFrame;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static bool WindowIsActive;

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static void CheckHz()
		{
			long currTime = DDUtils.GetCurrTime();

			HzChaserTime += 16L; // 16.666 == 60Hz
			HzChaserTime = LongTools.ToRange(HzChaserTime, currTime - 100L, currTime + 100L);

			while (currTime < HzChaserTime)
			{
				Thread.Sleep(1);
				currTime = DDUtils.GetCurrTime();
			}
			FrameStartTime = currTime;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void EachFrame()
		{
			// app > @ enter EachFrame

			//Ground.EL.ExecuteAllTask();

			// < app

			DDGround.EL.ExecuteAllTask();
			DDCurtain.EachFrame();

			if (DDSEUtils.EachFrame() == false)
			{
				DDMusicUtils.EachFrame();
			}
			if (DDGround.MainScreen != null && DDSubScreenUtils.CurrDrawScreenHandle == DDGround.MainScreen.GetHandle())
			{
				DDSubScreenUtils.ChangeDrawScreen(DX.DX_SCREEN_BACK);

				if (DDGround.RealScreenDraw_W == -1)
				{
					if (DX.DrawExtendGraph(0, 0, DDGround.RealScreen_W, DDGround.RealScreen_H, DDGround.MainScreen.GetHandle(), 0) != 0) // ? 失敗
						throw new DDError();
				}
				else
				{
					if (DX.DrawBox(0, 0, DDGround.RealScreen_W, DDGround.RealScreen_H, DX.GetColor(0, 0, 0), 1) != 0) // ? 失敗
						throw new DDError();

					if (DX.DrawExtendGraph(
						DDGround.RealScreenDraw_L,
						DDGround.RealScreenDraw_T,
						DDGround.RealScreenDraw_L + DDGround.RealScreenDraw_W,
						DDGround.RealScreenDraw_T + DDGround.RealScreenDraw_H, DDGround.MainScreen.GetHandle(), 0) != 0) // ? 失敗
						throw new DDError();
				}
			}

			// app > @ before ScreenFlip

			// < app

			GC.Collect(0);

			FrameProcessingMillis = (int)(DDUtils.GetCurrTime() - FrameStartTime);

			if (FrameProcessingMillis_Worst < FrameProcessingMillis || DDUtils.CountDown(ref FrameProcessingMillis_WorstFrame) == false)
			{
				FrameProcessingMillis_Worst = FrameProcessingMillis;
				FrameProcessingMillis_WorstFrame = 120;
			}

			// DxLib >

			DX.ScreenFlip();

			if (DX.CheckHitKey(DX.KEY_INPUT_ESCAPE) == 1 || DX.ProcessMessage() == -1)
			{
				throw new DDCoffeeBreak();
			}

			// < DxLib

			// app > @ after ScreenFlip

			// < app

			CheckHz();

			ProcFrame++;
			DDUtils.CountDown(ref FreezeInputFrame);
			WindowIsActive = DDUtils.IsWindowActive();

			if (IntTools.IMAX < ProcFrame) // 192.9日程度でカンスト
			{
				ProcFrame = IntTools.IMAX; // 2bs
				throw new DDError();
			}

			DDPad.EachFrame();
			DDKey.EachFrame();
			DDInput.EachFrame();
			DDMouse.EachFrame();

			if (DDGround.RealScreen_W != DDConsts.Screen_W || DDGround.RealScreen_H != DDConsts.Screen_H || DDGround.RealScreenDraw_W != -1)
			{
				if (DDGround.MainScreen == null)
					DDGround.MainScreen = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H);

				DDGround.MainScreen.ChangeDrawScreen();
			}
			else
			{
				if (DDGround.MainScreen != null)
				{
					DDGround.MainScreen.Dispose();
					DDGround.MainScreen = null;
				}
			}

			// app > @ leave EachFrame

			// < app
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void FreezeInput(int frame = 1) // frame: 1 == このフレームのみ, 2 == このフレームと次のフレーム ...
		{
			if (frame < 1 || IntTools.IMAX < frame)
				throw new DDError("frame: " + frame);

			FreezeInputFrame = Math.Max(FreezeInputFrame, frame); // frame より長いフレーム数が既に設定されていたら、そちらを優先する。
		}
	}
}
