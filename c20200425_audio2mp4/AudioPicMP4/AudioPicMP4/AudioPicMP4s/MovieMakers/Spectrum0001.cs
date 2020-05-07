using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.Drawing;
using System.IO;

namespace Charlotte.AudioPicMP4s.MovieMakers
{
	public class Spectrum0001
	{
		public Canvas2 DiscJacket;
		public WaveData Wave;
		public string DestMP4File;

		// <---- prm

		private FFmpeg FFmpeg;

		public void Perform()
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

		private void Perform2()
		{

		}
	}
}
