using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using DxLibDLL;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public class DDSimpleMenu
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public I3Color? Color = null;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public I3Color? BorderColor = null;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public I3Color? WallColor = null;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public DDPicture WallPicture = null;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public double WallCurtain = 0.0; // -1.0 ～ 1.0
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public int X = 16;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public int Y = 16;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public int YStep = 32;

		// <---- prm

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private bool MouseEnabled;

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public DDSimpleMenu()
		{
			this.MouseEnabled = DDUtils.GetMouseDispMode();
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public int Perform(string title, string[] items, int selectIndex)
		{
			DDCurtain.SetCurtain();
			DDEngine.FreezeInput();

			for (; ; )
			{
				if (this.MouseEnabled)
				{
					DDMouse.UpdatePos();

					int musSelIdxY = DDMouse.Y - (this.Y + this.YStep);

					if (0 <= musSelIdxY)
					{
						int musSelIdx = musSelIdxY / this.YStep;

						if (musSelIdx < items.Length)
						{
							selectIndex = musSelIdx;
						}
					}
					if (DDMouse.L.GetInput() == -1)
					{
						break;
					}
					if (DDMouse.R.GetInput() == -1)
					{
						selectIndex = items.Length - 1;
						break;
					}
				}

				bool chgsel = false;

				if (DDInput.A.IsPound())
				{
					break;
				}
				if (DDInput.B.IsPound())
				{
					if (selectIndex == items.Length - 1)
						break;

					selectIndex = items.Length - 1;
					chgsel = true;
				}
				if (DDInput.DIR_8.IsPound())
				{
					selectIndex--;
					chgsel = true;
				}
				if (DDInput.DIR_2.IsPound())
				{
					selectIndex++;
					chgsel = true;
				}

				selectIndex += items.Length;
				selectIndex %= items.Length;

				if (this.MouseEnabled && chgsel)
				{
					DDMouse.X = 0;
					DDMouse.Y = this.Y + (selectIndex + 1) * this.YStep + this.YStep / 2;

					DDMouse.ApplyPos();
				}

				DDCurtain.DrawCurtain();

				if (this.WallColor != null)
					DX.DrawBox(0, 0, DDConsts.Screen_W, DDConsts.Screen_H, DDUtils.GetColor(this.WallColor.Value), 1);

				if (this.WallPicture != null)
				{
					DDDraw.DrawRect(this.WallPicture, 0, 0, DDConsts.Screen_W, DDConsts.Screen_H);
					//DDDraw.DrawCenter(this.WallPicture, DDConsts.Screen_W / 2.0, DDConsts.Screen_H / 2.0); // old
					DDCurtain.DrawCurtain(this.WallCurtain);
				}
				if (this.Color != null)
					DDPrint.SetColor(this.Color.Value);

				if (this.BorderColor != null)
					DDPrint.SetBorder(this.BorderColor.Value);

				DDPrint.SetPrint(this.X, this.Y, this.YStep);
				//DDPrint.SetPrint(16, 16, 32); // old
				DDPrint.Print(title + "　(Mouse=" + this.MouseEnabled + ")");
				DDPrint.PrintRet();

				for (int c = 0; c < items.Length; c++)
				{
					DDPrint.Print(string.Format("[{0}] {1}", selectIndex == c ? ">" : " ", items[c]));
					DDPrint.PrintRet();
				}
				DDPrint.Reset();

				DDEngine.EachFrame();
			}
			DDEngine.FreezeInput();

			return selectIndex;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private class ButtonInfo
		{
			public DDInput.Button Button;
			public string Name;

			public ButtonInfo(DDInput.Button button, string name)
			{
				this.Button = button;
				this.Name = name;
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void PadConfig()
		{
			ButtonInfo[] btnInfos = new ButtonInfo[]
			{
				// app > @ btnInfos

				new ButtonInfo(DDInput.DIR_2, "下"),
				new ButtonInfo(DDInput.DIR_4, "左"),
				new ButtonInfo(DDInput.DIR_6, "右"),
				new ButtonInfo(DDInput.DIR_8, "上"),
				new ButtonInfo(DDInput.A, "ショットボタン"),
				new ButtonInfo(DDInput.B, "低速ボタン"),
				new ButtonInfo(DDInput.C, "ボムボタン"),
				//new ButtonInfo(DDInput.D, ""),
				//new ButtonInfo(DDInput.E, ""),
				//new ButtonInfo(DDInput.F, ""),
				//new ButtonInfo(DDInput.L, ""),
				//new ButtonInfo(DDInput.R, ""),
				new ButtonInfo(DDInput.PAUSE, "ポーズボタン"),
				//new ButtonInfo(DDInput.START, ""),

				// < app
			};

			foreach (ButtonInfo btnInfo in btnInfos)
				btnInfo.Button.Backup();

			try
			{
				foreach (ButtonInfo btnInfo in btnInfos)
					btnInfo.Button.BtnId = -1;

				DDCurtain.SetCurtain();
				DDEngine.FreezeInput();

				int currBtnIndex = 0;

				while (currBtnIndex < btnInfos.Length)
				{
					if (DDKey.GetInput(DX.KEY_INPUT_SPACE) == 1)
					{
						return;
					}
					if (DDKey.GetInput(DX.KEY_INPUT_Z) == 1)
					{
						currBtnIndex++;
						goto endInput;
					}

					{
						int pressBtnId = -1;

						for (int padId = 0; padId < DDPad.GetPadCount(); padId++)
							for (int btnId = 0; btnId < DDPad.PAD_BUTTON_MAX; btnId++)
								if (DDPad.GetInput(padId, btnId) == 1)
									pressBtnId = btnId;

						for (int c = 0; c < currBtnIndex; c++)
							if (btnInfos[c].Button.BtnId == pressBtnId)
								pressBtnId = -1;

						if (pressBtnId != -1)
						{
							btnInfos[currBtnIndex].Button.BtnId = pressBtnId;
							currBtnIndex++;
						}
					}
				endInput:

					DDCurtain.DrawCurtain();

					if (this.WallColor != null)
						DX.DrawBox(0, 0, DDConsts.Screen_W, DDConsts.Screen_H, DDUtils.GetColor(this.WallColor.Value), 1);

					if (this.WallPicture != null)
					{
						DDDraw.DrawRect(this.WallPicture, 0, 0, DDConsts.Screen_W, DDConsts.Screen_H);
						//DDDraw.DrawCenter(this.WallPicture, DDConsts.Screen_W / 2.0, DDConsts.Screen_H / 2.0); // old
						DDCurtain.DrawCurtain(this.WallCurtain);
					}
					if (this.Color != null)
						DDPrint.SetColor(this.Color.Value);

					if (this.BorderColor != null)
						DDPrint.SetBorder(this.BorderColor.Value);

					DDPrint.SetPrint(this.X, this.Y, this.YStep);
					//DDPrint.SetPrint(16, 16, 32); // old
					DDPrint.Print("ゲームパッドのボタン設定");
					DDPrint.PrintRet();

					for (int c = 0; c < btnInfos.Length; c++)
					{
						DDPrint.Print(string.Format("[{0}] {1}", currBtnIndex == c ? ">" : " ", btnInfos[c].Name));

						if (c < currBtnIndex)
						{
							int btnId = btnInfos[c].Button.BtnId;

							DDPrint.Print("　->　");

							if (btnId == -1)
								DDPrint.Print("割り当てナシ");
							else
								DDPrint.Print("" + btnId);
						}
						DDPrint.PrintRet();
					}
					DDPrint.Print("★　カーソルの機能に割り当てるボタンを押して下さい。");
					DDPrint.PrintRet();
					DDPrint.Print("★　[Z]を押すとボタンの割り当てをスキップします。");
					DDPrint.PrintRet();
					DDPrint.Print("★　スペースを押すとキャンセルします。");
					DDPrint.PrintRet();

					if (this.MouseEnabled)
					{
						DDPrint.Print("★　右クリックするとキャンセルします。");
						DDPrint.PrintRet();

						if (DDMouse.R.GetInput() == -1)
						{
							return;
						}
					}

					DDEngine.EachFrame();
				}
				btnInfos = null;
			}
			finally
			{
				if (btnInfos != null)
					foreach (ButtonInfo info in btnInfos)
						info.Button.Restore();

				DDEngine.FreezeInput();
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static int WinSzExp(int size, int index)
		{
			return (size * (8 + index)) / 8;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void WindowSizeConfig()
		{
			string[] items = new string[]
			{
				string.Format("{0} x {1} (デフォルト)", DDConsts.Screen_W, DDConsts.Screen_H),
				string.Format("{0} x {1}", WinSzExp(DDConsts.Screen_W,  1), WinSzExp(DDConsts.Screen_H,  1)),
				string.Format("{0} x {1}", WinSzExp(DDConsts.Screen_W,  2), WinSzExp(DDConsts.Screen_H,  2)),
				string.Format("{0} x {1}", WinSzExp(DDConsts.Screen_W,  3), WinSzExp(DDConsts.Screen_H,  3)),
				string.Format("{0} x {1}", WinSzExp(DDConsts.Screen_W,  4), WinSzExp(DDConsts.Screen_H,  4)),
				string.Format("{0} x {1}", WinSzExp(DDConsts.Screen_W,  5), WinSzExp(DDConsts.Screen_H,  5)),
				string.Format("{0} x {1}", WinSzExp(DDConsts.Screen_W,  6), WinSzExp(DDConsts.Screen_H,  6)),
				string.Format("{0} x {1}", WinSzExp(DDConsts.Screen_W,  7), WinSzExp(DDConsts.Screen_H,  7)),
				string.Format("{0} x {1}", WinSzExp(DDConsts.Screen_W,  8), WinSzExp(DDConsts.Screen_H,  8)),
				string.Format("{0} x {1}", WinSzExp(DDConsts.Screen_W,  9), WinSzExp(DDConsts.Screen_H,  9)),
				string.Format("{0} x {1}", WinSzExp(DDConsts.Screen_W, 10), WinSzExp(DDConsts.Screen_H, 10)),
				"フルスクリーン 画面に合わせる",
				"フルスクリーン 縦横比維持",
				"フルスクリーン 黒背景 (推奨)",
				"戻る",
			};

			int selectIndex = 0;

			for (; ; )
			{
				selectIndex = Perform("ウィンドウサイズ設定", items, selectIndex);

				switch (selectIndex)
				{
					case 0:
					case 1:
					case 2:
					case 3:
					case 4:
					case 5:
					case 6:
					case 7:
					case 8:
					case 9:
					case 10:
						DDMain.SetScreenSize(WinSzExp(DDConsts.Screen_W, selectIndex), WinSzExp(DDConsts.Screen_H, selectIndex));
						break;

					case 11:
						DDMain.SetScreenSize(DDGround.MonitorRect.W, DDGround.MonitorRect.H);
						break;

					case 12:
						{
							int w = DDGround.MonitorRect.W;
							int h = (DDConsts.Screen_H * DDGround.MonitorRect.W) / DDConsts.Screen_W;

							if (DDGround.MonitorRect.H < h)
							{
								h = DDGround.MonitorRect.H;
								w = (DDConsts.Screen_W * DDGround.MonitorRect.H) / DDConsts.Screen_H;

								if (DDGround.MonitorRect.W < w)
									throw new DDError();
							}
							DDMain.SetScreenSize(w, h);
						}
						break;

					case 13:
						{
							int w = DDGround.MonitorRect.W;
							int h = (DDConsts.Screen_H * DDGround.MonitorRect.W) / DDConsts.Screen_W;

							if (DDGround.MonitorRect.H < h)
							{
								h = DDGround.MonitorRect.H;
								w = (DDConsts.Screen_W * DDGround.MonitorRect.H) / DDConsts.Screen_H;

								if (DDGround.MonitorRect.W < w)
									throw new DDError();
							}
							DDMain.SetScreenSize(DDGround.MonitorRect.W, DDGround.MonitorRect.H);

							DDGround.RealScreenDraw_L = (DDGround.MonitorRect.W - w) / 2;
							DDGround.RealScreenDraw_T = (DDGround.MonitorRect.H - h) / 2;
							DDGround.RealScreenDraw_W = w;
							DDGround.RealScreenDraw_H = h;
						}
						break;

					case 14:
						goto endLoop;

					default:
						throw new DDError();
				}
			}
		endLoop:
			;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public int IntVolumeConfig(string title, int value, int minval, int maxval, int valStep, int valFastStep, Action<int> valChanged, Action pulse)
		{
			const int PULSE_FRM = 60;

			int origval = value;

			DDCurtain.SetCurtain();
			DDEngine.FreezeInput();

			for (; ; )
			{
				bool chgval = false;

				if (DDInput.A.IsPound() || this.MouseEnabled && DDMouse.L.GetInput() == -1)
				{
					break;
				}
				if (DDInput.B.IsPound() || this.MouseEnabled && DDMouse.R.GetInput() == -1)
				{
					if (value == origval)
						break;

					value = origval;
					chgval = true;
				}
				if (this.MouseEnabled)
				{
					value += DDMouse.Rot;
					chgval = true;
				}
				if (DDInput.DIR_8.IsPound())
				{
					value += valFastStep;
					chgval = true;
				}
				if (DDInput.DIR_6.IsPound())
				{
					value += valStep;
					chgval = true;
				}
				if (DDInput.DIR_4.IsPound())
				{
					value -= valStep;
					chgval = true;
				}
				if (DDInput.DIR_2.IsPound())
				{
					value -= valFastStep;
					chgval = true;
				}
				if (chgval)
				{
					value = IntTools.ToRange(value, minval, maxval);
					valChanged(value);
				}
				if (DDEngine.ProcFrame % PULSE_FRM == 0)
				{
					pulse();
				}

				DDCurtain.DrawCurtain();

				if (this.WallColor != null)
					DX.DrawBox(0, 0, DDConsts.Screen_W, DDConsts.Screen_H, DDUtils.GetColor(this.WallColor.Value), 1);

				if (this.WallPicture != null)
				{
					DDDraw.DrawRect(this.WallPicture, 0, 0, DDConsts.Screen_W, DDConsts.Screen_H);
					DDCurtain.DrawCurtain(this.WallCurtain);
				}
				if (this.Color != null)
					DDPrint.SetColor(this.Color.Value);

				if (this.BorderColor != null)
					DDPrint.SetBorder(this.BorderColor.Value);

				DDPrint.SetPrint(this.X, this.Y, this.YStep);
				DDPrint.Print(title);
				DDPrint.PrintRet();

				DDPrint.Print(string.Format("[{0}]　最小={1}　最大={2}", value, minval, maxval));
				DDPrint.PrintRet();

				DDPrint.Print("★　左＝下げる");
				DDPrint.PrintRet();
				DDPrint.Print("★　右＝上げる");
				DDPrint.PrintRet();
				DDPrint.Print("★　下＝速く下げる");
				DDPrint.PrintRet();
				DDPrint.Print("★　上＝速く上げる");
				DDPrint.PrintRet();
				DDPrint.Print("★　調整が終わったら決定ボタンを押して下さい。");
				DDPrint.PrintRet();

				DDEngine.EachFrame();
			}
			DDEngine.FreezeInput();

			return value;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public double VolumeConfig(string title, double rate, int minval, int maxval, int valStep, int valFastStep, Action<double> valChanged, Action pulse)
		{
			return VolumeValueToRate(
				IntVolumeConfig(
					title,
					RateToVolumeValue(rate, minval, maxval),
					minval,
					maxval,
					valStep,
					valFastStep,
					v => valChanged(VolumeValueToRate(v, minval, maxval)),
					pulse
					),
				minval,
				maxval
				);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static double VolumeValueToRate(int value, int minval, int maxval)
		{
			return (double)(value - minval) / (maxval - minval);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static int RateToVolumeValue(double rate, int minval, int maxval)
		{
			return minval + DoubleTools.ToInt(rate * (maxval - minval));
		}
	}
}
