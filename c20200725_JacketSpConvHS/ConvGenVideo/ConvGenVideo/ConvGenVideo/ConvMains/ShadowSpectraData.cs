using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.ConvMains
{
	public class ShadowSpectraData
	{
		public double[] ShadowSpectra = null;

		public void Projection(double[] spectra, double fallSpan = 0.01)
		{
			if (this.ShadowSpectra == null)
				this.ShadowSpectra = new double[spectra.Length];

			for (int index = 0; index < this.ShadowSpectra.Length; index++)
			{
				this.ShadowSpectra[index] -= fallSpan;
				this.ShadowSpectra[index] = Math.Max(this.ShadowSpectra[index], spectra[index]);
			}
		}
	}
}
