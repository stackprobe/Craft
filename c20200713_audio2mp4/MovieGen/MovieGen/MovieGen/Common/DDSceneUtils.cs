using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Common
{
	// memo: ifscene は yield retrun で代用出来そうなので実装しない。--> DDCommonEffect.GetTask()

	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public static class DDSceneUtils
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static IEnumerable<DDScene> Create(int frameMax)
		{
			for (int frame = 0; frame <= frameMax; frame++)
			{
				yield return new DDScene(frame, frameMax);
			}
		}
	}
}
