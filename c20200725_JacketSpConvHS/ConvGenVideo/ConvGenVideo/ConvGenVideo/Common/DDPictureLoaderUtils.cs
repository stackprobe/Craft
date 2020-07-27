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
	public static class DDPictureLoaderUtils
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static byte[] File2FileData(string file)
		{
			return DDResource.Load(file);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static int FileData2SoftImage(byte[] fileData)
		{
			int siHandle = -1;

			DDSystem.PinOn(fileData, p => siHandle = DX.LoadSoftImageToMem(p, fileData.Length));

			if (siHandle == -1)
				throw new DDError();

			int w;
			int h;

			GetSoftImageSize(siHandle, out w, out h);

			// RGB -> RGBA
			{
				int h2 = DX.MakeARGB8ColorSoftImage(w, h);

				if (h2 == -1) // ? 失敗
					throw new DDError();

				if (DX.BltSoftImage(0, 0, w, h, siHandle, 0, 0, h2) != 0) // ? 失敗
					throw new DDError();

				if (DX.DeleteSoftImage(siHandle) != 0) // ? 失敗
					throw new DDError();

				siHandle = h2;
			}

			return siHandle;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static int SoftImage2GraphicHandle(int siHandle_binding)
		{
			int gHandle = DX.CreateGraphFromSoftImage(siHandle_binding);

			if (gHandle == -1) // ? 失敗
				throw new DDError();

			if (DX.DeleteSoftImage(siHandle_binding) != 0) // ? 失敗
				throw new DDError();

			return gHandle;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static DDPicture.PictureInfo GraphicHandle2Info(int gHandle_binding)
		{
			int w;
			int h;

			GetGraphicHandleSize(gHandle_binding, out w, out h);

			return new DDPicture.PictureInfo()
			{
				Handle = gHandle_binding,
				W = w,
				H = h,
			};
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void GetSoftImageSize(int siHandle, out int w, out int h)
		{
			if (DX.GetSoftImageSize(siHandle, out w, out h) != 0)
				throw new DDError();

			if (
				w < 1 || IntTools.IMAX < w ||
				h < 1 || IntTools.IMAX < h
				)
				throw new DDError();
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void GetGraphicHandleSize(int gHandle, out int w, out int h)
		{
			if (DX.GetGraphSize(gHandle, out w, out h) != 0)
				throw new DDError();

			if (
				w < 1 || IntTools.IMAX < w ||
				h < 1 || IntTools.IMAX < h
				)
				throw new DDError();
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public class Dot
		{
			public int R;
			public int G;
			public int B;
			public int A;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static Dot GetSoftImageDot(int siHandle, int x, int y)
		{
			Dot dot = new Dot();

			if (DX.GetPixelSoftImage(siHandle, x, y, out dot.R, out dot.G, out dot.B, out dot.A) != 0)
				throw new DDError();

			if (
				dot.R < 0 || 255 < dot.R ||
				dot.G < 0 || 255 < dot.G ||
				dot.B < 0 || 255 < dot.B ||
				dot.A < 0 || 255 < dot.A
				)
				throw new DDError();

			return dot;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void SetSoftImageDot(int siHandle, int x, int y, Dot dot)
		{
			dot.R = IntTools.ToRange(dot.R, 0, 255);
			dot.G = IntTools.ToRange(dot.G, 0, 255);
			dot.B = IntTools.ToRange(dot.B, 0, 255);
			dot.A = IntTools.ToRange(dot.A, 0, 255);

			if (DX.DrawPixelSoftImage(siHandle, x, y, dot.R, dot.G, dot.B, dot.A) != 0)
				throw new DDError();
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static int CreateSoftImage(int w, int h)
		{
			if (
				w < 1 || IntTools.IMAX < w ||
				h < 1 || IntTools.IMAX < h
				)
				throw new DDError();

			int siHandle = DX.MakeARGB8ColorSoftImage(w, h);

			if (siHandle == -1) // ? 失敗
				throw new DDError();

			return siHandle;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void ReleaseSoftImage(int siHandle)
		{
			if (DX.DeleteSoftImage(siHandle) != 0) // ? 失敗
				throw new DDError();
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void ReleaseGraphicHandle(int gHandle)
		{
			if (DX.DeleteGraph(gHandle) != 0) // ? 失敗
				throw new DDError();
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void ReleaseInfo(DDPicture.PictureInfo info)
		{
			ReleaseGraphicHandle(info.Handle);
		}
	}
}
