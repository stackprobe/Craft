using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.Drawing;

namespace Charlotte.AudioPicMP4s
{
	public class SpectrumEffect01
	{
		private const double AUDIO_DELAY_SEC = 0.2;

		private WaveData Wave;

		public SpectrumEffect01(WaveData wave)
		{
			this.Wave = wave;
		}

		private AutoList<double> ShadowSpectra = new AutoList<double>();

		public void DrawTo(PictureData picture, int frame, int frameNum)
		{
			double sec = frame * 1.0 / VideoData.FPS;

			this.Wave.SetWavPart((int)((sec + AUDIO_DELAY_SEC) * this.Wave.WavHz));

			List<double> spectra = new List<double>();
			int hz = 10;

			for (int c = 1; c <= 9; c++)
			{
				for (int d = 0; d < 10; d++)
				{
					double spectrum = 0.0;

					for (int i = 0; i < c; i++)
					{
						spectrum = Math.Max(spectrum, this.Wave.GetSpectrum(hz));
						hz += 10;
					}

					spectrum /= 30.0; // 要調整
					spectrum = Vf(spectrum);

					spectra.Add(spectrum);
				}
			}
			for (int i = 0; i < spectra.Count; i++)
			{
				double v = this.ShadowSpectra[i];

				v -= 0.01;
				v = Math.Max(v, spectra[i]);

				this.ShadowSpectra[i] = v;
			}

			Canvas2 dest = new Canvas2(picture.GetFrame().GetWidth(), picture.GetFrame().GetHeight());

			using (Graphics g = dest.GetGraphics(false))
			{
				g.FillRectangle(new SolidBrush(Color.FromArgb(64, 0, 0, 0)), 0, 0, dest.GetWidth(), dest.GetHeight());

				int dr_l = 10;
				int dr_t = 10;
				int dr_w = dest.GetWidth() - 20;
				int dr_h = dest.GetHeight() - 20;

				for (int i = 0; i < spectra.Count; i++)
				{
					int x1 = (((i * 3 + 0) * dr_w) / (spectra.Count * 3 - 2));
					int x2 = (((i * 3 + 1) * dr_w) / (spectra.Count * 3 - 2));
					int w = x2 - x1;

					double v1 = this.ShadowSpectra[i];
					double v2 = spectra[i];

					v1 /= 2.0; // 要調整
					v2 /= 2.0; // 要調整

					int h1 = DoubleTools.ToInt(v1 * dr_h);
					int h2 = DoubleTools.ToInt(v2 * dr_h);

					int y1 = dr_h - h1;
					int y2 = dr_h - h2;

					g.FillRectangle(new SolidBrush(Color.FromArgb(128, 255, 255, 255)),
						dr_l + x1, dr_t + y1, w, h1);
					g.FillRectangle(new SolidBrush(Color.White),
						dr_l + x1, dr_t + y2, w, h2);
				}
			}
			using (Graphics g = picture.GetFrame().GetGraphics(false))
			{
				g.DrawImage(dest.GetImage(), 0, 0);
			}
		}

		private static double Vf(double v)
		{
			double r = 1.0;

			for (; ; )
			{
				r *= 0.9;

				double b = 1.0 - r;

				if (v <= b)
					break;

				v -= b;
				v /= 2.0;
				v += b;
			}
			return v;
		}
	}
}
