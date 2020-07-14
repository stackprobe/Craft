using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.MovieGens.MGEngines;
using DxLibDLL;
using Charlotte.Tools;

namespace Charlotte.MovieGens.SpectrumScreens
{
	public class SpectrumScreen0002
	{
		public DDSubScreen Screen;
		public DDSubScreen GraphScreen;
		public ShadowSpectraData ShadowSpectra = null;

		public SpectrumScreen0002()
		{
			this.Screen = new DDSubScreen(1920, 500, true); // g
			this.GraphScreen = new DDSubScreen(1600, 400, true); // g
		}

		public void Draw(double[] spectra)
		{
			if (spectra.Length != 90)
				throw null; // souteigai !!!

			if (this.ShadowSpectra == null)
				this.ShadowSpectra = new ShadowSpectraData();

			this.ShadowSpectra.Projection(spectra);

			DDSubScreenUtils.ChangeDrawScreen(this.GraphScreen);
			DX.ClearDrawScreen();

			for (int c = 0; c < 10; c++)
			{
				double v =
					this.ShadowSpectra.ShadowSpectra[c * 9 + 0] +
					this.ShadowSpectra.ShadowSpectra[c * 9 + 1] +
					this.ShadowSpectra.ShadowSpectra[c * 9 + 2] +
					this.ShadowSpectra.ShadowSpectra[c * 9 + 3] +
					this.ShadowSpectra.ShadowSpectra[c * 9 + 4] +
					this.ShadowSpectra.ShadowSpectra[c * 9 + 5] +
					this.ShadowSpectra.ShadowSpectra[c * 9 + 6] +
					this.ShadowSpectra.ShadowSpectra[c * 9 + 7] +
					this.ShadowSpectra.ShadowSpectra[c * 9 + 8];

				v /= 9.0;
				v *= 10.0;

				int iv = DoubleTools.ToInt(v);

				for (int d = 0; d < iv; d++)
				{
					int x1 = (c + 0) * 160;
					int x2 = (c + 1) * 160;
					int y1 = (d + 0) * 40;
					int y2 = (d + 1) * 40;

					x1 += 5;
					x2 -= 5;
					y1 += 3;
					y2 -= 3;

					DDDraw.SetBright(0.6, 0.4, 0.4);
					DDDraw.DrawRect(DDGround.GeneralResource.WhiteBox, x1, y1, x2 - x1, y2 - y1);
					DDDraw.Reset();
				}
			}
			for (int c = 0; c < 10; c++)
			{
				double v =
					spectra[c * 9 + 0] +
					spectra[c * 9 + 1] +
					spectra[c * 9 + 2] +
					spectra[c * 9 + 3] +
					spectra[c * 9 + 4] +
					spectra[c * 9 + 5] +
					spectra[c * 9 + 6] +
					spectra[c * 9 + 7] +
					spectra[c * 9 + 8];

				v /= 9.0;
				v *= 10.0;

				int iv = DoubleTools.ToInt(v);

				for (int d = 0; d < iv; d++)
				{
					int x1 = (c + 0) * 160;
					int x2 = (c + 1) * 160;
					int y1 = (d + 0) * 40;
					int y2 = (d + 1) * 40;

					x1 += 5;
					x2 -= 5;
					y1 += 3;
					y2 -= 3;

					DDDraw.SetBright(1.0, 0.8, 0.8);
					DDDraw.DrawRect(DDGround.GeneralResource.WhiteBox, x1, y1, x2 - x1, y2 - y1);
					DDDraw.Reset();
				}
			}

			DDSubScreenUtils.ChangeDrawScreen(this.Screen);
			DX.ClearDrawScreen();

			DDDraw.DrawBegin(DDPictureLoaders2.Wrapper(this.GraphScreen), this.Screen.GetSize().W / 2, this.Screen.GetSize().H / 2);
			DDDraw.SetBright(0, 0, 0);
			DDDraw.DrawEnd();
			DDDraw.Reset();

			DX.GraphFilter(this.Screen.GetHandle(), DX.DX_GRAPH_FILTER_GAUSS, 16, 1000);

			DDDraw.DrawCenter(DDPictureLoaders2.Wrapper(this.GraphScreen), this.Screen.GetSize().W / 2, this.Screen.GetSize().H / 2);

			DDSubScreenUtils.RestoreDrawScreen();
		}
	}
}
