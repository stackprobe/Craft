using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.AudioPicMP4s.MovieMakers
{
	public interface IVideoMaker
	{
		void MakeVideo(WaveData wave, WorkingDir wd, Action<Canvas2> addImage);
	}
}
