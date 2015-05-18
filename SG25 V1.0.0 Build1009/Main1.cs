using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using APS_Define_W32;
using APS168_W32;
using SG25.SetupDataSetTableAdapters;
using Microsoft.VisualBasic;
using System.Reflection;

namespace SG25
{
    public partial class Main1 : Form
    {
        public Main1()
        {
            InitializeComponent();
        }

        public delegate void SetAirsensor(System.Drawing.Color col);
        public delegate void SetEStop(bool status);
        public delegate void SetPumpOk(bool status);
        public delegate void SetDoor(bool status);
        public delegate void SetTextCallback(string text);


        StreamWriter FS;
        string MainPath;
        private float m_Theta = 0;
        private float m_Delta = 10;
        int n;
        Int32 DOReadValue;
        double AI_Value;         //PCI-7853 Card ID
        int CardID = 0;         //HSL Bus is 0
        int BusNo = 0;        //HSL-DI16DO16 Slave ID
        Label[] AI = new Label[17];
        Button[] DI = new Button[17];
        public string DOStatus;
        string DOArray;
        int i;
        int k;
        int j;
        int AVentTimeDisplay;
        public Thread AventTh;
        int LastAlarm = 0;

        public int LapseAutoinit = 5;
        public int MotorTick;
        public bool initMotor;
        bool motorStopped;
        bool VentFlg;
        bool CycleS;
        bool PlasmaDone;
        int ClkTimCount;
        int MaxRFT;
        int GhostTickAuto;
        bool CycleFirstTL;
        bool LeadFrameInFlg;
        bool LeadFrameOutFlg;
        int PicCount = 0;
        bool StartSimulation;
        //int[] DOSlotNoArr;
        //int[] DOChannelArr;
        //bool[] DOStateArr;

        Thread FrontDrCloseAutoTh;
        Thread BackDrCloseAutoTh;
        Thread BackDrOpenAutoTh;
        Thread FrontDrOpenAutoTh;
        Thread BothDoorsAutoTh;
        Thread TestThreadTh;
        Thread ClocktimerTh;
        Thread BothDoorsUpThAuto;
        Thread PicSimPlasmaProcess;

        double RealPressure;

        SetupDataSet MainsetupDatasetObj = new SetupDataSet();
        StartupTableAdapter MainstartupTAobj = new StartupTableAdapter();
        ProgramsTableAdapter MainprogramsTAobj = new ProgramsTableAdapter();
        SetupTableAdapter MainSetupTAobj = new SetupTableAdapter();



        /***********************Fome Load************************/

        private void Main_Load(System.Object sender, System.EventArgs e)
        {
            Class1.MainLoadFlg = true;
            //Class1.objLogevent = new LogEventAuto();
            //Class1.objLogevent.Show();
            //Class1.objLogevent.TopMost = true;
           

            if (Class1.conn.State == ConnectionState.Open)
            {
                Class1.conn.Close();
            }
            if (Class1.conn.State == ConnectionState.Closed)
            {
                Class1.conn.Open();
            }
            Class2.DGVDataAccess();

            // DGVLogList.Invoke((MethodInvoker)delegate { DGVLogList.DataSource = Class1.MainDataAccessLog; });
            // TODO: This line of code loads data into the 'sG25DataSet1.tblEventLogAccess' table. You can move, or remove it, as needed.

            Class2.WriteEventLog(Class1.AutoPageStartedCode, Class1.AutoPageStartedDesc);
            Class2.LogEventDB(Class1.AutoPageStartedCode, Class1.AutoPageStartedDesc);
            this.Text = "SCI Automation - " + " " + "SG25" + " " + Class1.VersionNo;
            this.startupTableAdapter.Fill(this.setupDataSet.Startup);

            try
            {
                this.programsTableAdapter.FillBy(this.setupDataSet.Programs, Class1.CurrentP);

            }
            catch
            {
            }
            string NowDate = null;

            NowDate = DateTime.Now.ToString();
            NowDate = NowDate.Replace("/", "-");
            NowDate = NowDate.Replace(":", "-");
            MainPath = ("C:/Program Files/SG25/AlarmLog/Logfile " + NowDate + ".csv");

            //Splash objSplash = new Splash();
            //objSplash.Hide();


            Class1.Auto = true;

            Class1.SetupRFRange = Convert.ToDouble(MainSetupTAobj.SelectRFRange());
            Class1.Gas1 = Convert.ToBoolean(MainSetupTAobj.GetSetupGas1());
            Class1.Gas2 = Convert.ToBoolean(MainSetupTAobj.GetSetupGas2());
            Class1.Gas3 = Convert.ToBoolean(MainSetupTAobj.GetSetupGas3());
            Class1.Gas1T = MainSetupTAobj.SelectGas1Type().ToString();
            Class1.Gas2T = MainSetupTAobj.SelectGas2Type().ToString();
            Class1.Gas3T = MainSetupTAobj.SelectGas3Type().ToString();
            Class1.Gas1R = Convert.ToDouble(MainSetupTAobj.SelectGas1Range());
            Class1.Gas2R = Convert.ToDouble(MainSetupTAobj.SelectGas2Range());
            Class1.Gas3R = Convert.ToDouble(MainSetupTAobj.SelectGas3Range());
            Class1.Venttime = (int)MainSetupTAobj.SelectVentTime();
            Class1.PunpDwnTime = (int)MainSetupTAobj.SelectPumpDwnTime();
            Class1.H2Gen = Convert.ToBoolean(MainSetupTAobj.GetSetupHasH2());
            Class1.TurboP = Convert.ToBoolean(MainSetupTAobj.GetSetupHasTurbo());
            Class1.UserIndex = (int)MainSetupTAobj.SelectUserIDIndex();


            G1T.Text = Class1.Gas1T;
            G2T.Text = Class1.Gas2T;
            //G3T.Text = Class1.Gas3T;
            Gas1set.Text = Class1.Gas1T;
            Gas2set.Text = Class1.Gas2T;
          //  Gas3set.Text = Class1.Gas3T;
            //AutoCycle = True
            Class1.AlarmActive = true;
            //Analog Output



            Class1.R_ID = Label47.Text;

            Class1.R_PT = Convert.ToDouble(Label35.Text);

            Class1.R_TTP = Convert.ToDouble(Label33.Text);

            Class1.R_G1 = Convert.ToDouble(Label52.Text);

            Class1.R_G2 = Convert.ToDouble(Label37.Text);

           // Class1.R_G3 = Convert.ToDouble(Label45.Text);

            Class1.R_PW = Convert.ToDouble(Label31.Text);

            Class1.R_RFT = Convert.ToDouble(Label29.Text) * 10;
            MaxRFT = (int)Class1.R_RFT;

            Class1.R_TP = Convert.ToDouble(Label51.Text);

            Class1.R_LP = Convert.ToDouble(Label49.Text);

            Int32 ret1 = default(Int32);
            double val1 = 0;
            try
            {
                val1 = Convert.ToDouble(Class1.R_PW / Class1.SetupRFRange * 10);

                // ret1 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAO, 0, val1);

                Class2.setAOValues(Convert.ToString(val1), 0);
            }
            catch (Exception ex)
            {
            }


            //try
            //{
            //    Int32 ret2 = default(Int32);
            //    double val2 = 0;

            //    val2 = Convert.ToDouble((Class1.R_G1 / (Class1.Gas1R / 10) * Class1.GCF1)/4);
            //    ret2 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAO, 1, val2);
            //}
            //catch (Exception ex)
            //{
            //}

            //try
            //{
            //    Int32 ret3 = default(Int32);
            //    double val3 = 0;

            //    val3 = Convert.ToDouble((Class1.R_G2 / (Class1.Gas2R / 10) * Class1.GCF2)/4);
            //    ret3 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAO, 2, val3);
            //}
            //catch (Exception ex)
            //{
            //}

            //try
            //{
            //    Int32 ret4 = default(Int32);
            //    double val4 = 0;

            //    val4 = Convert.ToDouble(Class1.R_G3 / (Class1.Gas3R / 10) * Class1.GCF3);
            //    ret4 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAO, 3, val4);
            //}
            //catch (Exception ex)
            //{
            //}
            try
            {
                //Analog Output
                Int32 ret5 = default(Int32);
                double val5 = 0;

                val5 = Convert.ToDouble(Class1.R_TP / 10);
                // ret5 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAI, 0, val5);
                Class2.setAOValues(Convert.ToString(val5), 1);

            }
            catch (Exception ex)
            {
            }


            try
            {
                //Analog Output
                Int32 ret6 = default(Int32);
                double val6 = 0;

                val6 = Convert.ToDouble(Class1.R_LP / 10);
                //ret6 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAI, 1, val6);
                Class2.setAOValues(Convert.ToString(val6), 2);

            }
            catch (Exception ex)
            {
            }
            //Timer1.Enabled = true;
            TestThreadTh = new Thread(new System.Threading.ThreadStart(TestThread));
            TestThreadTh.Start();

            RFTimeBar.Invoke((MethodInvoker)delegate { RFTimeBar.Maximum = MaxRFT; RFTimeBar.Minimum = 0; });
            //(int)Class1.R_RFT;
            RFTimeBar.Minimum = 0;
            LRFTime.Text = String.Format("{0:n1}", MaxRFT / 10);

            if (AlarmThread.IsBusy == false)
                AlarmThread.RunWorkerAsync();
            //    CyclesLbl.Text = StartupTableAdapter.SelectCyclesFromStartUp.ToString
            Shift.Text = "Shift Cycles:" + Environment.NewLine + this.startupTableAdapter.SelectShiftCyclesfromStartup().ToString();
            if (Class1.ByPassMode == true)
            {
                Startbtn.Font = new Font("Microsoft Sans Serif", 25);
                Startbtn.Text = "ByPass Mode Start";
                Class1.ByPass = true;
                GroupBox5.Text = "Message";
                AlarmBox.Text = "ByPass Mode";

            }
            else if (Class1.ByPassMode == false)
            {
                Startbtn.Font = new Font("Microsoft Sans Serif", 30);
            }
            Class1.DGVMain = DGVLogList;
            Class1.DGVMain.BringToFront();
            this.Invalidate();
        }

        /***********************End of Fome Load************************/

        /***********************Fome Activated************************/

        private void Main_Activated(System.Object sender, System.EventArgs e)
        {
            this.Text = "SCI Automation - " + " " + "SG25" + " " + Class1.VersionNo;
            //EM Me.ValuesReadThread.Enabled = True
            Class1.AlarmActive = true;
            Class2.LoadSetup();

            try
            {
                this.programsTableAdapter.FillBy(this.setupDataSet.Programs, Class1.CurrentP);

            }
            catch
            {
            }


            Class1.R_ID = Label47.Text;

            Class1.R_PT = Convert.ToDouble(Label35.Text);

            Class1.R_TTP = Convert.ToDouble(Label33.Text);

            Class1.R_G1 = Convert.ToDouble(Label52.Text);

            Class1.R_G2 = Convert.ToDouble(Label37.Text);

        //    Class1.R_G3 = Convert.ToDouble(Label45.Text);

            Class1.R_PW = Convert.ToDouble(Label31.Text);

            Class1.R_RFT = Convert.ToDouble(Label29.Text);

            Class1.R_TP = Convert.ToDouble(Label51.Text);

            Class1.R_LP = Convert.ToDouble(Label49.Text);



            RFTimeBar.Invoke((MethodInvoker)delegate { RFTimeBar.Maximum = MaxRFT; RFTimeBar.Minimum = 0; });
            //(int)Class1.R_RFT;
            LRFTime.Text = String.Format("{0:n1}", MaxRFT / 10);
            Class1.DGVMain = DGVLogList;
            this.Invalidate();
            //Timer1.Enabled = true;
        }

        /***********************End of Fome Activated************************/

        /*************************Start Functions******************************/

        private void BackDoorsUpThAuto()
        {
            //Class2.Create10DOArray(3, 1,4, 0); // Send back doors up with 1 new function CreateDOArrayDoors
            //DateTime StartTime = DateTime.Now;
            //DateTime NewTime;
            //TimeSpan Diff;
            //Class1.Milliseconds = 0;

            //do
            //{

            //    NewTime = DateTime.Now;
            //    Diff = NewTime - StartTime;
            //    Class1.Milliseconds = (int)Diff.Milliseconds;
            //    if (Class1.Milliseconds > 2000)
            //    {
            //        RaiseAlarm(22);
            //        Class1.BackDoorNotUp = true;
            //        Class2.Create10DOArray(3, 0,4, 1,8 ,0); // Send both back down with 1 new function Create4DOArrayDoors
            //        BackDrCloseAutoTh.Abort();
            //        return;
            //    }

            //}
            //while (Class1.DI_BackDoorUp == 0);
            //BDUAuto.Invoke((MethodInvoker)delegate {BDUAuto.BackColor = Color.Green; });
            //BackCloseDoorThAuto();

        }

        private void BackCloseDoorThAuto()
        {
            //Thread.Sleep(200);
            //Class2.Create10DOArray(9, 1);


            //if ((Class1.DI_BackDoorUp == 0 & Class1.SendAlarm != 25))
            //{
            //    RaiseAlarm(25);
            //    Class2.Create10DOArray(3, 0, 4,  1); // Send back doors close with 1 new function Create4DOArrayDoors
            //}

            //if (IsHandleCreated)
            //{
            //    BDCAuto.Invoke((MethodInvoker)delegate {BDCAuto.Visible = true; });

            //} 

            //BackDrCloseAutoTh.Abort();
        }

        private void BackDoorOpenAuto()
        {
            //Class2.Create10DOArray(9, 0); //open the Back door
            //DateTime StartTime = DateTime.Now;
            //DateTime NewTime;
            //TimeSpan Diff;
            //Class1.Milliseconds = 0;


            //do
            //{
            //    NewTime = DateTime.Now;
            //    Diff = NewTime - StartTime;
            //    Class1.Milliseconds = (int)Diff.Milliseconds;

            //    if (Class1.Milliseconds > 2000)
            //    {
            //        RaiseAlarm(28);
            //        BackDrOpenAutoTh.Abort();
            //        return;
            //    }

            //}
            //while (Class1.DI_ChDoorBack == 0);
            //Thread.Sleep(300);
            //if(IsHandleCreated)
            //{  BDCAuto.Invoke((MethodInvoker)delegate {BDCAuto.Visible = false; });}

            //BackDoorDownAuto();

        }

        private void BackDoorDownAuto()
        {

            // Class2.Create10DOArray(3, 0,4,  1); //Back Door Down
            Thread.Sleep(300);

            if (Class2.SetDoorsAlarm == true)
            {
                RaiseAlarm(29);
                return;
            }
            if (IsHandleCreated)
            {
                // BDUAuto.Invoke((MethodInvoker)delegate { BDUAuto.BackColor = Color.Red; });

            }

            BackDrOpenAutoTh.Abort();

        }

        private void FrDoorsUpThAuto()
        {
            //Class2.Create10DOArray(1, 1,2,  0); // Send front door up with 1 new function CreateDOArrayDoors
            //DateTime StartTime = DateTime.Now;
            //DateTime NewTime;
            //TimeSpan Diff;
            //Class1.Milliseconds = 0;

            //do
            //{

            //    NewTime = DateTime.Now;
            //    Diff = NewTime - StartTime;
            //    Class1.Milliseconds = (int)Diff.Milliseconds;
            //    if (Class1.Milliseconds > 2000)
            //    {
            //        RaiseAlarm(21);
            //        Class1.FrontDoorNotUp = true;
            //        Class2.Create10DOArray(1, 0, 2, 1, 8, 0); // Send front door down with 1 new function Create4DOArrayDoors
            //        FrontDrCloseAutoTh.Abort();
            //        return;
            //    }

            //}
            //while (Class1.DI_FrontDoorup == 0);
            //FDUAuto.BackColor = Color.Green;
            //CloseFrDoorThAuto();

        }

        private void CloseFrDoorThAuto()
        {
            //Thread.Sleep(300);
            //Class2.Create10DOArray(8, 1);//front door Close

            //if (Class1.DI_FrontDoorup == 0)
            //{
            //    RaiseAlarm(24);
            //    Class2.Create10DOArray(1, 0,2,  1); // Send front door down with 1 new function Create4DOArrayDoors
            //}

            //if (IsHandleCreated)
            //{
            //    FDCAuto.Invoke((MethodInvoker)delegate {FDCAuto.Visible = true; });

            //}
            //FrontDrCloseAutoTh.Abort();

        }

        private void FrOpenDoorsAuto()
        {
            //Class2.Create10DOArray(8, 0); //open the Front door
            ////Class2.Create10DOArrayDoors(1, 3, 0, 0);

            //DateTime StartTime = DateTime.Now;
            //DateTime NewTime;
            //TimeSpan Diff;
            //Class1.Milliseconds = 0;

            //do
            //{

            //    NewTime = DateTime.Now;
            //    Diff = NewTime - StartTime;
            //    Class1.Milliseconds = (int)Diff.Milliseconds;

            //    if (Class1.Milliseconds > 2000)
            //    {
            //        RaiseAlarm(27);
            //        Class1.BothDoorsNotUp = true;
            //        FrontDrOpenAutoTh.Abort();
            //        return;
            //    }

            //}
            //while (Class1.DI_ChDoor_Front == 0);
            //Thread.Sleep(200);
            //if(IsHandleCreated)
            //{
            //FDCAuto.Invoke((MethodInvoker)delegate { FDCAuto.Visible = false; });
            //}
            //FrDoorsDownAuto();

        }

        private void FrDoorsDownAuto()
        {

            // Class2.Create10DOArray(1, 0,2,  1); //Reset both up coils
            Thread.Sleep(300);

            if (Class2.SetDoorsAlarm == true)
            {
                RaiseAlarm(30);
                Class2.SetDoorsAlarm = false;
            }
            if (IsHandleCreated)
            {
                // FDUAuto.Invoke((MethodInvoker)delegate {FDUAuto.BackColor = Color.Red; });

            }

            FrontDrOpenAutoTh.Abort();

        }
        private void OpenDoorsAutoTh()
        {
            //            Class2.CreateDoorsDOArray(8, 0, 9, 0); //open the doors

            //            DateTime StartTime = DateTime.Now;
            //            DateTime NewTime;
            //            TimeSpan Diff;
            //            Class1.Milliseconds = 0;

            //            do
            //            {

            //                NewTime = DateTime.Now;
            //                Diff = NewTime - StartTime;
            //                Class1.Milliseconds = (int)Diff.Seconds;

            //                if (Class1.Milliseconds > 2)
            //                {
            //                    RaiseAlarm(29);
            //                    BothDoorsAutoTh.Abort();
            //                    return;
            //                }

            //            }
            //            while (Class1.DI_ChDoor_Front == 0 & Class1.DI_ChDoorBack == 0);
            //            if (IsHandleCreated)
            //            {
            //                FDCAuto.Invoke((MethodInvoker)delegate { FDCAuto.Visible = false; BDCAuto.Visible = false; });
            //            }
            //            Thread.Sleep(200);
            //            if (IsHandleCreated)
            //            {
            //                FDUAuto.Invoke((MethodInvoker)delegate { FDUAuto.BackColor = Color.Green; BDUAuto.BackColor = Color.Green; });
            //}
            //            //Thread.Sleep(200);
            //            DoorsDownAutoTh();

        }

        private void DoorsDownAutoTh()
        {

            //Class2.CreateDoorsDOArray(1, 0, 3, 0, 2, 1, 4, 1); //Reset both up coils
            Thread.Sleep(300);

            if (Class2.SetDoorsAlarm == true)
            {
                RaiseAlarm(32);
                Class2.SetDoorsAlarm = false;
            }
            if (IsHandleCreated)
            {
                // FDUAuto.Invoke((MethodInvoker)delegate { FDUAuto.BackColor = Color.Red; BDUAuto.BackColor = Color.Red; });

            }

            BothDoorsAutoTh.Abort();

        }

        private void DoorsUpThAuto()
        {
            //Thread.Sleep(50);
            //Class2.CreateDoorsDOArray(1, 1, 3, 1, 2, 0, 4, 0); // Send both doors up with 1 new function CreateDOArrayDoors
            //DateTime StartTime = DateTime.Now;
            //DateTime NewTime;
            //TimeSpan Diff;
            //Class1.Milliseconds = 0;

            //do
            //{

            //    NewTime = DateTime.Now;
            //    Diff = NewTime - StartTime;
            //    Class1.Milliseconds = (int)Diff.Milliseconds;
            //    if (Class1.Milliseconds > 2000)
            //    {
            //        RaiseAlarm(20);
            //        Class2.CreateDoorsDOArray(8, 0, 9, 0);
            //        Class2.CreateDoorsDOArray(1, 0, 3, 0, 2, 1, 4, 1); // Send both doors down with 1 new function Create4DOArrayDoors
            //        BothDoorsUpThAuto.Abort();
            //        return;
            //    }

            //}
            //while (Class1.DI_FrontDoorup == 0 & Class1.DI_BackDoorUp == 0);


            //FDUAuto.Invoke((MethodInvoker)delegate { FDUAuto.BackColor = Color.Green; BDUAuto.BackColor = Color.Green; });
            //CloseDoorsThAuto();

        }

        private void CloseDoorsThAuto()
        {
            //Thread.Sleep(200);
            //Class2.CreateDoorsDOArray(8, 1, 9, 1);

            //if (Class1.DI_FrontDoorup == 0 & Class1.DI_BackDoorUp == 0)
            //{
            //    RaiseAlarm(23);
            //    Class2.CreateDoorsDOArray(1, 0, 3, 0, 2, 1, 4, 1); // Send both doors down with 1 new function Create4DOArrayDoors
            //}

            //if (IsHandleCreated)
            //{
            //    FDCAuto.Invoke((MethodInvoker)delegate {FDCAuto.Visible = true; BDCAuto.Visible = true; });

            //}
            //BothDoorsUpThAuto.Abort();

        }

        private void PartIn()
        {
            //if (Class1.DI_LeadFrameIn == 0 & Class1.DI_LeadFrameOut == 0)
            //{

            //    //Thread.Sleep(400);
            //    do
            //    {
            //        if (Class1.BackDoorNotUp == true)
            //        {
            //            break;
            //        }
            //        //waiting for the door to come up
            //    }
            //    while (Class1.DI_BackDoorUp == 0);
            //    {
            //        if (Class1.SendAlarm == 36) { Thread.Sleep(500); } //add delay in case of alarm 36 to avoid double alarm 25 and 37.
            //        if (Class1.DI_BackDoorUp == 1 & Class1.DI_ChDoorBack == 0)
            //        {
            //            Class2.Create10DOArray(14, 1, 23, 1, 25, 1);
            //            MotorTick = 0;
            //        }
            //        else
            //        {
            //            if(Class1.DI_BackDoorUp==0)
            //            { RaiseAlarm(22);}
            //            if(Class1.DI_ChDoorBack==1)
            //            { RaiseAlarm(25); }

            //            //Class1.FatalError = true; //check
            //        }

            //        Class1.ClockTick = 0;
            //    }
            //}
        }

        private void BPPartIn()
        {
            //Class2.Create10DOArray(13, 1);//Enable DBA
            //if (Class1.DI_LeadFrameOut == 1)
            //{ 
            //while (Class1.DI_LeadFrameOut == 1)
            //{
            //    RaiseMessage(36);
            //    if (Class1.StopClick == true)
            //    {
            //        break;
            //    }
            //    if(IsHandleCreated)
            //    { AlarmBox.Invoke((MethodInvoker)delegate { AlarmBox.Text = "ByPass Mode"; GroupBox5.Text = "Message"; });}

            //}
            //}


            //  if (Class1.DI_LeadFrameIn == 0 & Class1.DI_LeadFrameOut == 0)
            //    {
            //         if (Class1.DI_BackDoorDown == 1 )
            //            { 
            //                 while (Class1.DI_LFINREQ == 0)
            //                    {
            //                       RaiseMessage(39);
            //                        if (Class1.StopClick == true)
            //                        {
            //                            break;
            //                        }
            //                    }
            //             if(IsHandleCreated)
            //             {  AlarmBox.Invoke((MethodInvoker)delegate { AlarmBox.Text = ""; GroupBox5.Text = "Message"; });}

            //             if(Class1.DI_LFINREQ==1)
            //             { 
            //                 Class2.Create10DOArray(14, 1, 23, 1, 25, 1, 24, 1);
            //                 MotorTick = 0;
            //             }

            //            }
            //            else
            //            {
            //                RaiseAlarm(25);
            //                //Class1.FatalError = true; //check
            //            }

            //            Class1.ClockTick = 0;
            //    }
            //      if(Class1.DI_LeadFrameIn == 1)
            //          {
            //              while (Class1.DI_LeadFrameIn == 1)
            //              {
            //                  RaiseMessage(37);
            //                  if (Class1.StopClick == true)
            //                  {
            //                      break;
            //                  }
            //              }
            //          if(IsHandleCreated)
            //          { AlarmBox.Invoke((MethodInvoker)delegate { AlarmBox.Text = ""; GroupBox5.Text = "Message"; });}

            //        }
            //        if(Class1.DI_NewLot==1)
            //        {
            //            while (Class1.DI_NewLot == 1)
            //            {
            //                RaiseMessage(38);
            //                if (Class1.StopClick == true)
            //                {
            //                    break;
            //                }
            //            }
            //            if(IsHandleCreated)
            //            { AlarmBox.Invoke((MethodInvoker)delegate { AlarmBox.Text = ""; GroupBox5.Text = "Message"; }); }

            //        }
        }

        private void FrontDoorClosed()
        {
            // if (Class1.DI_BackDoorUp == 1 & Class1.DI_ChDoorBack == 0)
            //     {

            //     if (Class1.DI_LeadFrameIn == 0 & Class1.DI_NewLot == 0)
            //         {

            //             if (Class1.DO_ChamMotor == false & Class1.DO_InConvMotor == false)
            //             {
            //                 FrontDrCloseAutoTh = new Thread(new System.Threading.ThreadStart(FrDoorsUpThAuto));
            //                 FrontDrCloseAutoTh.Start();
            //                 while (FrontDrCloseAutoTh.IsAlive)
            //                 {
            //                     Application.DoEvents();
            //                 }

            //             }
            //             else
            //             {
            //                 if (Class1.DO_ChamMotor == true)
            //                 {
            //                     RaiseAlarm(42);
            //                 }
            //                 if(Class1.DO_InConvMotor == true)
            //                 {
            //                     RaiseAlarm(40);
            //                 }
            //             }
            //         }
            //     else
            //     {
            //         if (Class1.DI_LeadFrameIn == 1)
            //         {
            //             RaiseAlarm(32);
            //         }
            //         else if (Class1.DI_NewLot == 1)
            //         {
            //             RaiseMessage(37);
            //             motorStopped = false;
            //             Class1.RunStep = 13;
            //             return;

            //         }
            //     }
            //     }

            // Thread.Sleep(200);
            //if (Class1.DI_ChDoor_Front == 0 & Class1.DI_ChDoorBack == 0)
            // {
            //     Class2.Create10DOArray(21, 0);//Pump Speed1 Off
            //     Thread.Sleep(100);
            // }

            // if(Class1.ACBool ==true )
            // {
            //     BothDoorsUpThAuto = new Thread(new System.Threading.ThreadStart(DoorsUpThAuto));
            //     BothDoorsUpThAuto.Start();
            //     while (BothDoorsUpThAuto.IsAlive)
            //     {
            //         Application.DoEvents();
            //     }
            //     Thread.Sleep(200);
            //     Class2.Create10DOArray(6, 0);//Vent Valve Closed

            // }
        }


        private void motorStop()
        {
            int count = 0;
            motorStopped = false;
            try
            {
                // Class2.Create10DOArray(23, 0,24,0, 25, 0,14,0);//23-Input Motor,24-Out Motor,25-Chamber Motor,14-Send Request  
                MotorTick = 0;
                motorStopped = true;
            }
            catch
            {
                Class1.FatalError = true;


            }

        }

        private void Startbtn_Click(System.Object sender, System.EventArgs e)
        {
            Class1.Stopping = false;
            ClkTimCount = 0;
            try
            {
                Class2.SetDO(Class1.DOSlotNum, 1, true); // Start Pump
                Thread.Sleep(200);
            }
            catch
            {
                Class1.FatalError = true;
            }
            finally
            {
                StartAutoCycle();
            }

        }

        private void InitialiseMotor()
        {
            //    Thread.Sleep(100);
            //Class2.Create10DOArray(13, 1);//DBA
            //if (Class1.DI_LFINREQ == 1) // Check that downstream is not busy
            //{
            //    if (Class1.DI_LeadFrameIn == 0 & Class1.DI_LeadFrameOut == 0)
            //    {
            //        BothDoorsAutoTh = new Thread(new System.Threading.ThreadStart(OpenDoorsAutoTh));
            //        BothDoorsAutoTh.Start();
            //        while (BothDoorsAutoTh.IsAlive)
            //        { Application.DoEvents(); }
            //        Thread.Sleep(500);

            //        if (Class1.DI_FrontDoorDown == 1)
            //        {

            //            if (Class1.DI_BackDoorDown == 1)
            //            {
            //               Class2.Create10DOArray(23, 1, 24, 1, 25, 1); // Start the motors
            //                initMotor = true;

            //            }else
            //                {
            //                    RaiseAlarm(31);//Back door is not down
            //                    Thread.Sleep(1000);
            //                    return;
            //                }
            //        }else
            //            {
            //                RaiseAlarm(30);//Front door is not down
            //                Thread.Sleep(1000);
            //                return;
            //            }
            //    }
            //    else
            //    {
            //        if (Class1.DI_LeadFrameIn == 1)
            //        {
            //            RaiseAlarm(32);
            //            return;
            //        }
            //        else if (Class1.DI_LeadFrameOut == 1)
            //        {
            //            RaiseMessage(36);

            //            return;
            //        }
            //    }

            //}
            //else
            //{
            //    RaiseMessage(39);

            //}
        }
        private void StartAutoCycle()
        {
            Thread.Sleep(500);
            Class2.ProgRead();
            if (Class1.Gas1DB == 0.0 && Class1.Gas2DB == 0.0)
            {
                RaiseAlarm(19);
                return;
            }

            if (Class1.AlarmPause == true)
            {
                MessageBox.Show("Please Reset the Outstanding Alarm and then Start again.");
                return;
            }

            if (CycleRun.IsBusy == false)
            {
                Startbtn.Visible = false;
                Help.Visible = false;
                //Modebtn.Visible = false;
                //Stopbtn.Visible = true;
                Class1.StopClick = false;
                Class1.PDTick = 0;
                Class1.PumpTick = 0;
                Class1.TTPTick = (int)Class1.R_TTP;
                Class1.DelayTick = 0;
                Class1.PlasmaTick = (int)Class1.R_RFT;
                Class1.PlasmaTick = Class1.PlasmaTick * 10;
                Class1.VentTick = Class1.Venttime;
                Class1.DoingPumpOn = false;
                Class1.DoingWaitForPressvalve = false;
                Class1.DoingWaitForPT = false;
                Class1.DoingTTP = false;
                Class1.DoingPlasma = false;
                Class1.DiDPlasma = false;
                Class1.DiDPumpDown = false;
                Class1.DiDTTP = false;
                Class1.CycleStart = true;
                Class1.RunStep = 0;
                PlasmaDone = false;
                motorStopped = false;
                Shift.Enabled = false;
                CycleS = false;
                initMotor = false;
                CycleFirstTL = false;
                LeadFrameInFlg = false;
                LeadFrameOutFlg = false;
                LastAlarm = 0;

                MotorTick = 0;
                RFTimeBar.Invoke((MethodInvoker)delegate { RFTimeBar.Maximum = MaxRFT; RFTimeBar.Minimum = 0; RFTimeBar.Value = (int)Class1.R_RFT * 10; });
                //(int)Class1.R_RFT;
                //Class2.Create10DOArray(13, 1); //DBA ON
                LRFTime.Text = String.Format("{0:n1}", MaxRFT / 10);
                Label24.Text = Class1.ClockTick.ToString();
                ClocktimerTh = new Thread(new System.Threading.ThreadStart(Clocktimer));
                ClocktimerTh.Start();
                CycleRun.RunWorkerAsync();
            }
            else
            {
                Class1.ByPass = false;
                // BPbtn.Text = "ByPass On";
                MessageBox.Show("Cycle is already running.");

            }

        }
        private void CycleRun_DoWork(System.Object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            do
            {


                if (Class1.RunStep == 0) //  Step 0:  At start button, clear the chamber from leadframes
                {
                    // if(Class1.DI_LFINREQ==1)// if nothing on out conveyor
                    // { 

                    if (initMotor == false)
                    {
                        InitialiseMotor();
                        MotorTick = 0;
                    }

                    //if (Class1.DI_LeadFrameIn == 1)
                    //{
                    //    motorStop();
                    //    RaiseMessage(32);
                    //}
                    //else
                    //  if (Class1.DI_LeadFrameOut == 1)
                    //  {
                    //      motorStop();
                    //      RaiseMessage(36);
                    //  }
                    else
                        if (MotorTick >= LapseAutoinit)
                        {
                            motorStop();
                            Thread.Sleep(20);
                            if (Class1.Stopping == false)
                            {
                                if (Class1.ByPass == false)
                                {
                                    //if (Class1.DO_ChamMotor == false & Class1.DO_ChamMotor == false)
                                    //{
                                    //    BackDrCloseAutoTh = new Thread(new System.Threading.ThreadStart(BackDoorsUpThAuto));
                                    //    BackDrCloseAutoTh.Start();
                                    //    while (BackDrCloseAutoTh.IsAlive)
                                    //    {
                                    //        Application.DoEvents();
                                    //    }
                                    //}
                                }
                            }

                            motorStopped = false;
                            // Class2.Create10DOArray(13, 0);//DBA Off
                            Class1.RunStep = 1;
                        }
                    // }
                    //  else { RaiseMessage(39); }
                }
                if (Class1.RunStep == 1)
                {

                    if (Class1.ACBool == true)
                    {
                        Class1.RunStep = 2;

                        Class1.StartCounting = true;
                        Class1.ClockTick = 0;
                    }
                    Class1.Venting = false;
                    //if (Class1.DI_NewLot == 0)
                    //{
                    if (Class1.PumpTick <= 10)
                    {
                        if (CycleFirstTL == false)
                        {
                            CycleFirstTL = true;

                            int[] DIOChannelArr = { Class1.DO27, Class1.DO26 }; //Turn on yellow light,Turn off green light
                            bool[] DIOStateArr = { true, false };
                            Class2.SetMultiDIO(DIOChannelArr, DIOStateArr);
                            //Class2.SetDO(Class1.DIOSlotNum, Class1.DO26, false);
                            //Class2.SetDO(Class1.DIOSlotNum, Class1.DO27, true);
                        }
                        RaiseMessage(34);
                        Class1.PumpOn = true;

                    }
                    else
                    {
                        //Class1.PumpOn = false;
                        Class1.PumpTick = 0;

                        i = 0;

                        // turn light from green to yellow
                        //set speed bits both to 1
                        // later add sleep mode
                    }

                    //}
                    //else
                    //{
                    //    Class1.StartCounting = true;

                    //    if (Class1.ByPass == true)
                    //    {
                    //        if(CycleFirstTL == true)
                    //        {
                    //            Class2.Create10DOArray(29, 1, 28, 0);
                    //            CycleFirstTL = false;
                    //        }
                    //        Class1.RunStep = 14;

                    //    }
                    //    else
                    //    {

                    //        if (Class1.ByPassMode == false)
                    //        {
                    //            if (IsHandleCreated)
                    //            {
                    //                AlarmBox.Invoke((MethodInvoker)delegate { AlarmBox.Text = ""; });
                    //            }
                    //        }   
                    //    PartIn();
                    //    Class1.RunStep = 2;
                    //    }


                    //}
                }


                if (Class1.RunStep == 2)
                {

                    int[] DIOChannelArr = { Class1.DO26, Class1.DO27 }; //Turn on green light,Turn off yellow light
                    bool[] DIOStateArr = { true, false };
                    Class2.SetMultiDIO(DIOChannelArr, DIOStateArr);
                    //Class2.SetDO(Class1.DIOSlotNum, Class1.DO26, true);
                    //Class2.SetDO(Class1.DIOSlotNum, Class1.DO27, false);
                    if (MotorTick >= Class1.MotorLapseTime) // Note. later change to sensor check
                    {
                        motorStop();
                    }
                    if (Class1.ACBool == true) { motorStopped = true; }
                    //if (motorStopped == true)
                    //{
                    //    if (Class1.DO_InConvMotor == false & Class1.DO_ChamMotor == false)
                    //    {
                    //        if (Class1.DI_LeadFrameIn == 0)
                    //        {
                    //            FrontDoorClosed();
                    //            LeadFrameInFlg = true;
                    //            motorStopped = false;
                    //        }
                    //        else
                    //        {
                    //            RaiseMessage(38);
                    //            motorStopped = false;
                    //            Class1.RunStep = 13;
                    //            //return; 
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (Class1.DO_ChamMotor == true)
                    //        {
                    //            RaiseAlarm(42);
                    //        }
                    //        if (Class1.DO_InConvMotor == true)
                    //        {
                    //            RaiseAlarm(40);
                    //        }
                    //    }


                    //    if (Class1.DI_ChDoor_Front == 0 & Class1.DI_ChDoorBack == 0)
                    //    {
                    //        Class2.Create10DOArray(18, 1, 6, 0, 7, 1); //Set Tuner to Manual & Close Vent Valve & open the Vacum Valve

                    //        if (Class1.R_G1 > 0)
                    //        {

                    //            Thread.Sleep(50);
                    //            Class2.Create10DOArray(10, 1); //Gas1
                    //        }
                    //        if (Class1.R_G2 > 0)
                    //        {
                    //            Thread.Sleep(50);
                    //            Class2.Create10DOArray(11, 1);////Gas2
                    //        }
                    //        Class1.RunStep = 3;
                    //    }
                    //    else
                    //    {
                    //        CycleRunCont();
                    //       // do nothing
                    //    }

                    //}


                }



                if (Class1.RunStep == 3)
                {
                    //Class2.Create10DOArray(20, 1); //Start pump
                    Class1.DoingPumpOn = true;
                    Class1.RunStep = 4;
                }



                if (Class1.RunStep == 4)  //Class1.ClockTick >= 6 & 
                {

                    Class1.DoingWaitForPressvalve = true;
                    Class1.RunStep = 5;

                }

                if (Class1.ClockTick >= 15 & Class1.RunStep == 5)
                {
                    //Class2.Create10DOArray(5, 1); // Pressure Valve
                    this.Invoke((MethodInvoker)delegate { Class2.SetDO(Class1.DOSlotNum, 20, true); });
                    Class1.RunStep = 6;
                }

                if (RealPressure < Class1.R_PT & Class1.RunStep == 6)
                {
                    Int32 ret2 = default(Int32);
                    double val2 = 0;
                    Class1.DoingWaitForPT = true;
                    val2 = Convert.ToDouble(Class1.R_G1 / (Class1.Gas1R / 10) * Class1.GCF1);
                    //ret2 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAO, 1, val2);
                    Class2.setAOValues(Convert.ToString(val2), 3);


                    Int32 ret3 = default(Int32);
                    double val3 = 0;
                    val3 = Convert.ToDouble(Class1.R_G2 / (Class1.Gas2R / 10) * Class1.GCF2);
                    // ret3 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAO, 2, val3);
                    Class2.setAOValues(Convert.ToString(val2), 4);

                    Class1.DiDPumpDown = true;
                    Class1.RunStep = 7;
                }



                //Wait TTP Time to elapse
                if (Class1.TTPTick <= 0 & Class1.RunStep == 7)
                {
                    Class1.DoingTTP = true;
                    Class1.DiDTTP = true;
                    Class1.RunStep = 8;
                }

                if (Class1.Intlk == true & Class1.RunStep == 8)
                {
                    if (Class1.DO_RFON == false)
                    {
                        this.Invoke((MethodInvoker)delegate { Class2.SetDO(Class1.DOSlotNum, 0, true);  }); //RF ON
                        Class1.RunStep = 9;
                    }
                }

                if (Class1.DO_RFON == true & Class1.RunStep == 9)
                {
                    Thread.Sleep(100);
                    //wait 100 msec
                    // Class2.Create10DOArray(18, 0); //need to check
                    this.Invoke((MethodInvoker)delegate { Class2.SetDO(Class1.DOSlotNum, 2, true); });
                    // Set Tuner to Automatic
                    Class1.RunStep = 10;
                }


                //Wait RF Time Time to elapse
                if (Class1.RFTick <= 0 & Class1.RunStep == 10)
                {
                    Class1.DoingPlasma = true;
                    Class1.DiDPlasma = true;
                    Class1.RunStep = 11;
                }



                if (Class1.StopClick == true | Class1.FatalError == true)
                {
                    if (Class1.ByPass == false)
                    {
                        this.Invoke((MethodInvoker)delegate { Class2.SetDO(Class1.DOSlotNum, 0, false); });//Close RF ON\
                        Class1.CycleStart = false;
                        CycleRunCont();
                        Class1.FatalError = false;
                        Class1.Venting = false;
                        Class1.Stopping = true;
                        motorStop();
                        i = 0;
                        if (BothDoorsAutoTh != null)
                        { BothDoorsAutoTh.Abort(); }

                        try
                        {
                            // AVentNow();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                        //Modebtn.Invoke ((MethodInvoker)delegate{Modebtn.Visible = true;Startbtn.Visible = true;Stopbtn.Visible = false;});
                        // RaiseAlarm(Class1.SendAlarm);
                    }
                    else
                    {
                        Class1.CycleStart = false;
                        CycleRunCont();
                        Class1.FatalError = false;
                        Class1.Venting = false;
                        Class1.Stopping = true;
                        motorStop();
                        i = 0;

                    }
                }

                if (Class1.RunStep == 11)
                {

                    int[] DOChannelArr = { 0, 20 }; //Turn off yellow light,Turn off green light,Turn on red light,Turn on Buzzer
                    bool[] DOStateArr = { false, false };
                    //Class2.SetMultiDO(DOChannelArr, DOStateArr);
                    this.Invoke((MethodInvoker)delegate { Class2.SetMultiDO(DOChannelArr, DOStateArr); });

                    //Class2.SetDO(Class1.DOSlotNum, 0, false); //RF
                    //Class2.SetDO(Class1.DOSlotNum, 20, false); //Pressure
                    PlasmaDone = true;
                    Class1.RunStep = 12;

                    // break; // TODO: might not be correct. Was : Exit Do
                }

                if (Class1.RunStep == 12 & Class1.DiDPlasma == true)
                {
                    Thread.Sleep(50);


                    int[] DOChannelArr = { 20, 21 }; //Pressure Valve off,Purge Valve On ,Gas1 Off,Gass 2 Off,VacVAlve Off
                    bool[] DOStateArr = { false, true };
                    //Class2.SetMultiDO(DOChannelArr, DOStateArr);
                    this.Invoke((MethodInvoker)delegate { Class2.SetMultiDO(DOChannelArr, DOStateArr); });


                    int[] DIOChannelArr = { Class1.DO24, Class1.DO25 }; //Pressure Valve off,Purge Valve On ,Gas1 Off,Gass 2 Off,VacVAlve Off
                    bool[] DIOStateArr = { false, false };
                    Class2.SetMultiDIO(DIOChannelArr, DIOStateArr);

                    this.Invoke((MethodInvoker)delegate { Class2.SetDO(Class1.DOSlotNum, 22, false); });//VacVAlve Off

                    //Class2.SetDO(Class1.DOSlotNum, 20, false);//Pressure Valve off
                    //Class2.SetDO(Class1.DOSlotNum, 21, true); //Purge Valve On 
                    //Class2.SetDO(Class1.DIOSlotNum, Class1.DO24, false);//Gas1 Off
                    //Class2.SetDO(Class1.DIOSlotNum, Class1.DO25, false); //Gass 2 Off


                    Class1.DoneOnce = false;
                    VentNow();
                    if (Class1.SendAlarm == 33)
                    {
                        Class1.RunStep = 13;
                    }
                    if (Class1.SendAlarm != 33)
                    {
                        if (Class1.RunStep != 13)
                        { Class1.RunStep = 1; }
                    }
                }

                if (Class1.RunStep == 13)
                {
                    // wait for Reset Alarm button
                }
                if (Class1.RunStep == 14)
                {
                    BPPartIn();
                    BPPartOut();
                    Class1.RunStep = 1;
                }

            } while (Class1.CycleStart == true);
            Thread.Sleep(1000);
        }

        private void CycleRun_RunWorkerCompleted(System.Object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            // do nothing
        }
        private void CycleRunCont()
        {

            initMotor = false;
            MotorTick = 0;
            Class1.PDTick = 0;
            Class1.PumpTick = 0;
            Class1.PumpOn = false;
            Class1.TTPTick = (int)Class1.R_TTP;
            Class1.DelayTick = 0;
            Class1.PlasmaTick = (int)Class1.R_RFT;
            Class1.PlasmaTick = Class1.PlasmaTick * 10;
            Class1.VentTick = Class1.Venttime;
            if (IsHandleCreated)
            { RFTimeBar.Invoke((MethodInvoker)delegate { RFTimeBar.Maximum = MaxRFT; RFTimeBar.Minimum = 0; }); }

            Class1.DoingPumpOn = false;
            Class1.DoingWaitForPressvalve = false;
            Class1.DoingWaitForPT = false;
            Class1.DoingTTP = false;
            Class1.DoingPlasma = false;
            Class1.DiDPlasma = false;
            Class1.DiDPumpDown = false;
            Class1.DiDTTP = false;
            PlasmaDone = false;
            motorStopped = false;
            CycleS = false;
            CycleFirstTL = false;
            LeadFrameInFlg = false;
            LeadFrameOutFlg = false;
            LastAlarm = 0;
        }
        private void DnStreamChk()
        {
            //Class2.Create10DOArray(13,1);
            //while(Class1.DI_LFINREQ==0)
            //{
            //    RaiseMessage(39);
            //    Thread.Sleep(5000);
            //    if(Class1.DI_FrontDoorDown==1 & Class1.DI_FrontDoorDown==1)
            //    { 
            //    BothDoorsUpThAuto = new Thread(new System.Threading.ThreadStart(DoorsUpThAuto));
            //    BothDoorsUpThAuto.Start();
            //    while(BothDoorsUpThAuto.IsAlive)
            //    {
            //        Application.DoEvents();
            //    }
            //    Thread.Sleep(200);
            //    Class2.Create10DOArray(6, 0);

            //    Thread.Sleep(200);
            //    Class2.Create10DOArray(7, 1);
            //    }
            //}
            //Class2.Create10DOArray(7, 0);

            //Thread.Sleep(200);
            //Class2.Create10DOArray(6, 1);

            //BothDoorsAutoTh = new Thread(new System.Threading.ThreadStart(OpenDoorsAutoTh));
            //BothDoorsAutoTh.Start();
            //while (BothDoorsAutoTh.IsAlive)
            //{ Application.DoEvents(); }
            //Thread.Sleep(500);

        }

        public void PartOut()
        {
            //int MotorTickOut=0;

            //if(Class1.DI_BackDoorDown==1 & Class1.DI_ChDoorBack==1)
            //{
            //    // Enable DBA
            //    Class2.Create10DOArray(13, 1);
            //    //Thread.Sleep(500);
            //    //DnStreamChk();
            //    //RaiseAlarm(0);

            //    while (Class1.DI_LFINREQ == 0 )
            //    {
            //        RaiseMessage(39);
            //        if (Class1.StopClick==true)
            //        {
            //            break;
            //        }
            //    }
            //    if(IsHandleCreated)
            //    { AlarmBox.Invoke((MethodInvoker)delegate { AlarmBox.Text = ""; GroupBox5.Text = "Message"; });}

            //    if (Class1.DI_LFINREQ == 1)  //DBS
            //    {
            //        Class2.Create10DOArray(24, 1, 25, 1); // run the  Center and output motor

            //        try
            //        {
            //            do
            //            {
            //                Thread.Sleep(1000);
            //                MotorTickOut += 1;
            //                if (MotorTickOut > Class1 .MotorLapseTime )
            //                {
            //                    break;
            //                }
            //            } while (Class1.DI_LFINREQ == 1); 
            //        }
            //        catch (Exception e)
            //        { }

            //        motorStop();

            //        Class2.Create10DOArray(13, 0); // disable DBA
            //        Class1.StartCounting = false ;
            //        if(Class1.ClockTick>0)
            //        { if(IsHandleCreated)
            //        {label54.Invoke((MethodInvoker)delegate { label54.Text = (3600 / Class1.ClockTick).ToString(); });}
            //        }

            //        if (Class1.DI_LeadFrameOut == 0)
            //        {
            //          if (Class1.Stopping ==false )
            //            {
            //                if (Class1.DO_ChamMotor == false & Class1.DO_ChamMotor == false)
            //                {
            //                    if (LeadFrameInFlg==false & LeadFrameOutFlg==true)
            //                    {


            //                    try
            //                    { 
            //                        BackDrCloseAutoTh = new Thread(new System.Threading.ThreadStart(BackDoorsUpThAuto));
            //                        BackDrCloseAutoTh.Start();
            //                        while (BackDrCloseAutoTh.IsAlive)
            //                        {
            //                            Application.DoEvents();
            //                        }
            //                    }
            //                    catch
            //                    {

            //                    }
            //                 Class1.RunStep = 1;
            //                }else
            //                {
            //                    RaiseAlarm(33);

            //                }
            //                }

            //            }
            //            motorStopped = false;
            //            Class2.Create10DOArray(13, 0);
            //         }
            //        else 
            //        {
            //            RaiseMessage(36);
            //            Class1.RunStep = 13;
            //            motorStopped = false;
            //            return;

            //        }


            //    }

            //}
        }
        public void BPPartOut()
        {
            //int MotorTickOut = 0;


            //// Enable DBA
            //Class2.Create10DOArray(13, 1);

            //while (Class1.DI_LFINREQ == 0)
            //{
            //    if (Class1.StopClick == true)
            //    {
            //        break;
            //    }
            //}
            //if(IsHandleCreated)
            //{ AlarmBox.Invoke((MethodInvoker)delegate { AlarmBox.Text = "ByPass Mode"; GroupBox5.Text = "Message"; });}

            //if (Class1.DI_LFINREQ == 1)  //DBS
            //{
            //    Class2.Create10DOArray(24, 1, 25, 1); // run the  Center and output motor

            //    try
            //    {
            //        do
            //        {
            //            Thread.Sleep(1000);
            //            MotorTickOut += 1;
            //            if (MotorTickOut > Class1.MotorLapseTime)
            //            {
            //                break;
            //            }
            //        } while (Class1.DI_LFINREQ == 1);
            //    }
            //    catch (Exception e)
            //    { }

            //    motorStop();
            //    Thread.Sleep(100);
            //    Class2.Create10DOArray(13, 0); // disable DBA

            //    if (Class1.DI_LeadFrameOut == 0)
            //    {

            //        if (Class1.Stopping == false)
            //        {

            //            Class1.RunStep = 1;
            //        }
            //        motorStopped = false;
            //        Class2.Create10DOArray(13, 0);
            //    }
            //    else 
            //    {
            //        while (Class1.DI_LeadFrameOut == 1)
            //        {
            //            RaiseMessage(36);
            //            if (Class1.StopClick == true)
            //            {
            //                break;
            //            }
            //        }
            //        if(IsHandleCreated)
            //        { AlarmBox.Invoke((MethodInvoker)delegate { AlarmBox.Text = "ByPass Mode"; GroupBox5.Text = "Message"; }); }

            //    }




            //}
        }



        /********************End of Start***************************************/
        private void Button17_Click(System.Object sender, System.EventArgs e)
        {
            ModifySuccess objModifySuccess = new ModifySuccess();
            objModifySuccess.Show();
        }

        private void Modebtn_Click(System.Object sender, System.EventArgs e)
        {
           // Class1.objLogevent.Close();
            //Class1.objLogevent.Dispose();
            Class1.ByPassMode = false;
            Class1.ByPass = false;
            ACbtn.Visible = false;
            Class1.MainLoadFlg = false;
            this.Close();
            this.Dispose();
            Mode objMode = new Mode();
            objMode.ShowDialog();

        }

        private void Exitbtn_Click(System.Object sender, System.EventArgs e)
        {
            System.Environment.Exit(0);
        }


        private void Stopbtn_Click(System.Object sender, System.EventArgs e)
        {
            Stopbtn.Visible = false;
            Class1.FatalError = false;
            Class1.Venting = false;
            Class1.Stopping = true;
            ACbtn.Visible = false;
            motorStop();
            if (Class1.ByPass == false)
            {
                if (BothDoorsAutoTh != null)
                {
                    BothDoorsAutoTh.Abort();
                }
                try
                {
                    //AVentNow();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                try
                {
                    // ByPassVent();
                }
                catch { }

            }


            //Modebtn.Visible = true;
            //Startbtn.Visible  = true;
            //Stopbtn.Visible = false;

            i = 0;


        }

        private void Main_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            Class1.AlarmActive = false;
            if (TestThreadTh != null)
            {
                TestThreadTh.Abort();
            }
            // Timer1.Enabled = false;
            //ClockTimer.Enabled = false;
            if (ClocktimerTh != null)
            { ClocktimerTh.Abort(); }
            this.Dispose();
        }

        private void Button8_Click(System.Object sender, System.EventArgs e)
        {
            //If UManual = "Allowed" Then
            //    Graph.ShowDialog()
            //Else
            //    Programs.ShowDialog()
            //End If

            Graph objGraph = new Graph();
            objGraph.ShowDialog();
        }

        private void Button6_Click(System.Object sender, System.EventArgs e)
        {
            //Reset the Buzzer
            Class1.FatalError = false;
            Class1.HoldBuzzer = true;
        }

        private void WirteLogThread_DoWork(System.Object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            string NowDate = null;

            NowDate = DateTime.Now.ToString();
            NowDate = NowDate.Replace("/", "-");
            NowDate = NowDate.Replace(":", "-");

            if (DateTime.Now.Date > Class1.FileStartTime)
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
                    //Dim RNow As DateTime
                    FS.WriteLine(DateTime.Now + "Program# " + Class1.CurrentP + " Alarm" + Class1.AlarmMsg + ",");
                    FS.Flush();
                    FS.Close();

                }
            }
            catch
            {
            }
        }

        private void Help_Click(System.Object sender, System.EventArgs e)
        {
            Documentation objDoc = new Documentation();
            objDoc.ShowDialog();

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

        private void AlarmHandler()
        {
            // this.alarmLogTableTableAdapter.Fill(this.setupDataSet.AlarmLogTable);
            try
            {
                AlarmLogTableTableAdapter MainAlarmTAobj = new AlarmLogTableTableAdapter();
                MainAlarmTAobj.Fill(setupDataSet.AlarmLogTable);
                //MainAlarmTAobj.Fill(MainsetupDatasetObj.AlarmLogTable);

                if (Class1.SendAlarm != LastAlarm)
                {
                    switch (Class1.SendAlarm)
                    {
                        case 0:
                            break;
                        case 1:
                            Class1.AlarmMsg = "# 1 - No Air";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 1, "No Air", "Check the Air connection, valve and sensor.");
                            Class1.FatalError = true;
                            break;
                        case 2:
                            Class1.AlarmMsg = "# 2 - Pump Error";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 2, "Pump Error", "Check the Vacuum Pump connection and temperature.");
                            Class1.FatalError = true;
                            break;
                        case 3:
                            Class1.AlarmMsg = "# 3 - No Plasma";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 3, "No Plasma", "Check the Program setting, Tuner and RF generator.");
                            Class1.FatalError = true;
                            break;
                        case 4:
                            Class1.AlarmMsg = "# 4 - High Reflected Power";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 4, "High Reflective Power", "Check the Program setting, Tuner and RF generator.");
                            Class1.FatalError = true;
                            break;
                        case 5:
                            Class1.AlarmMsg = "# 5 - No Gas 1";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 5, "No Gas 1", "Check the Gas 1 supply line and bottle.");
                            Class1.FatalError = true;
                            break;
                        case 6:
                            Class1.AlarmMsg = "# 6 - No Gas 2";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 6, "No Gas 2", "Check the Gas 2 supply line and bottle.");
                            Class1.FatalError = true;
                            break;
                        case 7:
                            Class1.AlarmMsg = "# 7 - No Gas 3";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 7, "No Gas 3", "Check the Gas 3 supply line and bottle.");
                            Class1.FatalError = true;
                            break;
                        case 8:
                            Class1.AlarmMsg = "# 8 - PumpDown Error";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 8, "Pump Down Error", "Check the program settings,the chamber O-Rings and the Pump.");
                            Class1.FatalError = true;
                            break;
                        case 9:
                            Class1.AlarmMsg = "# 9 - Door Sensor Error";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 9, "Door Sensor Error", "You cannot Start with the door open; or, check the door sensor.");
                            Class1.FatalError = true;
                            break;
                        case 10:
                            Class1.AlarmMsg = "# 10 - E-Stop";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 10, "E-Stop", "The E-Stop is engaged.");
                            Class1.FatalError = true;
                            break;
                        case 11:
                            Class1.AlarmMsg = "# 11 - MFC 1";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 11, "MFC 1", "Check Gas Valve 1 Or, Check the mass flow Controller.");
                            break;
                        case 12:
                            Class1.AlarmMsg = "# 12 - MFC 2";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 12, "MFC 2", "TCheck Gas Valve 1 Or, Check the mass flow Controller.");
                            break;
                        case 13:
                            Class1.AlarmMsg = "# 13 - MFC 3";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 13, "MFC 3", "Check Gas Valve 1 Or, Chec the mass flow Controller.");
                            break;
                        case 14:
                            Class1.AlarmMsg = "# 14 - Pres.Trig. Timeout";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 14, "Pres.Trig. Timeout", "The program Settings have too mach gas and/or too low Pressure Trigger.");
                            break;
                        case 15:
                            Class1.AlarmMsg = "# 15 - No Purge Gas";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 15, "No Purge Gas", "Check the Gas 1 supply line and bottle");
                            Class1.FatalError = true;
                            break;
                        case 16:
                            Class1.AlarmMsg = "# 16 - Reaching Plasma Pressure Timeout";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 16, "Reaching Plasma Pressure Timeoutt", "The System Cannot reach the Pressure trigger Point set in your recipe. Please check if the Gas value set in the recipe is correct, or check the gas lines.");
                            break;
                        case 17:
                            Class1.AlarmMsg = "# 17 - RF Interlock Error";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 17, "RF Interlock Error", "The System tried to turn on the RF power and a too high vacuum pressure.");
                            break;
                        case 18:
                            Class1.AlarmMsg = "# 18 - Plasma Interrupt";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 18, "Plasma Interrupt", "During Plasma The Stop Button or the E-Stop have been pressed, or there has been a power failure.");
                            break;

                        case 19:
                            Class1.AlarmMsg = "# 19 - Both Gas lines set to 0";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 19, "Both gas lines set to 0", "You are attempting to start an Automatic or Manual cycle without any gas set in your program.");
                            break;
                        case 20:
                            Class1.AlarmMsg = "# 20 - Both the Doors are not Up Error";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 20, "Both the Doors Up Error", "indicates that both doors have not reached their up sensors.");
                            break;
                        case 21:
                            Class1.AlarmMsg = "# 21 - Front Door is not Up Error";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 21, "Front Door Up Error", "indicates tha the front door only has not reached its up sensor.");
                            break;
                        case 22:
                            Class1.AlarmMsg = "# 22 - Back Door is not Up Error";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 22, "Back Door Open Error", "indicates tha the back door only has not reached its up sensor.");
                            break;
                        case 23:
                            Class1.AlarmMsg = "# 23 - Both the Doors are not Closed Error";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 23, "Both the Doors are not Closed Error", "Indicates that both doors are not closeed.");
                            break;
                        case 24:
                            Class1.AlarmMsg = "# 24 - Front Door is not Closed Error";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 24, "Front Door is not Closed Error", "Indicates the the front door is not closed.");
                            break;

                        case 25:
                            Class1.AlarmMsg = "# 25 - Back door is not closed Error";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 25, "back door is not closed Error", "Indicates the the back door is not closed.");
                            break;

                        case 26:
                            Class1.AlarmMsg = "# 26 - Both doors are not open Error";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 26, "Both doors are not open Error", "Indicates that both doors are not open.");
                            break;

                        case 27:
                            Class1.AlarmMsg = "# 27 - Front door is not open Error";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 27, "Front door is not open Error", "Indicates the the front door is not open.");
                            break;

                        case 28:
                            Class1.AlarmMsg = "# 28 - Back door is not open Error";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 28, "Back door is not open Error", "Indicates the the back door is not open.");
                            break;

                        case 29:
                            Class1.AlarmMsg = "# 29 - Both doors are not down Error";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 29, "Both doors are not down Error", "Indicates that both doors are not down.");
                            break;

                        case 30:
                            Class1.AlarmMsg = "# 30 - Front door is not down Error";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 30, "Front door is not down Error", "Indicates the the front door is not down.");
                            break;

                        case 31:
                            Class1.AlarmMsg = "# 31 - Back door is not down Error";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 31, "back door is not down Error", "Indicates the the back door is not down.");
                            break;
                        case 32:
                            Class1.AlarmMsg = "# 32 - Conveyor Input Error";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 32, " LeadFrameIn Sensor Error", " LeadFrameIn Sensor Error.");
                            break;
                        case 33:
                            Class1.AlarmMsg = "# 33 - LeadFrame in Chamber Error";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 33, "LeadFrame in Chamber Error", "A LeadFrame is in the Chamber. Please Remove to continue.");
                            Class1.FatalError = true;
                            break;
                        case 34:
                            Class1.AlarmMsg = "# 34 - No LeadFrame in the Upstream Conveyor";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 34, "No Lead Frame in the Upstream Conveyor", "No Lead Frame in the Upstream Conveyor.");
                            break;
                        case 35:
                            Class1.AlarmMsg = "# 35 - Down Stream Board is not available";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 35, "Down Stream Board is not available", "Down Stream Board is not available.");
                            break;
                        case 36:
                            Class1.AlarmMsg = "# 36 - LeadFrameOut Sensor Error, Please remove the LeadFrame and Reset Alarm / Message to continue the process";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 36, " LeadFrameOut Sensor Error", " LeadFrameOut Sensor Error.");
                            break;
                        case 37:
                            Class1.AlarmMsg = "# 37 - LeadFrame cannot enter the chamber";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 37, "LeadFrame cannot enter the chamber", "LeadFrame cannot enter the chamber.");
                            break;
                        case 38:
                            Class1.AlarmMsg = "# 38 - LeadFrame is in Input Conveyor";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 38, "LeadFrame is in Input Conveyor", "LeadFrame is in Input Conveyor.");
                            break;
                        case 39:
                            Class1.AlarmMsg = "# 39 - Downstream is busy";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 39, "Downstream is busy", "Downstream is busy.");
                            break;
                        case 40:
                            Class1.AlarmMsg = "# 40 - PartIn Motor is runnig error";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 40, "PartIn Motor is runnig error", "PartIn Motor is runnig error.");
                            Class1.FatalError = true;
                            break;
                        case 41:
                            Class1.AlarmMsg = "# 41 - PartOut Motor is running error";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 41, "PartOut Motor is running error", "PartOut Motor is running error.");
                            Class1.FatalError = true;
                            break;
                        case 42:
                            Class1.AlarmMsg = "# 42 - Chamber Motor is running error";
                            MainAlarmTAobj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 42, "Chamber Motor is running error", "Chamber Motor is running error.");
                            Class1.FatalError = true;
                            break;

                    }
                    LastAlarm = Class1.SendAlarm;
                }
            }
            catch
            { //MessageBox.Show("Downstream is busy why initializining the Cycle."); 
            }
        }

        private object RaiseAlarm(int Alarm)
        {
            object functionReturnValue = null;
            if (Alarm == 0)
            {
                Class1.AlarmPause = false;
                return 0;
                return functionReturnValue;
            }
            if (IsHandleCreated)
            {
                AlarmBox.Invoke((MethodInvoker)delegate { AlarmBox.Text = ""; GroupBox5.Text = "Alarms"; });

            }


            Class1.AlarmPause = true;
            Class1.SendAlarm = Alarm;
            Thread.Sleep(10);
            AlarmHandler();
            //SetText(AlarmMsg)
            try
            {
                if (IsHandleCreated)
                {
                    // AlarmBox.Text = Class1.AlarmMsg;
                    AlarmBox.Invoke((MethodInvoker)delegate { AlarmBox.Text = Class1.AlarmMsg; });
                    Class2.WriteAlarmLog(Class1.AlarmMsg);

                }
            }
            catch (Exception)
            {

                throw;
            }


            try
            {
                if (!WirteLogThread.IsBusy)
                    WirteLogThread.RunWorkerAsync();
            }
            catch
            {
            }
            Class1.PumpOn = false;
            return 0;
            return functionReturnValue;
        }

        private void RaiseMessage(int Alarm)
        {

            Class1.SendAlarm = Alarm;
            AlarmHandler();
            Thread.Sleep(10);
            if (IsHandleCreated)
            { AlarmBox.Invoke((MethodInvoker)delegate { AlarmBox.Text = Class1.AlarmMsg; GroupBox5.Text = "Message"; }); }

        }
        private void ResetAlarm_Click(System.Object sender, System.EventArgs e)
        {

            if (Class1.SendAlarm == 31)
            {
                BothDoorsAutoTh = new Thread(new System.Threading.ThreadStart(OpenDoorsAutoTh));
                BothDoorsAutoTh.Start();
                while (BothDoorsAutoTh.IsAlive)
                { Application.DoEvents(); }
                Thread.Sleep(500);
                AlarmBox.Invoke((MethodInvoker)delegate { AlarmBox.Text = ""; });
            }

            if (Class1.SendAlarm == 37 | Class1.SendAlarm == 38)
            {
                if (Class1.ByPass == false)
                {
                    motorStopped = false;
                    MotorTick = 0;
                    Class1.RunStep = 1;
                }
            }
            //if (Class1.SendAlarm == 36 )
            //{
            //    if(Class1.ByPass==false)
            //    {
            //        if (Class1.DI_LeadFrameOut == 0)
            //        {
            //            motorStopped = false;
            //            MotorTick = 0;
            //            Class1.RunStep = 1;
            //            AlarmBox.Invoke((MethodInvoker)delegate { AlarmBox.Text = ""; });
            //            BackDrCloseAutoTh = new Thread(new System.Threading.ThreadStart(BackDoorsUpThAuto));
            //            BackDrCloseAutoTh.Start();
            //            while (BackDrCloseAutoTh.IsAlive)
            //            {
            //                Application.DoEvents();
            //            }
            //            CycleRunCont();
            //            Class2.Create10DOArray(6, 0); // close Purge Valve
            //        }
            //        else if(Class1.DI_LeadFrameOut==1)
            //         {
            //             RaiseMessage(36);
            //             return;
            //        }


            //    }
            //}

            AlarmResetExit();
        }

        private void AlarmResetExit()
        {
            Class1.AlarmPause = false;
            RaiseAlarm(0);
            Class1.SendAlarm = 0;
            AlarmBox.Text = "";
            Class1.FatalError = false;
            Class1.BackDoorNotUp = false;
            Class1.FrontDoorNotUp = false;

        }

        private void Clocktimer()
        {
            do
            {
                Thread.Sleep(1000);
                Class2.WriteEventLog(Class1.ConnectionFailCode, Class1.ConnectionFailDesc);
                Class2.LogEventDB(Class1.ConnectionFailCode, Class1.ConnectionFailDesc);
                if (ClkTimCount >= 10)
                {

                    if (Class1.PumpOn == true)
                    {
                        Class1.PumpTick += 1;
                    }
                    if (Class1.StartCounting == true)
                    {
                        Class1.ClockTick = Class1.ClockTick + 1;
                    }

                    MotorTick = MotorTick + 1;

                    if (Class1.DiDPumpDown == true & Class1.DiDPlasma == false)
                    {
                        Class1.DelayTick = Class1.DelayTick + 1;
                    }

                    if (Class1.DiDPumpDown == true & Class1.DiDTTP == false)
                    {
                        Class1.TTPTick = Class1.TTPTick - 1;

                        Label42.Invoke((MethodInvoker)delegate { Label42.Text = Class1.TTPTick.ToString(); Button2.Visible = true; });


                        if (Class1.DiDTTP == false)
                        {
                            if (Button3.Visible == true)
                            {
                                Button3.Invoke((MethodInvoker)delegate { Button3.Visible = false; });

                            }
                            else
                            {
                                Button3.Invoke((MethodInvoker)delegate { Button3.Visible = true; });

                            }
                        }
                    }

                    if (Class1.DiDTTP == true & Class1.DO_RFON == true)
                    {
                        //Label42.Text = "0";
                        //Class1.PlasmaTick = Class1.PlasmaTick - 1;
                        Button3.Invoke((MethodInvoker)delegate { Button3.Visible = true; });

                        if (Class1.DiDPlasma == false & Class1.DiDTTP == true)
                        {
                            if (Button4.Visible == true)
                            {
                                Button4.Invoke((MethodInvoker)delegate { Button4.Visible = false; });

                            }
                            else
                            {
                                Button4.Invoke((MethodInvoker)delegate { Button4.Visible = true; });

                            }
                        }
                    }


                    if (Class1.DO_VacuumON == false)
                    {
                        if (Button1.Visible == true)
                        {
                            Button1.Invoke((MethodInvoker)delegate { Button1.Visible = false; });

                        }
                        else
                        {
                            Button1.Invoke((MethodInvoker)delegate { Button1.Visible = true; });

                        }
                    }

                    Button7.Invoke((MethodInvoker)delegate { Button7.Visible = false; });

                    Label24.Invoke((MethodInvoker)delegate { Label24.Text = Class1.ClockTick.ToString(); });

                    if (Class1.DO_VacuumON == true & Class1.DiDPumpDown == false)
                    {
                        Class1.PDTick = Class1.PDTick + 1;
                        Label40.Invoke((MethodInvoker)delegate { Label40.Text = Class1.PDTick.ToString(); Button1.Visible = true; });

                        if (Class1.DiDPumpDown == false)
                        {
                            if (Button2.Visible == true)
                            {
                                Button2.Invoke((MethodInvoker)delegate { Button2.Visible = false; });

                            }
                            else
                            {
                                Button2.Invoke((MethodInvoker)delegate { Button2.Visible = true; });

                            }
                        }
                    }




                    if (Class1.StopClick == true | Class1.DI_EStop == 0)
                    {
                        Class1.VentTick = Class1.VentTick - 1;
                        // Button4.Invoke((MethodInvoker)delegate { Button4.Visible = true; Button1.Visible = true; });


                        //if (Button5.Visible == true)
                        //{
                        //    Button5.Invoke((MethodInvoker)delegate { Button5.Visible = false; });

                        //}
                        //else
                        //{
                        //    Button5.Invoke((MethodInvoker)delegate { Button5.Visible = true; });

                        //}

                        Class1.Venting = true;
                        if (Class1.VentTick <= 0 & Class1.DoneOnce == false)
                        {
                            Label44.Invoke((MethodInvoker)delegate { Label44.Text = "0"; });

                            Button1.Invoke((MethodInvoker)delegate { Button1.Visible = false; Button2.Visible = false; Button3.Visible = false; Button4.Visible = false; Button5.Visible = false; Button7.Visible = true; });

                            long x = 0;
                            long y = 0;
                            x = Convert.ToInt32(CyclesLbl.Text) + 1;
                            MainstartupTAobj.UpdateCycles(x);
                            CyclesLbl.Invoke((MethodInvoker)delegate { CyclesLbl.Text = x.ToString(); });

                            string str = null;
                            str = Shift.Text;
                            str = str.Substring(14);
                            y = Convert.ToInt32(str);
                            y = (y + 1);
                            MainstartupTAobj.UpdateNewShiftCycles(y);
                            Shift.Invoke((MethodInvoker)delegate { Shift.Text = "Shift Cycles:" + Environment.NewLine + y.ToString(); Shift.Enabled = true; });
                            //Startbtn.Invoke((MethodInvoker)delegate { Startbtn.Enabled = true; Modebtn.Enabled = true; Startbtn.BackColor = Color.Green; Modebtn.BackColor = Color.CornflowerBlue; });
                            Class1.VentTick = 0;
                            this.Invalidate();
                            Class1.DoneOnce = true;
                        }
                        else
                        {
                            //Label44.Invoke((MethodInvoker)delegate { Label44.Text = Class1.VentTick.ToString(); });


                        }
                    }
                    if (Class1.DiDPlasma == true | Class1.StopClick == true)
                    {
                        Class1.VentTick = Class1.VentTick - 1;
                        Button4.Invoke((MethodInvoker)delegate { Button4.Visible = false; Button1.Visible = true; });
                        Button1.Invoke((MethodInvoker)delegate { Button1.Visible = false; Button2.Visible = false; Button3.Visible = false; Button4.Visible = false; Button5.Visible = false; });


                        //if (Button5.Visible == true)
                        //{
                        //    Button5.Invoke((MethodInvoker)delegate { Button5.Visible = false; });

                        //}
                        //else
                        //{
                        //    Button5.Invoke((MethodInvoker)delegate { Button5.Visible = true; });

                        //}
                        Class1.Venting = true;

                        if (Class1.VentTick <= 0 & Class1.DoneOnce == false) // Reset Cycle Buttons and Vent Label, Update DB with Cycles and Shift Cycles and set button back color
                        {
                            Label44.Invoke((MethodInvoker)delegate { Label44.Text = "0"; });
                            Button1.Invoke((MethodInvoker)delegate { Button1.Visible = false; Button2.Visible = false; Button3.Visible = false; Button4.Visible = false; Button5.Visible = false; });

                            long x = 0;
                            long y = 0;
                            x = Convert.ToInt32(CyclesLbl.Text) + 1;
                            MainstartupTAobj.UpdateCycles(x);
                            CyclesLbl.Invoke((MethodInvoker)delegate { CyclesLbl.Text = x.ToString(); });

                            string str = null;
                            str = Shift.Text;
                            str = str.Substring(14);
                            y = Convert.ToInt32(str);
                            y = (y + 1);
                            MainstartupTAobj.UpdateNewShiftCycles(y);
                            Shift.Invoke((MethodInvoker)delegate { Shift.Text = "Shift Cycles:" + Environment.NewLine + y.ToString(); Shift.Enabled = true; });
                            //Startbtn.Invoke((MethodInvoker)delegate { Startbtn.BackColor = Color.Green; Modebtn.BackColor = Color.CornflowerBlue; });
                            this.Invalidate();
                            Class1.DoneOnce = true;
                        }
                        else
                        {
                            //Label44.Invoke((MethodInvoker)delegate { Label44.Text = Class1.VentTick.ToString(); });
                        }
                    }
                    if (Class1.StopClick == true)
                    {
                        Button1.Invoke((MethodInvoker)delegate { Button1.Visible = false; Button2.Visible = false; Button3.Visible = false; Button4.Visible = false; Button5.Visible = false; Button7.Visible = true; });
                    }
                    ClkTimCount = 0;
                }

                ClkTimCount += 1;

                if (Class1.DiDTTP == true & Class1.DO_RFON == true)
                {
                    Label42.Text = "0";
                    Class1.PlasmaTick = Class1.PlasmaTick - 1;
                }

                if (Class1.VentTick <= 0 & Class1.RunStep < 12)
                {
                    Class1.Venting = false;
                }



                if (Class1.Venting == true)
                {
                    //Button5.Invoke((MethodInvoker)delegate { Button5.Visible = false; });
                    Thread.Sleep(400);
                    if (Button5.Visible == true)
                    {
                        Button5.Invoke((MethodInvoker)delegate { Button5.Visible = false; });

                    }
                    else
                    {
                        Button5.Invoke((MethodInvoker)delegate { Button5.Visible = true; });

                    }
                }



            } while (true);
        }


        private void TestThread()
        {
            double RealGas1 = 0;
            double RealGas2 = 0;
            double RealGas3 = 0;
            double RealRFPWR = 0;
            double RealRFREV = 0;
            double RealBias = 0;
            double RealTune = 0;
            double RealLoad = 0;
            double RealGAS1PS = 0;
            double RealGAS2PS = 0;
            double RealGAS3PS = 0;
            double RealPurge = 0;

            //need to check
            /*----------------------------------------*/

            //Class1.AlarmPause = false;

            /*----------------------------------------*/
            do
            {
                //---------------------Machine Simulation Code------------------------

                if (Class1.DO_ChamberUP == false)
                    ChamUpSen.BackColor = Color.Red;
                else
                    ChamUpSen.BackColor = Color.Green;

                if (Class1.DO_ChamberDown == true)
                    ChamDnSen.BackColor = Color.Green;
                else
                    ChamDnSen.BackColor = Color.Red;

                if (Class1.DO_PumpON == false)
                    PumpOnOff.BackColor = Color.Red;
                else
                    PumpOnOff.BackColor = Color.Green;

                if (Class1.DO_VacuumON == true)
                    VacuumValSen.BackColor = Color.Green;
                else
                    VacuumValSen.BackColor = Color.Red;


                if (Class1.DO_PressureON == true)
                    PressureValSen.BackColor = Color.Green;
                else
                    PressureValSen.BackColor = Color.Red;

                if (Class1.DO_TrolleyClampLeftLock == true)
                    TrolleyLL.BackColor = Color.Green;
                else
                    TrolleyLL.BackColor = Color.Red;

                if (Class1.DO_TrolleyClampRightLock == true)
                    TrolleyRL.BackColor = Color.Green;
                else
                    TrolleyRL.BackColor = Color.Red;

                if (Class1.DO_VentON == true)
                    VentValSen.BackColor = Color.Green;
                else
                    VentValSen.BackColor = Color.Red;

                if (Class1.DI_TrolClampRightLock == 1)
                    TrolleyRL.BackColor = Color.Green;
                else
                    TrolleyRL.BackColor = Color.Red;

                if (Class1.DI_TrolClampLeftLock == 1)
                    TrolleyLL.BackColor = Color.Green;
                else
                    TrolleyLL.BackColor = Color.Red;
                if (Class1.DI_TrayPosSnrRight == 1)
                    TrayPosSenR.BackColor = Color.Green;
                else
                    TrayPosSenR.BackColor = Color.Red;

                if (Class1.DI_TrayPosSnrLeft == 1)
                    TrayPosSenL.BackColor = Color.Green;
                else
                    TrayPosSenL.BackColor = Color.Red;

                if (Class1.DI_TravellerHome == 1)
                    TravelHomeSen.BackColor = Color.Green;
                else
                    TravelHomeSen.BackColor = Color.Red;

                if (Class1.DI_TravellerCenter == 1)
                    TravelCenterSen.BackColor = Color.Green;
                else
                    TravelCenterSen.BackColor = Color.Red;

                if (Class1.DI_TravellerRemote == 1)
                    TravelRemoteSen.BackColor = Color.Green;
                else
                    TravelRemoteSen.BackColor = Color.Red;

                if (Class1.DI_TravellerEndSnr == 1)
                    TravelEndSen.BackColor = Color.Green;
                else
                    TravelEndSen.BackColor = Color.Red;

                if (Class1.DI_TravellerClearSnr == 1)
                    TravelClearSen.BackColor = Color.Green;
                else
                    TravelClearSen.BackColor = Color.Red;

                if (Class1.DI_PumpOK == 1)
                    PumpOK.BackColor = Color.Green;
                else
                    PumpOK.BackColor = Color.Red;

                if (Class1.DO_Gas1ON == true)
                {
                    GasVal1Sen.BackColor = Color.Green;
                    
                }
                    
                else
                {
                     GasVal1Sen.BackColor = Color.Red;
                    
                }
                   

                if (Class1.DO_Gas2ON == true)
                {
                    GasVal2Sen.BackColor = Color.Green;
                  
                }
                    
                else
                {
                 GasVal2Sen.BackColor = Color.Red;
                
                }
                   
//-------------------------------------------------------------------------------------------------------

                    if (Stopbtn.Visible == false & Startbtn.Visible == false)
                    {
                        Panic.Invoke((MethodInvoker)delegate { Panic.Visible = true; });
                    }
                    else
                    {
                        Panic.Invoke((MethodInvoker)delegate { Panic.Visible = false; });
                    }
                MTHRD.Invoke((MethodInvoker)delegate { MTHRD.Visible = true; });

                if (CycleRun.IsBusy == true)
                {
                    RTHRD.Invoke((MethodInvoker)delegate { RTHRD.Visible = true; Modebtn.Visible = false; Startbtn.Visible = false; Stopbtn.Visible = true; });
                }
                else
                {
                    RTHRD.Invoke((MethodInvoker)delegate { RTHRD.Visible = false; Modebtn.Visible = true; Startbtn.Visible = true; Stopbtn.Visible = false; });
                    Panic.Invoke((MethodInvoker)delegate { Panic.Visible = false; });
                }
                if (ClocktimerTh != null)
                {
                    if (ClocktimerTh.IsAlive == true)
                    {
                        CTHRD.Invoke((MethodInvoker)delegate { CTHRD.Visible = true; });
                    }
                    else
                    {
                        CTHRD.Invoke((MethodInvoker)delegate { CTHRD.Visible = false; Stopbtn.Visible = false; });
                    }
                }

                if (Class1.FatalError == true)
                {
                    FTHRD.Invoke((MethodInvoker)delegate { FTHRD.Visible = true; });
                }
                else
                {
                    FTHRD.Invoke((MethodInvoker)delegate { FTHRD.Visible = false; });
                }


                Thread.Sleep(100);
                //Read DI
                if (Class1.DI_Air == 0)
                {
                    //SetBackcolor(Color.Red);
                    AirSensor.BackColor = Color.Red;
                }
                else
                {
                    //SetBackcolor(Color.Green);
                    AirSensor.BackColor = Color.Green;
                }
                if (Class1.DI_EStop == 0)
                {
                    LBLEstop.Invoke((MethodInvoker)delegate { LBLEstop.Visible = true; });

                    AlarmBox.Invoke((MethodInvoker)delegate { AlarmBox.Text = Class1.AlarmMsg; GroupBox5.Text = "Alarms"; });

                }
                else
                {
                    LBLEstop.Invoke((MethodInvoker)delegate { LBLEstop.Visible = false; });


                }
                /*----AlarmBox text Font Color*/


                if (GroupBox5.Text == "Message")
                {
                    AlarmBox.Invoke((MethodInvoker)delegate { AlarmBox.ForeColor = Color.Green; });

                }
                else if (GroupBox5.Text == "Alarms")
                {
                    AlarmBox.Invoke((MethodInvoker)delegate { AlarmBox.ForeColor = Color.Red; });


                }

                if (Class1.DI_ChamberUP == 1)
                {
                    //ChamberClosed.Invoke((MethodInvoker)delegate { ChamberClosed.Visible = false; });

                }
                else
                {
                    // ChamberClosed.Invoke((MethodInvoker)delegate { ChamberClosed.Visible = true; });

                }

                //End Read DI

                //Read DO
                if (Class1.DO_PumpON == false)
                {
                    // picPump.Invoke((MethodInvoker)delegate { picPump.Visible = false;});

                }
                else
                {
                    //picPump.Invoke((MethodInvoker)delegate { picPump.Visible = true; });

                }

                //if (Class1.DI_PumpOK == 0)
                //{
                //    PumpOnOff.Invoke((MethodInvoker)delegate { PumpOnOff.BackColor = Color.Red; });

                //}
                //else
                //{
                //    PumpOnOff.Invoke((MethodInvoker)delegate { PumpOnOff.BackColor = Color.Green; });

                //}
                if (Class1.DO_VacuumON == false)
                {
                    // VacValve.Invoke((MethodInvoker)delegate { VacValve.BackColor = Color.Black; VacOff.Visible = true; VacOn.Visible = false; });
                }
                else
                {
                    // VacValve.Invoke((MethodInvoker)delegate { VacValve.BackColor = Color.Green; VacOff.Visible = false; VacOn.Visible = true; });
                }
                if (Class1.DO_PressureON == false)
                {
                    // Press1.Invoke((MethodInvoker)delegate{Press1.BackColor = Color.Black;XLAOFF.Visible = true;XLAON.Visible = false;});
                }
                else
                {
                    //  Press1.Invoke((MethodInvoker)delegate { Press1.BackColor = Color.Green; XLAOFF.Visible = false; XLAON.Visible = true; });
                }
                if (Class1.DO_VentON == false)
                {
                    //Size value = new Size(101, 25);
                    // Button10.Invoke((MethodInvoker)delegate {  PurgeOFF.Visible = true; PurgeON.Visible = false; Button9.Visible = true; Button14.Visible = true; Button9.BackColor = Color.Black; Button14.BackColor = Color.Black; /*Button9.Size = value; */});
                }
                else
                {
                    //Size value = new Size(308, 25);
                    // Button10.Invoke((MethodInvoker)delegate {  PurgeOFF.Visible = false; PurgeON.Visible = true; Button9.Visible = true; Button14.Visible = true; Button9.BackColor = Color.Green; Button14.BackColor = Color.Green; /*Button9.Size = value;*/ });
                }
                if (Class1.DO_StatusMainpurge == false)
                {
                    // Button10.BackColor = Color.Black;
                    //MainPurgeOFF.Visible = true;
                    //MainPurgeON.Visible = false;
                }
                else
                {
                    // Button10.BackColor = Color.Green;
                    //MainPurgeOFF.Visible = false;
                    //MainPurgeON.Visible = true;
                }
                if (Class1.DO_Gas1ON == false)
                {
                    //  Button10.BackColor = Color.Black;
                    G1T.Invoke((MethodInvoker)delegate { G1T.Visible = true; G1XSAOff.Visible = true; G1XSAOn.Visible = false; });
                }
                else
                {
                    // Button10.BackColor = Color.Green;
                    G1T.Invoke((MethodInvoker)delegate { G1T.Visible = true; G1XSAOff.Visible = false; G1XSAOn.Visible = true; });
                }
                if (Class1.DO_Gas2ON == false)
                {
                    //GasVal1.BackColor = Color.Black;
                    G2T.Invoke((MethodInvoker)delegate { G2T.Visible = true; G2XSAOff.Visible = true; G2XSAOn.Visible = false; });
                }
                else
                {
                  //  GasVal1.BackColor = Color.Green;
                    G2T.Invoke((MethodInvoker)delegate { G2T.Visible = true; G2XSAOff.Visible = false; G2XSAOn.Visible = true; });
                }
                ////if (Class1.DO_StatusGAS3 == false)
                ////{
                ////    button13.BackColor = Color.Black;
                ////    G3T.Invoke((MethodInvoker)delegate {G3T.Visible = true;G3XSAOff.Visible = true;G3XSAOn.Visible = false;});
                //// }
                ////else
                ////{
                ////    button13.BackColor = Color.Green;
                ////    G3T.Invoke((MethodInvoker)delegate {G3T.Visible = true;G3XSAOff.Visible = false; G3XSAOn.Visible = true; });
                ////}
                //if (Class1.Gas3 == false)
                //{
                //    G3T.Invoke((MethodInvoker)delegate { G3T.Visible = false;G3XSAOff.Visible = false; G3XSAOn.Visible = false; });
                //  }


                if (Class1.DO_RFON == false)
                {
                    // RF1.Invoke((MethodInvoker)delegate { RF1.BackColor = Color.Black; RF2.BackColor = Color.Black; RFONpic.Visible = false; RFOFFpic.Visible = true; Plasma.Visible = false; });
                }
                else
                {
                    //  RF1.Invoke((MethodInvoker)delegate { RF1.BackColor = Color.Red; RF2.BackColor = Color.Red; Plasma.Visible = true; RFOFFpic.Visible = false; RFONpic.Visible = true; });
                }
                if (Class1.DO_ManualTuner == false)
                {
                    Button6.Invoke((MethodInvoker)delegate { Button6.Visible = false; Button11.Visible = true; });
                }
                else
                {
                    Button6.Invoke((MethodInvoker)delegate { Button6.Visible = true; Button11.Visible = false; });
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
                //RealPressure = Math.Pow(10, (Class1.AI_PressureValue - 6)); // using AGP100 Pirani
                //RealPressure = RealPressure + 0.02;

                RealPressure = Convert.ToDouble(Class1.AI_PressureValue / 10); // using Inficon CGD025D

                if (RealPressure < 100)
                {
                    PressureLbl.Invoke((MethodInvoker)delegate { PressureLbl.Text = Math.Round(RealPressure, 3).ToString(); });
                }
                else
                {
                    PressureLbl.Invoke((MethodInvoker)delegate { PressureLbl.Text = PressureLbl.Text = "Over Range"; });

                }
                //End Calculate the Pressure in mbar

                //Calculate Gas1
                RealGas1 = ((Class1.Gas1R / 10) * Class1.AI_GAS1Value);
                //* GCF1
                Label11.Invoke((MethodInvoker)delegate { Label11.Text = Math.Round(RealGas1, 2).ToString(); });
                if (Class1.DO_Gas1ON == false)
                    Label11.Invoke((MethodInvoker)delegate { Label11.Text = "0"; });

                //End Calculate Gas1

                //Calculate Gas2
                RealGas2 = Class1.Gas2R / 10 * Class1.AI_GAS2Value;
                //* GCF2

                Label9.Invoke((MethodInvoker)delegate { Label9.Text = Math.Round(RealGas2, 2).ToString(); });
                if (Class1.DO_Gas2ON == false)
                    Label9.Invoke((MethodInvoker)delegate { Label9.Text = "0"; });

                //End Calculate Gas2

                //Calculate Gas3
                RealGas3 = Class1.Gas3R / 10 * Class1.AI_GAS3Value;
                //* GCF3
                //Label10.Invoke((MethodInvoker)delegate {  Label10.Text = Math.Round(RealGas3, 2).ToString();});

                //if (Class1.DO_StatusGAS3 == false)
                //    Label10.Invoke((MethodInvoker)delegate { Label10.Text = "0"; });

                ////End Calculate Gas3

                ////Calculate AI_ARFPowerValue
                //RealRFPWR = Class1.SetupRFRange / 10 * Class1.AI_ARFPowerValue;
                //Label6.Invoke((MethodInvoker)delegate {  Label6.Text = Math.Round(RealRFPWR, 2).ToString();});

                if (Class1.DO_RFON == false)
                    Label6.Invoke((MethodInvoker)delegate { Label6.Text = "0"; });

                //End Calculate AI_ARFPowerValue

                //Calculate AI_RFRefelctedValue
                RealRFREV = Class1.SetupRFRange / 10 / 3 * Class1.AI_RFRefelctedValue;
                Label7.Invoke((MethodInvoker)delegate { Label7.Text = Math.Round(RealRFREV, 2).ToString(); });

                if (Class1.DO_RFON == false)
                    Label7.Invoke((MethodInvoker)delegate { Label7.Text = "0"; });

                //End Calculate AI_RFRefelctedValue

                //Calculate AI_BiasValue
                RealBias = 100 * Class1.AI_BiasValue;
                Label27.Invoke((MethodInvoker)delegate { Label27.Text = Math.Round(RealBias, 2).ToString(); });

                if (Class1.DO_RFON == false)
                    Label27.Invoke((MethodInvoker)delegate { Label27.Text = "0"; });

                //End Calculate AI_BiasValue

                //Calculate AI_TuneValue
                RealTune = 10 * Class1.AI_TuneValue;
                Label2.Invoke((MethodInvoker)delegate { Label2.Text = Math.Round(RealTune, 2).ToString(); });

                //End Calculate AI_TuneValue

                //Calculate AI_LoadValue
                RealLoad = 10 * Class1.AI_LoadValue;
                Label8.Invoke((MethodInvoker)delegate { Label8.Text = Math.Round(RealLoad, 2).ToString(); });

                //End Calculate AI_LoadValue

                //Calculate AI_GAS1PSValue
                RealGAS1PS = (Class1.AI_GAS1PSValue - 1) / 0.4;
                Label19.Invoke((MethodInvoker)delegate { Label19.Text = Math.Round(RealGAS1PS, 2).ToString(); });

                if (RealGAS1PS < 0)
                    Label19.Invoke((MethodInvoker)delegate { Label19.Text = "0.0"; });

                //End Calculate AI_GAS1PSValue

                //Calculate AI_GAS2PSValue
                RealGAS2PS = (Class1.AI_GAS2PSValue - 1) / 0.4;
                Label15.Invoke((MethodInvoker)delegate { Label15.Text = Math.Round(RealGAS2PS, 2).ToString(); });

                if (RealGAS2PS < 0)
                    Label15.Invoke((MethodInvoker)delegate { Label15.Text = "0.0"; });

                //End Calculate AI_GAS2PSValue

                //Calculate AI_GAS3PSValue
                //RealGAS3PS = (Class1.AI_GAS3PSValue - 1) / 0.4;
                ////Label17.Invoke((MethodInvoker)delegate { Label17.Text = Math.Round(RealGAS3PS, 2).ToString(); });

                //if (RealGAS3PS < 0)
                  //  Label17.Invoke((MethodInvoker)delegate { Label17.Text = "0.0"; });

                //End Calculate AI_GAS3PSValue

                //Calculate AI_PURGEPSValue
                RealPurge = (Class1.AI_PURGEPSValue - 1) / 0.4;
              //  Label22.Invoke((MethodInvoker)delegate { Label22.Text = Math.Round(RealPurge, 2).ToString(); });
                if (RealPurge < 0)
                //    Label22.Invoke((MethodInvoker)delegate { Label22.Text = "0.0"; });


                //End Calculate AI_PURGEPSValue

                //End Read AI

                //Activate RF Countdown
                RFTimeBar.Invoke((MethodInvoker)delegate { RFTimeBar.Maximum = MaxRFT; RFTimeBar.Minimum = 0; });
                //(int)Class1.R_RFT;


                if (Class1.CycleStart == true)

                    if (Class1.RunStep == 2)
                    {

                        try
                        {
                            Class1.RFTick = Class1.PlasmaTick;
                            RFTimeBar.Invoke((MethodInvoker)delegate { RFTimeBar.Value = Class1.RFTick; });
                            double Buf1 = Convert.ToDouble(Class1.RFTick);
                            double SRFTick = Buf1 / 10;
                            LRFTime.Invoke((MethodInvoker)delegate { LRFTime.Text = string.Format("{0:n1}", SRFTick); });

                        }
                        catch
                        {
                        }
                    }

                {
                    if (Class1.DO_RFON == true)
                    {
                        Class1.RFTick = Class1.PlasmaTick;
                        if (Class1.RFTick <= 0)
                            Class1.RFTick = 0;
                        double Buf1 = Convert.ToDouble(Class1.RFTick);
                        double SRFTick = Buf1 / 10;
                        LRFTime.Invoke((MethodInvoker)delegate { LRFTime.Text = string.Format("{0:n1}", SRFTick); });

                        try
                        {
                            RFTimeBar.Invoke((MethodInvoker)delegate { RFTimeBar.Value = Class1.RFTick; });

                        }
                        catch
                        {
                        }
                        //}else
                        //{
                        //    //Class1.RFTick =(int)Class1.R_RFT;
                        //    //RFTimeBar.Invoke((MethodInvoker)delegate { RFTimeBar.Value = Class1.RFTick; });

                        //    RFTimeBar.Invoke((MethodInvoker)delegate { RFTimeBar.Value = 0; });
                    }


                }
                //End Activate RF Countdown
                if (Class1.AlarmActive == true)
                {
                    if (AlarmThread.IsBusy == false)
                        AlarmThread.RunWorkerAsync();
                }
                //------------------------

                //--------------------------------
                //Tower lights
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

                    int[] DIOChannelArr = { Class1.DO27, Class1.DO26, Class1.DO28, Class1.DO29 }; //Turn off yellow light,Turn off green light,Turn on red light,Turn on Buzzer
                    bool[] DIOStateArr = { false, false, true, true };
                    Class2.SetMultiDIO(DIOChannelArr, DIOStateArr);
                }

                if (Class1.AlarmPause == false & Class1.TLDid2 == false)
                {
                    Class1.TLDid1 = false;
                    Class1.TLDid2 = true;
                    // Class2.Create10DOArray(27, 0, 30, 0, 28, 1);


                    int[] DOChannelArr = { Class1.DO28, Class1.DO29, Class1.DO27 }; //Turn off red light,Turn off Buzzer,Turn on yellow light
                    bool[] DOStateArr = { false, false, true };
                    Class2.SetMultiDIO(DOChannelArr, DOStateArr);

                    Class1.HoldBuzzer = false;
                }

                if (Class1.CycleStart == true & CycleS == false & Class1.AlarmPause == false)
                {


                    int[] DOChannelArr = { Class1.DO28, Class1.DO27, Class1.DO26 }; //Turn off red light,Turn off yellow light,Turn on green light
                    bool[] DOStateArr = { false, false, true };
                    Class2.SetMultiDIO(DOChannelArr, DOStateArr);


                    //Class2.SetDO(Class1.DIOSlotNum, Class1.DO28, false);
                    //Class2.SetDO(Class1.DIOSlotNum, Class1.DO27, false);
                    //Class2.SetDO(Class1.DIOSlotNum, Class1.DO26, true);

                    CycleS = true;
                }
                if (PlasmaDone == true & Class1.AlarmPause == false)
                {

                    int[] DIOChannelArr = { Class1.DO27, Class1.DO26 }; //Turn on yellow light,Turn off green light
                    bool[] DIOStateArr = { true, false };
                    Class2.SetMultiDIO(DIOChannelArr, DIOStateArr);

                    //Class2.SetDO(Class1.DIOSlotNum, Class1.DO27, true);
                    //Class2.SetDO(Class1.DIOSlotNum, Class1.DO26, false);
                    PlasmaDone = false;

                }
                if (Class1.AlarmPause == false & Class1.StopClick == true & Class1.CycleStart == true)
                {
                    //Class2.Create10DOArray(28, 1, 29, 0);
                    //Class2.SetDO(Class1.DIOSlotNum, Class1.DO27, true);
                    //Class2.SetDO(Class1.DIOSlotNum, Class1.DO26, false);


                    int[] DIOChannelArr = { Class1.DO27, Class1.DO26 }; //Turn off green light,Turn on yellow light
                    bool[] DIOStateArr = { true, false };
                    Class2.SetMultiDIO(DIOChannelArr, DIOStateArr);
                }


                //End tower lights

                //*****************************************

                //Alarms

                //*****************************************

                //No Air
                if (Class1.DI_Air == 0)
                {
                    if (Class1.AlarmPause == false)
                        RaiseAlarm(1);
                }

                //if (Class1.DI_LeadFrameOut == 1)
                // {
                //     if (Class1.AlarmPause == false)
                //     RaiseMessage(36);
                // }
                //if (Class1.DI_LeadFrameOut == 0)
                //{
                //    AlarmBox.Invoke((MethodInvoker)delegate{if(AlarmBox.Text=="# 36 - LeadFrame cannot enter the Output Conveyor");});
                //    AlarmBox.Invoke((MethodInvoker)delegate { if (AlarmBox.Text == "");});    
                //}
                //Pump Health Bit Down
                //if (Class1.DO_StatusPumpON == true & Class1.DI_PumpOK == 0 & Class1.ClockTick > 5)
                //{
                //    if (Class1.AlarmPause == false)
                //        RaiseAlarm(2);
                //}
                //if(LeadFrameInFlg==true)
                //{ 
                //if (Class1.DI_LeadFrameOut == 1)
                //{
                //    if (Class1.SendAlarm != 36)
                //    {
                //        LeadFrameOutFlg = true;
                //        LeadFrameInFlg = false;
                //    }
                //}
                //}
                if (Class1.SendAlarm == 33)
                {
                    AlarmBox.Invoke((MethodInvoker)delegate { AlarmBox.Text = "# 33 - LeadFrame in Chamber Error"; GroupBox5.Text = "Alarms"; });
                }
                //No Gas1 Pressure Sensor
                if (Class1.R_G1 > 0)
                {
                    if (Class1.AI_GAS1PSValue < 1.04)
                    {
                        if (Class1.AlarmPause == false)
                            RaiseAlarm(5);
                    }
                }
                //No Gas2 Pressure Sensor
                if (Class1.R_G2 > 0)
                {
                    if (Class1.AI_GAS2PSValue < 1.04)
                    {
                        if (Class1.AlarmPause == false)
                            RaiseAlarm(6);
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
                                RaiseAlarm(7);
                        }
                    }
                }

                //No Purge Gas Pressure sensor
                if (Class1.AI_PURGEPSValue < 1.04)
                {
                    if (Class1.AlarmPause == false)
                        RaiseAlarm(15);
                }

                if (Class1.Intlk == false & Class1.DO_RFON == true)
                {
                    if (Class1.AlarmPause == false)
                        RaiseAlarm(17);
                }


                if (Class1.CycleStart == true)
                {
                    //Pumpdown Error
                    if (Class1.DiDPumpDown == false)
                    {
                        if (Class1.PDTick > Class1.PunpDwnTime)
                        {
                            if (Class1.AlarmPause == false)
                                RaiseAlarm(8);
                        }
                    }

                    //EStop
                    if (Class1.DI_EStop == 0)
                    {
                        if (Class1.AlarmPause == false)
                            RaiseAlarm(10);
                    }

                    //Pressure Tigger Error
                    if (Class1.CycleStart == true)
                    {
                        if (Class1.DiDPumpDown == true)
                        {
                            if (Class1.DO_Gas1ON == true | Class1.DO_Gas2ON == true)
                            {
                                if (Class1.TriggerError == true)
                                {
                                    if (Class1.AlarmPause == false)
                                        RaiseAlarm(14);
                                }
                            }
                        }
                    }

                    //MFC1

                    if (Class1.DiDPumpDown == true & Class1.DiDPlasma == false)
                    {
                        if (Class1.DelayTick > 2 & Class1.StopClick == false)
                        {
                            Class1.DelayTick = 0;
                            if (Class1.R_G1 > 0)
                            {
                                if (Class1.DO_Gas1ON == false | Class1.AI_GAS1Value <= 0.0)
                                {
                                    if (Class1.AlarmPause == false)
                                        RaiseAlarm(11);
                                }
                            }

                            //MFC2

                            if (Class1.DelayTick > 2)
                            {
                                if (Class1.R_G2 > 0)
                                {
                                    if (Class1.DO_Gas2ON == false | Class1.AI_GAS2Value <= 0.0)
                                    {
                                        if (Class1.AlarmPause == false)
                                            RaiseAlarm(12);
                                    }
                                }
                            }

                            //MFC3

                            //if (Class1.Gas3 == true)
                            //{
                            //    if (Class1.DelayTick > 2)
                            //    {
                            //        if (Class1.R_G3 > 0)
                            //        {
                            //            if (Class1.DO_StatusGAS3 == false | Class1.AI_GAS3Value <= 0.0)
                            //            {
                            //                if (Class1.AlarmPause == false)
                            //                    RaiseAlarm(13);
                            //            }
                            //        }
                            //    }
                            //}

                        }

                        //No Plasma  and  High Reflected Power
                        if (Class1.DO_RFON == true)
                        {
                            if (Class1.RFTick > 2 & Class1.StopClick == false)
                            {
                                if (Class1.RFTick < (Class1.R_RFT - 50))
                                {
                                    if (Class1.AI_BiasValue < 0)//changed to 0 from 0.1-------*****NEED TO CHECK*****-----------------
                                    {
                                        if (Class1.AlarmPause == false)
                                            RaiseAlarm(3);
                                    }
                                    if (Class1.DO_RFON == true & Class1.AI_RFRefelctedValue > 2.0)
                                    {
                                        if (Class1.AlarmPause == false)
                                            RaiseAlarm(4);
                                    }
                                }
                            }
                        }

                    }
                }

                if (Class1.StopClick == true & Class1.DO_RFON == true)
                {
                    if (Class1.AlarmPause == false)
                        RaiseAlarm(18);
                }

                //***************************************

                //End Alarms

                //***************************************



                //Check Setup for Gas3
                if (Class1.Gas3 == true)
                {
                    // Gas3Cover.Invoke((MethodInvoker)delegate { Gas3Cover.Visible = false; Label10.Visible = true; Label12.Visible = true; Label17.Visible = true; G3T.Visible = true; G3XSAOff.Visible = true; G3XSAOn.Visible = true; Gas3set.Visible = true; Label45.Visible = true; });
                }
                else
                {
                    //  Gas3Cover.Invoke((MethodInvoker)delegate { Gas3Cover.Visible = true; Label10.Visible = false; Label12.Visible = false; Label17.Visible = false; G3T.Visible = false; G3XSAOff.Visible = false; G3XSAOn.Visible = false; Gas3set.Visible = false; Label45.Visible = false; });
                }
                //End Check Setup for Gas3

            } while (true);
            MTHRD.Invoke((MethodInvoker)delegate { MTHRD.Visible = false; });
        }

        private void Main_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

            Class1.R_ID = Label47.Text;

            Class1.R_PT = Convert.ToDouble(Label35.Text);

            Class1.R_TTP = Convert.ToDouble(Label33.Text);

            Class1.R_G1 = Convert.ToDouble(Label52.Text);

            Class1.R_G2 = Convert.ToDouble(Label37.Text);

          //  Class1.R_G3 = Convert.ToDouble(Label45.Text);

            Class1.R_PW = Convert.ToDouble(Label31.Text);

            Class1.R_RFT = Convert.ToDouble(Label29.Text);

            Class1.R_TP = Convert.ToDouble(Label51.Text);

            Class1.R_LP = Convert.ToDouble(Label49.Text);

        }




        private void Shift_Click(System.Object sender, System.EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reset the Shift Cycle Counter?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
            {
                MainstartupTAobj.UpdateNewShiftCycles(0);
                Shift.Text = "Shift Cycles:" + Environment.NewLine + MainstartupTAobj.SelectShiftCyclesfromStartup().ToString();
            }
        }


        private void BBuzzer_Click(object sender, EventArgs e)
        {
            Class1.HoldBuzzer = true;
            Class1.FatalError = false;
            //Class2.Create10DOArray(30, 0);
            Class2.SetDO(Class1.DIOSlotNum, Class1.DO29, true);
        }


        private void VentNow()
        {
            int j = Class1.Venttime;


            int[] DOChannelArr = { 0, 22 }; //Pressure Valve off,Purge Valve On ,Gas1 Off,Gass 2 Off,VacVAlve Off
            bool[] DOStateArr = { false, false };
            //Class2.SetMultiDO(DOChannelArr, DOStateArr);
            this.Invoke((MethodInvoker)delegate { Class2.SetMultiDO(DOChannelArr, DOStateArr); });


            int[] DIOChannelArr = { Class1.DO24, Class1.DO25 }; //Pressure Valve off,Purge Valve On ,Gas1 Off,Gass 2 Off,VacVAlve Off
            bool[] DIOStateArr = { false, false };
            //Class2.SetMultiDIO(DIOChannelArr, DIOStateArr);
            this.Invoke((MethodInvoker)delegate { Class2.SetMultiDIO(DIOChannelArr, DIOStateArr); });

            //Class2.SetDO(Class1.DOSlotNum, 0, false);//RF off
            //Class2.SetDO(Class1.DOSlotNum, 22, false);//Vacuum off
            //Class2.SetDO(Class1.DIOSlotNum, Class1.DO24, false);//Gas1 off
            //Class2.SetDO(Class1.DIOSlotNum, Class1.DO25, false);//Gas2 off

            Thread.Sleep(400);
            //Class2.Create10DOArray(6, 1); // open Purge Valve
            //Class2.SetDO(Class1.DOSlotNum, 21, true); // open Purge Valve
            this.Invoke((MethodInvoker)delegate { Class2.SetDO(Class1.DOSlotNum, 21, true); });
            do
            {
                Label44.Invoke((MethodInvoker)delegate { Label44.Text = j.ToString(); });
                if (j <= 0)
                {
                    //Open Doors
                    //Class2.Create10DOArray(13, 1);
                    BothDoorsAutoTh = new Thread(new System.Threading.ThreadStart(OpenDoorsAutoTh));
                    BothDoorsAutoTh.Start();
                    while (BothDoorsAutoTh.IsAlive)
                    { Application.DoEvents(); }
                    Thread.Sleep(500);
                    Class1.DiDPlasma = false;
                    Class1.DiDPumpDown = false;
                    Class1.DiDTTP = false;
                    Class1.StopClick = false;
                    k = 0;
                    Class1.VentTick = Class1.Venttime;
                    Class1.RunStep = 1;

                    if (Class1.ACBool == false)
                    {
                        PartOut();
                    }
                    else
                    {
                        Class1.StartCounting = false;
                        { label54.Invoke((MethodInvoker)delegate { label54.Text = (3600 / Class1.ClockTick).ToString(); }); }
                        Class1.RunStep = 1;
                    }

                    CycleRunCont();
                    //Class2.Create10DOArray(6, 0); // close Purge Valve
                    //Class2.SetDO(Class1.DOSlotNum, 21, true);  // close Purge Valve
                    this.Invoke((MethodInvoker)delegate { Class2.SetDO(Class1.DOSlotNum, 21, true); });
                    break;
                }
                j -= 1;
                Thread.Sleep(1000);

            } while (true);
        }

        //private void AVentNow()
        //{
        //    int j = Class1.Venttime;

        //    Class2.Create10DOArray(17, 0, 5,0, 7, 0, 10, 0, 11, 0,13,1);
        //    Thread.Sleep(400);
        //    Class2.Create10DOArray(6, 1); // open Purge Valve
        //    do
        //    {
        //        if (IsHandleCreated)
        //            {
        //                Label44.Invoke((MethodInvoker)delegate { Label44.Text = j.ToString(); });
        //            }


        //            if (j <=0)
        //            {
        //                Class1.DiDPlasma = false;
        //                Class1.DiDPumpDown = false;
        //                Class1.DiDTTP = false;
        //                Class1.StopClick = true;
        //                k = 0;
        //                Class1.VentTick = Class1.Venttime;
        //                //Class1.RunStep = 0;
        //                //Open Doors
        //                Thread.Sleep(800);


        //                BothDoorsAutoTh = new Thread(new System.Threading.ThreadStart(OpenDoorsAutoTh));
        //                BothDoorsAutoTh.Start();
        //                while (BothDoorsAutoTh.IsAlive)
        //                {
        //                    Application.DoEvents(); 
        //                }
        //                Thread.Sleep(500);

        //                PartOut();


        //                Class2.Create10DOArray(6, 0); // close Purge Valve
        //                Class2.Create10DOArray(20, 0);// Off Pump
        //                if (ClocktimerTh != null)
        //                    if (ClocktimerTh.IsAlive == true)
        //                    {
        //                        {
        //                            ClocktimerTh.Abort();
        //                        }
        //                    }
        //                Class1.CycleStart = false;
        //                CycleRun.CancelAsync();
        //                CycleRun.Dispose();
        //                break;

        //            }
        //            j -= 1; 
        //            Thread.Sleep(1000);      
        //    } while (true);
        //}

        //private void ByPassVent()
        //{

        //    Class1.DiDPlasma = false;
        //    Class1.DiDPumpDown = false;
        //    Class1.DiDTTP = false;
        //    Class1.StopClick = true;
        //    Class1.VentTick = Class1.Venttime;
        //    Thread.Sleep(100);
        //    BPPartOut();
        //    Thread.Sleep(100);
        //    Class2.Create10DOArray(20, 0);
        //    if (ClocktimerTh != null)
        //        if (ClocktimerTh.IsAlive == true)
        //        {
        //            {
        //                ClocktimerTh.Abort();
        //            }
        //        }
        //    Class1.CycleStart = false;
        //    CycleRun.CancelAsync();
        //    CycleRun.Dispose();
        //    Button1.Visible = false;

        //}
        private void Panic_Click(object sender, EventArgs e)
        {
            CycleRun.Dispose();
            ClocktimerTh.Abort();
            this.Close();
            this.Dispose();
            Mode objMode = new Mode();
            objMode.ShowDialog();
        }

        private void ACbtn_Click(object sender, EventArgs e)
        {
            if (Class1.ACBool == false)
            {
                Class1.ACBool = true;
                ACbtn.Text = "Auto Off";
            }
            else
            {
                Class1.ACBool = false;
                ACbtn.Text = "Auto On";
            }
        }

        private void BPbtn_Click(object sender, EventArgs e)
        {
            if (Class1.ByPass == false)
            {
                Class1.ByPass = true;
                //BPbtn.Text = "ByPass Off";
                StartAutoCycle();
            }
            else
            {
                Class1.ByPass = false;
                //BPbtn.Text = "ByPass On";
            }
        }

        private void BRed_Click(object sender, EventArgs e)
        {
            if (GhostTickAuto == 0)
            {
                GhostTickAuto = 1;
            }
            else if (GhostTickAuto == 3)
            {
                ACbtn.Visible = true;

            }
        }

        private void BYellow_Click(object sender, EventArgs e)
        {
            if (GhostTickAuto == 1)
            {
                GhostTickAuto = 2;
            }
        }

        private void BGreen_Click(object sender, EventArgs e)
        {
            if (GhostTickAuto == 2)
            {
                GhostTickAuto = 3;
            }
        }


       
        private void button12_Click(object sender, EventArgs e)
        {
             if (PicCount == 10 | PicCount == 0)
            {
                PicCount = 0;
                picRun(PicCount);
                PicCount = PicCount + 1;
            }
            else if (PicCount > 0)
            {
                picRun(PicCount);
                PicCount = PicCount + 1;
            }
        }

        private void picRun(int count)
        {
            switch (count)
            {
                case 0:
                   // TrayPosSenR.BackColor = Color.Red;
                    //TrayPosSenL.BackColor = Color.Red;
                    pictureBox2.Visible = true;
                    StepPictures.Visible = true;
                   
                    //GasVal2Sen.Location = new Point(77, 62);
                    int[] DOChannelArr = { 1, 12, 13,20,22,16,17,21 };
                    bool[] DOStateArr = { true, true, false,false,false,true,true,true };
                    Class2.SetMultiDO(DOChannelArr, DOStateArr);
                    int[] DIOChannelArr1 = { Class1.DO24,Class1.DO25 };
                    bool[] DIOStateArr1 = {  false, false };
                    Class2.SetMultiDIO(DIOChannelArr1, DIOStateArr1);
                    break;
                case 1:
                    
                    Class2.SetDO(Class1.DOSlotNum, 21, false);
                    StepPictures.Image = Properties.Resources.Step_1;
                    //TravelHomeSen.BackColor = Color.Red;
                    //TravelRemoteSen.BackColor = Color.Green;
                    GasVal1Sen.Location = new Point(452, 62);
                    GasVal2Sen.Location = new Point(571, 62);
                    TravelEndSen.Location = new Point(609, 276);
                    TravelClearSen.Location = new Point(402, 276);
                    
                    break;
                case 2:
                    StepPictures.Image = Properties.Resources.Step_2;
                   
                    TravelEndSen.Location = new Point(609, 266);
                    TravelClearSen.Location = new Point(404, 266);
                    break;
                case 3:
                    StepPictures.Image = Properties.Resources.Step_3;
                    
                    break;
                case 4:
                    StepPictures.Image = Properties.Resources.Step_4;
                    break;
                case 5:
                    StepPictures.Image = Properties.Resources.Step_5;
                    break;
                case 6:
                    StepPictures.Image = Properties.Resources.Step_6;
                    break;
                case 7:
                    StepPictures.Image = Properties.Resources.Step_7;
                    TravelEndSen.Location = new Point(609, 276);
                    TravelClearSen.Location = new Point(404, 276);
                    break;
                case 8:
                    StepPictures.Image = Properties.Resources.Step_8;
                    //TravelHomeSen.BackColor = Color.Green;
                    //TravelRemoteSen.BackColor = Color.Red;
                    //TrayPosSenR.BackColor = Color.Green;
                    //TrayPosSenL.BackColor = Color.Green;
                    pictureBox2.Image = Properties.Resources.Trolley_Gray;
                     int[] DOChannelArr2 = { 16,17 };
                    bool[] DOStateArr2 = { false,false };
                    Class2.SetMultiDO(DOChannelArr2, DOStateArr2);
                    TravelEndSen.Location = new Point(909, 276);
                    TravelClearSen.Location = new Point(696, 276);
                    break;
                case 9:
                    StepPictures.Image = Properties.Resources.Step_9;
                   
                   // GasVal2Sen.Location = new Point(77, 98);
                    int[] DOChannelArr1 = { 12, 13,20,22, };
                    bool[] DOStateArr1 = { false, true, true, true };
                    Class2.SetMultiDO(DOChannelArr1, DOStateArr1);
                    int[] DIOChannelArr = { Class1.DO24,Class1.DO25 };
                    bool[] DIOStateArr = {  true, true };
                    Class2.SetMultiDIO(DIOChannelArr, DIOStateArr);
                    //TravelHomeSen.BackColor = Color.Green;
                    GasVal1Sen.Location = new Point(452, 151);
                    GasVal2Sen.Location = new Point(571, 151);
                    
                    break;

                default:
                    break;
            }
        }

        private void DGVLogList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtLogEventLog.Text = DGVLogList.Rows[e.RowIndex].Cells[0].Value.ToString() + " " + DGVLogList.Rows[e.RowIndex].Cells[1].Value.ToString() + " " + DGVLogList.Rows[e.RowIndex].Cells[2].Value.ToString();
        }
    }

}

