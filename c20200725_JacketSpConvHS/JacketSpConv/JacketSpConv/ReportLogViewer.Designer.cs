namespace Charlotte
{
	partial class ReportLogViewer
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportLogViewer));
			this.MainSheet = new System.Windows.Forms.DataGridView();
			this.ReportStatus = new System.Windows.Forms.Label();
			this.CBDipsSuccess = new System.Windows.Forms.CheckBox();
			this.CBDispSkipped = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.MainSheet)).BeginInit();
			this.SuspendLayout();
			// 
			// MainSheet
			// 
			this.MainSheet.AllowUserToAddRows = false;
			this.MainSheet.AllowUserToDeleteRows = false;
			this.MainSheet.AllowUserToResizeRows = false;
			this.MainSheet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.MainSheet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.MainSheet.Location = new System.Drawing.Point(12, 42);
			this.MainSheet.Name = "MainSheet";
			this.MainSheet.ReadOnly = true;
			this.MainSheet.RowHeadersVisible = false;
			this.MainSheet.RowTemplate.Height = 21;
			this.MainSheet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.MainSheet.Size = new System.Drawing.Size(760, 507);
			this.MainSheet.TabIndex = 3;
			this.MainSheet.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.MainSheet_CellContentClick);
			this.MainSheet.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MainSheet_ColumnHeaderMouseClick);
			// 
			// ReportStatus
			// 
			this.ReportStatus.AutoSize = true;
			this.ReportStatus.ForeColor = System.Drawing.Color.Blue;
			this.ReportStatus.Location = new System.Drawing.Point(236, 13);
			this.ReportStatus.Name = "ReportStatus";
			this.ReportStatus.Size = new System.Drawing.Size(92, 20);
			this.ReportStatus.TabIndex = 2;
			this.ReportStatus.Text = "ReportStatus";
			// 
			// CBDipsSuccess
			// 
			this.CBDipsSuccess.AutoSize = true;
			this.CBDipsSuccess.Checked = true;
			this.CBDipsSuccess.CheckState = System.Windows.Forms.CheckState.Checked;
			this.CBDipsSuccess.Location = new System.Drawing.Point(12, 12);
			this.CBDipsSuccess.Name = "CBDipsSuccess";
			this.CBDipsSuccess.Size = new System.Drawing.Size(93, 24);
			this.CBDipsSuccess.TabIndex = 0;
			this.CBDipsSuccess.Text = "成功を表示";
			this.CBDipsSuccess.UseVisualStyleBackColor = true;
			this.CBDipsSuccess.CheckedChanged += new System.EventHandler(this.CBDipsSuccess_CheckedChanged);
			// 
			// CBDispSkipped
			// 
			this.CBDispSkipped.AutoSize = true;
			this.CBDispSkipped.Checked = true;
			this.CBDispSkipped.CheckState = System.Windows.Forms.CheckState.Checked;
			this.CBDispSkipped.Location = new System.Drawing.Point(111, 12);
			this.CBDispSkipped.Name = "CBDispSkipped";
			this.CBDispSkipped.Size = new System.Drawing.Size(119, 24);
			this.CBDispSkipped.TabIndex = 1;
			this.CBDispSkipped.Text = "スキップを表示";
			this.CBDispSkipped.UseVisualStyleBackColor = true;
			this.CBDispSkipped.CheckedChanged += new System.EventHandler(this.CBDispSkipped_CheckedChanged);
			// 
			// ReportLogViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 561);
			this.Controls.Add(this.CBDispSkipped);
			this.Controls.Add(this.CBDipsSuccess);
			this.Controls.Add(this.ReportStatus);
			this.Controls.Add(this.MainSheet);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MinimizeBox = false;
			this.Name = "ReportLogViewer";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "レポート";
			this.Load += new System.EventHandler(this.ReportLogViewer_Load);
			this.Shown += new System.EventHandler(this.ReportLogViewer_Shown);
			((System.ComponentModel.ISupportInitialize)(this.MainSheet)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView MainSheet;
		private System.Windows.Forms.Label ReportStatus;
		private System.Windows.Forms.CheckBox CBDipsSuccess;
		private System.Windows.Forms.CheckBox CBDispSkipped;
	}
}
