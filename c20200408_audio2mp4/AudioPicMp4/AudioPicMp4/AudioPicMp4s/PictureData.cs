using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.AudioPicMp4s
{
	/// <summary>
	/// 画像
	/// </summary>
	public class PictureData
	{
		private Canvas2 Canvas;

		public PictureData(Canvas2 canvas)
		{
			if (canvas == null)
				throw new Exception("canvas is null");

			this.Canvas = canvas;
		}
	}
}
