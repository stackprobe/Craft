using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;

namespace Charlotte
{
	public class Ground : IDisposable
	{
		public static Ground I;

		public string ConvGenVideoFile;
		public Logger Logger;

		// ---- 受信

		public NamedEventUnit EvStopConv = new NamedEventUnit(Consts.IPC_IDENT + "_SC");

		// ---- 送信

		public NamedEventUnit EvCancellable_Y = new NamedEventUnit(Consts.IPC_IDENT + "_CY");
		public NamedEventUnit EvCancellable_N = new NamedEventUnit(Consts.IPC_IDENT + "_CN");
		public NamedEventUnit EvMessage_Normal = new NamedEventUnit(Consts.IPC_IDENT + "_MN");
		public NamedEventUnit EvMessage_StartGenVideo = new NamedEventUnit(Consts.IPC_IDENT + "_MS");
		public NamedEventUnit EvMessage_GenVideoRunning = new NamedEventUnit(Consts.IPC_IDENT + "_MR");
		public NamedEventUnit EvMessage_UserCancelled = new NamedEventUnit(Consts.IPC_IDENT + "_UC");
		public FileCommunicator CmProgressRate = new FileCommunicator(Consts.IPC_IDENT + "_PR");
		public FileCommunicator CmReport = new FileCommunicator(Consts.IPC_IDENT + "_RP");

		// ----

		private LimitCounter DisposeOnce = LimitCounter.One();

		public void Dispose()
		{
			if (this.DisposeOnce.Issue())
			{
				ExceptionDam.Section(eDam =>
				{
					eDam.Dispose(ref this.EvStopConv);
					eDam.Dispose(ref this.EvCancellable_Y);
					eDam.Dispose(ref this.EvCancellable_N);
					eDam.Dispose(ref this.EvMessage_Normal);
					eDam.Dispose(ref this.EvMessage_StartGenVideo);
					eDam.Dispose(ref this.EvMessage_GenVideoRunning);
					eDam.Dispose(ref this.EvMessage_UserCancelled);
					eDam.Dispose(ref this.CmProgressRate);
					eDam.Dispose(ref this.CmReport);
				});
			}
		}
	}
}
