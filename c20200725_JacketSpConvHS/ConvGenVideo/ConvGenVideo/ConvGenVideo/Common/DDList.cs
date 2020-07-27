using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public class DDList<T>
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private List<T> Inner = new List<T>();

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public int Count { get; private set; }

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public DDList()
		{
			//this.Count = 0;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void Clear()
		{
			this.Count = 0;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void Add(T element)
		{
			if (this.Count < this.Inner.Count)
				this.Inner[this.Count] = element;
			else
				this.Inner.Add(element);

			this.Count++;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public T this[int index]
		{
			get
			{
				return this.Inner[index];
			}

			set
			{
				this.Inner[index] = value;
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void FastRemoveAt(int index)
		{
			this.Inner[index] = this.Inner[--this.Count];
			this.Inner[this.Count] = default(T);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void RemoveAll(Predicate<T> match)
		{
			for (int r = 0; r < this.Count; r++)
			{
				if (match(this.Inner[r]))
				{
					int w = r;

					while (++r < this.Count)
						if (match(this.Inner[r]) == false)
							this.Inner[w++] = this.Inner[r];

					for (int index = w; index < this.Count; index++)
						this.Inner[index] = default(T);

					this.Count = w;
					return;
				}
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public IEnumerable<T> Iterate()
		{
#if false
			return this.Inner; // 要素が変更・追加されると例外を投げるっぽい。
#else
			for (int index = 0; index < this.Count; index++)
			{
				yield return this.Inner[index];
			}
#endif
		}
	}
}
