using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using Charlotte.AudioPicMP4s.Internal;

namespace Charlotte.AudioPicMP4s.MovieMakers.VideoMakers.VideoImageMakers.Backgrounds
{
	public class Background0002 : AbstractVideoImageMaker
	{
		private const int BLUR_COUNT = 30;

		private Canvas2[] DiscJackets = new Canvas2[BLUR_COUNT];
		private bool LeftToRight;

		public Background0002(string discJacketFile, bool leftToRight)
		{
			this.DiscJackets[0] = new Canvas2(discJacketFile);

			for (int index = 1; index < BLUR_COUNT; index++)
			{
				this.DiscJackets[index] = PictureUtils.BlurOnce(this.DiscJackets[index - 1], index);
			}
			this.LeftToRight = leftToRight;
		}

		private D2Size DrawSize;
		private D2Point DrawPos1;
		private D2Point DrawPos2;

		public override IEnumerable<Canvas2> GetImageSequence()
		{
			this.DrawSize = Utils.TangentSize(
				new D2Size(AudioPicMP4Props.VIDEO_W, AudioPicMP4Props.VIDEO_H),
				new D2Size(this.DiscJackets[0].GetWidth(), this.DiscJackets[0].GetHeight()),
				false
				);

			this.DrawPos1 = new D2Point(0.0, 0.0);
			this.DrawPos2 = new D2Point(
				this.DrawSize.W - AudioPicMP4Props.VIDEO_W,
				this.DrawSize.H - AudioPicMP4Props.VIDEO_H
				);

			this.DrawPos2 *= -1.0;

			const int NAZO_MARGIN = 3;

			for (int index = BLUR_COUNT - 1; 0 <= index; index--)
			{
				this.Draw(index);
				yield return null;
			}
			while (this.Frame + BLUR_COUNT + NAZO_MARGIN < this.FrameNum)
			{
				this.Draw(0);
				yield return null;
			}
			for (int index = 0; index < BLUR_COUNT; index++)
			{
				this.Draw(index);
				yield return null;
			}
			for (; ; )
			{
				this.Draw(BLUR_COUNT - 1);
				yield return null;
			}
		}

		private void Draw(int index)
		{
			Canvas2 discJacket = this.DiscJackets[index];

			double rate = this.LeftToRight ? this.Rate : this.InvRate;

			double l = this.DrawPos1.X + (this.DrawPos2.X - this.DrawPos1.X) * rate;
			double t = this.DrawPos1.Y + (this.DrawPos2.Y - this.DrawPos1.Y) * rate;
			double w = this.DrawSize.W;
			double h = this.DrawSize.H;

			PictureUtils.Paste(this.FrameImg, discJacket, l, t, w, h);
		}
	}
}
