namespace SG25
{
    partial class RFTimeCont
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
            this.components = new System.ComponentModel.Container();
            this.cmdContinue = new System.Windows.Forms.Button();
            this.cmdStartOver = new System.Windows.Forms.Button();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmdContinue
            // 
            this.cmdContinue.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cmdContinue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdContinue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdContinue.Location = new System.Drawing.Point(94, 103);
            this.cmdContinue.Name = "cmdContinue";
            this.cmdContinue.Size = new System.Drawing.Size(153, 65);
            this.cmdContinue.TabIndex = 27;
            this.cmdContinue.Text = "Continue";
            this.cmdContinue.UseVisualStyleBackColor = false;
            this.cmdContinue.Click += new System.EventHandler(this.cmdContinue_Click);
            // 
            // cmdStartOver
            // 
            this.cmdStartOver.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cmdStartOver.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdStartOver.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdStartOver.Location = new System.Drawing.Point(342, 103);
            this.cmdStartOver.Name = "cmdStartOver";
            this.cmdStartOver.Size = new System.Drawing.Size(153, 65);
            this.cmdStartOver.TabIndex = 28;
            this.cmdStartOver.Text = "Start Over";
            this.cmdStartOver.UseVisualStyleBackColor = false;
            this.cmdStartOver.Click += new System.EventHandler(this.cmdStartOver_Click);
            // 
            // Timer1
            // 
            this.Timer1.Interval = 1000;
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(32, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(563, 20);
            this.label1.TabIndex = 29;
            this.label1.Text = "Plasma Process Stopped in the middle, Do you want to continue or Start over ?";
            // 
            // RFTimeCont
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(607, 196);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdStartOver);
            this.Controls.Add(this.cmdContinue);
            this.Name = "RFTimeCont";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MsgResidentialTimePage";
            this.Load += new System.EventHandler(this.MsgResidentialTimePage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button cmdContinue;
        internal System.Windows.Forms.Button cmdStartOver;
        private System.Windows.Forms.Timer Timer1;
        private System.Windows.Forms.Label label1;
    }
}