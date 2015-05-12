using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SG25
{
    public partial class Mode : Form 
    {
        public Mode()
        {
            InitializeComponent();
        }
        

        private void BManual_Click(object sender, EventArgs e)
        {
            SG25.Class1.AutoCycle = false;
            SG25.Class1.ManualCycle = true;
            this.Close();
            this.Dispose();
            SG25.Class1.Auto = false;
            SG25.Class1.ManualP = true;
            SG25.Class1.User = false;
            SG25.Class1.SetupPage = false;
            SG25.Class1.Program = false;
            if (IsFormAlreadyOpen(typeof(SG25.Main1)) == null) { }
            else
            {
                SG25.Main1.ActiveForm.Close();
                SG25.Main1.ActiveForm.Dispose();
            }
            if (IsFormAlreadyOpen(typeof(SG25.Manual)) == null)
            {
               // QMLEX.Manual.ActiveForm.Show();
                SG25.Manual f10 = new SG25.Manual();
                f10.ShowDialog();
                 

            }
            else
            {
                
            }
                           
            //QMLEX.Main1.ActiveForm.Startbtn.Visible = false;
            this.Close();

        }

        private void Mode_Load(object sender, EventArgs e)
        {
            
            if (SG25.Class1.UManual == "Denied")
            {
                BManual.Visible = false;
            }
            else
            {
                BManual.Visible = true;
            }

            if (SG25.Class1.UUser == "Denied")
            {
                BUsers.Visible = false;
            }
            else
            {
                BUsers.Visible = true;
            }

            if (SG25.Class1.USetupPage == "Denied")
            {
                BSetup.Visible = false;
            }
            else
            {
                BSetup.Visible = true;
            }

            if (SG25.Class1.UProgram == "Denied")
            {
                BPrograms.Visible = false;
            }
            else
            {
                BPrograms.Visible = true;
            }
		

        }

        private void BAuto_Click(object sender, EventArgs e)
        {
            SG25.Class1.AutoCycle = true;
            SG25.Class1.ManualCycle = false;
            this.Close();
            this.Dispose();
            SG25.Class1.Auto = true;
            SG25.Class1.ManualP = false;
            SG25.Class1.User = false;
            SG25.Class1.SetupPage = false;
            SG25.Class1.Program = false;
            if (IsFormAlreadyOpen(typeof(SG25.Manual)) == null) { }
            else
            {
                SG25.Manual.ActiveForm.Close();
                SG25.Manual.ActiveForm.Dispose();
            }
            SG25.Main1 f6 = new SG25.Main1();
            f6.ShowDialog();
            f6.Startbtn.Visible = true;
            this.Close();


        }

        private void BUsers_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
            
            SG25.Users f7 = new SG25.Users();
            f7.ShowDialog();
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
            SG25.Class1.AutoCycle = false;
            SG25.Class1.ManualCycle = false;
            SG25.AlarmLog f8 = new SG25.AlarmLog();
            f8.ShowDialog();

        }

        private void BPrograms_Click(object sender, EventArgs e)
        {
            SG25.Programs f9 = new SG25.Programs();
            f9.ShowDialog();
        }
        public static Form IsFormAlreadyOpen(Type FormType)
        {
            foreach (Form OpenForm in Application.OpenForms)
            {
                if (OpenForm.GetType() == FormType)
                    return OpenForm;
            }

            return null;
        }
        private void Button5_Click(object sender, EventArgs e)
        {
            this.Close();
            if (IsFormAlreadyOpen(typeof(SG25.Main1))==null) { }
            else
            SG25.Main1.ActiveForm.Close();
            if (IsFormAlreadyOpen(typeof(SG25.Manual)) == null) { }
            else
            SG25.Manual.ActiveForm.Close();
            if (IsFormAlreadyOpen(typeof(SG25.Graph)) == null) { }
            else
            SG25.Graph.ActiveForm.Close();
            if (IsFormAlreadyOpen(typeof(SG25.Setup1)) == null) { }
            else
            SG25.Setup1.ActiveForm.Close();
            if (IsFormAlreadyOpen(typeof(SG25.Users)) == null) { }
            else
            SG25.Users.ActiveForm.Close();
            if (IsFormAlreadyOpen(typeof(SG25.Programs)) == null) { }
            else
            SG25.Programs.ActiveForm.Close();
           
            if (!Avantech.bModbusConnected)
            {
                Class1.AlarmActive = false;
                Class2.ConnClose();
                Class1.IsLoginedIn = false;
                Class2.UpdateUserLoginedIn(Class1.TheUser, Class1.IsLoginedIn);
                System.Environment.Exit(0);
            }
            else
            {
                this.Close();
                this.Dispose();
                Class1.IsLoginedIn = false;
                Class2.UpdateUserLoginedIn(Class1.TheUser, Class1.IsLoginedIn);
                Splash objSplash = new Splash();
                objSplash.ShowDialog();
            }
           

        }
        private void Mode_Activated(object sender, System.EventArgs e)
        {

            if (SG25.Class1.UManual == "Denied")
            {
                BManual.Visible = false;
            }
            else
            {
                BManual.Visible = true;
            }

            if (SG25.Class1.UUser == "Denied")
            {
                BUsers.Visible = false;
            }

            else
            {
                BUsers.Visible = true;
            }

            if (SG25.Class1.USetupPage == "Denied")
            {
                BSetup.Visible = false;
            }
            else
            {
                BSetup.Visible = true;
            }

            if (SG25.Class1.UProgram == "Denied")
            {
                BPrograms.Visible = false;
            }
            else
            {
                BPrograms.Visible = true;
            }
            try
            {

                Class2.LoadSetup();
          
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        

       
        private void BSetup_Click(object sender, EventArgs e)
        {
            
            SG25.Setup1 SetupPage = new Setup1();
            SetupPage.ShowDialog();
        }

        private void StartupBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void Mode_Activated_1(object sender, EventArgs e)
        {
            Class2.LoadSetup();
        }

        private void chkByPassMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkByPassMode.Checked == true)
            {
                Class1.ByPassMode = true;
            }
            else
            {
                Class1.ByPassMode = false;
            }
        }

    }
}
