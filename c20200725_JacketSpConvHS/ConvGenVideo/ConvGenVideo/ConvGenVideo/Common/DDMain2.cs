using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public static class DDMain2
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Perform(Action routine)
		{
			ExceptionDam.Section(eDam =>
			{
				eDam.Invoke(() =>
				{
					DDMain.GameStart();

					try
					{
						routine();
					}
					catch (DDCoffeeBreak)
					{ }

					DDMain.GameEnd();
				});

				DDMain.GameEnd2(eDam);
			});
		}
	}
}
