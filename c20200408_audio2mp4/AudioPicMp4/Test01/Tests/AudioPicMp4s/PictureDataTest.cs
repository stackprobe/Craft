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

			const int D_MAX = 10;
			int dTarg = D_MAX;
			int d = D_MAX;

			for (int frameCount = 0; frameCount < frameNum; frameCount++)
			{
				picture.SetFrame(frameCount * 1.0 / frameNum);

				// ---- darkness ---

				if (frameCount == 10)
					dTarg = 0;
				else if (frameCount + 10 + D_MAX == frameNum)
					dTarg = D_MAX;

				if (d < dTarg)
					d++;
				else if (dTarg < d)
					d--;

				if (0 < d)
					picture.SetDrakness(d * 1.0 / D_MAX);

				// ----

				picture.Save(string.Format(@"C:\temp\{0:D3}.png", frameCount));
			}
		}
	}
}
