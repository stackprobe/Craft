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
	public class DDCommonEffect
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public List<DDPicture> Pictures = new List<DDPicture>();
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public int FramePerPicture = 1;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public double X = DDConsts.Screen_W / 2.0;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public double Y = DDConsts.Screen_H / 2.0;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public double R = 0.0;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public double Z = 1.0;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public double A = 1.0;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public double XAdd = 0.0;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public double YAdd = 0.0;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public double RAdd = 0.0;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public double ZAdd = 0.0;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public double AAdd = 0.0;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public double XAdd2 = 0.0;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public double YAdd2 = 0.0;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public double RAdd2 = 0.0;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public double ZAdd2 = 0.0;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public double AAdd2 = 0.0;

		// <---- prm

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public DDCommonEffect()
		{ }

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public DDCommonEffect(DDPicture picture)
		{
			this.Pictures.Add(picture);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private IEnumerable<bool> GetTaskSequence()
		{
			if (this.Pictures.Count == 0) // ? 画像が追加されていない。
				throw new DDError();

			int outOfCameraFrame = 0;

			for (int frame = 0; ; frame++)
			{
				double drawX = this.X - DDGround.ICamera.X;
				double drawY = this.Y - DDGround.ICamera.Y;

				DDDraw.SetAlpha(this.A);
				DDDraw.DrawBegin(this.Pictures[(frame / this.FramePerPicture) % this.Pictures.Count], drawX, drawY);
				DDDraw.DrawRotate(this.R);
				DDDraw.DrawZoom(this.Z);
				DDDraw.DrawEnd();

				this.X += this.XAdd;
				this.Y += this.YAdd;
				this.R += this.RAdd;
				this.Z += this.ZAdd;
				this.A += this.AAdd;

				this.XAdd += this.XAdd2;
				this.YAdd += this.YAdd2;
				this.RAdd += this.RAdd2;
				this.ZAdd += this.ZAdd2;
				this.AAdd += this.AAdd2;

				if (DDUtils.IsOutOfScreen(new D2Point(drawX, drawY)))
				//if (DDUtils.IsOutOfCamera(new D2Point(this.X, this.Y)))
				{
					outOfCameraFrame++;

					if (20 < outOfCameraFrame)
						break;
				}
				else
					outOfCameraFrame = 0;

				if (this.A < 0.0)
					break;

				yield return true;
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public Func<bool> GetTask()
		{
			return EnumerableTools.Supplier(this.GetTaskSequence());
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void Fire()
		{
			this.Fire(DDGround.EL);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void Fire(DDTaskList tl)
		{
			tl.Add(this.GetTask());
		}
	}
}
