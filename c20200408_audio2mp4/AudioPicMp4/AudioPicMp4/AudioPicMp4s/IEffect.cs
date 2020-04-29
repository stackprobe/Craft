using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Charlotte.AudioPicMP4s
{
	/// <summary>
	/// 映像に重ねるスペクトラム効果
	/// </summary>
	public interface IEffect
	{
		void DrawTo(PictureData picture, int frame, int frameNum);
	}
}
