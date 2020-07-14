using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Common
{
	// memo: ifscene は yield retrun で代用出来そうなので実装しない。--> DDCommonEffect.GetTask()

	public struct DDScene
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public int Numer;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public int Denom;

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public DDScene(int numer, int denom)
		{
			this.Numer = numer;
			this.Denom = denom;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public double Rate
		{
			get
			{
				return this.Numer / (double)this.Denom;
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public int Remaining
		{
			get
			{
				return this.Denom - this.Numer;
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public double RemainingRate
		{
			get
			{
				return this.Remaining / (double)this.Denom;
			}
		}
	}
}
