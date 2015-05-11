using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using APS_Define_W32;
using APS168_W32;
using System.IO;
using System.Threading;

namespace SG25
{
    public partial class Leak : Form
    {
        public Leak()
        {
            InitializeComponent();
        }

        //PCI-7856 Card ID
        int CardID = 0;
        //HSL Bus is 0
        int BusNo = 0;
        //HSL-DI16DO16 Slave ID
        //int moduleDI = 1;
        //HSL-D032 Slave ID
       // int moduleDO = 3;
        //HSL-AO4 Slave ID
        //int moduleAO = 5;
        //HSL-AI16AO2 Slave ID
        //int moduleAI = 7;
     //   int moduleDIDO = 3;
        Label[] AI = new Label[17];
        Button[] DI = new Button[17];
        public string DOStatus;
        string DOArray;
        Int32 T_Tick;
        double NewPress;
        Int32 NewPDT;
        double RealPressure;
        string Passed;
        Thread LeakThread;
        public Thread bothDoorsUpTh;
        public Thread bothDoorsOpenTh;

        string TPath;
        private void Leak_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
           if (LeakThread != null)
            {
                LeakThread.Abort();
            }
        }


        private void Leak_Load(System.Object sender, System.EventArgs e)
        {
            string NowDate = null;
            Button7.Visible = false;
            T_Tick = 0;
            Label7.Text =Class1.PunpDwnTime + " sec";

            NewPDT = Convert.ToInt16(Class2.Read("LeakT", "Setup"));
            Label7.Text = NewPDT.ToString();
            //PunpDwnTime
           // NewPress = 0.05;  // assigned by DB
            NewPress =Convert.ToDouble(Class2.Read("LeakP","Setup"));
            //R_PT
            Label6.Text = NewPress.ToString();
            NowDate = DateTime.Now.ToString();
            NowDate = NowDate.Replace("/", "-");
            NowDate = NowDate.Replace(":", "-");
            TPath = ("C:/Program Files/QML-EX/LeakTest/LeakTestLog " + NowDate + " .csv");
        }


        private void Button2_Click(System.Object sender, System.EventArgs e)  //start button
        {
            Button9.Enabled = false;//
            GBDoor.Enabled = false;
            Button2.Visible = false;
            Button1.Visible = false;
            Button7.Visible = true;
            
            StartBW.RunWorkerAsync();
        
        }


        private void Button1_Click(System.Object sender, System.EventArgs e) //exit button
        {
            //if (Class1.DI_FrontDoorup == 1 & Class1.DI_BackDoorUp == 1)
            //{
            //    bothDoorsOpenTh = new Thread(new System.Threading.ThreadStart(OpenDoorsTh));
            //    bothDoorsOpenTh.Start();
            //    rdCloseDoor.Enabled = false;
            //    rdOpenDoor.Enabled = false;
            //}

            Thread.Sleep(500);
           // Class2.Create10DOArray(6,0, 7,0, 5,0); //purge off 

           
            int[] DOChannelArr = { 21, 22, 20 }; //vent close,vacuum close,pres close
            bool[] DOStateArr = { true, false };
            Class2.SetMultiDO(DOChannelArr, DOStateArr);

            //Class2.SetDO(Class1.DOSlotNum, 21, false);//vent close
            //Class2.SetDO(Class1.DOSlotNum, 22, false);//vacuum close
            //Class2.SetDO(Class1.DOSlotNum, 20, false);//pres close


          
            if (LeakThread!=null)
            {
                LeakThread.Abort();
            }
            if (bothDoorsUpTh!=null)
            {  bothDoorsUpTh.Abort();}
            if (bothDoorsOpenTh!=null)
            {  bothDoorsOpenTh.Abort();}
            Class1.LeakOpen = false;
            Class1.GBManualDoors.Enabled = true;
            Class1.btnIOManual.Enabled = true;
            Class1.btnManualMode.Enabled = true;
            Class1.btnManualLeak.Enabled = true;
            Class1.btnManualProg.Enabled = true;
            Class1.btnManualSetup.Enabled = true;
            Class1.btnManualStart.Enabled = true;
           // Class1.btnMaualSMEMA.Enabled = true;
            Class1.btnManualTuner.Enabled = true;
            Class1.btnManualStop.Enabled = true;
            Class1.objLeak = null;
            if (Class1.openIO!=null)
            { Class1.openIO.Location = new Point(400, 400);}
            
            this.Hide();
            this.Dispose();
        }

     private void LeakTestTh()
        {
            T_Tick = -3;
            do
            {
                Thread.Sleep(1000);
                if (Class1.DO_PressureON == true)
                {
                    //RealPressure = Math.Pow(10, (Class1.AI_PressureValue - 6)); // using AGP100 Pirani
                    //RealPressure = RealPressure + 0.02;
                    RealPressure = Convert.ToDouble(Class1.AI_PressureValue / 10); // using Inficon CGD025D
                    PressureLbl.Invoke((MethodInvoker)delegate { PressureLbl.Text = (Math.Round(RealPressure, 2)).ToString(); });
                }
                else
                {
                    PressureLbl.Invoke((MethodInvoker)delegate { PressureLbl.Text ="Over Range" ; });
                   
                }
                //End Calculate the Pressure in mbar
                //End Read AI
                //Display Timer
                T_Tick += 1;
                Label1.Invoke((MethodInvoker)delegate{Label1.Text=T_Tick.ToString();});
               
                //End Display Timer

                //And Label1.Text > NewPDT
                if (RealPressure < NewPress & Class1.DO_PressureON == true)
                {
                    Class1.LeaktestResult = "Passed";
                    Label5.Invoke((MethodInvoker)delegate { Label5.Text ="Passed" ; });
                    Passed = "Passed";
                    Button9.Invoke((MethodInvoker)delegate { Button9.Enabled = true; });
                    if (LeakThread != null)
                    {
                        LeakThread.Abort();
                    }
                   
                }
                else
                {

                    Label5.Invoke((MethodInvoker)delegate { Label5.Text = "Testing"; });
                }

                //And RealPressure > NewPress
               
                if ((Convert.ToInt32(Label1.Text)) > NewPDT)
                {
                    Class1.LeaktestResult = "Not Passed";
                    Label5.Invoke((MethodInvoker)delegate{Label5.Text = "Not Passed";});
                    Passed = " Not Passed";
                    Button9.Invoke((MethodInvoker)delegate { Button9.Enabled = true; });
                    if (LeakThread != null)
                    {
                        LeakThread.Abort();
                    }
                   
                 }

                if (T_Tick > 0)
                {
                    if (Class1.DO_VacuumON == false)
                    {
                       // Class2.Create10DOArray(7, 1);//vacumvalve
                       
                        Class2.SetDO(Class1.DOSlotNum, 22, true);//vacuum close
                       
                        
                    }
                }
                if (T_Tick > 8)
                {
                    if (Class1.DO_PressureON == false)
                    {
                       // Class2.Create10DOArray(5, 1);//pressure valve
                       
                        Class2.SetDO(Class1.DOSlotNum, 20, true);//pres close
                    }
                }
            } while (true);

        }

      private void Button7_Click(System.Object sender, System.EventArgs e) //Stop  button
        {
          //  Class2.Create10DOArray(7, 0, 5, 0, 6,1, 20,0);//vacum valve off

            
            int[] DOChannelArr = { 22, 20,21,1 }; //vacuum close,pres close,vent close,pump off
            bool[] DOStateArr = { false,false,true,false };
            Class2.SetMultiDO(DOChannelArr, DOStateArr);

            //Class2.SetDO(Class1.DOSlotNum, 22, false);
           // Class2.SetDO(Class1.DOSlotNum, 20, false);
            //Class2.SetDO(Class1.DOSlotNum, 21, true);
           // Class2.SetDO(Class1.DOSlotNum, 1, false);

            if (LeakThread != null)
            {
                LeakThread.Abort();
            }
            Label5.Text = "Interrupted";
            Button9.Enabled = false;
            Button1.Visible = true;
            Button2.Visible = true;
            GBDoor.Enabled = true;

            Thread.Sleep(500);
            bothDoorsOpenTh = new Thread(new System.Threading.ThreadStart(OpenDoorsTh));
            bothDoorsOpenTh.Start();
            rdCloseDoor.Enabled = false;
            rdOpenDoor.Enabled = false;
            Button7.Visible = false;
        }


        private void Button9_Click(System.Object sender, System.EventArgs e)
        {
            if (WirteLogThread.IsBusy == false)
                WirteLogThread.RunWorkerAsync();
        }


        private void WirteLogThread_RunWorkerCompleted(System.Object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            System.IO.StreamWriter fs = null;
            string NowDate = null;


            NowDate = DateTime.Now.ToString();
            NowDate = NowDate.Replace("/", "-");
            NowDate = NowDate.Replace(":", "-");


            try
            {
                if (DateTime.Now.Date >Class1.FileStartTime)
                {
                    TPath = ("C:/Program Files/QML-EX/LeakTest/LeakTestLog " + NowDate + " .csv");
                }
                fs = File.AppendText(TPath);
                //Dim RNow As DateTime
                fs.WriteLine("SCI Automation Pte. Ltd" + Environment.NewLine);
                fs.WriteLine("QML-EX Batch Plasma System" + Environment.NewLine);
                fs.WriteLine("www.sciplasma.com" + Environment.NewLine);
                fs.WriteLine(DateTime.Now + " The Leak Test was done with pressure set at:  " + Environment.NewLine);
                fs.WriteLine((Math.Round(NewPress,2)).ToString() + " mbar " + Environment.NewLine);
                fs.WriteLine("in : " + Label1.Text + " sec" + Environment.NewLine);
                fs.WriteLine("Result : " +Class1.LeaktestResult + Environment.NewLine + Environment.NewLine);
                fs.Flush();
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            MessageBox.Show("File Created");
        }




        private void Button8_Click(System.Object sender, System.EventArgs e)
        {
            OpenFileDialog MyDialog = new OpenFileDialog();
            Stream myStream = null;

            MyDialog.Title = "Leak test Log File";
            MyDialog.InitialDirectory = "C:/Program Files/QML-EX/LeakTest/LeakTestLogs";
            MyDialog.Filter = "txt files (*.csv)|*.csv";
            MyDialog.FilterIndex = 2;
            MyDialog.RestoreDirectory = true;

            if (MyDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string str = MyDialog.FileName.ToString();

                try
                {
                    using (StreamReader sr = new StreamReader(str))
                    {
                        string line = null;
                        TextBox1.Text = "";
                        do
                        {
                            line = sr.ReadLine();
                            TextBox1.Text = TextBox1.Text + line;
                            TextBox1.Text = TextBox1.Text + Environment.NewLine;
                        } while (!(line == null));
                        sr.Close();
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show("The file could not be read:");
                }
            }
        }

        private void Label6_DoubleClick(object sender, EventArgs e)
        {
            Input obj = new Input();
            obj.TopMost = true;
            obj.ShowDialog();
            
            if (Class1 .RetInput == "" | Class1 .RetInput ==null)
            {
                return;
            }
            else
            {
                NewPress = Convert.ToDouble(Class1.RetInput);
                Label6.Text = Class1.RetInput;
                //Class2.LeakUpdate(Class1.RetInput);
                Class2.Update(Class1.RetInput,"Setup","LeakP");
                Class1.RetInput = "";
                //update DB
            }
        }


        private void rdCloseDoor_Click(object sender, EventArgs e) // close both doors
        {
            bothDoorsUpTh = new Thread(new System.Threading.ThreadStart(DoorsUpTh));
            bothDoorsUpTh.Start();
            rdOpenDoor.Enabled = false;
            rdCloseDoor.Enabled = false;
        }


        private void DoorsUpTh()
        {
            //Class2.Create10DOArray(1, 1, 3, 1, 2, 0, 4, 0); // Send both doors up with 1 new function CreateDOArrayDoors
            //DateTime StartTime = DateTime.Now;
            //DateTime NewTime;
            //TimeSpan Diff;
            //Class1.Milliseconds = 0;

            //do
            //{

            //    NewTime = DateTime.Now;
            //    Diff = NewTime - StartTime;
            //    Class1.Milliseconds = (int)Diff.Seconds;
            //    if (Class1.Milliseconds > 2)
            //    {
            //       // MRaiseAlarm(20);
            //        MessageBox.Show("Both the Doors Up Error", "indicates that both doors have not reached their up sensors.");
                   
            //        Class2.Create10DOArray(8, 0, 9, 0, 1, 0, 3, 0, 2, 1, 4, 1); // Send both doors down with 1 new function Create4DOArrayDoors
            //        rdOpenDoor.Invoke((MethodInvoker)delegate { rdOpenDoor.Enabled = true; });
            //        rdCloseDoor.Invoke((MethodInvoker)delegate { rdCloseDoor.Enabled = true; });
            //        bothDoorsUpTh.Abort();
            //        return;
            //    }

            //}
            //while (Class1.DI_FrontDoorup == 0 & Class1.DI_BackDoorUp == 0);
            //CloseDoorsTh();

        }

        private void CloseDoorsTh()
        {
            //Thread.Sleep(200);
            //Class2.Create10DOArray(8,1, 9, 1);

            //if (Class1.DI_FrontDoorup == 0 & Class1.DI_BackDoorUp == 0)
            //{
            //    //MRaiseAlarm(23);
            //    MessageBox.Show("Both the Doors are not Closed Error", "Indicates that both doors are not closeed.");
            //    Class2.Create10DOArray(1, 0,3,0, 2,1, 4,  1); // Send both doors down with 1 new function Create4DOArrayDoors
            //}

            //if (IsHandleCreated)
            //{
            //    rdOpenDoor.Invoke((MethodInvoker)delegate { rdOpenDoor.Enabled = true; });
            //    rdCloseDoor.Invoke((MethodInvoker)delegate { rdCloseDoor.Enabled = true; });
            //}
            //bothDoorsUpTh.Abort();

        }



        private void rdOpenDoor_Click(object sender, EventArgs e) // open both doors
        {
            bothDoorsOpenTh = new Thread(new System.Threading.ThreadStart(OpenDoorsTh));
            bothDoorsOpenTh.Start();
            rdCloseDoor.Enabled = false;
            rdOpenDoor.Enabled = false;


        }
        private void OpenDoorsTh()
        {
            //Class2.Create10DOArray(8, 0,9,  0); //open the doors
            ////Class2.Create10DOArrayDoors(1, 3, 0, 0);

            //DateTime StartTime = DateTime.Now;
            //DateTime NewTime;
            //TimeSpan Diff;
            //Class1.Milliseconds = 0;


            //do
            //{

            //    NewTime = DateTime.Now;
            //    Diff = NewTime - StartTime;
            //    Class1.Milliseconds = (int)Diff.Seconds;

            //    if (Class1.Milliseconds > 2)
            //    {
            //       // MRaiseAlarm(29);
            //        MessageBox.Show("Both doors are not down Error", "Indicates that both doors are not down.");
            //        rdOpenDoor.Invoke((MethodInvoker)delegate { rdOpenDoor.Enabled = true; });
            //        rdCloseDoor.Invoke((MethodInvoker)delegate { rdCloseDoor.Enabled = true; });
            //        bothDoorsOpenTh.Abort();
            //        return;
            //    }

            //}
            //while (Class1.DI_ChDoor_Front == 0 & Class1.DI_ChDoorBack == 0);
            //Thread.Sleep(200);
            //DoorsDownTh();

        }

        private void DoorsDownTh()
        {

           // Class2.Create10DOArray(1,  0,3,0, 2,1, 4,   1); //Reset both up coils
            Thread.Sleep(600);

            if (Class2.SetDoorsAlarm == true)
            {
                //MRaiseAlarm(32);
                MessageBox.Show("Function error", "CreateArray aborted.Call for Service.");
                Class2.SetDoorsAlarm = false;
            }
            if (IsHandleCreated)
            {
                rdOpenDoor.Invoke((MethodInvoker)delegate { rdOpenDoor.Enabled = true; });
                rdCloseDoor.Invoke((MethodInvoker)delegate { rdCloseDoor.Enabled = true; });
            }

            bothDoorsOpenTh.Abort();

        }

        private void Label7_DoubleClick(object sender, EventArgs e)
        {
            Input obj = new Input();
            obj.TopMost = true;
            obj.ShowDialog();
            if (Class1.RetInput == "" | Class1.RetInput==null)
            {
                return;
            }
            else
            {
                NewPDT = Convert.ToInt32(Class1.RetInput);
                
                Label7.Text = NewPDT.ToString();
                //Class2.UpdateLeakTime(NewPDT.ToString ());
                Class2.Update(NewPDT.ToString(), "Setup", "LeakT");
                Class1.RetInput = "";
                //update DB
                 
            }
        }

        private void Button2_Enter(object sender, EventArgs e)
        {
            Label5.Text = "Started";
           
        }

   

        private void StartBW_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(200);
        }

        private void StartBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if (Class1.DI_FrontDoorDown == 1 & Class1.DI_BackDoorDown == 1)
           // {
                bothDoorsUpTh = new Thread(new System.Threading.ThreadStart(DoorsUpTh)); //Close doors
                bothDoorsUpTh.Start();
                rdOpenDoor.Enabled = false;
                rdCloseDoor.Enabled = false;
                Thread.Sleep(500);
          //  }

            Button1.Visible = false;

            //Class2.Create10DOArray(6, 0, 20, 1);/// purge off//pump on

           
            int[] DOChannelArr = { 21, 1 }; //vent close,pump on
            bool[] DOStateArr = { false, true };
            Class2.SetMultiDO( DOChannelArr, DOStateArr);
            
            //Class2.SetDO(Class1.DOSlotNum, 21, false);//vent close
            //Class2.SetDO(Class1.DOSlotNum, 1, true);//pump on

                                                
            Thread.Sleep(5000);
            LeakThread = new Thread(new System.Threading.ThreadStart(LeakTestTh));
            LeakThread.Start();
            T_Tick = 0;
            StartBW.CancelAsync();
        }

        private void TextBox1_DoubleClick(object sender, EventArgs e)
        {
            TextBox1.Text = "";
        }
    }
}
