using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using DxLibDLL;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public static class DDDerivations
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static DDPicture GetPicture(DDPicture picture, int l, int t, int w, int h)
		{
			if (
				l < 0 || IntTools.IMAX < l ||
				t < 0 || IntTools.IMAX < t ||
				w < 1 || IntTools.IMAX - l < w ||
				h < 1 || IntTools.IMAX - t < h
				)
				throw new DDError();

			// ? 範囲外
			if (
				picture.Get_W() < l + w ||
				picture.Get_H() < t + h
				)
				throw new DDError();

			return new DDPicture(
				() =>
				{
					int handle = DX.DerivationGraph(l, t, w, h, picture.GetHandle());

					if (handle == -1) // ? 失敗
						throw new DDError();

					return new DDPicture.PictureInfo()
					{
						Handle = handle,
						W = w,
						H = h,
					};
				},
				DDPictureLoaderUtils.ReleaseInfo, // やる事同じなので共用しちゃう。
				DDDerivationUtils.Add
				);
		}
	}
}
