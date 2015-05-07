namespace SG25
{
    partial class Mode
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
            this.Button7 = new System.Windows.Forms.Button();
            this.BPrograms = new System.Windows.Forms.Button();
            this.Button5 = new System.Windows.Forms.Button();
            this.BUsers = new System.Windows.Forms.Button();
            this.BSetup = new System.Windows.Forms.Button();
            this.BManual = new System.Windows.Forms.Button();
            this.BAuto = new System.Windows.Forms.Button();
            this.chkByPassMode = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // Button7
            // 
            this.Button7.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button7.Location = new System.Drawing.Point(12, 296);
            this.Button7.Name = "Button7";
            this.Button7.Size = new System.Drawing.Size(244, 138);
            this.Button7.TabIndex = 13;
            this.Button7.Text = "Alarm Log";
            this.Button7.UseVisualStyleBackColor = true;
            this.Button7.Click += new System.EventHandler(this.Button7_Click);
            // 
            // BPrograms
            // 
            this.BPrograms.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BPrograms.Location = new System.Drawing.Point(262, 296);
            this.BPrograms.Name = "BPrograms";
            this.BPrograms.Size = new System.Drawing.Size(244, 138);
            this.BPrograms.TabIndex = 12;
            this.BPrograms.Text = "Programs";
            this.BPrograms.UseVisualStyleBackColor = true;
            this.BPrograms.Click += new System.EventHandler(this.BPrograms_Click);
            // 
            // Button5
            // 
            this.Button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button5.Location = new System.Drawing.Point(12, 440);
            this.Button5.Name = "Button5";
            this.Button5.Size = new System.Drawing.Size(494, 138);
            this.Button5.TabIndex = 11;
            this.Button5.Text = "Log Off && Exit";
            this.Button5.UseVisualStyleBackColor = true;
            this.Button5.Click += new System.EventHandler(this.Button5_Click);
            // 
            // BUsers
            // 
            this.BUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BUsers.Location = new System.Drawing.Point(262, 8);
            this.BUsers.Name = "BUsers";
            this.BUsers.Size = new System.Drawing.Size(244, 138);
            this.BUsers.TabIndex = 10;
            this.BUsers.Text = "Users";
            this.BUsers.UseVisualStyleBackColor = true;
            this.BUsers.Click += new System.EventHandler(this.BUsers_Click);
            // 
            // BSetup
            // 
            this.BSetup.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BSetup.Location = new System.Drawing.Point(262, 152);
            this.BSetup.Name = "BSetup";
            this.BSetup.Size = new System.Drawing.Size(244, 138);
            this.BSetup.TabIndex = 9;
            this.BSetup.Text = "Setup";
            this.BSetup.UseVisualStyleBackColor = true;
            this.BSetup.Click += new System.EventHandler(this.BSetup_Click);
            // 
            // BManual
            // 
            this.BManual.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BManual.Location = new System.Drawing.Point(12, 152);
            this.BManual.Name = "BManual";
            this.BManual.Size = new System.Drawing.Size(244, 138);
            this.BManual.TabIndex = 8;
            this.BManual.Text = "Manual";
            this.BManual.UseVisualStyleBackColor = true;
            this.BManual.Click += new System.EventHandler(this.BManual_Click);
            // 
            // BAuto
            // 
            this.BAuto.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BAuto.Location = new System.Drawing.Point(12, 8);
            this.BAuto.Name = "BAuto";
            this.BAuto.Size = new System.Drawing.Size(244, 138);
            this.BAuto.TabIndex = 7;
            this.BAuto.Text = "Auto";
            this.BAuto.UseVisualStyleBackColor = true;
            this.BAuto.Click += new System.EventHandler(this.BAuto_Click);
            // 
            // chkByPassMode
            // 
            this.chkByPassMode.AutoSize = true;
            this.chkByPassMode.BackColor = System.Drawing.SystemColors.ControlLight;
            this.chkByPassMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkByPassMode.Location = new System.Drawing.Point(25, 115);
            this.chkByPassMode.Name = "chkByPassMode";
            this.chkByPassMode.Size = new System.Drawing.Size(118, 22);
            this.chkByPassMode.TabIndex = 15;
            this.chkByPassMode.Text = "Bypass Mode";
            this.chkByPassMode.UseVisualStyleBackColor = false;
            this.chkByPassMode.Visible = false;
            this.chkByPassMode.CheckedChanged += new System.EventHandler(this.chkByPassMode_CheckedChanged);
            // 
            // Mode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 587);
            this.ControlBox = false;
            this.Controls.Add(this.chkByPassMode);
            this.Controls.Add(this.Button7);
            this.Controls.Add(this.BPrograms);
            this.Controls.Add(this.Button5);
            this.Controls.Add(this.BUsers);
            this.Controls.Add(this.BSetup);
            this.Controls.Add(this.BManual);
            this.Controls.Add(this.BAuto);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Mode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mode";
            this.Activated += new System.EventHandler(this.Mode_Activated_1);
            this.Load += new System.EventHandler(this.Mode_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button Button7;
        internal System.Windows.Forms.Button BPrograms;
        internal System.Windows.Forms.Button Button5;
        internal System.Windows.Forms.Button BUsers;
        internal System.Windows.Forms.Button BSetup;
        internal System.Windows.Forms.Button BManual;
        internal System.Windows.Forms.Button BAuto;
        private System.Windows.Forms.CheckBox chkByPassMode;
     //   private SetupDataSet setup1;
    }
}