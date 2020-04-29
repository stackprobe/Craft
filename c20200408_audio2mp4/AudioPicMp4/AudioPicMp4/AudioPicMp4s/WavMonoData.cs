using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.AudioPicMp4s
{
	public class WavMonoData
	{
		public WaveData Parent;
		public double[] WavData;

		// <---- prm

		private Dictionary<string, WavMixedData> Cache = DictionaryTools.Create<WavMixedData>();

		public WavMixedData GetMixed(int hz, int phase)
		{
			if (hz < 1 || IntTools.IMAX < hz)
				throw new Exception("Bad hz: " + hz);

			if (phase < 0 || 1 < phase)
				throw new Exception("Bad phase: " + phase);

			string ident = hz + "_" + phase;

			if (this.Cache[ident] == null)
				this.Cache[ident] = this.CreateMixed(hz, phase);

			return this.Cache[ident];
		}

		private WavMixedData CreateMixed(int hz, int phase)
		{
			WavMixedData ret = new WavMixedData()
			{
				Parent = this,
				Hz = hz,
				Phase = phase,
				WavData = new double[this.WavData.Length],
			};

			for (int offset = 0; offset < this.WavData.Length; offset++)
			{
				double aa = offset * hz * (Math.PI * 2.0) / this.Parent.WavHz + phase * (Math.PI * 0.5);
				double vv = this.WavData[offset];

				double ss = Math.Sin(aa) * vv;

				ret.WavData[offset] = ss;
			}

			return ret;
		}

		public double GetSpectrum(int hz, int offset, int windowSize = WaveData.DEFAULT_WINDOW_SIZE, bool hammingFlag = WaveData.DEFAULT_HAMMING_FLAG)
		{
			return
				this.GetMixed(hz, 0).GetSpectrum(offset, windowSize, hammingFlag) +
				this.GetMixed(hz, 1).GetSpectrum(offset, windowSize, hammingFlag);
		}
	}
}
