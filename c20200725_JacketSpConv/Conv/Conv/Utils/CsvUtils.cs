using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.Utils
{
	public static class CsvUtils
	{
		public static void WriteRow(CsvFileWriter writer, SpectrumGraph graph)
		{
			foreach (double level in graph.Spectra)
				writer.WriteCell(level.ToString("F9"));

			writer.EndRow();
		}
	}
}
