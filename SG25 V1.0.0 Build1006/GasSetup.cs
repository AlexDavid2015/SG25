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
    public partial class GasSetup : Form
    {
        public GasSetup()
        {
            InitializeComponent();
        }

        private void Button1_Click(System.Object sender, System.EventArgs e)
        {
            LRange.Text = "10";
        }

        private void Button2_Click(System.Object sender, System.EventArgs e)
        {
            LRange.Text = "100";
        }

        private void Button3_Click(System.Object sender, System.EventArgs e)
        {
            LRange.Text = "200";
        }

        private void Button4_Click(System.Object sender, System.EventArgs e)
        {
            LRange.Text = "500";
        }

        private void Button5_Click(System.Object sender, System.EventArgs e)
        {
            LRange.Text = "1000";
        }

        private void Button6_Click(System.Object sender, System.EventArgs e)
        {
            LType.Text = "Argon";
            if (Class1.Gasedit == 1)
            {
                Class1.GCF1 = 1.39;
            }
            else if (Class1.Gasedit == 2)
            {
                Class1.GCF2 = 1.39;
            }
            else
            {
                Class1.GCF3 = 1.39;
            }
        }

        private void Button7_Click(System.Object sender, System.EventArgs e)
        {
            LType.Text = "Oxygen";
            if (Class1.Gasedit == 1)
            {
                Class1.GCF1 = 0.99;
            }
            else if (Class1.Gasedit == 2)
            {
                Class1.GCF2 = 0.99;
            }
            else
            {
                Class1.GCF3 = 0.99;
            }
        }

        private void Button8_Click(System.Object sender, System.EventArgs e)
        {
            LType.Text = "Nitrogen";
            if (Class1.Gasedit == 1)
            {
                Class1.GCF1 = 1;
            }
            else if (Class1.Gasedit == 2)
            {
                Class1.GCF2 = 1;
            }
            else
            {
                Class1.GCF3 = 1;
            }
        }

        private void Button9_Click(System.Object sender, System.EventArgs e)
        {
            LType.Text = "Hydrogen";
            if (Class1.Gasedit == 1)
            {
                Class1.GCF1 = 1;
            }
            else if (Class1.Gasedit == 2)
            {
                Class1.GCF2 = 1;
            }
            else
            {
                Class1.GCF3 = 1;
            }
        }

        private void Button10_Click(System.Object sender, System.EventArgs e)
        {
            LType.Text = "Ar/H2";
            if (Class1.Gasedit == 1)
            {
                Class1.GCF1 = 1.39;
            }
            else if (Class1.Gasedit == 2)
            {
                Class1.GCF2 = 1.39;
            }
            else
            {
                Class1.GCF3 = 1.39;
            }
        }

        private void Button11_Click(System.Object sender, System.EventArgs e)
        {
            LType.Text = "N2/H2";
            if (Class1.Gasedit == 1)
            {
                Class1.GCF1 = 1;
            }
            else if (Class1.Gasedit == 2)
            {
                Class1.GCF2 = 1;
            }
            else
            {
                Class1.GCF3 = 1;
            }
        }


        private void Button12_Click(System.Object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(LRange.Text))
            {
                MessageBox.Show("No Range value selected");
                return;
            }
            if (string.IsNullOrEmpty(LType.Text))
            {
                MessageBox.Show("No Type value selected");
                return;
            }


            if (Class1.Gasedit == 1)
            {
                Class1.Gas1R = Convert.ToDouble(LRange.Text);
                Class1.Gas1T = LType.Text;
            }
            else if (Class1.Gasedit == 2)
            {
                Class1.Gas2R = Convert.ToDouble(LRange.Text);
                Class1.Gas2T = LType.Text;
            }
            else if (Class1.Gasedit == 3)
            {
                Class1.Gas3R = Convert.ToDouble(LRange.Text);
                Class1.Gas3T = LType.Text;
            }
            else
            {
                MessageBox.Show("No Gas line selected");
            }
            this.Close();
            this.Dispose();
        }

        private void GasSetup_Load(System.Object sender, System.EventArgs e)
        {
            LType.Text = "";
            LRange.Text = "";
           // GroupBox2.Enabled = false;

        }

        private void Button13_Click(System.Object sender, System.EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void Button14_Click(System.Object sender, System.EventArgs e)
        {
            CustomGas gs=new CustomGas();
                gs.ShowDialog();
                LType.Text = Class1.GasCT;

            if (Class1.Gasedit == 1)
            {
                Class1.GCF1 = Class1.GCFC;
            }
            else if (Class1.Gasedit == 2)
            {
                Class1.GCF2 = Class1.GCFC;
            }
            else
            {
                Class1.GCF3 = Class1.GCFC;
            }
        }

        private void Button15_Click(System.Object sender, System.EventArgs e)
        {
            LType.Text = "None";
            if (Class1.Gasedit == 1)
            {
                Class1.GCF1 = 1;
            }
            else if (Class1.Gasedit == 2)
            {
                Class1.GCF2 = 1;
            }
            else
            {
                Class1.GCF3 = 1;
            }
        }

        private void LRange_TextChanged(System.Object sender, System.EventArgs e)
        {
            GroupBox2.Enabled = true;

        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            LRange.Text = "10";
        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            LRange.Text = "100";
        }

        private void Button3_Click_1(object sender, EventArgs e)
        {
            LRange.Text = "200";
        }

        private void Button4_Click_1(object sender, EventArgs e)
        {
            LRange.Text = "500";
        }

        private void Button7_Click_1(object sender, EventArgs e)
        {
            
        }

        private void Button6_Click_1(object sender, EventArgs e)
        {
            
        }

        private void Button8_Click_1(object sender, EventArgs e)
        {
            
        }

        private void Button15_Click_1(object sender, EventArgs e)
        {
            LType.Text = "None";
            if (Class1.Gasedit == 1)
            {
                Class1.GCF1 = 1;
            }
            else if (Class1.Gasedit == 2)
            {
                Class1.GCF2 = 1;
            }
            else
            {
                Class1.GCF3 = 1;
            }
        }

        private void Button10_Click_1(object sender, EventArgs e)
        {
            LType.Text = "Ar/H2";
            if (Class1.Gasedit == 1)
            {
                Class1.GCF1 = 1.39;
            }
            else if (Class1.Gasedit == 2)
            {
                Class1.GCF2 = 1.39;
            }
            else
            {
                Class1.GCF3 = 1.39;
            }
        }

        private void Button9_Click_1(object sender, EventArgs e)
        {
            LType.Text = "Hydrogen";
            if (Class1.Gasedit == 1)
            {
                Class1.GCF1 = 1;
            }
            else if (Class1.Gasedit == 2)
            {
                Class1.GCF2 = 1;
            }
            else
            {
                Class1.GCF3 = 1;
            }
        }

        private void Button14_Click_1(object sender, EventArgs e)
        {
            CustomGas gs = new CustomGas();
            gs.ShowDialog();
            LType.Text = Class1.GasCT;

            if (Class1.Gasedit == 1)
            {
                Class1.GCF1 = Class1.GCFC;
            }
            else if (Class1.Gasedit == 2)
            {
                Class1.GCF2 = Class1.GCFC;
            }
            else
            {
                Class1.GCF3 = Class1.GCFC;
            }
        }

        private void Button12_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(LRange.Text))
            {
                MessageBox.Show("No Range value selected");
                return;
            }
            if (string.IsNullOrEmpty(LType.Text))
            {
                MessageBox.Show("No Type value selected");
                return;
            }


            if (Class1.Gasedit == 1)
            {
                Class1.Gas1R = Convert.ToDouble(LRange.Text);
                //Class1.Gas1T = LType.Text;
                Class1.Gas1T=string.Copy(LType.Text);
            }
            else if (Class1.Gasedit == 2)
            {
                Class1.Gas2R = Convert.ToDouble(LRange.Text);
           //     Class1.Gas2T = LType.Text;
                Class1.Gas2T = string.Copy(LType.Text);
            }
            else if (Class1.Gasedit == 3)
            {
                Class1.Gas3R = Convert.ToDouble(LRange.Text);
                //Class1.Gas3T = LType.Text;
                Class1.Gas3T = string.Copy(LType.Text);
            }
            else
            {
                MessageBox.Show("No Gas line selected");
            }
            this.Close();
            this.Dispose();
        }

        private void Button13_Click_1(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void LType_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {
            LType.Text = "Nitrogen";
            if (Class1.Gasedit == 1)
            {
                Class1.GCF1 = 1;
            }
            else if (Class1.Gasedit == 2)
            {
                Class1.GCF2 = 1;
            }
            else
            {
                Class1.GCF3 = 1;
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            LType.Text = "Oxygen";
            if (Class1.Gasedit == 1)
            {
                Class1.GCF1 = 0.99;
            }
            else if (Class1.Gasedit == 2)
            {
                Class1.GCF2 = 0.99;
            }
            else
            {
                Class1.GCF3 = 0.99;
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            LType.Text = "Argon";
            if (Class1.Gasedit == 1)
            {
                Class1.GCF1 = 1.39;
            }
            else if (Class1.Gasedit == 2)
            {
                Class1.GCF2 = 1.39;
            }
            else
            {
                Class1.GCF3 = 1.39;
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            LType.Text = "Hydrogen";
            if (Class1.Gasedit == 1)
            {
                Class1.GCF1 = 1;
            }
            else if (Class1.Gasedit == 2)
            {
                Class1.GCF2 = 1;
            }
            else
            {
                Class1.GCF3 = 1;
            }
        }

        private void Button11_Click_1(object sender, EventArgs e)
        {

        }

        private void button20_Click(object sender, EventArgs e)
        {
            LType.Text = "N2/H2";
            if (Class1.Gasedit == 1)
            {
                Class1.GCF1 = 1;
            }
            else if (Class1.Gasedit == 2)
            {
                Class1.GCF2 = 1;
            }
            else
            {
                Class1.GCF3 = 1;
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            LType.Text = "Ar/H2";
            if (Class1.Gasedit == 1)
            {
                Class1.GCF1 = 1.39;
            }
            else if (Class1.Gasedit == 2)
            {
                Class1.GCF2 = 1.39;
            }
            else
            {
                Class1.GCF3 = 1.39;
            }
        }

        private void GroupBox2_Enter(object sender, EventArgs e)
        {

        }

        
       // public GasSetup()
        //{
          //  Load += GasSetup_Load;
        //}
        
        
    }
}
