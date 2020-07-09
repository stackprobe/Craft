using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.MovieGens.MGEngines;

namespace Charlotte.MovieGens
{
	public class MovieGen0001
	{
		private SpectrumData SpData;
		private int Frame = 0;

		private void MG_EachFrame()
		{
			// 60hz -> 20hz

			DDEngine.EachFrame(); // 1
			DDEngine.EachFrame(); // 2
			DDEngine.EachFrame(); // 3

			this.Frame++;
		}

		public void Main01()
		{
			this.SpData = new SpectrumData(@"C:\temp\a0001\Spectrum.csv");

			foreach (DDScene scene in DDSceneUtils.Create(20))
			{
				// TODO

				this.MG_EachFrame();
			}
			foreach (DDScene scene in DDSceneUtils.Create(20))
			{
				// TODO

				this.MG_EachFrame();
			}
			foreach (DDScene scene in DDSceneUtils.Create(20))
			{
				// TODO

				this.MG_EachFrame();
			}
			while (this.Frame < this.SpData.Rows.Length)
			{
				// TODO

				this.MG_EachFrame();
			}
		}
	}
}
