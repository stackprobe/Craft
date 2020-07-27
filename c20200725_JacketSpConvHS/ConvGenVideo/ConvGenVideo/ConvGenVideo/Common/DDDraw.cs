using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DxLibDLL;
using Charlotte.Tools;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public static class DDDraw
	{
		// Extra >

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private class ExtraInfo
		{
			public DDTaskList TL = null;
			public bool BlendInv = false;
			public bool Mosaic = false;
			public bool IntPos = false;
			public bool IgnoreError = false;
			public int A = -1; // -1 == 無効
			public int BlendAdd = -1; // -1 == 無効
			public I3Color Bright = new I3Color(-1, 0, 0);
		};

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static ExtraInfo Extra = new ExtraInfo();

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Reset()
		{
			Extra = new ExtraInfo();
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void SetTaskList(DDTaskList tl)
		{
			Extra.TL = tl;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void SetBlendInv()
		{
			Extra.BlendInv = true;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void SetMosaic()
		{
			Extra.Mosaic = true;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void SetIntPos()
		{
			Extra.IntPos = true;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void SetIgnoreError()
		{
			Extra.IgnoreError = true;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void SetAlpha(double a)
		{
			int pal = DoubleTools.ToInt(a * 255.0);

			pal = IntTools.ToRange(pal, 0, 255);

			Extra.A = pal;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void SetBlendAdd(double a)
		{
			int pal = DoubleTools.ToInt(a * 255.0);

			pal = IntTools.ToRange(pal, 0, 255);

			Extra.BlendAdd = pal;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void SetBright(double r, double g, double b)
		{
			int pR = DoubleTools.ToInt(r * 255.0);
			int pG = DoubleTools.ToInt(g * 255.0);
			int pB = DoubleTools.ToInt(b * 255.0);

			pR = IntTools.ToRange(pR, 0, 255);
			pG = IntTools.ToRange(pG, 0, 255);
			pB = IntTools.ToRange(pB, 0, 255);

			Extra.Bright = new I3Color(pR, pG, pB);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void SetBright(I3Color color)
		{
			color.R = IntTools.ToRange(color.R, 0, 255);
			color.G = IntTools.ToRange(color.G, 0, 255);
			color.B = IntTools.ToRange(color.B, 0, 255);

			Extra.Bright = color;
		}

		// < Extra

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private interface ILayoutInfo
		{ }

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private class FreeInfo : ILayoutInfo
		{
			public double LTX;
			public double LTY;
			public double RTX;
			public double RTY;
			public double RBX;
			public double RBY;
			public double LBX;
			public double LBY;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private class RectInfo : ILayoutInfo
		{
			public double L;
			public double T;
			public double R;
			public double B;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private class SimpleInfo : ILayoutInfo
		{
			public double X;
			public double Y;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private class DrawInfo
		{
			public DDPicture Picture;
			public ILayoutInfo Layout;
			public ExtraInfo Extra;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static void SetBlend(int mode, int pal)
		{
			if (DX.SetDrawBlendMode(mode, pal) != 0) // ? 失敗
				throw new DDError();
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static void ResetBlend()
		{
			if (DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0) != 0) // ? 失敗
				throw new DDError();
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static void SetBright(int r, int g, int b)
		{
			if (DX.SetDrawBright(r, g, b) != 0) // ? 失敗
				throw new DDError();
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static void ResetBright()
		{
			if (DX.SetDrawBright(255, 255, 255) != 0) // ? 失敗
				throw new DDError();
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static void DrawPicMain(DrawInfo info)
		{
			// app > @ enter DrawPicMain

			// < app

			if (info.Extra.A != -1)
			{
				SetBlend(DX.DX_BLENDMODE_ALPHA, info.Extra.A);
			}
			else if (info.Extra.BlendAdd != -1)
			{
				SetBlend(DX.DX_BLENDMODE_ADD, info.Extra.BlendAdd);
			}
			else if (info.Extra.BlendInv)
			{
				SetBlend(DX.DX_BLENDMODE_INVSRC, 255);
			}

			if (info.Extra.Mosaic)
			{
				DX.SetDrawMode(DX.DX_DRAWMODE_NEAREST);
			}
			if (info.Extra.Bright.R != -1)
			{
				SetBright(info.Extra.Bright.R, info.Extra.Bright.G, info.Extra.Bright.B);
			}

			{
				FreeInfo u = info.Layout as FreeInfo;

				if (u != null)
				{
					if (
						info.Extra.IntPos ?
						DX.DrawModiGraph(
							DoubleTools.ToInt(u.LTX),
							DoubleTools.ToInt(u.LTY),
							DoubleTools.ToInt(u.RTX),
							DoubleTools.ToInt(u.RTY),
							DoubleTools.ToInt(u.RBX),
							DoubleTools.ToInt(u.RBY),
							DoubleTools.ToInt(u.LBX),
							DoubleTools.ToInt(u.LBY),
							info.Picture.GetHandle(),
							1
							)
							!= 0
						:
						DX.DrawModiGraphF(
							(float)u.LTX,
							(float)u.LTY,
							(float)u.RTX,
							(float)u.RTY,
							(float)u.RBX,
							(float)u.RBY,
							(float)u.LBX,
							(float)u.LBY,
							info.Picture.GetHandle(),
							1
							)
							!= 0
						)
					// ? 失敗
					{
						if (info.Extra.IgnoreError == false)
							throw new DDError();
					}
					goto endDraw;
				}
			}

			{
				RectInfo u = info.Layout as RectInfo;

				if (u != null)
				{
					if (
						info.Extra.IntPos ?
						DX.DrawExtendGraph(
							DoubleTools.ToInt(u.L),
							DoubleTools.ToInt(u.T),
							DoubleTools.ToInt(u.R),
							DoubleTools.ToInt(u.B),
							info.Picture.GetHandle(),
							1
							)
							!= 0
						:
						DX.DrawExtendGraphF(
							(float)u.L,
							(float)u.T,
							(float)u.R,
							(float)u.B,
							info.Picture.GetHandle(),
							1
							)
							!= 0
						)
					// ? 失敗
					{
						if (info.Extra.IgnoreError == false)
							throw new DDError();
					}
					goto endDraw;
				}
			}

			{
				SimpleInfo u = info.Layout as SimpleInfo;

				if (u != null)
				{
					if (
						info.Extra.IntPos ?
						DX.DrawGraph(
							DoubleTools.ToInt(u.X),
							DoubleTools.ToInt(u.Y),
							info.Picture.GetHandle(),
							1
							)
							!= 0
						:
						DX.DrawGraphF(
							(float)u.X,
							(float)u.Y,
							info.Picture.GetHandle(),
							1
							)
							!= 0
						)
					// ? 失敗
					{
						if (info.Extra.IgnoreError == false)
							throw new DDError();
					}
					goto endDraw;
				}
			}

			throw new DDError(); // ? 不明なレイアウト
		endDraw:

			if (info.Extra.A != -1 || info.Extra.BlendAdd != -1 || info.Extra.BlendInv)
			{
				ResetBlend();
			}
			if (info.Extra.Mosaic)
			{
				DX.SetDrawMode(DX.DX_DRAWMODE_BILINEAR);
			}
			if (info.Extra.Bright.R != -1)
			{
				ResetBright();
			}

			// app > @ leave DrawPicMain

			// < app
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static void DrawPic(DDPicture picture, ILayoutInfo layout_binding)
		{
			DrawInfo info = new DrawInfo()
			{
				Picture = picture,
				Layout = layout_binding,
				Extra = Extra,
			};

			if (Extra.TL == null)
			{
				DrawPicMain(info);
			}
			else
			{
				Extra.TL.Add(() =>
				{
					DrawPicMain(info);
					return false;
				});
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawFree(DDPicture picture, double ltx, double lty, double rtx, double rty, double rbx, double rby, double lbx, double lby)
		{
			FreeInfo u = new FreeInfo()
			{
				LTX = ltx,
				LTY = lty,
				RTX = rtx,
				RTY = rty,
				RBX = rbx,
				RBY = rby,
				LBX = lbx,
				LBY = lby,
			};

			DrawPic(picture, u);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawFree(DDPicture picture, D2Point lt, D2Point rt, D2Point rb, D2Point lb)
		{
			DrawFree(picture, lt.X, lt.Y, rt.X, rt.Y, rb.X, rb.Y, lb.X, lb.Y);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawFree(DDPicture picture, P4Poly poly)
		{
			DrawFree(picture, poly.LT, poly.RT, poly.RB, poly.LB);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawRect_LTRB(DDPicture picture, double l, double t, double r, double b)
		{
			if (
				l < -(double)IntTools.IMAX || (double)IntTools.IMAX - 1.0 < l ||
				t < -(double)IntTools.IMAX || (double)IntTools.IMAX - 1.0 < t ||
				r < l + 1.0 || (double)IntTools.IMAX < r ||
				b < t + 1.0 || (double)IntTools.IMAX < b
				)
				throw new DDError();


			RectInfo u = new RectInfo()
			{
				L = l,
				T = t,
				R = r,
				B = b,
			};

			DrawPic(picture, u);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawRect(DDPicture picture, double l, double t, double w, double h)
		{
			DrawRect_LTRB(picture, l, t, l + w, t + h);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawRect(DDPicture picture, D4Rect rect)
		{
			DrawRect(picture, rect.L, rect.T, rect.W, rect.H);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawSimple(DDPicture picture, double x, double y)
		{
			if (
				x < -(double)IntTools.IMAX || (double)IntTools.IMAX < x ||
				y < -(double)IntTools.IMAX || (double)IntTools.IMAX < y
				)
				throw new DDError();

			SimpleInfo u = new SimpleInfo()
			{
				X = x,
				Y = y,
			};

			DrawPic(picture, u);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawCenter(DDPicture picture, double x, double y)
		{
			if (
				x < -(double)IntTools.IMAX || (double)IntTools.IMAX < x ||
				y < -(double)IntTools.IMAX || (double)IntTools.IMAX < y
				)
				throw new DDError();

			DrawBegin(picture, x, y);
			DrawEnd();
		}

		// DrawBegin ～ DrawEnd >

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private class DBInfo
		{
			public DDPicture Picture;
			public double X;
			public double Y;
			public FreeInfo Layout;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static DBInfo DB = null;

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawBeginRect_LTRB(DDPicture picture, double l, double t, double r, double b)
		{
			DrawBeginRect(picture, l, t, r - l, b - t);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawBeginRect(DDPicture picture, double l, double t, double w, double h)
		{
			DrawBegin(picture, l + w / 2.0, t + h / 2.0);
			DrawSetSize(w, h);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawBegin(DDPicture picture, double x, double y)
		{
			if (DB != null)
				throw new DDError();

			double w = picture.Get_W();
			double h = picture.Get_H();

			w /= 2.0;
			h /= 2.0;

			DB = new DBInfo()
			{
				Picture = picture,
				X = x,
				Y = y,
				Layout = new FreeInfo()
				{
					LTX = -w,
					LTY = -h,
					RTX = w,
					RTY = -h,
					RBX = w,
					RBY = h,
					LBX = -w,
					LBY = h,
				},
			};
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawSlide(double x, double y)
		{
			if (DB == null)
				throw new DDError();

			DB.Layout.LTX += x;
			DB.Layout.LTY += y;
			DB.Layout.RTX += x;
			DB.Layout.RTY += y;
			DB.Layout.RBX += x;
			DB.Layout.RBY += y;
			DB.Layout.LBX += x;
			DB.Layout.LBY += y;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawRotate(double rot)
		{
			if (DB == null)
				throw new DDError();

			DDUtils.Rotate(ref DB.Layout.LTX, ref DB.Layout.LTY, rot);
			DDUtils.Rotate(ref DB.Layout.RTX, ref DB.Layout.RTY, rot);
			DDUtils.Rotate(ref DB.Layout.RBX, ref DB.Layout.RBY, rot);
			DDUtils.Rotate(ref DB.Layout.LBX, ref DB.Layout.LBY, rot);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawZoom_X(double z)
		{
			if (DB == null)
				throw new DDError();

			DB.Layout.LTX *= z;
			DB.Layout.RTX *= z;
			DB.Layout.RBX *= z;
			DB.Layout.LBX *= z;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawZoom_Y(double z)
		{
			if (DB == null)
				throw new DDError();

			DB.Layout.LTY *= z;
			DB.Layout.RTY *= z;
			DB.Layout.RBY *= z;
			DB.Layout.LBY *= z;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawZoom(double z)
		{
			DrawZoom_X(z);
			DrawZoom_Y(z);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawSetSize_W(double w)
		{
			if (DB == null)
				throw new DDError();

			w /= 2.0;

			DB.Layout.LTX = -w;
			DB.Layout.RTX = w;
			DB.Layout.RBX = w;
			DB.Layout.LBX = -w;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawSetSize_H(double h)
		{
			if (DB == null)
				throw new DDError();

			h /= 2.0;

			DB.Layout.LTY = -h;
			DB.Layout.RTY = -h;
			DB.Layout.RBY = h;
			DB.Layout.LBY = h;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawSetSize(double w, double h)
		{
			DrawSetSize_W(w);
			DrawSetSize_H(h);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawEnd()
		{
			if (DB == null)
				throw new DDError();

			DB.Layout.LTX += DB.X;
			DB.Layout.LTY += DB.Y;
			DB.Layout.RTX += DB.X;
			DB.Layout.RTY += DB.Y;
			DB.Layout.RBX += DB.X;
			DB.Layout.RBY += DB.Y;
			DB.Layout.LBX += DB.X;
			DB.Layout.LBY += DB.Y;

			DrawPic(DB.Picture, DB.Layout);
			DB = null;
		}

		// < DrawBegin ～ DrawEnd
	}
}
