namespace SG25
{
    partial class Users
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Users));
            this.Label5 = new System.Windows.Forms.Label();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.Label10 = new System.Windows.Forms.Label();
            this.Label9 = new System.Windows.Forms.Label();
            this.UserIDTXT = new System.Windows.Forms.TextBox();
            this.UsersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.setupDataSet = new SG25.SetupDataSet();
            this.Label7 = new System.Windows.Forms.Label();
            this.ProgramsLBL = new System.Windows.Forms.Label();
            this.SetupLBL = new System.Windows.Forms.Label();
            this.UsersLBL = new System.Windows.Forms.Label();
            this.ManualLBL = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.TextBox2 = new System.Windows.Forms.TextBox();
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripModify = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripAddNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.usersTableAdapter = new SG25.SetupDataSetTableAdapters.UsersTableAdapter();
            this.GraphLBL = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UsersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setupDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label5.ForeColor = System.Drawing.Color.White;
            this.Label5.Location = new System.Drawing.Point(946, -7);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(89, 31);
            this.Label5.TabIndex = 36;
            this.Label5.Text = "Graph";
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label10.ForeColor = System.Drawing.Color.White;
            this.Label10.Location = new System.Drawing.Point(932, 253);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(167, 31);
            this.Label10.TabIndex = 47;
            this.Label10.Text = "Stored Index";
            this.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Label10.Visible = false;
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label9.ForeColor = System.Drawing.Color.White;
            this.Label9.Location = new System.Drawing.Point(946, 87);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(80, 31);
            this.Label9.TabIndex = 46;
            this.Label9.Text = "Index";
            // 
            // UserIDTXT
            // 
            this.UserIDTXT.AcceptsReturn = true;
            this.UserIDTXT.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.UsersBindingSource, "ID", true));
            this.UserIDTXT.Enabled = false;
            this.UserIDTXT.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserIDTXT.Location = new System.Drawing.Point(944, 122);
            this.UserIDTXT.Name = "UserIDTXT";
            this.UserIDTXT.Size = new System.Drawing.Size(133, 38);
            this.UserIDTXT.TabIndex = 45;
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
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label7.ForeColor = System.Drawing.Color.White;
            this.Label7.Location = new System.Drawing.Point(292, 209);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(85, 31);
            this.Label7.TabIndex = 38;
            this.Label7.Text = "Setup";
            // 
            // ProgramsLBL
            // 
            this.ProgramsLBL.BackColor = System.Drawing.Color.PapayaWhip;
            this.ProgramsLBL.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ProgramsLBL.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.UsersBindingSource, "Programs", true));
            this.ProgramsLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProgramsLBL.ForeColor = System.Drawing.Color.Black;
            this.ProgramsLBL.Location = new System.Drawing.Point(429, 251);
            this.ProgramsLBL.Name = "ProgramsLBL";
            this.ProgramsLBL.Size = new System.Drawing.Size(133, 33);
            this.ProgramsLBL.TabIndex = 44;
            this.ProgramsLBL.Text = "Programs";
            this.ProgramsLBL.Click += new System.EventHandler(this.ProgramsLBL_Click);
            // 
            // SetupLBL
            // 
            this.SetupLBL.BackColor = System.Drawing.Color.PapayaWhip;
            this.SetupLBL.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.SetupLBL.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.UsersBindingSource, "Setup", true));
            this.SetupLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SetupLBL.ForeColor = System.Drawing.Color.Black;
            this.SetupLBL.Location = new System.Drawing.Point(290, 251);
            this.SetupLBL.Name = "SetupLBL";
            this.SetupLBL.Size = new System.Drawing.Size(133, 33);
            this.SetupLBL.TabIndex = 43;
            this.SetupLBL.Text = "Setup";
            this.SetupLBL.Click += new System.EventHandler(this.SetupLBL_Click);
            // 
            // UsersLBL
            // 
            this.UsersLBL.BackColor = System.Drawing.Color.PapayaWhip;
            this.UsersLBL.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.UsersLBL.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.UsersBindingSource, "Users", true));
            this.UsersLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsersLBL.ForeColor = System.Drawing.Color.Black;
            this.UsersLBL.Location = new System.Drawing.Point(151, 251);
            this.UsersLBL.Name = "UsersLBL";
            this.UsersLBL.Size = new System.Drawing.Size(133, 33);
            this.UsersLBL.TabIndex = 42;
            this.UsersLBL.Text = "Users";
            this.UsersLBL.Click += new System.EventHandler(this.UsersLBL_Click);
            // 
            // ManualLBL
            // 
            this.ManualLBL.BackColor = System.Drawing.Color.PapayaWhip;
            this.ManualLBL.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ManualLBL.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.UsersBindingSource, "Manual", true));
            this.ManualLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ManualLBL.ForeColor = System.Drawing.Color.Black;
            this.ManualLBL.Location = new System.Drawing.Point(12, 251);
            this.ManualLBL.Name = "ManualLBL";
            this.ManualLBL.Size = new System.Drawing.Size(133, 33);
            this.ManualLBL.TabIndex = 40;
            this.ManualLBL.Text = "Manual";
            this.ManualLBL.Click += new System.EventHandler(this.ManualLBL_Click);
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.ForeColor = System.Drawing.Color.White;
            this.Label8.Location = new System.Drawing.Point(431, 209);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(131, 31);
            this.Label8.TabIndex = 39;
            this.Label8.Text = "Programs";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.ForeColor = System.Drawing.Color.White;
            this.Label6.Location = new System.Drawing.Point(153, 209);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(86, 31);
            this.Label6.TabIndex = 37;
            this.Label6.Text = "Users";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.ForeColor = System.Drawing.Color.White;
            this.Label4.Location = new System.Drawing.Point(14, 209);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(102, 31);
            this.Label4.TabIndex = 35;
            this.Label4.Text = "Manual";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.ForeColor = System.Drawing.Color.White;
            this.Label2.Location = new System.Drawing.Point(470, 87);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(134, 31);
            this.Label2.TabIndex = 32;
            this.Label2.Text = "Password";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.ForeColor = System.Drawing.Color.White;
            this.Label1.Location = new System.Drawing.Point(14, 87);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(72, 31);
            this.Label1.TabIndex = 31;
            this.Label1.Text = "User";
            // 
            // TextBox2
            // 
            this.TextBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.UsersBindingSource, "Password", true));
            this.TextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox2.Location = new System.Drawing.Point(468, 122);
            this.TextBox2.Name = "TextBox2";
            this.TextBox2.Size = new System.Drawing.Size(450, 38);
            this.TextBox2.TabIndex = 30;
            this.TextBox2.Click += new System.EventHandler(this.TextBox2_Click);
            this.TextBox2.TextChanged += new System.EventHandler(this.TextBox2_TextChanged);
            // 
            // TextBox1
            // 
            this.TextBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.UsersBindingSource, "UUser", true));
            this.TextBox1.Enabled = false;
            this.TextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox1.Location = new System.Drawing.Point(12, 122);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.Size = new System.Drawing.Size(450, 38);
            this.TextBox1.TabIndex = 29;
            this.TextBox1.Click += new System.EventHandler(this.TextBox1_Click);
            this.TextBox1.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BindingSource = this.UsersBindingSource;
            this.bindingNavigator1.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.toolStripSeparator1,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.toolStripSeparator2,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.ToolStripModify,
            this.toolStripSeparator3,
            this.ToolStripAddNew,
            this.toolStripSeparator4,
            this.ToolStripDelete,
            this.toolStripSeparator5,
            this.ToolStripSave,
            this.toolStripSeparator6,
            this.ToolStripButton2});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 308);
            this.bindingNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigator1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigator1.Size = new System.Drawing.Size(1129, 103);
            this.bindingNavigator1.TabIndex = 51;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.AutoSize = false;
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 100);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.AutoSize = false;
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(100, 100);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 103);
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.AutoSize = false;
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(100, 100);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 103);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(100, 100);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 103);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.AutoSize = false;
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(100, 100);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 103);
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.AutoSize = false;
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(100, 100);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 103);
            // 
            // ToolStripModify
            // 
            this.ToolStripModify.AutoSize = false;
            this.ToolStripModify.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolStripModify.Enabled = false;
            this.ToolStripModify.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.ToolStripModify.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripModify.Image")));
            this.ToolStripModify.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripModify.Name = "ToolStripModify";
            this.ToolStripModify.Size = new System.Drawing.Size(100, 100);
            this.ToolStripModify.Text = "Modify";
            this.ToolStripModify.Click += new System.EventHandler(this.ToolStripModify_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 103);
            // 
            // ToolStripAddNew
            // 
            this.ToolStripAddNew.AutoSize = false;
            this.ToolStripAddNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolStripAddNew.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.ToolStripAddNew.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripAddNew.Image")));
            this.ToolStripAddNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripAddNew.Name = "ToolStripAddNew";
            this.ToolStripAddNew.Size = new System.Drawing.Size(100, 100);
            this.ToolStripAddNew.Text = "Add New";
            this.ToolStripAddNew.Click += new System.EventHandler(this.ToolStripAddNew_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 103);
            // 
            // ToolStripDelete
            // 
            this.ToolStripDelete.AutoSize = false;
            this.ToolStripDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolStripDelete.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.ToolStripDelete.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripDelete.Image")));
            this.ToolStripDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripDelete.Name = "ToolStripDelete";
            this.ToolStripDelete.Size = new System.Drawing.Size(100, 100);
            this.ToolStripDelete.Text = "Delete";
            this.ToolStripDelete.Click += new System.EventHandler(this.ToolStripDelete_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 103);
            // 
            // ToolStripSave
            // 
            this.ToolStripSave.AutoSize = false;
            this.ToolStripSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolStripSave.Enabled = false;
            this.ToolStripSave.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.ToolStripSave.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripSave.Image")));
            this.ToolStripSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripSave.Name = "ToolStripSave";
            this.ToolStripSave.Size = new System.Drawing.Size(100, 100);
            this.ToolStripSave.Text = "Save";
            this.ToolStripSave.Click += new System.EventHandler(this.ToolStripSave_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 103);
            // 
            // ToolStripButton2
            // 
            this.ToolStripButton2.AutoSize = false;
            this.ToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolStripButton2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.ToolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripButton2.Image")));
            this.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButton2.Name = "ToolStripButton2";
            this.ToolStripButton2.Size = new System.Drawing.Size(100, 100);
            this.ToolStripButton2.Text = "EXIT";
            this.ToolStripButton2.Click += new System.EventHandler(this.ToolStripButton2_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1129, 25);
            this.toolStrip1.TabIndex = 52;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // usersTableAdapter
            // 
            this.usersTableAdapter.ClearBeforeFill = true;
            // 
            // GraphLBL
            // 
            this.GraphLBL.BackColor = System.Drawing.Color.PapayaWhip;
            this.GraphLBL.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.GraphLBL.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.UsersBindingSource, "Programs", true));
            this.GraphLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GraphLBL.ForeColor = System.Drawing.Color.Black;
            this.GraphLBL.Location = new System.Drawing.Point(794, 45);
            this.GraphLBL.Name = "GraphLBL";
            this.GraphLBL.Size = new System.Drawing.Size(133, 33);
            this.GraphLBL.TabIndex = 53;
            this.GraphLBL.Text = "Graph";
            this.GraphLBL.Visible = false;
            // 
            // Users
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1129, 411);
            this.ControlBox = false;
            this.Controls.Add(this.GraphLBL);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.bindingNavigator1);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.Label10);
            this.Controls.Add(this.Label9);
            this.Controls.Add(this.UserIDTXT);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.ProgramsLBL);
            this.Controls.Add(this.SetupLBL);
            this.Controls.Add(this.UsersLBL);
            this.Controls.Add(this.ManualLBL);
            this.Controls.Add(this.Label8);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.TextBox2);
            this.Controls.Add(this.TextBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Users";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Users";
            this.Load += new System.EventHandler(this.Users_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UsersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setupDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label10;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.TextBox UserIDTXT;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.Label ProgramsLBL;
        internal System.Windows.Forms.Label SetupLBL;
        internal System.Windows.Forms.Label UsersLBL;
        internal System.Windows.Forms.Label ManualLBL;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox TextBox2;
        internal System.Windows.Forms.TextBox TextBox1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private SetupDataSet setupDataSet;
        internal System.Windows.Forms.BindingSource UsersBindingSource;
        internal SetupDataSetTableAdapters.UsersTableAdapter usersTableAdapter;
        internal System.Windows.Forms.BindingNavigator bindingNavigator1;
        internal System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        internal System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        internal System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        internal System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        internal System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        internal System.Windows.Forms.ToolStripButton ToolStripModify;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        internal System.Windows.Forms.ToolStripButton ToolStripAddNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton ToolStripDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        internal System.Windows.Forms.ToolStripButton ToolStripSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        internal System.Windows.Forms.ToolStripButton ToolStripButton2;
        internal System.Windows.Forms.Label GraphLBL;
    }
}