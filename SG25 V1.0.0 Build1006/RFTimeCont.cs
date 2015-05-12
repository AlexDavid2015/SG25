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
    public partial class RFTimeCont : Form
    {
        int RFTimeContTick = 0;// record the elapsed time in secs
        public RFTimeCont()
        {
            InitializeComponent();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {

            label1.Text = "            "+"    RF Time : " + RFTimeContTick.ToString();
            if (RFTimeContTick == 0)
            {
                Timer1.Stop();// Stop the Timer1
                Class2.Update("False", "tblPlasmaProcess", "RFFlag");
                this.Close();
                this.Dispose();
                Main1 objMain1 = new Main1();// Auto page
                objMain1.ShowDialog();
                // Auto Page Idle
            }
            RFTimeContTick--;
        }

        private void cmdContinue_Click(object sender, EventArgs e)
        { 
            RFTimeContTick = Class1.RFTimeAuto;
            cmdStartOver.Enabled = false;
            Timer1.Enabled = true;
            
            // Auto Page Start
        }

        private void cmdStartOver_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
            Class2.Update("False", "tblPlasmaProcess", "RFFlag");
            Main1 objMain1 = new Main1();// Auto page
            objMain1.ShowDialog();
            // Auto Page Idle
        }

        private void MsgResidentialTimePage_Load(object sender, EventArgs e)
        {
            label1.Text="Plasma Process Stopped in the middle, Do you want to continue or Start over?";
           
        }
    }
}
