using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;

namespace Charlotte.AudioPicMP4s
{
	public static class FFmpegConv
	{
		public static void MakeWavFile(string movieOrAudioFile, string destWavFile)
		{
			FileTools.Delete(destWavFile);

			if (Path.GetExtension(movieOrAudioFile).ToLower() == ".wav")
			{
				File.Copy(movieOrAudioFile, destWavFile);
			}
			else
			{
				using (FFmpegMedia audio = new FFmpegMedia())
				using (WorkingDir wd = new WorkingDir())
				{
					audio.PutAudioFile(movieOrAudioFile);

					ProcessTools.Batch(new string[]
				{
					// ステレオにする。
					string.Format(@"{0}ffmpeg.exe -i {1} -map 0:{2} -ac 2 out.wav",
						AudioPicMP4Props.FFmpegPathBase,
						audio.GetFile(),
						audio.GetInfo().GetAudioStreamIndex()
						),
				},
					wd.GetPath(".")
					);

					{
						string midFile = wd.GetPath("out.wav");

						if (File.Exists(midFile) == false)
							throw new Exception();

						File.Copy(midFile, destWavFile);
					}
				}
			}
		}
	}
}
