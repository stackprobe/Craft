using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;

namespace Charlotte.SpectrumGens.Engines
{
	public static class MP3Conv
	{
		public static void MP3FileToWavFile(string rMP3File, string wWavFile)
		{
			if (string.IsNullOrEmpty(rMP3File))
				throw null;

			if (string.IsNullOrEmpty(wWavFile))
				throw null;

			FileTools.Delete(wWavFile);

			if (File.Exists(rMP3File) == false)
				throw null;

			if (File.Exists(wWavFile))
				throw null;

			using (WorkingDir wd = new WorkingDir())
			{
				File.Copy(rMP3File, wd.GetPath("1.mp3"));

				ProcessTools.Batch(new string[]
				{
					Consts.FFMPEG_FILE + " -i 1.mp3 2.wav",
				},
				wd.GetPath(".")
				);

				if (File.Exists(wd.GetPath("2.wav")) == false)
					throw new Exception("変換に失敗しました。");

				File.Copy(wd.GetPath("2.wav"), wWavFile);

				if (File.Exists(wWavFile) == false)
					throw new Exception("コピーに失敗しました。");
			}
		}
	}
}
