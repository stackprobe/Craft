using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Tools;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public class DDSound
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private Func<byte[]> Func_GetFileData;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private int HandleCount;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private int[] Handles = null; // null == Unloaded

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public Action PostLoaded = () => { };

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public DDSound(string file, int handleCount)
			: this(() => DDResource.Load(file), handleCount)
		{ }

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public DDSound(Func<byte[]> getFileData, int handleCount)
		{
			this.Func_GetFileData = getFileData;
			this.HandleCount = handleCount;

			DDSoundUtils.Add(this);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void Unload()
		{
			if (this.Handles != null)
			{
				foreach (int handle in this.Handles)
					if (DX.DeleteSoundMem(handle) != 0) // ? 失敗
						throw new DDError();

				this.Handles = null;
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public bool IsLoaded()
		{
			return this.Handles != null;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public int GetHandle(int index)
		{
			if (this.Handles == null)
			{
				this.Handles = new int[this.HandleCount];

				{
					byte[] fileData = this.Func_GetFileData();
					int handle = -1;

					DDSystem.PinOn(fileData, p => handle = DX.LoadSoundMemByMemImage(p, fileData.Length));

					if (handle == -1) // ? 失敗
						throw new DDError();

					this.Handles[0] = handle;
				}

				for (int ndx = 1; ndx < this.HandleCount; ndx++)
				{
					int handle = DX.DuplicateSoundMem(this.Handles[0]);

					if (handle == -1) // ? 失敗
						throw new DDError();

					this.Handles[ndx] = handle;
				}

				this.PostLoaded();
			}
			return this.Handles[index];
		}
	}
}
