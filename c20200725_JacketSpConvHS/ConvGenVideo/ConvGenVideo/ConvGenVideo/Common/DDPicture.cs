using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public class DDPicture
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public class PictureInfo
		{
			public int Handle;
			public int W;
			public int H;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private PictureInfo Info = null; // null == Unloaded
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private Func<PictureInfo> Loader;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private Action<PictureInfo> Unloader;

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public DDPicture(Func<PictureInfo> loader, Action<PictureInfo> unloader, Action<DDPicture> adder)
		{
			this.Loader = loader;
			this.Unloader = unloader;

			adder(this);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void Unload()
		{
			// この画像を参照している derivation を先に Unload しなければならない。

			if (this.Info != null)
			{
				this.Unloader(this.Info);
				this.Info = null;
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		protected virtual PictureInfo GetInfo()
		{
			if (this.Info == null)
				this.Info = this.Loader();

			return this.Info;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public int GetHandle()
		{
			return this.GetInfo().Handle;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public int Get_W()
		{
			return this.GetInfo().W;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public int Get_H()
		{
			return this.GetInfo().H;
		}
	}
}
