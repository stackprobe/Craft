using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.SpectrumGens.Engines
{
	public class SpectrumGraph
	{
		public double R1 = 0.035;
		public double R2 = 0.9;

		// <---- prm

		public double[] Spectra;

		public SpectrumGraph(Func<int, double> getSpectrumByHz)
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
						spectrum = Math.Max(spectrum, getSpectrumByHz(hz));
						hz += 10;
					}

					spectrum *= R1;
					spectrum = Vf(spectrum);

					spectra.Add(spectrum);
				}
			}
			this.Spectra = spectra.ToArray();
		}

		private double Vf(double v)
		{
			double r = 1.0;

			for (; ; )
			{
				r *= R2;

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
