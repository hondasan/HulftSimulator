namespace HulftSchedulerApp
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblHulftId = new System.Windows.Forms.Label();
            this.txtHulftId = new System.Windows.Forms.TextBox();
            this.lblFolder = new System.Windows.Forms.Label();
            this.txtFolderPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblStartFile = new System.Windows.Forms.Label();
            this.cboStartFile = new System.Windows.Forms.ComboBox();
            this.lblInterval = new System.Windows.Forms.Label();
            this.numInterval = new System.Windows.Forms.NumericUpDown();
            this.lblIntervalUnit = new System.Windows.Forms.Label();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.lblLog = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // lblHulftId
            // 
            this.lblHulftId.AutoSize = true;
            this.lblHulftId.Location = new System.Drawing.Point(24, 22);
            this.lblHulftId.Name = "lblHulftId";
            this.lblHulftId.Size = new System.Drawing.Size(102, 12);
            this.lblHulftId.TabIndex = 0;
            this.lblHulftId.Text = "HULFTファイルID";
            // 
            // txtHulftId
            // 
            this.txtHulftId.Location = new System.Drawing.Point(152, 19);
            this.txtHulftId.Name = "txtHulftId";
            this.txtHulftId.Size = new System.Drawing.Size(302, 19);
            this.txtHulftId.TabIndex = 1;
            // 
            // lblFolder
            // 
            this.lblFolder.AutoSize = true;
            this.lblFolder.Location = new System.Drawing.Point(24, 56);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(125, 12);
            this.lblFolder.TabIndex = 2;
            this.lblFolder.Text = "送信ファイル格納フォルダ";
            // 
            // txtFolderPath
            // 
            this.txtFolderPath.Location = new System.Drawing.Point(152, 53);
            this.txtFolderPath.Name = "txtFolderPath";
            this.txtFolderPath.ReadOnly = true;
            this.txtFolderPath.Size = new System.Drawing.Size(360, 19);
            this.txtFolderPath.TabIndex = 3;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(518, 51);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(90, 23);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "参照...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
            // 
            // lblStartFile
            // 
            this.lblStartFile.AutoSize = true;
            this.lblStartFile.Location = new System.Drawing.Point(24, 91);
            this.lblStartFile.Name = "lblStartFile";
            this.lblStartFile.Size = new System.Drawing.Size(76, 12);
            this.lblStartFile.TabIndex = 5;
            this.lblStartFile.Text = "開始ファイル名";
            // 
            // cboStartFile
            // 
            this.cboStartFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStartFile.FormattingEnabled = true;
            this.cboStartFile.Location = new System.Drawing.Point(152, 88);
            this.cboStartFile.Name = "cboStartFile";
            this.cboStartFile.Size = new System.Drawing.Size(302, 20);
            this.cboStartFile.TabIndex = 6;
            // 
            // lblInterval
            // 
            this.lblInterval.AutoSize = true;
            this.lblInterval.Location = new System.Drawing.Point(24, 126);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(79, 12);
            this.lblInterval.TabIndex = 7;
            this.lblInterval.Text = "送信周期（秒）";
            // 
            // numInterval
            // 
            this.numInterval.Location = new System.Drawing.Point(152, 124);
            this.numInterval.Maximum = new decimal(new int[] {
            86400,
            0,
            0,
            0});
            this.numInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numInterval.Name = "numInterval";
            this.numInterval.Size = new System.Drawing.Size(120, 19);
            this.numInterval.TabIndex = 8;
            this.numInterval.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // lblIntervalUnit
            // 
            this.lblIntervalUnit.AutoSize = true;
            this.lblIntervalUnit.Location = new System.Drawing.Point(278, 126);
            this.lblIntervalUnit.Name = "lblIntervalUnit";
            this.lblIntervalUnit.Size = new System.Drawing.Size(29, 12);
            this.lblIntervalUnit.TabIndex = 9;
            this.lblIntervalUnit.Text = "秒間";
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(518, 121);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(90, 23);
            this.btnStartStop.TabIndex = 10;
            this.btnStartStop.Text = "開始";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.BtnStartStop_Click);
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(24, 167);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(63, 12);
            this.lblLog.TabIndex = 11;
            this.lblLog.Text = "送信ログ";
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(26, 189);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(582, 230);
            this.txtLog.TabIndex = 12;
            this.txtLog.Text = "";
            this.txtLog.WordWrap = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 441);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.lblIntervalUnit);
            this.Controls.Add(this.numInterval);
            this.Controls.Add(this.lblInterval);
            this.Controls.Add(this.cboStartFile);
            this.Controls.Add(this.lblStartFile);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtFolderPath);
            this.Controls.Add(this.lblFolder);
            this.Controls.Add(this.txtHulftId);
            this.Controls.Add(this.lblHulftId);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(650, 480);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HULFT 定期送信シミュレータ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblHulftId;
        private System.Windows.Forms.TextBox txtHulftId;
        private System.Windows.Forms.Label lblFolder;
        private System.Windows.Forms.TextBox txtFolderPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblStartFile;
        private System.Windows.Forms.ComboBox cboStartFile;
        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.NumericUpDown numInterval;
        private System.Windows.Forms.Label lblIntervalUnit;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.RichTextBox txtLog;
    }
}
