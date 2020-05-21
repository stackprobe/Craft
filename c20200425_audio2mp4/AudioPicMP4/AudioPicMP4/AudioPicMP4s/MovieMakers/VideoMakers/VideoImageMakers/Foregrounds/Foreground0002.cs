using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.AudioPicMP4s.Internal;
using Charlotte.Tools;
using Charlotte.AudioPicMP4s.Internal.SpectrumGraphs;
using System.Drawing;

namespace Charlotte.AudioPicMP4s.MovieMakers.VideoMakers.VideoImageMakers.Foregrounds
{
	public class Foreground0002 : AbstractVideoImageMaker
	{
		private const int MARGIN_TB = 10;
		private const int BAR_WIDTH = 10;
		private const int BAR_INTERVAL = 10;

		public override IEnumerable<Tools.Canvas2> GetImageSequence()
		{
			ShadowSpectraData ssL = new ShadowSpectraData();
			ShadowSpectraData ssR = new ShadowSpectraData();

			for (; ; )
			{
				this.Wave.SetWavPart(DoubleTools.ToInt((this.Frame * 1.0 / AudioPicMP4Props.FPS + AudioPicMP4Props.AUDIO_DELAY_SEC) * this.Wave.WavHz));
				SpectrumGraph0001 sgL = new SpectrumGraph0001(hz => this.Wave.GetSpectrum_L(hz)) { R1 = 0.5, R2 = 0.5 };
				SpectrumGraph0001 sgR = new SpectrumGraph0001(hz => this.Wave.GetSpectrum_R(hz)) { R1 = 0.5, R2 = 0.5 };
				ssL.Projection(sgL.Spectra);
				ssR.Projection(sgR.Spectra);
				int w = sgL.Spectra.Length * (BAR_WIDTH + BAR_INTERVAL) + BAR_INTERVAL;

				Canvas2 frameImg_L = new Canvas2(w, AudioPicMP4Props.VIDEO_H);
				Canvas2 frameImg_R = new Canvas2(w, AudioPicMP4Props.VIDEO_H);

				PictureUtils.Fill(frameImg_L, Color.Transparent);
				PictureUtils.Fill(frameImg_R, Color.Transparent);

				this.DrawSpectra(frameImg_L, sgL, ssL, Color.FromArgb(255, 255, 200), false);
				this.DrawSpectra(frameImg_R, sgR, ssR, Color.FromArgb(200, 255, 255), true);

				Canvas2 frameImg = new Canvas2(w * 2, AudioPicMP4Props.VIDEO_H);

				PictureUtils.Paste(frameImg, frameImg_L, 0, 0, w, AudioPicMP4Props.VIDEO_H);
				PictureUtils.Paste(frameImg, frameImg_R, w, 0, w, AudioPicMP4Props.VIDEO_H);

				yield return frameImg;
			}
		}

		private void DrawSpectra(Canvas2 frameImg, SpectrumGraph0001 sg, ShadowSpectraData ss, Color barColor, bool rightToLeft)
		{
			using (Graphics g = frameImg.GetGraphics(false))
			{
				int dr_l = 0;
				int dr_t = MARGIN_TB;
				int dr_w = frameImg.GetWidth();
				int dr_h = frameImg.GetHeight() - MARGIN_TB * 2;

				for (int index = 0; index < sg.Spectra.Length; index++)
				{
					int x = (rightToLeft ? sg.Spectra.Length - 1 - index : index) * (BAR_WIDTH + BAR_INTERVAL) + BAR_INTERVAL;
					int w = BAR_WIDTH;

					double v1 = ss.ShadowSpectra[index];
					double v2 = sg.Spectra[index];

					v1 *= 1.0; // 要調整
					v2 *= 1.0; // 要調整

					int h1 = DoubleTools.ToInt(v1 * dr_h);
					int h2 = DoubleTools.ToInt(v2 * dr_h);

					int y1 = dr_h - h1;
					int y2 = dr_h - h2;

					g.FillRectangle(new SolidBrush(Color.FromArgb(128, barColor.R, barColor.G, barColor.B)),
						dr_l + x, dr_t + y1, w, h1);
					g.FillRectangle(new SolidBrush(barColor),
						dr_l + x, dr_t + y2, w, h2);
				}
			}
		}
	}
}
