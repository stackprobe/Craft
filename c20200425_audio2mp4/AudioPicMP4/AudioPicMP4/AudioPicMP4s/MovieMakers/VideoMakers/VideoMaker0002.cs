using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.AudioPicMP4s.MovieMakers.VideoMakers
{
	public class VideoMaker0002 : IVideoMaker
	{
		private IVideoImageMaker VideoImageMaker;

		public VideoMaker0002(IVideoImageMaker videoImageMaker)
		{
			this.VideoImageMaker = videoImageMaker;
		}

		public void MakeVideo(WaveData wave, WorkingDir wd, Action<Canvas2> addImage)
		{
			int frameNum = DoubleTools.ToInt((wave.Length * 1.0 / wave.WavHz) * AudioPicMP4Props.FPS);

			for (int frame = 0; frame < frameNum; frame++)
			{
				Canvas2 frameImg = new Canvas2(AudioPicMP4Props.VIDEO_W, AudioPicMP4Props.VIDEO_H);

				// TODO

				addImage(frameImg);
			}
		}
	}
}
