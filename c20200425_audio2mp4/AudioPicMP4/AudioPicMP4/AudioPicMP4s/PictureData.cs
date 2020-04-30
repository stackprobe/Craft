using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.Drawing;
using System.Drawing.Imaging;

namespace Charlotte.AudioPicMP4s
{
	/// <summary>
	/// 画像
	/// </summary>
	public class PictureData
	{
		private const int JPEG_QUARITY = 90;

		private Canvas2 Canvas;

		public PictureData(Canvas2 canvas)
		{
			this.Canvas = canvas;
		}

		public void Save(string file)
		{
			this.Canvas.Save(file, ImageFormat.Jpeg, JPEG_QUARITY);
		}
	}
}
