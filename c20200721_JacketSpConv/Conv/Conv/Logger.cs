using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Utils;

namespace Charlotte
{
	public class Logger
	{
		private LogWriter StatLog;
		private LogWriter InfoLog;

		public Logger(LogWriter stat, LogWriter info)
		{
			this.StatLog = stat;
			this.InfoLog = info;
		}

		public void Stat(object message)
		{
			this.StatLog.WriteLine(message);
			this.InfoLog.WriteLine(message);
		}

		public void Info(object message)
		{
			this.InfoLog.WriteLine(message);
		}
	}
}
