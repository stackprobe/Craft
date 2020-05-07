using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Charlotte.Tools;

namespace Charlotte.AudioPicMP4s.MovieMakers.VideoMakers.VideoImageMakers.Options
{
	public abstract class VIMFadeInOut : AbstractVideoImageMaker
	{
		protected int MaxValue;
		protected int TargetValue;
		protected int Value;

		public VIMFadeInOut(int maxValue, int targetValue, int value)
		{
			this.MaxValue = maxValue;
			this.TargetValue = targetValue;
			this.Value = value;
		}

		public override IEnumerable<Canvas2> GetImageSequence()
		{
			for (; ; )
			{
				this.EachFrame();
				this.Approach();
				this.DrawToFrameImg();

				yield return null;
			}
		}

		protected abstract void EachFrame();

		private void Approach()
		{
			if (this.Value < this.TargetValue)
				this.Value++;
			else if (this.TargetValue < this.Value)
				this.Value--;
		}

		private void DrawToFrameImg()
		{
			if (this.Value != 0)
			{
				double a = this.Value * 1.0 / this.MaxValue;

				PictureUtils.Filter(this.FrameImg, Color.Black, a);
			}
		}
	}
}
