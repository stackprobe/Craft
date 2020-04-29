using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;

namespace Charlotte.AudioPicMP4s
{
	/// <summary>
	/// 映像(映像のみ、音声無し)
	/// </summary>
	public class VideoData
	{
		public const int FPS = 20;

		private PictureData MainPicture;
		private int FrameNum;
		private string WDir;

		public VideoData(PictureData mainPicture, int frameNum, string wDir)
		{
			if (mainPicture == null)
				throw new Exception("mainPicture is null");

			if (frameNum < 1 || IntTools.IMAX < frameNum)
				throw new Exception("Bad frameNum: " + frameNum);

			if (string.IsNullOrEmpty(wDir) || Directory.Exists(wDir) == false)
				throw new Exception("Bad wDir: " + wDir);

			this.MainPicture = mainPicture;
			this.FrameNum = frameNum;
			this.WDir = wDir;
		}

		public class FadeInOutInfo
		{
			public int StartMargin;
			public int EndMargin; // -1 == 無効
			public int FadeInOutSpan;

			// <---- prm

			public int Max;
			public int Target;
			public int Level;

			public void Reset()
			{
				this.Max = this.FadeInOutSpan;
				this.Target = this.Max;
				this.Level = this.Max;
			}

			public void EachFrame(int frame, int frameNum, PictureData picture)
			{
				if (frame == this.StartMargin)
					this.Target = 0;
				else if (this.EndMargin != -1 && frame == frameNum - this.EndMargin - this.FadeInOutSpan)
					this.Target = this.Max;

				if (this.Level < this.Target)
					this.Level++;
				else if (this.Target < this.Level)
					this.Level--;

				if (0 < this.Level)
					picture.SetDrakness(this.Level * 1.0 / this.Max);
			}
		}

		public void MakeImages(FadeInOutInfo fadeInOut, SpectrumEffectData se, FadeInOutInfo seFadeInOut)
		{
			fadeInOut.Reset();
			seFadeInOut.Reset();

			for (int frame = 0; frame < this.FrameNum; frame++)
			{
				this.MainPicture.SetFrame(frame * 1.0 / (this.FrameNum - 1));
				fadeInOut.EachFrame(frame, this.FrameNum, this.MainPicture);
				se.DrawTo(this.MainPicture, frame * 1.0 / FPS);
				seFadeInOut.EachFrame(frame, this.FrameNum, this.MainPicture);
				this.MainPicture.Save(this.GetImageFile(frame));
			}
		}

		public string GetImageFile(int frame)
		{
			return Path.Combine(this.WDir, frame + ".jpg");
		}
	}
}
