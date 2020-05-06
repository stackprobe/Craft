using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;
using System.Text.RegularExpressions;

namespace Charlotte.AudioPicMP4s
{
	public class FFmpegAudio : IDisposable
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
		private string AudioFile = null;

		private FFmpegAudioInfo AudioInfo = null;

		public void PutAudioFile(string file)
		{
			string ext = Path.GetExtension(file);

			if (Regex.IsMatch(ext, @"^\.[0-9A-Za-z]+$") == false)
				throw new Exception("Bad (audio) ext: " + ext);

			if (this.AudioFile != null)
			{
				FileTools.Delete(this.AudioFile);
				this.AudioFile = null;
				this.AudioInfo = null;
			}

			{
				string wFile = this.WD.GetPath("audio" + ext);

				File.Copy(file, wFile);

				this.AudioFile = wFile;
			}
		}

		public string GetAudioFile()
		{
			if (this.AudioFile == null)
				throw new Exception("AudioFile is null");

			return this.AudioFile;
		}

		public FFmpegAudioInfo GetAudioInfo()
		{
			if (this.AudioInfo == null)
				this.AudioInfo = new FFmpegAudioInfo(this.GetAudioFile());

			return this.AudioInfo;
		}
	}
}
