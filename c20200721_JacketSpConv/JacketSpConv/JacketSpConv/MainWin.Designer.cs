﻿namespace Charlotte
{
	partial class MainWin
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWin));
			this.MainTimer = new System.Windows.Forms.Timer(this.components);
			this.InputDir = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.BtnInputDir = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.OutputDir = new System.Windows.Forms.TextBox();
			this.BtnOutputDir = new System.Windows.Forms.Button();
			this.BtnStart = new System.Windows.Forms.Button();
			this.OutputToInputDir = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// MainTimer
			// 
			this.MainTimer.Enabled = true;
			this.MainTimer.Tick += new System.EventHandler(this.MainTimer_Tick);
			// 
			// InputDir
			// 
			this.InputDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.InputDir.Location = new System.Drawing.Point(54, 73);
			this.InputDir.Name = "InputDir";
			this.InputDir.ReadOnly = true;
			this.InputDir.Size = new System.Drawing.Size(540, 27);
			this.InputDir.TabIndex = 1;
			this.InputDir.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.InputDir_KeyPress);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(50, 50);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "入力フォルダ：";
			// 
			// BtnInputDir
			// 
			this.BtnInputDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnInputDir.Location = new System.Drawing.Point(600, 73);
			this.BtnInputDir.Name = "BtnInputDir";
			this.BtnInputDir.Size = new System.Drawing.Size(50, 27);
			this.BtnInputDir.TabIndex = 2;
			this.BtnInputDir.Text = "...";
			this.BtnInputDir.UseVisualStyleBackColor = true;
			this.BtnInputDir.Click += new System.EventHandler(this.BtnInputDir_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(50, 150);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 20);
			this.label2.TabIndex = 3;
			this.label2.Text = "出力フォルダ：";
			// 
			// OutputDir
			// 
			this.OutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.OutputDir.Location = new System.Drawing.Point(54, 173);
			this.OutputDir.Name = "OutputDir";
			this.OutputDir.ReadOnly = true;
			this.OutputDir.Size = new System.Drawing.Size(540, 27);
			this.OutputDir.TabIndex = 4;
			this.OutputDir.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OutputDir_KeyPress);
			// 
			// BtnOutputDir
			// 
			this.BtnOutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnOutputDir.Location = new System.Drawing.Point(600, 173);
			this.BtnOutputDir.Name = "BtnOutputDir";
			this.BtnOutputDir.Size = new System.Drawing.Size(50, 27);
			this.BtnOutputDir.TabIndex = 5;
			this.BtnOutputDir.Text = "...";
			this.BtnOutputDir.UseVisualStyleBackColor = true;
			this.BtnOutputDir.Click += new System.EventHandler(this.BtnOutputDir_Click);
			// 
			// BtnStart
			// 
			this.BtnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnStart.Location = new System.Drawing.Point(450, 250);
			this.BtnStart.Name = "BtnStart";
			this.BtnStart.Size = new System.Drawing.Size(200, 60);
			this.BtnStart.TabIndex = 7;
			this.BtnStart.Text = "変換開始";
			this.BtnStart.UseVisualStyleBackColor = true;
			this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
			// 
			// OutputToInputDir
			// 
			this.OutputToInputDir.AutoSize = true;
			this.OutputToInputDir.Checked = true;
			this.OutputToInputDir.CheckState = System.Windows.Forms.CheckState.Checked;
			this.OutputToInputDir.Location = new System.Drawing.Point(54, 206);
			this.OutputToInputDir.Name = "OutputToInputDir";
			this.OutputToInputDir.Size = new System.Drawing.Size(236, 24);
			this.OutputToInputDir.TabIndex = 6;
			this.OutputToInputDir.Text = "入力フォルダと同じ場所に出力する";
			this.OutputToInputDir.UseVisualStyleBackColor = true;
			this.OutputToInputDir.CheckedChanged += new System.EventHandler(this.OutputToInputDir_CheckedChanged);
			// 
			// MainWin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(684, 351);
			this.Controls.Add(this.OutputToInputDir);
			this.Controls.Add(this.BtnStart);
			this.Controls.Add(this.BtnOutputDir);
			this.Controls.Add(this.OutputDir);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.BtnInputDir);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.InputDir);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainWin";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "音楽ファイルをスペクトラム付き動画に変換するプログラム";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWin_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWin_FormClosed);
			this.Load += new System.EventHandler(this.MainWin_Load);
			this.Shown += new System.EventHandler(this.MainWin_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Timer MainTimer;
		private System.Windows.Forms.TextBox InputDir;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button BtnInputDir;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox OutputDir;
		private System.Windows.Forms.Button BtnOutputDir;
		private System.Windows.Forms.Button BtnStart;
		private System.Windows.Forms.CheckBox OutputToInputDir;
	}
}

