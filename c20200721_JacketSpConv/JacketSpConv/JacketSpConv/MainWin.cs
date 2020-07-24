using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Security.Permissions;
using System.Windows.Forms;
using Charlotte.Tools;
using Charlotte.Chocomint.Dialogs;

namespace Charlotte
{
	public partial class MainWin : Form
	{
		#region ALT_F4 抑止

		private bool XPressed = false;

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			const int WM_SYSCOMMAND = 0x112;
			const long SC_CLOSE = 0xF060L;

			if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt64() & 0xFFF0L) == SC_CLOSE)
			{
				this.XPressed = true;
				return;
			}
			base.WndProc(ref m);
		}

		#endregion

		public MainWin()
		{
			InitializeComponent();

			this.MinimumSize = this.Size;
		}

		private void MainWin_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void MainWin_Shown(object sender, EventArgs e)
		{
			// -- 0001

			Ground.I = new Ground();

			{
				string file = @".\Conv.exe";

				if (File.Exists(file) == false)
				{
					file = @"..\..\..\..\Conv\Conv\bin\Release\Conv.exe";

					if (File.Exists(file) == false)
						throw new Exception();
				}
				Ground.I.ConvExeFile = file;
			}

			if (Ground.I.Load())
			{
				this.InputDir.Text = Ground.I.InputDir;
				this.OutputDir.Text = Ground.I.OutputDir;
				this.OutputToInputDir.Checked = Ground.I.OutputToInputDir;
				this.OutputOverwriteMode.Checked = Ground.I.OutputOverwriteMode;
			}

			this.UIRefresh();

			// ----

			this.MTBusy.Leave();
		}

		private void MainWin_FormClosing(object sender, FormClosingEventArgs e)
		{
			// noop
		}

		private void MainWin_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.MTBusy.Enter(); // 2bs

			// ----

			// -- 9999
		}

		private void CloseWindow()
		{
			using (this.MTBusy.Section())
			{
				try
				{
					// -- 9000

					Ground.I.InputDir = this.InputDir.Text;
					Ground.I.OutputDir = this.OutputDir.Text;
					Ground.I.OutputToInputDir = this.OutputToInputDir.Checked;
					Ground.I.OutputOverwriteMode = this.OutputOverwriteMode.Checked;
					Ground.I.Save();

					Ground.I.Dispose();
					Ground.I = null;

					// ----
				}
				catch (Exception e)
				{
					MessageDlgTools.Error("Error @ CloseWindow()", e);
				}
				this.MTBusy.Enter();
				this.Close();
			}
		}

		private VisitorCounter MTBusy = new VisitorCounter(1);
		private long MTCount;

		private void MainTimer_Tick(object sender, EventArgs e)
		{
			if (this.MTBusy.HasVisitor())
				return;

			this.MTBusy.Enter();
			try
			{
				if (this.XPressed)
				{
					this.XPressed = false;
					this.CloseWindow();
					return;
				}

				// -- 3001

				// ----
			}
			catch (Exception ex)
			{
				MessageDlgTools.Error("Error @ Timer", ex);
			}
			finally
			{
				this.MTBusy.Leave();
				this.MTCount++;
			}
		}

		private void UIRefresh()
		{
			{
				bool flag = this.OutputToInputDir.Checked;

				this.BtnOutputDir.Enabled = flag == false;

				if (flag)
					this.OutputDir.Text = this.InputDir.Text;
			}
		}

		private void UIEvent_Go(Action routine)
		{
			using (this.MTBusy.Section())
			{
				try
				{
					routine();
				}
				catch (Exception e)
				{
					MessageDlgTools.Error("Error @ UIEvent_Go()", e);
				}
			}
		}

		private void BtnInputDir_Click(object sender, EventArgs e)
		{
			this.UIEvent_Go(() =>
			{
				string dir = this.InputDir.Text;

				dir = InputFolderDlgTools.Existing("入力フォルダ", "入力フォルダを選択して下さい。", false, dir);

				if (dir != null)
				{
					this.InputDir.Text = dir;
					this.UIRefresh();
				}
			});
		}

		private void BtnOutputDir_Click(object sender, EventArgs e)
		{
			this.UIEvent_Go(() =>
			{
				string dir = this.OutputDir.Text;

				dir = InputFolderDlgTools.Existing("出力フォルダ", "出力フォルダを選択して下さい。", false, dir);

				if (dir != null)
				{
					this.OutputDir.Text = dir;
					this.UIRefresh();
				}
			});
		}

		private void OutputToInputDir_CheckedChanged(object sender, EventArgs e)
		{
			this.UIEvent_Go(() =>
			{
				this.UIRefresh();
			});
		}

		private void InputDir_KeyPress(object sender, KeyPressEventArgs e)
		{
			this.UIEvent_Go(() =>
			{
				if (e.KeyChar == 1) // ctrl_a
				{
					this.InputDir.SelectAll();
				}
			});
		}

		private void OutputDir_KeyPress(object sender, KeyPressEventArgs e)
		{
			this.UIEvent_Go(() =>
			{
				if (e.KeyChar == 1) // ctrl_a
				{
					this.OutputDir.SelectAll();
				}
			});
		}

		private void BtnStart_Click(object sender, EventArgs e)
		{
			this.UIEvent_Go(() =>
			{
				string inputDir = this.InputDir.Text;
				string outputDir = this.OutputDir.Text;

				try
				{
					if (inputDir == "")
						throw new Exception("入力フォルダを選択して下さい。");

					if (outputDir == "")
						throw new Exception("出力フォルダを選択して下さい。");

					inputDir = FileTools.MakeFullPath(inputDir);
					outputDir = FileTools.MakeFullPath(outputDir);

					if (Directory.Exists(inputDir) == false)
						throw new Exception("入力フォルダが存在しません。");

					if (Directory.Exists(outputDir) == false)
						throw new Exception("出力フォルダが存在しません。");
				}
				catch (Exception ex)
				{
					MessageDlgTools.Warning("変換開始_失敗", ex);
					return;
				}

				this.Visible = false;

				new ConvMain().Perform(inputDir, outputDir, this.OutputOverwriteMode.Checked);

				this.CloseWindow();
				//this.Visible = true; // 再表示は不要
			});
		}
	}
}
