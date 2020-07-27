using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public class DDMusic
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public DDSound Sound;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public double Volume = 0.5; // 0.0 ～ 1.0

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public DDMusic(string file)
			: this(new DDSound(file, 1))
		{ }

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public DDMusic(Func<byte[]> getFileData)
			: this(new DDSound(getFileData, 1))
		{ }

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public DDMusic(DDSound sound_binding)
		{
			this.Sound = sound_binding;
			this.Sound.PostLoaded = () => DDSoundUtils.SetVolume(this.Sound.GetHandle(0), 0.0); // ロードしたらミュートしておく。

			DDMusicUtils.Add(this);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void Play(bool once = false, bool resume = false, double volume = 1.0, int fadeFrameMax = 30)
		{
			DDMusicUtils.Play(this, once, resume, volume, fadeFrameMax);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void Touch()
		{
			this.Sound.GetHandle(0);
		}
	}
}
