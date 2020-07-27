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
	public static class DDUtils
	{
		// DX.* >

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static bool IsWindowActive()
		{
			return DX.GetActiveFlag() != 0;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static long GetCurrTime()
		{
			return DX.GetNowHiPerformanceCount() / 1000L;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static bool GetMouseDispMode()
		{
			return DX.GetMouseDispFlag() != 0;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void SetMouseDispMode(bool mode)
		{
			DX.SetMouseDispFlag(mode ? 1 : 0);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static uint GetColor(I3Color color)
		{
			return DX.GetColor(color.R, color.G, color.B);
		}

		// < DX.*

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static byte[] SplitableJoin(string[] lines)
		{
			return BinTools.SplittableJoin(lines.Select(line => Encoding.UTF8.GetBytes(line)).ToArray());
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static string[] Split(byte[] data)
		{
			return BinTools.Split(data).Select(bLine => Encoding.UTF8.GetString(bLine)).ToArray();
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Noop(params object[] dummyPrms)
		{
			// noop
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static T FastDesertElement<T>(List<T> list, Predicate<T> match, T defval = default(T))
		{
			for (int index = 0; index < list.Count; index++)
				if (match(list[index]))
					return ExtraTools.FastDesertElement(list, index);

			return defval;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static bool CountDown(ref int count)
		{
			if (count < 0)
				count++;
			else if (0 < count)
				count--;
			else
				return false;

			return true;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Approach(ref double value, double target, double rate)
		{
			value -= target;
			value *= rate;
			value += target;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Range(ref double value, double minval, double maxval)
		{
			value = DoubleTools.ToRange(value, minval, maxval);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Range(ref int value, int minval, int maxval)
		{
			value = IntTools.ToRange(value, minval, maxval);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Minim(ref double value, double minval)
		{
			value = Math.Min(value, minval);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Minim(ref int value, int minval)
		{
			value = Math.Min(value, minval);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Maxim(ref double value, double minval)
		{
			value = Math.Max(value, minval);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Maxim(ref int value, int minval)
		{
			value = Math.Max(value, minval);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Rotate(ref double x, ref double y, double rot)
		{
			double w;

			w = x * Math.Cos(rot) - y * Math.Sin(rot);
			y = x * Math.Sin(rot) + y * Math.Cos(rot);
			x = w;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static double GetDistance(double x, double y)
		{
			return Math.Sqrt(x * x + y * y);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static double GetDistance(D2Point pt)
		{
			return GetDistance(pt.X, pt.Y);
		}

		// ret:
		// 0.0 ～ Math.PI * 2.0
		// 右真横(0,0 -> 1,0方向)を0.0として時計回り。(但し、X軸プラス方向を右、Y軸プラス方向を下)
		// 1周は Math.PI * 2.0
		//
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static double GetAngle(double x, double y)
		{
			if (y < 0.0) return Math.PI * 2.0 - GetAngle(x, -y);
			if (x < 0.0) return Math.PI - GetAngle(-x, y);

			if (x <= 0.0) return Math.PI / 2.0;
			if (y <= 0.0) return 0.0;

			double r1 = 0.0;
			double r2 = Math.PI / 2.0;
			double t = y / x;
			double rm;

			for (int c = 1; ; c++)
			{
				rm = (r1 + r2) / 2.0;

				if (10 <= c)
					break;

				double rmt = Math.Tan(rm);

				if (t < rmt)
					r2 = rm;
				else
					r1 = rm;
			}
			return rm;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static double GetAngle(D2Point pt)
		{
			return GetDistance(pt.X, pt.Y);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static D2Point AngleToPoint(double angle, double distance)
		{
			return new D2Point(
				distance * Math.Cos(angle),
				distance * Math.Sin(angle)
				);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		/// <summary>
		/// 円1と円2が衝突しているか判定する。
		/// </summary>
		/// <param name="pt1">円1の中心</param>
		/// <param name="r1">円1の半径</param>
		/// <param name="pt2">円2の中心</param>
		/// <param name="r2">円2の半径</param>
		/// <returns>衝突しているか</returns>
		public static bool IsCrashed_Circle_Circle(D2Point pt1, double r1, D2Point pt2, double r2)
		{
			return GetDistance(pt1 - pt2) < r1 + r2;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		/// <summary>
		/// 円1と点2が衝突しているか判定する。
		/// </summary>
		/// <param name="pt1">円1の中心</param>
		/// <param name="r1">円1の半径</param>
		/// <param name="pt2">点2の座標</param>
		/// <returns>衝突しているか</returns>
		public static bool IsCrashed_Circle_Point(D2Point pt1, double r1, D2Point pt2)
		{
			return GetDistance(pt1 - pt2) < r1;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		/// <summary>
		/// 円1と矩形2が衝突しているか判定する。
		/// </summary>
		/// <param name="pt1">円1の中心</param>
		/// <param name="r1">円1の半径</param>
		/// <param name="rect2">矩形2の座標</param>
		/// <returns>衝突しているか</returns>
		public static bool IsCrashed_Circle_Rect(D2Point pt1, double r1, D4Rect rect2)
		{
			if (pt1.X < rect2.L) // 左
			{
				if (pt1.Y < rect2.T) // 左上
				{
					return IsCrashed_Circle_Point(pt1, r1, new D2Point(rect2.L, rect2.T));
				}
				else if (rect2.B < pt1.Y) // 左下
				{
					return IsCrashed_Circle_Point(pt1, r1, new D2Point(rect2.L, rect2.B));
				}
				else // 左中段
				{
					return rect2.L < pt1.X + r1;
				}
			}
			else if (rect2.R < pt1.X) // 右
			{
				if (pt1.Y < rect2.T) // 右上
				{
					return IsCrashed_Circle_Point(pt1, r1, new D2Point(rect2.R, rect2.T));
				}
				else if (rect2.B < pt1.Y) // 右下
				{
					return IsCrashed_Circle_Point(pt1, r1, new D2Point(rect2.R, rect2.B));
				}
				else // 右中段
				{
					return pt1.X - r1 < rect2.R;
				}
			}
			else // 真上・真ん中・真下
			{
				return rect2.T - r1 < pt1.Y && pt1.Y < rect2.B + r1;
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		/// <summary>
		/// 矩形1と点2が衝突しているか判定する。
		/// </summary>
		/// <param name="rect1">矩形1の座標</param>
		/// <param name="pt2">点2の座標</param>
		/// <returns>衝突しているか</returns>
		public static bool IsCrashed_Rect_Point(D4Rect rect1, D2Point pt2)
		{
			return
				rect1.L < pt2.X && pt2.X < rect1.R &&
				rect1.T < pt2.Y && pt2.Y < rect1.B;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		/// <summary>
		/// 矩形1と矩形2が衝突しているか判定する。
		/// </summary>
		/// <param name="rect1">矩形1の座標</param>
		/// <param name="rect2">矩形2の座標</param>
		/// <returns>衝突しているか</returns>
		public static bool IsCrashed_Rect_Rect(D4Rect rect1, D4Rect rect2)
		{
			return
				rect1.L < rect2.R && rect2.L < rect1.R &&
				rect1.T < rect2.B && rect2.T < rect1.B;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static bool IsOut(D2Point pt, D4Rect rect, double margin = 0.0)
		{
			return
				pt.X < rect.L - margin || rect.R + margin < pt.X ||
				pt.Y < rect.T - margin || rect.B + margin < pt.Y;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static bool IsOutOfScreen(D2Point pt, double margin = 0.0)
		{
			return IsOut(pt, new D4Rect(0, 0, DDConsts.Screen_W, DDConsts.Screen_H), margin);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static bool IsOutOfCamera(D2Point pt, double margin = 0.0)
		{
			return IsOut(pt, new D4Rect(DDGround.ICamera.X, DDGround.ICamera.Y, DDConsts.Screen_W, DDConsts.Screen_H), margin);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void UpdateInput(ref int counter, bool status)
		{
			if (status)
			{
				counter = Math.Max(0, counter);
				counter++;
			}
			else
				counter = 0 < counter ? -1 : 0;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private const int POUND_FIRST_DELAY = 17;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private const int POUND_DELAY = 4;

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static bool IsPound(int counter)
		{
			return counter == 1 || POUND_FIRST_DELAY < counter && (counter - POUND_FIRST_DELAY) % POUND_DELAY == 1;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static DDRandom Random = new DDRandom();
	}
}
