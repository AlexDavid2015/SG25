using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Common;
using SG25.SetupDataSetTableAdapters;

namespace SG25
{
    public partial class Users : Form
    {
        public Users()
        {
            InitializeComponent();
        }

        bool AddingNew;
        SetupDataSet setupDatasetObj = new SetupDataSet();
        SetupTableAdapter setupTAobj = new SetupTableAdapter();
        UsersTableAdapter userTAobj = new UsersTableAdapter();

        private void Users_Load(System.Object sender, System.EventArgs e)
        {
            // TODO: This line of code loads data into the 'setupDataSet.Users' table. You can move, or remove it, as needed.
            this.usersTableAdapter.Fill(this.setupDataSet.Users);
            int i = 0;

            setupTAobj.Fill(setupDatasetObj.Setup);
            userTAobj.Fill(setupDatasetObj.Users);
            Class1.UserIndex = (int)setupTAobj.SelectUserIDIndex();
            setupTAobj.Dispose();

            ToolStripModify.Enabled = false;
            ToolStripAddNew.Enabled = true;
            ToolStripDelete.Enabled = true;
            ToolStripSave.Enabled = false;
            ToolStripButton2.Enabled = true;
        }



       private void ManualLBL_Click(System.Object sender, System.EventArgs e)
        {
            SG25.UsersAuthor objUserAuthor = new SG25.UsersAuthor();
            objUserAuthor.ShowDialog();
            this.ManualLBL.Text =Class1.AllowedDenied;
            if (AddingNew == false)
                ModifyButtons();
        }

       

        private void UsersLBL_Click(System.Object sender, System.EventArgs e)
        {
            SG25.UsersAuthor objUsersAuthor = new SG25.UsersAuthor();
            objUsersAuthor.ShowDialog();
            this.UsersLBL.Text = Class1.AllowedDenied;
            if (AddingNew == false)
                ModifyButtons();
        }

        private void SetupLBL_Click(System.Object sender, System.EventArgs e)
        {
            SG25.UsersAuthor objUsersAuthor = new SG25.UsersAuthor();
            objUsersAuthor.ShowDialog();
            this.SetupLBL.Text = Class1.AllowedDenied;
            if (AddingNew == false)
                ModifyButtons();
        }

        private void ProgramsLBL_Click(System.Object sender, System.EventArgs e)
        {
            SG25.UsersAuthor objUsersAuthor = new SG25.UsersAuthor();
            objUsersAuthor.ShowDialog();
            this.ProgramsLBL.Text = Class1.AllowedDenied;
            if (AddingNew == false)
                ModifyButtons();
        }



        private void TextBox1_Click(System.Object sender, System.EventArgs e)
        {
            Class1.AlphaPadsend = TextBox1.Text;
            SG25.Alphapad objAlphapad = new SG25.Alphapad();
            objAlphapad.ShowDialog();
            TextBox1.Text =Class1.AlphaPadret;// you need this do not remove
                Class1.AlphaPadsend = TextBox1.Text;
            if (AddingNew == false)
                ModifyButtons();
        }

        private void TextBox2_Click(System.Object sender, System.EventArgs e)
        {
            Class1.AlphaPadsend = TextBox2.Text;
            SG25.Alphapad objAlphapad = new SG25.Alphapad();
            objAlphapad.ShowDialog();
            TextBox2.Text = Class1.AlphaPadret;// you need this do not remove
            Class1.AlphaPadsend = TextBox2.Text;
            if (AddingNew == false)
                ModifyButtons();
        }

        private void ToolStripButton2_Click(System.Object sender, System.EventArgs e)
        {
            ToolStripModify.Enabled = false;
            ToolStripAddNew.Enabled = true;
            ToolStripDelete.Enabled = true;
            ToolStripSave.Enabled = false;
            ToolStripButton2.Enabled = true;

            this.Close();
            this.Dispose();

            Mode objMode = new Mode();
            objMode.ShowDialog();

        }


        private void ToolStripModify_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                int ans;
               Class1.UserIndex = Convert.ToInt16(UserIDTXT.Text.ToString());
                ans=userTAobj.UpdateUsers(this.TextBox2.Text, this.ManualLBL.Text, this.GraphLBL.Text, this.UsersLBL.Text, this.SetupLBL.Text, this.ProgramsLBL.Text,Class1.UserIndex);
              }
            catch (Exception ex)
            {
                string msg = ex.ToString();
                MessageBox.Show (msg); // ("Cannot have Useres with same name!");//need to change this text also i think can i
                AddingNew = false;
               
            }


                ToolStripModify.Enabled = false;
                ToolStripAddNew.Enabled = true;
                ToolStripDelete.Enabled = true;
                ToolStripSave.Enabled = false;
                ModifySuccess objModifySuccess=new ModifySuccess();
                objModifySuccess.Show();
            }

        private void ToolStripAddNew_Click(System.Object sender, System.EventArgs e)
        {
            AddingNew = true;
            TextBox1.Enabled = true;
            UsersBindingSource.AddNew();
            ToolStripAddNew.Enabled = false;
            ToolStripDelete.Enabled = false;
            ToolStripModify.Enabled = false;
            ToolStripSave.Enabled = true;
            ManualLBL.Text = "Denied";
            UsersLBL.Text = "Denied";
            SetupLBL.Text = "Denied";
            ProgramsLBL.Text = "Denied";
        }

        private void ToolStripSave_Click(System.Object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox1.Text))
            {
                MessageBox.Show("The USER field cannot be empty.");
                return;

            }
            TextBox1.Enabled = false;

            if (string.IsNullOrEmpty(TextBox2.Text))
            {
                MessageBox.Show("The PASSWORD field cannot be empty.");
                return;

            }

            Class1.UserIndex =Class1.UserIndex + 1;
            setupTAobj.UpdateUserIndex(Class1.UserIndex);
           
            try
            {
                userTAobj.InsertUsers(Class1.UserIndex, this.TextBox1.Text, this.TextBox2.Text, this.ManualLBL.Text, this.GraphLBL.Text, this.UsersLBL.Text, this.SetupLBL.Text, this.ProgramsLBL.Text, false);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot have Useres with same name!");
            }

            userTAobj.Fill(setupDatasetObj.Users);
            this.Refresh();
            ToolStripSave.Enabled = false;
            ToolStripAddNew.Enabled = true;
            ToolStripDelete.Enabled = true;
            AddingNew = false;
            //Class1.AlphaPadret = "";
            //TextBox2.Text = "";
            
        }

        private void ToolStripDelete_Click(System.Object sender, System.EventArgs e)
        {
            int i=userTAobj.Fill(setupDatasetObj.Users);
            if (i < 2)
            {
                MessageBox.Show("There must be at least one record in the database. If you want to delete this record, you must create another one first.", "Warning", MessageBoxButtons.OK);
                return;
            }
            userTAobj.DeleteUsers(Convert.ToInt16(UserIDTXT.Text));
            
            //userTAobj.Fill(setupDatasetObj.Users);
            MessageBox.Show("Record deleted. ");
this.usersTableAdapter.Fill(this.setupDataSet.Users);
        }
        private void ModifyButtons()
        {
            ToolStripModify.Enabled = true;
            ToolStripAddNew.Enabled = false;
            ToolStripDelete.Enabled = false;
            ToolStripSave.Enabled = false;
            ToolStripButton2.Enabled = true;
        }


        private void TextBox1_TextChanged(System.Object sender, System.EventArgs e)
        {
            if (TextBox1.Text.Length > 20)
            {
                MessageBox.Show("Max 20 alpha-numeric characters!");
                TextBox1.Text = "";
            }

        }

        private void TextBox2_TextChanged(System.Object sender, System.EventArgs e)
        {
            if (TextBox2.Text.Length > 20)
            {
                MessageBox.Show("Max 20 alpha-numeric characters!");
                TextBox2.Text = "";
            }
        }

        private void GraphLBL_Click(object sender, EventArgs e)
        {

        }




       

        

    }
}
