using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Apax_IO_Module_Library
{
    public partial class Wait_Form1 : Form
    {
        private int iCount;
        private int iRunTime;
        private int iMilliSec;

        public Wait_Form1()
        {
            InitializeComponent();
            iCount = 1;
            iRunTime = 0;
            iMilliSec = 0;

        }
        public void Start_Wait()
        {

            this.iMilliSec = 4500;
            this.Timer1.Enabled = true;
        }
        public void Start_Wait(int iWait)
        {
            this.iMilliSec = iWait;
            this.Timer1.Enabled = true;
        }
        public void End_Wait()
        {
            Timer1.Enabled = false;
            this.Close();
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            int iLabelStatus;
            //iLabelStatus = (iCount % 23);
            iLabelStatus = iCount;
            iRunTime = (iRunTime + Timer1.Interval);

            switch (iLabelStatus)
            {
                case 0:
                    this.lbl_Wait.Text = "Please wait...Loading Setup...Setup Range ";
                    iCount = 0;
                    break;
                case 1:
                    this.lbl_Wait.Text = "Please wait...Loading Setup...Gas1 ";
                    break;
                case 2:
                    this.lbl_Wait.Text = "Please wait...Loading Setup...Gas2 ";
                    break;
                case 3:
                    this.lbl_Wait.Text = "Please wait...Loading Setup...Setup Range ";
                    break;
                case 4:
                    this.lbl_Wait.Text = "Please wait...Loading Setup...Gas1T";
                    break;
                case 5:
                    this.lbl_Wait.Text = "Please wait...Loading Setup...Gas2T ";
                    break;
                case 6:
                    this.lbl_Wait.Text = "Please wait...Loading Setup...Gas1R";
                    break;
                case 7:
                    this.lbl_Wait.Text = "Please wait...Loading Setup...Gas2R ";
                    break;
                case 8:
                    this.lbl_Wait.Text = "Please wait...Loading Setup..Vent Time ";
                    break;
                case 9:
                    this.lbl_Wait.Text = "Please wait...Loading Setup...PumpDwn Time ";
                    break;
                case 10:
                    this.lbl_Wait.Text = "Please wait...Loading Setup...H2Gen ";
                    break;
                case 11:
                    this.lbl_Wait.Text = "Please wait...Loading Setup...TurboP ";
                    break;
                case 12:
                    this.lbl_Wait.Text = "Please wait...Loading Setup...UserIndex ";
                    break;
                case 13:
                    this.lbl_Wait.Text = "Please wait...Loading Setup...GCF1 ";
                    break;
                case 14:
                    this.lbl_Wait.Text = "Please wait...Loading Setup..GCF2 ";
                    break;
                case 15:
                    this.lbl_Wait.Text = "Please wait...Loading Setup...GCF3 ";
                    break;
                case 16:
                    this.lbl_Wait.Text = "Please wait...Loading Recipe...Pressure Trig ";
                    break;
                case 17:
                    this.lbl_Wait.Text = "Please wait...Loading Recipe...Time to Plasma ";
                    break;
                case 18:
                    this.lbl_Wait.Text = "Please wait...Loading Recipe...Gas1 ";
                    break;
                case 19:
                    this.lbl_Wait.Text = "Please wait...Loading Recipe...Gas2 ";
                    break;
                case 20:
                    this.lbl_Wait.Text = "Please wait...Loading Recipe...RF Power ";
                    break;
                case 21:
                    this.lbl_Wait.Text = "Please wait...Loading Recipe...RF Time ";
                    break;
                case 22:
                    this.lbl_Wait.Text = "Please wait...Loading Recipe...Tune Position ";
                    break;
                case 23:
                    this.lbl_Wait.Text = "Please wait...Loading Recipe...Load Position ";
                    break;
            }
            if ((iRunTime >= iMilliSec))
            {
                progressBar1.Value = 100;
                End_Wait();
            }
            
            else
            {
                progressBar1.Value = (int)(iRunTime * (100.0 / (double)iMilliSec));
            }
            iCount = (iCount + 1);
        }

        private void Wait_Form_Load(object sender, EventArgs e)
        {

        }

    }
}