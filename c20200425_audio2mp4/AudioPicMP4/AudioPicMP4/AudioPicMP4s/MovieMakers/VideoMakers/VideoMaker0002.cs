using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.AudioPicMP4s.MovieMakers.VideoMakers
{
	public class VideoMaker0002 : IVideoMaker
	{
		private AbstractVideoImageMaker[] VideoImageMakers;

		public VideoMaker0002(params AbstractVideoImageMaker[] videoImageMakers_binding)
		{
			this.VideoImageMakers = videoImageMakers_binding;
		}

		public void MakeVideo(WaveData wave, WorkingDir wd, Action<Canvas2> addImage)
		{
			int frameNum = DoubleTools.ToInt((wave.Length * 1.0 / wave.WavHz) * AudioPicMP4Props.FPS);

			foreach (AbstractVideoImageMaker videoImageMaker in this.VideoImageMakers)
			{
				videoImageMaker.FrameNum = frameNum;
				videoImageMaker.Wave = wave;
				videoImageMaker.WD = wd;
			}
			for (int frame = 0; frame < frameNum; frame++)
			{
				Canvas2 frameImg = new Canvas2(AudioPicMP4Props.VIDEO_W, AudioPicMP4Props.VIDEO_H);

				foreach (AbstractVideoImageMaker videoImageMaker in this.VideoImageMakers)
				{
					videoImageMaker.FrameImg = frameImg;
					videoImageMaker.Frame = frame;
					videoImageMaker.Rate = frame * 1.0 / (frameNum - 1);
					videoImageMaker.InvRate = 1.0 - videoImageMaker.Rate;

					{
						Canvas2 currFrameImg = videoImageMaker.GetImage();

						if (currFrameImg != null)
						{
							PictureUtils.Paste(frameImg, currFrameImg);
						}
					}

					videoImageMaker.FrameImg = null;
					videoImageMaker.Frame = -1;
					videoImageMaker.Rate = -1.0;
					videoImageMaker.InvRate = -1.0;
				}
				addImage(frameImg);
			}
			foreach (AbstractVideoImageMaker videoImageMaker in this.VideoImageMakers)
			{
				videoImageMaker.FrameNum = -1;
				videoImageMaker.Wave = null;
				videoImageMaker.WD = null;
			}
		}
	}
}
