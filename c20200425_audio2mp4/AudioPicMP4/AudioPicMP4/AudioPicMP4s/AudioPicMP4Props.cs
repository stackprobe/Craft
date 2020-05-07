using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.AudioPicMP4s
{
	public static class AudioPicMP4Props
	{
		/// <summary>
		/// フルパス SJIS 空白を含まないこと。
		/// </summary>
		public static string FFmpegPathBase = @"C:\app\ffmpeg-3.2.4-win64-shared\bin\";

		public static int FPS = 20;
		public static int VIDEO_W = 1920;
		public static int VIDEO_H = 1080;
		public static double AUDIO_DELAY_SEC = 0.2;
		public static int JPEG_QUALITY = 90;
	}
}
