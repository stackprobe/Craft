using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;
using System.Text.RegularExpressions;

namespace Charlotte.AudioPicMP4s
{
	public class FFmpegMediaInfo
	{
		private int TotalTimeCentisecond = -1;
		private int AudioStreamIndex = -1; // -1 == no audio stream
		private int VideoStreamIndex = -1; // -1 == no video stream

		public FFmpegMediaInfo(string file)
		{
			string[] lines;

			using (WorkingDir wd = new WorkingDir())
			{
				ProcessTools.Batch(new string[]
				{
					string.Format(@"{0}ffprobe.exe {1} 2> stderr.tmp",
						AudioPicMP4Props.FFmpegPathBase,
						file
						),
				},
				wd.GetPath(".")
				);

				string text = File.ReadAllText(file, Encoding.UTF8);

				lines = FileTools.TextToLines(text);
			}

			foreach (string fLine in lines)
			{
				string line = fLine.Trim();

				if (Regex.IsMatch(line, "^Duration: [0-9]{2}:[0-9]{2}:[0-9]{2}.[0-9]{2},"))
				{
					int h = int.Parse(line.Substring(10, 2));
					int m = int.Parse(line.Substring(13, 2));
					int s = int.Parse(line.Substring(16, 2));
					int c = int.Parse(line.Substring(19, 2));

					int t = h;
					t *= 60;
					t += m;
					t *= 60;
					t += s;
					t *= 100;
					t += c;

					this.TotalTimeCentisecond = t;
				}
				else if (Regex.IsMatch(line, "^Stream.*Audio:"))
				{
					string[] tokens = StringTools.Tokenize(line, StringTools.DECIMAL, true, true);

					this.AudioStreamIndex = int.Parse(tokens[1]);
				}
				else if (Regex.IsMatch(line, "^Stream.*Video:"))
				{
					string[] tokens = StringTools.Tokenize(line, StringTools.DECIMAL, true, true);

					this.VideoStreamIndex = int.Parse(tokens[1]);
				}
			}

			if (this.TotalTimeCentisecond < 0 || IntTools.IMAX < this.TotalTimeCentisecond)
				throw new Exception("Bad TotalTimeCentisecond: " + this.TotalTimeCentisecond);

			if (this.AudioStreamIndex < 0 || IntTools.IMAX < this.AudioStreamIndex)
				this.AudioStreamIndex = -1;

			if (this.VideoStreamIndex < 0 || IntTools.IMAX < this.VideoStreamIndex)
				this.VideoStreamIndex = -1;
		}

		public int GetTotalTileCentisecond()
		{
			return this.TotalTimeCentisecond;
		}

		public bool HasAudioStream()
		{
			return this.AudioStreamIndex != -1;
		}

		public int GetAudioStreamIndex()
		{
			if (this.AudioStreamIndex == -1)
				throw new Exception("no audio stream");

			return this.AudioStreamIndex;
		}

		public bool HasVideoStream()
		{
			return this.VideoStreamIndex != -1;
		}

		public int GetVideoStreamIndex()
		{
			if (this.VideoStreamIndex == -1)
				throw new Exception("no video stream");

			return this.VideoStreamIndex;
		}
	}
}
