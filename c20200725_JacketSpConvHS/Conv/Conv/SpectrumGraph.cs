﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
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

			//var m = new { cMax = 8, dEnd = 5, hzAdd = 25 }; // 40 本, 10hz ～ 4510hz
			//var m = new { cMax = 9, dEnd = 5, hzAdd = 20 }; // 45 本, 10hz ～ 4510hz
			var m = new { cMax = 10, dEnd = 6, hzAdd = 13 }; // 60 本, 10hz ～ 4300hz
			//var m = new { cMax = 9, dEnd = 10, hzAdd = 10 }; // 90 本, 10hz ～ 4510hz

			for (int c = 1; c <= m.cMax; c++)
			{
				for (int d = 0; d < m.dEnd; d++)
				{
					double spectrum = 0.0;

					for (int i = 0; i < c; i++)
					{
						spectrum = Math.Max(spectrum, getSpectrumByHz(hz));
						hz += m.hzAdd;
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
