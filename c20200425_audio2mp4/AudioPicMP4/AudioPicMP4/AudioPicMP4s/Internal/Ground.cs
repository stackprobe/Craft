using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Charlotte.AudioPicMP4s.Internal
{
	internal class Ground
	{
		private static Ground _i = null;

		public static Ground I
		{
			get
			{
				if (_i == null)
					_i = new Ground();

				return _i;
			}
		}

		public string wavCsvExeFile;
		public string MasterExeFile;

		private Ground()
		{
			this.wavCsvExeFile = Path.Combine(Directory.GetCurrentDirectory(), "wavCsv.exe");

			if (File.Exists(this.wavCsvExeFile) == false)
			{
				this.wavCsvExeFile = @"C:\Factory\SubTools\wavCsv.exe";

				if (File.Exists(this.wavCsvExeFile) == false)
					throw new Exception("no wavCsv.exe");
			}

			this.MasterExeFile = Path.Combine(Directory.GetCurrentDirectory(), "Master.exe");

			if (File.Exists(this.MasterExeFile) == false)
			{
				this.MasterExeFile = @"C:\Factory\Program\WavMaster\Master.exe";

				if (File.Exists(this.MasterExeFile) == false)
					throw new Exception("no Master.exe");
			}
		}
	}
}
