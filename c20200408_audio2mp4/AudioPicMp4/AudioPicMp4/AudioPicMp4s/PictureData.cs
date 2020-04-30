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
		private const int BLUR_DEPTH = 5;
		private const double WALL_DARKNESS_RATE = 0.5;
		private const double R1 = 0.2;
		private const double R2 = 0.1;
		private const int JPEG_QUARITY = 90;

		private Canvas2 DiscJacket;
		private Canvas2 BluredDiscJacket;
		private Canvas2 MarginedDiscJacket;
		private Canvas2 Frame;

		public PictureData(Canvas2 discJacket, int frame_w, int frame_h)
		{
			if (discJacket == null)
				throw new Exception("discJacket is null");

			this.DiscJacket = discJacket;
			this.BluredDiscJacket = PictureUtils.Blur(discJacket, BLUR_DEPTH);
			PictureUtils.Filter_Color(this.BluredDiscJacket, Color.Black, WALL_DARKNESS_RATE);
			this.MarginedDiscJacket = PictureUtils.PutMargin(discJacket, discJacket.GetWidth(), discJacket.GetHeight());
			this.Frame = new Canvas2(frame_w, frame_h);
		}

		public void SetFrame(double rate)
		{
			double invRate = 1.0 - rate;

			{
				double w = this.Frame.GetWidth();
				double h = this.BluredDiscJacket.GetHeight() * this.Frame.GetWidth() * 1.0 / this.BluredDiscJacket.GetWidth();

				if (h < this.Frame.GetHeight()) // ? はみ出さない -> はみ出すようにする。
				{
					w = this.BluredDiscJacket.GetWidth() * this.Frame.GetHeight() * 1.0 / this.BluredDiscJacket.GetHeight();
					h = this.Frame.GetHeight();
				}
				w += w * R1 * rate;
				h += h * R1 * rate;

				double l = (this.Frame.GetWidth() - w) / 2.0;
				double t = (this.Frame.GetHeight() - h) / 2.0;

				//ProcMain.WriteLog("LTWH_B: " + string.Join(", ", l, t, w, h)); // test

				using (Graphics g = this.Frame.GetGraphics())
				{
					g.DrawImage(this.BluredDiscJacket.GetImage(), (float)l, (float)t, (float)w, (float)h);
				}
			}

			{
				double w = this.Frame.GetWidth();
				double h = this.DiscJacket.GetHeight() * this.Frame.GetWidth() * 1.0 / this.DiscJacket.GetWidth();

				if (this.Frame.GetHeight() < h) // ? はみ出す -> はみ出さないようにする。
				{
					w = this.DiscJacket.GetWidth() * this.Frame.GetHeight() * 1.0 / this.DiscJacket.GetHeight();
					h = this.Frame.GetHeight();
				}
				w *= this.MarginedDiscJacket.GetWidth() * 1.0 / this.DiscJacket.GetWidth();
				h *= this.MarginedDiscJacket.GetHeight() * 1.0 / this.DiscJacket.GetHeight();

				w += w * R2 * invRate;
				h += h * R2 * invRate;

				double l = (this.Frame.GetWidth() - w) / 2.0;
				double t = (this.Frame.GetHeight() - h) / 2.0;

				//ProcMain.WriteLog("LTWH_F: " + string.Join(", ", l, t, w, h)); // test

				using (Graphics g = this.Frame.GetGraphics())
				{
					g.DrawImage(this.MarginedDiscJacket.GetImage(), (float)l, (float)t, (float)w, (float)h);
				}
			}
		}

		public void SetDrakness(double rate)
		{
			PictureUtils.Filter_Color(this.Frame, Color.Black, rate);
		}

		public void Save(string file)
		{
			this.Frame.Save(file, ImageFormat.Jpeg, JPEG_QUARITY);
		}

		public Canvas2 GetFrame()
		{
			return this.Frame;
		}
	}
}
