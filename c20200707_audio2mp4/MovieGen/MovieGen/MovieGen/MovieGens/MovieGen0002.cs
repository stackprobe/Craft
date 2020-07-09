using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.MovieGens.MGEngines;
using Charlotte.Common;
using Charlotte.Tools;
using System.IO;
using DxLibDLL;
using Charlotte.MovieGens.SpectrumScreens;

namespace Charlotte.MovieGens
{
	public class MovieGen0002
	{
		private const string R_DIR = @"C:\temp\a1001";
		private const string W_DIR = @"C:\temp\a2001";

		private const string SS_DIR = @"C:\wb2\20200708_動画テストデータ\ss";

		private SpectrumData SpData;
		private int Frame = 0;

		private void MG_EachFrame()
		{
#if true // 本番・生成用
			DX.SaveDrawScreen(0, 0, DDConsts.Screen_W, DDConsts.Screen_H, Path.Combine(W_DIR, string.Format("{0}.bmp", this.Frame)));

			DDEngine.EachFrame();
#else // テスト・再生用
			// 60hz -> 20hz

			DDEngine.EachFrame(); // 1
			DDEngine.EachFrame(); // 2
			DDEngine.EachFrame(); // 3
#endif

			this.Frame++;
		}

		public void Main01()
		{
			FileTools.Delete(W_DIR);
			FileTools.CreateDir(W_DIR);

			this.SpData = new SpectrumData(Path.Combine(R_DIR, "Spectrum.csv"));

			foreach (DDScene scene in DDSceneUtils.Create(20))
			{
				DDCurtain.DrawCurtain();

				this.MG_EachFrame();
			}

			Func<bool> backLayer = () => true;
			Func<bool> foreLayer = EnumerableTools.Supplier(GetLayer01(Path.Combine(SS_DIR, "0006.png")));

			double fowLv = 0.0;

			for (int frmcnt = 0; this.Frame < this.SpData.Rows.Length; frmcnt++)
			{
				{
					Func<bool> nextLayer = null;

#if true // 本番用
					switch (frmcnt)
					{
						case 15: nextLayer = EnumerableTools.Supplier(GetLayer02(Path.Combine(SS_DIR, "0003.png"), 1)); break;
						case 30: nextLayer = EnumerableTools.Supplier(GetLayer02(Path.Combine(SS_DIR, "0007.png"), -1)); break;
						case 45: nextLayer = EnumerableTools.Supplier(GetLayer02(Path.Combine(SS_DIR, "0008.png"), 1)); break;
						case 60: nextLayer = EnumerableTools.Supplier(GetLayer02(Path.Combine(SS_DIR, "0013.png"), -1)); break;
						case 75: nextLayer = EnumerableTools.Supplier(GetLayer02(Path.Combine(SS_DIR, "0017.png"), 1)); break;
						case 90: nextLayer = EnumerableTools.Supplier(GetLayer03(Path.Combine(SS_DIR, "0018.png"))); break;
						case 160: nextLayer = EnumerableTools.Supplier(GetLayer04(Path.Combine(SS_DIR, "0019.png"))); break;
					}
#else // test test test
					switch (frmcnt)
					{
						case 10: nextLayer = EnumerableTools.Supplier(GetLayer04(Path.Combine(SS_DIR, "0019.png"))); break;
					}
#endif
					if (nextLayer != null)
					{
						backLayer = foreLayer;
						foreLayer = nextLayer;
					}
				}

				backLayer();
				foreLayer();

				if (this.SpData.Rows.Length - 40 < this.Frame)
				{
					DDUtils.Approach(ref fowLv, -1.0, 0.9);

					DDCurtain.DrawCurtain(fowLv);
				}

				this.MG_EachFrame();
			}
		}

		private IEnumerable<bool> GetLayer01(string imgFile)
		{
			DDPicture img = DDPictureLoaders.Standard(imgFile); // g

			double y = -190.0;
			double b = 10000.0;
			double cLv = -1.0;

			for (; ; )
			{
				using (DDSubScreen workScreen = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H))
				{
					DDSubScreenUtils.ChangeDrawScreen(workScreen);

					DDDraw.DrawBegin(img, DDConsts.Screen_W / 2, DDConsts.Screen_H / 2 + y);
					DDDraw.DrawZoom(1.353);
					DDDraw.DrawEnd();

					DX.GraphFilter(workScreen.GetHandle(), DX.DX_GRAPH_FILTER_GAUSS, 16, (int)b); // 1
					DX.GraphFilter(workScreen.GetHandle(), DX.DX_GRAPH_FILTER_GAUSS, 16, (int)b); // 2
					DX.GraphFilter(workScreen.GetHandle(), DX.DX_GRAPH_FILTER_GAUSS, 16, (int)b); // 3
					DX.GraphFilter(workScreen.GetHandle(), DX.DX_GRAPH_FILTER_GAUSS, 16, (int)b); // 4
					DX.GraphFilter(workScreen.GetHandle(), DX.DX_GRAPH_FILTER_GAUSS, 16, (int)b); // 5

					DDSubScreenUtils.RestoreDrawScreen();

					DDDraw.DrawSimple(DDPictureLoaders2.Wrapper(workScreen), 0, 0);
				}

				DDCurtain.DrawCurtain(cLv);

				DDUtils.Approach(ref y, 190.0, 0.9);
				DDUtils.Approach(ref b, 0.0, 0.7);
				DDUtils.Approach(ref cLv, 0.0, 0.8);

				yield return true;
			}
		}

		private IEnumerable<bool> GetLayer02(string imgFile, int xDirSign)
		{
			DDPicture img = DDPictureLoaders.Standard(imgFile); // g

			double x = DDConsts.Screen_W * xDirSign;
			double y = -190.0;
			double a = 0.1;

			for (; ; )
			{
				DDDraw.DrawBegin(img, DDConsts.Screen_W / 2 + x, DDConsts.Screen_H / 2 + y);
				DDDraw.DrawZoom(1.353);
				DDDraw.SetAlpha(a);
				DDDraw.DrawEnd();

				DDUtils.Approach(ref x, 0.0, 0.45);
				DDUtils.Approach(ref y, 190.0, 0.9);
				DDUtils.Approach(ref a, 1.0, 0.95);

				yield return true;
			}
		}

		private IEnumerable<bool> GetLayer03(string imgFile)
		{
			DDPicture img = DDPictureLoaders.Standard(imgFile); // g

			double a = 0.0;
			double z = 2.0;

			double t1x = 100;
			double t2x = 100;
			double t3x = 100;

			for (int frmcnt = 0; ; frmcnt++)
			{
				DDDraw.DrawBegin(img, DDConsts.Screen_W / 2, DDConsts.Screen_H / 2);
				DDDraw.DrawZoom(1.353);
				DDDraw.SetAlpha(a);
				DDDraw.DrawEnd();

				DDDraw.DrawBegin(img, DDConsts.Screen_W / 2, DDConsts.Screen_H / 2);
				DDDraw.DrawZoom(1.353 * z);
				DDDraw.SetAlpha(a / 2.0);
				DDDraw.DrawEnd();

				I3Color color = new I3Color((int)(255 * a), (int)(255 * a), (int)(255 * a));

				if (10 < frmcnt)
				{
					DDFontUtils.DrawString((int)(1000 + t1x), 620, "TOKYO", DDFontUtils.GetFont("Ink Free", 100), false, color);

					DDUtils.Approach(ref t1x, 0.0, 0.8);
				}
				if (15 < frmcnt)
				{
					DDFontUtils.DrawString((int)(1200 + t2x), 720, "LOVE", DDFontUtils.GetFont("Ink Free", 200), false, color);

					DDUtils.Approach(ref t2x, 0.0, 0.8);
				}
				if (20 < frmcnt)
				{
					DDFontUtils.DrawString((int)(1000 + t3x), 900, "STORY　　　　1991", DDFontUtils.GetFont("Ink Free", 100), false, color);

					DDUtils.Approach(ref t3x, 0.0, 0.8);
				}

				DDUtils.Approach(ref a, 1.0, 0.98);
				DDUtils.Approach(ref z, 1.0, 0.98);

				yield return true;
			}
		}

		private IEnumerable<bool> GetLayer04(string imgFile)
		{
			DDPicture img = DDPictureLoaders.Standard(imgFile); // g

			double y = -190.0;
			double a = 0.0;
			double spa = 0.0;

			SpectrumScreen0001 spScr = new SpectrumScreen0001();

			for (int frmcnt = 0; ; frmcnt++)
			{
				DDDraw.DrawBegin(img, DDConsts.Screen_W / 2, DDConsts.Screen_H / 2 - y);
				DDDraw.DrawZoom(1.353);
				DDDraw.SetAlpha(a);
				DDDraw.DrawEnd();

				spScr.Draw(this.SpData.Rows[this.Frame]);

				DDDraw.DrawBegin(DDPictureLoaders2.Wrapper(spScr.Screen), 1500, 400);
				DDDraw.SetAlpha(spa);
				DDDraw.DrawZoom(0.75);
				DDDraw.DrawEnd();

				DDUtils.Approach(ref y, 190.0, 0.9995);
				DDUtils.Approach(ref a, 1.0, 0.9);

				if (60 < frmcnt)
					DDUtils.Approach(ref spa, 1.0, 0.95);

				yield return true;
			}
		}
	}
}
