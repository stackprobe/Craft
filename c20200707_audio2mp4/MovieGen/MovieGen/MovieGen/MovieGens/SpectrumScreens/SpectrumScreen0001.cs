using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using DxLibDLL;
using Charlotte.MovieGens.MGEngines;

namespace Charlotte.MovieGens.SpectrumScreens
{
	public class SpectrumScreen0001
	{
		public DDSubScreen Screen;
		public DDSubScreen GraphScreen;
		public ShadowSpectraData ShadowSpectra = null;

		public SpectrumScreen0001()
		{
			this.Screen = new DDSubScreen(1000, 1000, true); // g
			this.GraphScreen = new DDSubScreen(880, 880, true); // g
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

			for (int c = 0; c < 30; c++)
			{
				double v =
					this.ShadowSpectra.ShadowSpectra[c * 3 + 0] +
					this.ShadowSpectra.ShadowSpectra[c * 3 + 1] +
					this.ShadowSpectra.ShadowSpectra[c * 3 + 2];

				v /= 3.0;

				int x1 = c * 30;
				int x2 = x1 + 10;
				int y1 = (int)((1.0 - v) * 600);
				int y2 = 600;

				if (y1 + 1 < y2)
				{
					DDDraw.SetBright(0.4, 0.5, 0.6);
					DDDraw.DrawRect(DDGround.GeneralResource.WhiteBox, x1, y1, x2 - x1, y2 - y1);
					DDDraw.Reset();
				}
			}
			for (int c = 0; c < 30; c++)
			{
				double v =
					spectra[c * 3 + 0] +
					spectra[c * 3 + 1] +
					spectra[c * 3 + 2];

				v /= 3.0;

				int x1 = c * 30;
				int x2 = x1 + 10;
				int y1 = (int)((1.0 - v) * 600);
				int y2 = 600;

				if (y1 + 1 < y2)
				{
					DDDraw.DrawRect(DDGround.GeneralResource.WhiteBox, x1, y1, x2 - x1, y2 - y1);
				}
			}

			DDDraw.DrawRect(DDGround.GeneralResource.WhiteBox, 0, 620, 880, 10);

			DDFontUtils.DrawString(0,
				640, "小田和正", DDFontUtils.GetFont("メイリオ", 80));
			DDFontUtils.DrawString(270,
				740, "ラブ・ストーリーは突然に", DDFontUtils.GetFont("メイリオ", 50));
			DDFontUtils.DrawString(30,
				840, "『 東京ラブストーリー ( 1991 ) 』　主題歌", DDFontUtils.GetFont("メイリオ", 40));

			DDSubScreenUtils.ChangeDrawScreen(this.Screen);
			DX.ClearDrawScreen();

			DDDraw.DrawBegin(DDPictureLoaders2.Wrapper(this.GraphScreen), 500, 500);
			DDDraw.SetBright(0, 0, 0);
			DDDraw.DrawEnd();
			DDDraw.Reset();

			DX.GraphFilter(this.Screen.GetHandle(), DX.DX_GRAPH_FILTER_GAUSS, 16, 1000);

			DDDraw.DrawCenter(DDPictureLoaders2.Wrapper(this.GraphScreen), 500, 500);

			DDSubScreenUtils.RestoreDrawScreen();
		}
	}
}
