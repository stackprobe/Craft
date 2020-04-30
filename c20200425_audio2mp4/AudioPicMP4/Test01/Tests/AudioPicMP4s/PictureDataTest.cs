using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.AudioPicMP4s;
using Charlotte.Tools;
using System.IO;

namespace Charlotte.Tests.AudioPicMP4s
{
	public class PictureDataTest
	{
		public void Test01()
		{
			PictureData picture = new PictureData(new Canvas2(@"C:\wb2\20191204_ジャケット的な\バンドリ_イニシャル.jpg"), 1920, 1080);

			const int FRAME_NUM = 200;

			const int D_MAX = 20;
			int dTarg = D_MAX;
			int d = D_MAX;

			using (WorkingDir wd = new WorkingDir())
			{
				FileTools.CreateDir(wd.GetPath("img"));

				for (int frame = 0; frame < FRAME_NUM; frame++)
				{
					Console.WriteLine(frame); // test

					picture.SetFrame(frame * 1.0 / FRAME_NUM);

					// ---- 暗くする ----

					if (frame == 10)
						dTarg = 0;
					else if (frame + 10 + D_MAX == FRAME_NUM)
						dTarg = D_MAX;

					if (d < dTarg)
						d++;
					else if (dTarg < d)
						d--;

					if (0 < d)
						picture.SetDrakness(d * 1.0 / D_MAX);

					// ----

					picture.Save(wd.GetPath("img\\" + frame + ".jpg"));
				}

				ProcessTools.Batch(new string[]
				{
					@"C:\app\ffmpeg-4.1.3-win64-shared\bin\ffmpeg.exe -r 20 -i %%d.jpg ..\out.mp4",
				},
				wd.GetPath("img")
				);

				File.Copy(wd.GetPath("out.mp4"), @"C:\temp\1.mp4", true);
			}
		}
	}
}
