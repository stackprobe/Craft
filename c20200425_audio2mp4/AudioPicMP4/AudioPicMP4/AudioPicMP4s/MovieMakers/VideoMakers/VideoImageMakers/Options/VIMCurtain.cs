using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.Drawing;

namespace Charlotte.AudioPicMP4s.MovieMakers.VideoMakers.VideoImageMakers.Options
{
	public class VIMCurtain : AbstractVideoImageMaker
	{
		private Color Color;
		private double A;

		public VIMCurtain(double a)
			: this(Color.Black, a)
		{ }

		public VIMCurtain(Color color, double a)
		{
			this.Color = color;
			this.A = a;
		}

		public override IEnumerable<Canvas2> GetImageSequence()
		{
			for (; ; )
			{
				PictureUtils.Filter(this.FrameImg, this.Color, this.A);

				yield return null;
			}
		}
	}
}
