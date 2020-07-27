using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Chocomint.Dialogs;
using Charlotte.Tools;
using System.IO;

namespace Charlotte
{
	public class ConvMain
	{
		public void Perform(string inputDir, string outputDir, bool outputOverwriteMode)
		{
			try
			{
				using (WorkingDir wd = new WorkingDir())
				{
					string successfulFile = wd.MakePath();
					double progressRate = 0.0;

					WaitDlgTools.Show(
						Consts.CONV_PROCESSING_TITLE,
						Consts.CONV_PROCESSING_MESSAGE_NORMAL,
						() =>
						{
							ProcessTools.Batch(new string[]
							{
								string.Format(Ground.I.ConvExeFile + " CS-Conv \"{0}\" \"{1}\" \"{2}\" \"{3}\"", inputDir, outputDir, outputOverwriteMode ? 1 : 0, successfulFile),
							},
							ProcMain.SelfDir
							);
						},
						() =>
						{
							if (Ground.I.EvCancellable_Y.WaitForMillis(0))
							{
								WaitDlg.Cancellable = true;
							}
							if (Ground.I.EvCancellable_N.WaitForMillis(0))
							{
								WaitDlg.Cancellable = false;
							}
							if (Ground.I.EvMessage_Normal.WaitForMillis(0))
							{
								WaitDlg.MessagePost.Post(Consts.CONV_PROCESSING_MESSAGE_NORMAL);
							}
							if (Ground.I.EvMessage_StartGenVideo.WaitForMillis(0))
							{
								WaitDlg.MessagePost.Post(Consts.CONV_PROCESSING_MESSAGE_START_GEN_VIDEO);
							}
							if (Ground.I.EvMessage_GenVideoRunning.WaitForMillis(0))
							{
								WaitDlg.MessagePost.Post(Consts.CONV_PROCESSING_MESSAGE_GEN_VIDEO_RUNNING);
							}

							{
								byte[] message = Ground.I.CmProgressRate.Recv();

								if (message != null)
									progressRate = double.Parse(Encoding.ASCII.GetString(message));
							}

							return progressRate;
						},
						() =>
						{
							Ground.I.EvStop_Conv.Set();
							Ground.I.EvStop_Master.Set();
						});

					if (File.Exists(successfulFile) == false)
						throw new Exception("変換プロセスは正常に動作しませんでした。");

					bool userCancelled = Ground.I.EvMessage_UserCancelled.WaitForMillis(0);

					if (WaitDlg.LastCancelled || userCancelled)
						MessageDlgTools.Show(MessageDlg.Mode_e.Warning, "変換中止", "変換プロセスを中止しました。(" + (userCancelled ? 1 : 0) + ")");
					else
						MessageDlgTools.Information("変換完了", "変換プロセスは終了しました。");
				}
			}
			catch (Exception ex)
			{
				MessageDlgTools.Error("変換処理エラー", ex);
			}
		}
	}
}
