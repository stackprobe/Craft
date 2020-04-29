using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Charlotte.AudioPicMP4s
{
	/// <summary>
	/// 映像に重ねるスペクトラム効果
	/// </summary>
	public class SpectrumEffectData
	{
		public void DrawTo(PictureData picture, double sec)
		{
			// test
			{
				using (Graphics g = Graphics.FromImage(picture.GetFrame().GetImage()))
				{
					g.FillRectangle(new SolidBrush(Color.FromArgb(128, 0, 0, 0)), 50, 50, 100, 100);
					g.FillRectangle(new SolidBrush(Color.White), 60, 60, 80, 80);
				}
			}
		}
	}
}
