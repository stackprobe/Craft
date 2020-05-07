using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;

namespace Charlotte.AudioPicMP4s
{
	public class MasterUtils
	{
		/// <summary>
		/// 音量の均一化を行う。
		/// 既に適切な音量であれば何もしない。
		/// </summary>
		/// <param name="targetWavFile">対象[.wav]ファイル</param>
		/// <returns>音量の均一化を行った場合 true そうでない場合 false</returns>
		public static bool Perform(string targetWavFile)
		{
			using (WorkingDir wd = new WorkingDir())
			{
				string wavFile = wd.GetPath("audio.wav");

				File.Copy(targetWavFile, wavFile);


			}
		}
	}
}
