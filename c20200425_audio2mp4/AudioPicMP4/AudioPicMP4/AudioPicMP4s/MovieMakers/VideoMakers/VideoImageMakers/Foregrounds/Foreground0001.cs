using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.AudioPicMP4s.MovieMakers.VideoMakers.VideoImageMakers.Foregrounds
{
	public class Foreground0001 : AbstractVideoImageMaker
	{
		private IEnumerable<Canvas2> GetImageSequence()
		{
			throw null; // TODO
		}

		private Func<Canvas2> ImageSq = null;

		public override Canvas2 GetImage()
		{
			if (this.ImageSq == null)
				this.ImageSq = EnumerableTools.Supplier(this.GetImageSequence());

			return this.ImageSq();
		}
	}
}
