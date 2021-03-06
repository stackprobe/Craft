﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.AudioPicMP4s.MovieMakers.VideoMakers.VideoImageMakers.Options
{
	public class VIMFadeIn : VIMFadeInOut
	{
		private int Delay;

		public VIMFadeIn(int delay, int span)
			: base(span, span, span)
		{
			this.Delay = delay;
		}

		protected override void EachFrame()
		{
			if (this.Frame == this.Delay)
				this.TargetValue = 0;
		}
	}
}
