using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace SG25
{
    public partial class IOInfo : Form
    {
        public static IOInfo instance;
        Thread IOTh;
        
        public IOInfo()
        {
            InitializeComponent();
            instance = this;
        }

        private void IOInfo_Load(object sender, EventArgs e)
        {
           Class1.IOShowInfoForm = this;
        }

        private void getInfo()
        {
            bool[] DOInfoArray=new bool[36];
            string[] DOStrArr=new string[36];
            string DOArray;

            bool[] DIInfoArray=new bool[36];
            string[] DIStrArr = new string[36];
            string DIArray;

            do
            {
                Thread.Sleep(20);
               // DOInfoArray = Class1.DOArrayValues;
                DOInfoArray = Class1.DOIArrayValues;
                
                for (int i = 0; i <= DOInfoArray.Length - 1; i++)
                {
                    DOStrArr[i] =(Convert.ToInt16(DOInfoArray[i])).ToString();
                }
                DOArray=string.Join("",DOStrArr);
                    
                {
                    if (IsHandleCreated)
                    { label2.Invoke((MethodInvoker)delegate { label2.Text = DOArray; }); }
                }

                DIInfoArray = Class1.DIOArrayValues;

                for (int i = 0; i <= DIInfoArray.Length - 1; i++)
                {
                    DIStrArr[i] = (Convert.ToInt16(DIInfoArray[i])).ToString();
                }
                DIArray = string.Join("", DIStrArr);

                {
                    if (IsHandleCreated)
                    { label4.Invoke((MethodInvoker)delegate { label4.Text = DIArray; label36.Text = Class1.ManCycleTime; label42.Text = Class1.ErrorCounter.ToString(); }); }
                }
                           

            } while (Class1.IOStopped==false);
            Class1.IOStopped = true;
            if (IOTh != null)
            {
                IOTh.Abort();
            }

        }

        public static IOInfo Instance()
        {
            if (instance == null)
            {
                instance = new IOInfo();
               
            }

            return instance;
            
        }

        private void button24_Click(object sender, EventArgs e)
        {
            if (Class1.Add200msec == true)
            {
                Class1.Add200msec = false;
                button24.Text = "Add 200 msec";
            }
            else
            {
                Class1.Add200msec = true;
                button24.Text = "Remove 200 msec";
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            Class1.IOOpen = false;
            if (IOTh != null)
            {
                IOTh.Abort();
            }
             this.Hide();
           // this .Close();
            //this.Dispose();
        }

        private void IOInfo_Activated(object sender, EventArgs e)
        {       
            IOTh = new Thread(new System.Threading.ThreadStart(getInfo));
             IOTh.Start();}

    }
}
