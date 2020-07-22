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
	public class DDTable<T>
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private T[] Inner;

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public int W { get; private set; }
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public int H { get; private set; }

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public DDTable(int w, int h, Func<int, int, T> getCell)
			: this(w, h)
		{
			this.SetAllCell(getCell);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public DDTable(int w, int h)
		{
			if (
				w < 1 || IntTools.IMAX < w ||
				h < 1 || IntTools.IMAX / w < h
				)
				throw new DDError();

			this.Inner = new T[w * h];
			this.W = w;
			this.H = h;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void SetAllCell(Func<int, int, T> getCell)
		{
			for (int x = 0; x < this.W; x++)
			{
				for (int y = 0; y < this.H; y++)
				{
					this[x, y] = getCell(x, y);
				}
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void GetAllCell(Action<int, int, T> setCell)
		{
			for (int x = 0; x < this.W; x++)
			{
				for (int y = 0; y < this.H; y++)
				{
					setCell(x, y, this[x, y]);
				}
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void ChangeAllCell(Func<int, int, T, T> chgCell)
		{
			for (int x = 0; x < this.W; x++)
			{
				for (int y = 0; y < this.H; y++)
				{
					this[x, y] = chgCell(x, y, this[x, y]);
				}
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public T this[int x, int y]
		{
			get
			{
				return this.Inner[x + y * this.W];
			}

			set
			{
				this.Inner[x + y * this.W] = value;
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public T GetCell(int x, int y, T defval = default(T))
		{
			if (
				x < 0 || this.W <= x ||
				y < 0 || this.H <= y
				)
				return defval;

			return this[x, y];
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public IEnumerable<T> Iterate()
		{
#if true
			return this.Inner; // 要素が変更されても問題無いっぽい。
#else
			for (int index = 0; index < this.Inner.Length; index++)
			{
				yield return this.Inner[index];
			}
#endif
		}
	}
}
