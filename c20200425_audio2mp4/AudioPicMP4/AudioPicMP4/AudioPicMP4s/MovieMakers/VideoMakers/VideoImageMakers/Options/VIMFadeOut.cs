using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.AudioPicMP4s.MovieMakers.VideoMakers.VideoImageMakers.Options
{
	public class VIMFadeOut : VIMFadeInOut
	{
		private int Delay;

		public VIMFadeOut(int delay, int span)
			: base(span, 0, 0)
		{
			this.Delay = delay;
		}

		protected override void EachFrame()
		{
			if (this.Frame == this.FrameNum - this.Delay - this.MaxValue)
				this.TargetValue = this.MaxValue;
		}
	}
}
