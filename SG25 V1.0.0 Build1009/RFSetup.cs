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
    public partial class RFSetup : Form
    {
        public RFSetup()
        {
            InitializeComponent();
        }

        private void Button1_Click(System.Object sender, System.EventArgs e)
        {
           Class1.SetupRFRange = 200.0;
            this.Close();
            this.Dispose();
        }

        private void Button2_Click(System.Object sender, System.EventArgs e)
        {
            Class1.SetupRFRange = 300.0;
            this.Close();
            this.Dispose();
        }

        private void Button3_Click(System.Object sender, System.EventArgs e)
        {
            Class1.SetupRFRange = 400.0;
            this.Close();
            this.Dispose();
        }

        private void Button4_Click(System.Object sender, System.EventArgs e)
        {
            Class1.SetupRFRange = 600.0;
            this.Close();
            this.Dispose();
        }

        private void Button5_Click(System.Object sender, System.EventArgs e)
        {
            Class1.SetupRFRange = 1000.0;
            this.Close();
            this.Dispose();
        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            Class1.SetupRFRange = 300.0;
            this.Close();
            this.Dispose();
        }

        private void Button4_Click_1(object sender, EventArgs e)
        {
            Class1.SetupRFRange = 600.0;
            this.Close();
            this.Dispose();
        }

        

        

    }
}
