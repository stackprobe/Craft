using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using Charlotte.AudioPicMP4s.Internal;
using System.Drawing;

namespace Charlotte.AudioPicMP4s.MovieMakers.VideoMakers.VideoImageMakers.Backgrounds01
{
	public class Background0001 : AbstractVideoImageMaker
	{
		private Canvas2 DiscJacket;
		private Canvas2 BluredDiscJacket;
		private Canvas2 MarginedDiscJacket;

		private double R1; // def: 0.2
		private double R2; // def: 0.1

		public Background0001(string discJacketFile, double r1 = 0.2, double r2 = 0.1)
		{
			this.DiscJacket = new Canvas2(discJacketFile);
			this.BluredDiscJacket = PictureUtils.Blur(this.DiscJacket, 5); // 要調整
			this.MarginedDiscJacket = PictureUtils.PutMargin(this.DiscJacket);

			this.R1 = r1;
			this.R2 = r2;
		}

		public override IEnumerable<Canvas2> GetImageSequence()
		{
			for (; ; )
			{
				double r1 = 1.0 + this.R1 * this.Rate;
				double r2 = 1.0 + this.R2 * this.InvRate;

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

				PictureUtils.Paste(this.FrameImg, this.BluredDiscJacket, wallRect);
				PictureUtils.Filter(this.FrameImg, Color.Black, 0.5); // 要調整
				PictureUtils.Paste(this.FrameImg, this.MarginedDiscJacket, frntRect);

				yield return null;
			}
		}
	}
}
