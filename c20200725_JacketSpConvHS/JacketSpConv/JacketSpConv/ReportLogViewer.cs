using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using Charlotte.Chocomint.Dialogs;
using Charlotte.Tools;

namespace Charlotte
{
	public partial class ReportLogViewer : Form
	{
		public ReportLogViewer()
		{
			InitializeComponent();

			this.MinimumSize = this.Size;
			this.ReportStatus.Text = "";
		}

		private void ReportLogViewer_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void ReportLogViewer_Shown(object sender, EventArgs e)
		{
			try
			{
				this.MS_Init();
				this.MS_AutoResize();
				this.LoadReport();
				this.MS_AutoResize();

				ExtraTools.SetEnabledDoubleBuffer(this.MainSheet);

				this.MS_Refresh();
				this.MainSheet.ClearSelection();
			}
			catch (Exception ex)
			{
				MessageDlgTools.Error("レポート_初期化エラー", ex);

				// clear
				this.MainSheet.RowCount = 0;
				this.MainSheet.ColumnCount = 0;
			}
		}

		private void MS_Init()
		{
			this.MainSheet.RowCount = 0;
			this.MainSheet.ColumnCount = 0;

			this.MainSheet.ColumnCount = 3;
			this.MainSheet.Columns[0].HeaderText = "ファイル";
			this.MainSheet.Columns[1].HeaderText = "結果";
			this.MainSheet.Columns[2].HeaderText = "処理時間(秒)";

			this.MainSheet.Columns[0].SortMode = DataGridViewColumnSortMode.Programmatic;
			this.MainSheet.Columns[1].SortMode = DataGridViewColumnSortMode.Programmatic;
			this.MainSheet.Columns[2].SortMode = DataGridViewColumnSortMode.Programmatic;

			this.MainSheet.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
		}

		private void MS_AutoResize()
		{
			this.MainSheet.Columns[0].Width = 1000;
			this.MainSheet.Columns[1].Width = 1000;
			this.MainSheet.Columns[2].Width = 1000;

			this.MainSheet.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

			this.MainSheet.Columns[0].Width += 30;
			this.MainSheet.Columns[1].Width += 30;
			this.MainSheet.Columns[2].Width += 30;
		}

		private static string GetReportFile()
		{
			string file = @".\ConvReport.log";

			if (File.Exists(file) == false)
			{
				file = @"..\..\..\..\Conv\Conv\bin\Release\ConvReport.log";

				if (File.Exists(file) == false)
					throw new Exception("レポートが見つかりません。");
			}
			return file;
		}

		private void LoadReport()
		{
			this.MainSheet.Visible = false;
			try
			{
				int successCount = 0;
				int skippedCount = 0;
				int failedCount = 0;

				string[] lines = File.ReadAllLines(GetReportFile(), Encoding.UTF8);

				for (int rowidx = 0; rowidx * 2 < lines.Length; rowidx++)
				{
					string file = lines[rowidx * 2 + 0];
					string cond = lines[rowidx * 2 + 1];
					string elap;

					file = StringTools.GetIsland(file, "] ").Right;
					cond = StringTools.GetIsland(cond, "] ").Right;

					{
						StringTools.Island ilnd = StringTools.GetIsland(cond, "　(処理時間：");

						cond = ilnd.Left;
						elap = ilnd.Right;
					}

					elap = StringTools.GetIsland(elap, " ").Left;

					this.MainSheet.RowCount++;
					DataGridViewRow msRow = this.MainSheet.Rows[this.MainSheet.RowCount - 1];

					msRow.Cells[0].Value = file;
					msRow.Cells[1].Value = cond;
					msRow.Cells[2].Value = elap;

					switch (this.MS_GetRowCondByCondString(cond))
					{
						case MS_RowCond_e.Success: successCount++; break;
						case MS_RowCond_e.Skipped: skippedCount++; break;
						case MS_RowCond_e.Failed: failedCount++; break;

						default:
							throw null;
					}
				}
				this.ReportStatus.Text = string.Format("成功：{0} , スキップ：{1} , 失敗：{2}", successCount, skippedCount, failedCount);
			}
			finally
			{
				this.MainSheet.Visible = true;
			}
		}

		private void MainSheet_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			// noop
		}

		private void MainSheet_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex == -1)
			{
				this.MS_Sort(e.ColumnIndex);
			}
		}

		private void MS_Sort(int colidx)
		{
			SortOrder order = this.MainSheet.Columns[colidx].HeaderCell.SortGlyphDirection;

			if (order == SortOrder.Ascending)
				order = SortOrder.Descending;
			else
				order = SortOrder.Ascending;

			int orderSign = order == SortOrder.Ascending ? 1 : -1;

			if (colidx == 0)
			{
				this.MS_Sort((a, b) => StringTools.CompIgnoreCase(
					"" + a.Cells[0].Value,
					"" + b.Cells[0].Value
					)
					* orderSign
					);
			}
			else if (colidx == 1)
			{
				this.MS_Sort((a, b) => StringTools.Comp(
					"" + a.Cells[1].Value,
					"" + b.Cells[1].Value
					)
					* orderSign
					);
			}
			else // colidx == 2
			{
				this.MS_Sort((a, b) => DoubleTools.Comp(
					DoubleTools.ToDouble("" + a.Cells[2].Value, 0.0, IntTools.IMAX, 0.0),
					DoubleTools.ToDouble("" + b.Cells[2].Value, 0.0, IntTools.IMAX, 0.0)
					)
					* orderSign
					);
			}
			this.MainSheet.Columns[0].HeaderCell.SortGlyphDirection = SortOrder.None;
			this.MainSheet.Columns[1].HeaderCell.SortGlyphDirection = SortOrder.None;
			this.MainSheet.Columns[2].HeaderCell.SortGlyphDirection = SortOrder.None;

			this.MainSheet.Columns[colidx].HeaderCell.SortGlyphDirection = order;
		}

		private void MS_Sort(Comparison<DataGridViewRow> comp)
		{
			this.MainSheet.Sort(new MS_Comp()
			{
				Comp = comp,
			});

			this.MainSheet.ClearSelection();
		}

		private class MS_Comp : IComparer
		{
			public Comparison<DataGridViewRow> Comp;

			public int Compare(object a, object b)
			{
				return Comp((DataGridViewRow)a, (DataGridViewRow)b);
			}
		}

		private void CBDipsSuccess_CheckedChanged(object sender, EventArgs e)
		{
			this.MS_Refresh();
		}

		private void CBDispSkipped_CheckedChanged(object sender, EventArgs e)
		{
			this.MS_Refresh();
		}

		private void MS_Refresh()
		{
			bool dispSuccess = this.CBDipsSuccess.Checked;
			bool dispSkipped = this.CBDispSkipped.Checked;

			for (int rowidx = 0; rowidx < this.MainSheet.RowCount; rowidx++)
			{
				DataGridViewRow msRow = this.MainSheet.Rows[rowidx];
				bool visFlag;

				switch (this.MS_GetRowCond(msRow))
				{
					case MS_RowCond_e.Success: visFlag = dispSuccess; break;
					case MS_RowCond_e.Skipped: visFlag = dispSkipped; break;
					case MS_RowCond_e.Failed: visFlag = true; break;

					default:
						throw null;
				}
				msRow.Visible = visFlag;
			}
		}

		private enum MS_RowCond_e
		{
			Success,
			Skipped,
			Failed,
		}

		private MS_RowCond_e MS_GetRowCond(DataGridViewRow msRow)
		{
			string cond = "" + msRow.Cells[1].Value;

			return this.MS_GetRowCondByCondString(cond);
		}

		private MS_RowCond_e MS_GetRowCondByCondString(string cond)
		{
			if (cond == "成功")
				return MS_RowCond_e.Success;

			if (cond.EndsWith("(スキップ)"))
				return MS_RowCond_e.Skipped;

			return MS_RowCond_e.Failed;
		}
	}
}
