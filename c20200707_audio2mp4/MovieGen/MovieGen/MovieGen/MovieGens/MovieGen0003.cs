using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.MovieGens.MGEngines;
using Charlotte.Common;
using Charlotte.Tools;
using System.IO;
using Charlotte.MovieGens.SpectrumScreens;
using DxLibDLL;

namespace Charlotte.MovieGens
{
	public class MovieGen0003
	{
		private const string R_DIR = @"C:\temp\a1001";
		private const string W_DIR = @"C:\temp\a2001";

		private SpectrumData SpData_L;
		private SpectrumData SpData_R;
		private int Frame = 0;

		private void MG_EachFrame()
		{
#if true
			DX.SaveDrawScreen(0, 0, DDConsts.Screen_W, DDConsts.Screen_H, Path.Combine(W_DIR, string.Format("{0}.bmp", this.Frame)));

			DDEngine.EachFrame();
#else
			// 60hz -> 20hz

			DDEngine.EachFrame(); // 1
			DDEngine.EachFrame(); // 2
			DDEngine.EachFrame(); // 3
#endif

			this.Frame++;
		}

		public void Main01()
		{
			DDPicture img = DDPictureLoaders.Standard(@"C:\wb2\20200708_動画テストデータ\ss\0020.png"); // g

			FileTools.Delete(W_DIR);
			FileTools.CreateDir(W_DIR);

			this.SpData_L = new SpectrumData(Path.Combine(R_DIR, "Spectrum_L.csv"));
			this.SpData_R = new SpectrumData(Path.Combine(R_DIR, "Spectrum_R.csv"));

			SpectrumScreen0002 spScr = new SpectrumScreen0002();

			double z = 1.2;
			double cLv = -1.0;
			double fowLv = 0.0;

			while (this.Frame < this.SpData_L.Rows.Length)
			{
				double[] row_L = this.SpData_L.Rows[this.Frame];
				double[] row_R = this.SpData_R.Rows[this.Frame];

				DDDraw.DrawBegin(img, DDConsts.Screen_W / 2, DDConsts.Screen_H / 2 + 80);
				DDDraw.DrawZoom(z * 1.353);
				DDDraw.DrawEnd();

				DDCurtain.DrawCurtain(Math.Min(cLv, fowLv));

				spScr.Draw(row_L);

				DDDraw.SetAlpha(0.8);
				DDDraw.DrawBegin(DDPictureLoaders2.Wrapper(spScr.Screen), DDConsts.Screen_W / 2, 180);
				DDDraw.DrawZoom(-0.9);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				spScr.Draw(row_R);

				DDDraw.SetAlpha(0.8);
				DDDraw.DrawBegin(DDPictureLoaders2.Wrapper(spScr.Screen), DDConsts.Screen_W / 2, 900);
				DDDraw.DrawZoom(0.9);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				if (this.SpData_L.Rows.Length - 40 < this.Frame)
					DDUtils.Approach(ref fowLv, -1.0, 0.9);

				if (40 < this.Frame)
					DDUtils.Approach(ref cLv, 1.0, 0.99);

				DDUtils.Approach(ref z, 1.0, 0.999);

				this.MG_EachFrame();
			}
		}
	}
}
