using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Charlotte.AudioPicMp4s.Internal
{
	public class Ground
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
		public string ImgToolsExeFile;

		private Ground()
		{
			this.wavCsvExeFile = Path.Combine(Directory.GetCurrentDirectory(), "wavCsv.exe");

			if (File.Exists(this.wavCsvExeFile) == false)
			{
				this.wavCsvExeFile = @"C:\Factory\SubTools\wavCsv.exe";

				if (File.Exists(this.wavCsvExeFile) == false)
					throw new Exception("no wavCsv.exe");
			}

			this.ImgToolsExeFile = Path.Combine(Directory.GetCurrentDirectory(), "ImgTools.exe");

			if (File.Exists(this.ImgToolsExeFile) == false)
			{
				this.ImgToolsExeFile = @"C:\app\Kit\ImgTools\ImgTools.exe";

				if (File.Exists(this.ImgToolsExeFile) == false)
					throw new Exception("no ImgTools.exe");
			}
		}
	}
}
