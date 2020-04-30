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
			int w = canvas.GetWidth();
			int h = canvas.GetHeight();

			Canvas2 dest = new Canvas2(w, h);

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
						new Rectangle(xa, ya, w, h),
						0, 0, w, h,
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

		public static void Filter(Canvas2 canvas, Color color, double a)
		{
			int w = canvas.GetWidth();
			int h = canvas.GetHeight();

			Canvas2 mask = new Canvas2(w, h);

			using (Graphics g = mask.GetGraphics(false))
			{
				g.FillRectangle(new SolidBrush(color), 0, 0, w, h);
			}
			using (Graphics g = canvas.GetGraphics())
			{
				g.DrawImage(
					mask.GetImage(),
					new Rectangle(0, 0, w, h),
					0, 0, w, h,
					GraphicsUnit.Pixel,
					GetIA_Alpha(a)
					);
			}
		}

		public static Canvas2 PutMargin(Canvas2 canvas)
		{
			const int MARGIN = 10;

			Canvas2 dest = new Canvas2(canvas.GetWidth() + MARGIN * 2, canvas.GetHeight() + MARGIN * 2);

			using (Graphics g = dest.GetGraphics(false))
			{
				g.FillRectangle(new SolidBrush(Color.Transparent), 0, 0, dest.GetWidth(), dest.GetHeight());
				g.DrawImage(canvas.GetImage(), MARGIN, MARGIN, canvas.GetWidth(), canvas.GetHeight());
			}
			return dest;
		}

		public static Canvas2 Expand(Canvas2 canvas, int w, int h)
		{
			Canvas2 dest = new Canvas2(w, h);

			using (Graphics g = dest.GetGraphics())
			{
				g.DrawImage(canvas.GetImage(), 0, 0, w, h);
			}
			return dest;
		}

		public static void Paste(Canvas2 dest, Canvas2 src, double l, double t, double w, double h)
		{
#if true
			const double MARGIN = 10;

			if (w < src.GetWidth() + MARGIN || h < src.GetHeight() + MARGIN)
				src = Expand(src, (int)(w * 0.99), (int)(h * 0.99));

			using (Graphics g = dest.GetGraphics())
			{
				g.DrawImage(src.GetImage(), (float)l, (float)t, (float)w, (float)h);
			}
#else // 試していない。
			using (Graphics g = dest.GetGraphics())
			{
				g.DrawImage(
					src.GetImage(),
					new PointF[]
					{
						new PointF((float)l, (float)t),
						new PointF((float)(l + w), (float)t),
						new PointF((float)l, (float)(t + h)),
					}
					);
			}
#endif
		}
	}
}
