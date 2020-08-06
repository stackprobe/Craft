using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public static class Consts
	{
		public const string IPC_IDENT = "{33e5978f-9010-4ea9-a23a-19875946523f}"; // shared_uuid

		public const string CONV_PROCESSING_TITLE = "変換中";
		public const string CONV_PROCESSING_MESSAGE_NORMAL = "変換しています...";
		public const string CONV_PROCESSING_MESSAGE_START_GEN_VIDEO = "映像生成プロセスを起動します！";
		public const string CONV_PROCESSING_MESSAGE_GEN_VIDEO_RUNNING = "映像生成プロセスを実行しています...";

		public const int CONV_THREAD_COUNT_MIN = 1;
		public const int CONV_THREAD_COUNT_DEF = 4;
		public const int CONV_THREAD_COUNT_MAX = 16;
	}
}
