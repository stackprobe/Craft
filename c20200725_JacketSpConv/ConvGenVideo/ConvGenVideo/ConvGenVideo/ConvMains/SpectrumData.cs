using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.ConvMains
{
	public class SpectrumData
	{
		public double[][] Rows;

		public SpectrumData(string csvFile)
		{
			List<double[]> dest = new List<double[]>();

			using (CsvFileReader reader = new CsvFileReader(csvFile))
			{
				for (; ; )
				{
					string[] sRow = reader.ReadRow();

					if (sRow == null)
						break;

					double[] row = sRow.Select(v => double.Parse(v)).ToArray();

					if (row.Length != Consts.SPECTRA_LEN)
						throw null;

					if (row.Any(v => v < 0.0 || 1.0 < v))
						throw null;

					dest.Add(row);
				}
			}
			this.Rows = dest.ToArray();

			{
				double valMax = this.Rows.Select(row => row.Max()).Max();

				this.Rows = this.Rows.Select(row => row.Select(v => v / valMax).ToArray()).ToArray();
			}
		}
	}
}
