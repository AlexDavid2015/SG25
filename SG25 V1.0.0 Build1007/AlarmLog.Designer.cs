namespace SG25
{
    partial class AlarmLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlarmLog));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.ToolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.BN1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.AlarmLogTableBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.setupDataSet = new SG25.SetupDataSet();
            this.BindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.BindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.BindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.BindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.BindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.BindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.BindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.BindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.BindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.setupDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.alarmLogTableTableAdapter = new SG25.SetupDataSetTableAdapters.AlarmLogTableTableAdapter();
            this.DGV = new System.Windows.Forms.DataGridView();
            this.alarmCurrentPDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alarmDateTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alarmNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alarmDescriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alarmHintDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TextBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.BN1)).BeginInit();
            this.BN1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AlarmLogTableBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setupDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setupDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.SuspendLayout();
            // 
            // Timer1
            // 
            this.Timer1.Interval = 1000;
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // ToolStripSeparator9
            // 
            this.ToolStripSeparator9.Name = "ToolStripSeparator9";
            this.ToolStripSeparator9.Size = new System.Drawing.Size(6, 100);
            // 
            // ToolStripButton7
            // 
            this.ToolStripButton7.AutoSize = false;
            this.ToolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolStripButton7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.ToolStripButton7.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripButton7.Image")));
            this.ToolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButton7.Name = "ToolStripButton7";
            this.ToolStripButton7.Size = new System.Drawing.Size(97, 97);
            this.ToolStripButton7.Text = "Close File";
            this.ToolStripButton7.Click += new System.EventHandler(this.ToolStripButton7_Click);
            // 
            // ToolStripSeparator8
            // 
            this.ToolStripSeparator8.Name = "ToolStripSeparator8";
            this.ToolStripSeparator8.Size = new System.Drawing.Size(6, 100);
            // 
            // ToolStripButton5
            // 
            this.ToolStripButton5.AutoSize = false;
            this.ToolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolStripButton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.ToolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripButton5.Image")));
            this.ToolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButton5.Name = "ToolStripButton5";
            this.ToolStripButton5.Size = new System.Drawing.Size(97, 97);
            this.ToolStripButton5.Text = "Open Log File";
            this.ToolStripButton5.Click += new System.EventHandler(this.ToolStripButton5_Click);
            // 
            // ToolStripSeparator5
            // 
            this.ToolStripSeparator5.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ToolStripSeparator5.Name = "ToolStripSeparator5";
            this.ToolStripSeparator5.Size = new System.Drawing.Size(6, 100);
            // 
            // ToolStripButton2
            // 
            this.ToolStripButton2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ToolStripButton2.AutoSize = false;
            this.ToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolStripButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ToolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripButton2.Image")));
            this.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButton2.Name = "ToolStripButton2";
            this.ToolStripButton2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ToolStripButton2.Size = new System.Drawing.Size(97, 97);
            this.ToolStripButton2.Text = "EXIT";
            this.ToolStripButton2.Click += new System.EventHandler(this.ToolStripButton2_Click);
            // 
            // ToolStripSeparator4
            // 
            this.ToolStripSeparator4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ToolStripSeparator4.Name = "ToolStripSeparator4";
            this.ToolStripSeparator4.Size = new System.Drawing.Size(6, 100);
            // 
            // ToolStripSeparator7
            // 
            this.ToolStripSeparator7.Name = "ToolStripSeparator7";
            this.ToolStripSeparator7.Size = new System.Drawing.Size(6, 100);
            // 
            // ToolStripSeparator6
            // 
            this.ToolStripSeparator6.Name = "ToolStripSeparator6";
            this.ToolStripSeparator6.Size = new System.Drawing.Size(6, 100);
            // 
            // ToolStripButton4
            // 
            this.ToolStripButton4.AutoSize = false;
            this.ToolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripButton4.Image")));
            this.ToolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButton4.Name = "ToolStripButton4";
            this.ToolStripButton4.Size = new System.Drawing.Size(97, 97);
            this.ToolStripButton4.Text = "Fast Up";
            this.ToolStripButton4.Click += new System.EventHandler(this.ToolStripButton4_Click);
            // 
            // ToolStripSeparator3
            // 
            this.ToolStripSeparator3.Name = "ToolStripSeparator3";
            this.ToolStripSeparator3.Size = new System.Drawing.Size(6, 100);
            // 
            // ToolStripButton3
            // 
            this.ToolStripButton3.AutoSize = false;
            this.ToolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripButton3.Image")));
            this.ToolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButton3.Name = "ToolStripButton3";
            this.ToolStripButton3.Size = new System.Drawing.Size(97, 97);
            this.ToolStripButton3.Text = "Fast Down";
            this.ToolStripButton3.Click += new System.EventHandler(this.ToolStripButton3_Click);
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(6, 100);
            // 
            // BN1
            // 
            this.BN1.AddNewItem = null;
            this.BN1.AutoSize = false;
            this.BN1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.BN1.BindingSource = this.AlarmLogTableBindingSource;
            this.BN1.CountItem = this.BindingNavigatorCountItem;
            this.BN1.DeleteItem = null;
            this.BN1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BN1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BindingNavigatorMoveFirstItem,
            this.ToolStripSeparator2,
            this.ToolStripButton3,
            this.BindingNavigatorSeparator,
            this.BindingNavigatorMovePreviousItem,
            this.BindingNavigatorSeparator1,
            this.BindingNavigatorPositionItem,
            this.BindingNavigatorCountItem,
            this.BindingNavigatorSeparator2,
            this.BindingNavigatorMoveNextItem,
            this.ToolStripSeparator3,
            this.ToolStripButton4,
            this.ToolStripSeparator6,
            this.BindingNavigatorMoveLastItem,
            this.ToolStripSeparator7,
            this.ToolStripSeparator4,
            this.ToolStripButton2,
            this.ToolStripSeparator5,
            this.ToolStripButton5,
            this.ToolStripSeparator8,
            this.ToolStripButton7,
            this.ToolStripSeparator9});
            this.BN1.Location = new System.Drawing.Point(0, 765);
            this.BN1.MoveFirstItem = this.BindingNavigatorMoveFirstItem;
            this.BN1.MoveLastItem = this.BindingNavigatorMoveLastItem;
            this.BN1.MoveNextItem = this.BindingNavigatorMoveNextItem;
            this.BN1.MovePreviousItem = this.BindingNavigatorMovePreviousItem;
            this.BN1.Name = "BN1";
            this.BN1.PositionItem = this.BindingNavigatorPositionItem;
            this.BN1.Size = new System.Drawing.Size(1262, 100);
            this.BN1.TabIndex = 4;
            this.BN1.Text = "BindingNavigator1";
            // 
            // AlarmLogTableBindingSource
            // 
            this.AlarmLogTableBindingSource.DataMember = "AlarmLogTable";
            this.AlarmLogTableBindingSource.DataSource = this.setupDataSet;
            // 
            // setupDataSet
            // 
            this.setupDataSet.DataSetName = "SetupDataSet";
            this.setupDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // BindingNavigatorCountItem
            // 
            this.BindingNavigatorCountItem.Name = "BindingNavigatorCountItem";
            this.BindingNavigatorCountItem.Size = new System.Drawing.Size(35, 97);
            this.BindingNavigatorCountItem.Text = "of {0}";
            this.BindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // BindingNavigatorMoveFirstItem
            // 
            this.BindingNavigatorMoveFirstItem.AutoSize = false;
            this.BindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("BindingNavigatorMoveFirstItem.Image")));
            this.BindingNavigatorMoveFirstItem.Name = "BindingNavigatorMoveFirstItem";
            this.BindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.BindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(97, 97);
            this.BindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // BindingNavigatorSeparator
            // 
            this.BindingNavigatorSeparator.Name = "BindingNavigatorSeparator";
            this.BindingNavigatorSeparator.Size = new System.Drawing.Size(6, 100);
            // 
            // BindingNavigatorMovePreviousItem
            // 
            this.BindingNavigatorMovePreviousItem.AutoSize = false;
            this.BindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("BindingNavigatorMovePreviousItem.Image")));
            this.BindingNavigatorMovePreviousItem.Name = "BindingNavigatorMovePreviousItem";
            this.BindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.BindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(97, 97);
            this.BindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // BindingNavigatorSeparator1
            // 
            this.BindingNavigatorSeparator1.Name = "BindingNavigatorSeparator1";
            this.BindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 100);
            // 
            // BindingNavigatorPositionItem
            // 
            this.BindingNavigatorPositionItem.AccessibleName = "Position";
            this.BindingNavigatorPositionItem.AutoSize = false;
            this.BindingNavigatorPositionItem.Name = "BindingNavigatorPositionItem";
            this.BindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 22);
            this.BindingNavigatorPositionItem.Text = "0";
            this.BindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // BindingNavigatorSeparator2
            // 
            this.BindingNavigatorSeparator2.Name = "BindingNavigatorSeparator2";
            this.BindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 100);
            // 
            // BindingNavigatorMoveNextItem
            // 
            this.BindingNavigatorMoveNextItem.AutoSize = false;
            this.BindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("BindingNavigatorMoveNextItem.Image")));
            this.BindingNavigatorMoveNextItem.Name = "BindingNavigatorMoveNextItem";
            this.BindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.BindingNavigatorMoveNextItem.Size = new System.Drawing.Size(97, 97);
            this.BindingNavigatorMoveNextItem.Text = "Move next";
         
            // 
            // BindingNavigatorMoveLastItem
            // 
            this.BindingNavigatorMoveLastItem.AutoSize = false;
            this.BindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("BindingNavigatorMoveLastItem.Image")));
            this.BindingNavigatorMoveLastItem.Name = "BindingNavigatorMoveLastItem";
            this.BindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.BindingNavigatorMoveLastItem.Size = new System.Drawing.Size(97, 97);
            this.BindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // alarmLogTableTableAdapter
            // 
            this.alarmLogTableTableAdapter.ClearBeforeFill = true;
            // 
            // DGV
            // 
            this.DGV.AllowUserToAddRows = false;
            this.DGV.AllowUserToDeleteRows = false;
            this.DGV.AutoGenerateColumns = false;
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.alarmCurrentPDataGridViewTextBoxColumn,
            this.alarmDateTimeDataGridViewTextBoxColumn,
            this.alarmNumberDataGridViewTextBoxColumn,
            this.alarmDescriptionDataGridViewTextBoxColumn,
            this.alarmHintDataGridViewTextBoxColumn});
            this.DGV.DataSource = this.AlarmLogTableBindingSource;
            this.DGV.Location = new System.Drawing.Point(0, 0);
            this.DGV.Name = "DGV";
            this.DGV.ReadOnly = true;
            this.DGV.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV.Size = new System.Drawing.Size(1262, 460);
            this.DGV.TabIndex = 1;
            // 
            // alarmCurrentPDataGridViewTextBoxColumn
            // 
            this.alarmCurrentPDataGridViewTextBoxColumn.DataPropertyName = "AlarmCurrentP";
            this.alarmCurrentPDataGridViewTextBoxColumn.HeaderText = "Program";
            this.alarmCurrentPDataGridViewTextBoxColumn.Name = "alarmCurrentPDataGridViewTextBoxColumn";
            this.alarmCurrentPDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // alarmDateTimeDataGridViewTextBoxColumn
            // 
            this.alarmDateTimeDataGridViewTextBoxColumn.DataPropertyName = "AlarmDateTime";
            dataGridViewCellStyle2.Format = "F";
            dataGridViewCellStyle2.NullValue = null;
            this.alarmDateTimeDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.alarmDateTimeDataGridViewTextBoxColumn.HeaderText = "DateTime";
            this.alarmDateTimeDataGridViewTextBoxColumn.Name = "alarmDateTimeDataGridViewTextBoxColumn";
            this.alarmDateTimeDataGridViewTextBoxColumn.ReadOnly = true;
            this.alarmDateTimeDataGridViewTextBoxColumn.Width = 200;
            // 
            // alarmNumberDataGridViewTextBoxColumn
            // 
            this.alarmNumberDataGridViewTextBoxColumn.DataPropertyName = "AlarmNumber";
            this.alarmNumberDataGridViewTextBoxColumn.HeaderText = "Alarm #";
            this.alarmNumberDataGridViewTextBoxColumn.Name = "alarmNumberDataGridViewTextBoxColumn";
            this.alarmNumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.alarmNumberDataGridViewTextBoxColumn.Width = 50;
            // 
            // alarmDescriptionDataGridViewTextBoxColumn
            // 
            this.alarmDescriptionDataGridViewTextBoxColumn.DataPropertyName = "AlarmDescription";
            this.alarmDescriptionDataGridViewTextBoxColumn.HeaderText = "Alarm Description";
            this.alarmDescriptionDataGridViewTextBoxColumn.Name = "alarmDescriptionDataGridViewTextBoxColumn";
            this.alarmDescriptionDataGridViewTextBoxColumn.ReadOnly = true;
            this.alarmDescriptionDataGridViewTextBoxColumn.Width = 150;
            // 
            // alarmHintDataGridViewTextBoxColumn
            // 
            this.alarmHintDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.alarmHintDataGridViewTextBoxColumn.DataPropertyName = "AlarmHint";
            this.alarmHintDataGridViewTextBoxColumn.HeaderText = "Alarm Hint";
            this.alarmHintDataGridViewTextBoxColumn.Name = "alarmHintDataGridViewTextBoxColumn";
            this.alarmHintDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // TextBox1
            // 
            this.TextBox1.Location = new System.Drawing.Point(0, 455);
            this.TextBox1.Multiline = true;
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBox1.Size = new System.Drawing.Size(1278, 307);
            this.TextBox1.TabIndex = 9;
            // 
            // AlarmLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 865);
            this.ControlBox = false;
            this.Controls.Add(this.TextBox1);
            this.Controls.Add(this.DGV);
            this.Controls.Add(this.BN1);
            this.Name = "AlarmLog";
            this.Text = "AlarmLogTable";
            this.Load += new System.EventHandler(this.AlarmLog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BN1)).EndInit();
            this.BN1.ResumeLayout(false);
            this.BN1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AlarmLogTableBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setupDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setupDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Timer Timer1;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator9;
        internal System.Windows.Forms.ToolStripButton ToolStripButton7;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator8;
        internal System.Windows.Forms.ToolStripButton ToolStripButton5;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator5;
        internal System.Windows.Forms.ToolStripButton ToolStripButton2;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator4;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator7;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator6;
        internal System.Windows.Forms.ToolStripButton ToolStripButton4;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator3;
        internal System.Windows.Forms.ToolStripButton ToolStripButton3;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
        internal System.Windows.Forms.BindingNavigator BN1;
        internal System.Windows.Forms.ToolStripLabel BindingNavigatorCountItem;
        internal System.Windows.Forms.ToolStripButton BindingNavigatorMoveFirstItem;
        internal System.Windows.Forms.ToolStripSeparator BindingNavigatorSeparator;
        internal System.Windows.Forms.ToolStripButton BindingNavigatorMovePreviousItem;
        internal System.Windows.Forms.ToolStripSeparator BindingNavigatorSeparator1;
        internal System.Windows.Forms.ToolStripTextBox BindingNavigatorPositionItem;
        internal System.Windows.Forms.ToolStripSeparator BindingNavigatorSeparator2;
        internal System.Windows.Forms.ToolStripButton BindingNavigatorMoveNextItem;
        internal System.Windows.Forms.ToolStripButton BindingNavigatorMoveLastItem;
       
        private SetupDataSet setupDataSet;
        private System.Windows.Forms.BindingSource setupDataSetBindingSource;
        internal System.Windows.Forms.BindingSource AlarmLogTableBindingSource;
        private SetupDataSetTableAdapters.AlarmLogTableTableAdapter alarmLogTableTableAdapter;
        private System.Windows.Forms.DataGridView DGV;
        private System.Windows.Forms.DataGridViewTextBoxColumn alarmCurrentPDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn alarmDateTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn alarmNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn alarmDescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn alarmHintDataGridViewTextBoxColumn;
        internal System.Windows.Forms.TextBox TextBox1;
    }
}