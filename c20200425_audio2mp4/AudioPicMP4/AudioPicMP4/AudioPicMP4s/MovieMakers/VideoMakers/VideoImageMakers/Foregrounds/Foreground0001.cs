using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using Charlotte.AudioPicMP4s.Internal;
using Charlotte.AudioPicMP4s.Internal.SpectrumGraphs;
using System.Drawing;

namespace Charlotte.AudioPicMP4s.MovieMakers.VideoMakers.VideoImageMakers.Foregrounds
{
	public class Foreground0001 : AbstractVideoImageMaker
	{
		private const int MARGIN_TB = 10;
		private const int BAR_WIDTH = 10;
		private const int BAR_INTERVAL = 20;

		public override IEnumerable<Canvas2> GetImageSequence()
		{
			ShadowSpectraData ss = new ShadowSpectraData();

			for (; ; )
			{
				this.Wave.SetWavPart(DoubleTools.ToInt((this.Frame * 1.0 / AudioPicMP4Props.FPS + AudioPicMP4Props.AUDIO_DELAY_SEC) * this.Wave.WavHz));
				SpectrumGraph0001 sg = new SpectrumGraph0001(hz => this.Wave.GetSpectrum(hz));
				ss.Projection(sg.Spectra);
				int w = sg.Spectra.Length * (BAR_WIDTH + BAR_INTERVAL) + BAR_INTERVAL;

				Canvas2 frameImg = new Canvas2(w, AudioPicMP4Props.VIDEO_H);

				PictureUtils.Fill(frameImg, Color.Transparent);

				this.DrawSpectra(frameImg, sg, ss);

				yield return frameImg;
			}
		}

		private void DrawSpectra(Canvas2 frameImg, SpectrumGraph0001 sg, ShadowSpectraData ss)
		{
			using (Graphics g = frameImg.GetGraphics(false))
			{
				int dr_l = 0;
				int dr_t = MARGIN_TB;
				int dr_w = frameImg.GetWidth();
				int dr_h = frameImg.GetHeight() - MARGIN_TB * 2;

				for (int index = 0; index < sg.Spectra.Length; index++)
				{
					int x = index * (BAR_WIDTH + BAR_INTERVAL) + BAR_INTERVAL;
					int w = BAR_WIDTH;

					double v1 = ss.ShadowSpectra[index];
					double v2 = sg.Spectra[index];

					v1 *= 0.5; // 要調整
					v2 *= 0.5; // 要調整

					int h1 = DoubleTools.ToInt(v1 * dr_h);
					int h2 = DoubleTools.ToInt(v2 * dr_h);

					int y1 = dr_h - h1;
					int y2 = dr_h - h2;

					g.FillRectangle(new SolidBrush(Color.FromArgb(128, 255, 255, 255)),
						dr_l + x, dr_t + y1, w, h1);
					g.FillRectangle(new SolidBrush(Color.White),
						dr_l + x, dr_t + y2, w, h2);
				}
			}
		}
	}
}
