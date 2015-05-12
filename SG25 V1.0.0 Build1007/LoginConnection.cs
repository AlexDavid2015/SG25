using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace SG25
{
    public partial class LoginConnection : Form
    {
        public LoginConnection()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (txtPW.Text!="")
             {
                 string password = txtPW.Text;
                 Class1.Password = usersTableAdapter1.SelectPass("ADMIN");
                 Thread.Sleep(200);
                if(txtPW.Text==Class1.Password)
                {
                    Class1.UManual = usersTableAdapter1.GetMaunal("ADMIN");
                    Thread.Sleep(200);
                    Class1.UUser = usersTableAdapter1.GetUsers("ADMIN");
                    Thread.Sleep(200);
                    Class1.USetupPage = usersTableAdapter1.GetSetup("ADMIN");
                    Thread.Sleep(200);
                    Class1.UProgram = usersTableAdapter1.GetPrograms("ADMIN");
                    Thread.Sleep(200);
                    txtPW.Text = "";
                    Class1.Password = "";
                    Class1.IsLoginedIn = true;
                    Class1.TheUser = "ADMIN";
                    Class1.IsLoginedIn = true;
                    Class2.UpdateUserLoginedIn(Class1.TheUser, Class1.IsLoginedIn);
                    Class2.WriteEventLog(Class1.LoginSuccessfullyCode, Class1.LoginSuccessfullyDesc);
                    Class2.LogEventDB(Class1.LoginSuccessfullyCode, Class1.LoginSuccessfullyDesc);
                    Class1.LoginOK = true;
                    this.Close();
                    this.Dispose();
                    return;
                    
                }else
                {
                    MessageBox.Show("Wrong Password");
                    txtPW.Text = "";
                    txtPW.Focus();
                }
                                
            }
        }

        private void txtPW_Click(object sender, EventArgs e)
        {
            Class1.AlphaPadret = "";
            Alphapad objAlpha = new Alphapad();
            objAlpha.ShowDialog();
            try
            {
                txtPW.Text = Class1.AlphaPadret;

            }
            catch
            {
            }
        }
    }
}
