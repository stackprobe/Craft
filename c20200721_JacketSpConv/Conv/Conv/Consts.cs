﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public class Consts
	{
		public const string IPC_IDENT = "{8e0d71e4-a754-4a40-a5f1-47dad142fbc3}"; // shared_uuid

		public const string EV_STOP_MASTER = IPC_IDENT + "_SM";

		public const string FFMPEG_FILE = @"C:\app\ffmpeg-4.1.3-win64-shared\bin\ffmpeg.exe";
		public const string MASTER_FILE = @"C:\Factory\Program\WavMaster\Master.exe";
		public const string wavCsv_FILE = @"C:\Factory\SubTools\wavCsv.exe";
		public const string ImgTools_FILE = @"C:\app\Kit\ImgTools\ImgTools.exe";

		public static string[] AUDIO_EXTS = new string[]
		{
			".ogg",
			".mp3",
			".wav",
		};

		public static string[] JACKET_EXTS = new string[]
		{
			".bmp",
			".jpg",
			".jpeg",
			".png",
			".gif",
		};

		public const string MOVIE_EXT = ".mp4";

		public const int JACKET_W_MIN = 10;
		public const int JACKET_H_MIN = 10;
		public const int JACKET_W_MAX = 1800;
		public const int JACKET_H_MAX = 1000;

		public const int FPS = 20;
		public const double AUDIO_DELAY_SEC = 0.2;
		public const int JPEG_QUALITY = 90;
	}
}
