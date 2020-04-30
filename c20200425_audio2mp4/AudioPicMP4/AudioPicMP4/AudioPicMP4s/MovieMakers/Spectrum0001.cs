using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.Drawing;

namespace Charlotte.AudioPicMP4s.MovieMakers
{
	public class Spectrum0001
	{
		private PictureData Picture;
		private WaveData Wave;
		private string DestMP4File;

		public Spectrum0001(PictureData picture, WaveData wave, string destMP4File)
		{
			this.Picture = picture;
			this.Wave = wave;
			this.DestMP4File = destMP4File;
		}

		public void Perform()
		{
			// TODO
		}
	}
}
