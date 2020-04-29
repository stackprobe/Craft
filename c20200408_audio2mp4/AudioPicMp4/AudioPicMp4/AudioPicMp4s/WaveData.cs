using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;
using Charlotte.AudioPicMP4s.Internal;

namespace Charlotte.AudioPicMP4s
{
	/// <summary>
	/// 音声
	/// </summary>
	public class WaveData
	{
		private double[] WavData_L;
		private double[] WavData_R;
		public int WavHz { get; private set; }

		public WaveData(string wavFile)
		{
			using (WorkingDir wd = new WorkingDir())
			{
				File.Copy(wavFile, wd.GetPath("1.wav"));

				ProcessTools.Batch(new string[]
				{
					Ground.I.wavCsvExeFile + " /W2C 1.wav 1.csv 1.hz",
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

		public int Length
		{
			get
			{
				return this.WavData_L.Length;
			}
		}

		private const int WINDOW_SIZE = 1000;

		private double[] WavPart_L = new double[WINDOW_SIZE];
		private double[] WavPart_R = new double[WINDOW_SIZE];

		public void SetWavPart(int startPos)
		{
			if (startPos < 0 || this.WavData_L.Length - WINDOW_SIZE < startPos) // ? startPos out of range
			{
				for (int offset = 0; offset < WINDOW_SIZE; offset++)
				{
					this.WavPart_L[offset] = 0.0;
					this.WavPart_R[offset] = 0.0;
				}
			}
			else
			{
				for (int offset = 0; offset < WINDOW_SIZE; offset++)
				{
					double rate = offset * 1.0 / (WINDOW_SIZE - 1);
					double hh = Hamming(rate);

					this.WavPart_L[offset] = this.WavData_L[startPos + offset] * hh;
					this.WavPart_R[offset] = this.WavData_R[startPos + offset] * hh;
				}
			}
		}

		private static double Hamming(double rate)
		{
			return 0.5 - 0.5 * Math.Cos(rate * Math.PI * 2.0);
		}

		public double GetSpectrum(int hz)
		{
			return (this.GetSpectrum_L(hz) + this.GetSpectrum_R(hz)) / 2.0;
		}

		public double GetSpectrum_L(int hz)
		{
			return this.GetSpectrum(hz, this.WavPart_L);
		}

		public double GetSpectrum_R(int hz)
		{
			return this.GetSpectrum(hz, this.WavPart_R);
		}

		private double GetSpectrum(int hz, double[] windowData)
		{
			if (hz < 1 || IntTools.IMAX < hz)
				throw new Exception("Bad hz: " + hz);

			double cc = 0.0;
			double ss = 0.0;

			for (int offset = 0; offset < WINDOW_SIZE; offset++)
			{
				double aa = offset * hz * (Math.PI * 2.0) / this.WavHz;
				double vv = windowData[offset];

				cc += Math.Cos(aa) * vv;
				ss += Math.Sin(aa) * vv;
			}
			return cc * cc + ss * ss;
		}
	}
}
