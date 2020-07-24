using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Common;
using System.IO;
using Charlotte.Tools;

namespace Charlotte.ConvMains
{
	public class MovieGen0001
	{
		private SpectrumData SpData;
		private int Frame = 0;

		private void MG_EachFrame()
		{
			//DX.SaveDrawScreen(0, 0, DDConsts.Screen_W, DDConsts.Screen_H, Path.Combine(this.WDir, string.Format("{0}.bmp", this.Frame))); // old
			DDEngine.EachFrame();

			this.Frame++;
		}

		public void Main01(string spectrumFile, DDPicture jacket, string wDir, int spBarNum, int spBarWidth, int spBarHeight, I3Color spBarColor, double spBarAlpha, double z2)
		{
			FileTools.Delete(wDir);
			FileTools.CreateDir(wDir);

			this.SpData = new SpectrumData(spectrumFile);

			double a = -1.0;
			double foa = 0.0;

			double xz = DDConsts.Screen_W * 1.0 / jacket.Get_W();
			double yz = DDConsts.Screen_H * 1.0 / jacket.Get_H();

			double bz1 = Math.Max(xz, yz);
			double bz2 = Math.Min(xz, yz);

			double z1 = 1.0;
			//double z2 = 2.0;

			const int JACKET_MARGIN = 10;

			using (DDSubScreen pseudoMainScreen = new DDSubScreen(
				DDConsts.Screen_W,
				DDConsts.Screen_H
				))
			using (DDSubScreen workScreen = new DDSubScreen(
				DDConsts.Screen_W,
				DDConsts.Screen_H
				))
			using (DDSubScreen jacketScreen = new DDSubScreen(
				jacket.Get_W() + JACKET_MARGIN * 2,
				jacket.Get_H() + JACKET_MARGIN * 2,
				true
				))
			{
				// ---- jacketScreen

				DDSubScreenUtils.ChangeDrawScreen(jacketScreen);
				DX.ClearDrawScreen();

				DDDraw.DrawCenter(jacket, jacketScreen.GetSize().W / 2, jacketScreen.GetSize().H / 2);

				DDSubScreenUtils.RestoreDrawScreen();

				// ----

				SpectrumScreen0001 spScr = new SpectrumScreen0001(spBarNum, spBarWidth, spBarHeight, spBarColor);

				while (this.Frame < this.SpData.Rows.Length)
				{
					double[] row = this.SpData.Rows[this.Frame];

					spScr.Draw(row);

					// ---- workScreen

					DDSubScreenUtils.ChangeDrawScreen(workScreen);

					DDDraw.DrawBegin(jacket, DDConsts.Screen_W / 2, DDConsts.Screen_H / 2);
					DDDraw.DrawZoom(bz1 * z1);
					DDDraw.DrawEnd();

					DX.GraphFilter(workScreen.GetHandle(), DX.DX_GRAPH_FILTER_GAUSS, 16, 1000);

					//DDSubScreenUtils.RestoreDrawScreen();

					// ---- pseudoMainScreen

					DDSubScreenUtils.ChangeDrawScreen(pseudoMainScreen);

					DDDraw.DrawSimple(workScreen.ToPicture(), 0, 0);

					DDCurtain.DrawCurtain(-0.5);

					DDDraw.DrawBegin(
						jacketScreen.ToPicture(),
						//jacket,
						DDConsts.Screen_W / 2, DDConsts.Screen_H / 2);
					DDDraw.DrawZoom(bz2 * z2);
					DDDraw.DrawEnd();

					DDCurtain.DrawCurtain(Math.Min(a, foa));

					DDDraw.SetAlpha(spBarAlpha);
					DDDraw.DrawCenter(spScr.Screen.ToPicture(), DDConsts.Screen_W / 2, DDConsts.Screen_H - spBarHeight / 2 - 10);
					DDDraw.Reset();

					// ここでフレームを保存
					DX.SaveDrawScreen(0, 0, DDConsts.Screen_W, DDConsts.Screen_H, Path.Combine(wDir, string.Format("{0}.bmp", this.Frame)));

					DDSubScreenUtils.RestoreDrawScreen();

					// ---- 実際に表示される画面の描画

					DDCurtain.DrawCurtain();

					{
						double rate = this.Frame * 1.0 / this.SpData.Rows.Length;
						const int PROGRESS_BAR_H = 10;

						DDDraw.DrawRect(DDGround.GeneralResource.WhiteBox, 0, (DDConsts.Screen_H - PROGRESS_BAR_H) / 2, DDConsts.Screen_W * rate, PROGRESS_BAR_H);
					}

					// ----

					if (40 < this.Frame)
						DDUtils.Approach(ref a, 0.0, 0.985);

					if (this.SpData.Rows.Length - 40 < this.Frame)
						DDUtils.Approach(ref foa, -1.0, 0.9);

					//DDUtils.Approach(ref z1, 1.2, 0.999);
					//z1 += 0.0001;
					DDUtils.Approach(ref z2, 1.0, 0.9985);

					this.MG_EachFrame();
				}
			}
		}
	}
}
