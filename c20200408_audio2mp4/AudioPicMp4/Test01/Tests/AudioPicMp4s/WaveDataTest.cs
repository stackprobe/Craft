using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.AudioPicMp4s;
using Charlotte.Tools;

namespace Charlotte.Tests.AudioPicMp4s
{
	public class WaveDataTest
	{
		public void Test01()
		{
			Test01_a(@"C:\var\mp4\mp4\hbn.wav", @"C:\temp\hbn.csv");
			//Test01_a(@"C:\var\mp4\mp4\ddd.wav", @"C:\temp\ddd.csv");
			//Test01_a(@"C:\var\mp4\mp4\mho.wav", @"C:\temp\mho.csv");
			//Test01_a(@"C:\var\mp4\mp4\rlg.wav", @"C:\temp\rlg.csv");
		}

		private void Test01_a(string rFile, string wFile)
		{
			WaveData wave = new WaveData(rFile);

			using (CsvFileWriter writer = new CsvFileWriter(wFile))
			{
				for (double sec = 0.0; sec < wave.Length * 1.0 / wave.WavHz; sec += 0.1)
				{
					Console.WriteLine("sec: " + sec); // test

					wave.SetWavPart(DoubleTools.ToInt(sec * wave.WavHz));

					for (int hz = 30; hz < 4200; hz += 10)
					{
						writer.WriteCell(wave.GetSpectrum(hz).ToString("F9"));
					}
					writer.EndRow();
				}
			}
		}
	}
}
