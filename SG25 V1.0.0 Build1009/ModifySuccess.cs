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
    public partial class ModifySuccess : Form
    {
        public ModifySuccess()
            {
                InitializeComponent();
            }

        private void OK_Button_Click(System.Object sender, System.EventArgs e)
	        {
		        this.DialogResult = System.Windows.Forms.DialogResult.OK;
		        this.Close();
	        }

	    private void Cancel_Button_Click(System.Object sender, System.EventArgs e)
	        {
		        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		        this.Close();
	        }

	   private void timer1_Tick_1(object sender, EventArgs e)
        {
            this.Close();
        }

       private void ModifySuccess_Load(object sender, EventArgs e)
       {
           Timer timer1 = new Timer();
           timer1.Interval = 10000;
           timer1.Start();
       }
    }

}

