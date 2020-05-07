using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.AudioPicMP4s.Internal.SpectrumGraphs
{
	internal class SpectrumGraph0001
	{
		public double[] Spectra;

		public SpectrumGraph0001(WaveData wave)
		{
			List<double> spectra = new List<double>();
			int hz = 10;

			for (int c = 1; c <= 9; c++)
			{
				for (int d = 0; d < 10; d++)
				{
					double spectrum = 0.0;

					for (int i = 0; i < c; i++)
					{
						spectrum = Math.Max(spectrum, wave.GetSpectrum(hz));
						hz += 10;
					}

					spectrum *= 0.035; // 要調整
					spectrum = Vf(spectrum);

					spectra.Add(spectrum);
				}
			}
			this.Spectra = spectra.ToArray();
		}

		private static double Vf(double v)
		{
			double r = 1.0;

			for (; ; )
			{
				r *= 0.9;

				double b = 1.0 - r;

				if (v <= b)
					break;

				v -= b;
				v /= 2.0;
				v += b;
			}
			return v;
		}
	}
}
