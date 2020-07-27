using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using DxLibDLL;
using System.Drawing;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public class DDSubScreen : IDisposable
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private int W; // -1 == Disposed
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private int H;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private bool AFlag;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private int Handle = -1; // -1 == Unloaded

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public DDSubScreen(int w, int h, bool aFlag = false)
		{
			this.W = w;
			this.H = h;
			this.AFlag = aFlag;
			this.Handle = -1;

			DDSubScreenUtils.Add(this);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void Dispose()
		{
			if (this.W == -1) // ? Already disposed
				return;

			DDSubScreenUtils.Remove(this);

			this.Unload();

			this.W = -1;
			this.H = -1;
			this.AFlag = false;

			this.Picture = null;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void Unload()
		{
			if (this.Handle != -1)
			{
				if (DX.DeleteGraph(this.Handle) != 0) // ? 失敗
					throw new DDError();

				this.Handle = -1;
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public int GetHandle()
		{
			if (this.Handle == -1)
			{
				this.Handle = DX.MakeScreen(this.W, this.H, this.AFlag ? 1 : 0);

				if (this.Handle == -1) // ? 失敗
					throw new DDError();
			}
			return this.Handle;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void ChangeDrawScreen()
		{
			DDSubScreenUtils.ChangeDrawScreen(this);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public I2Size GetSize()
		{
			return new I2Size(this.W, this.H);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private DDPicture Picture = null;

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public DDPicture ToPicture()
		{
			if (this.Picture == null)
				this.Picture = DDPictureLoaders2.Wrapper(this);

			return this.Picture;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public IDisposable Section()
		{
			this.ChangeDrawScreen();
			return new AnonyDisposable(() => DDSubScreenUtils.RestoreDrawScreen());
		}
	}
}
