using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using Charlotte.Tests.AudioPicMP4s.MovieMakers;
using Charlotte.Tests.AudioPicMP4s;
using Charlotte.Tests.AudioPicMP4s.MovieMakers.VideoMakers;

namespace Charlotte
{
	class Program
	{
		public const string APP_IDENT = "{777c6a92-7305-4c43-8200-23fc4344644a}";
		public const string APP_TITLE = "AudioPicMP4";

		static void Main(string[] args)
		{
			ProcMain.CUIMain(new Program().Main2, APP_IDENT, APP_TITLE);

#if DEBUG
			//if (ProcMain.CUIError)
			{
				Console.WriteLine("Press ENTER.");
				Console.ReadLine();
			}
#endif
		}

		private void Main2(ArgsReader ar)
		{
			//new FFmpegConvTest().Test01();
			//new FFmpegConvTest().Test02();
			//new MasterUtilsTest().Test01();
			//new MasterUtilsTest().Test02();
			//new MovieMaker0001Test().Test01();
			//new MovieMaker0002Test().Test01();
			//new MovieMaker0002Test().Test02();
			new VideoMaker0002Test().Test01();
			//new VideoMaker0002Test().Test02();
		}
	}
}
