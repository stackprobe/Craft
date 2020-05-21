using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using Charlotte.AudioPicMP4s.Internal;
using System.Drawing;
using Charlotte.AudioPicMP4s.Internal.SpectrumGraphs;

namespace Charlotte.AudioPicMP4s.MovieMakers.VideoMakers
{
	public class VideoMaker0001 : IVideoMaker
	{
		public string DiscJacketFile;

		// <---- prm

		private Canvas2 DiscJacket;
		private Canvas2 BluredDiscJacket;
		private Canvas2 MarginedDiscJacket;

		public void MakeVideo(WaveData wave, WorkingDir wd, Action<Canvas2> addImage)
		{
			this.DiscJacket = new Canvas2(this.DiscJacketFile);
			this.BluredDiscJacket = PictureUtils.Blur(this.DiscJacket, 5); // 要調整
			this.MarginedDiscJacket = PictureUtils.PutMargin(this.DiscJacket);

			ShadowSpectraData ss = new ShadowSpectraData();
			FadeInOutData f1 = new FadeInOutData() { MaxValue = 20 };
			FadeInOutData f2 = new FadeInOutData();
			f1.Rate = 1.0;

			int frameNum = DoubleTools.ToInt((wave.Length * 1.0 / wave.WavHz) * AudioPicMP4Props.FPS);

			for (int frame = 0; frame < frameNum; frame++)
			{
				if (frame == 2 * AudioPicMP4Props.FPS)
					f1.TargetRate = 0.0;

				if (frame == frameNum - 10 - f2.MaxValue)
					f2.TargetRate = 1.0;

				f1.Approach();
				f2.Approach();

				double rate = frame * 1.0 / (frameNum - 1);
				double invRate = 1.0 - rate;

				double r1 = 1.0 + 0.2 * rate;
				double r2 = 1.0 + 0.1 * invRate;

				D4Rect wallRect;
				D4Rect frntRect;

				{
					D2Size size = Utils.TangentSize(
						new D2Size(AudioPicMP4Props.VIDEO_W, AudioPicMP4Props.VIDEO_H),
						new D2Size(this.DiscJacket.GetWidth(), this.DiscJacket.GetHeight()),
						false
						);

					size.W *= r1;
					size.H *= r1;

					wallRect = Utils.Centering(new D2Size(AudioPicMP4Props.VIDEO_W, AudioPicMP4Props.VIDEO_H), size);
				}

				{
					D2Size size = Utils.TangentSize(
						new D2Size(AudioPicMP4Props.VIDEO_W, AudioPicMP4Props.VIDEO_H),
						new D2Size(this.DiscJacket.GetWidth(), this.DiscJacket.GetHeight()),
						true
						);

					size.W *= r2;
					size.H *= r2;

					// マージン分
					size.W *= this.MarginedDiscJacket.GetWidth() * 1.0 / this.DiscJacket.GetWidth();
					size.H *= this.MarginedDiscJacket.GetHeight() * 1.0 / this.DiscJacket.GetHeight();

					frntRect = Utils.Centering(new D2Size(AudioPicMP4Props.VIDEO_W, AudioPicMP4Props.VIDEO_H), size);
				}

				Canvas2 frameImg = new Canvas2(AudioPicMP4Props.VIDEO_W, AudioPicMP4Props.VIDEO_H);

				PictureUtils.Paste(frameImg, this.BluredDiscJacket, wallRect);
				PictureUtils.Filter(frameImg, Color.Black, 0.5); // 要調整
				PictureUtils.Paste(frameImg, this.MarginedDiscJacket, frntRect);
				PictureUtils.Filter(frameImg, Color.Black, 0.1); // 要調整

				PictureUtils.Filter(frameImg, Color.Black, f1.Rate); // 背景カーテン

				wave.SetWavPart(DoubleTools.ToInt((frame * 1.0 / AudioPicMP4Props.FPS + AudioPicMP4Props.AUDIO_DELAY_SEC) * wave.WavHz));
				SpectrumGraph0001 sg = new SpectrumGraph0001(hz => wave.GetSpectrum(hz));
				ss.Projection(sg.Spectra);
				this.DrawSpectra(frameImg, sg, ss);

				PictureUtils.Filter(frameImg, Color.Black, f2.Rate); // 前景カーテン

				addImage(frameImg);
			}
		}

		private void DrawSpectra(Canvas2 frameImg, SpectrumGraph0001 sg, ShadowSpectraData ss)
		{
			using (Graphics g = frameImg.GetGraphics(false))
			{
				int dr_l = 10;
				int dr_t = 10;
				int dr_w = frameImg.GetWidth() - 20;
				int dr_h = frameImg.GetHeight() - 20;

				for (int index = 0; index < sg.Spectra.Length; index++)
				{
					int x1 = (((index * 3 + 0) * dr_w) / (sg.Spectra.Length * 3 - 2));
					int x2 = (((index * 3 + 1) * dr_w) / (sg.Spectra.Length * 3 - 2));
					int w = x2 - x1;

					double v1 = ss.ShadowSpectra[index];
					double v2 = sg.Spectra[index];

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
		}
	}
}
