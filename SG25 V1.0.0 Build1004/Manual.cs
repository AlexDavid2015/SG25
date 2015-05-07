using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using APS_Define_W32;
using APS168_W32;
using System.Data.Common;
using System.IO;
using SG25.SetupDataSetTableAdapters;
using System.Reflection;

 
namespace SG25
{
    public partial class Manual : Form 
    {
        public Manual()
        {
            InitializeComponent();
        }
        public delegate void SetAirsensor(System.Drawing.Color col);
        public delegate void SetEStop(bool status);
        public delegate void SetPumpOk(bool status);
        public delegate void SetDoor(bool status);
        private float m_Theta = 0;
        private float m_Delta = 10;
        int n;
        StreamWriter FS;
        string MainPath; //PCI-7856 Card ID
        int CardID = 0;  //HSL Bus is 0
        int BusNo = 0;  //HSL-DI16DO16 Slave ID
        public int a;
        public int b;
        public Thread ventTh;
       
       // int moduleAO = 2;
        //HSL-AI16AO2 Slave ID
        //int moduleAI = 5;
       // int moduleDIDO = 3;
        Label[] AI = new Label[17];
        Button[] DI = new Button[17];
        public string DOStatus;
        string DOArray;
        int VentTimeDisplay;
        bool DidEndRun = false;
        int PVShutoffTime;
        int TTick;
        int GhostTick;
        string NowDate;
        public Thread bothDoorsUpTh;
        public Thread bothDoorsOpenTh;
        public Thread AutocycleTh;
        public Thread valueRead;
       
        SetupDataSet ManualsetupDatasetObj = new SetupDataSet();
        StartupTableAdapter MaualstartupTAobj = new StartupTableAdapter();
        ProgramsTableAdapter MaualprogramsTAobj = new ProgramsTableAdapter();
        SetupTableAdapter ManualSetupTAobj = new SetupTableAdapter();
        AlarmLogTableTableAdapter ManualAlarmTAobj = new AlarmLogTableTableAdapter();
        public int count = 0;

       

        private void Modebtn_Click(System.Object sender, System.EventArgs e)
        {
            Class1.IOStopped = true;
            Class1.Connected = false;
            Class1.btnAutoStart.Enabled = true;
            if(Class1.IOShowInfoForm!=null)
            { Class1.IOShowInfoForm.Hide();}

            Class1.IOInfoSamePg = false;
            Class1.OpenfromManual = false;
            
            valueRead.Abort();
            Class2.ManualELMaintenance();
            this.Close();
            this.Dispose();
            
            
            Mode objMode = new Mode();
            objMode.ShowDialog();
         }

        private void Manual_Load(System.Object sender, System.EventArgs e)
        {
            if(Class1.conn.State==ConnectionState.Open)
            {
                Class1.conn.Close();
            }
            if(Class1.conn.State==ConnectionState.Closed)
            {
                Class1.conn.Open();
            }
            Class1.GBEventLogtxt = this.txtEventLog;

            Class2.ManualLogEventDB("001", "Manual Page Loaded");
            Class2.WriteManualEventLog("001", "Manual Page Loaded");
            Class1.RetFormManual = this;
            Class1.Stopped = false;
            Button16.Visible = false;
            valueRead = new Thread(new System.Threading.ThreadStart(ValueReadBW));
            valueRead.IsBackground = true;
            valueRead.Start();
            // TODO: This line of code loads data into the 'setupDataSet.Programs' table. You can move, or remove it, as needed.
            this.Text = "SCI Automation - " + " " + "SG25" + " " + Class1.VersionNo;
            
            try
            {
                this.programsTableAdapter.FillBy(this.setupDataSet.Programs, Class1.CurrentP);

            }
            catch
            {
            }


            Class1.Auto = true;
            Class1.OpenfromManual = true;
            Class1.connFlag = true;

            Class1.SetupRFRange = Convert.ToDouble(ManualSetupTAobj.SelectRFRange());
            Class1.Gas1 = Convert.ToBoolean(ManualSetupTAobj.GetSetupGas1());
            Class1.Gas2 = Convert.ToBoolean(ManualSetupTAobj.GetSetupGas2());
            Class1.Gas3 = Convert.ToBoolean(ManualSetupTAobj.GetSetupGas3());
            Class1.Gas1T = ManualSetupTAobj.SelectGas1Type().ToString();
            Class1.Gas2T = ManualSetupTAobj.SelectGas2Type().ToString();
            Class1.Gas3T = ManualSetupTAobj.SelectGas3Type().ToString();
            Class1.Gas1R = Convert.ToDouble(ManualSetupTAobj.SelectGas1Range());
            Class1.Gas2R = Convert.ToDouble(ManualSetupTAobj.SelectGas2Range());
            Class1.Gas3R = Convert.ToDouble(ManualSetupTAobj.SelectGas3Range());
            Class1.Venttime = (int)ManualSetupTAobj.SelectVentTime();
            Class1.PunpDwnTime = (int)ManualSetupTAobj.SelectPumpDwnTime();
            Class1.H2Gen = Convert.ToBoolean(ManualSetupTAobj.GetSetupHasH2());
            Class1.TurboP = Convert.ToBoolean(ManualSetupTAobj.GetSetupHasTurbo());
            Class1.UserIndex = (int)ManualSetupTAobj.SelectUserIDIndex();
            G1T.Text = Class1.Gas1T;
            G2T.Text = Class1.Gas2T;
            G3T.Text = Class1.Gas3T;

            FlatButtonAppearance fa2 = Button2.FlatAppearance;
            fa2.BorderSize = 0;
            if(Button14.Enabled==true)
            { 
            Class1.ManualStop = true;
}

            NowDate = DateTime.Now.ToString();
            NowDate = NowDate.Replace("/", "-");
            NowDate = NowDate.Replace(":", "-");
           
            MainPath = ("C:/Program Files/SG25/AlarmLog/Logfile " + NowDate + ".csv");
            Class1.TLDid2 = false;
            Class1.IOInfoSamePg = false;
           
           if(Class1.IOOpen==true)
           {
               Class1.IOInfoSamePg = true;
               Class1.IOStopped = false;
               Class1.openIO.Show();
               Class1.openIO.TopMost = true;
           }
            if(Class1.LeakOpen==true)
            {

                Class1.objLeak = new Leak();
                Class1.objLeak.Show();
                Class1.objLeak.TopMost = true;
            }

            this.Invalidate();

        }


        private void Manual_Activated(object sender, System.EventArgs e)
        {
            this.Text = "SCI Automation - " + " " + "SG25" + " " + Class1.VersionNo;
            G1T.Text = Class1.Gas1T;
            G2T.Text = Class1.Gas2T;
            G3T.Text = Class1.Gas3T;

            try
            {
                MaualprogramsTAobj.FillBy(ManualsetupDatasetObj.Programs, Class1.CurrentP);

            }
            catch
            {
            }

            Int32 ret1 = default(Int32);
            double val1 = 0;

            try
            {
                if (Class1.ProgFlag == true)
                {
                    Label34.Text = Class1.R_PW.ToString();
                    val1 = Convert.ToDouble(Class1.R_PW / Class1.SetupRFRange * 10);
                    ret1 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAO, 0, val1);
                }
                else
                {
                    Class2.RecipeUpload();
                    Label34.Text = Class1.R_PW.ToString();
                    val1 = Convert.ToDouble(Class1.R_PW / Class1.SetupRFRange * 10);
                    ret1 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAO, 0, val1);
                }


            }
            catch (Exception ex)
            {
            }
            try
            {
                Int32 ret2 = default(Int32);
                double val2 = 0;
                if (Class1.ProgFlag == true)
                {
                    Label31.Text = Class1.R_G1.ToString();
                    val2 = Convert.ToDouble(Class1.R_G1 / 20) * Class1.GCF1;
                    ret2 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAO, 1, val2);
                }
                else
                {
                    Class2.RecipeUpload();
                    Label31.Text = Class1.R_G1.ToString();
                    val2 = Convert.ToDouble(Class1.R_G1 / 20) * Class1.GCF1;
                    ret2 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAO, 1, val2);
                }


            }
            catch (Exception ex)
            {
            }

            try
            {
                Int32 ret3 = default(Int32);
                double val3 = 0;
                if (Class1.ProgFlag == true)
                {
                    Label32.Text = Class1.R_G2.ToString();
                    val3 = Convert.ToDouble(Class1.R_G2 / 20) * Class1.GCF2;
                    ret3 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAO, 2, val3);
                }
                else
                {
                    Class2.RecipeUpload();
                    Label32.Text = Class1.R_G2.ToString();
                    val3 = Convert.ToDouble(Class1.R_G2 / 20) * Class1.GCF2;
                    ret3 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAO, 2, val3);
                }

            }
            catch (Exception ex)
            {
            }

            try
            {
                Int32 ret4 = default(Int32);
                double val4 = 0;
                if (Class1.ProgFlag == true)
                {
                    Label33.Text = Class1.R_G3.ToString();
                    val4 = Convert.ToDouble(Class1.R_G3 / 20) * Class1.GCF3;
                    ret4 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAO, 3, val4);
                }
                else
                {
                    Class2.RecipeUpload();
                    Label33.Text = Class1.R_G3.ToString();
                    val4 = Convert.ToDouble(Class1.R_G3 / 20) * Class1.GCF3;
                    ret4 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAO, 3, val4);
                }

            }
            catch (Exception ex)
            {
            }


            try
            {
                //Analog Output
                Int32 ret5 = default(Int32);
                double val5 = 0;
                if (Class1.ProgFlag == true)
                {
                    Label29.Text = Class1.R_TP.ToString();
                    val5 = Convert.ToDouble(Class1.R_TP / 10);
                    ret5 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAI, 0, val5);

                }
                else
                {
                    Class2.RecipeUpload();
                    Label29.Text = Class1.R_TP.ToString();
                    val5 = Convert.ToDouble(Class1.R_TP / 10);
                    ret5 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAI, 0, val5);

                }


            }
            catch (Exception ex)
            {
            }


            try
            {
                //Analog Output
                Int32 ret6 = default(Int32);
                double val6 = 0;

                if (Class1.ProgFlag == true)
                {
                    Label30.Text = Class1.R_LP.ToString();
                    val6 = Convert.ToDouble(Class1.R_LP / 10);
                    ret6 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAI, 1, val6);

                }
                else
                {
                    Class2.RecipeUpload();
                    Label30.Text = Class1.R_LP.ToString();
                    val6 = Convert.ToDouble(Class1.R_LP / 10);
                    ret6 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAI, 1, val6);

                }

            }
            catch (Exception ex)
            {
            }



            //EM  Add same as in Main
            if (Class1.IOFlg == true)
            {
                Class1.Stopped = false;




            }
            Class1.ProgFlag = false;
        }

        private void Manual_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            Class1.Stopped = true;
            Class1.AutoRFCycle = false;
            valueRead.Abort();
            Button21.Visible = false;
         
        }

      
       
       

        private void Button14_Click(System.Object sender, System.EventArgs e)
        {
            //if(Class1.DI_LeadFrameIn==0 & Class1.DI_LeadFrameOut==0)
            //{
             //Start
           Class2.ProgRead();

           //AlarmBox.Text = "Manual Cycle Started; Pump Started";
           Class2.ManualLogEventDB("002", "Manual Cycle Started; Pump Started");
           Class2.WriteManualEventLog("002", "Manual Cycle Started; Pump Started");
          //if (Class1.Gas1DB == 0.0 && Class1.Gas2DB == 0.0)
          //  {
          //      MRaiseAlarm(19);
          //      return;
          //  }

          //  if (Class1.AlarmPause == true)
          //  {
          //      MessageBox.Show("Please Reset the Outstanding Alarm and then Start again.");
          //      Button14.BackColor = Color.PaleGreen;
          //      Class2.ManualLogEventDB("003", "Manual Cycle can not be Started due to outstanding Alarm");
          //      Class2.WriteManualEventLog("003", "Manual Cycle can not be Started due to outstanding Alarm");
          //      return;
          //  }

            //--------------- close  doors--------------------------------------
            Thread.Sleep(200);
            bothDoorsUpTh = new Thread(new System.Threading.ThreadStart(DoorsUpTh));
            if (Class1.DORunning == true) 
            { Thread.Sleep(200); }
            bothDoorsUpTh.Start();
            //do
            //{ }//wait
            //while (Class1.DI_FrontDoorup == 0);
            Application.DoEvents();
            Thread.Sleep(200);
            Button14.BackColor = Color.Gray;
            Button14.Enabled = false;
            Class1.DoingManualCycle = true;
            Class1.MRFTick = 0;
            Class1.MGasTick = 0;
            Modebtn.Enabled = false;
            GBDoor.Enabled = false;
            Class1.ManualStop = false;
            Class1.ManualCycleStarted = true;
            VentTimeDisplay = Class1.Venttime;
            PVShutoffTime =Convert.ToInt32 (VentTimeDisplay * 0.9);

            Button3.Enabled = false;

            
            n = 0;

            //Class2.Create10DOArray(6, 0);//Close Vent Valve


            Class2.SetDO(Class1.DOSlotNum, 1, true);//Start pump
            Thread.Sleep(100);
           // this.Button13.Visible = true;
            Int32 ret1 = default(Int32);
            double val1 = 0;
             Class1.R_PW=Convert.ToDouble( Label34.Text);
                       try{
                        val1 = Convert.ToDouble(Class1.R_PW / Class1.SetupRFRange * 10);
                        ret1 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAO, 0, val1);

                }
                catch (Exception ex)
                {
                }
               try
                {
                    Int32 ret2 = default(Int32);
                    double val2 = 0;
                   Class1.R_G1=Convert.ToDouble(Label31.Text);
                        
                        val2 = Convert.ToDouble(Class1.R_G1 / 20) * Class1.GCF1;
                        ret2 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAO, 1, val2);
                  }
                catch (Exception ex)
                {
                }


                try
                {
                    Int32 ret3 = default(Int32);
                    double val3 = 0;
                  Class1.R_G2=Convert.ToDouble( Label32.Text);
                  val3 = Convert.ToDouble(Class1.R_G2 / 20) * Class1.GCF2;
                  ret3 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAO, 2, val3);
                    }
                  
                catch (Exception ex)
                {
                }

                try
                {
                    Int32 ret4 = default(Int32);
                    double val4 = 0;
                   Class1.R_G3=Convert.ToDouble(Label33.Text);
                   val4 = Convert.ToDouble(Class1.R_G3 / 20) * Class1.GCF3;
                   ret4 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAO, 3, val4);
                   
                }
                catch (Exception ex)
                {
                }


                try
                {
                    //Analog Output
                    Int32 ret5 = default(Int32);
                    double val5 = 0;
                    Class1.R_TP=Convert.ToDouble(Label29.Text);
                    val5 = Convert.ToDouble(Class1.R_TP / 10);
                    ret5 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAI, 0, val5);
                  


                }
                catch (Exception ex)
                {
                }


                try
                {
                    //Analog Output
                    Int32 ret6 = default(Int32);
                    double val6 = 0;
                    Class1.R_LP = Convert.ToDouble(Label30.Text);
                    val6 = Convert.ToDouble(Class1.R_LP / 10);
                    ret6 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAI, 1, val6);

                }
                catch (Exception ex)
                {
                }
            //}
            //else if (Class1.DI_LeadFrameIn == 1)
            //{
            //    MRaiseAlarm(38);
            //}
            //else if (Class1.DI_LeadFrameOut == 1)
            //{
            //    MRaiseAlarm(36);
            //}
        }

        private void Button13_Click(System.Object sender, System.EventArgs e)
        {
            //Pumpdown

            if (Class1.DO_VacuumON == false)
            {
                this.Button14.Visible = false;
                this.Button14.BackColor = Color.PaleGreen;

              //  Class2.Create10DOArray(7, 1);  //Open Vac valve
                Class2.SetDO(Class1.DOSlotNum, 22, true); //Open Vac valve

                AlarmBox.Text = "Pumping Down";
                try
                {
                    Wait30Sec.RunWorkerAsync();
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                //Class2.Create10DOArray(7, 0); //Close Vac valve
                Class2.SetDO(Class1.DOSlotNum, 22, false); //Close Vac valve
                AlarmBox.Text = "Aborted Pumping Down";
            }

           Class1.ManualGas = true;
        }

        private void Button12_Click(System.Object sender, System.EventArgs e)
        {
            //Add Gasses
            
            Class1.MGasTick = 0;
           
          //if (Class1.DO_StatusMainpurge == false)
          //  {
          //      Thread.Sleep(200);
          //      Class2.Create10DOArray(12, 1);
          //      //Open Main Purge Valve
          //      Class1.ManualGas = true;
          //      this.Button11.Visible = true;
          //  }
          //  else
          //  {
          //      Thread.Sleep(200);
          //      Class2.Create10DOArray(12, 0);
          //      //Close Main Purge Valve
          //      Class1.ManualGas = false;
          //      this.Button11.Visible = false;
          //  }
            

            if (Class1.ManualGas == true)
            {
                AlarmBox.Text = "Adding Process Gases";

                if (Class1.R_G1 > 0)
                {
                    Thread.Sleep(200);
                    //Class2.Create10DOArray(10, 1);
                    Class2.SetDO(Class1.DIOSlotNum, Class1.DO24, true);//Gas1 on

                    Thread.Sleep(100);
                }

                if (Class1.R_G2 > 0)
                {
                    Thread.Sleep(200);
                   // Class2.Create10DOArray(11, 1);
                    Class2.SetDO(Class1.DIOSlotNum, Class1.DO25, true);//Gas2 on
                }

               /* if (Class1.R_G3 > 0)
                {
                    Class2.Create10DOArray(10, 1);
                }*/
                //Class2.Create10DOArray(2, 1) 'Open Main Purge Valve
                Class1.ManualGas = false;
            }
            else
            {
                Thread.Sleep(200);

                int[] DIOSlotNoArr = { Class1.DIOSlotNum, Class1.DIOSlotNum };
                int[] DIOChannelArr = { Class1.DO24, Class1.DO25 }; //Gas1 off,Gas2 off
                bool[] DIOStateArr = { false, false };
                Class2.SetMultiDIO(DIOSlotNoArr, DIOChannelArr, DIOStateArr);

                //Class2.SetDO(Class1.DIOSlotNum, Class1.DO24, false);//Gas1 off
                //Class2.SetDO(Class1.DIOSlotNum, Class1.DO25, false);//Gas2 off

                AlarmBox.Text = "Aborting Process Gases";

                Class1.ManualGas = true;
            }
            this.Button11.Visible = true;
            

        }


        private void Button11_Click(System.Object sender, System.EventArgs e)
        {
            //Plasma
            
            if (Class1.Intlk == true)
            {
                Class1.MRFTick = 0;
                if (Class1.DO_RFON == false)
                {
                    //Class2.Create10DOArray(17, 1);
                    Class2.SetDO(Class1.DOSlotNum, 0, true);

                    AlarmBox.Text = "Plasma On";
                }
                else
                {
                    //Class2.Create10DOArray(17, 0);
                    Class2.SetDO(Class1.DOSlotNum, 0, false);
                    AlarmBox.Text = "Plasma Off";
                }
            }
            else
            {
                MessageBox.Show("Sorry, RF Interlock not satisfied!");
            }

          
        }


        private void Button9_Click(System.Object sender, System.EventArgs e)
        { //STOP
            AlarmBox.Text = "Manual Cycle Set To Stop";

            //if (Class1.DO_StatusPressValve == true)
            //{
            //    if (VentTimeDisplay < PVShutoffTime)
            //        Class2.Create10DOArray(5, 0);  //Close Pressure Valve
            //    Thread.Sleep(500);
            //}
           // Class2.Create10DOArray(5, 0);  
            Class2.SetDO(Class1.DOSlotNum, 20, false); //Close Pressure Valve

            Thread.Sleep(200);
            Button14.BackColor = Color.Gray;
            Button3.BackColor = Color.Gray;
            Modebtn.BackColor = Color.Gray;
            Button14.Enabled = false;
            Button3.Enabled = false;
            Class1.ManualCycleStarted = false;
            //Modebtn.Enabled = false;

           
            Class1.AutoRFCycle = false;
            Class1.FVent = false;
            if (Class1.Autocycle == true)
            {
             Class1.Autocycle = false;
            AutocycleTh.Abort();
            }
           
            TTick = 0;
            Label35.Text = TTick.ToString();
            Button14.BackColor = Color.PaleGreen;
            Class2.ManualLogEventDB("004", "Manual Cycle stopped");
            Class2.WriteManualEventLog("004", "Manual Cycle stopped");
            VentNow();



        }

        private void BPrograms_Click(System.Object sender, System.EventArgs e)
        {
            if (Class1.IOShowInfoForm!=null)
            { Class1.IOShowInfoForm.Hide();}
            
            Programs objProg = new Programs();
            objProg.ShowDialog();
        }


        private void AlarmThread_RunWorkerCompleted(System.Object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            AlarmBox.Text = Class1.AlarmMsg;
        }



        private object MRaiseAlarm(int Alarm)
        {
            object functionReturnValue = null;
            if (Alarm == 0)
            {
                Class1.AlarmMsg = "";
                Class1.SendAlarm = 0;
                Class1.AlarmPause = false;
                return 0;

            }
            Class1.AlarmPause = true;
            Class1.SendAlarm = Alarm;
            AlarmHandler();
            //SetText(AlarmMsg);
            AlarmBox.Invoke((MethodInvoker)delegate { AlarmBox.Text = Class1.AlarmMsg.ToString(); });
            Class2.WriteManualAlarmLog(Class1.AlarmMsg.ToString());
            try
            {
                WirteLogThread.RunWorkerAsync();

            }
            catch (Exception e) { Console.WriteLine(e); }

            //this.Invoke((MethodInvoker)delegate { this.Refresh(); });
            // return 0;

            return functionReturnValue;

        }

        private void ResetAlarm_Click(System.Object sender, System.EventArgs e)
        {
            //if (Class1.DI_LeadFrameIn == 1)
            //{
            //    return;
            //}
            //if (Class1.DI_LeadFrameOut == 1)
            //{
            //    return;
            //}
            ResetAlarmExit();
        }
        private void ResetAlarmExit()
        {
            Class1.AlarmPause = false;
            MRaiseAlarm(0);
            Class1.SendAlarm = 0;
            AlarmBox.Text = "";
            Class1.FatalError = false;
        }
        private void Button15_Click(System.Object sender, System.EventArgs e)
        {
            if (Class1.IOShowInfoForm!=null)
            { Class1.IOShowInfoForm.Hide();}
            
            Setup1 objSetup1 = new Setup1();
            objSetup1.ShowDialog();
            
        }

        private void Button2_Click(System.Object sender, System.EventArgs e)
        {
            TunerPage objTuner = new TunerPage();
            objTuner.Show();
            
        }



        private void Button7_Click(System.Object sender, System.EventArgs e)
        {
            Class2.ManualLogEventDB("002", "I/O Page Loaded");
            Class2.WriteManualEventLog("002", "I/O Page Loaded");
            Class1.CMP = true; // set the doingmanualcycle = true because we called the page from the Manual Page
            Class1.Stopped = true;
            Class1.Connected = true;
            Class1.btnAutoStart.Enabled = false;
            valueRead.Abort();
            if(Class1.LeakOpen==true)
            { Class1.objLeak.Close(); }
            if (Class1.IOShowInfoForm!=null)
            { Class1.IOShowInfoForm.Hide();}
            //Class1.Connobj.Show();
            
            Class1.RetForm.Show();
            this.Hide();
         }

      
        private void Button19_Click(System.Object sender, System.EventArgs e)
        {
            Documentation objDoc = new Documentation();
            objDoc.ShowDialog();
         }


        private void Button6_Click(System.Object sender, System.EventArgs e)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            try
            {
                p.StartInfo.FileName = Class1.sProjectPath + "\\Manuals\\Dummy.pdf";
                p.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("cannot find the requested file.");
            }
        }
        
        
        private void Button3_Click(System.Object sender, System.EventArgs e)
        {
            Class1.btnManualStart = Button14;
            Class1.GBManualDoors = GBDoor;
            Class1.btnManualMode = Modebtn;
            Class1.btnManualLeak = Button3;
            Class1.btnManualProg = BPrograms;
            Class1.btnManualSetup = Button15;
            Class1.btnIOManual = Button7;
            
            Class1.btnManualTuner = Button2;
            Class1.btnManualStop = Button9;
            Class1.btnManualStop.Enabled = false;
           // Class1.btnMaualSMEMA.Enabled = false;
            Class1.btnManualStart.Enabled = false;
            Class1.GBManualDoors.Enabled = false;
            Class1.btnIOManual.Enabled = false;
            Class1.btnManualMode.Enabled = false;
            Class1.btnManualLeak.Enabled = false;
            Class1.btnManualProg.Enabled = false;
            Class1.btnManualSetup.Enabled = false;
            Class1.btnManualTuner.Enabled = false;
            
            
            if (Class1.objLeak == null)
            {
                Class1.objLeak = new Leak();
                Class1.objLeak.Show();
                Class1.LeakOpen = true;
            }
            Class1.objLeak.TopMost = true;
            if (Class1.openIO!=null)
            { Class1.openIO.Location = new Point(10, 10);}
            
        }

        private void Button1_Click(System.Object sender, System.EventArgs e)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            try
            {
                p.StartInfo.FileName = Class1.sProjectPath + "\\Manuals\\Dummy.pdf";
                
                p.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("cannot find the requested file.");
            }
        }

        private void Button5_Click(System.Object sender, System.EventArgs e)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            try
            {
                p.StartInfo.FileName = Class1.sProjectPath + "\\Manuals\\Dummy.pdf";
                p.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("cannot find the requested file.");
            }
        }

        private void Button4_Click(System.Object sender, System.EventArgs e)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            try
            {
                p.StartInfo.FileName = Class1.sProjectPath + "\\Manuals\\Dummy.pdf";
                p.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("cannot find the requested file.");
            }
        }

        private void Button8_Click(System.Object sender, System.EventArgs e)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            try
            {
                p.StartInfo.FileName = Class1.sProjectPath + "\\Manuals\\Dummy.pdf";
                p.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("cannot find the requested file.");
            }
        }

        

        private void AlarmHandler()
        {
            ManualAlarmTAobj.Fill(ManualsetupDatasetObj.AlarmLogTable);
            switch (Class1.SendAlarm)
            {
                case 0:
                    break; // TODO: might not be correct. Was : Exit Select

                    break;
                case 1:
                    Class1.AlarmMsg = "# 1 - No Air";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 1, "No Air", "Check the Air connection, valve and sensor.");
                    Class1.FatalError = true;
                    break;
                case 2:
                    Class1.AlarmMsg = "# 2 - Pump Error";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 2, "Pump Error", "Check the Vacuum Pump connection and temperature.");
                    Class1.FatalError = true;
                    break;
                case 3:
                    Class1.AlarmMsg = "# 3 - No Plasma";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 3, "No Plasma", "Check the Program setting, Tuner and RF generator.");
                    break;
                //FatalError = True
                case 4:
                    Class1.AlarmMsg = "# 4 - High Reflected Power";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 4, "High Reflective Power", "Check the Program setting, Tuner and RF generator.");
                    break;
                case 5:
                    Class1.AlarmMsg = "# 5 - No Gas 1";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 5, "No Gas 1", "Check the Gas 1 supply line and bottle.");
                    Class1.FatalError = true;
                    break;
                case 6:
                    Class1.AlarmMsg = "# 6 - No Gas 2";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP,DateTime.Now, 6, "No Gas 2", "Check the Gas 2 supply line and bottle.");
                    Class1.FatalError = true;
                    break;
                case 7:
                    Class1.AlarmMsg = "# 7 - No Gas 3";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 7, "No Gas 3", "Check the Gas 3 supply line and bottle.");
                    Class1.FatalError = true;
                    break;
                case 8:
                    Class1.AlarmMsg = "# 8 - PumpDown Error";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 8, "Pump Down Error", "Check the program settings,the chamber O-Rings and the Pump.");
                    break;
                case 9:
                    Class1.AlarmMsg = "# 9 - Door Sensor Error";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 9, "Door Sensor Error", "You cannot Start with the door open; or, check the door sensor.");
                    break;
                case 10:
                    Class1.AlarmMsg = "# 10 - E-Stop";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 10, "E-Stop", "The E-Stop is engaged.");
                    Class1.FatalError = true;
                    break;
                case 11:
                    Class1.AlarmMsg = "# 11 - MFC 1";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 11, "MFC 1", "Check Gas Valve 1 Or, Check the mass flow Controller.");
                    break;
                case 12:
                    Class1.AlarmMsg = "# 12 - MFC 2";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 12, "MFC 2", "TCheck Gas Valve 1 Or, Check the mass flow Controller.");
                    break;
                case 13:
                    Class1.AlarmMsg = "# 13 - MFC 3";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 13, "MFC 3", "Check Gas Valve 1 Or, Chec the mass flow Controller.");
                    break;
                case 14:
                    Class1.AlarmMsg = "# 14 - Pres.Trig. Timeout";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 14, "Pres.Trig. Timeout", "The program Settings have too mach gas and/or too low Pressure Trigger.");
                    break;
                case 15:
                    Class1.AlarmMsg = "# 15 - No Purge Gas";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 15, "No Purge Gas", "Check the Gas 1 supply line and bottle");
                    break;
                case 16:
                    Class1.AlarmMsg = "# 16 - Reaching Plasma Pressure Timeout";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 16, "Reaching Plasma Pressure Timeoutt", "The System Cannot reach the Pressure trigger Point set in your recipe. Please check if the Gas value set in the recipe is correct, or check the gas lines.");
                    break;
                case 17:
                    Class1.AlarmMsg = "# 17 - RF Interlock Error";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 17, "RF Interlock Error", "The System tried to turn on the RF power and a too high vacuum pressure.");
                    break;
                case 18:
                    Class1.AlarmMsg = "# 18 - Plasma Interrupt";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 18, "Plasma Interrupt", "During Plasma The Stop Button or the E-Stop have been pressed, or there has been a power failure.");
                    break;
                case 19:
                    Class1.AlarmMsg = "# 19 - Both Gas lines set to 0";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 19, "Both gas lines set to 0", "You are attempting to start an Automatic or Manual cycle without any gas set in your program.");

                    break;
                case 20:
                    Class1.AlarmMsg = "# 20 - Both the Doors are not Up Error";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 20, "Both the Doors Up Error", "indicates that both doors have not reached their up sensors.");
                    break;
                case 21:
                    Class1.AlarmMsg = "# 21 - Front Door is not Up Error";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 21, "Front Door Up Error", "indicates tha the front door only has not reached its up sensor.");
                    break;
                case 22:
                    Class1.AlarmMsg = "# 22 - Back Door is not Up Error";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 22, "Back Door Open Error", "indicates tha the back door only has not reached its up sensor.");
                    break;
                case 23:
                    Class1.AlarmMsg = "# 23 - Both the Doors are not Closed Error";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 23, "Both the Doors are not Closed Error", "Indicates that both doors are not closeed.");
                    break;
                case 24:
                    Class1.AlarmMsg = "# 24 - Front Door is not Closed Error";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 24, "Front Door is not Closed Error", "Indicates the the front door is not closed.");
                    break;

                case 25:
                    Class1.AlarmMsg = "# 25 - Back door is not closed Error";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 25, "back door is not closed Error", "Indicates the the back door is not closed.");
                    break;

                case 26:
                    Class1.AlarmMsg = "# 26 - Both doors are not open Error";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 26, "Both doors are not open Error", "Indicates that both doors are not open.");
                    break;

                case 27:
                    Class1.AlarmMsg = "# 27 - Front door is not open Error";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 27, "Front door is not open Error", "Indicates the the front door is not open.");
                    break;

                case 28:
                    Class1.AlarmMsg = "# 28 - Back door is not open Error";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 28, "Back door is not open Error", "Indicates the the back door is not open.");
                    break;

                case 29:
                    Class1.AlarmMsg = "# 29 - Both doors are not down Error";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 29, "Both doors are not down Error", "Indicates that both doors are not down.");
                    break;

                case 30:
                    Class1.AlarmMsg = "# 30 - Front door is not down Error";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 30, "Front door is not down Error", "Indicates the the front door is not down.");
                    break;

                case 31:
                    Class1.AlarmMsg = "# 31 - back door is not down Error";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 31, "Back door is not down Error", "Indicates the the back door is not down.");
                    break;
                case 32:
                    Class1.AlarmMsg = "# 32 - DO function aborted.";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 32, "Function error", "CreateArray aborted.Call for Service.");
                    break;
                case 36:
                    Class1.AlarmMsg = "# 36 - LeadFrameOut Sensor Error";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 36, " LeadFrameOut Sensor Error", " LeadFrameOut Sensor Error.");
                    break;

                case 38:
                    Class1.AlarmMsg = "# 38 - LeadFrame is in Input Conveyor";
                    ManualAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 38, "LeadFrame is in Input Conveyor", "LeadFrame is in Input Conveyor.");
                    break;
                
            }
            //fileAlarm = System.IO.File.Create("c:\Filelog" & Now.ToString & ".txt")

        }

       
        private void SetBackcolor(System.Drawing.Color col)
        {
            if (this.AirSensor.InvokeRequired)
            {
                SetAirsensor d = new SetAirsensor(SetBackcolor);
                this.Invoke(d, new object[] { col });
            }
            else
            {
                this.AirSensor.BackColor = col;
            }
        }

        private void Wait30Sec_DoWork(System.Object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Thread.Sleep(1000);
         
        }

        private void Wait30Sec_RunWorkerCompleted(System.Object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (Class1.ManualStop == false)
            {
                Thread.Sleep(200);
                //Class2.Create10DOArray(5, 1);
                Class2.SetDO(Class1.DOSlotNum, 20, true);// Pressure Valve open
                
                if (Class1.FVent == false)
                {
                    this.Button12.Visible = true;
                }

            }
        }

        private void StopManualRun_RunWorkerCompleted(System.Object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
           Class1.DoingManualCycle = false;
           Thread.Sleep(200);
          
           int[] DOSlotNoArr = { Class1.DOSlotNum, Class1.DOSlotNum };
           int[] DOChannelArr = { 0, 22 }; //RF off,Vacuum Off,Gas1 off,Gas2 off,Pump off
           bool[] DOStateArr = { false, false };
           Class2.SetMultiDO(DOSlotNoArr, DOChannelArr, DOStateArr);

           int[] DIOSlotNoArr = {  Class1.DIOSlotNum };
           int[] DIOChannelArr = { Class1.DO25 }; //RF off,Vacuum Off,Gas1 off,Gas2 off,Pump off
           bool[] DIOStateArr = { false };
           Class2.SetMultiDIO(DIOSlotNoArr, DIOChannelArr, DIOStateArr);
           Class2.SetDO(Class1.DIOSlotNum, Class1.DO24, false);//Gas1 off
           Class2.SetDO(Class1.DOSlotNum, 1, false);//Pump off

           //Class2.SetDO(Class1.DOSlotNum, 0, false); //RF off
          // Class2.SetDO(Class1.DOSlotNum, 22, false);//Vacuum Off
         //  Class2.SetDO(Class1.DIOSlotNum, Class1.DO24, false);//Gas1 off
         //  Class2.SetDO(Class1.DIOSlotNum, Class1.DO25, false);//Gas2 off
          
        }

        private void VentTimerTh()
        {
            do
            {
                Thread.Sleep(1000);
                
                if (StopManualRun.IsBusy == false)
                {
                    VentTimeDisplay = VentTimeDisplay - 1;
                    if(IsHandleCreated)
                    LBLVentTime.Invoke((MethodInvoker)delegate { LBLVentTime.Text = VentTimeDisplay.ToString(); });
                   
                    if (VentTimeDisplay < 1)
                    {
                        //Open Doors
                        bothDoorsOpenTh = new Thread(new System.Threading.ThreadStart(OpenDoorsTh));
                        bothDoorsOpenTh.Start();
                        Application.DoEvents();
                        Thread.Sleep(500);
                        //Class2.Create10DOArray( 6, 0);
                        Class2.SetDO(Class1.DOSlotNum, 21, false);
                        if(IsHandleCreated)
                        {
                            Button14.Invoke((MethodInvoker)delegate { Button14.Visible = true; Button14.Enabled = true; });
                        Modebtn.Invoke((MethodInvoker)delegate { Modebtn.Enabled = true; Modebtn.BackColor = Color.CornflowerBlue; Button3.Enabled = true; GBDoor.Enabled = true; LBLVentTime.Visible = false; });
                        }
                      
                        
                        Button3.BackColor = Color.Gold;
                        
                       
                       

                        DidEndRun = false;
                        Class1.FVent = false;
                        //Class2.Create10DOArray(6, 0); //close Purge Valve
                        if(IsHandleCreated)
                        LBLVentTime.Invoke((MethodInvoker)delegate { LBLVentTime.Text = ""; AlarmBox.Text = ""; });
                       
                        if(ventTh!=null)
                        { ventTh.Abort(); }
                        
                        return;

                    }

                    //SHUT OFF THE PRESSURE VALVE DURING VENTING

                   
                    //Please Do not delete*********************************
                    //if (VentTimeDisplay <= 0 | Class1.FVent == true)
                    //{
                    //    Class2.Create10DOArray(6, 0); //close Purge Valve
                    //    Class2.Create10DOArray(12, 0); //close Main Purge Valve

                    //    //TODO: activate door open when the rest has been debugged

                    //    this.Button14.Visible = true;
                    //    Modebtn.Enabled = true;
                    //    Button3.Enabled = true;
                    //    Button3.BackColor = Color.Gold;
                    //    Modebtn.BackColor = Color.CornflowerBlue;
                    //    LBLVentTime.Visible = false;

                    //    DidEndRun = false;
                    //    Class1.FVent = false;
                      
                    //    if (ventTh != null)
                    //    { ventTh.Abort(); }

                    //    bothDoorsOpenTh.Abort();
                    //    return;
                    //}
                    //Please Do not delete*********************************

                }
            } while (true);
        }


        //private void Button16_Click(System.Object sender, System.EventArgs e)
        //{
        //    this.Button14.Visible = true;
        //    Button3.Enabled = true;
        //    Button3.BackColor = Color.Gold;
        //    Modebtn.BackColor = Color.CornflowerBlue;
        //    Button14.BackColor = Color.PaleGreen;
        //    Class1.AutoRFCycle = false;
        //    LBLVentTime.Visible = false;

        //    if (ventTh != null)
        //    { ventTh.Abort(); }
        //    Class2.Create10DOArray(6, 0);   //close Purge Valve
        //    Class2.Create10DOArray(12, 0);  //close Main Purge Valve
        //    Class1.ManualGas = false;
        //    Class1.FVent = true;
        //    this.Button14.Visible = true;
        //    Modebtn.Enabled = true;
        //    LBLVentTime.Visible = false;

        //    this.Button11.Visible = false;
        //    this.Button12.Visible = false;
        //    this.Button13.Visible = false;
        //    Class1.Autocycle = false;
        //    AutocycleTh.Abort();
        //    TTick = 0;
        //    Label35.Text = TTick.ToString();
        //    Class1.DoingManualCycle = false;

        //    ForceVent objForceVent = new ForceVent();
        //    objForceVent.ShowDialog();

        //}



        private void PictureBox2_Click(System.Object sender, System.EventArgs e)
        {
            if (Class1.DO_StatusMainpurge == true | Class1.DO_RFON == true)
            {
                MessageBox.Show("Attention, to close the Pressure Valve now, you must first close your process gas flow and / or turn off Plasma!");
                return;
            }
            if (Class1.DO_PressureON == false)
            {
                //Class2.Create10DOArray(5, 1); 
                Class2.SetDO(Class1.DOSlotNum, 20, true);// Pressure Valve on
                this.Button12.Visible = true;
                Wait30Sec.CancelAsync();
            }
            else
            {
                //Class2.Create10DOArray(5, 0); // Pressure Valve
                Class2.SetDO(Class1.DOSlotNum, 20, false);// Pressure Valve off
                this.Button12.Visible = false;
                this.Button11.Visible = false;
            }
        }

        private object VentNow()
        {
           
            VentTimeDisplay =Class1.Venttime;
            
            ventTh = new Thread(new System.Threading.ThreadStart(VentTimerTh));
            ventTh.Start();
            Class1.ManualStop = true;
            Class1.ManualGas = false;
            Class1.MRFTick = 0;
            Class1.MGasTick = 0;
           // Class2.Create10DOArray(6, 1);
            Class2.SetDO(Class1.DOSlotNum, 21, true);
          
            if (StopManualRun.IsBusy == false)
            {
                StopManualRun.RunWorkerAsync();

            }
            DidEndRun = true;
            this.Button11.Visible = false;
            this.Button12.Visible = false;
            this.Button13.Visible = false;
            LBLVentTime.Visible = true;
            Class1.DoingManualCycle = false;
            
            return 0;
        }

        private void Button21_Click(System.Object sender, System.EventArgs e)
        {
            if (Class1.Intlk == true)
            {
                if (Class1.Autocycle == true)
                {
                    Class1.Autocycle = false;
                    AutocycleTh.Abort();
                    Button21.Text = "Turn RF AutoCycle ON";
                    //Class2.Create10DOArray(18, 1);  
                    //Class2.Create10DOArray(18, 0);

                    int[] DOSlotNoArr = { Class1.DOSlotNum, Class1.DOSlotNum };
                    int[] DOChannelArr = { 2, 0 }; //Manual tuner on,RF off
                    bool[] DOStateArr = { true,false };
                    Class2.SetMultiDO(DOSlotNoArr, DOChannelArr, DOStateArr);
                    
                    //Class2.SetDO(Class1.DOSlotNum, 2, true);//Manual tuner on
                    //Class2.SetDO(Class1.DOSlotNum, 0, false); // RF Off

                    Class1.AutoRFCycle = false;
                }
                else
                {
                    AutocycleTh = new Thread(new System.Threading.ThreadStart(AutocycleTimer));
                    AutocycleTh.Start();
                    Class1.Autocycle = true;
                    Button21.Text = "Turn RF AutoCycle OFF";
                    TTick = 0;
                    Class1.AutoRFCycle = true;
                }
            }
            else
            {
                MessageBox.Show("Interlock Active");
                Button21.Text = "Turn RF AutoCycle ON";
            }

        }



        private void AutocycleTimer()
        {
            do
            {
                Thread.Sleep(1000);
                TTick = TTick + 1;
                Label35.Invoke((MethodInvoker)delegate { Label35.Text = TTick.ToString(); });
               


                if (TTick > 5 & TTick < 8)
                {
                   // Class2.Create10DOArray(18, 0); // Auto Tuner

                    int[] DOSlotNoArr = { Class1.DOSlotNum, Class1.DOSlotNum };
                    int[] DOChannelArr = { 2, 3 }; //Manual tuner on,RF off
                    bool[] DOStateArr = {  false, true};
                    Class2.SetMultiDO(DOSlotNoArr, DOChannelArr, DOStateArr);

                    //Class2.SetDO(Class1.DOSlotNum, 2, false);//Manual Tuner off
                    //Class2.SetDO(Class1.DOSlotNum, 3, true); //Auto Tuner On

                }

                if (TTick < 3 & TTick > 0)
                {
                   // Class2.Create10DOArray(17, 1); //RF ON
                    Class2.SetDO(Class1.DOSlotNum, 0, true); //RF ON
                }
                else if (TTick > 59)  //& TTick < 30
                {
                   // Class2.Create10DOArray(17, 0); 
                    Class2.SetDO(Class1.DOSlotNum, 0, false); // RF OFF
                }
               
                
                if (TTick >= 80)
                {
                    TTick = 0;
                }
            } while (Class1.Autocycle = true);
        }

        private void BRed_Click(System.Object sender, System.EventArgs e)
        {
            if (GhostTick == 0)
            {
                GhostTick = 1;
            }
            else if (GhostTick == 3)
            {
                Button21.Visible = true;

                Label35.Visible = true;
            }
        }

        private void BYellow_Click(System.Object sender, System.EventArgs e)
        {
            if (GhostTick == 1)
            {
                GhostTick = 2;
            }
        }

        private void BGreen_Click(System.Object sender, System.EventArgs e)
        {
            if (GhostTick == 2)
            {
                GhostTick = 3;
            }
        }


        private void WirteLogThread_DoWork(System.Object sender, System.ComponentModel.DoWorkEventArgs e)
        {

            NowDate = DateTime.Now.ToString();
            NowDate = NowDate.Replace("/", "-");
            NowDate = NowDate.Replace(":", "-");

            if (DateTime.Now.Date >Class1.FileStartTime)
            {
                MainPath = ("C:/Program Files/SG25/AlarmLog/Logfile " + NowDate + ".csv");
            }

            try
            {
                if (string.IsNullOrEmpty(Class1.AlarmMsg))
                {
                    //do nothing
                }
                else
                {
                    FS = File.AppendText(MainPath);
                    FS.WriteLine(DateTime.Now + "  Program#  " +Class1.CurrentP + "  Alarm  " +Class1.AlarmMsg + " , ");
                    FS.Flush();
                    FS.Close();

                }
            }
            catch
            {
            }
        }
        private void Button14_Enter(System.Object sender, System.EventArgs e)
        {
            //if (Class1.DI_LeadFrameIn == 0 & Class1.DI_LeadFrameOut == 0)
            //{
                Button14.BackColor = Color.Gray;
                Button3.BackColor = Color.Gray;
                Modebtn.BackColor = Color.Gray;
            //}
        }


        private void Button22_Click(System.Object sender, System.EventArgs e)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            try
            {
                p.StartInfo.FileName = Class1.sProjectPath + "\\Manuals\\Dummy.pdf";
                p.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("cannot find the requested file.");
            }
        }

        private void BBuzzer_Click(object sender, EventArgs e)
        {
            
               // Class2.Create10DOArray(30, 0); 
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO29, false); //Turn off Buzzer
           Class1.FatalError = false;
        }

        private void Exit_Click(object sender, EventArgs e)
        {

        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

 
        private void rdCloseDoor_Click(object sender, EventArgs e) // close both doors
        { 
        //    if(Class1.DI_LeadFrameIn==0 & Class1.DI_LeadFrameOut==0)
        //    {
        //    bothDoorsUpTh = new Thread(new System.Threading.ThreadStart(DoorsUpTh));
        //    bothDoorsUpTh.Start();
        //    rdOpenDoor.Enabled = false;
        //    rdCloseDoor.Enabled = false;
        //    }
        //else if (Class1.DI_LeadFrameIn == 1)
        //{
        //    MRaiseAlarm(38);
        //}
        //else if (Class1.DI_LeadFrameOut == 1)
        //{
        //    MRaiseAlarm(36);
            //} 
           
        }


        private void DoorsUpTh()
        {
            //Class2.CreateDoorsDOArray(1, 1, 3, 1, 2, 0, 4, 0); // Send both doors up with 1 new function CreateDOArrayDoors
            //DateTime StartTime = DateTime.Now ;
            //DateTime NewTime;
            //TimeSpan Diff;
            //Class1.Milliseconds = 0;
           
            //   do
            //    {
                    
            //        NewTime = DateTime.Now;
            //        Diff = NewTime - StartTime;
            //        Class1.Milliseconds = (int)Diff.Seconds ;
            //        if (Class1.Milliseconds > 2)
            //        { 
            //            MRaiseAlarm(20);
            //            Class2.CreateDoorsDOArray(8, 0, 9, 0);
                        
            //            Class2.CreateDoorsDOArray(1, 0, 3, 0, 2, 1, 4, 1); // Send both doors down with 1 new function Create4DOArrayDoors
            //            rdOpenDoor.Invoke((MethodInvoker)delegate { rdOpenDoor.Enabled = true; });
            //            rdCloseDoor.Invoke((MethodInvoker)delegate { rdCloseDoor.Enabled = true; });
            //            bothDoorsUpTh.Abort();
            //            return;
            //        }
                         
            //    }
            //   while (Class1.DI_FrontDoorup == 0 & Class1.DI_BackDoorUp == 0);
            //   CloseDoorsTh();
               
        }

        private void CloseDoorsTh()
        {
            ////Thread.Sleep(200);
            //Class2.CreateDoorsDOArray(8, 1, 9, 1);

            //if (Class1.DI_FrontDoorup == 0 & Class1.DI_BackDoorUp == 0)
            //{   MRaiseAlarm(23);
            //Class2.CreateDoorsDOArray(1, 0, 3, 0, 2, 1, 4, 1); // Send both doors down with 1 new function Create4DOArrayDoors
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
            //bothDoorsOpenTh = new Thread(new System.Threading.ThreadStart(OpenDoorsTh));
            //bothDoorsOpenTh.Start();
            //rdCloseDoor.Enabled = false;
            //rdOpenDoor.Enabled = false;
           
        }
        private void OpenDoorsTh()
        {
            //Class2.CreateDoorsDOArray(8, 0, 9, 0); //open the doors

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
            //        MRaiseAlarm(29);
            //        rdOpenDoor.Invoke((MethodInvoker)delegate { rdOpenDoor.Enabled = true; });
            //        rdCloseDoor.Invoke((MethodInvoker)delegate { rdCloseDoor.Enabled = true; });
            //        bothDoorsOpenTh.Abort();
            //        return;
            //    }

            //}
            //while (Class1.DI_ChDoor_Front == 0 & Class1.DI_ChDoorBack == 0);
            ////Thread.Sleep(200);
            //DoorsDownTh();
                     
        }

        private void DoorsDownTh()
        {

           // Class2.CreateDoorsDOArray(1, 0, 3, 0, 2, 1, 4, 1); //Reset both up coils
            Thread.Sleep(600);

            if (Class2.SetDoorsAlarm == true) { 
                MRaiseAlarm(32);
                Class2.SetDoorsAlarm = false;
            }
            if (IsHandleCreated)
            {
                rdOpenDoor.Invoke((MethodInvoker)delegate { rdOpenDoor.Enabled = true; });
                rdCloseDoor.Invoke((MethodInvoker)delegate { rdCloseDoor.Enabled = true; });
            }

            bothDoorsOpenTh.Abort();

        }

        private void ValueReadBW()
        {
            DateTime STime;
            DateTime ETime;
            TimeSpan Diff;
            Class2.DOArray = Class1.DOStatus2;
            do
            {
                
                STime = DateTime.Now;
                Thread.Sleep(190); // Changed from 10 msec to 190
                
                if (Class1.Add200msec == true) { Thread.Sleep(200); }
                if (Class1.DO_ManualTuner == false)
                {if(IsHandleCreated)
                { AutoMan .Invoke ((MethodInvoker )delegate{AutoMan.BackColor =Color.Green; AutoMan .Text ="Auto";});}
                    
                }
                else
                {
                    if (IsHandleCreated) { AutoMan.Invoke((MethodInvoker)delegate { AutoMan.BackColor = Color.Red; AutoMan.Text = "Man"; }); }
                   
                }


                if (IsHandleCreated) {  LError.Invoke((MethodInvoker)delegate { LError.Text = Class1.ApsRet.ToString(); });}
               
                
                DateTime StartC = DateTime.Now;
                if (Class1.DI_Air == 0)
                {
                    //SetBackcolor(Color.Red);
                    AirSensor.BackColor = Color.Red;
                }
                else
                {
                    AirSensor.BackColor = Color.Green;
                    //SetBackcolor(Color.Green);
                }
             
                if (Class1.DI_EStop == 0)
                {
                    LBLEstop.Invoke((MethodInvoker)delegate { LBLEstop.Visible = true; });

                    //AlarmBox.Invoke((MethodInvoker)delegate { AlarmBox.Text = Class1.AlarmMsg; GroupBox5.Text = "Alarms"; });

                }
                else
                {
                    LBLEstop.Invoke((MethodInvoker)delegate { LBLEstop.Visible = false; });


                }


                if (Class1.DI_PumpOK == 0)
                {
                    PumpOnOff.BackColor = Color.Red;
                    if (IsHandleCreated)
                    { Button13.Invoke((MethodInvoker)delegate { Button13.Visible = false; }); }
                }
                else
                {
                    PumpOnOff.BackColor = Color.Green;
                    if (IsHandleCreated) {  Button13.Invoke((MethodInvoker)delegate { Button13.Visible= true; });}
                   

                }

                //---------------------------***CHECK**--------------------------


                if (Class1.DI_ChamberUP == 1)
                {
                    ChamberClosed.Invoke((MethodInvoker)delegate { ChamberClosed.Visible = false; });

                }
                else
                {
                    ChamberClosed.Invoke((MethodInvoker)delegate { ChamberClosed.Visible = true; });

                }
                //------------------------------------****CHECK***------------------------------

                if (Class1.DO_PumpON == false)
                {
                    picPump.Invoke((MethodInvoker)delegate { picPump.Visible = false; });

                }
                else
                {
                    picPump.Invoke((MethodInvoker)delegate { picPump.Visible = true; });

                }
                if (Class1.DO_VacuumON == false)
                {


                    VacOff.Invoke((MethodInvoker)delegate { VacOff.Visible = true; });
                    VacOn.Invoke((MethodInvoker)delegate { VacOn.Visible = false; });

                    if (Class1.DO_VentON == false)
                    {
                        VacValve.BackColor = Color.Black;
                    }
                    else
                    {
                        VacValve.BackColor = Color.Green;
                    }

                }
                else
                {
                    VacValve.BackColor = Color.Green;
                    VacOff.Invoke((MethodInvoker)delegate { VacOff.Visible = false; });
                    VacOn.Invoke((MethodInvoker)delegate { VacOn.Visible = true; });

                }



                if (Class1.DO_PressureON == false)
                {
                    Press1.BackColor = Color.Black;
                    Press2.BackColor = Color.Black;
                    XLAOFF.Invoke((MethodInvoker)delegate { XLAOFF.Visible = true; });
                    XLAON.Invoke((MethodInvoker)delegate { XLAON.Visible = false; });

                }
                else
                {
                    Press1.BackColor = Color.Green;
                    Press2.BackColor = Color.Green;
                    XLAOFF.Invoke((MethodInvoker)delegate { XLAOFF.Visible = false; });
                    XLAON.Invoke((MethodInvoker)delegate { XLAON.Visible = true; });

                }



                if (Class1.DO_VentON == false)
                {
                   // Button10.BackColor = Color.Black;
                    PurgeOFF.Invoke((MethodInvoker)delegate { PurgeOFF.Visible = true; });
                    PurgeON.Invoke((MethodInvoker)delegate { PurgeON.Visible = false; });
                    Size value = new Size(114, 25);
                    Button17.Invoke((MethodInvoker)delegate {  Button17.Text = ""; Button17.BackColor = Color.Black; });

                }
                else
                {
                    Size value = new Size(346, 25);
                    Button17.Invoke((MethodInvoker)delegate { Button17.Visible = true; Button17.BackColor = Color.Green; Button17.Text = "N2 Purge Line"; });
                    PurgeOFF.Invoke((MethodInvoker)delegate { PurgeOFF.Visible = false; });
                    PurgeON.Invoke((MethodInvoker)delegate { PurgeON.Visible = true; });

                }


                if (Class1.DO_Gas1ON == false)
                {
                    Button10.BackColor = Color.Black;
                    G11.BackColor = Color.Black;
                    G1XSAOff.Invoke((MethodInvoker)delegate { G1XSAOff.Visible = true; });
                    G1XSAOn.Invoke((MethodInvoker)delegate { G1XSAOn.Visible = false; });
                    G1T.Invoke((MethodInvoker)delegate { G1T.Visible = true; });
                }

                else
                {
                    G11.BackColor = Color.Green;
                    Button10.BackColor = Color.Green;
                    G1XSAOff.Invoke((MethodInvoker)delegate { G1XSAOff.Visible = false; });
                    G1XSAOn.Invoke((MethodInvoker)delegate { G1XSAOn.Visible = true; });
                    G1T.Invoke((MethodInvoker)delegate { G1T.Visible = true; });
                }
                if (Class1.DO_Gas2ON == false)
                {
                    G21.BackColor = Color.Black;
                    G2XSAOff.Invoke((MethodInvoker)delegate { G2XSAOff.Visible = true; });
                    G2XSAOn.Invoke((MethodInvoker)delegate { G2XSAOn.Visible = false; });
                    G2T.Invoke((MethodInvoker)delegate { G2T.Visible = true; });
                }
                else
                {
                    G21.BackColor = Color.Green;
                    Button10.BackColor = Color.Green;
                    G2XSAOff.Invoke((MethodInvoker)delegate { G2XSAOff.Visible = false; });
                    G2XSAOn.Invoke((MethodInvoker)delegate { G2XSAOn.Visible = true; });
                    G2T.Invoke((MethodInvoker)delegate { G2T.Visible = true; });
                }

                //if (Class1.Gas3 == false)
                //{
                //    if (IsHandleCreated)
                //    {
                //        //this.CreateControl();
                //        G3T.Invoke((MethodInvoker)delegate { G3T.Visible = false; });
                //        G3XSAOff.Invoke((MethodInvoker)delegate { G3XSAOff.Visible = false; });
                //        G3XSAOn.Invoke((MethodInvoker)delegate { G3XSAOn.Visible = false; });
                //        Label10.Invoke((MethodInvoker)delegate { Label10.Visible = false; });
                //        Label33.Invoke((MethodInvoker)delegate { Label33.Visible = false; });
                //    }

                //}
                //else
                //{
                //    if (Class1.DO_StatusGAS3 == false)
                //    {
                //        G31.BackColor = Color.Black;
                //        G3XSAOff.Invoke((MethodInvoker)delegate { G3XSAOff.Visible = true; });
                //        G3XSAOn.Invoke((MethodInvoker)delegate { G3XSAOn.Visible = false; });
                //        G3T.Invoke((MethodInvoker)delegate { G3T.Visible = true; });
                //    }
                //    else
                //    {
                //        G31.BackColor = Color.Green;
                //        Button10.BackColor = Color.Green;
                //        G3XSAOff.Invoke((MethodInvoker)delegate { G3XSAOff.Visible = false; });
                //        G3XSAOn.Invoke((MethodInvoker)delegate { G3XSAOn.Visible = true; });
                //        G3T.Invoke((MethodInvoker)delegate { G3T.Visible = true; });
                //    }
                //    {
                //        G3T.Invoke((MethodInvoker)delegate { G3T.Visible = true; });
                //        Label10.Invoke((MethodInvoker)delegate { Label10.Visible = true; });
                //        Label33.Invoke((MethodInvoker)delegate { Label33.Visible = true; });
                //    }
                //}

                if (Class1.DO_RFON == false)
                {
                    RF1.BackColor = Color.Black;
                    RF2.BackColor = Color.Black;
                    RF3.BackColor = Color.Black;
                    RFONpic.Invoke((MethodInvoker)delegate { RFONpic.Visible = false; });
                    RFOFFpic.Invoke((MethodInvoker)delegate { RFOFFpic.Visible = true; });
                    Plasma.Invoke((MethodInvoker)delegate { Plasma.Visible = false;Plasma.SendToBack(); });
                    
                }
                else
                {
                    RF1.BackColor = Color.Red;
                    RF2.BackColor = Color.Red;
                    RF3.BackColor = Color.Red;
                    RFONpic.Invoke((MethodInvoker)delegate { RFONpic.Visible = true; });
                    RFOFFpic.Invoke((MethodInvoker)delegate { RFOFFpic.Visible = false; });
                    Plasma.Invoke((MethodInvoker)delegate { Plasma.Visible = true; });
                    //Plasma.BringToFront(); VacValve.BringToFront(); Press2.BringToFront();
                    

                }

                if (Class1.DO_RedLight == false)
                {
                    BRed.BackColor = Color.DimGray;
                }
                else
                {
                    BRed.BackColor = Color.Red;
                }
                if (Class1.DO_YellowLight == false)
                {
                    BYellow.BackColor = Color.DimGray;
                }
                else
                {
                    BYellow.BackColor = Color.DarkOrange;
                }
                if (Class1.DO_GreenLight == false)
                {
                    BGreen.BackColor = Color.DimGray;
                }
                else
                {
                    BGreen.BackColor = Color.Green;
                }
                if (Class1.DO_BuzzerON == false)
                {
                    BBuzzer.BackColor = Color.DimGray;
                }
                else
                {
                    BBuzzer.BackColor = Color.Red;
                }



                //End Read DO

                //Read AI

                //Calculate the Pressure in mbar
                // RealPressure = Math.Pow(10, (Class1.AI_PressureValue - 6)); // using AGP100 Pirani
                //RealPressure = RealPressure + 0.02;

                //TODO: verify with documentation that the formual is correct.
                Class1.RealPressure = Convert.ToDouble(Class1.AI_PressureValue / 10); // using Inficon CGD025D
                if (Class1.RealPressure < 100)
                {
                    PressureLbl.Invoke((MethodInvoker)delegate { PressureLbl.Text = Math.Round(Class1.RealPressure, 2).ToString(); });

                }
                else
                {

                    PressureLbl.Invoke((MethodInvoker)delegate { PressureLbl.Text = "Over Range"; });
                }
                //End Calculate the Pressure in mbar



                //Calculate Gas1
                Class1.RealGas1 = ((Class1.Gas1R / 10) * Class1.AI_GAS1Value);
                /// GCF1 '* GCF1
                if (Class1.RealGas1<0)
                { Label11.Invoke((MethodInvoker)delegate { Label11.Text = "0"; }); }
                else {Label11.Invoke((MethodInvoker)delegate { Label11.Text = Math.Round(Class1.RealGas1, 2).ToString(); }); }

                if (Class1.DoingManualCycle == false | Class1.DO_Gas1ON == false)

                    Label11.Invoke((MethodInvoker)delegate { Label11.Text = "0"; });

                //End Calculate Gas1

                //Calculate Gas2
                Class1.RealGas2 = ((Class1.Gas2R / 10) * Class1.AI_GAS2Value);
                /// GCF2 '* GCF2
                if (Class1.RealGas2<0)
                { Label9.Invoke((MethodInvoker)delegate { Label9.Text = "0"; }); }
                else {   Label9.Invoke((MethodInvoker)delegate { Label9.Text = Math.Round(Class1.RealGas2, 2).ToString(); });}


                if (Class1.DoingManualCycle == false | Class1.DO_Gas2ON == false)
                    Label9.Invoke((MethodInvoker)delegate { Label9.Text = "0"; });

                //End Calculate Gas2

                //Calculate Gas3
                Class1.RealGas3 = Class1.Gas3R / 10 * Class1.AI_GAS3Value;
                //* GCF3
                //if (Class1.RealGas3<0)
                //{ Label10.Invoke((MethodInvoker)delegate { Label10.Text = "0"; }); }
                //else {   Label10.Invoke((MethodInvoker)delegate { Label10.Text = Math.Round(Class1.RealGas3, 2).ToString(); });}
              
                //if (Class1.DoingManualCycle == false | Class1.DO_StatusGAS3 == false)

                //    Label10.Invoke((MethodInvoker)delegate { Label10.Text = "0"; });
                //End Calculate Gas3

                //Calculate AI_ARFPowerValue
                Class1.RealRFPWR = Class1.SetupRFRange / 10 * Class1.AI_ARFPowerValue;
                if (Class1.RealRFPWR<0)
                { Label6.Invoke((MethodInvoker)delegate { Label6.Text = "0"; }); }
                else {  Label6.Invoke((MethodInvoker)delegate { Label6.Text = Math.Round(Class1.RealRFPWR).ToString(); });}
               
                if (Class1.DoingManualCycle == false)
                    Label6.Invoke((MethodInvoker)delegate { Label6.Text = "0"; });
                //End Calculate AI_ARFPowerValue

                //Calculate AI_RFRefelctedValue
                Class1.RealRFREV = Class1.SetupRFRange / 10 / 3 * Class1.AI_RFRefelctedValue;
                if (Class1.RealRFREV<0)
                { Label7.Invoke((MethodInvoker)delegate { Label7.Text = "0"; }); }
                else {  Label7.Invoke((MethodInvoker)delegate { Label7.Text = Math.Round(Class1.RealRFREV, 2).ToString(); });}
               
                if (Class1.DoingManualCycle == false)

                    Label7.Invoke((MethodInvoker)delegate { Label7.Text = "0"; });
                //End Calculate AI_RFRefelctedValue

                //Calculate AI_BiasValue
                Class1.RealBias = 100 * Class1.AI_BiasValue;
                if (Class1.RealBias < 0)
                { Label27.Invoke((MethodInvoker)delegate { Label27.Text = "0"; }); }
                else {  Label27.Invoke((MethodInvoker)delegate { Label27.Text = Math.Round(Class1.RealBias, 2).ToString(); });}
                if (Class1.DoingManualCycle == false)
                 Label27.Invoke((MethodInvoker)delegate { Label27.Text = "0"; });
                //End Calculate AI_BiasValue

                //Calculate AI_TuneValue
                Class1.RealTune = 10 * Class1.AI_TuneValue;
                if (Class1.RealTune<0)
                {
                    Label2.Invoke((MethodInvoker)delegate { Label2.Text = "0"; });
                }
                else { 
                    Label2.Invoke((MethodInvoker)delegate { Label2.Text = Math.Round(Class1.RealTune, 2).ToString(); }); 
                }
               
                //End Calculate AI_TuneValue

                //Calculate AI_LoadValue
                Class1.RealLoad = 10 * Class1.AI_LoadValue;
                if (Class1.RealLoad<0)
                { Label8.Invoke((MethodInvoker)delegate { Label8.Text = "0"; }); }
                else{Label8.Invoke((MethodInvoker)delegate { Label8.Text = Math.Round(Class1.RealLoad, 2).ToString(); });}
                
                //End Calculate AI_LoadValue

                //Calculate AI_GAS1PSValue
                // the gas pressure sensor returns 1-5 VDC representing 0 to 10 Bar

                Class1.RealGAS1PS = Convert.ToDouble((Class1.AI_GAS1PSValue - 1) * 2.5);
                if (Class1.RealGAS1PS<0)
                { Label19.Invoke((MethodInvoker)delegate { Label19.Text = "0"; }); }
                else { Label19.Invoke((MethodInvoker)delegate { Label19.Text = Class1.RealGAS1PS.ToString(); }); }
               
                if (Class1.RealGAS1PS < 0)

                    Label19.Invoke((MethodInvoker)delegate { Label19.Text = "0.0"; });
                //End Calculate AI_GAS1PSValue

                //Calculate AI_GAS2PSValue
                Class1.RealGAS2PS = Convert.ToDouble((Class1.AI_GAS2PSValue - 1) * 2.5);
                if (Class1.RealGAS2PS<0)
                { Label15.Invoke((MethodInvoker)delegate { Label15.Text ="0"; }); }
                else { Label15.Invoke((MethodInvoker)delegate { Label15.Text = Class1.RealGAS2PS.ToString(); }); }
               
                if (Class1.RealGAS2PS < 0)
                    Label15.Invoke((MethodInvoker)delegate { Label15.Text = "0.0"; });

                //End Calculate AI_GAS2PSValue

                //Calculate AI_GAS3PSValue
                Class1.RealGAS3PS = ((Class1.AI_GAS3PSValue) - 1) * 2.5;

                Label17.Invoke((MethodInvoker)delegate { Label17.Text = Class1.RealGAS3PS.ToString(); });
                if (Class1.RealGAS3PS < 0)

                    Label17.Invoke((MethodInvoker)delegate { Label17.Text = "0.0"; });
                //End Calculate AI_GAS3PSValue

                //Calculate AI_PURGEPSValue
                Class1.RealPurge = ((Class1.AI_PURGEPSValue - 1) * 2.5);
                if (Class1.RealPurge<0)
                { Label22.Invoke((MethodInvoker)delegate { Label22.Text = "0"; }); }
                else
                { Label22.Invoke((MethodInvoker)delegate { Label22.Text = Math.Round(Class1.RealPurge, 2).ToString(); }); }
               
                if (Class1.RealPurge < 0)

                    Label22.Invoke((MethodInvoker)delegate { Label22.Text = "0.0"; });

                //End Calculate AI_PURGEPSValue

                //End Read AI


                if (Class1.DO_Gas1ON == false)
                {
                    //?????????

                }



                if (Class1.AlarmActive == true)
                {
                    {
                        if (AlarmThread.IsBusy == false)
                            AlarmThread.RunWorkerAsync();
                    }
                }

                //Towerlights ***************************************

                if (Class1.FirstCycle == false)
                {
                    Thread.Sleep(1000);
                    Class1.FirstCycle = true;
                    Class1.TLDid1 = false;
                    Class1.TLDid2 = false;
                }
               
                if (Class1.AlarmPause == true & Class1.TLDid1 == false)
                {
                    Class1.TLDid1 = true;
                    Class1.TLDid2 = false;
                    // Class2.Create10DOArray(28, 0, 29, 0, 27, 1, 30, 1);

                    int[] DOSlotNoArr = { Class1.DIOSlotNum, Class1.DIOSlotNum, Class1.DIOSlotNum, Class1.DIOSlotNum };
                    int[] DOChannelArr = { Class1.DO27, Class1.DO26, Class1.DO28, Class1.DO29 }; //Turn off yellow light,Turn off green light,Turn on red light,Turn on Buzzer
                    bool[] DOStateArr = { false, false, true, true };
                    Class2.SetMultiDIO(DOSlotNoArr, DOChannelArr, DOStateArr);
                    
                   // Class2.SetDO(Class1.DIOSlotNum, 3 + AvantechDIOs.m_iDoOffset, false);//Turn off yellow light
                   // Class2.SetDO(Class1.DIOSlotNum, 2 + AvantechDIOs.m_iDoOffset, false);//Turn off green light
                  //  Class2.SetDO(Class1.DIOSlotNum, 4 + AvantechDIOs.m_iDoOffset, true);//Turn on red light
                   // Class2.SetDO(Class1.DIOSlotNum, 5 + AvantechDIOs.m_iDoOffset, true);//Turn on Buzzer
                                     
                   
                }
                           

                if (Class1.AlarmPause == false & Class1.TLDid2 == false)
                {
                    Class1.TLDid1 = false;
                    Class1.TLDid2 = true;
                    // Class2.Create10DOArray(27, 0, 30, 0, 28, 1);

                    int[] DOSlotNoArr = { Class1.DIOSlotNum, Class1.DIOSlotNum, Class1.DIOSlotNum };
                    int[] DOChannelArr = { Class1.DO28, Class1.DO29, Class1.DO27 }; //Turn off red light,Turn off Buzzer,Turn on yellow light
                    bool[] DOStateArr = { false, false, true };
                    Class2.SetMultiDIO(DOSlotNoArr, DOChannelArr, DOStateArr);
                    
                   // Class2.SetDO(Class1.DIOSlotNum, 4 + AvantechDIOs.m_iDoOffset, false);//Turn off red light
                   // Class2.SetDO(Class1.DIOSlotNum, 5 + AvantechDIOs.m_iDoOffset, false);//Turn off Buzzer
                   // Class2.SetDO(Class1.DIOSlotNum, 3 + AvantechDIOs.m_iDoOffset, true);//Turn on yellow light
                    
                    Class1.HoldBuzzer = false;
                }

                //*****************************************

                //Alarms

                //*****************************************



                //No Air
            
                if (Class1.DI_Air == 0)
                {
                    if (Class1.AlarmPause == false)
                        MRaiseAlarm(1);
                }

                //Pump Health Bit Down  NOTE: NOT AVAILABLE WITH ALCATEL PUMP. uSE ONLY WITXDS 35i EDWARDS PUMP.
                //////GetDI();
                //////if (Class1.DO_StatusPumpON == true)
                //////{

                //////    if (Class1.DI_PumpOK == 0)
                //////    {
                //////        if (Class1.AlarmPause == false)
                //////            MRaiseAlarm(2);
                //////    }
                //////}


                //No Gas1 Pressure Sensor
                if (Class1.R_G1 > 0)
                {
                    if (Class1.AI_GAS1PSValue < 1.04)
                    {
                        if (Class1.AlarmPause == false)
                            MRaiseAlarm(5);
                    }
                }
                //No Gas2 Pressure Sensor
                if (Class1.R_G2 > 0)
                {
                    if (Class1.AI_GAS2PSValue < 1.04)
                    {
                        if (Class1.AlarmPause == false)
                            MRaiseAlarm(6);
                    }
                }
                if (Class1.R_G3 > 0)
                {
                    //No Gas3 Pressure Sensor
                    if (Class1.Gas3 == true)
                    {
                        if (Class1.AI_GAS3PSValue < 1.04)
                        {
                            if (Class1.AlarmPause == false)
                                MRaiseAlarm(7);
                        }
                    }
                }

                //No Purge Gas Pressure sensor
                if (Class1.AI_PURGEPSValue < 1.04)
                {
                    if (Class1.AlarmPause == false)
                        MRaiseAlarm(15);
                }

                if (Class1.Intlk == false & Class1.DO_RFON == true)
                {
                    if (Class1.AlarmPause == false)
                        MRaiseAlarm(17);
                }

                // DoingManualCycle = False
                if (Class1.DoingManualCycle == true)
                {

                    //if (Class1.DI_FrontDoorup == 0 || Class1.DI_BackDoorUp == 0 || Class1.DI_ChDoor_Front == 1 || Class1.DI_ChDoorBack == 1)
                    //{
                    //    //if (Class1.AlarmPause == false)
                    //    //MRaiseAlarm(9);
                    //    //DoingManualCycle = False
                    //}
                }

                //EStop
                if (Class1.DI_EStop == 0)
                {
                    if (Class1.AlarmPause == false)
                        MRaiseAlarm(10);
                }



                //MFC1

                if (Class1.DO_StatusMainpurge == true)
                {
                    Class1.MGasTick = Class1.MGasTick + 1;   // to be investigated
                }

                if (Class1.R_G1 > 0 & Class1.ManualStop == false & Class1.ManualGas == true & Class1.MGasTick > 20)
                {
                    Class1.MGasTick = 21;
                    if (Class1.DO_Gas1ON == false | Class1.AI_GAS1Value <= 0.0)
                    {
                        if (Class1.AlarmPause == false)
                            MRaiseAlarm(11);
                    }
                }

                //MFC2

                if (Class1.R_G2 > 0 & Class1.ManualStop == false & Class1.ManualGas == true & Class1.MGasTick > 20)
                {
                    Class1.MGasTick = 21;
                    if (Class1.DO_Gas2ON == false | Class1.AI_GAS2Value <= 0.0)
                    {
                        if (Class1.AlarmPause == false)
                            MRaiseAlarm(12);
                    }
                }

                //MFC3

                //if (Class1.Gas3 == true)
                //{
                //    if (Class1.R_G3 > 0 & Class1.ManualStop == false & Class1.ManualGas == true & Class1.MGasTick > 20)
                //    {
                //        Class1.MGasTick = 21;
                //        if (Class1.DO_StatusGAS3 == false | Class1.AI_GAS3Value <= 0.0)
                //        {
                //            if (Class1.AlarmPause == false)
                //                MRaiseAlarm(13);
                //        }
                //    }
                //}

                if (Class1.DO_RFON == true)
                {
                    Class1.MRFTick = Class1.MRFTick + 1;
                }

                //No Plasma  and  High Reflected Power
                if (Class1.DO_RFON == true & Class1.RFTick > 20)
                {
                    Class1.MRFTick = 21;
                    if (Class1.AI_BiasValue < 0.1)
                    {
                        if (Class1.AlarmPause == false)
                            MRaiseAlarm(3);
                    }
                    if (Class1.DO_RFON == true & Class1.AI_RFRefelctedValue > 2.0)
                    {
                        if (Class1.AlarmPause == false)
                            MRaiseAlarm(4);
                    }
                }

                //CHECK THIS****************************************************************
                if (Class1.FatalError == true & Class1.ManualCycleStarted==true)
                {
                    if (ventTh == null)
                    {
                        ventTh = new Thread(new System.Threading.ThreadStart(VentTimerTh));
                        ventTh.Start();
                    }
                }
                   
                //***************************************

                //End Alarms
                //DateTime EndC = DateTime.Now;
                //int MSec = 0;
                //TimeSpan Diff = EndC - StartC;
                //MSec = (int)Diff.Milliseconds;

                if (IsHandleCreated)
                {
                    if (InvokeRequired)
                    {
                        //label36.Invoke((MethodInvoker)delegate { label36.Text = count.ToString(); });

                    }

                }
                count += 1;

                ETime = DateTime.Now; 
                Diff = ETime -STime ;
                Class1.ManCycleTime = Diff.Milliseconds.ToString();
               
            }
            while (true);
        }


       

        private void button23_Click(object sender, EventArgs e)
        {
            Class1.IOOpen = true;
            Class1.IOStopped = false;
            Class1.IOInfoSamePg = true;
            Class1.openIO.Show();
            Class1.openIO.TopMost = true;
            
        }

        private void Label1_DoubleClick(object sender, EventArgs e)
        {
            Modebtn.Enabled = true;
            Modebtn.BackColor = Color.CornflowerBlue;
        }

        private void AutoMan_Click(object sender, EventArgs e)
        {
            if (AutoMan.BackColor == Color.Green)
            {
               // Class2.Create10DOArray(18, 1,19,0);

                int[] DOSlotNoArr = { Class1.DOSlotNum, Class1.DOSlotNum };
                int[] DOChannelArr = { 2, 3 }; //Manual Tuner,Auto Tuner
                bool[] DOStateArr = { true, false  };
                Class2.SetMultiDO(DOSlotNoArr, DOChannelArr, DOStateArr);

                //Class2.SetDO(Class1.DOSlotNum, 2, true);//Manual Tuner
                //Class2.SetDO(Class1.DOSlotNum, 3, false);//Auto Tuner

                AutoMan.BackColor = Color.Red;
            }
            else
            {
               // Class2.Create10DOArray(18, 0,19,1);

                int[] DOSlotNoArr = { Class1.DOSlotNum, Class1.DOSlotNum };
                int[] DOChannelArr = { 2, 3 }; //Manual Tuner,Auto Tuner
                bool[] DOStateArr = {  false,true };
                Class2.SetMultiDO(DOSlotNoArr, DOChannelArr, DOStateArr);

                //Class2.SetDO(Class1.DOSlotNum, 2, false);//Manual Tuner
                //Class2.SetDO(Class1.DOSlotNum, 3, true);//Auto Tuner

                AutoMan.BackColor = Color.Green;
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            Class2.SetDO(Class1.DOSlotNum, 0, true);
            Class2.SetDO(Class1.DOSlotNum, 1, true);
            Class2.SetDO(Class1.DOSlotNum, 2, true);
            Class2.SetDO(Class1.DOSlotNum, 3, true);
            Class2.SetDO(Class1.DOSlotNum, 4, true);
            Class2.SetDO(Class1.DOSlotNum, 5, true);
            Class2.SetDO(Class1.DOSlotNum, 6, true);
            Class2.SetDO(Class1.DOSlotNum, 7, true);
            Class2.SetDO(Class1.DOSlotNum, 8, true);
            Class2.SetDO(Class1.DOSlotNum, 9, true);
            Class2.SetDO(Class1.DOSlotNum, 10, true);
            Class2.SetDO(Class1.DOSlotNum, 11, true);
            Class2.SetDO(Class1.DOSlotNum, 12, true);
            Class2.SetDO(Class1.DOSlotNum, 13, true);
            Class2.SetDO(Class1.DOSlotNum, 14, true);
            Class2.SetDO(Class1.DOSlotNum, 15, true);
            Class2.SetDO(Class1.DOSlotNum, 16, true);
            Class2.SetDO(Class1.DOSlotNum, 17, true);
            Class2.SetDO(Class1.DOSlotNum, 18, true);
            Class2.SetDO(Class1.DOSlotNum, 19, true);
            Class2.SetDO(Class1.DOSlotNum, 20, true);
            Class2.SetDO(Class1.DOSlotNum, 21, true);
            Class2.SetDO(Class1.DOSlotNum, 22, true);
            Class2.SetDO(Class1.DOSlotNum, 23, true);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO24, true);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO25, true);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO26, true);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO27, true);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO28, true);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO29, true);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO30, true);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO31, true);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO32, true);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO33, true);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO34, true);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO35, true);
          
           
        }

        private void button24_Click(object sender, EventArgs e)
        {
            Class2.SetDO(Class1.DOSlotNum, 0, false);
            Class2.SetDO(Class1.DOSlotNum, 1, false);
            Class2.SetDO(Class1.DOSlotNum, 2, false);
            Class2.SetDO(Class1.DOSlotNum, 3, false);
            Class2.SetDO(Class1.DOSlotNum, 4, false);
            Class2.SetDO(Class1.DOSlotNum, 5, false);
            Class2.SetDO(Class1.DOSlotNum, 6, false);
            Class2.SetDO(Class1.DOSlotNum, 7, false);
            Class2.SetDO(Class1.DOSlotNum, 8, false);
            Class2.SetDO(Class1.DOSlotNum, 9, false);
            Class2.SetDO(Class1.DOSlotNum, 10, false);
            Class2.SetDO(Class1.DOSlotNum, 11, false);
            Class2.SetDO(Class1.DOSlotNum, 12, false);
            Class2.SetDO(Class1.DOSlotNum, 13, false);
            Class2.SetDO(Class1.DOSlotNum, 14, false);
            Class2.SetDO(Class1.DOSlotNum, 15, false);
            Class2.SetDO(Class1.DOSlotNum, 16, false);
            Class2.SetDO(Class1.DOSlotNum, 17, false);
            Class2.SetDO(Class1.DOSlotNum, 18, false);
            Class2.SetDO(Class1.DOSlotNum, 19, false);
            Class2.SetDO(Class1.DOSlotNum, 20, false);
            Class2.SetDO(Class1.DOSlotNum, 21, false);
            Class2.SetDO(Class1.DOSlotNum, 22, false);
            Class2.SetDO(Class1.DOSlotNum, 23, false);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO24, false);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO25, false);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO26, false);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO27, false);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO28, false);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO29, false);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO30, false);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO31, false);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO32, false);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO33, false);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO34, false);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO35, false);
           
        }
    }
}
