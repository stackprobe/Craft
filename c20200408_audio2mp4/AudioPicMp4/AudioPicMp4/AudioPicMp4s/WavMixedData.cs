using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.AudioPicMp4s
{
	public class WavMixedData
	{
		public WavMonoData Parent;
		public int Hz;
		public int Phase;
		public double[] WavData;

		// <---- prm

		public double GetSpectrum(int offset, int windowSize = WaveData.DEFAULT_WINDOW_SIZE, bool hammingFlag = WaveData.DEFAULT_HAMMING_FLAG)
		{
			if (offset < 0 || this.WavData.Length <= offset)
				throw new Exception("Bad offset: " + offset);

			if (windowSize < 2 || IntTools.IMAX < windowSize || windowSize % 2 != 0)
				throw new Exception("Bad windowSize: " + windowSize);

			//hammingFlag

			int start = offset - windowSize / 2;
			int end = offset + windowSize / 2;

			if (start < 0 || this.WavData.Length < end)
				return 0.0;

			double ss = 0.0;

			for (int index = start; index < end; index++)
				ss += this.WavData[index] * Hamming((index - start) * 1.0 / (windowSize - 1));

			return ss * ss;
		}

		private static double Hamming(double rate)
		{
			return 0.5 - 0.5 * Math.Cos(rate * Math.PI * 2.0);
		}
	}
}
