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
		public FileCommunicator CmProgressRate = new FileCommunicator(Consts.IPC_IDENT + "_PR");

		// ----

		public void Dispose()
		{
			if (this.CmProgressRate != null)
			{
				this.CmProgressRate.Dispose();
				this.CmProgressRate = null;

				this.EvStopConv.Dispose();
				this.EvCancellable_Y.Dispose();
				this.EvCancellable_N.Dispose();
				this.EvMessage_Normal.Dispose();
				this.EvMessage_StartGenVideo.Dispose();
				this.EvMessage_GenVideoRunning.Dispose();
			}
		}
	}
}
