using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.Drawing;
using System.Drawing.Imaging;

namespace Charlotte.AudioPicMP4s
{
	public static class PictureUtils
	{
		public static Canvas2 Blur(Canvas2 canvas, int deep)
		{
			for (; 1 <= deep; deep--)
			{
				canvas = Blur(canvas, 1, 0);
				canvas = Blur(canvas, 0, 1);
			}
			return canvas;
		}

		private static Canvas2 Blur(Canvas2 canvas, int xa, int ya)
		{
			Canvas2 dest = new Canvas2(canvas.GetWidth(), canvas.GetHeight());

			using (Graphics g = dest.GetGraphics())
			{
				g.DrawImage(canvas.GetImage(), -xa, -ya);
				g.DrawImage(
					canvas.GetImage(),
					new Rectangle(0, 0, canvas.GetWidth(), canvas.GetHeight()),
					0, 0, canvas.GetWidth(), canvas.GetHeight(),
					GraphicsUnit.Pixel,
					GetIA_Alpha(0.5)
					);
				g.DrawImage(
					canvas.GetImage(),
					new Rectangle(xa, ya, canvas.GetWidth(), canvas.GetHeight()),
					0, 0, canvas.GetWidth(), canvas.GetHeight(),
					GraphicsUnit.Pixel,
					GetIA_Alpha(1 / 3.0)
					);
			}
			return dest;
		}

		private static ImageAttributes GetIA_Alpha(double a)
		{
			ColorMatrix cm = new ColorMatrix();
			cm.Matrix00 = 1F;
			cm.Matrix11 = 1F;
			cm.Matrix22 = 1F;
			cm.Matrix33 = (float)a;
			cm.Matrix44 = 1F;
			ImageAttributes ia = new ImageAttributes();
			ia.SetColorMatrix(cm);
			return ia;
		}

		public static void Filter_Color(Canvas2 canvas, Color color, double rate)
		{
			Canvas2 maskImg = new Canvas2(canvas.GetWidth(), canvas.GetHeight());

			maskImg.AntiAliasing = false;

			using (Graphics g = maskImg.GetGraphics())
			{
				g.FillRectangle(new SolidBrush(color), 0, 0, maskImg.GetWidth(), maskImg.GetHeight());
			}
			using (Graphics g = canvas.GetGraphics())
			{
				g.DrawImage(
					maskImg.GetImage(),
					new Rectangle(0, 0, canvas.GetWidth(), canvas.GetHeight()),
					0, 0, canvas.GetWidth(), canvas.GetHeight(),
					GraphicsUnit.Pixel,
					GetIA_Alpha(rate)
					);
			}
		}
	}
}
