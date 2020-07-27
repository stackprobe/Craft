using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.Tools;
using DxLibDLL;

namespace Charlotte.ConvMains
{
	public class SpectrumScreen0001 : IDisposable
	{
		public DDSubScreen Screen;
		public DDSubScreen GraphScreen;
		public ShadowSpectraData ShadowSpectra = null;

		public int BarNum;
		public int Bar_W;
		public int Bar_H;
		public I3Color BarColor;

		public SpectrumScreen0001(int barNum, int barWidth, int barHeight, I3Color barColor)
		{
			this.Screen = new DDSubScreen(960, barHeight + 100, true); // g
			this.GraphScreen = new DDSubScreen(900, barHeight, true); // g

			this.BarNum = barNum;
			this.Bar_W = barWidth;
			this.Bar_H = barHeight;
			this.BarColor = barColor;
		}

		public void Dispose()
		{
			if (this.Screen != null)
			{
				this.Screen.Dispose();
				this.Screen = null;
			}
			if (this.GraphScreen != null)
			{
				this.GraphScreen.Dispose();
				this.GraphScreen = null;
			}
		}

		public void Draw(double[] spectra)
		{
			if (spectra.Length != Consts.SPECTRA_LEN)
				throw null;

			if (this.ShadowSpectra == null)
				this.ShadowSpectra = new ShadowSpectraData();

			this.ShadowSpectra.Projection(spectra);

			DDSubScreenUtils.ChangeDrawScreen(this.GraphScreen);
			DX.ClearDrawScreen();

			for (int layer = 0; layer < 2; layer++)
			{
				for (int bi = 0; bi < this.BarNum; bi++)
				{
					int c1 = (Consts.SPECTRA_LEN * (bi + 0)) / this.BarNum;
					int c2 = (Consts.SPECTRA_LEN * (bi + 1)) / this.BarNum;

					double v = 0.0;

					for (int c = c1; c < c2; c++)
					{
						v += (layer == 0 ? this.ShadowSpectra.ShadowSpectra : spectra)[c];
					}
					v /= c2 - c1;

					int x1 = ((this.GraphScreen.GetSize().W - this.Bar_W) * bi) / (this.BarNum - 1);
					int x2 = x1 + this.Bar_W;
					int y1 = (int)((1.0 - v) * this.GraphScreen.GetSize().H);
					int y2 = this.GraphScreen.GetSize().H;

					if (y1 + 1 < y2)
					{
						double bright = layer == 0 ? 0.5 : 1.0;

						DDDraw.SetBright(
							bright * (this.BarColor.R / 255.0),
							bright * (this.BarColor.G / 255.0),
							bright * (this.BarColor.B / 255.0)
							);
						DDDraw.DrawRect(DDGround.GeneralResource.WhiteBox, x1, y1, x2 - x1, y2 - y1);
						DDDraw.Reset();
					}
				}
			}

			DDSubScreenUtils.ChangeDrawScreen(this.Screen);
			DX.ClearDrawScreen();

			for (int c = 0; c < 2; c++)
			{
				DDDraw.DrawBegin(this.GraphScreen.ToPicture(), this.Screen.GetSize().W / 2, this.Screen.GetSize().H / 2);
				DDDraw.SetBright(0, 0, 0);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				DX.GraphFilter(this.Screen.GetHandle(), DX.DX_GRAPH_FILTER_GAUSS, 16, 1000);
			}

			DDDraw.DrawCenter(this.GraphScreen.ToPicture(), this.Screen.GetSize().W / 2, this.Screen.GetSize().H / 2);

			DDSubScreenUtils.RestoreDrawScreen();
		}
	}
}
