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
    public partial class CustomGas : Form
    {
        public CustomGas()
        {
            InitializeComponent();
        }

        private void TextBox1_Click(object sender, System.EventArgs e)
        {
            Alphapad h = new Alphapad();

                h.ShowDialog();
            this.TextBox1.Text = Class1.AlphaPadret;
            Class1.GasCT = Class1.AlphaPadret;
        }


        private void TextBox2_Click(System.Object sender, System.EventArgs e)
        {
            Numeric_Pad k = new Numeric_Pad();
                k.ShowDialog();
            this.TextBox2.Text = Convert.ToString(Class1.NumPadret);
            Class1.GCFC = Class1.NumPadret;
        }

        private void Button1_Click(System.Object sender, System.EventArgs e)
        {
            this.Close();

        }

        private void CustomGas_Load(System.Object sender, System.EventArgs e)
        {
            this.TextBox1.Text = "";
            this.TextBox2.Text = "";
            Button1.Enabled = false;
        }

        private void TextBox1_TextChanged(System.Object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox1.Text) | string.IsNullOrEmpty(TextBox2.Text))
            {
            }
            else
            {
                Button1.Enabled = true;
            }
        }

        private void TextBox2_TextChanged(System.Object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox1.Text) | string.IsNullOrEmpty(TextBox2.Text))
            {
            }
            else
            {
                Button1.Enabled = true;
            }
        }


        private void Button2_Click(System.Object sender, System.EventArgs e)
        {
            Class1.GasCT = "";
            Class1.GCFC = 1;
            this.Close();
        }

        private void TextBox2_TextChanged_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox1.Text) | string.IsNullOrEmpty(TextBox2.Text))
            {
            }
            else
            {
                Button1.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Visible = false;
            Numeric_Pad k = new Numeric_Pad();
            k.ShowDialog();
            this.TextBox2.Text = Convert.ToString(Class1.NumPadret);
            Class1.GCFC = Class1.NumPadret;
        }

        private void TextBox1_TextChanged_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox1.Text) | string.IsNullOrEmpty(TextBox2.Text))
            {
            }
            else
            {
                Button1.Enabled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Visible = false;
            Alphapad h = new Alphapad();

            h.ShowDialog();
            this.TextBox1.Text = Class1.AlphaPadret;
            Class1.GasCT = Class1.AlphaPadret;
        }

        private void Button2_Click_1(object sender, EventArgs e)
        {

        }

        
       

    }
}
