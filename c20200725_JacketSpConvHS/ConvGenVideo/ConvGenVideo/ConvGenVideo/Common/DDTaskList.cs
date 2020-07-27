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
	public class DDTaskList
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private DDList<Func<bool>> Tasks = new DDList<Func<bool>>();

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void Add(Func<bool> task)
		{
			this.Tasks.Add(task);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void ExecuteAllTask()
		{
			for (int index = 0; index < this.Tasks.Count; index++)
			{
				if (this.Tasks[index]() == false) // ? 終了
				{
					this.Tasks[index] = null;
				}
			}
			this.Tasks.RemoveAll(task => task == null);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void Clear()
		{
			this.Tasks.Clear();
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public int Count
		{
			get
			{
				return this.Tasks.Count;
			}
		}
	}
}
