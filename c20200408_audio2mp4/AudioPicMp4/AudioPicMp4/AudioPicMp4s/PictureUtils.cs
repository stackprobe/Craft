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
		public static Canvas2 Blur(Canvas2 canvas, int depth)
		{
			for (int count = 0; count < depth; count++)
				canvas = BlurOnce(canvas, count);

			return canvas;
		}

		private static readonly int[,] DIRECTIONS = new int[,]
		{
			{ -1, -1 },
			{ -1, 0 },
			{ -1, 1 },
			{ 0, 1 },
			{ 1, 1 },
			{ 1, 0 },
			{ 1, -1 },
			{ 0, -1 },
		};

		private static Canvas2 BlurOnce(Canvas2 canvas, int phase)
		{
			Canvas2 dest = new Canvas2(canvas.GetWidth(), canvas.GetHeight());

			using (Graphics g = dest.GetGraphics())
			{
				g.DrawImage(canvas.GetImage(), 0, 0);

				for (int count = 0; count < 8; count++)
				{
					int direction = (count + phase) % 8;

					int xa = DIRECTIONS[direction, 0];
					int ya = DIRECTIONS[direction, 1];

					g.DrawImage(
						canvas.GetImage(),
						new Rectangle(xa, ya, canvas.GetWidth(), canvas.GetHeight()),
						0, 0, canvas.GetWidth(), canvas.GetHeight(),
						GraphicsUnit.Pixel,
						GetIA_Alpha(1.0 / (2.0 + count))
						);
				}
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

		public static void Filter_Color(Canvas2 canvas, Color color, double a)
		{
			Canvas2 maskImg = new Canvas2(canvas.GetWidth(), canvas.GetHeight());

			using (Graphics g = maskImg.GetGraphics(false))
			{
				g.FillRectangle(new SolidBrush(color), 0, 0, maskImg.GetWidth(), maskImg.GetHeight());
			}
			using (Graphics g = canvas.GetGraphics())
			{
				g.DrawImage(
					maskImg.GetImage(),
					new Rectangle(0, 0, canvas.GetWidth(), canvas.GetHeight()),
					0, 0, maskImg.GetWidth(), maskImg.GetHeight(),
					GraphicsUnit.Pixel,
					GetIA_Alpha(a)
					);
			}
		}

		public static Canvas2 PutMargin(Canvas2 canvas, int xMargin, int yMargin)
		{
			Canvas2 dest = new Canvas2(canvas.GetWidth() + xMargin * 2, canvas.GetHeight() + yMargin * 2);

			using (Graphics g = dest.GetGraphics(false))
			{
				g.FillRectangle(new SolidBrush(Color.Transparent), 0, 0, dest.GetWidth(), dest.GetHeight());
				g.DrawImage(canvas.GetImage(), xMargin, yMargin, canvas.GetWidth(), canvas.GetHeight());
			}
			return dest;
		}
	}
}
