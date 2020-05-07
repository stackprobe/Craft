using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;
using System.Text.RegularExpressions;

namespace Charlotte.AudioPicMP4s
{
	public class FFmpegMedia : IDisposable
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

		/// <summary>
		/// フルパス SJIS 空白を含まないこと。
		/// </summary>
		private string MFile = null;

		private FFmpegMediaInfo MInfo = null;

		public void PutAudioFile(string file)
		{
			string ext = Path.GetExtension(file);

			if (Regex.IsMatch(ext, @"^\.[0-9A-Za-z]+$") == false)
				throw new Exception("Bad (audio) ext: " + ext);

			if (this.MFile != null)
			{
				FileTools.Delete(this.MFile);
				this.MFile = null;
				this.MInfo = null;
			}

			{
				string wFile = this.WD.GetPath("audio" + ext);

				File.Copy(file, wFile);

				this.MFile = wFile;
			}
		}

		public string GetFile()
		{
			if (this.MFile == null)
				throw new Exception("MediaFile is null");

			return this.MFile;
		}

		public FFmpegMediaInfo GetInfo()
		{
			if (this.MInfo == null)
				this.MInfo = new FFmpegMediaInfo(this.GetFile());

			return this.MInfo;
		}
	}
}
