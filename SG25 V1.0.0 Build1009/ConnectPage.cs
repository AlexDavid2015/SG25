using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using APS_Define_W32;
using APS168_W32;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Threading;
using System.Data.SqlClient;
using System.Net.Sockets;
using Advantech.Adam;
using Apax_IO_Module_Library;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;


namespace SG25
{
    public partial class ConnectPage : Form
    {
        Label[] AI = new Label[12];
        Button[] DI = new Button[36];
        Button[] DO = new Button[36];
       
        CheckBox[] AIChkRanges = new CheckBox[12];// 12 AI ranges
        string IP = "10.0.0.1";// can be configured by AdamApax.NET Utility
        
        public int BGWConnectAvantechStep = 0;
        public int BGWConnectTimes = 0;
        public bool BGWConnectTimerStart = false;
        public bool DisconnectErrorOK = false;

        public ConnectPage()
        {  
            InitializeComponent();//constructor   
        }

        public delegate void SetTextCallback(string text, int pos);


        private void ConnectPage_Activated(object sender, EventArgs e)
        {
            Class1.DOOutputTimerGlobal = DataAcquitionTimer;
            int i = 0;
            Class1.FirstRunObj.Hide();
            if (Avantech.bModbusConnected)
            {
                if (!MainThread.IsBusy)
                {
                    MainThread.RunWorkerAsync();
                }
            }
            else
            {
                txtConnection.Text = "OffLine";
                cmdPWR.BackColor = Color.Gray;
                cmdIO.BackColor = Color.Gray;

                // -2 means that modules not connected
                lblAIErr.Text = "-2";
                lblDIOErr.Text = "-2";
                lblDIErr.Text = "-2";
                lblDOErr.Text = "-2";
                lblAOErr.Text = "-2";

                for (i = 0; i < 12; i++)
                {
                    SetText("0", i);//SetText(Convert.ToString(AIvalues[i]), i);
                }
                for (i = 0; i < 36; i++)
                {
                    DI[i].BackColor = Color.Red;
                }
                for (i = 0; i < 36; i++)
                {
                    DO[i].BackColor = Color.Red;
                }
                for (i = 0; i < 8; i++)
                {
                   Class1.txtAOOutputVals[i].Text = "";
                   Class1.AOTrackBarVals[i].Value = 0;
                }
            }
        }

	   

        private void DI5040_InterfaceRefresh()
        {
            try
            {
             txtDICntMin.Text = AvantechDIs.txtCntMin;
            chkBoxDIFilterEnable.Checked = AvantechDIs.chkBoxDiFilterEnable;
            }
            catch(Exception ex)
            {

                MessageBox.Show("Connection Error, Please Check the Advantech Connection");
            }
           
        }

        private void DO5046_InterfaceRefresh()
        {
            try
            {

            chbxDOEnableSafety.Checked = AvantechDOs.m_bIsEnableSafetyFcn;
            btnDOSetSafetyValue.Enabled = AvantechDOs.m_bIsEnableSafetyFcn;

            bool bDORet;
            int iDOChannelTotal = AvantechDOs.m_aConf.HwIoTotal[AvantechDOs.m_tmpidx];
            bool[] DOvalues = new bool[iDOChannelTotal];

            cmdNETWORK.BackColor = Color.Green;
            bDORet = AvantechDOs.RefreshData(ref DOvalues);
            lblDOErr.Invoke((MethodInvoker)delegate { lblDOErr.Text = bDORet ? "0" : "-1"; });//lblDOErr.Text = bDORet ? "0" : "-1";
            cmdNETWORK.BackColor = Color.Gray;
            if (bDORet)
            {
                for (int i = 0; i < iDOChannelTotal; i++)
                {
                    DO[i].BackColor = DOvalues[i] ? Color.Green : Color.Red;
                }
            }
            else
            {
                MessageBox.Show("Digital Output refresh first time failed!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            } 
            }
            catch (Exception ex)
            {

                MessageBox.Show("Connection Error, Please Check the Advantech Connection");
            }
        }

        private void AO_InitialDataTabPages()
        {
            try
            {

            int i = 0, idx = 0;
            byte type = (byte)_HardwareIOType.AO;   //APAX-5028 is AO module
            ListViewItem lvItem;
            string[] strRanges;

            for (i = 0; i < AvantechAOs.m_aConf.HwIoType.Length; i++)
            {
                if (AvantechAOs.m_aConf.HwIoType[i] == type)
                    idx = i;
            }
            AvantechAOs.m_tmpidx = idx;

            if (AvantechAOs.m_tmpidx == 0)
            {
                AvantechAOs.m_adamSocket.Configuration().GetModuleTotalRange((int)AvantechAOs.m_idxID, AvantechAOs.m_aConf, 0);
                AvantechAOs.m_usRanges_supAO = AvantechAOs.m_aConf.wHwIoType_0_Range;
            }
            else if (AvantechAOs.m_tmpidx == 1)
            {
                AvantechAOs.m_adamSocket.Configuration().GetModuleTotalRange((int)AvantechAOs.m_idxID, AvantechAOs.m_aConf, 1);
                AvantechAOs.m_usRanges_supAO = AvantechAOs.m_aConf.wHwIoType_1_Range;
            }
            else if (AvantechAOs.m_tmpidx == 2)
            {
                AvantechAOs.m_adamSocket.Configuration().GetModuleTotalRange((int)AvantechAOs.m_idxID, AvantechAOs.m_aConf, 2);
                AvantechAOs.m_usRanges_supAO = AvantechAOs.m_aConf.wHwIoType_2_Range;
            }
            else if (AvantechAOs.m_tmpidx == 3)
            {
                AvantechAOs.m_adamSocket.Configuration().GetModuleTotalRange((int)AvantechAOs.m_idxID, AvantechAOs.m_aConf, 3);
                AvantechAOs.m_usRanges_supAO = AvantechAOs.m_aConf.wHwIoType_3_Range;
            }
            else
            {
                AvantechAOs.m_adamSocket.Configuration().GetModuleTotalRange((int)AvantechAOs.m_idxID, AvantechAOs.m_aConf, 4);
                AvantechAOs.m_usRanges_supAO = AvantechAOs.m_aConf.wHwIoType_4_Range;
            }
            //init range combobox
            strRanges = new string[AvantechAOs.m_aConf.HwIoType_TotalRange[AvantechAOs.m_tmpidx]];
            for (i = 0; i < strRanges.Length; i++)
            {
                strRanges[i] = AnalogOutput.GetRangeName(AvantechAOs.m_usRanges_supAO[i]);
            }
            SetAORangeComboBox(strRanges);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Connection Error, Please Check the Advantech Connection");
            }

        }

        public void SetAORangeComboBox(string[] strRanges)
        {
            try
            {
            cbxAORange.BeginUpdate();
            cbxAORange.Items.Clear();
            for (int i = 0; i < strRanges.Length; i++)
                cbxAORange.Items.Add(strRanges[i]);
            if (cbxAORange.Items.Count > 0)
                cbxAORange.SelectedIndex = 0;
            cbxAORange.EndUpdate();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Connection Error, Please Check the Advantech Connection");
            }
        }

        private void AI_InitialDataTabPages()
        {
            try
            {

            int i = 0, idx = 0;
            byte type = (byte)_HardwareIOType.AI;   //APAX-5017H is AI module
            ListViewItem lvItem;
            string[] strRanges;
            ushort[] m_usRanges_supAI;

            for (i = 0; i < AvantechAIs.m_aConf.HwIoType.Length; i++)
            {
                if (AvantechAIs.m_aConf.HwIoType[i] == type)
                    idx = i;
            }
            AvantechAIs.m_tmpidx = idx;

            //init range combobox
            if (AvantechAIs.m_tmpidx == 0)
            {
                AvantechAIs.m_adamSocket.Configuration().GetModuleTotalRange((int)AvantechAIs.m_idxID, AvantechAIs.m_aConf, 0);
                m_usRanges_supAI = AvantechAIs.m_aConf.wHwIoType_0_Range;
            }
            else if (AvantechAIs.m_tmpidx == 1)
            {
                AvantechAIs.m_adamSocket.Configuration().GetModuleTotalRange((int)AvantechAIs.m_idxID, AvantechAIs.m_aConf, 1);
                m_usRanges_supAI = AvantechAIs.m_aConf.wHwIoType_1_Range;
            }
            else if (AvantechAIs.m_tmpidx == 2)
            {
                AvantechAIs.m_adamSocket.Configuration().GetModuleTotalRange((int)AvantechAIs.m_idxID, AvantechAIs.m_aConf, 2);
                m_usRanges_supAI = AvantechAIs.m_aConf.wHwIoType_2_Range;
            }
            else if (AvantechAIs.m_tmpidx == 3)
            {
                AvantechAIs.m_adamSocket.Configuration().GetModuleTotalRange((int)AvantechAIs.m_idxID, AvantechAIs.m_aConf, 3);
                m_usRanges_supAI = AvantechAIs.m_aConf.wHwIoType_3_Range;
            }
            else
            {
                AvantechAIs.m_adamSocket.Configuration().GetModuleTotalRange((int)AvantechAIs.m_idxID, AvantechAIs.m_aConf, 4);
                m_usRanges_supAI = AvantechAIs.m_aConf.wHwIoType_4_Range;
            }
            //Get combobox items of Range
            strRanges = new string[AvantechAIs.m_aConf.HwIoType_TotalRange[AvantechAIs.m_tmpidx]];
            for (i = 0; i < strRanges.Length; i++)
            {
                strRanges[i] = AnalogInput.GetRangeName(m_usRanges_supAI[i]);
            }
            SetAIRangeComboBox(strRanges);
            //Get combobox items of Burnout Detect Mode
            SetAIBurnoutFcnValueComboBox(new string[] { "Down Scale", "Up Scale" });
            //Get combobox items of Sampling rate (Hz/Ch)
            SetAISampleRateComboBox(new string[] { "100", "1000" });

            }
            catch (Exception ex)
            {

                MessageBox.Show("Connection Error, Please Check the Advantech Connection");
            }
        }

        public void SetAIRangeComboBox(string[] strRanges)
        {
            try
            {
            cbxAIRange.BeginUpdate();
            cbxAIRange.Items.Clear();


            for (int i = 0; i < strRanges.Length; i++)
                cbxAIRange.Items.Add(strRanges[i]);

            if (cbxAIRange.Items.Count > 0)
                cbxAIRange.SelectedIndex = 0;
            cbxAIRange.EndUpdate(); 
            }
            catch (Exception ex)
            {

                MessageBox.Show("Connection Error, Please Check the Advantech Connection");
            }
        }

        public void SetAIBurnoutFcnValueComboBox(string[] strRanges)
        {
            try
            {
            cbxAIBurnoutValue.BeginUpdate();
            cbxAIBurnoutValue.Items.Clear();
            for (int i = 0; i < strRanges.Length; i++)
                cbxAIBurnoutValue.Items.Add(strRanges[i]);

            if (cbxAIBurnoutValue.Items.Count > 0)
                cbxAIBurnoutValue.SelectedIndex = 0;
            cbxAIBurnoutValue.EndUpdate();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Connection Error, Please Check the Advantech Connection");
            }
           
        }

        public void SetAISampleRateComboBox(string[] strSampleRate)
        {
            try
            {
            cbxAISampleRate.BeginUpdate();
            cbxAISampleRate.Items.Clear();
            for (int i = 0; i < strSampleRate.Length; i++)
                cbxAISampleRate.Items.Add(strSampleRate[i]);

            if (cbxAISampleRate.Items.Count > 0)
                cbxAISampleRate.SelectedIndex = 0;
            cbxAISampleRate.EndUpdate();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Connection Error, Please Check the Advantech Connection");
            }
           
        }

        private void AI5017H_InterfaceRefresh()
        {
            try
            {

          
            AI_InitialDataTabPages();
            AvantechAIs.m_iScanCount = 0; 
            }
            catch (Exception ex)
            {

                MessageBox.Show("Connection Error, Please Check the Advantech Connection");
            }
        }

        private void AO5028_InterfaceRefresh()
        {
            // InitialDataTabPages
            AO_InitialDataTabPages();
            AvantechAOs.m_iScanCount = 0;

            try
            {

           
            string strSelPageName = "AO";//string strSelPageName = tabControl1.TabPages[tabControl1.SelectedIndex].Text;
            int idx = 0;
            int iAOChannelTotal = AvantechAOs.m_aConf.HwIoTotal[AvantechAOs.m_tmpidx];
            ushort usVal;

            AvantechAOs.StatusBar_IO = "";//StatusBar_IO.Text = "";

            AvantechAOs.m_adamSocket.Disconnect();
            AvantechAOs.m_adamSocket.Connect(AvantechAOs.m_adamSocket.GetIP(), ProtocolType.Tcp, 502);

            if (strSelPageName == "Module Information")
            {
                AvantechAOs.m_iFailCount = 0;
                AvantechAOs.m_iScanCount = 0;
            }
            else if (strSelPageName == "AO")
            {
                AvantechAOs.RefreshRanges();
                AvantechAOs.RefreshAoStartupValues();
                AvantechAOs.RefreshSafetySetting();
                chbxAOEnableSafety.Checked = AvantechAOs.m_bIsEnableSafetyFcn;


                for (idx = 0; idx < iAOChannelTotal; idx++)
                {
                    if (AvantechAOs.m_adamSocket.AnalogOutput().GetCurrentValue(AvantechAOs.m_idxID, idx, out usVal))// m_idxID is SlotNum(its the index ID of the 5 I/O modules)
                    {
                        AnalogOutput.GetRangeHighLow(AvantechAOs.m_usRanges[idx], out AvantechAOs.m_fHighVals[idx], out AvantechAOs.m_fLowVals[idx]);
                        AvantechAOs.m_fOutputVals[idx] = AnalogOutput.GetScaledValue(AvantechAOs.m_usRanges[idx], usVal);
                        //RefreshOutputPanel(m_fHighVal[idx], m_fLowVal[idx], m_fOutputVal[idx]);// use the High, Low and Output Vals to update UI correspondingly
                        RefreshAnalogOutputPanel(AvantechAOs.m_fHighVals[idx], AvantechAOs.m_fLowVals[idx],
                            AvantechAOs.m_fOutputVals[idx], idx);
                    }
                    else
                        AvantechAOs.StatusBar_IO += "GetValues() filed!";//this.StatusBar_IO.Text += "GetValues() filed!";
                }
            }
            //for (int idx = 0; idx < iAOChannelTotal; idx++)
            //{
            //    //RefreshOutputPanel(m_fHighVal[idx], m_fLowVal[idx], m_fOutputVal[idx]);// use the High, Low and Output Vals to update UI correspondingly
            //    RefreshAnalogOutputPanel(AvantechAOs.m_fHighVals[idx], AvantechAOs.m_fLowVals[idx],
            //        AvantechAOs.m_fOutputVals[idx], idx);
            //} 
            }
            catch (Exception ex)
            {

                MessageBox.Show("Connection Error, Please Check the Advantech Connection");
            }
        }

        private void AO5028_InterfaceInitialize()
        {
            try
            {
        
            int iAOChannelTotal = AvantechAOs.m_aConf.HwIoTotal[AvantechAOs.m_tmpidx];
            for (int i = 0; i < iAOChannelTotal; i++)
            {
                Class1.AOHighVals[i].Text = AvantechAOs.m_fHighVals[i].ToString();
                Class1.AOLowVals[i].Text = AvantechAOs.m_fLowVals[i].ToString();
                Class1.AOOutputVals[i].Text = AvantechAOs.m_fOutputVals[i].ToString();
                Class1.AOTrackBarVals[i].Value = Convert.ToInt32(Class1.AOTrackBarVals[i].Minimum + (Class1.AOTrackBarVals[i].Maximum - Class1.AOTrackBarVals[i].Minimum) * (AvantechAOs.m_fOutputVals[i] - AvantechAOs.m_fLowVals[i]) / (AvantechAOs.m_fHighVals[i] - AvantechAOs.m_fLowVals[i]));
            }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Connection Error, Please Check the Advantech Connection");
            }
        }

        private void DIO5045_InterfaceRefresh()
        {
            try
            {

           
            // DIO DI's UI interface update
            txtDIO_DICntMin.Text = AvantechDIOs.txtCntMin;
            chkBoxDIO_DIFilterEnable.Checked = AvantechDIOs.chkBoxDiFilterEnable;

            // DIO DO's EnableSafety and SetSafetyValue interface update
            chbxDIO_DOEnableSafety.Checked = AvantechDIOs.m_bIsEnableSafetyFcn;
            btnDIO_DOSetSafetyValue.Enabled = AvantechDIOs.m_bIsEnableSafetyFcn;

            int i = 0;
            // DI total
            int iDIChannelTotal = AvantechDIs.m_aConf.HwIoTotal[AvantechDIs.m_tmpidx];
            // DO total
            int iDOChannelTotal = AvantechDOs.m_aConf.HwIoTotal[AvantechDOs.m_tmpidx];
            // DIO's DI Channel
            bool bDIORet;
            int iDIO_DIChannelTotal = AvantechDIOs.m_aConf.HwIoTotal[AvantechDIOs.m_DIidx];
            bool[] DIO_DIvalues = new bool[iDIO_DIChannelTotal];
            // DIO's DO Channel
            int iDIO_DOChannelTotal = AvantechDIOs.m_aConf.HwIoTotal[AvantechDIOs.m_DOidx];
            bool[] DIO_DOvalues = new bool[iDIO_DOChannelTotal];
            cmdNETWORK.BackColor = Color.Green;
            bDIORet = AvantechDIOs.RefreshData(ref DIO_DIvalues, ref DIO_DOvalues);
            //DIO_DOvalues.CopyTo(Class1.DOIArrayValues,0);
            //lblDIOErr.Invoke((MethodInvoker)delegate { lblDIOErr.Text = bDIORet ? "0" : "-1"; });//lblDIOErr.Text = bDIORet ? "0" : "-1";
            
            cmdNETWORK.BackColor = Color.Gray;
            if (bDIORet)
            {
                AvantechDIOs.m_iScanCount++;
                AvantechDIOs.m_iFailCount = 0;
                for (i = iDIChannelTotal; i < iDIChannelTotal + iDIO_DIChannelTotal; i++)
                {
                    DI[i].BackColor = DIO_DIvalues[i - iDIChannelTotal] ? Color.Green : Color.Red;
                    //Class1.DIOArrayValues[i] = DIO_DIvalues[i - iDIChannelTotal];
                }
                for (i = iDOChannelTotal; i < iDOChannelTotal + iDIO_DOChannelTotal; i++)
                {
                    DO[i].BackColor = DIO_DOvalues[i - iDOChannelTotal] ? Color.Green : Color.Red;
                    //Class1.DOIArrayValues[i]= DIO_DOvalues[i - iDOChannelTotal];
                   // Class1.GetDO(Class1.DOIArrayValues);
                }
            }
            else
            {
                MessageBox.Show("Digital Input/Output refresh first time failed!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Connection Error, Please Check the Advantech Connection");
            }
        }

        public static void AvantechSystemInitialize() 
        {
            //// Avantech System and ScanTime initialize
            //TreeNode treeNode = new TreeNode(IP);
            //Avantech.ScanTimeInitialize();
            //Avantech.SetIPAddress(IP);
            //Avantech.AfterSelect_CouplerDevice(treeNode);

            // Avantech System and ScanTime initialize
            try
            {
            TreeNode treeNode = new TreeNode(SystemGlobals.IP);
            Avantech.ScanTimeInitialize();
            Avantech.SetIPAddress(SystemGlobals.IP);
            Avantech.AfterSelect_CouplerDevice(treeNode);
           
            }
            catch (Exception ex)
            {

                MessageBox.Show("Connection Error, Please Check the Advantech Connection");
            }
            
            
        }

        private void AvantechIO_Modules_Initialize()
        {
           try
            {

            // 5040 DI initialize(1)
            AvantechDIs.Initialize(SystemGlobals.IP, SystemGlobals.DISlotNum, Avantech.m_ScanTime_LocalSys[0]);
            AvantechDIs.Start();
            AvantechDIs.tabControl1_SelectedIndexChanged();
            bool bDIRet;
            int iDIChannelTotal = AvantechDIs.m_aConf.HwIoTotal[AvantechDIs.m_tmpidx];
            bool[] DIvalues = new bool[iDIChannelTotal];
            DI5040_InterfaceRefresh();


            // 5046 DO initialize(3)
            AvantechDOs.Initialize(SystemGlobals.IP, SystemGlobals.DOSlotNum, Avantech.m_ScanTime_LocalSys[0]);
            AvantechDOs.Start();
            AvantechDOs.tabControl1_SelectedIndexChanged();
            // DO start to refresh one time to get the latest DO values
            DO5046_InterfaceRefresh();


            // 5028 AO initialize(5)
            AvantechAOs.Initialize(SystemGlobals.IP, SystemGlobals.AOSlotNum, Avantech.m_ScanTime_LocalSys[0]);
            AvantechAOs.Start();
            //AvantechAOs.tabControl1_SelectedIndexChanged();// Get High values, Low values and current Output values from AO module
            AO5028_InterfaceRefresh();// Get High values, Low values and current Output values from AO module and Update UI correspondingly
            //AO5028_InterfaceInitialize();// Use these High values, Low values and current Output values to update UI 


            // 5017H AI initialize(7)
            AvantechAIs.Initialize(SystemGlobals.IP, SystemGlobals.AISlotNum, Avantech.m_ScanTime_LocalSys[0]);
            AvantechAIs.Start();
            AvantechAIs.tabControl1_SelectedIndexChanged();
            AI5017H_InterfaceRefresh();


            // 5045 DI/O initialize(9)
            AvantechDIOs.Initialize(SystemGlobals.IP, SystemGlobals.DIOSlotNum, Avantech.m_ScanTime_LocalSys[0]);
            AvantechDIOs.Start();
            AvantechDIOs.tabControl1_SelectedIndexChanged_DI();
            AvantechDIOs.tabControl1_SelectedIndexChanged_DO();
            // DIO start to refresh one time to get the latest DIO values
            DIO5045_InterfaceRefresh();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Connection Error, Please Check the Fieldbus Connection. This system will shutdown");
                Environment.Exit(0);
            }
        }

        public void IO_UI_Arrays_Assign()
        {
            //Assign the AI Array
            AI[0] = lblAI_0;
            AI[1] = lblAI_1;
            AI[2] = lblAI_2;
            AI[3] = lblAI_3;
            AI[4] = lblAI_4;
            AI[5] = lblAI_5;
            AI[6] = lblAI_6;
            AI[7] = lblAI_7;
            AI[8] = lblAI_8;
            AI[9] = lblAI_9;
            AI[10] = lblAI_10;
            AI[11] = lblAI_11;

            //Assign the DI Array(0-23: 5040 DI, 24-35: 5045 DI)
            DI[0] = btnDIAir_0;
            DI[1] = btnDIEstop_1;
            DI[2] = btnDIPumpOK_2;
            DI[3] = btnDIChamberUp_3;
            DI[4] = btnDIChamberDn_4;
            DI[5] = btnDIShutterDrUP_5;
            DI[6] = btnDIShutterDrDn_6;
            DI[7] = btnDITrolleyRL_7;
            DI[8] = btnDITrolleyClamLL_8;
            DI[9] = btnDIConvUp_9;
            DI[10] = btnDIConvDn_10;
            DI[11] = btnDITraMotorHn_11;
            DI[12] = btnDITravMCtr_12;
            DI[13] = btnDITravRemote_13;
            DI[14] = btnDITravEndSr_14;
            DI[15] = btnDITravClrSr_15;
            DI[16] = btnDITrayPosSnrR_16;
            DI[17] = btnDITrayPosSnrL_17;
            DI[18] = btnDITrolleySnrR_18;
            DI[19] = btnDITrolleySnrL_19;
            DI[20] = btnDIByPassKeySw_20;
            DI[21] = btnDIDrCoverSw_21;
            DI[22] = btnDIChillerAlarm1_22;
            DI[23] = btnDIChillAlm2_23;
            DI[24] = btnDIChillAlm3_24;
            DI[25] = btnDIStandBy_25;
            DI[26] = btnDIStandBy_26;
            DI[27] = btnDIStandBy_27;
            DI[28] = btnDIStandBy_28;
            DI[29] = btnDIStandBy_29;
            DI[30] = btnDIStandBy_30;
            DI[31] = btnDIStandBy_31;
            DI[32] = btnDIStandBy_32;
            DI[33] = btnDIStandBy_33;
            DI[34] = btnDIStandBy_34;
            DI[35] = btnDIStandBy_35;

            //Assign the DO Array(0-23: 5046 DO, 24-35: 5045 DO)
            DO[0] = btnDORFON_0;
            DO[1] = btnDOPumpOn_1;
            DO[2] = btnDOManualTuner_2;
            DO[3] = btnDOAutoTuner_3;
            DO[4] = btnDOTraMotorFW_4;
            DO[5] = btnDOTraMotorBW_5;
            DO[6] = btnDOTraMotorBr1_6;
            DO[7] = btnDOConvMotor23FW_7;
            DO[8] = btnDOConvMotor23BW_8;
            DO[9] = btnDOConvMotorBr23_9;
            DO[10] = btnDOStandBy_10;
            DO[11] = btnDOStandBy_11;
            DO[12] = btnDOChamberUp_12;
            DO[13] = btnDOChamberDn_13;
            DO[14] = btnDOShutterDrUp_14;
            DO[15] = btnDOShutterDrDn_15;
            DO[16] = btnDOTrolleyClamRLk_16;
            DO[17] = btnDOTrolleyClamLLk_17;
            DO[18] = btnDOConvUp_18;
            DO[19] = btnDOConvDn_19;
            DO[20] = btnDOPressureOn_20;
            DO[21] = btnDOVentOn_21;
            DO[22] = btnDOVacuumOn_22;
            DO[23] = btnDOStandBy_23;
            DO[24] = btnDOGas1_24;
            DO[25] = btnDOGas2On_25;
            DO[26] = btnDOTowerLightGr_26;
            DO[27] = btnDOYellowLight_27;
            DO[28] = btnDORedLight_28;
            DO[29] = btnDOBuzzer_29;
            DO[30] = btnDOSafetySwLk_30;
            DO[31] = btnDOStandBy_31;
            DO[32] = btnDOSpare_32;
            DO[33] = btnDOSpare_33;
            DO[34] = btnDOSpare_34;
            DO[35] = btnDOSpare_35;
            Class1.DOOutputTimerGlobal = DataAcquitionTimer;

            // Assign AO High Vals
            Class1.AOHighVals[0] = lblRFSET_0High;
            Class1.AOHighVals[1] = lblTNSET_1High;
            Class1.AOHighVals[2] = lblLDSET_2High;
            Class1.AOHighVals[3] = lblG1SET_3High;
            Class1.AOHighVals[4] = lblG2SET_4High;
            Class1.AOHighVals[5] = lblSPARE_5High;
            Class1.AOHighVals[6] = lblSPARE_6High;
            Class1.AOHighVals[7] = lblSPARE_7High;

            // Assign AO Low Vals
            Class1.AOLowVals[0] = lblRFSET_0Low;
            Class1.AOLowVals[1] = lblTNSET_1Low;
            Class1.AOLowVals[2] = lblLDSET_2Low;
            Class1.AOLowVals[3] = lblG1SET_3Low;
            Class1.AOLowVals[4] = lblG2SET_4Low;
            Class1.AOLowVals[5] = lblSPARE_5Low;
            Class1.AOLowVals[6] = lblSPARE_6Low;
            Class1.AOLowVals[7] = lblSPARE_7Low;

            // Assign AO Output label Vals
            Class1.AOOutputVals[0] = lblRFSET_0Value;
            Class1.AOOutputVals[1] = lblTNSET_1Value;
            Class1.AOOutputVals[2] = lblLDSET_2Value;
            Class1.AOOutputVals[3] = lblG1SET_3Value;
            Class1.AOOutputVals[4] = lblG2SET_4Value;
            Class1.AOOutputVals[5] = lblSPARE_5Value;
            Class1.AOOutputVals[6] = lblSPARE_6Value;
            Class1.AOOutputVals[7] = lblSPARE_7Value;

            // Assign AO Output Text Vals
            Class1.txtAOOutputVals[0] = txtRFSET_0Val;
            Class1.txtAOOutputVals[1] = txtTNSET_1Val;
            Class1.txtAOOutputVals[2] = txtLDSET_2Val;
            Class1.txtAOOutputVals[3] = txtG1SET_3Val;
            Class1.txtAOOutputVals[4] = txtG2SET_4Val;
            Class1.txtAOOutputVals[5] = txtSPARE_5Val;
            Class1.txtAOOutputVals[6] = txtSPARE_6Val;
            Class1.txtAOOutputVals[7] = txtSPARE_7Val;

            // Assign TrackBar array
            Class1.AOTrackBarVals[0] = tBarRFSET_0Val;
            Class1.AOTrackBarVals[1] = tBarTNSET_1Val;
            Class1.AOTrackBarVals[2] = tBarLDSET_2Val;
            Class1.AOTrackBarVals[3] = tBarG1SET_3Val;
            Class1.AOTrackBarVals[4] = tBarG2SET_4Val;
            Class1.AOTrackBarVals[5] = tBarSPARE_5Val;
            Class1.AOTrackBarVals[6] = tBarSPARE_6Val;
            Class1.AOTrackBarVals[7] = tBarSPARE_7Val;

            // Assign AO Checkbox array
            Class1.AOChkRanges[0] = chbxRFSET_0Range;
            Class1.AOChkRanges[1] = chbxTNSET_1Range;
            Class1.AOChkRanges[2] = chbxLDSET_2Range;
            Class1.AOChkRanges[3] = chbxG1SET_3Range;
            Class1.AOChkRanges[4] = chbxG2SET_4Range;
            Class1.AOChkRanges[5] = chbxSPARE_5Range;
            Class1.AOChkRanges[6] = chbxSPARE_6Range;
            Class1.AOChkRanges[7] = chbxSPARE_7Range;

            // Assign AI Checkbox array
            AIChkRanges[0] = chbxAI_0Range;
            AIChkRanges[1] = chbxAI_1Range;
            AIChkRanges[2] = chbxAI_2Range;
            AIChkRanges[3] = chbxAI_3Range;
            AIChkRanges[4] = chbxAI_4Range;
            AIChkRanges[5] = chbxAI_5Range;
            AIChkRanges[6] = chbxAI_6Range;
            AIChkRanges[7] = chbxAI_7Range;
            AIChkRanges[8] = chbxAI_8Range;
            AIChkRanges[9] = chbxAI_9Range;
            AIChkRanges[10] = chbxAI_10Range;
            AIChkRanges[11] = chbxAI_11Range;
        }

        public static void LoadSystemSetting_DB()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = SystemGlobals.ConnectionString;
            string cmdText = "SELECT IP, ScanTime, ConnectionTimeOut, SendTimeOut, ReceiveTimeOut, DOSlotNum, AOSlotNum, AISlotNum, DIOSlotNum, DISlotNum, DIO_DIindex, DIO_DOindex FROM Avantech";
            SqlCommand command = new SqlCommand(cmdText, conn);
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while(reader.Read())
                {
                    SystemGlobals.IP = reader["IP"].ToString();
                    SystemGlobals.ScanTime = Convert.ToInt32(reader["ScanTime"].ToString());
                    SystemGlobals.ConnectionTimeOut = Convert.ToInt32(reader["ConnectionTimeOut"].ToString());
                    SystemGlobals.SendTimeOut = Convert.ToInt32(reader["SendTimeOut"].ToString());
                    SystemGlobals.ReceiveTimeOut = Convert.ToInt32(reader["ReceiveTimeOut"].ToString());
                    SystemGlobals.DOSlotNum = Convert.ToInt32(reader["DOSlotNum"].ToString());
                    SystemGlobals.AOSlotNum = Convert.ToInt32(reader["AOSlotNum"].ToString());
                    SystemGlobals.AISlotNum = Convert.ToInt32(reader["AISlotNum"].ToString());
                    SystemGlobals.DIOSlotNum = Convert.ToInt32(reader["DIOSlotNum"].ToString());
                    SystemGlobals.DISlotNum = Convert.ToInt32(reader["DISlotNum"].ToString());
                    SystemGlobals.DIO_DIindex = Convert.ToInt32(reader["DIO_DIindex"].ToString());
                    SystemGlobals.DIO_DOindex = Convert.ToInt32(reader["DIO_DOindex"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot get information about Avantech. Check database integrity", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (conn != null)
                {
                    conn.Close();
                }
                return;
            }
            conn.Close();
            reader.Close();
        }

        public void ConnectPage_Load(System.Object sender, System.EventArgs e)   
        {
            Class2.ConnOpen();
            Class2.GetEventCode();
           
            string Version = this.GetType().Assembly.GetName().Version.ToString();
            Version = Version.Remove(5, 2);
            Assembly assembly = Assembly.GetExecutingAssembly();
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                AssemblyDescriptionAttribute descriptionAttribute = (AssemblyDescriptionAttribute)attributes[0];
                string buf1 = descriptionAttribute.Description.ToString();
                buf1 = buf1.Remove(5, 1);
                Class1.VersionNo = Version + " " + buf1;
                Class1.sProjectPath = "C:\\Program Files\\SG25\\SG25 V" + Class1.VersionNo + "";
            }

            // Load Default System Setting From Database

            Class1.DOOutputTimerGlobal = DataAcquitionTimer;
            Class1.btnAutoStart = this.cmdStartProgram;

            LoadConnectionPageSettings();
            Class1.RetForm = this;
            this.Hide();
            
                if (Avantech.bModbusConnected)
                { 
                    
                    if ((!Class1.IsLoginedIn) && (Class2.LastUserLoginReload())) // if already come in last time, resume to last point
                    {
                    Class1.AutoCycle = true;
                    Class1.ManualCycle = false;
                    Class1.Auto = true;
                    Class1.ManualP = false;
                    Class1.User = false;
                    Class1.SetupPage = false;
                    Class1.Program = false;
                    // Load Setup
                    // Load Last Recipe
                    Class2.DBAutoPPstatusRead();
                    if (Class1.RFFlag)
                    {
                        RFTimeCont objMsgRfTimeCountPage = new RFTimeCont();
                        objMsgRfTimeCountPage.ShowDialog();
                    }
                    else
                    {
                        if (Class1.TheUser == "OPUSER" || Class1.TheUser == "OP")
                        {
                            Main1 objMain1 = new Main1();// Auto page
                            objMain1.ShowDialog();
                        }
                        else
                        {
                            Mode objMode = new Mode();// Model page
                            objMode.ShowDialog();
                        }
                    }
                }
                else
                {
                   //  otherwise go to Splash and need to login in
                    Splash objSplash = new Splash();
                    objSplash.ShowDialog();
                }
                
          
                }
                else// Already No connection to Avantech, need to reload connection
                {
                    
                   // if(Class1.connectedObjFlg==false)
                   // {
                    UserConnectionSelectionPage objUserConnectSelectPage = new UserConnectionSelectionPage();
                    objUserConnectSelectPage.ShowDialog();
                    if (Class1.IsContinueClicked)// indicating it is ADMIN(then need to show Connection page after connected)
                    {
                        LoadConnectionPageSettings();// Initialize Avantech etc
                        //this.Show();
                        Class1.IsContinueClicked = false;// reset
                    }
                   // }
                }
                return;
            
           
        }

        public void LoadConnectionPageSettings()
        {
            // Load Default System Setting From Database
            LoadSystemSetting_DB();

            // IO UI Arrays Assign
            IO_UI_Arrays_Assign();

            // System Initialize
            AvantechSystemInitialize();

            if (Avantech.bModbusConnected)
            {
                Avantech.ThreadStartShowWaitForm();
                // Connection status
                txtConnection.Text = "OnLine";
                cmdPWR.BackColor = Color.Green;
                cmdIO.BackColor = Color.Green;


                // All IOs initialization
                AvantechIO_Modules_Initialize();

                // Testing Output Split
                DataAcquitionTimer.Enabled = true;
                Class1.DOOutputTimerGlobal = DataAcquitionTimer;

                try
                {
                    if (!MainThread.IsBusy)
                    {
                        MainThread.RunWorkerAsync();
                    }
                   
                    Avantech.MainThreadEnabled = true;
                    Avantech.AIEnabled = true;
                    Avantech.DIOEnabled = true;
                    Avantech.DIEnabled = true;
                    Avantech.DOEnabled = true;
                    Avantech.AOEnabled = true;
                    Avantech.Avantech_AllFailCheckEnabled = true;
                    //MessageBox.Show("Connect to Avantech Success!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Program already running. This instance will abort.");
                    //if(MainThread.IsBusy==false)
                    System.Environment.Exit(0);
                }
                Class2.LoadSetup();
                Class2.RecipeUpload();
                Class1.AutoC = Convert.ToInt16(Class2.Read("Autostart", "Setup"));
                if (Class1.AutoC == 1)
                {
                    ChkAutoStart.Checked = true;
                }
                else
                {
                    ChkAutoStart.Checked = false;
                }

                if (ChkAutoStart.Checked == true)
                {
                   // Class1.Connected = true;
                    if (AutoStartup.IsBusy == false)
                    {
                        //Class1.Connected = true;
                       // cmdStartProgram.Enabled = false;
                        AutoStartup.RunWorkerAsync();
                    }

                }

            }
            else
            {
                txtConnection.Text = "OffLine";
                cmdPWR.BackColor = Color.Gray;
                cmdIO.BackColor = Color.Gray;


                // -2 means that modules not connected
                lblAIErr.Text = "-2";
                lblDIOErr.Text = "-2";
                lblDIErr.Text = "-2";
                lblDOErr.Text = "-2";
                lblAOErr.Text = "-2";
                
                DataAcquitionTimer.Enabled = false;
                if(Class1.Connected==true)
                {
                    AvantechConnectTimer.Enabled = true;
                }
                
                AvantechAOs.Initialize(SystemGlobals.IP, SystemGlobals.AOSlotNum, Avantech.m_ScanTime_LocalSys[0]);//AvantechAOs.Initialize(IP, AOSlotNum, Avantech.m_ScanTime_LocalSys[0]);
            }
        }


        public void cmdConnect_Click(System.Object sender, System.EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                MessageBox.Show("Modbus is already connected", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }
            else
            {
                AvantechSystemInitialize();
                if (Avantech.bModbusConnected)
                {
                    DisconnectErrorOK = false;
                    lblResumeConnectionInfo.Text = "";
                    // Connection status
                    txtConnection.Text = "OnLine";
                    cmdPWR.BackColor = Color.Green;
                    cmdIO.BackColor = Color.Green;
                    // Corresponding UI enabled
                    gbDI.Enabled = true;
                    gbDO.Enabled = true;
                    gbAO.Enabled = true;
                    gbAI.Enabled = true;
                    gbAORangeSettings.Enabled = true;
                    gbAOSafetyFunction.Enabled = true;

                    // All IOs initialization
                    AvantechIO_Modules_Initialize();

                    // Testing Output Split
                    DataAcquitionTimer.Enabled = true;

                    try
                    {
                        if (!MainThread.IsBusy)
                        {
                            MainThread.RunWorkerAsync();
                        }
                        Avantech.MainThreadEnabled = true;
                        Avantech.AIEnabled = true;
                        Avantech.DIOEnabled = true;
                        Avantech.DIEnabled = true;
                        Avantech.DOEnabled = true;
                        Avantech.AOEnabled = true;
                        Avantech.Avantech_AllFailCheckEnabled = true;
                        MessageBox.Show("Successful Connection to Fieldbus!");
                        //Avantech.ThreadStartShowWaitForm();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Program already running. This instance will abort.");
                        //if(MainThread.IsBusy==false)
                        System.Environment.Exit(0);
                    }
                }
                else
                {
                    txtConnection.Text = "OffLine";
                    cmdPWR.BackColor = Color.Gray;
                    cmdIO.BackColor = Color.Gray;

                    // -2 means that modules not connected
                    lblAIErr.Text = "-2";
                    lblDIOErr.Text = "-2";
                    lblDIErr.Text = "-2";
                    lblDOErr.Text = "-2";
                    lblAOErr.Text = "-2";

                    AvantechAOs.Initialize(SystemGlobals.IP, SystemGlobals.AOSlotNum, Avantech.m_ScanTime_LocalSys[0]);//AvantechAOs.Initialize(IP, AOSlotNum, Avantech.m_ScanTime_LocalSys[0]);
                    MessageBox.Show("Failed to Connect to Fieldbus! Please check the phyiscal connection and Modbus address setting!");
                }
            }
        }


       
        public void ConnectPage_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            //int ret2 = APS168.APS_close();
            //ret2 = APS168_W32.APS168.APS_close();
            AvantechDIs.FreeResource();
            AvantechDOs.FreeResource();
            AvantechAOs.FreeResource();
            AvantechAIs.FreeResource();
            AvantechDIOs.FreeResource();

            // Corresponding UI disabled
           
            gbDI.Enabled = false;
            gbDO.Enabled = false;
            gbAO.Enabled = false;
            gbAI.Enabled = false;
            gbAORangeSettings.Enabled = false;
            gbAOSafetyFunction.Enabled = false;
            Avantech.bModbusConnected = false;

           // Application.Exit();
        }

        private void cmdHelp_Click(System.Object sender, System.EventArgs e)
	    {
		    System.Diagnostics.Process p = new System.Diagnostics.Process();

		    try {
                p.StartInfo.FileName = "C:/Users/Paloma/Documents/Visual Studio 2013/Projects/c# CxTitan16apr14_beta3/c# CxTitan16apr14_beta2/CxTitan/Manuals/APS_FunctionLibrary_V1.2.pdf";
			    p.Start();
		    } catch (Exception ex) {
			    MessageBox.Show(ex.ToString());
		    }

        }

        private void btnDOTraMotorFW_4_Click(System.Object sender, System.EventArgs e)
	    {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                if (btnDOTraMotorFW_4.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DOSlotNum, 4, bVal);
                    btnDOTraMotorFW_4.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 4, bVal);
                    btnDOTraMotorFW_4.BackColor = Color.Red;
                }
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOTraMotorFW_4.BackColor == Color.Red)
                {
                    btnDOTraMotorFW_4.BackColor = Color.Green;
                }
                else
                {
                    btnDOTraMotorFW_4.BackColor = Color.Red;
                }
            }
	    }

        private void btnDOTraMotorBW_5_Click(System.Object sender, System.EventArgs e)
	    {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                if (btnDOTraMotorBW_5.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DOSlotNum, 5, bVal);
                    btnDOTraMotorBW_5.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 5, bVal);
                    btnDOTraMotorBW_5.BackColor = Color.Red;
                }
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOTraMotorBW_5.BackColor == Color.Red)
                {
                    btnDOTraMotorBW_5.BackColor = Color.Green;
                }
                else
                {
                    btnDOTraMotorBW_5.BackColor = Color.Red;
                }
            }
	    }

        private void btnDOTraMotorBr1_6_Click(System.Object sender, System.EventArgs e)
	    {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                if (btnDOTraMotorBr1_6.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DOSlotNum, 6, bVal);
                    btnDOTraMotorBr1_6.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 6, bVal);
                    btnDOTraMotorBr1_6.BackColor = Color.Red;
                }
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOTraMotorBr1_6.BackColor == Color.Red)
                {
                    btnDOTraMotorBr1_6.BackColor = Color.Green;
                }
                else
                {
                    btnDOTraMotorBr1_6.BackColor = Color.Red;
                }
            }
	    }

        private void btnDOConvMotor23FW_7_Click(System.Object sender, System.EventArgs e)
	    {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                if (btnDOConvMotor23FW_7.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DOSlotNum, 7, bVal);
                    btnDOConvMotor23FW_7.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 7, bVal);
                    btnDOConvMotor23FW_7.BackColor = Color.Red;
                }
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOConvMotor23FW_7.BackColor == Color.Red)
                {
                    btnDOConvMotor23FW_7.BackColor = Color.Green;
                }
                else
                {
                    btnDOConvMotor23FW_7.BackColor = Color.Red;
                }
            }
	    }

        private void btnDOConvMotor23BW_8_Click(System.Object sender, System.EventArgs e)
	    {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                if (btnDOConvMotor23BW_8.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DOSlotNum, 8, bVal);
                    btnDOConvMotor23BW_8.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 8, bVal);
                    btnDOConvMotor23BW_8.BackColor = Color.Red;
                }
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOConvMotor23BW_8.BackColor == Color.Red)
                {
                    btnDOConvMotor23BW_8.BackColor = Color.Green;
                }
                else
                {
                    btnDOConvMotor23BW_8.BackColor = Color.Red;
                }
            }
	    }

        private void btnDOConvMotorBr23_9_Click(System.Object sender, System.EventArgs e)
	    {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                if (btnDOConvMotorBr23_9.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DOSlotNum, 9, bVal);
                    btnDOConvMotorBr23_9.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 9, bVal);
                    btnDOConvMotorBr23_9.BackColor = Color.Red;
                }
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOConvMotorBr23_9.BackColor == Color.Red)
                {
                    btnDOConvMotorBr23_9.BackColor = Color.Green;
                }
                else
                {
                    btnDOConvMotorBr23_9.BackColor = Color.Red;
                }
            }
	    }

        private void btnDOStandBy_10_Click(System.Object sender, System.EventArgs e)
	    {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                if (btnDOStandBy_10.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DOSlotNum, 10, bVal);
                    btnDOStandBy_10.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 10, bVal);
                    btnDOStandBy_10.BackColor = Color.Red;
                }
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOStandBy_10.BackColor == Color.Red)
                {
                    btnDOStandBy_10.BackColor = Color.Green;
                }
                else
                {
                    btnDOStandBy_10.BackColor = Color.Red;
                }
            }
	    }

        private void btnDOStandBy_11_Click(System.Object sender, System.EventArgs e)
	    {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                if (btnDOStandBy_11.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DOSlotNum, 11, bVal);
                    btnDOStandBy_11.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 11, bVal);
                    btnDOStandBy_11.BackColor = Color.Red;
                }
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOStandBy_11.BackColor == Color.Red)
                {
                    btnDOStandBy_11.BackColor = Color.Green;
                }
                else
                {
                    btnDOStandBy_11.BackColor = Color.Red;
                }
            }
	    }

        private void btnDOChamberUp_12_Click(System.Object sender, System.EventArgs e)
	    {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                if (btnDOChamberUp_12.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DOSlotNum, 12, bVal);
                    btnDOChamberUp_12.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 12, bVal);
                    btnDOChamberUp_12.BackColor = Color.Red;
                }
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOChamberUp_12.BackColor == Color.Red)
                {
                    btnDOChamberUp_12.BackColor = Color.Green;
                }
                else
                {
                    btnDOChamberUp_12.BackColor = Color.Red;
                }
            }
	    }

        private void btnDOChamberDn_13_Click(System.Object sender, System.EventArgs e)
	    {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                if (btnDOChamberDn_13.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DOSlotNum, 13, bVal);
                    btnDOChamberDn_13.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 13, bVal);
                    btnDOChamberDn_13.BackColor = Color.Red;
                }
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOChamberDn_13.BackColor == Color.Red)
                {
                    btnDOChamberDn_13.BackColor = Color.Green;
                }
                else
                {
                    btnDOChamberDn_13.BackColor = Color.Red;
                }
            }
	    }

        private void btnDOShutterDrUp_14_Click(System.Object sender, System.EventArgs e)
	    {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                if (btnDOShutterDrUp_14.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DOSlotNum, 14, bVal);
                    btnDOShutterDrUp_14.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 14, bVal);
                    btnDOShutterDrUp_14.BackColor = Color.Red;
                }
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOShutterDrUp_14.BackColor == Color.Red)
                {
                    btnDOShutterDrUp_14.BackColor = Color.Green;
                }
                else
                {
                    btnDOShutterDrUp_14.BackColor = Color.Red;
                }
            }
	    }

        private void btnDOShutterDrDn_15_Click(System.Object sender, System.EventArgs e)
	    {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                if (btnDOShutterDrDn_15.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DOSlotNum, 15, bVal);
                    btnDOShutterDrDn_15.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 15, bVal);
                    btnDOShutterDrDn_15.BackColor = Color.Red;
                }
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOShutterDrDn_15.BackColor == Color.Red)
                {
                    btnDOShutterDrDn_15.BackColor = Color.Green;
                }
                else
                {
                    btnDOShutterDrDn_15.BackColor = Color.Red;
                }
            }
	    }

	    private void BGWConnect_RunWorkerCompleted(System.Object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
	    {
            //TextBox1.Text = "OffLine";
            //TextBox2.Text = "OffLine";
            //this.Show();

            //MessageBox.Show("Lost connection!");
	    }


	    public void MainThread_DoWork(System.Object sender, System.ComponentModel.DoWorkEventArgs e)
	    {
            int i = 0;
            double Ai_Value = 0;
            Int32 ret2 = default(Int32);
            double FreeSlot = 0;

            try
            {

             // AI 5017H 
            bool bAIRet;
            int iAIChannelTotal = AvantechAIs.m_aConf.HwIoTotal[AvantechAIs.m_tmpidx];
            double[] AIvalues = new double[iAIChannelTotal];
            string[] strAIvalues = new string[iAIChannelTotal];

            // DIO 5045
           
            bool bDIORet;
            // DIO's DI Channel
            int iDIO_DIChannelTotal = AvantechDIOs.m_aConf.HwIoTotal[AvantechDIOs.m_DIidx];
            bool[] DIO_DIvalues = new bool[iDIO_DIChannelTotal];
            // DIO's DO Channel
            int iDIO_DOChannelTotal = AvantechDIOs.m_aConf.HwIoTotal[AvantechDIOs.m_DOidx];
            bool[] DIO_DOvalues = new bool[iDIO_DOChannelTotal];

            // DI 5040
            bool bDIRet;
            int iDIChannelTotal = AvantechDIs.m_aConf.HwIoTotal[AvantechDIs.m_tmpidx];
            bool[] DIvalues = new bool[iDIChannelTotal];

            // DO 5046
            bool bDORet;
            int iDOChannelTotal = AvantechDOs.m_aConf.HwIoTotal[AvantechDOs.m_tmpidx];
            bool[] DOvalues = new bool[iDOChannelTotal];

            // AO 5028
            bool bAORet;
            int iAOChannelTotal = AvantechAOs.m_aConf.HwIoTotal[AvantechAOs.m_tmpidx];
            double[] AOvalues = new double[iAOChannelTotal];
            string[] strAOvalues = new string[iAOChannelTotal];

            
            while (Avantech.MainThreadEnabled)
            {
                //StatusBar_IO.Text = "Polling (Interval=" + timer1.Interval.ToString() + "ms): ";

                //// ***********DI*********** ////
                //if (Avantech.DIEnabled)
                //{
                //    cmdNETWORK.BackColor = Color.Green;
                //    bDIRet = AvantechDIs.RefreshData(ref DIvalues);
                //    DIvalues.CopyTo(Class1.DIOArrayValues, 0);
                //    Class1.GetDI(Class1.DIOArrayValues);
                //    lblDIErr.Invoke((MethodInvoker)delegate { lblDIErr.Text = bDIRet ? "0" : "-1"; });//lblDIErr.Text = bDIRet ? "0" : "-1";
                //    cmdNETWORK.BackColor = Color.Gray;
                //    if (bDIRet)
                //    {
                //        AvantechDIs.m_iScanCount++;
                //        AvantechDIs.m_iFailCount = 0;
                //        this.Invoke((MethodInvoker)delegate
                //        {
                //            for (i = 0; i < iDIChannelTotal; i++)
                //            {
                //                DI[i].BackColor = DIvalues[i] ? Color.Green : Color.Red;
                                
                //            }
                //        });
                        
                //       }
                //    else
                //    {
                //        AvantechDIs.m_iFailCount++;
                //    }

                //    if (AvantechDIs.m_iFailCount > 0)
                //    {
                //        Avantech.DIEnabled = false;//timer1.Enabled = false;
                //        Avantech.bModbusConnected = false;
                //        AvantechConnectTimer.Enabled = true;
                //        ResetAvantechConnectTimerProperty();
                //    }
                //    if (AvantechDIs.m_iScanCount % 50 == 0)
                //        GC.Collect();
                //}
                ////// ***********End of DI*********** ////

                ////// ***********AI*********** ////
                //if (Avantech.AIEnabled)
                //{
                //    cmdNETWORK.BackColor = Color.Green;
                //    //bAIRet = AvantechAIs.RefreshData(ref AIvalues);
                //    bAIRet = AvantechAIs.RefreshData(ref strAIvalues);
                //    lblAIErr.Invoke((MethodInvoker)delegate { lblAIErr.Text = bAIRet ? "0" : "-1"; });//lblAIErr.Text = bAIRet ? "0" : "-1";
                //    cmdNETWORK.BackColor = Color.Gray;
                //    if (bAIRet)
                //    {
                //        AvantechAIs.m_iScanCount++;
                //        AvantechAIs.m_iFailCount = 0;
                //        for (i = 0; i < iAIChannelTotal; i++)
                //        {
                //            SetText(strAIvalues[i], i);//SetText(Convert.ToString(AIvalues[i]), i);
                //          switch (i)
                //          {
                //              case 0:
                //                  Class1.AI_PressureValue =Convert.ToDouble(strAIvalues[0]);
                //                  if(Class1.AI_PressureValue<6.0)
                //                  {
                //                      Class1.Intlk = true;
                //                  }
                //                  else
                //                  {
                //                      Class1.Intlk = false;
                //                  }
                //                  break;
                //              case 1:
                //                  Class1.AI_ARFPowerValue =Math.Round(Convert.ToDouble((strAIvalues[1])), 1);
                //                  break;
                //              case 2:
                //                  Class1.AI_RFRefelctedValue = Math.Round(Convert.ToDouble((strAIvalues[2])), 1);
                //                  break;
                //              case 3:
                //                  Class1.AI_BiasValue = Math.Round(Convert.ToDouble((strAIvalues[3])), 1);
                //                  break;
                //              case 4:
                //                  Class1.AI_TuneValue = Math.Round(Convert.ToDouble((strAIvalues[4])), 2);
                //                  break;
                //              case 5:
                //                  Class1.AI_LoadValue = Math.Round(Convert.ToDouble((strAIvalues[5])), 2);
                //                  break;
                //              case 6:
                //                 Class1.AI_GAS1PSValue = Math.Round(Convert.ToDouble((strAIvalues[6])), 2);
                //                  break;
                //              case 7:
                //                  Class1.AI_GAS2PSValue = Math.Round(Convert.ToDouble((strAIvalues[7])), 2);
                //                  break;
                //              case 8:
                //                  Class1.AI_GAS1Value = Math.Round(Convert.ToDouble((strAIvalues[8])), 2);
                //                  break;
                //              case 9:
                //                  Class1.AI_GAS2Value = Math.Round(Convert.ToDouble((strAIvalues[9])), 2);
                //                  break;
                                  
                //              default:
                //                  break; // TODO: might not be correct. Was : Exit Select

                //                  break;
                //          }
                //        }
                //    }
                //    else
                //    {
                //        AvantechAIs.m_iFailCount++;
                //    }

                //    if (AvantechAIs.m_iFailCount > 0)
                //    {
                //        Avantech.AIEnabled = false;//timer1.Enabled = false;
                //        Avantech.bModbusConnected = false;
                //        AvantechConnectTimer.Enabled = true;
                //        ResetAvantechConnectTimerProperty();
                //        //MessageBox.Show("Please check the physical connection and MODBUS address setting!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                //    }
                //    if (AvantechAIs.m_iScanCount % 50 == 0)
                //        GC.Collect();
                //}
                //// ***********End of AI*********** ////
              

                 if (Avantech.DOEnabled)
                {
                    cmdNETWORK.BackColor = Color.Green;
                    Thread.Sleep(50);
                    cmdNETWORK.BackColor = Color.Gray;
                }
                if (Avantech.AOEnabled)
                {
                    cmdNETWORK.BackColor = Color.Green;
                    Thread.Sleep(50);
                    cmdNETWORK.BackColor = Color.Gray;
                }

                if (Avantech.DIEnabled)
                {
                    cmdNETWORK.BackColor = Color.Green;
                    Thread.Sleep(50);
                    cmdNETWORK.BackColor = Color.Gray;
                }

                if (Avantech.AIEnabled)
                {
                    cmdNETWORK.BackColor = Color.Green;
                    Thread.Sleep(50);
                    cmdNETWORK.BackColor = Color.Gray;
                }

                if (Avantech.DIOEnabled)
                {
                    cmdNETWORK.BackColor = Color.Green;
                    Thread.Sleep(50);
                    cmdNETWORK.BackColor = Color.Gray;
                }

                Thread.Sleep(Avantech.m_ScanTime_LocalSys[0]); //Thread.Sleep(Avantech.m_ScanTime_LocalSys[0]);// same as Avantech's timer1(1000)
            }
            }
            catch (Exception ex)
            {

                //MessageBox.Show("Connection error, Please check advantech Connection. ");
            }
                       
           
	    }

	    private void MainThread_RunWorkerCompleted(System.Object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
	    {
		    //Thread.Sleep(1000)
		    if (BGWConnect.IsBusy == false) {
			    BGWConnect.RunWorkerAsync();
		    }

	    }


	    private void SetText(string text, int pos)
	    {
		    try {
			    if (this.AI[pos].InvokeRequired) {
				    SetTextCallback d = new SetTextCallback(SetText);
				    this.Invoke(d, new object[] {
					    text,
					    pos
				    });
			    } 
                else {
				    this.AI[pos].Text = text;
			    }
            } catch (Exception ex) 
            {
		    }
	    }

        private void cmdStartProgram_Click(object sender, EventArgs e) 
        {
            Class1.RetForm = this;
            this.Hide();
            Mode objmode = new Mode();
            objmode.ShowDialog();
     
	    }


        public void cmdExit_Click(object sender, EventArgs e)
        {
            if (Class1.Connected == false & Class1.TunePageConnected==false)
            {
                if (Class1.TheUser!=null)
                {
                    Class1.IsLoginedIn = false;
                    Class2.UpdateUserLoginedIn(Class1.TheUser, Class1.IsLoginedIn);
                }
                
                Close();
                System.Environment.Exit(0);
            }
            else
            {
                if (Class1.CMP == true)
                {
                    Class1.IOFlg = true;

                }
                if (Class1.TunePageConnected == true)
                {
                    this.Hide();
                    return;
                }
                this.Hide();
                try
                {
                    Class1.RetFormManual.ShowDialog();
                    Class1.CMP = false;
                }
                catch { }


            }
        }

        public void cmdDisconnect_Click(object sender, EventArgs e)
        {
            // Disconnect IO modules
            AvantechDIs.FreeResource();
            AvantechDOs.FreeResource();
            AvantechAOs.FreeResource();
            AvantechAIs.FreeResource();
            AvantechDIOs.FreeResource();

            // Corresponding UI disabled
            txtConnection.Text = "OffLine";
            cmdPWR.BackColor = Color.Gray;
            cmdIO.BackColor = Color.Gray;
            gbDI.Enabled = false;
            gbDO.Enabled = false;
            gbAO.Enabled = false;
            gbAI.Enabled = false;
            gbAORangeSettings.Enabled = false;
            gbAOSafetyFunction.Enabled = false;
            //Class1.Connected = false;
            //Connect.Enabled = true;
            //// Thread and timer disable
            //if(MainThread.IsBusy)
            //{
            //    MainThread.CancelAsync();
            //}            
            //Avantech.MainThreadEnabled = false;
            //OutputTimer.Enabled = false;

            // Connection status disable
            Avantech.bModbusConnected = false;
        }

        private void BGWConnect_DoWork(object sender, DoWorkEventArgs e)
        {
            ////Disconnect
            //int ret = APS168.APS_close();
            // APS168.APS_stop_field_bus(CardID, BusNo);
            
            ////Timer1.Enabled = False
            ////End
	    }

        private void btnDOPumpOn_1_Click(object sender, EventArgs e)
        {
           if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                if (btnDOPumpOn_1.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DOSlotNum, 1, bVal);
                    btnDOPumpOn_1.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 1, bVal);
                    btnDOPumpOn_1.BackColor = Color.Red;
                }
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOPumpOn_1.BackColor == Color.Red)
                {
                    btnDOPumpOn_1.BackColor = Color.Green;
                }
                else
                {
                    btnDOPumpOn_1.BackColor = Color.Red;
                }
            }
        }

        private void btnDOManualTuner_2_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                if (btnDOManualTuner_2.BackColor == Color.Red)
                {
                    if (Class1.DO_AutoTuner == false)
                    {
                        bVal = true;
                        Class2.SetDO(Class1.DOSlotNum, 2, bVal);
                        btnDOManualTuner_2.BackColor = Color.Green;
                    }
                    else
                        MessageBox.Show("Switch to Manual Tune, Please Turned Auto Tune off!");
                    
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 2, bVal);
                    btnDOManualTuner_2.BackColor = Color.Red;
                }
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOManualTuner_2.BackColor == Color.Red)
                {
                    btnDOManualTuner_2.BackColor = Color.Green;
                }
                else
                {
                    btnDOManualTuner_2.BackColor = Color.Red;
                }
            }
        }

        private void btnDOAutoTuner_3_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                if (btnDOAutoTuner_3.BackColor == Color.Red)
                {
                    if (Class1.DO_ManualTuner == false)
                    {
                        bVal = true;
                        Class2.SetDO(Class1.DOSlotNum, 3, bVal);
                        btnDOAutoTuner_3.BackColor = Color.Green;
                    }
                    else
                        MessageBox.Show("Switch to Auto Tune, Please Turned Manual Tune off!");
                    
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 3, bVal);
                    btnDOAutoTuner_3.BackColor = Color.Red;
                }
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOAutoTuner_3.BackColor == Color.Red)
                {
                    btnDOAutoTuner_3.BackColor = Color.Green;
                }
                else
                {
                    btnDOAutoTuner_3.BackColor = Color.Red;
                }
            }
        }

        private void btnDOTrolleyClamRLk_16_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                if (btnDOTrolleyClamRLk_16.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DOSlotNum, 16, bVal);
                    btnDOTrolleyClamRLk_16.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 16, bVal);
                    btnDOTrolleyClamRLk_16.BackColor = Color.Red;
                }
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOTrolleyClamRLk_16.BackColor == Color.Red)
                {
                    btnDOTrolleyClamRLk_16.BackColor = Color.Green;
                }
                else
                {
                    btnDOTrolleyClamRLk_16.BackColor = Color.Red;
                }
            }
        }

        private void btnDOTrolleyClamLLk_17_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                if (btnDOTrolleyClamLLk_17.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DOSlotNum, 17, bVal);
                    btnDOTrolleyClamLLk_17.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 17, bVal);
                    btnDOTrolleyClamLLk_17.BackColor = Color.Red;
                }
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOTrolleyClamLLk_17.BackColor == Color.Red)
                {
                    btnDOTrolleyClamLLk_17.BackColor = Color.Green;
                }
                else
                {
                    btnDOTrolleyClamLLk_17.BackColor = Color.Red;
                }
            }
        }

        private void btnDOConvUp_18_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                if (btnDOConvUp_18.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DOSlotNum, 18, bVal);
                    btnDOConvUp_18.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 18, bVal);
                    btnDOConvUp_18.BackColor = Color.Red;
                }
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOConvUp_18.BackColor == Color.Red)
                {
                    btnDOConvUp_18.BackColor = Color.Green;
                }
                else
                {
                    btnDOConvUp_18.BackColor = Color.Red;
                }
            }
        }

        private void btnDOConvDn_19_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                if (btnDOConvDn_19.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DOSlotNum, 19, bVal);
                    btnDOConvDn_19.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 19, bVal);
                    btnDOConvDn_19.BackColor = Color.Red;
                }
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOConvDn_19.BackColor == Color.Red)
                {
                    btnDOConvDn_19.BackColor = Color.Green;
                }
                else
                {
                    btnDOConvDn_19.BackColor = Color.Red;
                }
            }
        }

        private void btnDOPressureOn_20_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                if (btnDOPressureOn_20.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DOSlotNum, 20, bVal);
                    btnDOPressureOn_20.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 20, bVal);
                    btnDOPressureOn_20.BackColor = Color.Red;
                }
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOPressureOn_20.BackColor == Color.Red)
                {
                    btnDOPressureOn_20.BackColor = Color.Green;
                }
                else
                {
                    btnDOPressureOn_20.BackColor = Color.Red;
                }
            }
        }

        private void btnDOVentOn_21_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                if (btnDOVentOn_21.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DOSlotNum, 21, bVal);
                    btnDOVentOn_21.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 21, bVal);
                    btnDOVentOn_21.BackColor = Color.Red;
                }
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOVentOn_21.BackColor == Color.Red)
                {
                    btnDOVentOn_21.BackColor = Color.Green;
                }
                else
                {
                    btnDOVentOn_21.BackColor = Color.Red;
                }
            }
        }

        private void btnDOVacuumOn_22_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                if (btnDOVacuumOn_22.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DOSlotNum, 22, bVal);
                    btnDOVacuumOn_22.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 22, bVal);
                    btnDOVacuumOn_22.BackColor = Color.Red;
                }
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOVacuumOn_22.BackColor == Color.Red)
                {
                    btnDOVacuumOn_22.BackColor = Color.Green;
                }
                else
                {
                    btnDOVacuumOn_22.BackColor = Color.Red;
                }
            }
        }

        private void btnDOStandBy_23_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                if (btnDOStandBy_23.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DOSlotNum, 23, bVal);
                    btnDOStandBy_23.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 23, bVal);
                    btnDOStandBy_23.BackColor = Color.Red;
                }
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOStandBy_23.BackColor == Color.Red)
                {
                    btnDOStandBy_23.BackColor = Color.Green;
                }
                else
                {
                    btnDOStandBy_23.BackColor = Color.Red;
                }
            }
        }

        // From DO_24 onwards to DO_35, use AvantechDIOs as they are belong to APAX5045
        // DIOSlotNum = 9 and channel starting from AvantechDIOs.m_iDoOffset = 12 to 23
        private void btnDOGas1_24_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                //Avantech.DIOEnabled = false;
                if (!AvantechDIOs.CheckControllable())
                {
                    lblDIOErr.Text = "-1";
                    return;
                }
                if (btnDOGas1_24.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DIOSlotNum, AvantechDIOs.m_iDoOffset, bVal);
                    btnDOGas1_24.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DIOSlotNum, AvantechDIOs.m_iDoOffset, bVal);
                    btnDOGas1_24.BackColor = Color.Red;
                }
                //Avantech.DIOEnabled = true;
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOGas1_24.BackColor == Color.Red)
                {
                    btnDOGas1_24.BackColor = Color.Green;
                }
                else
                {
                    btnDOGas1_24.BackColor = Color.Red;
                }
            }
        }

        private void btnDOGas2On_25_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                //Avantech.DIOEnabled = false;
                if (!AvantechDIOs.CheckControllable())
                {
                    lblDIOErr.Text = "-1";
                    return;
                }
                if (btnDOGas2On_25.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DIOSlotNum, 1 + AvantechDIOs.m_iDoOffset, bVal);
                    btnDOGas2On_25.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DIOSlotNum, 1 + AvantechDIOs.m_iDoOffset, bVal);
                    btnDOGas2On_25.BackColor = Color.Red;
                }
                //Avantech.DIOEnabled = true;
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOGas2On_25.BackColor == Color.Red)
                {
                    btnDOGas2On_25.BackColor = Color.Green;
                }
                else
                {
                    btnDOGas2On_25.BackColor = Color.Red;
                }
            }
        }

        private void btnDOTowerLightGr_26_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                //Avantech.DIOEnabled = false;
                if (!AvantechDIOs.CheckControllable())
                {
                    lblDIOErr.Text = "-1";
                    return;
                }
                if (btnDOTowerLightGr_26.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DIOSlotNum, 2 + AvantechDIOs.m_iDoOffset, bVal);
                    btnDOTowerLightGr_26.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DIOSlotNum, 2 + AvantechDIOs.m_iDoOffset, bVal);
                    btnDOTowerLightGr_26.BackColor = Color.Red;
                }
                //Avantech.DIOEnabled = true;
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOTowerLightGr_26.BackColor == Color.Red)
                {
                    btnDOTowerLightGr_26.BackColor = Color.Green;
                }
                else
                {
                    btnDOTowerLightGr_26.BackColor = Color.Red;
                }
            }
        }

        private void btnDOYellowLight_27_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                //Avantech.DIOEnabled = false;
                if (!AvantechDIOs.CheckControllable())
                {
                    lblDIOErr.Text = "-1";
                    return;
                }
                if (btnDOYellowLight_27.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DIOSlotNum, 3 + AvantechDIOs.m_iDoOffset, bVal);
                    btnDOYellowLight_27.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DIOSlotNum, 3 + AvantechDIOs.m_iDoOffset, bVal);
                    btnDOYellowLight_27.BackColor = Color.Red;
                }
                //Avantech.DIOEnabled = true;
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOYellowLight_27.BackColor == Color.Red)
                {
                    btnDOYellowLight_27.BackColor = Color.Green;
                }
                else
                {
                    btnDOYellowLight_27.BackColor = Color.Red;
                }
            }
        }

        private void btnDORedLight_28_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                //Avantech.DIOEnabled = false;
                if (!AvantechDIOs.CheckControllable())
                {
                    lblDIOErr.Text = "-1";
                    return;
                }
                if (btnDORedLight_28.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DIOSlotNum, 4 + AvantechDIOs.m_iDoOffset, bVal);
                    btnDORedLight_28.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DIOSlotNum, 4 + AvantechDIOs.m_iDoOffset, bVal);
                    btnDORedLight_28.BackColor = Color.Red;
                }
                //Avantech.DIOEnabled = true;
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDORedLight_28.BackColor == Color.Red)
                {
                    btnDORedLight_28.BackColor = Color.Green;
                }
                else
                {
                    btnDORedLight_28.BackColor = Color.Red;
                }
            }
        }

        private void btnDOBuzzer_29_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                //Avantech.DIOEnabled = false;
                if (!AvantechDIOs.CheckControllable())
                {
                    lblDIOErr.Text = "-1";
                    return;
                }
                if (btnDOBuzzer_29.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DIOSlotNum, 5 + AvantechDIOs.m_iDoOffset, bVal);
                    btnDOBuzzer_29.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DIOSlotNum, 5 + AvantechDIOs.m_iDoOffset, bVal);
                    btnDOBuzzer_29.BackColor = Color.Red;
                }
                //Avantech.DIOEnabled = true;
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOBuzzer_29.BackColor == Color.Red)
                {
                    btnDOBuzzer_29.BackColor = Color.Green;
                }
                else
                {
                    btnDOBuzzer_29.BackColor = Color.Red;
                }
            }
        }

        private void btnDOSafetySwLk_30_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                //Avantech.DIOEnabled = false;
                if (!AvantechDIOs.CheckControllable())
                {
                    lblDIOErr.Text = "-1";
                    return;
                }
                if (btnDOSafetySwLk_30.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DIOSlotNum, 6 + AvantechDIOs.m_iDoOffset, bVal);
                    btnDOSafetySwLk_30.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DIOSlotNum, 6 + AvantechDIOs.m_iDoOffset, bVal);
                    btnDOSafetySwLk_30.BackColor = Color.Red;
                }
                //Avantech.DIOEnabled = true;
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOSafetySwLk_30.BackColor == Color.Red)
                {
                    btnDOSafetySwLk_30.BackColor = Color.Green;
                }
                else
                {
                    btnDOSafetySwLk_30.BackColor = Color.Red;
                }
            }
        }

        private void btnDOStandBy_31_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                //Avantech.DIOEnabled = false;
                if (!AvantechDIOs.CheckControllable())
                {
                    lblDIOErr.Text = "-1";
                    return;
                }
                if (btnDOStandBy_31.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DIOSlotNum, 7 + AvantechDIOs.m_iDoOffset, bVal);
                    btnDOStandBy_31.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DIOSlotNum, 7 + AvantechDIOs.m_iDoOffset, bVal);
                    btnDOStandBy_31.BackColor = Color.Red;
                }
                //Avantech.DIOEnabled = true;
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOStandBy_31.BackColor == Color.Red)
                {
                    btnDOStandBy_31.BackColor = Color.Green;
                }
                else
                {
                    btnDOStandBy_31.BackColor = Color.Red;
                }
            }
        }

        private void btnDOSpare_32_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                //Avantech.DIOEnabled = false;
                if (!AvantechDIOs.CheckControllable())
                {
                    lblDIOErr.Text = "-1";
                    return;
                }
                if (btnDOSpare_32.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DIOSlotNum, 8 + AvantechDIOs.m_iDoOffset, bVal);
                    btnDOSpare_32.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DIOSlotNum, 8 + AvantechDIOs.m_iDoOffset, bVal);
                    btnDOSpare_32.BackColor = Color.Red;
                }
                //Avantech.DIOEnabled = true;
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOSpare_32.BackColor == Color.Red)
                {
                    btnDOSpare_32.BackColor = Color.Green;
                }
                else
                {
                    btnDOSpare_32.BackColor = Color.Red;
                }
            }
        }

        private void btnDOSpare_33_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                //Avantech.DIOEnabled = false;
                if (!AvantechDIOs.CheckControllable())
                {
                    lblDIOErr.Text = "-1";
                    return;
                }
                if (btnDOSpare_33.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DIOSlotNum, 9 + AvantechDIOs.m_iDoOffset, bVal);
                    btnDOSpare_33.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DIOSlotNum, 9 + AvantechDIOs.m_iDoOffset, bVal);
                    btnDOSpare_33.BackColor = Color.Red;
                }
                //Avantech.DIOEnabled = true;
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOSpare_33.BackColor == Color.Red)
                {
                    btnDOSpare_33.BackColor = Color.Green;
                }
                else
                {
                    btnDOSpare_33.BackColor = Color.Red;
                }
            }
        }

        private void btnDOSpare_34_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                //Avantech.DIOEnabled = false;
                if (!AvantechDIOs.CheckControllable())
                {
                    lblDIOErr.Text = "-1";
                    return;
                }
                if (btnDOSpare_34.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DIOSlotNum, 10 + AvantechDIOs.m_iDoOffset, bVal);
                    btnDOSpare_34.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DIOSlotNum, 10 + AvantechDIOs.m_iDoOffset, bVal);
                    btnDOSpare_34.BackColor = Color.Red;
                }
                //Avantech.DIOEnabled = true;
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOSpare_34.BackColor == Color.Red)
                {
                    btnDOSpare_34.BackColor = Color.Green;
                }
                else
                {
                    btnDOSpare_34.BackColor = Color.Red;
                }
            }
        }

        private void btnDOSpare_35_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                //Avantech.DIOEnabled = false;
                if (!AvantechDIOs.CheckControllable())
                {
                    lblDIOErr.Text = "-1";
                    return;
                }
                if (btnDOSpare_35.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DIOSlotNum, 11 + AvantechDIOs.m_iDoOffset, bVal);
                    btnDOSpare_35.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DIOSlotNum, 11 + AvantechDIOs.m_iDoOffset, bVal);
                    btnDOSpare_35.BackColor = Color.Red;
                }
                //Avantech.DIOEnabled = true;
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDOSpare_35.BackColor == Color.Red)
                {
                    btnDOSpare_35.BackColor = Color.Green;
                }
                else
                {
                    btnDOSpare_35.BackColor = Color.Red;
                }
            }
        }
        
        private void txtAO_1Val_KeyPress(object sender, KeyPressEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[1] = true;
        }

        private void txtAO_1Val_MouseDown(object sender, MouseEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[1] = true;
        }

        private void txtAO_2Val_KeyPress(object sender, KeyPressEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[2] = true;
        }

        private void txtAO_2Val_MouseDown(object sender, MouseEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[2] = true;
        }

        private void txtAO_3Val_KeyPress(object sender, KeyPressEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[3] = true;
        }

        private void txtAO_3Val_MouseDown(object sender, MouseEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[3] = true;
        }

        private void txtAO_4Val_KeyPress(object sender, KeyPressEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[4] = true;
        }

        private void txtAO_4Val_MouseDown(object sender, MouseEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[4] = true;
        }

        private void txtAO_5Val_KeyPress(object sender, KeyPressEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[5] = true;
        }

        private void txtAO_5Val_MouseDown(object sender, MouseEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[5] = true;
        }

        private void txtAO_6Val_KeyPress(object sender, KeyPressEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[6] = true;
        }

        private void txtAO_6Val_MouseDown(object sender, MouseEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[6] = true;
        }

        private void txtAO_7Val_KeyPress(object sender, KeyPressEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[7] = true;
        }

        private void txtAO_7Val_MouseDown(object sender, MouseEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[7] = true;
        }

        // Only related to AO
        /// </summary>
        /// <returns></returns>
        private bool RefreshAOData()
        {
            int iChannelTotal = AvantechAOs.m_aConf.HwIoTotal[AvantechAOs.m_tmpidx];
            ushort[] usVal;
            string[] strVal;
            //float fHigh = 0, fLow = 0;
            float[] fHighVals = new float[iChannelTotal];
            float[] fLowVals = new float[iChannelTotal];

            if (!AvantechAOs.m_adamSocket.AnalogOutput().GetValues(AvantechAOs.m_idxID, iChannelTotal, out usVal))
            {
                //StatusBar_IO.Text += "ApiErr:" + m_adamSocket.Modbus().LastError.ToString() + " ";
                return false;
            }
            strVal = new string[usVal.Length];

            for (int i = 0; i < iChannelTotal; i++)
            {
                if (AvantechAOs.IsShowRawData)
                    strVal[i] = usVal[i].ToString("X04");
                else
                    strVal[i] = AnalogOutput.GetScaledValue(AvantechAOs.m_usRanges[i], usVal[i]).ToString(AnalogOutput.GetFloatFormat(AvantechAOs.m_usRanges[i]));
                Class1.AOOutputVals[i].Text = strVal[i].ToString();//listViewChInfo.Items[i].SubItems[3].Text = strVal[i].ToString();  //moduify "Value" column
            }

            //Update tBarOutputVal
            for (int j = 0; j < iChannelTotal; j++)
            {
                if (!AvantechAOs.m_bAOValueModified[j])
                {
                    AnalogOutput.GetRangeHighLow(AvantechAOs.m_usRanges[j], out fHighVals[j], out fLowVals[j]);
                    RefreshAnalogOutputPanel(fHighVals[j], fLowVals[j],
                        AnalogOutput.GetScaledValue(AvantechAOs.m_usRanges[j], usVal[j]), j);
                    //RefreshOutputPanel(fHighVals[j], fLowVals[j], AnalogOutput.GetScaledValue(m_usRanges[j], usVal[j]));
                }
            }

            return true;
        }

        private void RefreshAnalogOutputPanel(float fHigh, float fLow, float fOutputVal, int idx)
        {
            Class1.AOHighVals[idx].Text = fHigh.ToString();
            Class1.AOLowVals[idx].Text = fLow.ToString();
            Class1.txtAOOutputVals[idx].Text = fOutputVal.ToString("0.000");// textbox refresh
            Class1.AOOutputVals[idx].Text = fOutputVal.ToString("0.000");// output label refresh
            Class1.AOTrackBarVals[idx].Value = Convert.ToInt32(Class1.AOTrackBarVals[idx].Minimum + (Class1.AOTrackBarVals[idx].Maximum - Class1.AOTrackBarVals[idx].Minimum) * (fOutputVal - fLow) / (fHigh - fLow));
        }

        private void SetAO(int idx)
        {
            AvantechAOs.m_bAOValueModified[idx] = false;// b_AOValueModified = false;
            if (!AvantechAOs.CheckControllable())
            {
                lblAOErr.Text = "-1";
                return;
            }

            Avantech.AOEnabled = false;// timer1.Enabled = false;
            float fVal, fHigh, fLow;
            if (Class1.txtAOOutputVals[idx].Text.Length == 0)
            {
                MessageBox.Show("Illegal value!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                return;
            }

            try
            {
                //Get range higf & low
                AnalogOutput.GetRangeHighLow(AvantechAOs.m_usRanges[idx], out fHigh, out fLow);
                if (fHigh - fLow == 0)
                {
                    MessageBox.Show("GetRangeHighLow() failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    return;
                }
                //convert output value to float
                fVal = 0.0f;
                if (Class1.txtAOOutputVals[idx].Text != null && Class1.txtAOOutputVals[idx].Text.Length > 0)
                {
                    try
                    {
                        fVal = Convert.ToSingle(Class1.txtAOOutputVals[idx].Text);
                    }
                    catch
                    {
                        System.Windows.Forms.MessageBox.Show("Invalid value: " + Class1.txtAOOutputVals[idx].Text);
                    }
                }
                if (fVal > fHigh || fVal < fLow)
                {
                    MessageBox.Show("Illegal value! Please enter the value " + fLow.ToString() + " ~ " + fHigh.ToString() + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    return;
                }
                //Set channel value
                if (AvantechAOs.m_adamSocket.AnalogOutput().SetCurrentValue(AvantechAOs.m_idxID, idx, AvantechAOs.m_usRanges[idx], fVal))
                {
                    //RefreshOutputPanel(fHigh, fLow, fVal);
                    RefreshAnalogOutputPanel(fHigh, fLow, fVal, idx);// refresh special index of analog output panel

                    // Same functionality of setting values
                    //lblAO_0High.Text = fHigh.ToString();
                    //lblAO_0Low.Text = fLow.ToString();
                    //txtAO_0Val.Text = fVal.ToString("0.000");
                    //lblAO_0Value.Text = fVal.ToString("0.000");
                }
                else
                {
                    MessageBox.Show("Change current value failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            RefreshAOData();//RefreshData();
            string strInfo = string.Format("Set output AO_{0} value done!", idx);
            MessageBox.Show(strInfo, "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);

            Avantech.AOEnabled = true;//timer1.Enabled = true;
        }

      


        private void btnAO_0ApplyOutput_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                //AvantechAOs.m_bAOValueModified[0] = false;
                //if (!AvantechAOs.CheckControllable())
                //    return;

                //// timer1.Enabled = false;
                //float fVal, fHigh, fLow;
                //if (txtAOOutputVals[0].Text.Length == 0)
                //{
                //    MessageBox.Show("Illegal value!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                //    return;
                //}

                //try
                //{
                //    //Get range higf & low
                //    AnalogOutput.GetRangeHighLow(AvantechAOs.m_usRanges[0], out fHigh, out fLow);
                //    if (fHigh - fLow == 0)
                //    {
                //        MessageBox.Show("GetRangeHighLow() failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                //        return;
                //    }
                //    //convert output value to float
                //    fVal = 0.0f;
                //    if (txtAOOutputVals[0].Text != null && txtAOOutputVals[0].Text.Length > 0)
                //    {
                //        try
                //        {
                //            fVal = Convert.ToSingle(txtAOOutputVals[0].Text);
                //        }
                //        catch
                //        {
                //            System.Windows.Forms.MessageBox.Show("Invalid value: " + txtAOOutputVals[0].Text);
                //        }
                //    }
                //    if (fVal > fHigh || fVal < fLow)
                //    {
                //        MessageBox.Show("Illegal value! Please enter the value " + fLow.ToString() + " ~ " + fHigh.ToString() + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                //        return;
                //    }
                //    //Set channel value
                //    if (AvantechAOs.m_adamSocket.AnalogOutput().SetCurrentValue(AvantechAOs.m_idxID, 0, AvantechAOs.m_usRanges[0], fVal))
                //    {
                //        //RefreshOutputPanel(fHigh, fLow, fVal);
                //        RefreshAnalogOutputPanel(fHigh, fLow, fVal, 0);

                //        // Same functionality of setting values
                //        //lblAO_0High.Text = fHigh.ToString();
                //        //lblAO_0Low.Text = fLow.ToString();
                //        //txtAO_0Val.Text = fVal.ToString("0.000");
                //        //lblAO_0Value.Text = fVal.ToString("0.000");
                //    }
                //    else
                //    {
                //        MessageBox.Show("Change current value failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                //        return;
                //    }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.ToString());
                //    return;
                //}
                //RefreshAOData();//RefreshData();
                //MessageBox.Show("Set output AO_0 value done!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);

                ////timer1.Enabled = true;
                SetAO(0);
            }
            else
            {
                MessageBox.Show("Modbus connection error!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnAO_1ApplyOutput_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                SetAO(1);
            }
            else
            {
                MessageBox.Show("Modbus connection error!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnAO_2ApplyOutput_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                SetAO(2);
            }
            else
            {
                MessageBox.Show("Modbus connection error!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnAO_3ApplyOutput_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                SetAO(3);
            }
            else
            {
                MessageBox.Show("Modbus connection error!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnAO_4ApplyOutput_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                SetAO(4);
            }
            else
            {
                MessageBox.Show("Modbus connection error!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnAO_5ApplyOutput_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                SetAO(5);
            }
            else
            {
                MessageBox.Show("Modbus connection error!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnAO_6ApplyOutput_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                SetAO(6);
            }
            else
            {
                MessageBox.Show("Modbus connection error!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnAO_7ApplyOutput_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                SetAO(7);
            }
            else
            {
                MessageBox.Show("Modbus connection error!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnAOApplySelRange_Click(object sender, EventArgs e)
        {
            DialogResult result;
            if (!AvantechAOs.CheckControllable())
                return;
            Avantech.AOEnabled = false;//timer1.Enabled = false;

            result = MessageBox.Show("After changing range setting, you need to configure proper start-up value again!", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                bool bRet = true;
                bool i_bApplyAll = chbxAOApplyAll.Checked;
                //if (listViewChInfo.SelectedIndices.Count == 0 && !i_bApplyAll)
                //{
                //    MessageBox.Show("Please select the target channel in the listview!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                //    bRet = false;
                //}

                // if no selection, Messagebox out
                if (((!chbxRFSET_0Range.Checked) && (!chbxTNSET_1Range.Checked) && (!chbxLDSET_2Range.Checked) && (!chbxG1SET_3Range.Checked)
                    && (!chbxG2SET_4Range.Checked) && (!chbxSPARE_5Range.Checked) && (!chbxSPARE_6Range.Checked) && (!chbxSPARE_7Range.Checked)) && !i_bApplyAll)
                {
                    MessageBox.Show("Please select the target channel in the listview!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    bRet = false;
                }
                if (bRet)
                {
                    ushort[] usRanges = new ushort[AvantechAOs.m_usRanges.Length];
                    int iChannelTotal = AvantechAOs.m_aConf.HwIoTotal[AvantechAOs.m_tmpidx];
                    ushort usVal;
                    Array.Copy(AvantechAOs.m_usRanges, 0, usRanges, 0, AvantechAOs.m_usRanges.Length);
                    if (i_bApplyAll)// all selected
                    {
                        for (int i = 0; i < usRanges.Length; i++)
                        {
                            usRanges[i] = AnalogOutput.GetRangeCode2Byte(cbxAORange.SelectedItem.ToString());
                            //// Set Channel value to 0 to be safe first when change range  (Disable this for purpose use only!!!)
                            //if (!AvantechAOs.m_adamSocket.AnalogOutput().SetCurrentValue(AvantechAOs.m_idxID, i, AvantechAOs.m_usRanges[i], 0))
                            //{
                            //    //RefreshAnalogOutputPanel(fHigh, fLow, fVal, idx);// refresh special index of analog output panel
                            //    MessageBox.Show("Set channel default value 0 failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                            //    return;
                            //}
                        }
                    }
                    else// only some of them selected
                    {
                        //for (int i = 0; i < listViewChInfo.SelectedIndices.Count; i++)
                        //{
                        //    usRanges[listViewChInfo.SelectedIndices[i]] = AnalogOutput.GetRangeCode2Byte(cbxRange.SelectedItem.ToString());
                        //}

                        for (int i = 0; i < iChannelTotal; i++)
                        {
                            if (Class1.AOChkRanges[i].Checked) // means that it is selected, otherwise skip it
                            {
                                usRanges[i] = AnalogOutput.GetRangeCode2Byte(cbxAORange.SelectedItem.ToString());
                                //// Set Channel value to 0 to be safe first when change range  (Disable this for purpose use only !!!)
                                //if (!AvantechAOs.m_adamSocket.AnalogOutput().SetCurrentValue(AvantechAOs.m_idxID, i, AvantechAOs.m_usRanges[i], 0))
                                //{
                                //    //RefreshAnalogOutputPanel(fHigh, fLow, fVal, idx);// refresh special index of analog output panel
                                //    MessageBox.Show("Set channel default value 0 failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                                //    return;
                                //}
                            }
                        }
                    }
                    
                    if (AvantechAOs.m_adamSocket.AnalogOutput().SetRanges(AvantechAOs.m_idxID, iChannelTotal, usRanges))
                    {
                        MessageBox.Show("Set ranges done!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                        AvantechAOs.RefreshRanges();
                        // Update Range column
                        //for (int i = 0; i < iChannelTotal; i++)
                        //    listViewChInfo.Items[i].SubItems[4].Text = AnalogOutput.GetRangeName(m_usRanges[i]).ToString();

                        // Refresh low and high range vals of AO after applying new kind of ranges
                        for (int idx = 0; idx < iChannelTotal; idx++)
                        {
                            if (AvantechAOs.m_adamSocket.AnalogOutput().GetCurrentValue(AvantechAOs.m_idxID, idx, out usVal))// m_idxID is SlotNum(its the index ID of the 5 I/O modules)
                            {
                                AnalogOutput.GetRangeHighLow(AvantechAOs.m_usRanges[idx], out AvantechAOs.m_fHighVals[idx], out AvantechAOs.m_fLowVals[idx]);
                                AvantechAOs.m_fOutputVals[idx] = AnalogOutput.GetScaledValue(AvantechAOs.m_usRanges[idx], usVal);
                                //RefreshOutputPanel(m_fHighVal[idx], m_fLowVal[idx], m_fOutputVal[idx]);// use the High, Low and Output Vals to update UI correspondingly
                                RefreshAnalogOutputPanel(AvantechAOs.m_fHighVals[idx], AvantechAOs.m_fLowVals[idx],
                                    AvantechAOs.m_fOutputVals[idx], idx);
                            }
                            else
                                AvantechAOs.StatusBar_IO += "GetValues() filed!";//this.StatusBar_IO.Text += "GetValues() filed!";
                        }
                        AvantechAOs.RefreshAoStartupValues();
                    }
                    else
                    {
                        MessageBox.Show("Set ranges failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    }

                }

            }
            Avantech.AOEnabled = true;//timer1.Enabled = true;
        }

        private void btnAOSetSafetyValue_Click(object sender, EventArgs e)
        {
            if (!AvantechAOs.CheckControllable())
                return;

            Avantech.AOEnabled = false;//timer1.Enabled = false;
            int iChannelTotal = AvantechAOs.m_aConf.HwIoTotal[AvantechAOs.m_tmpidx];
            float[] fAOSafetyVals = new float[iChannelTotal];

            for (int i = 0; i < iChannelTotal; i++)
                fAOSafetyVals[i] = AnalogOutput.GetScaledValue(AvantechAOs.m_usRanges[i], AvantechAOs.m_usAOSafetyVals[i]);

            string[] szRanges = new string[iChannelTotal];

            for (int idx = 0; idx < szRanges.Length; idx++)
                szRanges[idx] = AnalogInput.GetRangeName(AvantechAOs.m_usRanges[idx]);

            AO_FormSafetySetting AO_formSafety = new AO_FormSafetySetting(iChannelTotal, fAOSafetyVals, szRanges);
            AO_formSafety.ApplySafetyValueClick += new AO_FormSafetySetting.EventHandler_ApplySafetyValueClick(AO_FormSafety_ApplySafetyValueClick);

            AO_formSafety.ShowDialog();
            AO_formSafety.Dispose();
            AO_formSafety = null;

            Avantech.AOEnabled = true;//timer1.Enabled = true;
        }

        /// <summary>
        ///  Apply setting when user configure safety status
        /// </summary>
        /// <param name="bVal"></param>
        private void AO_FormSafety_ApplySafetyValueClick(string[] szVal)
        {
            int iChannelTotal = AvantechAOs.m_aConf.HwIoTotal[AvantechAOs.m_tmpidx];
            float fVal, fHigh, fLow;
            ushort[] usAOSafetyVals = new ushort[AvantechAOs.m_usAOSafetyVals.Length];
            for (int i = 0; i < iChannelTotal; i++)
            {
                fVal = 0.0f;
                if (szVal[i] != null && szVal[i].Length > 0)
                {
                    try
                    {
                        fVal = Convert.ToSingle(szVal[i]);
                    }
                    catch
                    {
                        System.Windows.Forms.MessageBox.Show("Invalid value: " + szVal[i]);
                    }
                }

                AnalogOutput.GetRangeHighLow(AvantechAOs.m_usRanges[i], out fHigh, out fLow);

                if (fHigh - fLow == 0)
                {
                    MessageBox.Show("GetRangeHighLow() failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    return;
                }

                if (fVal > fHigh || fVal < fLow)
                {
                    MessageBox.Show("Channel " + i.ToString() + " is illegal value! Please enter the value " + fLow.ToString() + " ~ " + fHigh.ToString() + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    return;
                }

                usAOSafetyVals[i] = Convert.ToUInt16(65535.0f * ((fVal - fLow) / (fHigh - fLow)));
            }

            if (!AvantechAOs.m_adamSocket.AnalogOutput().SetSafetyValues(AvantechAOs.m_idxID, iChannelTotal, usAOSafetyVals))
                MessageBox.Show("Set safety value failed! (Err: " + AvantechAOs.m_adamSocket.Modbus().LastError.ToString() + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);

            AvantechAOs.RefreshSafetySetting();

        }

        private void chbxAOEnableSafety_CheckedChanged(object sender, EventArgs e)
        {
            btnAOSetAsSafety.Enabled = chbxAOEnableSafety.Checked;
            btnAOSetSafetyValue.Enabled = chbxAOEnableSafety.Checked;
            if (!AvantechAOs.CheckControllable())
                return;

            if (!AvantechAOs.m_adamSocket.Configuration().OUT_SetSafetyEnable(AvantechAOs.m_idxID, chbxAOEnableSafety.Checked))
                MessageBox.Show("Set safety function failed! (Err: " + AvantechAOs.m_adamSocket.Modbus().LastError.ToString() + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);

            AvantechAOs.RefreshSafetySetting();
        }

        // Set Startup value(when power off and on again, the output value use this value
        private void btnAOSetAsStartup_Click(object sender, EventArgs e)
        {
            if (!AvantechAOs.CheckControllable())
                return;
            float fVal, fHigh, fLow;
            int iChannelTotal = AvantechAOs.m_aConf.HwIoTotal[AvantechAOs.m_tmpidx];
            ushort[] usStartupAO = new ushort[AvantechAOs.m_usStartupAO.Length];

            Avantech.AOEnabled = false;//timer1.Enabled = false;
            if (((!chbxRFSET_0Range.Checked) && (!chbxTNSET_1Range.Checked) && (!chbxLDSET_2Range.Checked) && (!chbxG1SET_3Range.Checked)
                && (!chbxG2SET_4Range.Checked) && (!chbxSPARE_5Range.Checked) && (!chbxSPARE_6Range.Checked) && (!chbxSPARE_7Range.Checked)))
            {
                MessageBox.Show("Please select the target channel in the listview!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                return;
            }

            try
            {
                for (int i = 0; i < iChannelTotal; i++)
                {
                    if (Class1.AOChkRanges[i].Checked) // if it is selected
                    {
                        if (Class1.txtAOOutputVals[i].Text.Length == 0)
                            {
                                MessageBox.Show("Illegal value!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                                return;
                            }

                            //Get range higf & low
                            AnalogOutput.GetRangeHighLow(AvantechAOs.m_usRanges[i], out fHigh, out fLow);
                            if (fHigh - fLow == 0)
                            {
                                MessageBox.Show("GetRangeHighLow() failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                                return;
                            }
                            //convert output value to float
                            fVal = 0.0f;
                            if (Class1.txtAOOutputVals[i].Text != null && Class1.txtAOOutputVals[i].Text.Length > 0)
                            {
                                try
                                {
                                    fVal = Convert.ToSingle(Class1.txtAOOutputVals[i].Text);
                                }
                                catch
                                {
                                    System.Windows.Forms.MessageBox.Show("Invalid value: " + Class1.txtAOOutputVals[i].Text);
                                }
                            }
                            if (fVal > fHigh || fVal < fLow)
                            {
                                MessageBox.Show("Illegal value! Please enter the value " + fLow.ToString() + " ~ " + fHigh.ToString() + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                                return;
                            }
                            
                            Array.Copy(AvantechAOs.m_usStartupAO, 0, usStartupAO, 0, AvantechAOs.m_usStartupAO.Length);
                            usStartupAO[i] = Convert.ToUInt16(65535.0f * ((fVal - fLow) / (fHigh - fLow)));
                    }
                }

                if (AvantechAOs.m_adamSocket.AnalogOutput().SetStartupValues(AvantechAOs.m_idxID, usStartupAO))
                {
                    MessageBox.Show("Set AO startup values done!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    AvantechAOs.RefreshAoStartupValues();
                }
                else
                {
                    MessageBox.Show("Set AO startup values failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                }
            }
            catch
            {
                MessageBox.Show("Illegal value!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                return;
            }
            Avantech.AOEnabled = true;//timer1.Enabled = true;
        }

        private void btnAOSetAsSafety_Click(object sender, EventArgs e)// seems has some problems, need to check(not multiple selections?)
        {
            if (!AvantechAOs.CheckControllable())
                return;

            if (((!chbxRFSET_0Range.Checked) && (!chbxTNSET_1Range.Checked) && (!chbxLDSET_2Range.Checked) && (!chbxG1SET_3Range.Checked)
                 && (!chbxG2SET_4Range.Checked) && (!chbxSPARE_5Range.Checked) && (!chbxSPARE_6Range.Checked) && (!chbxSPARE_7Range.Checked)))
            {
                MessageBox.Show("Please select the target channel in the listview!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                return;
            }

            try
            {
                float fVal, fHigh, fLow;
                int iChannelTotal = AvantechAOs.m_aConf.HwIoTotal[AvantechAOs.m_tmpidx];
                ushort[] usAOSafetyVals = new ushort[AvantechAOs.m_usAOSafetyVals.Length];
                //Get range higf & low
                for (int i = 0; i < iChannelTotal; i++)
                {
                    if (Class1.AOChkRanges[i].Checked)// if it is selected
                    {
                        AnalogOutput.GetRangeHighLow(AvantechAOs.m_usRanges[i], out fHigh, out fLow);
                        if (fHigh - fLow == 0)
                        {
                            MessageBox.Show("GetRangeHighLow() failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                            return;
                        }
                        //convert output value to float
                        fVal = 0.0f;
                        if (Class1.txtAOOutputVals[i].Text != null && Class1.txtAOOutputVals[i].Text.Length > 0)
                        {
                            try
                            {
                                fVal = Convert.ToSingle(Class1.txtAOOutputVals[i].Text);
                            }
                            catch
                            {
                                System.Windows.Forms.MessageBox.Show("Invalid value: " + Class1.txtAOOutputVals[i].Text);
                            }
                        }
                        
                        //Array.Copy(AvantechAOs.m_usAOSafetyVals, 0, usAOSafetyVals, 0, AvantechAOs.m_usAOSafetyVals.Length);

                        usAOSafetyVals[i] = Convert.ToUInt16(65535.0f * ((fVal - fLow) / (fHigh - fLow)));

                        AvantechAOs.m_usAOSafetyVals[i] = usAOSafetyVals[i];
                    }
                }

                //Array.Copy(AvantechAOs.m_usAOSafetyVals, 0, usAOSafetyVals, 0, AvantechAOs.m_usAOSafetyVals.Length);

                if (!AvantechAOs.m_adamSocket.AnalogOutput().SetSafetyValues(AvantechAOs.m_idxID, iChannelTotal, usAOSafetyVals))
                    MessageBox.Show("Set safety value failed! (Err: " + AvantechAOs.m_adamSocket.Modbus().LastError.ToString() + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                else
                    MessageBox.Show("Set safety value done!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);

                AvantechAOs.RefreshSafetySetting();

            }
            catch
            {
                return;
            }
            return;
        }

        private void chbxDOEnableSafety_CheckedChanged(object sender, EventArgs e)
        {
            btnDOSetSafetyValue.Enabled = chbxDOEnableSafety.Checked;
            if (!AvantechDOs.CheckControllable())
                return;

            if (!AvantechDOs.m_adamSocket.Configuration().OUT_SetSafetyEnable(AvantechDOs.m_idxID, chbxDOEnableSafety.Checked))
                MessageBox.Show("Set safety function failed! (Err: " + AvantechDOs.m_adamSocket.Modbus().LastError.ToString() + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);

            AvantechDOs.RefreshSafetySetting();
        }

        private void chbxDIO_DOEnableSafety_CheckedChanged(object sender, EventArgs e)
        {
            btnDIO_DOSetSafetyValue.Enabled = chbxDIO_DOEnableSafety.Checked;
            if (!AvantechDIOs.CheckControllable())
                return;

            if (!AvantechDIOs.m_adamSocket.Configuration().OUT_SetSafetyEnable(AvantechDIOs.m_idxID, chbxDIO_DOEnableSafety.Checked))
                MessageBox.Show("Set safety function failed! (Err: " + AvantechDIOs.m_adamSocket.Modbus().LastError.ToString() + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);

            AvantechDIOs.RefreshSafetySetting();
        }

        private void btnDOSetSafetyValue_Click(object sender, EventArgs e)
        {
            if (!AvantechDOs.CheckControllable())
                return;

            //timer1.Enabled = false;

            int iChannelTotal = AvantechDOs.m_aConf.HwIoTotal[AvantechDOs.m_tmpidx];
            DO_FormSafetySetting DO_formSafety = new DO_FormSafetySetting(iChannelTotal, AvantechDOs.m_bDOSafetyVals);
            DO_formSafety.ApplySafetyValueClick += new DO_FormSafetySetting.EventHandler_ApplySafetyValueClick(DO_FormSafety_ApplySafetyValueClick);

            DO_formSafety.ShowDialog();
            DO_formSafety.Dispose();
            DO_formSafety = null;
            //timer1.Enabled = true;
        }

        /// <summary>
        ///  Apply setting when user configure safety status
        /// </summary>
        /// <param name="bVal"></param>
        private void DO_FormSafety_ApplySafetyValueClick(bool[] bVal)
        {
            if (!AvantechDOs.m_adamSocket.DigitalOutput().SetSafetyValues(AvantechDOs.m_idxID, bVal))
                MessageBox.Show("Set safety value failed! (Err: " + AvantechDOs.m_adamSocket.Modbus().LastError.ToString() + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
            else
                MessageBox.Show("Set safety value ok!", "Info");
            AvantechDOs.RefreshSafetySetting();
        }

        private void btnDIO_DOSetSafetyValue_Click(object sender, EventArgs e)
        {
            if (!AvantechDIOs.CheckControllable())
                return;

            //timer1.Enabled = false;

            int iChannelTotal = AvantechDIOs.m_aConf.HwIoTotal[AvantechDIOs.m_DOidx];

            DIO_FormSafetySetting DIO_FormSafety = new DIO_FormSafetySetting(iChannelTotal, AvantechDIOs.m_bDOSafetyVals);
            DIO_FormSafety.ApplySafetyValueClick += new DIO_FormSafetySetting.EventHandler_ApplySafetyValueClick(DIO_FormSafety_ApplySafetyValueClick);

            DIO_FormSafety.ShowDialog();
            DIO_FormSafety.Dispose();
            DIO_FormSafety = null;
            //timer1.Enabled = true;
        }

        /// <summary>
        ///  Apply setting when user configure safety status
        /// </summary>
        /// <param name="bVal"></param>
        private void DIO_FormSafety_ApplySafetyValueClick(bool[] bVal)
        {
            if (!AvantechDIOs.m_adamSocket.DigitalOutput().SetSafetyValues(AvantechDIOs.m_idxID, bVal))
                MessageBox.Show("Set safety value failed! (Err: " + AvantechDIOs.m_adamSocket.Modbus().LastError.ToString() + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
            else
                MessageBox.Show("Set safety value ok!", "Info");
            AvantechDIOs.RefreshSafetySetting();
        }

        private void btnDIApplySetting_Click(object sender, EventArgs e)
        {
            uint uiWidth = 10;
            bool bDI = chkBoxDIFilterEnable.Checked;
            string strCntMin = txtDICntMin.Text;
            uiWidth = Convert.ToUInt32(strCntMin);
            if (uiWidth > AvantechDIs.DI_FILTER_WIDTH_MAX || uiWidth < AvantechDIs.DI_FILTER_WIDTH_MIN)
            {
                MessageBox.Show("Error: Illegal parameter. The range of Di filter width is " + AvantechDIs.DI_FILTER_WIDTH_MIN.ToString() + "~" + AvantechDIs.DI_FILTER_WIDTH_MAX + " (0.1ms).\nAnd the width value have to be multiple of 5.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                return;
            }
            if (AvantechDIs.m_adamSocket.DigitalInput().SetDigitalFilterMiniSignalWidth(AvantechDIs.m_idxID, uiWidth, bDI))
            {
                if (uiWidth % 5 == 0)
                    MessageBox.Show("Set digital filter width done!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                else
                    MessageBox.Show("Set digital filter width done!\nThe width value have to be multiple of 5.\n (" + uiWidth.ToString() + " => " + Convert.ToString(uiWidth - uiWidth % 5) + ")", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
            }
            else
            {
                MessageBox.Show("Set digital filter width failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);

                return;
            }
        }

        private void btnDIO_DIApplySetting_Click(object sender, EventArgs e)
        {
            uint uiWidth = 10;
            bool bDI = chkBoxDIO_DIFilterEnable.Checked;
            string strCntMin = txtDIO_DICntMin.Text;
            uiWidth = Convert.ToUInt32(strCntMin);
            if (uiWidth > AvantechDIOs.DI_FILTER_WIDTH_MAX || uiWidth < AvantechDIOs.DI_FILTER_WIDTH_MIN)
            {
                MessageBox.Show("Error: Illegal parameter. The range of Di filter width is " + AvantechDIOs.DI_FILTER_WIDTH_MIN.ToString() + "~" + AvantechDIOs.DI_FILTER_WIDTH_MAX + " (0.1ms).\nAnd the width value have to be multiple of 5.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                return;
            }
            if (AvantechDIOs.m_adamSocket.DigitalInput().SetDigitalFilterMiniSignalWidth(AvantechDIOs.m_idxID, uiWidth, bDI))
            {
                if (uiWidth % 5 == 0)
                    MessageBox.Show("Set digital filter width done!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                else
                    MessageBox.Show("Set digital filter width done!\nThe width value have to be multiple of 5.\n (" + uiWidth.ToString() + " => " + Convert.ToString(uiWidth - uiWidth % 5) + ")", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
            }
            else
            {
                MessageBox.Show("Set digital filter width failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);

                return;
            }
        }

        private void btnAIApplySelRange_Click(object sender, EventArgs e)
        {
            if (!AvantechAIs.CheckControllable())
                return;
            Avantech.AIEnabled = false;//timer1.Enabled = false;

            bool bRet = true;
            if (((!chbxAI_0Range.Checked) && (!chbxAI_1Range.Checked) && (!chbxAI_2Range.Checked) && (!chbxAI_3Range.Checked)
                && (!chbxAI_4Range.Checked) && (!chbxAI_5Range.Checked) && (!chbxAI_6Range.Checked) && (!chbxAI_7Range.Checked)
                && (!chbxAI_8Range.Checked) && (!chbxAI_9Range.Checked) && (!chbxAI_10Range.Checked) && (!chbxAI_11Range.Checked)) && !chbxAIApplyAll.Checked)
            {
                MessageBox.Show("Please select the target channel in the listview!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                bRet = false;
            }
            if (bRet)
            {
                int iChannelTotal = AvantechAIs.m_aConf.HwIoTotal[AvantechAIs.m_tmpidx];
                ushort[] usRanges = new ushort[AvantechAIs.m_usRanges.Length];
                Array.Copy(AvantechAIs.m_usRanges, 0, usRanges, 0, AvantechAIs.m_usRanges.Length);
                if (chbxAIApplyAll.Checked)
                {
                    for (int i = 0; i < usRanges.Length; i++)
                    {
                        usRanges[i] = AnalogInput.GetRangeCode2Byte(cbxAIRange.SelectedItem.ToString());
                    }
                }
                else// only some of them selected
                {
                    //for (int i = 0; i < listViewChInfo.SelectedIndices.Count; i++)
                    //{
                    //    usRanges[listViewChInfo.SelectedIndices[i]] = AnalogInput.GetRangeCode2Byte(cbxRange.SelectedItem.ToString());
                    //}
                    for(int i = 0; i < iChannelTotal; i++)
                    {
                        if(AIChkRanges[i].Checked)
                        {
                            usRanges[i] = AnalogInput.GetRangeCode2Byte(cbxAIRange.SelectedItem.ToString());
                        }
                    }
                }
                if (AvantechAIs.m_adamSocket.AnalogInput().SetRanges(AvantechAIs.m_idxID, iChannelTotal, usRanges))
                {
                    AvantechAIs.RefreshRanges();
                }
                else
                {
                    MessageBox.Show("Set ranges failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                }
            }
            Avantech.AIEnabled = true;//timer1.Enabled = true;
        }

        private void btnAIBurnoutFcn_Click(object sender, EventArgs e)
        {
            if (!AvantechAIs.CheckControllable())
                return;
            Avantech.AIEnabled = false;//timer1.Enabled = false;
            int iChannelTotal = AvantechAIs.m_aConf.HwIoTotal[AvantechAIs.m_tmpidx];
            if (chbxAIApplyAll.Checked)
            {               
                if (chkAIBurnoutFcn.Checked)
                    AvantechAIs.m_uiBurnoutMask = (uint)(0x1 << iChannelTotal) - 1;
                else
                    AvantechAIs.m_uiBurnoutMask = 0x0;
            }
            else
            {
                int idx = 0;
                //for (int i = 0; i < listViewChInfo.Items.Count; i++)
                //{
                //    if (listViewChInfo.Items[i].Selected)
                //    {
                //        idx = i;
                //        break;
                //    }
                //}
                for (int i = 0; i < iChannelTotal; i++)
                {
                    if (Class1.AOChkRanges[i].Checked) // means that it is selected, otherwise skip it
                    {
                        idx = i;
                        break;
                    }
                }

                uint uiMask = (uint)(0x1 << idx);
                if (chkAIBurnoutFcn.Checked)
                    AvantechAIs.m_uiBurnoutMask |= uiMask;
                else
                    AvantechAIs.m_uiBurnoutMask &= ~uiMask;
            }
            if (AvantechAIs.m_adamSocket.AnalogInput().SetBurnoutFunEnable(AvantechAIs.m_idxID, AvantechAIs.m_uiBurnoutMask))
            {
                MessageBox.Show("Set burnout enable function done!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                AvantechAIs.RefreshBurnoutSetting(true, false); //refresh burnout mask value
            }
            else
                MessageBox.Show("Set burnout enable function failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
            Avantech.AIEnabled = true;//timer1.Enabled = true;
        }

        private void btnAIBurnoutValue_Click(object sender, EventArgs e)
        {
            uint uiVal;
            if (cbxAIBurnoutValue.SelectedIndex == 0)
                uiVal = 0;
            else
                uiVal = 0xFFFF;
            if (!AvantechAIs.CheckControllable())
                return;
            Avantech.AIEnabled = false;//timer1.Enabled = false;
            if (AvantechAIs.m_adamSocket.AnalogInput().SetBurnoutValue(AvantechAIs.m_idxID, uiVal))
            {
                MessageBox.Show("Set burnout value done!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                AvantechAIs.RefreshBurnoutSetting(false, true);     //refresh burnout detect mode
            }
            else
                MessageBox.Show("Set burnout value failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
            Avantech.AIEnabled = true;//timer1.Enabled = true;
        }

        private void btnAISampleRate_Click(object sender, EventArgs e)
        {
            int iIdx = cbxAISampleRate.SelectedIndex;
            if (!AvantechAIs.CheckControllable())
                return;
            Avantech.AIEnabled = false;//timer1.Enabled = false;

            uint uiRate;

            if (AvantechAIs.m_aConf.GetModuleName() == "5017")
            {
                if (iIdx == 0)
                    uiRate = 1;
                else
                    uiRate = 10;
            }
            else //if (m_aConf.GetModuleName() == "5017H")
            {
                if (iIdx == 0)
                    uiRate = 100;
                else
                    uiRate = 1000;
            }
            if (AvantechAIs.m_adamSocket.AnalogInput().SetSampleRate(AvantechAIs.m_idxID, uiRate))
            {
                MessageBox.Show("Set sampling rate done!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                AvantechAIs.RefreshAiSampleRate();
            }
            else
                MessageBox.Show("Set sampling rate failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);

            Avantech.AIEnabled = true;//timer1.Enabled = true;
        }

           
        private void chbxAIApplyAll_CheckedChanged(object sender, EventArgs e)
        {
            int iChannelTotal = AvantechAIs.m_aConf.HwIoTotal[AvantechAIs.m_tmpidx];
            for (int i = 0; i < iChannelTotal; i++)
            {
                AIChkRanges[i].Checked = chbxAIApplyAll.Checked;
            }
        }

        private void chbxAOApplyAll_CheckedChanged(object sender, EventArgs e)
        {
            int iChannelTotal = AvantechAOs.m_aConf.HwIoTotal[AvantechAOs.m_tmpidx];
            for (int i = 0; i < iChannelTotal; i++)
            {
                Class1.AOChkRanges[i].Checked = chbxAOApplyAll.Checked;
            }
        }

        private void cmdSystemSettings_Click(object sender, EventArgs e)
        {
            //SystemSettingsPage systemSettingsPage = new SystemSettingsPage();
            //systemSettingsPage.ShowDialog();
        }

        private void cmdSystemToDefault_Click(object sender, EventArgs e)
        {
           if (Avantech.bModbusConnected)
            {
                MessageBox.Show("Modbus is already connected, no need to restore to default!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }
            else
            {
                // Load System DB Settings
                LoadSystemSetting_DB();

                AvantechSystemInitialize();
                if (Avantech.bModbusConnected)
                {
                    DisconnectErrorOK = false;
                    lblResumeConnectionInfo.Text = "";
                    // Connection status
                    txtConnection.Text = "OnLine";
                    cmdPWR.BackColor = Color.Green;
                    cmdIO.BackColor = Color.Green;
                    // Corresponding UI enabled
                    gbDI.Enabled = true;
                    gbDO.Enabled = true;
                    gbAO.Enabled = true;
                    gbAI.Enabled = true;
                    gbAORangeSettings.Enabled = true;
                    gbAOSafetyFunction.Enabled = true;

                    // All IOs initialization
                    AvantechIO_Modules_Initialize();

                    // Testing Output Split
                    DataAcquitionTimer.Enabled = true;

                    try
                    {
                        //if(!MainThread.IsBusy)
                        //{
                        //    MainThread.RunWorkerAsync();
                        //}                        
                        //Avantech.MainThreadEnabled = true;
                        Avantech.AIEnabled = true;
                        Avantech.DIOEnabled = true;
                        Avantech.DIEnabled = true;
                        Avantech.DOEnabled = true;
                        Avantech.AOEnabled = true;
                        Avantech.Avantech_AllFailCheckEnabled = true;
                        MessageBox.Show("Successful Connection to Fieldbus!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Program already running. This instance will abort.");
                        //if(MainThread.IsBusy==false)
                        System.Environment.Exit(0);
                    }
                }
                else
                {
                    txtConnection.Text = "OffLine";
                    cmdPWR.BackColor = Color.Gray;
                    cmdIO.BackColor = Color.Gray;

                    // -2 means that modules not connected
                    lblAIErr.Text = "-2";
                    lblDIOErr.Text = "-2";
                    lblDIErr.Text = "-2";
                    lblDOErr.Text = "-2";
                    lblAOErr.Text = "-2";

                    AvantechAOs.Initialize(SystemGlobals.IP, SystemGlobals.AOSlotNum, Avantech.m_ScanTime_LocalSys[0]);//AvantechAOs.Initialize(IP, AOSlotNum, Avantech.m_ScanTime_LocalSys[0]);
                    MessageBox.Show("Failed to Connect to Fieldbus! Please check the phyiscal connection and Modbus address setting!");
                }
            }
        }

        private void cmdCouplerInformation_Click(object sender, EventArgs e)
        {
            CouplerInformation couplerInformation = new CouplerInformation();
            couplerInformation.ShowDialog();
        }

        private void ChkAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkAutoStart.Checked == true)
            {
                Class1.Autostart = 1;
                Class2.Update("1", "Setup", "Autostart");
                // Class2.autoStartUpdate(1);

            }
            else
            {
                Class1.Autostart = 0;
                //Class2.autoStartUpdate(0);
                Class2.Update("0", "Setup", "Autostart");
            }
        }

        private void AutoStartup_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Class1.RetForm = this;
           // cmdStartProgram.Enabled = false;
            this.Hide();
            if (Class1.OpenfromManual == false)
            {
                //Class1.Connected = false;
                cmdStartProgram.Enabled = true;
                Splash objSplash = new Splash();
                objSplash.ShowDialog();
            }
            else
                this.Show();
        }

  
        private void AvantechConnectTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (BGWConnectAvantechStep == 0)// request connection
                {
                    AvantechSystemInitialize();
                    if (Avantech.bModbusConnected)
                    {
                        DisconnectErrorOK = false;
                        lblResumeConnectionInfo.Text = "";
                        txtConnection.Text = "OnLine";
                        cmdPWR.BackColor = Color.Green;
                        cmdIO.BackColor = Color.Green;
                        AvantechIO_Modules_Initialize();

                        DataAcquitionTimer.Enabled = true;
                        if (!MainThread.IsBusy)
                        {
                            MainThread.RunWorkerAsync();
                        }
                        Avantech.MainThreadEnabled = true;
                        Avantech.AIEnabled = true;
                        Avantech.DIOEnabled = true;
                        Avantech.DIEnabled = true;
                        Avantech.DOEnabled = true;
                        Avantech.AOEnabled = true;
                        Avantech.Avantech_AllFailCheckEnabled = true;
                        BGWConnectAvantechStep = 1;
                        MessageBox.Show("Successful Connection to Fieldbus!");
                        ThreadStart newStart = new ThreadStart(Avantech.ShowWaitMsg1);
                        Thread waitThread = new Thread(Avantech.ShowWaitMsg1);
                        waitThread.Start();
                        AvantechConnectTimer.Stop();
                        ResetAvantechConnectTimerProperty();
                    }
                    else
                    {
                        BGWConnectAvantechStep = 0;
                        if(BGWConnectTimerStart==false)
                        {
                       
                        if (Avantech.Avantech_AllFailCheckEnabled)// check all fail status
                        {
                            if ((!Avantech.DIEnabled) && (!Avantech.AIEnabled) && (!Avantech.DOEnabled) && (!Avantech.AOEnabled) &&
                            (!Avantech.DIOEnabled))
                            {
                                BGWConnectTimerStart = true;
                                MessageBox.Show(new Form { TopMost = true }, "Please check the physical connection and MODBUS address setting!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                                AvantechConnectTimer.Enabled = true;
                                DisconnectErrorOK = true;
                                ResetAvantechConnectTimerProperty();
                                Avantech.Avantech_AllFailCheckEnabled = false;
                                DataAcquitionTimer.Enabled = false;

                                // UI related disabled
                                int i = 0;
                                txtConnection.Text = "OffLine";
                                cmdPWR.BackColor = Color.Gray;
                                cmdIO.BackColor = Color.Gray;

                                // -2 means that modules not connected
                                lblAIErr.Text = "-2";
                                lblDIOErr.Text = "-2";
                                lblDIErr.Text = "-2";
                                lblDOErr.Text = "-2";
                                lblAOErr.Text = "-2";

                                for (i = 0; i < 12; i++)
                                {
                                    SetText("0", i);//SetText(Convert.ToString(AIvalues[i]), i);
                                }
                                for (i = 0; i < 36; i++)
                                {
                                    DI[i].BackColor = Color.Red;
                                }
                                for (i = 0; i < 36; i++)
                                {
                                    DO[i].BackColor = Color.Red;
                                }
                                for (i = 0; i < 8; i++)
                                {
                                    Class1.txtAOOutputVals[i].Text = "";
                                    Class1.AOTrackBarVals[i].Value = 0;
                                }
                            }
                        }
                        }
                    }
                }
                if (BGWConnectAvantechStep == 1)
                {
                    AvantechConnectTimer.Stop();
                    BGWConnectTimerStart = false;
                    //Avantech.bBGWConnectAvantech = false;// disable request connection
                }
                if(DisconnectErrorOK==true)
                {
                    Class1.DisconnectTimeElapsedSecs = Convert.ToInt32(BGWConnectTimes * AvantechConnectTimer.Interval * 0.001f);
                    lblResumeConnectionInfo.Text = Class1.DisconnectTimeElapsedSecs + "Seconds has elapsed since disconnection!";
                BGWConnectTimes++;
                if (BGWConnectTimes >= 4)// larger than 20 secs
                {
                    AvantechConnectTimer.Stop();
                    BGWConnectTimerStart = false;
                    MessageBox.Show("More than 20 secs Auto Connect Time Elapsed! Please connect manually !");
                    DisconnectErrorOK = false;
                    //Avantech.bBGWConnectAvantech = false;// not request connection anymore, jump out
                } 
                }
            }
            catch
            {

                MessageBox.Show("Connection erro, Please check Advantech Connection");
            }
          
        }

        public void ResetAvantechConnectTimerProperty()
        {
            BGWConnectAvantechStep = 0;
            BGWConnectTimes = 0;
        }

        private void btnDORFON_0_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                bool bVal = false;
                if (!AvantechDOs.CheckControllable())
                {
                    lblDOErr.Text = "-1";
                    return;
                }
                //Avantech.APAX5046_DOEnabled = false;//Avantech.MainThreadEnabled = false;
                if (btnDORFON_0.BackColor == Color.Red)
                {
                    bVal = true;
                    Class2.SetDO(Class1.DOSlotNum, 0, bVal);
                    btnDORFON_0.BackColor = Color.Green;
                }
                else
                {
                    bVal = false;
                    Class2.SetDO(Class1.DOSlotNum, 0, bVal);
                    btnDORFON_0.BackColor = Color.Red;
                }
                //Avantech.APAX5046_DOEnabled = true;//Avantech.MainThreadEnabled = true;
            }
            else
            {
                // no connection, just click as normal buttons
                if (btnDORFON_0.BackColor == Color.Red)
                {
                    btnDORFON_0.BackColor = Color.Green;
                }
                else
                {
                    btnDORFON_0.BackColor = Color.Red;
                }
            }
        }

        private void tBarRFSET_0Val_ValueChanged(object sender, EventArgs e)
        {
            //float fVal;
            //fVal = (AvantechAOs.m_fHigh - AvantechAOs.m_fLow) * (tBarAO_0Val.Value - tBarAO_0Val.Minimum) / (tBarAO_0Val.Maximum - tBarAO_0Val.Minimum) + AvantechAOs.m_fLow;
            //txtAO_0Val.Text = fVal.ToString("0.000");//txtOutputVal.Text = fVal.ToString("0.000");

            float fVal;
            fVal = (AvantechAOs.m_fHighVals[0] - AvantechAOs.m_fLowVals[0]) * (tBarRFSET_0Val.Value - tBarRFSET_0Val.Minimum) / (tBarRFSET_0Val.Maximum - tBarRFSET_0Val.Minimum) + AvantechAOs.m_fLowVals[0];
            txtRFSET_0Val.Text = fVal.ToString("0.000");
        }

        private void tBarRFSET_0Val_MouseDown(object sender, MouseEventArgs e)
        {
            //AvantechAOs.b_AOValueModified = true;
            //txtAO_0Val.SelectAll();

            AvantechAOs.m_bAOValueModified[0] = true;
            txtRFSET_0Val.SelectAll();
        }

        private void txtRFSET_0Val_MouseDown(object sender, MouseEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[0] = true;
        }

        private void txtRFSET_0Val_KeyPress(object sender, KeyPressEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[0] = true;
        }

        private void tBarTNSET_1Val_MouseDown(object sender, MouseEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[1] = true;
            txtTNSET_1Val.SelectAll();
        }

        private void tBarTNSET_1Val_ValueChanged(object sender, EventArgs e)
        {
            float fVal;
            fVal = (AvantechAOs.m_fHighVals[1] - AvantechAOs.m_fLowVals[1]) * (tBarTNSET_1Val.Value - tBarTNSET_1Val.Minimum) / (tBarTNSET_1Val.Maximum - tBarTNSET_1Val.Minimum) + AvantechAOs.m_fLowVals[1];
            txtTNSET_1Val.Text = fVal.ToString("0.000");
        }

        private void txtTNSET_1Val_KeyPress(object sender, KeyPressEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[1] = true;
        }

        private void txtTNSET_1Val_MouseDown(object sender, MouseEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[1] = true;

        }

        private void tBarLDSET_2Val_MouseDown(object sender, MouseEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[2] = true;
            txtLDSET_2Val.SelectAll();
        }

        private void tBarLDSET_2Val_ValueChanged(object sender, EventArgs e)
        {
            float fVal;
            fVal = (AvantechAOs.m_fHighVals[2] - AvantechAOs.m_fLowVals[2]) * (tBarLDSET_2Val.Value - tBarLDSET_2Val.Minimum) / (tBarLDSET_2Val.Maximum - tBarLDSET_2Val.Minimum) + AvantechAOs.m_fLowVals[2];
            txtLDSET_2Val.Text = fVal.ToString("0.000");
        }

        private void txtLDSET_2Val_KeyPress(object sender, KeyPressEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[2] = true;
        }

        private void txtLDSET_2Val_MouseDown(object sender, MouseEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[2] = true;
        }

        private void tBarG1SET_3Val_MouseDown(object sender, MouseEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[3] = true;
            txtG1SET_3Val.SelectAll();
        }

        private void tBarG1SET_3Val_ValueChanged(object sender, EventArgs e)
        {
            float fVal;
            fVal = (AvantechAOs.m_fHighVals[3] - AvantechAOs.m_fLowVals[3]) * (tBarG1SET_3Val.Value - tBarG1SET_3Val.Minimum) / (tBarG1SET_3Val.Maximum - tBarG1SET_3Val.Minimum) + AvantechAOs.m_fLowVals[3];
            txtG1SET_3Val.Text = fVal.ToString("0.000");
        }

        private void txtG1SET_3Val_KeyPress(object sender, KeyPressEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[3] = true;
        }

        private void txtG1SET_3Val_MouseDown(object sender, MouseEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[3] = true;
        }

        private void tBarG2SET_4Val_MouseDown(object sender, MouseEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[4] = true;
            txtG2SET_4Val.SelectAll();
        }

        private void tBarG2SET_4Val_ValueChanged(object sender, EventArgs e)
        {
            float fVal;
            fVal = (AvantechAOs.m_fHighVals[4] - AvantechAOs.m_fLowVals[4]) * (tBarG2SET_4Val.Value - tBarG2SET_4Val.Minimum) / (tBarG2SET_4Val.Maximum - tBarG2SET_4Val.Minimum) + AvantechAOs.m_fLowVals[4];
            txtG2SET_4Val.Text = fVal.ToString("0.000");
        }

        private void txtG2SET_4Val_KeyPress(object sender, KeyPressEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[4] = true;
        }

        private void txtG2SET_4Val_MouseDown(object sender, MouseEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[4] = true;
        }

        private void tBarSPARE_5Val_MouseDown(object sender, MouseEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[5] = true;
            txtSPARE_5Val.SelectAll();
        }

        private void tBarSPARE_5Val_ValueChanged(object sender, EventArgs e)
        {
            float fVal;
            fVal = (AvantechAOs.m_fHighVals[5] - AvantechAOs.m_fLowVals[5]) * (tBarSPARE_5Val.Value - tBarSPARE_5Val.Minimum) / (tBarSPARE_5Val.Maximum - tBarSPARE_5Val.Minimum) + AvantechAOs.m_fLowVals[5];
            txtSPARE_5Val.Text = fVal.ToString("0.000");
        }

        private void txtSPARE_5Val_KeyPress(object sender, KeyPressEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[5] = true;
        }

        private void txtSPARE_5Val_MouseDown(object sender, MouseEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[5] = true;
        }

        private void tBarSPARE_6Val_MouseDown(object sender, MouseEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[6] = true;
            txtSPARE_6Val.SelectAll();
        }

        private void tBarSPARE_6Val_ValueChanged(object sender, EventArgs e)
        {
            float fVal;
            fVal = (AvantechAOs.m_fHighVals[6] - AvantechAOs.m_fLowVals[6]) * (tBarSPARE_6Val.Value - tBarSPARE_6Val.Minimum) / (tBarSPARE_6Val.Maximum - tBarSPARE_6Val.Minimum) + AvantechAOs.m_fLowVals[6];
            txtSPARE_6Val.Text = fVal.ToString("0.000");
        }

        private void txtSPARE_6Val_KeyPress(object sender, KeyPressEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[6] = true;
        }

        private void txtSPARE_6Val_MouseDown(object sender, MouseEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[6] = true;
        }

        private void tBarSPARE_7Val_MouseDown(object sender, MouseEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[7] = true;
            txtSPARE_7Val.SelectAll();
        }

        private void tBarSPARE_7Val_ValueChanged(object sender, EventArgs e)
        {
            float fVal;
            fVal = (AvantechAOs.m_fHighVals[7] - AvantechAOs.m_fLowVals[7]) * (tBarSPARE_7Val.Value - tBarSPARE_7Val.Minimum) / (tBarSPARE_7Val.Maximum - tBarSPARE_7Val.Minimum) + AvantechAOs.m_fLowVals[7];
            txtSPARE_7Val.Text = fVal.ToString("0.000");
        }

        private void txtSPARE_7Val_KeyPress(object sender, KeyPressEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[7] = true;
        }

        private void txtSPARE_7Val_MouseDown(object sender, MouseEventArgs e)
        {
            AvantechAOs.m_bAOValueModified[7] = true;
        }

        private void btnRFSET_0ApplyOutput_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
               SetAO(0);
            }
            else
            {
                MessageBox.Show("Modbus connection error!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnTNSET_1ApplyOutput_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                SetAO(1);
            }
            else
            {
                MessageBox.Show("Modbus connection error!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnLDSET_2ApplyOutput_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                SetAO(2);
            }
            else
            {
                MessageBox.Show("Modbus connection error!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnG1SET_3ApplyOutput_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                SetAO(3);
            }
            else
            {
                MessageBox.Show("Modbus connection error!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnG2SET_4ApplyOutput_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                SetAO(4);
            }
            else
            {
                MessageBox.Show("Modbus connection error!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnSPARE_5ApplyOutput_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                SetAO(5);
            }
            else
            {
                MessageBox.Show("Modbus connection error!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnSPARE_6ApplyOutput_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                SetAO(6);
            }
            else
            {
                MessageBox.Show("Modbus connection error!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnSPARE_7ApplyOutput_Click(object sender, EventArgs e)
        {
            if (Avantech.bModbusConnected)
            {
                SetAO(7);
            }
            else
            {
                MessageBox.Show("Modbus connection error!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        private void DataAcquitionTimer_Tick(object sender, EventArgs e)
        {
            int i = 0;
            bool bDORet;
            try
            {

                int iDOChannelTotal = AvantechDOs.m_aConf.HwIoTotal[AvantechDOs.m_tmpidx];
                bool[] DOvalues = new bool[iDOChannelTotal];

                bool bAORet;
                int iAOChannelTotal = AvantechAOs.m_aConf.HwIoTotal[AvantechAOs.m_tmpidx];
                double[] AOvalues = new double[iAOChannelTotal];
                string[] strAOvalues = new string[iAOChannelTotal];

                bool bDIORet;
                // DIO's DI Channel
                int iDIO_DIChannelTotal = AvantechDIOs.m_aConf.HwIoTotal[AvantechDIOs.m_DIidx];
                bool[] DIO_DIvalues = new bool[iDIO_DIChannelTotal];
                // DIO's DO Channel
                bool bDOIRet;
                int iDIO_DOChannelTotal = AvantechDIOs.m_aConf.HwIoTotal[AvantechDIOs.m_DOidx];
                bool[] DIO_DOvalues = new bool[iDIO_DOChannelTotal];



                // DI 5040
                bool bDIRet;
                int iDIChannelTotal = AvantechDIs.m_aConf.HwIoTotal[AvantechDIs.m_tmpidx];
                bool[] DIvalues = new bool[iDIChannelTotal];

                bool bAIRet;
                int iAIChannelTotal = AvantechAIs.m_aConf.HwIoTotal[AvantechAIs.m_tmpidx];
                double[] AIvalues = new double[iAIChannelTotal];
                string[] strAIvalues = new string[iAIChannelTotal];

                // ***********DO*********** ////
                if (Avantech.DOEnabled)
                {
                    try
                    {
                        cmdNETWORK.BackColor = Color.Green;
                        bDORet = AvantechDOs.RefreshData(ref DOvalues);
                        DOvalues.CopyTo(Class1.DOArrayValues, 0);
                        DOvalues.CopyTo(Class1.DOIArrayValues, 0);
                        Class1.GetDO(Class1.DOIArrayValues);
                        lblDOErr.Text = bDORet ? "0" : "-1";//lblDOErr.Text = bDORet ? "0" : "-1";
                        cmdNETWORK.BackColor = Color.Gray;
                        if (bDORet)
                        {
                            AvantechDOs.m_iScanCount++;
                            AvantechDOs.m_iFailCount = 0;
                            for (i = 0; i < iDOChannelTotal; i++)
                            {
                                DO[i].BackColor = DOvalues[i] ? Color.Green : Color.Red;
                            }

                        }
                        else
                        {
                            AvantechDOs.m_iFailCount++;
                        }

                        if (AvantechDOs.m_iFailCount > 0)
                        {
                            Avantech.DOEnabled = false;//timer1.Enabled = false;
                            //OutputTimer.Enabled = false;
                            Avantech.bModbusConnected = false;
                            AvantechConnectTimer.Enabled = true;
                            ResetAvantechConnectTimerProperty();
                            //MessageBox.Show("Please check the physical connection and MODBUS address setting!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);

                        }
                        if (AvantechDOs.m_iScanCount % 50 == 0)
                            GC.Collect();
                    }
                    catch
                    {

                        MessageBox.Show("Connection error, Please check Advantech Connection");
                    }

                }
                // ***********End of DO*********** ////



                // ***********AO*********** ////
                if (Avantech.AOEnabled)
                {
                    try
                    {
                        cmdNETWORK.BackColor = Color.Green;
                        bAORet = AvantechAOs.RefreshData(ref strAOvalues);

                        lblAOErr.Text = bAORet ? "0" : "-1";//lblDOErr.Text = bDORet ? "0" : "-1";
                        cmdNETWORK.BackColor = Color.Gray;
                        if (bAORet)
                        {
                            AvantechAOs.m_iScanCount++;
                            AvantechAOs.m_iFailCount = 0;
                        }
                        else
                        {
                            AvantechAOs.m_iFailCount++;
                        }

                        if (AvantechAOs.m_iFailCount > 0)
                        {
                            Avantech.AOEnabled = false;//timer1.Enabled = false;
                            //OutputTimer.Enabled = false;
                            Avantech.bModbusConnected = false;
                            AvantechConnectTimer.Enabled = true;
                            ResetAvantechConnectTimerProperty();
                            //MessageBox.Show("Please check the physical connection and MODBUS address setting!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                            ModuleErrorDialog objModuleErrorDialog = new ModuleErrorDialog();
                            objModuleErrorDialog.ShowDialog();
                        }
                        if (AvantechAOs.m_iScanCount % 50 == 0)
                            GC.Collect();
                    }
                    catch
                    {
                        MessageBox.Show("Connection error, Please check Advantech Connection");

                    }
                }
                // ***********End of AO*********** ////

                if (Avantech.DIEnabled)
                {
                    cmdNETWORK.BackColor = Color.Green;
                    bDIRet = AvantechDIs.RefreshData(ref DIvalues);
                    DIvalues.CopyTo(Class1.DIOArrayValues, 0);
                    Class1.GetDI(Class1.DIOArrayValues);
                    lblDIErr.Invoke((MethodInvoker)delegate { lblDIErr.Text = bDIRet ? "0" : "-1"; });//lblDIErr.Text = bDIRet ? "0" : "-1";
                    cmdNETWORK.BackColor = Color.Gray;
                    if (bDIRet)
                    {
                        AvantechDIs.m_iScanCount++;
                        AvantechDIs.m_iFailCount = 0;
                        this.Invoke((MethodInvoker)delegate
                        {
                            for (i = 0; i < iDIChannelTotal; i++)
                            {
                                DI[i].BackColor = DIvalues[i] ? Color.Green : Color.Red;

                            }
                        });

                    }
                    else
                    {
                        AvantechDIs.m_iFailCount++;
                    }

                    if (AvantechDIs.m_iFailCount > 0)
                    {
                        Avantech.DIEnabled = false;//timer1.Enabled = false;
                        Avantech.bModbusConnected = false;
                        AvantechConnectTimer.Enabled = true;
                        ResetAvantechConnectTimerProperty();
                        ModuleErrorDialog objModuleErrorDialog = new ModuleErrorDialog();
                        objModuleErrorDialog.ShowDialog();
                    }
                    if (AvantechDIs.m_iScanCount % 50 == 0)
                        GC.Collect();
                    //cmdNETWORK.BackColor = Color.Gray;
                }
                //// ***********End of DI*********** ////


                //// ***********AI*********** ////
                if (Avantech.AIEnabled)
                {
                    cmdNETWORK.BackColor = Color.Green;
                    //bAIRet = AvantechAIs.RefreshData(ref AIvalues);
                    bAIRet = AvantechAIs.RefreshData(ref strAIvalues);
                    lblAIErr.Invoke((MethodInvoker)delegate { lblAIErr.Text = bAIRet ? "0" : "-1"; });//lblAIErr.Text = bAIRet ? "0" : "-1";
                    cmdNETWORK.BackColor = Color.Gray;
                    if (bAIRet)
                    {
                        AvantechAIs.m_iScanCount++;
                        AvantechAIs.m_iFailCount = 0;
                        for (i = 0; i < iAIChannelTotal; i++)
                        {
                            SetText(strAIvalues[i], i);//SetText(Convert.ToString(AIvalues[i]), i);
                            switch (i)
                            {
                                case 0:
                                    Class1.AI_PressureValue = Convert.ToDouble(strAIvalues[0]);
                                    if (Class1.AI_PressureValue < 6.0)
                                    {
                                        Class1.Intlk = true;
                                    }
                                    else
                                    {
                                        Class1.Intlk = false;
                                    }
                                    break;
                                case 1:
                                    Class1.AI_ARFPowerValue = Math.Round(Convert.ToDouble((strAIvalues[1])), 1);
                                    break;
                                case 2:
                                    Class1.AI_RFRefelctedValue = Math.Round(Convert.ToDouble((strAIvalues[2])), 1);
                                    break;
                                case 3:
                                    Class1.AI_BiasValue = Math.Round(Convert.ToDouble((strAIvalues[3])), 1);
                                    break;
                                case 4:
                                    Class1.AI_TuneValue = Math.Round(Convert.ToDouble((strAIvalues[4])), 2);
                                    break;
                                case 5:
                                    Class1.AI_LoadValue = Math.Round(Convert.ToDouble((strAIvalues[5])), 2);
                                    break;
                                case 6:
                                    Class1.AI_GAS1PSValue = Math.Round(Convert.ToDouble((strAIvalues[6])), 2);
                                    break;
                                case 7:
                                    Class1.AI_GAS2PSValue = Math.Round(Convert.ToDouble((strAIvalues[7])), 2);
                                    break;
                                case 8:
                                    Class1.AI_GAS1Value = Math.Round(Convert.ToDouble((strAIvalues[8])), 2);
                                    break;
                                case 9:
                                    Class1.AI_GAS2Value = Math.Round(Convert.ToDouble((strAIvalues[9])), 2);
                                    break;

                                default:
                                    break; // TODO: might not be correct. Was : Exit Select

                                    break;
                            }
                        }
                    }
                    else
                    {
                        AvantechAIs.m_iFailCount++;
                    }

                    if (AvantechAIs.m_iFailCount > 0)
                    {
                        Avantech.AIEnabled = false;//timer1.Enabled = false;
                        Avantech.bModbusConnected = false;
                        AvantechConnectTimer.Enabled = true;
                        ResetAvantechConnectTimerProperty();
                        //MessageBox.Show("Please check the physical connection and MODBUS address setting!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                        ModuleErrorDialog objModuleErrorDialog = new ModuleErrorDialog();
                        objModuleErrorDialog.ShowDialog();
                    }
                    if (AvantechAIs.m_iScanCount % 50 == 0)
                        GC.Collect();
                }
                if (Avantech.DIOEnabled)
                {

                    try
                    {
                        cmdNETWORK.BackColor = Color.Green;
                        bDIORet = AvantechDIOs.Refresh_DIData(ref DIO_DIvalues);//bDIORet = AvantechDIOs.RefreshData(ref DIO_DIvalues, ref DIO_DOvalues);
                        bDOIRet = AvantechDIOs.Refresh_DOData(ref DIO_DOvalues);
                        lblDIOErr.Text = bDIORet ? "0" : "-1";//lblDIOErr.Text = bDIORet ? "0" : "-1";
                        cmdNETWORK.BackColor = Color.Gray;
                        if (bDIORet)
                        {
                            AvantechDIOs.m_iScanCount++;
                            AvantechDIOs.m_iFailCount = 0;
                            for (i = iDIChannelTotal; i < iDIChannelTotal + iDIO_DIChannelTotal; i++)
                            {
                                DI[i].BackColor = DIO_DIvalues[i - iDIChannelTotal] ? Color.Green : Color.Red;
                                //Class1.DIOArrayValues[i] = DIO_DIvalues[i - iDIChannelTotal];
                                //Class1.GetDI(Class1.DIOArrayValues);
                            }
                            for (i = iDOChannelTotal; i < iDOChannelTotal + iDIO_DOChannelTotal; i++)
                            {
                                DO[i].BackColor = DIO_DOvalues[i - iDOChannelTotal] ? Color.Green : Color.Red;
                                //Class1.DOIArrayValues[i] = DIO_DOvalues[i - iDOChannelTotal];
                            }
                        }
                        else
                        {
                            AvantechDIOs.m_iFailCount++;
                        }

                        if (AvantechDOs.m_iFailCount > 0)
                        {
                            Avantech.DIOEnabled = false;//timer1.Enabled = false;
                            //OutputTimer.Enabled = false;
                            Avantech.bModbusConnected = false;
                            AvantechConnectTimer.Enabled = true;
                            ResetAvantechConnectTimerProperty();
                            ModuleErrorDialog objModuleErrorDialog = new ModuleErrorDialog();
                            objModuleErrorDialog.ShowDialog();
                        }
                        if (AvantechDIOs.m_iScanCount % 50 == 0)
                            GC.Collect();
                    }
                    catch
                    {

                        MessageBox.Show("Connection error, Please check Advantech Connection");
                    }
                }
            }
            catch (Exception ex)
            {
                //Environment.Exit(0);
                //MessageBox.Show("Connection error, Please check Advantech Connection");

            }
        }

              
    }
}

   
    
      
  

