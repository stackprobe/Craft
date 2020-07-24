using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.Tools;
using System.IO;

namespace Charlotte.ConvMains
{
	public class ConvMain
	{
		public void Perform(string spectrumFile, string jacketFile, string wDir, string cancelledFile, string successfulFile)
		{
			Ground.I.CancelledFile = cancelledFile;

			DDPicture jacket = DDPictureLoaders.Standard(jacketFile); // g

			DDResource.Load_DirectMode = true;
			jacket.GetHandle(); // pre-load
			DDResource.Load_DirectMode = false;

			new MovieGen0001().Main01(
				spectrumFile,
				jacket,
				wDir,
				20,
				12,
				230,
				GetBarColor(),
				0.6,
				3.0
				);

			File.WriteAllBytes(successfulFile, BinTools.EMPTY);
		}

		private I3Color GetBarColor()
		{
			int[] cs = SecurityTools.CRandom.ChooseOne(new int[][]
			{
				new int[] { 255, 128, 0 },
				new int[] { 255, 255, 0 },
			});

			SecurityTools.CRandom.Shuffle(cs);

			return new I3Color(cs[0], cs[1], cs[2]);
		}
	}
}
