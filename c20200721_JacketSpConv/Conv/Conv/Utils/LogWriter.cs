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
			for (int c = 0; c < 3; c++)
			{
				if (1 <= c)
				{
					Thread.Sleep(2000);
				}

				try
				{
					using (StreamWriter writer = new StreamWriter(this.LogFile, this.Wrote, Encoding.UTF8))
					{
						writer.WriteLine("[" + DateTime.Now + "] " + message);
					}
					this.Wrote = true;
					break;
				}
				catch
				{ }
			}
		}
	}
}
