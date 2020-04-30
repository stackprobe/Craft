using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;

namespace Charlotte.AudioPicMP4s
{
	/// <summary>
	/// 動画(映像と音声)
	/// </summary>
	public class MovieWorkbench : IDisposable
	{
		private WorkingDir WD;

		public MovieWorkbench()
		{
			this.WD = new WorkingDir();
		}

		public string GetImageFile(int index)
		{
			return Path.Combine(this.GetImageDir(), index + ".jpg");
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

		public void Dispose()
		{
			this.WD.Dispose();
			this.WD = null;
		}
	}
}
