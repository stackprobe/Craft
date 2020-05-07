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

		public static bool Mastering(string sourceWavFile, string destWavFile, Action<string[]> writeReport = null)
		{
			FileTools.Delete(destWavFile);

			using (WorkingDir wd = new WorkingDir())
			{
				File.Copy(sourceWavFile, wd.GetPath("in.wav"));

				ProcessTools.Batch(new string[]
				{
					string.Format(@"{0} /E {1} in.wav out.wav report.txt",
						Ground.I.MasterExeFile,
						CANCEL_EV_NAME
						),
				},
				wd.GetPath(".")
				);

				{
					string reportFile = wd.GetPath("report.txt");

					if (File.Exists(reportFile) == false)
						throw new Exception("[Mastering]レポートファイルが出力されませんでした。");

					if (writeReport != null)
						writeReport(File.ReadAllLines(reportFile, StringTools.ENCODING_SJIS));
				}

				{
					string midFile = wd.GetPath("out.wav");

					if (File.Exists(midFile) == false)
						return false;

					File.Copy(midFile, destWavFile);
				}
			}
			return true;
		}
	}
}
