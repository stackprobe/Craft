using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public class DDGeneralResource
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public DDPicture Dummy = DDPictureLoaders.Standard(@"General\Dummy.png");
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public DDPicture WhiteBox = DDPictureLoaders.Standard(@"General\WhiteBox.png");
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public DDPicture WhiteCircle = DDPictureLoaders.Standard(@"General\WhiteCircle.png");

		// 新しいリソースをここへ追加...
	}
}
