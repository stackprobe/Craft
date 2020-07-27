using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;

namespace Charlotte.Tests
{
	public class Test0001
	{
		public void Test01()
		{
			DDCurtain.SetCurtain();
			DDEngine.FreezeInput();

			foreach (DDScene scene in DDSceneUtils.Create(600))
			{
				if (DDInput.A.GetInput() == 1)
				{
					break;
				}

				DDCurtain.DrawCurtain();

				DDDraw.DrawBegin(DDGround.GeneralResource.Dummy, DDConsts.Screen_W / 2.0, DDConsts.Screen_H / 2.0);
				DDDraw.DrawZoom(5.0);
				DDDraw.DrawRotate(scene.Rate * 5.0);
				DDDraw.DrawEnd();

				DDEngine.EachFrame();
			}

			DDEngine.FreezeInput();
		}
	}
}
