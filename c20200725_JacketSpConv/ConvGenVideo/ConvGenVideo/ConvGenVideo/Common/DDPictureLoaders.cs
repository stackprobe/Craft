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
	/// <summary>
	/// Unload()する必要あり。
	/// 必要なし -> DDPictureLoader2
	/// </summary>
	public static class DDPictureLoaders
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static DDPicture Standard(string file)
		{
			return new DDPicture(
				() => DDPictureLoaderUtils.GraphicHandle2Info(DDPictureLoaderUtils.SoftImage2GraphicHandle(DDPictureLoaderUtils.FileData2SoftImage(DDPictureLoaderUtils.File2FileData(file)))),
				DDPictureLoaderUtils.ReleaseInfo,
				DDPictureUtils.Add
				);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static DDPicture Inverse(string file)
		{
			return new DDPicture(
				() =>
				{
					int siHandle = DDPictureLoaderUtils.FileData2SoftImage(DDPictureLoaderUtils.File2FileData(file));
					int w;
					int h;

					DDPictureLoaderUtils.GetSoftImageSize(siHandle, out w, out h);

					for (int x = 0; x < w; x++)
					{
						for (int y = 0; y < h; y++)
						{
							DDPictureLoaderUtils.Dot dot = DDPictureLoaderUtils.GetSoftImageDot(siHandle, x, y);

							dot.R ^= 0xff;
							dot.G ^= 0xff;
							dot.B ^= 0xff;

							DDPictureLoaderUtils.SetSoftImageDot(siHandle, x, y, dot);
						}
					}
					return DDPictureLoaderUtils.GraphicHandle2Info(DDPictureLoaderUtils.SoftImage2GraphicHandle(siHandle));
				},
				DDPictureLoaderUtils.ReleaseInfo,
				DDPictureUtils.Add
				);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static DDPicture Mirror(string file)
		{
			return new DDPicture(
				() =>
				{
					int siHandle = DDPictureLoaderUtils.FileData2SoftImage(DDPictureLoaderUtils.File2FileData(file));
					int w;
					int h;

					DDPictureLoaderUtils.GetSoftImageSize(siHandle, out w, out h);

					{
						int h2 = DDPictureLoaderUtils.CreateSoftImage(w * 2, h);

						for (int x = 0; x < w; x++)
						{
							for (int y = 0; y < h; y++)
							{
								DDPictureLoaderUtils.Dot dot = DDPictureLoaderUtils.GetSoftImageDot(siHandle, x, y);

								DDPictureLoaderUtils.SetSoftImageDot(h2, x, y, dot);
								DDPictureLoaderUtils.SetSoftImageDot(h2, w * 2 - 1 - x, y, dot);
							}
						}
						DDPictureLoaderUtils.ReleaseSoftImage(siHandle);
						siHandle = h2;
					}

					return DDPictureLoaderUtils.GraphicHandle2Info(DDPictureLoaderUtils.SoftImage2GraphicHandle(siHandle));
				},
				DDPictureLoaderUtils.ReleaseInfo,
				DDPictureUtils.Add
				);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static DDPicture BgTrans(string file)
		{
			return new DDPicture(
				() =>
				{
					int siHandle = DDPictureLoaderUtils.FileData2SoftImage(DDPictureLoaderUtils.File2FileData(file));
					int w;
					int h;

					DDPictureLoaderUtils.GetSoftImageSize(siHandle, out w, out h);

					DDPictureLoaderUtils.Dot targetDot = DDPictureLoaderUtils.GetSoftImageDot(siHandle, 0, 0); // 左上隅のピクセル

					for (int x = 0; x < w; x++)
					{
						for (int y = 0; y < h; y++)
						{
							DDPictureLoaderUtils.Dot dot = DDPictureLoaderUtils.GetSoftImageDot(siHandle, x, y);

							if (
								targetDot.R == dot.R &&
								targetDot.G == dot.G &&
								targetDot.B == dot.B
								)
							{
								dot.A = 0;

								DDPictureLoaderUtils.SetSoftImageDot(siHandle, x, y, dot);
							}
						}
					}
					return DDPictureLoaderUtils.GraphicHandle2Info(DDPictureLoaderUtils.SoftImage2GraphicHandle(siHandle));
				},
				DDPictureLoaderUtils.ReleaseInfo,
				DDPictureUtils.Add
				);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static DDPicture SelectARGB(string file, string mode) // mode: "XXXX", X == "ARGB"
		{
			const string s_argb = "ARGB";

			int ia = s_argb.IndexOf(mode[0]);
			int ir = s_argb.IndexOf(mode[1]);
			int ig = s_argb.IndexOf(mode[2]);
			int ib = s_argb.IndexOf(mode[3]);

			return new DDPicture(
				() =>
				{
					int siHandle = DDPictureLoaderUtils.FileData2SoftImage(DDPictureLoaderUtils.File2FileData(file));
					int w;
					int h;

					DDPictureLoaderUtils.GetSoftImageSize(siHandle, out w, out h);

					for (int x = 0; x < w; x++)
					{
						for (int y = 0; y < h; y++)
						{
							DDPictureLoaderUtils.Dot dot = DDPictureLoaderUtils.GetSoftImageDot(siHandle, x, y);

							List<int> argb = new int[]
							{
								dot.A,
								dot.R,
								dot.G,
								dot.B,
							}
							.ToList();

							dot.A = argb[ia];
							dot.R = argb[ir];
							dot.G = argb[ig];
							dot.B = argb[ib];

							DDPictureLoaderUtils.SetSoftImageDot(siHandle, x, y, dot);
						}
					}
					return DDPictureLoaderUtils.GraphicHandle2Info(DDPictureLoaderUtils.SoftImage2GraphicHandle(siHandle));
				},
				DDPictureLoaderUtils.ReleaseInfo,
				DDPictureUtils.Add
				);
		}

		// 新しい画像ローダーをここへ追加...
	}
}
