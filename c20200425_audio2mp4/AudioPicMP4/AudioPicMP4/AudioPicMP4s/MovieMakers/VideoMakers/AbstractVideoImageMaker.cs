using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.AudioPicMP4s.MovieMakers.VideoMakers
{
	public abstract class AbstractVideoImageMaker
	{
		public int FrameNum;
		public WaveData Wave;
		public WorkingDir WD;

		// <---- 最初に GetImage() を呼び出す前に設定される。

		public Canvas2 FrameImg;
		public int Frame;
		public double Rate;
		public double InvRate;

		// <---- GetImage() を呼び出す前に設定される。

		/// <summary>
		/// フレーム数と同じ回数呼び出される。
		/// </summary>
		/// <returns>フレーム画像に上書きするイメージ, null == 不要</returns>
		public abstract Canvas2 GetImage();
	}
}
