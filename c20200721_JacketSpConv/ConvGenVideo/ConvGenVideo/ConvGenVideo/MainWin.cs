using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Security.Permissions;
using Charlotte.Tools;
using Charlotte.Common;

// ^ sync @ G2_MainWin

namespace Charlotte
{
	public partial class MainWin : Form
	{
		// sync > @ G2_MainWin

		#region ALT_F4 抑止

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			const int WM_SYSCOMMAND = 0x112;
			const long SC_CLOSE = 0xF060L;

			if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt64() & 0xFFF0L) == SC_CLOSE)
				return;

			base.WndProc(ref m);
		}

		#endregion

		public MainWin()
		{
			InitializeComponent();
		}

		private void MainWin_Load(object sender, EventArgs e)
		{
			// noop
		}

		public static MainWin I = null;

		private void MainWin_Shown(object sender, EventArgs e)
		{
			ProcMain.WriteLog = message =>
			{
				if (message is Exception)
					throw new AggregateException((Exception)message);
				else
					throw new Exception("Bad log: " + message);
			};

			bool aliving = true;

			DDAdditionalEvents.PostGameStart_G2 = () =>
			{
				this.BeginInvoke((MethodInvoker)delegate
				{
					if (aliving)
						this.Visible = false;
				});
			};

			Thread th = new Thread(() =>
			{
				I = this;
				new Program2().Main2();
				I = null;

				this.BeginInvoke((MethodInvoker)delegate
				{
					aliving = false;
					this.Close();
				});
			});

			th.Start();
		}

		private void MainWin_FormClosing(object sender, FormClosingEventArgs e)
		{
			// noop
		}

		private void MainWin_FormClosed(object sender, FormClosedEventArgs e)
		{
			// noop
		}

		// < sync
	}
}
