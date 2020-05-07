using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.AudioPicMP4s.MovieMakers.VideoMakers
{
	public interface IVideoImageMaker
	{
		void DrawImage(Tools.Canvas2 frameImg, WaveData wave, Tools.WorkingDir wd, int frame, int frameNum);
	}
}
