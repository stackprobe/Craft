using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using Charlotte.AudioPicMP4s.Internal;
using Charlotte.AudioPicMP4s.Internal.SpectrumGraphs;

namespace Charlotte.AudioPicMP4s.MovieMakers
{
	public class MovieMaker0001
	{
		public string DiscJacketFile;
		public string SourceMediaFile;
		public string DestMP4File;
		public bool MasterFlag = false;

		// <---- prm

		private WorkingDir WD;
		private FFmpeg FFmpeg;

		public void Perform()
		{
			this.WD = new WorkingDir();
			try
			{
				this.FFmpeg = new FFmpeg();
				try
				{
					this.FFmpeg.Audio = new FFmpegMedia();
					try
					{
						this.Perform2();
					}
					finally
					{
						this.FFmpeg.Audio.Dispose();
						this.FFmpeg.Audio = null;
					}
				}
				finally
				{
					this.FFmpeg.Dispose();
					this.FFmpeg = null;
				}
			}
			finally
			{
				this.WD.Dispose();
				this.WD = null;
			}
		}

		private const int VIDEO_W = 1920;
		private const int VIDEO_H = 1080;
		private const double AUDIO_DELAY_SEC = 0.2;
		private const int JPEG_QUALITY = 90;

		private Canvas2 DiscJacket;
		private Canvas2 BluredDiscJacket;
		private Canvas2 MarginedDiscJacket;
		private WaveData Wave;

		private void Perform2()
		{
			this.DiscJacket = new Canvas2(this.DiscJacketFile);
			this.BluredDiscJacket = PictureUtils.Blur(this.DiscJacket, 5); // 要調整
			this.MarginedDiscJacket = PictureUtils.PutMargin(this.DiscJacket);

			{
				string wavFile = this.WD.GetPath("audio.wav");
				string masterWavFile = this.WD.GetPath("audio2.wav");

				FFmpegConv.MakeWavFile(this.SourceMediaFile, wavFile);

				if (this.MasterFlag && MasterUtils.Mastering(wavFile, masterWavFile))
				{
					this.Wave = new WaveData(masterWavFile);
					this.FFmpeg.Audio.PutAudioFile(masterWavFile);
				}
				else
				{
					this.Wave = new WaveData(wavFile);
					this.FFmpeg.Audio.PutAudioFile(this.SourceMediaFile);
				}
			}

			ShadowSpectraData ss = new ShadowSpectraData();
			FadeInOutData f1 = new FadeInOutData() { MaxValue = 20 };
			FadeInOutData f2 = new FadeInOutData();
			f1.Rate = 1.0;

			int frameNum = DoubleTools.ToInt((this.Wave.Length * 1.0 / this.Wave.WavHz) * this.FFmpeg.FPS);

			for (int frame = 0; frame < frameNum; frame++)
			{
				if (frame == 2 * this.FFmpeg.FPS)
					f1.TargetRate = 0.0;

				if (frame == frameNum - 10 - f2.MaxValue)
					f2.TargetRate = 1.0;

				f1.Approach();
				f2.Approach();

				double rate = frame * 1.0 / (frameNum - 1);
				double invRate = 1.0 - rate;

				double r1 = 1.0 + 0.2 * rate;
				double r2 = 1.0 + 0.1 * invRate;

				D4Rect wallRect;
				D4Rect frntRect;

				{
					D2Size size = Utils.TangentSize(new D2Size(VIDEO_W, VIDEO_H), new D2Size(this.DiscJacket.GetWidth(), this.DiscJacket.GetHeight()), false);

					size.W *= r1;
					size.H *= r1;

					wallRect = Utils.Centering(new D2Size(VIDEO_W, VIDEO_H), size);
				}

				{
					D2Size size = Utils.TangentSize(new D2Size(VIDEO_W, VIDEO_H), new D2Size(this.DiscJacket.GetWidth(), this.DiscJacket.GetHeight()), true);

					size.W *= r2;
					size.H *= r2;

					// マージン分
					size.W *= this.MarginedDiscJacket.GetWidth() * 1.0 / this.DiscJacket.GetWidth();
					size.H *= this.MarginedDiscJacket.GetHeight() * 1.0 / this.DiscJacket.GetHeight();

					frntRect = Utils.Centering(new D2Size(VIDEO_W, VIDEO_H), size);
				}

				Canvas2 frameImg = new Canvas2(VIDEO_W, VIDEO_H);

				PictureUtils.Paste(frameImg, this.BluredDiscJacket, wallRect);
				PictureUtils.Filter(frameImg, Color.Black, 0.5); // 要調整
				PictureUtils.Paste(frameImg, this.MarginedDiscJacket, frntRect);
				PictureUtils.Filter(frameImg, Color.Black, 0.1); // 要調整

				PictureUtils.Filter(frameImg, Color.Black, f1.Rate); // 背景カーテン

				this.Wave.SetWavPart(DoubleTools.ToInt((frame * 1.0 / this.FFmpeg.FPS + AUDIO_DELAY_SEC) * this.Wave.WavHz));
				SpectrumGraph0001 sg = new SpectrumGraph0001(this.Wave);
				ss.Projection(sg.Spectra);
				DrawSpectra(frameImg, sg, ss);

				PictureUtils.Filter(frameImg, Color.Black, f2.Rate); // 前景カーテン

				frameImg.Save(this.FFmpeg.GetImageFile(frame), ImageFormat.Jpeg, JPEG_QUALITY);

				GC.Collect();
			}

			this.FFmpeg.MakeMovie();

			File.Copy(this.FFmpeg.GetMovieFile(), this.DestMP4File);
		}

		private void DrawSpectra(Canvas2 frameImg, SpectrumGraph0001 sg, ShadowSpectraData ss)
		{
			using (Graphics g = frameImg.GetGraphics(false))
			{
				int dr_l = 10;
				int dr_t = 10;
				int dr_w = frameImg.GetWidth() - 20;
				int dr_h = frameImg.GetHeight() - 20;

				for (int index = 0; index < sg.Spectra.Length; index++)
				{
					int x1 = (((index * 3 + 0) * dr_w) / (sg.Spectra.Length * 3 - 2));
					int x2 = (((index * 3 + 1) * dr_w) / (sg.Spectra.Length * 3 - 2));
					int w = x2 - x1;

					double v1 = ss.ShadowSpectra[index];
					double v2 = sg.Spectra[index];

					v1 /= 2.0; // 要調整
					v2 /= 2.0; // 要調整

					int h1 = DoubleTools.ToInt(v1 * dr_h);
					int h2 = DoubleTools.ToInt(v2 * dr_h);

					int y1 = dr_h - h1;
					int y2 = dr_h - h2;

					g.FillRectangle(new SolidBrush(Color.FromArgb(128, 255, 255, 255)),
						dr_l + x1, dr_t + y1, w, h1);
					g.FillRectangle(new SolidBrush(Color.White),
						dr_l + x1, dr_t + y2, w, h2);
				}
			}
		}
	}
}
