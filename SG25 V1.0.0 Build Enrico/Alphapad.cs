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
    public partial class Alphapad : Form
    {
        public Alphapad()
        {
            InitializeComponent();
        }
       

        private void Alphapad_Load(object sender, EventArgs e)
        {
            this.TB1.Text = Class1.AlphaPadsend;
            this.TB1.Text = "";
            TB1.SelectionStart = 0;
            TB1.SelectionLength = TB1.Text.Length;
            TB1.HideSelection = false;
            TB1.Focus();
            
        }
        
            private void Button1_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "Q";
            }

            private void Button38_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "1";
            }

            private void Button37_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "2";
            }

            private void Button36_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "3";
            }

            private void Button35_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "4";
            }

            private void Button34_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "5";
            }

            private void Button33_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "6";
            }

            private void Button32_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "7";
            }

            private void Button31_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "8";
            }

            private void Button30_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "9";
            }

            private void Button29_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "0";
            }

            private void Clearbtn_Click_1(object sender, EventArgs e)
            {
                this.TB1.Text = "";
            }

            private void Button28_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "-";
            }

            private void Button27_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "_";
            }

            private void Button2_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "W";
            }

            private void Button3_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "E";
            }

            private void Button4_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "R";
            }

            private void Button5_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "T";
            }

            private void Button6_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "Y";
            }

            private void Button7_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "U";
            }

            private void Button8_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "I";
            }

            private void Button9_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "O";
            }

            private void Button10_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "P";
            }

            private void Button19_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "A";
            }

            private void Button18_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "S";
            }

            private void Button17_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "D";
            }

            private void Button16_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "F";
            }

            private void Button15_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "G";
            }

            private void Button14_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "H";
            }

            private void Button13_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "J";
            }

            private void Button12_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "K";
            }

            private void Button11_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "L";
            }

            private void Button26_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "Z";
            }

            private void Button25_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "X";
            }

            private void Button24_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "C";
            }

            private void Button23_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "V";
            }

            private void Button22_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "B";
            }

            private void Button21_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "N";
            }

            private void Button20_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + "M";
            }

            private void Spacebtn_Click_1(object sender, EventArgs e)
            {
                if (TB1.Text == Class1.AlphaPadsend)
                {
                    TB1.Text = "";
                }
                this.TB1.Text = this.TB1.Text + " ";
            }

            private void Deletebtn_Click_1(object sender, EventArgs e)
            {
                string temp = "";

                try
                {
                    temp = Convert.ToString(this.TB1.Text);
                    temp = temp.Substring(0, temp.Length - 1);
                    this.TB1.Text = temp;

                }
                catch (Exception)
                {
                }
            }

            private void Cancelbtn_Click_1(object sender, EventArgs e)
            {
                Class1.AlphaPadret = Class1.AlphaPadsend;
                        this.Close();
                         this.Dispose();
            }

            private void EnterBtn_Click_1(object sender, EventArgs e)
            {
                Class1.AlphaPadret = this.TB1.Text;
                if (Alphapad.ActiveForm.CausesValidation == true)
                {
                    Class1.openAlphapad = true;
               //     QMLEX.Class1.AlphaPadret = this.TB1.Text;
                   
                    
                    //  if (Class1.openAlphapad == true)
                    //{
            //        QMLEX.Alphapad.ActiveForm.Hide();
                //    QMLEX.Alphapad.ActiveForm.Close();
                  //  QMLEX.Alphapad.ActiveForm.Dispose();
                }
               
                      this.Close();
                       this.Dispose();
          
                   //      Alphapad.ActiveForm.Close();
            }

            private void TB1_TextChanged_1(object sender, EventArgs e)
            {

                TB1.SelectionStart = 0;
                TB1.SelectionLength = TB1.Text.Length;
                TB1.HideSelection = false;
                TB1.Focus();
            }



        }


    
}
