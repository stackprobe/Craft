using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.MovieGens.MGEngines;
using Charlotte.Tools;
using DxLibDLL;
using System.IO;
using Charlotte.MovieGens.SpectrumScreens;

namespace Charlotte.MovieGens
{
	public class MovieGen0001
	{
		private string RDir;
		private string WDir;

		private SpectrumData SpData;
		private int Frame = 0;

		private void MG_EachFrame()
		{
#if true // 本番・生成用
			DX.SaveDrawScreen(0, 0, DDConsts.Screen_W, DDConsts.Screen_H, Path.Combine(this.WDir, string.Format("{0}.bmp", this.Frame)));

			DDEngine.EachFrame();
#else // テスト・再生用
			// 60hz -> 20hz

			DDEngine.EachFrame(); // 1
			DDEngine.EachFrame(); // 2
			DDEngine.EachFrame(); // 3
#endif

			this.Frame++;
		}

		public void Main01(string rDir, string wRootDir, int spBarNum, int spBarWidth, int spBarHeight, I3Color spBarColor, double spBarAlpha)
		{
			string wLocalDir = string.Format("Bar={0:D2}_Bar-W={1:D2}_Bar-H={2:D3}_Bar-C={3}_Bar-A={4:F3}", spBarNum, spBarWidth, spBarHeight, spBarColor, spBarAlpha);

			this.RDir = rDir;
			this.WDir = Path.Combine(wRootDir, wLocalDir);

			if (Directory.Exists(this.RDir) == false)
				throw new Exception("no RDir: " + this.RDir);

			FileTools.Delete(this.WDir);
			FileTools.CreateDir(this.WDir);

			DDPicture jacket = DDPictureLoaders.Standard(Path.Combine(this.RDir, "Jacket.jpg")); // g

			this.SpData = new SpectrumData(Path.Combine(this.RDir, "Spectrum.csv"));

			double a = -1.0;
			double foa = 0.0;

			double xz = DDConsts.Screen_W * 1.0 / jacket.Get_W();
			double yz = DDConsts.Screen_H * 1.0 / jacket.Get_H();

			double bz1 = Math.Max(xz, yz);
			double bz2 = Math.Min(xz, yz);

			double z1 = 1.0;
			double z2 = 2.0;

			const int JACKET_MARGIN = 10;

			DDSubScreen workScreen = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H); // g
			DDSubScreen jacketScreen = new DDSubScreen(jacket.Get_W() + JACKET_MARGIN * 2, jacket.Get_H() + JACKET_MARGIN * 2, true); // g

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

				// ---- workScreen

				DDSubScreenUtils.ChangeDrawScreen(workScreen);

				DDDraw.DrawBegin(jacket, DDConsts.Screen_W / 2, DDConsts.Screen_H / 2);
				DDDraw.DrawZoom(bz1 * z1);
				DDDraw.DrawEnd();

				DX.GraphFilter(workScreen.GetHandle(), DX.DX_GRAPH_FILTER_GAUSS, 16, 1000);

				DDSubScreenUtils.RestoreDrawScreen();

				// ----

				DDDraw.DrawSimple(workScreen.ToPicture(), 0, 0);

				DDCurtain.DrawCurtain(-0.5);

				DDDraw.DrawBegin(
					jacketScreen.ToPicture(),
					//jacket,
					DDConsts.Screen_W / 2, DDConsts.Screen_H / 2);
				DDDraw.DrawZoom(bz2 * z2);
				DDDraw.DrawEnd();

				DDCurtain.DrawCurtain(Math.Min(a, foa));

				spScr.Draw(this.SpData.Rows[this.Frame]);

				DDDraw.SetAlpha(spBarAlpha); // ★要調整
				DDDraw.DrawCenter(spScr.Screen.ToPicture(), DDConsts.Screen_W / 2, DDConsts.Screen_H - spBarHeight / 2 - 10);
				DDDraw.Reset();

				if (40 < this.Frame)
					DDUtils.Approach(ref a, 0.0, 0.985);

				if (this.SpData.Rows.Length - 40 < this.Frame)
					DDUtils.Approach(ref foa, -1.0, 0.9);

				//DDUtils.Approach(ref z1, 1.2, 0.999);
				//z1 += 0.0001;
				DDUtils.Approach(ref z2, 1.0, 0.9985);

				this.MG_EachFrame();
			}

			// ゴミ内のハンドルだけでも開放する。
			{
				DDPictureUtils.UnloadAll();
				DDSubScreenUtils.UnloadAll();
			}
		}
	}
}
