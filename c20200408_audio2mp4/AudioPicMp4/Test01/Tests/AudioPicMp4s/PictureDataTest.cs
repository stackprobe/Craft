using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.AudioPicMP4s;
using Charlotte.Tools;

namespace Charlotte.Tests.AudioPicMP4s
{
	public class PictureDataTest
	{
		public void Test01()
		{
			PictureData picture = new PictureData(new Canvas2(@"C:\wb2\20191204_ジャケット的な\バンドリ_イニシャル.jpg"), 1920, 1080);
			const int frameNum = 200;

			for (int frameCount = 0; frameCount < frameNum; frameCount++)
			{
				picture.SetFrame(frameCount * 1.0 / frameNum);

				// TODO picture.SetDrakness();

				picture.Save(string.Format(@"C:\temp\{0:D3}.png", frameCount));
			}
		}
	}
}
