namespace SG25
{
    partial class Splash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Splash));
            this.Label3 = new System.Windows.Forms.Label();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.PictureBox2 = new System.Windows.Forms.PictureBox();
            this.Button2 = new System.Windows.Forms.Button();
            this.Label2 = new System.Windows.Forms.Label();
            this.TextBox2 = new System.Windows.Forms.TextBox();
            this.Button1 = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.Version = new System.Windows.Forms.Label();
            this.LB = new System.Windows.Forms.ListBox();
            this.UsersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.setupDataSet = new SG25.SetupDataSet();
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.usersTableAdapter = new SG25.SetupDataSetTableAdapters.UsersTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UsersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setupDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.ForeColor = System.Drawing.Color.White;
            this.Label3.Location = new System.Drawing.Point(252, 422);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(90, 24);
            this.Label3.TabIndex = 21;
            this.Label3.Text = "Copyright";
            this.Label3.Click += new System.EventHandler(this.Label3_Click);
            // 
            // PictureBox1
            // 
            this.PictureBox1.BackColor = System.Drawing.Color.White;
            this.PictureBox1.Image = global::SG25.Properties.Resources.ISO_Perspective_View_ON;
            this.PictureBox1.Location = new System.Drawing.Point(102, 12);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(240, 384);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox1.TabIndex = 19;
            this.PictureBox1.TabStop = false;
            // 
            // PictureBox2
            // 
            this.PictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.PictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox2.Image")));
            this.PictureBox2.Location = new System.Drawing.Point(12, 12);
            this.PictureBox2.Name = "PictureBox2";
            this.PictureBox2.Size = new System.Drawing.Size(96, 384);
            this.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox2.TabIndex = 20;
            this.PictureBox2.TabStop = false;
            // 
            // Button2
            // 
            this.Button2.Location = new System.Drawing.Point(533, 467);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(121, 99);
            this.Button2.TabIndex = 17;
            this.Button2.Text = "Exit";
            this.Button2.UseVisualStyleBackColor = true;
            this.Button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.ForeColor = System.Drawing.Color.White;
            this.Label2.Location = new System.Drawing.Point(348, 334);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(53, 13);
            this.Label2.TabIndex = 16;
            this.Label2.Text = "Password";
            // 
            // TextBox2
            // 
            this.TextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox2.Location = new System.Drawing.Point(347, 349);
            this.TextBox2.MaxLength = 8;
            this.TextBox2.Name = "TextBox2";
            this.TextBox2.PasswordChar = '*';
            this.TextBox2.Size = new System.Drawing.Size(307, 47);
            this.TextBox2.TabIndex = 15;
            this.TextBox2.Click += new System.EventHandler(this.TextBox2_Click);
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(12, 466);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(498, 100);
            this.Button1.TabIndex = 14;
            this.Button1.Text = "Enter";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.ForeColor = System.Drawing.Color.White;
            this.Label1.Location = new System.Drawing.Point(348, 295);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(74, 13);
            this.Label1.TabIndex = 13;
            this.Label1.Text = "Selected User";
            // 
            // Version
            // 
            this.Version.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Version.AutoSize = true;
            this.Version.BackColor = System.Drawing.Color.Transparent;
            this.Version.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Version.ForeColor = System.Drawing.Color.White;
            this.Version.Location = new System.Drawing.Point(12, 422);
            this.Version.Name = "Version";
            this.Version.Size = new System.Drawing.Size(75, 24);
            this.Version.TabIndex = 22;
            this.Version.Text = "Version";
            this.Version.Click += new System.EventHandler(this.Version_Click);
            // 
            // LB
            // 
            this.LB.DataSource = this.UsersBindingSource;
            this.LB.DisplayMember = "UUser";
            this.LB.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F);
            this.LB.FormattingEnabled = true;
            this.LB.ItemHeight = 25;
            this.LB.Location = new System.Drawing.Point(351, 13);
            this.LB.Name = "LB";
            this.LB.ScrollAlwaysVisible = true;
            this.LB.Size = new System.Drawing.Size(275, 254);
            this.LB.TabIndex = 25;
            this.LB.Click += new System.EventHandler(this.LB_Click);
            // 
            // UsersBindingSource
            // 
            this.UsersBindingSource.DataMember = "Users";
            this.UsersBindingSource.DataSource = this.setupDataSet;
            // 
            // setupDataSet
            // 
            this.setupDataSet.DataSetName = "SetupDataSet";
            this.setupDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // TextBox1
            // 
            this.TextBox1.Location = new System.Drawing.Point(348, 311);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.Size = new System.Drawing.Size(100, 20);
            this.TextBox1.TabIndex = 26;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            // 
            // usersTableAdapter
            // 
            this.usersTableAdapter.ClearBeforeFill = true;
            // 
            // Splash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(670, 579);
            this.ControlBox = false;
            this.Controls.Add(this.PictureBox2);
            this.Controls.Add(this.TextBox1);
            this.Controls.Add(this.LB);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.PictureBox1);
            this.Controls.Add(this.Button2);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.TextBox2);
            this.Controls.Add(this.Button1);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.Version);
            this.Name = "Splash";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SCI Automation SG25";
            this.Load += new System.EventHandler(this.Splash_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Splash_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UsersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setupDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.PictureBox PictureBox1;
        internal System.Windows.Forms.PictureBox PictureBox2;
        internal System.Windows.Forms.Button Button2;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox TextBox2;
        internal System.Windows.Forms.Button Button1;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Label Version;
        private System.Windows.Forms.ListBox LB;
        private System.Windows.Forms.TextBox TextBox1;
        private System.Windows.Forms.Timer timer1;
        private SetupDataSet setupDataSet;
        private SetupDataSetTableAdapters.UsersTableAdapter usersTableAdapter;
        internal System.Windows.Forms.BindingSource UsersBindingSource;
    }
}