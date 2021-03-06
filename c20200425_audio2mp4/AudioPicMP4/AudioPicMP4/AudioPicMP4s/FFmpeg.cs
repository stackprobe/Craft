﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;
using System.Text.RegularExpressions;
using Charlotte.AudioPicMP4s.Internal;

namespace Charlotte.AudioPicMP4s
{
	public class FFmpeg : IDisposable
	{
		private WorkingDir WD = new WorkingDir();

		public void Dispose()
		{
			if (this.WD != null)
			{
				this.WD.Dispose();
				this.WD = null;
			}
		}

		// ---- Image ----

		public string GetImageFile(int frame)
		{
			return Path.Combine(this.GetImageDir(), frame + ".jpg");
		}

		private string ImageDir = null;

		public string GetImageDir()
		{
			if (this.ImageDir == null)
			{
				string dir = this.WD.GetPath("img");

				FileTools.CreateDir(dir);

				this.ImageDir = dir;
			}
			return this.ImageDir;
		}

		// ---- Audio ----

		public FFmpegMedia Audio = null;

		// ---- Video ----

		public string GetVideoFile()
		{
			return this.WD.GetPath("video.mp4");
		}

		// ---- Movie ----

		public string GetMovieFile()
		{
			return this.WD.GetPath("movie.mp4");
		}

		// ----

		public void MakeMovie()
		{
			if (this.Audio == null)
				throw new Exception("Audio is null");

			FileTools.Delete(this.GetVideoFile());
			FileTools.Delete(this.GetMovieFile());

			ProcessTools.Batch(new string[]
			{
				string.Format(@"{0}ffmpeg.exe -r {1} -i %%d.jpg ..\video.mp4",
					AudioPicMP4Props.FFmpegPathBase,
					AudioPicMP4Props.FPS
					),
			},
			this.GetImageDir()
			);

			if (File.Exists(this.GetVideoFile()) == false)
				throw new Exception();

			ProcessTools.Batch(new string[]
			{
				string.Format(@"{0}ffmpeg.exe -i video.mp4 -i {1} -map 0:0 -map 1:{2} -vcodec copy -codec:a copy movie.mp4",
					AudioPicMP4Props.FFmpegPathBase,
					this.Audio.GetFile(),
					this.Audio.GetInfo().GetAudioStreamIndex()
					),
			},
			this.WD.GetPath(".")
			);

			if (Utils.IsEmptyFile(this.GetMovieFile()))
			{
				FileTools.Delete(this.GetMovieFile());

				ProcessTools.Batch(new string[]
				{
					// -codec:a copy を除去
					string.Format(@"{0}ffmpeg.exe -i video.mp4 -i {1} -map 0:0 -map 1:{2} -vcodec copy movie.mp4",
						AudioPicMP4Props.FFmpegPathBase,
						this.Audio.GetFile(),
						this.Audio.GetInfo().GetAudioStreamIndex()
						),
				},
				this.WD.GetPath(".")
				);

				if (Utils.IsEmptyFile(this.GetMovieFile()))
					throw new Exception();
			}
		}
	}
}
