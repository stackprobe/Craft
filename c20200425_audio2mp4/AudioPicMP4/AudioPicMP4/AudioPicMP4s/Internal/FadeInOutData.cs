using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.AudioPicMP4s.Internal
{
	internal class FadeInOutData
	{
		public int MaxValue = 10;

		// <---- prm

		public int TargetValue = 0;
		public int Value = 0;

		public void Approach()
		{
			if (this.Value < this.TargetValue)
				this.Value++;
			else if (this.TargetValue < this.Value)
				this.Value--;
		}

		public double Rate
		{
			get
			{
				return this.Value * 1.0 / this.MaxValue;
			}

			set
			{
				int val = DoubleTools.ToInt(value * this.MaxValue);

				this.TargetValue = val;
				this.Value = val;
			}
		}

		public double TargetRate
		{
			set
			{
				int val = DoubleTools.ToInt(value * this.MaxValue);

				this.TargetValue = val;
			}
		}
	}
}
