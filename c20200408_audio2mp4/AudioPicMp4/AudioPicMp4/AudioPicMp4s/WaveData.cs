using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;

namespace Charlotte.AudioPicMp4s
{
	/// <summary>
	/// 音声
	/// </summary>
	public class WaveData
	{
		private double[] WavData_L;
		private double[] WavData_R;
		private int WavHz;

		public WaveData(string wavFile)
		{
			using (WorkingDir wd = new WorkingDir())
			{
				File.Copy(wavFile, wd.GetPath("1.wav"));

				ProcessTools.Batch(new string[]
				{
					@"C:\Factory\SubTools\wavCsv.exe /W2C 1.wav 1.csv 1.hz",
				},
				wd.GetPath(".")
				);

				using (CsvFileReader reader = new CsvFileReader(wd.GetPath("1.csv")))
				{
					List<double> wavData_L = new List<double>();
					List<double> wavData_R = new List<double>();

					for (; ; )
					{
						string[] row = reader.ReadRow();

						if (row == null)
							break;

						wavData_L.Add((int.Parse(row[0]) / 65536.0 - 0.5) * 2.0);
						wavData_R.Add((int.Parse(row[1]) / 65536.0 - 0.5) * 2.0);
					}
					if (wavData_L.Count == 0)
						throw new Exception("wavData_L.Count == 0");

					this.WavData_L = wavData_L.ToArray();
					this.WavData_R = wavData_R.ToArray();
				}
				this.WavHz = int.Parse(File.ReadAllText(wd.GetPath("1.hz"), Encoding.ASCII));

				if (this.WavHz < 1 || IntTools.IMAX < this.WavHz)
					throw new Exception("Bad WavHz: " + this.WavHz);
			}
		}
	}
}
