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
		private LogWriter ReportLog;

		public Logger(LogWriter stat, LogWriter info, LogWriter report)
		{
			this.StatLog = stat;
			this.InfoLog = info;
			this.ReportLog = report;
		}

		public void Stat(object message)
		{
			CoutLine("[Stat] " + message);

			this.StatLog.WriteLine(message);
			this.InfoLog.WriteLine(message);
		}

		public void Info(object message)
		{
			CoutLine("[Info] " + message);

			this.InfoLog.WriteLine(message);
		}

		public void Report(object message)
		{
			CoutLine("[Report] " + message);

			this.ReportLog.WriteLine(message);

			Ground.I.CmReport.Send(Encoding.UTF8.GetBytes(message.ToString()));
		}

		private void CoutLine(string message)
		{
			Console.WriteLine(message);
		}
	}
}
