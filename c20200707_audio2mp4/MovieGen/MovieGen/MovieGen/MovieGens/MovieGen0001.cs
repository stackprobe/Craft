using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.MovieGens.MGEngines;
using Charlotte.Tools;
using DxLibDLL;
using System.IO;

namespace Charlotte.MovieGens
{
	public class MovieGen0001
	{
		private const string R_DIR = @"C:\temp\a1001";
		private const string W_DIR = @"C:\temp\a2001";

		private SpectrumData SpData;
		private int Frame = 0;

		private void MG_EachFrame()
		{
#if !true
			DX.SaveDrawScreen(0, 0, DDConsts.Screen_W, DDConsts.Screen_H, Path.Combine(W_DIR, string.Format("{0}.bmp", this.Frame)));

			DDEngine.EachFrame();
#else
			// 60hz -> 20hz

			DDEngine.EachFrame(); // 1
			DDEngine.EachFrame(); // 2
			DDEngine.EachFrame(); // 3
#endif

			this.Frame++;
		}

		public void Main01()
		{
			this.SpData = new SpectrumData(Path.Combine(R_DIR, "Spectrum.csv"));

			while (this.Frame < this.SpData.Rows.Length)
			{
				double[] row = this.SpData.Rows[this.Frame];

				DDCurtain.DrawCurtain();
				DDPrint.SetPrint(0, 20, 23);

				for (int index = 0; index < 45; index++)
				{
					double lv = 0.0;

					for (int c = 0; c < 2; c++)
						lv += row[index * 2 + c];

					int iLv = DoubleTools.ToInt(lv * 145.0);

					for (int c = 0; c < iLv; c++)
						DDPrint.Print("*");

					DDPrint.PrintRet();
				}

				this.MG_EachFrame();
			}
		}

		public void Main02()
		{
			this.SpData = new SpectrumData(Path.Combine(R_DIR, "Spectrum.csv"));

			foreach (DDScene scene in DDSceneUtils.Create(20))
			{
				// TODO ???

				this.MG_EachFrame();
			}
		}

		public void Perform()
		{
			FileTools.Delete(W_DIR);
			FileTools.CreateDir(W_DIR);

			// ---- choose one !

			//this.Main01();
			this.Main02();

			// ----
		}
	}
}
