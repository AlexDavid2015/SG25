using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SG25
{
    public partial class UserConnectionSelectionPage : Form
    {
        public UserConnectionSelectionPage()
        {
            InitializeComponent();
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            // Disconnect IO modules
            AvantechDIs.FreeResource();
            AvantechDOs.FreeResource();
            AvantechAOs.FreeResource();
            AvantechAIs.FreeResource();
            AvantechDIOs.FreeResource();
            /// Disable Coupler Avantech modules
            if (Class1.TheUser!=null)
            {
                 Class1.IsLoginedIn = false;
                 Class2.UpdateUserLoginedIn(Class1.TheUser, Class1.IsLoginedIn);
            }
           
            System.Environment.Exit(1);
        }

        private void cmdContinue_Click(object sender, EventArgs e)
        {
            LoginConnection objLogin = new LoginConnection();
            objLogin.ShowDialog();
            if(Class1.LoginOK==true)
            {
                Class1.LoginOK = false;
                this.Close();
                this.Dispose();
                Class1.IsContinueClicked = true;
            }
           
        }

        private void cmdStartOver_Click(object sender, EventArgs e)
        { 
            Class1.connectedObjFlg = true;
            this.Close();
            this.Dispose();
            Class1.Connobj.Close();
            Class1.Connobj.Dispose();
            Class1.Connobj = new ConnectPage();
            Class1.Connobj.Show();
          
           }
            //Class1.RetForm = Class1.Connobj;
           
        }

      
    }

