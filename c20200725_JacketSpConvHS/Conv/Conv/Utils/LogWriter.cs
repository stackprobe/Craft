using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace Charlotte.Utils
{
	public class LogWriter : IDisposable
	{
		private string LogFile;

		public LogWriter(string file)
		{
			this.LogFile = file;
		}

		public void Dispose()
		{
			// noop
		}

		private bool Wrote = false;

		public void WriteLine(object message)
		{
			for (int c = 0; c < 6; c++)
			{
				if (1 <= c)
				{
					Thread.Sleep(2000);
				}

				try
				{
					using (StreamWriter writer = new StreamWriter(this.LogFile, this.Wrote, Encoding.UTF8))
					{
						WriteLogLine(writer, message);

						if (1 <= c)
							WriteLogLine(writer, "★ログ出力をリトライしました。リトライ回数：" + c);
					}
					this.Wrote = true;
					return;
				}
				catch
				{ }
			}
		}

		private static void WriteLogLine(StreamWriter writer, object message)
		{
			writer.WriteLine("[" + DateTime.Now + "] " + message);
		}
	}
}
