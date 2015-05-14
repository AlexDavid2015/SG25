namespace SG25
{
    partial class UserConnectionSelectionPage
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
            this.cmdContinue = new System.Windows.Forms.Button();
            this.cmdStartOver = new System.Windows.Forms.Button();
            this.cmdExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmdContinue
            // 
            this.cmdContinue.Location = new System.Drawing.Point(59, 12);
            this.cmdContinue.Name = "cmdContinue";
            this.cmdContinue.Size = new System.Drawing.Size(107, 71);
            this.cmdContinue.TabIndex = 1;
            this.cmdContinue.Text = "Continue";
            this.cmdContinue.UseVisualStyleBackColor = true;
            this.cmdContinue.Click += new System.EventHandler(this.cmdContinue_Click);
            // 
            // cmdStartOver
            // 
            this.cmdStartOver.Location = new System.Drawing.Point(248, 12);
            this.cmdStartOver.Name = "cmdStartOver";
            this.cmdStartOver.Size = new System.Drawing.Size(107, 71);
            this.cmdStartOver.TabIndex = 2;
            this.cmdStartOver.Text = "Start Over";
            this.cmdStartOver.UseVisualStyleBackColor = true;
            this.cmdStartOver.Click += new System.EventHandler(this.cmdStartOver_Click);
            // 
            // cmdExit
            // 
            this.cmdExit.Location = new System.Drawing.Point(439, 12);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.Size = new System.Drawing.Size(107, 71);
            this.cmdExit.TabIndex = 3;
            this.cmdExit.Text = "Exit";
            this.cmdExit.UseVisualStyleBackColor = true;
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // UserConnectionSelectionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 107);
            this.ControlBox = false;
            this.Controls.Add(this.cmdExit);
            this.Controls.Add(this.cmdStartOver);
            this.Controls.Add(this.cmdContinue);
            this.Name = "UserConnectionSelectionPage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Selection Connection Page";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button cmdContinue;
        internal System.Windows.Forms.Button cmdStartOver;
        internal System.Windows.Forms.Button cmdExit;
    }
}