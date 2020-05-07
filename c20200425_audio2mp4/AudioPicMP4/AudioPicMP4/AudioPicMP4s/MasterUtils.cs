using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;
using Charlotte.AudioPicMP4s.Internal;

namespace Charlotte.AudioPicMP4s
{
	public class MasterUtils
	{
		public const string CANCEL_EV_NAME = "{3f8c34fe-ef63-43dc-87e1-aba025777fe7}";

		/// <summary>
		/// 音量の均一化を行う。
		/// 既に適切な音量であれば何もしない。
		/// </summary>
		/// <param name="targetWavFile">対象[.wav]ファイル</param>
		/// <returns>音量の均一化を行った場合 true そうでない場合 false</returns>
		public static bool Mastering(string targetWavFile, Action<string[]> writeReport = null)
		{
			using (WorkingDir wd = new WorkingDir())
			{
				File.Copy(targetWavFile, wd.GetPath("in.wav"));

				ProcessTools.Batch(new string[]
				{
					string.Format(@"{0} /E {1} in.wav out.wav report.txt",
						Ground.I.MasterExeFile,
						CANCEL_EV_NAME
						),
				},
				wd.GetPath(".")
				);

				if (writeReport != null)
					writeReport(File.ReadAllLines(wd.GetPath("report.txt"), StringTools.ENCODING_SJIS));

				{
					string midFile = wd.GetPath("out.wav");

					if (File.Exists(midFile) == false)
						return false;

					File.Copy(midFile, targetWavFile, true);
				}
			}
			return true;
		}
	}
}
