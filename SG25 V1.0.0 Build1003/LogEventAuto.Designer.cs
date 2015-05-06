namespace SG25
{
    partial class LogEventAuto
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.txtLogEventLog = new System.Windows.Forms.TextBox();
            this.DGVLogList = new System.Windows.Forms.DataGridView();
            this.btnExit = new System.Windows.Forms.Button();
            this.groupBox14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVLogList)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.txtLogEventLog);
            this.groupBox14.Controls.Add(this.DGVLogList);
            this.groupBox14.Location = new System.Drawing.Point(12, 12);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(296, 357);
            this.groupBox14.TabIndex = 241;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "groupBox14";
            // 
            // txtLogEventLog
            // 
            this.txtLogEventLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLogEventLog.ForeColor = System.Drawing.Color.Navy;
            this.txtLogEventLog.Location = new System.Drawing.Point(7, 292);
            this.txtLogEventLog.Multiline = true;
            this.txtLogEventLog.Name = "txtLogEventLog";
            this.txtLogEventLog.Size = new System.Drawing.Size(283, 43);
            this.txtLogEventLog.TabIndex = 1;
            this.txtLogEventLog.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // DGVLogList
            // 
            this.DGVLogList.AllowUserToAddRows = false;
            this.DGVLogList.AllowUserToDeleteRows = false;
            this.DGVLogList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGVLogList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.DGVLogList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.DGVLogList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.DGVLogList.EnableHeadersVisualStyles = false;
            this.DGVLogList.Location = new System.Drawing.Point(7, 19);
            this.DGVLogList.Name = "DGVLogList";
            this.DGVLogList.RowHeadersVisible = false;
            this.DGVLogList.Size = new System.Drawing.Size(283, 271);
            this.DGVLogList.TabIndex = 0;
            this.DGVLogList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVLogList_CellDoubleClick);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(115, 389);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 242;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // LogEventAuto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 424);
            this.ControlBox = false;
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.groupBox14);
            this.Name = "LogEventAuto";
            this.Text = "LogEventAuto";
            this.Load += new System.EventHandler(this.LogEventAuto_Load);
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVLogList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.TextBox txtLogEventLog;
        private System.Windows.Forms.DataGridView DGVLogList;
        private System.Windows.Forms.Button btnExit;
    }
}