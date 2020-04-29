using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.Drawing;

namespace Charlotte.AudioPicMP4s
{
	/// <summary>
	/// 画像
	/// </summary>
	public class PictureData
	{
		private Canvas2 MainImage;
		private Canvas2 Frame;

		public PictureData(Canvas2 mainImage, int frame_w, int frame_h)
		{
			if (mainImage == null)
				throw new Exception("mainImage is null");

			this.MainImage = mainImage;
			this.Frame = new Canvas2(frame_w, frame_h);
		}

		private const int BLUR_DEEP = 30;

		public void SetFrame(double rate)
		{
			double invRate = 1.0 - rate;

			{
				double w = this.Frame.GetWidth();
				double h = this.MainImage.GetHeight() * this.Frame.GetWidth() * 1.0 / this.MainImage.GetWidth();

				if (h < this.Frame.GetHeight()) // ? はみ出さない -> はみ出すようにする。
				{
					w = this.MainImage.GetWidth() * this.Frame.GetHeight() * 1.0 / this.MainImage.GetHeight();
					h = this.Frame.GetHeight();
				}
				w += w * 0.2 * rate;
				h += h * 0.2 * rate;

				double l = (this.Frame.GetWidth() - w) / 2.0;
				double t = (this.Frame.GetHeight() - h) / 2.0;

				using (Graphics g = this.Frame.GetGraphics())
				{
					g.DrawImage(this.MainImage.GetImage(), (float)l, (float)t, (float)w, (float)h);
				}
			}

			this.Frame = PictureUtils.Blur(this.Frame, BLUR_DEEP); // TODO 重い

			{
				double w = this.Frame.GetWidth();
				double h = this.MainImage.GetHeight() * this.Frame.GetWidth() * 1.0 / this.MainImage.GetWidth();

				if (this.Frame.GetHeight() < h) // ? はみ出す -> はみ出さないようにする。
				{
					w = this.MainImage.GetWidth() * this.Frame.GetHeight() * 1.0 / this.MainImage.GetHeight();
					h = this.Frame.GetHeight();
				}
				w += w * 0.1 * invRate;
				h += h * 0.1 * invRate;

				double l = (this.Frame.GetWidth() - w) / 2.0;
				double t = (this.Frame.GetHeight() - h) / 2.0;

				using (Graphics g = this.Frame.GetGraphics())
				{
					g.DrawImage(this.MainImage.GetImage(), (float)l, (float)t, (float)w, (float)h);
				}
			}
		}

		public void SetDrakness(double rate)
		{
			PictureUtils.Filter_Color(this.Frame, Color.Black, rate);
		}

		public void Save(string file)
		{
			this.Frame.Save(file);
		}
	}
}
