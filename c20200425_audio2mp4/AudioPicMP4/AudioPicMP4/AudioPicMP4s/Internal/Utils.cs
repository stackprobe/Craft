using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Tools;

namespace Charlotte.AudioPicMP4s.Internal
{
	internal static class Utils
	{
		public static bool IsEmptyFile(string file)
		{
			return File.Exists(file) == false || new FileInfo(file).Length == 0;
		}

		public static D2Size TangentSize(D2Size frame, D2Size size, bool interior)
		{
			double w = frame.W;
			double h = frame.W * size.H / size.W;

			if (interior ? frame.H < h : h < frame.H)
			{
				w = frame.H * size.W / size.H;
				h = frame.H;
			}
			return new D2Size(w, h);
		}

		public static D4Rect Centering(D2Size frame, D2Size size)
		{
			return new D4Rect(
				(frame.W - size.W) / 2.0,
				(frame.H - size.H) / 2.0,
				size.W,
				size.H
				);
		}
	}
}
