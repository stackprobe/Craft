using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Charlotte.Utils
{
	public class LogWriter : IDisposable
	{
		private StreamWriter Writer;

		public LogWriter(string file)
		{
			this.Writer = new StreamWriter(file, false, Encoding.UTF8);
		}

		public void Dispose()
		{
			if (this.Writer != null)
			{
				this.Writer.Dispose();
				this.Writer = null;
			}
		}

		public void WriteLine(object message)
		{
			this.Writer.WriteLine("[" + DateTime.Now + "] " + message);
		}
	}
}
