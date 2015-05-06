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
    public partial class Numeric_Pad : Form
    {
        public Numeric_Pad()
        {
            InitializeComponent();
        }

        private void Numeric_Pad_Load(object sender, EventArgs e)
        {
            //this.TB1.Text = Class1.NumPadsend.ToString();
            TB1.SelectionStart = 0;
            TB1.SelectionLength = TB1.Text.Length;
            TB1.HideSelection = false;
            TB1.Focus();

        }

        private void TB1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button9_Click(object sender, EventArgs e)
        {
            //if (String.Compare(TB1.Text, Class1.NumPadsend.ToString()) == 0)
            //    TB1.Text = "";
            this.TB1.Text = this.TB1.Text + "9";
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            //if (String.Compare(TB1.Text, Class1.NumPadsend.ToString()) == 0)
            //    TB1.Text = "";
            this.TB1.Text = this.TB1.Text + "8";
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            //if (String.Compare(TB1.Text, Class1.NumPadsend.ToString()) == 0)
            //    TB1.Text = "";
            this.TB1.Text = this.TB1.Text + "7";
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            //if (String.Compare(TB1.Text, Class1.NumPadsend.ToString()) == 0)
            //    TB1.Text = "";
            this.TB1.Text = this.TB1.Text + "6";
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            //if (String.Compare(TB1.Text, Class1.NumPadsend.ToString()) == 0)
            //    TB1.Text = "";
            this.TB1.Text = this.TB1.Text + "5";
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            //if (String.Compare(TB1.Text, Class1.NumPadsend.ToString()) == 0)
            //    TB1.Text = "";
            this.TB1.Text = this.TB1.Text + "4";
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            //if (String.Compare(TB1.Text, Class1.NumPadsend.ToString()) == 0)
            //    TB1.Text = "";
            this.TB1.Text = this.TB1.Text + "3";
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            //if (String.Compare(TB1.Text, Class1.NumPadsend.ToString()) == 0)
            //    TB1.Text = "";
            this.TB1.Text = this.TB1.Text + "2";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //if (String.Compare(TB1.Text, Class1.NumPadsend.ToString()) == 0)
            //    TB1.Text = "";
            this.TB1.Text = this.TB1.Text + "1";
        }

        private void Zerobtn_Click(object sender, EventArgs e)
        {
            //if (String.Compare(TB1.Text, Class1.NumPadsend.ToString()) == 0)
            //    TB1.Text = "";
            this.TB1.Text = this.TB1.Text + "0";
        }

        private void Dotbtn_Click(object sender, EventArgs e)
        {
            //if (String.Compare(TB1.Text, Class1.NumPadsend.ToString()) == 0) /* --------Need to Check-------*/
            //    TB1.Text = "";
            this.TB1.Text = this.TB1.Text + ".";
        }

        private void Delbtn_Click(object sender, EventArgs e)
        {
            string temp = null;

            try
            {
                temp = this.TB1.Text;
                temp = temp.Substring(0, temp.Length - 1);
                this.TB1.Text = temp;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void Clearbtn_Click(object sender, EventArgs e)
        {
            this.TB1.Text = "";

        }

        private void Cancelbtn_Click(object sender, EventArgs e)
        {
            Class1.NumPadret = Class1.NumPadsend;
            this.Close();
            this.Dispose();

        }

        private void Enterbtn_Click(object sender, EventArgs e)
        {
            Class1.NumPadret = Convert.ToDouble(TB1.Text);
            this.Close();
            this.Dispose();

        }
    }
}
