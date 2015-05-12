using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using Advantech.Adam;
using Apax_IO_Module_Library;

namespace SG25
{
    public static class AvantechDIs
    {
        //************************** DI module(APAX-5040) **************************//
        // Global object
        public const string APAX_INFO_NAME = "APAX";
        public const string DVICE_TYPE = "5040";

        public static AdamSocket m_adamSocket;
        public static int[] m_iTimeout;
        public static string m_szIP = "";

        public static Apax5000Config m_aConf;
        public static int m_idxID;
        public static int m_ScanTime_LocalSys;
        public static int m_iFailCount, m_iScanCount;
        public static int m_tmpidx;
        public static string[] m_szSlots;// Container of all solt device type

        public static int DI_FILTER_WIDTH_MAX = 400;
        public static int DI_FILTER_WIDTH_MIN = 5;
        public static bool m_bStartFlag = false;
        public static ushort m_usStart;
        public static ushort m_usLength;
        public static string StatusBar_IO = "";

        // Some Text information
        public static string txtModule;       //Information-> Module
        public static string txtID;                //Information -> Switch ID
        public static string txtSupportKernelFw;     //Information -> Support kernel Fw
        public static string txtFwVer;                    //Firmware version

        public static string txtCntMin;
        public static bool chkBoxDiFilterEnable;
        // Some Text information

        public static void Initialize()
        {
            m_szSlots = null;
            m_iScanCount = 0;
            m_iFailCount = 0;
            m_idxID = -1; // Set in invalid num 
            m_ScanTime_LocalSys = 500;// Scan time default 500 ms
            //timer1.Interval = m_ScanTime_LocalSys;
            m_iTimeout = new int[3];
            m_iTimeout[0] = 2000; // Connection Timeout    
            m_iTimeout[1] = 2000; // Send Timeout
            m_iTimeout[2] = 2000;// Receive Timeout
           
        }

        public static void Initialize(string IP, int SlotNum, int ScanTime)
        {
            m_szSlots = null;
            m_idxID = SlotNum; // Set Slot_ID
            m_iScanCount = 0;
            m_iFailCount = 0;
            m_ScanTime_LocalSys = ScanTime;// Scan time

            //timer1.Interval = m_ScanTime_LocalSys;
            m_iTimeout = new int[3];
            m_iTimeout[0] = 2000;// Connection Timeout    
            m_iTimeout[1] = 2000;// Send Timeout
            m_iTimeout[2] = 2000;// Receive Timeout
            m_szIP = IP;

          
        }

        /// <summary>
        /// Used for change I/O module 
        /// </summary>
        /// <returns></returns>
        public static bool FreeResource()
        {
            if (m_bStartFlag)
            {
                m_bStartFlag = false;
                Avantech.DIEnabled = false;//timer1.Enabled = false;

                m_adamSocket.Configuration().SYS_SetLocateModule(m_idxID, 0);
                m_adamSocket.Disconnect();
                m_adamSocket = null;
            }
            return true;
        }

        public static void formIP_ApplyIPAddressClick(string strIP)
        {
            m_szIP = strIP;
        }

        public static bool SetIp()
        {
            int count = 0;
            while (((count <= 2) && (m_szIP == "")))
            {
                IPForm formIP = new IPForm();
                formIP.ApplyIPAddressClick += new IPForm.EventHandler_ApplyIPAddressClick(formIP_ApplyIPAddressClick);
                formIP.ShowDialog();
                formIP.Dispose();
                formIP = null;
                count++;
            }
            if ((m_szIP == null))
            {
                return false;
            }
            return true;
        }

        public static bool StartRemote()
        {
            try
            {

            if ((m_szIP == ""))
            {
                if (!SetIp())
                {
                    MessageBox.Show("Demo failed! Please set up IP address.", "Error");
                    return false;
                }
            }
            try
            {
                if (!OpenDevice())
                {
                    throw new System.Exception("Open Local Device Fail.");
                }
                if (!DeviceFind())
                {
                    throw new System.Exception("Find " + DVICE_TYPE + "Device Fail.");
                }
                if (!RefreshConfiguration())
                {
                    throw new System.Exception("Get" + DVICE_TYPE + " Device Configuration Fail.");
                }
            }
            catch (Exception ex)
            {
              
                return false;
            }
            StatusBar_IO = "Starting to remote...";//this.StatusBar_IO.Text = "Starting to remote...";
           
            InitialDataTabPages();
            //this.Text = (APAX_INFO_NAME + DVICE_TYPE);
            m_iScanCount = 0;
            //this.tabControl1.SelectedIndex = 0;
            return true;

            }
            catch (Exception ex)
            {

                MessageBox.Show("Connection error, Please check the Fieldbus Connection. This system will shutdown ");
                return false;
            }
        }

        public static bool OpenDevice()
        {
            m_adamSocket = new AdamSocket(AdamType.Apax5070);
            m_adamSocket.SetTimeout(m_iTimeout[0], m_iTimeout[1], m_iTimeout[2]);
            if (m_adamSocket.Connect(AdamType.Apax5070, m_szIP, ProtocolType.Tcp))
            {
                if (!m_adamSocket.Configuration().GetSlotInfo(out m_szSlots))
                {
                    StatusBar_IO = "GetSlotInfo() Failed! ";//this.StatusBar_IO.Text = "GetSlotInfo() Failed! ";
                    return false;
                }
            }
            return true;
        }

        public static bool DeviceFind()
        {
            try
            {

            int iLoop = 0;
            int iDeviceNum = 0;
            if ((m_idxID == -1))
            {
                for (iLoop = 0; iLoop < m_szSlots.Length; iLoop++)
                {
                    if ((m_szSlots[iLoop] == null))
                        continue;
                    if ((string.Compare(m_szSlots[iLoop], 0, DVICE_TYPE, 0, DVICE_TYPE.Length) == 0))
                    {
                        iDeviceNum++;
                        if ((iDeviceNum == 1))// Record first find device
                        {

                            m_idxID = iLoop;// Get DVICE_TYPE Solt

                        }
                    }
                }
            }
            else if ((string.Compare(m_szSlots[m_idxID], 0, DVICE_TYPE, 0, DVICE_TYPE.Length) == 0))
            {
                iDeviceNum++;
            }

            if ((iDeviceNum == 1))
            {
                return true;
            }
            else if ((iDeviceNum > 1))
            {
                MessageBox.Show("Found " + iDeviceNum.ToString() + " " + DVICE_TYPE + " devices." + " It's will demo Solt " + m_idxID.ToString() + ".", "Warning");
                return true;
            }
            else
            {
                MessageBox.Show(("Can\'t find any "
                                + (DVICE_TYPE + " device!")), "Error");
                return false;
            }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Connection error, Please check advantech Connection. ");
                return false;
                
            }
        }

        /// <summary>
        /// APAX I/O module information init function
        /// </summary>
        /// <returns></returns>
        public static bool RefreshConfiguration()
        {


            if (m_adamSocket.Configuration().GetModuleConfig(m_idxID, out m_aConf))
            {

                txtModule = m_aConf.GetModuleName();//txtModule.Text = m_aConf.GetModuleName();       //Information-> Module
                txtID = m_idxID.ToString();//txtID.Text = m_idxID.ToString();                //Information -> Switch ID
                txtSupportKernelFw = m_aConf.wSupportFwVer.ToString("X04").Insert(2, ".");//txtSupportKernelFw.Text = m_aConf.wSupportFwVer.ToString("X04").Insert(2, ".");     //Information -> Support kernel Fw
                txtFwVer = m_aConf.wFwVerNo.ToString("X04").Insert(2, ".");//txtFwVer.Text = m_aConf.wFwVerNo.ToString("X04").Insert(2, ".");                    //Firmware version
                RefreshModbusAddressSetting();
            }
            else
            {
                StatusBar_IO = " GetModuleConfig(Error:" + m_adamSocket.Configuration().LastError.ToString() + ") Failed! ";//StatusBar_IO.Text = " GetModuleConfig(Error:" + m_adamSocket.Configuration().LastError.ToString() + ") Failed! ";
                return false;
            }
            return true;
        }

        /// <summary>
        ///  Initial every tab of I/O modules Information
        /// </summary>
        /// <param name="aConf">apax 5000 device object</param>
        public static void InitialDataTabPages()
        {
            int i = 0, idx = 0;
            byte type = (byte)_HardwareIOType.DI;   //APAX-5040 is DI module
            ListViewItem lvItem;

            for (i = 0; i < m_aConf.HwIoType.Length; i++)
            {
                if (m_aConf.HwIoType[i] == type)
                    idx = i;
            }
            m_tmpidx = idx;
            //listViewChInfo.BeginUpdate();
            //listViewChInfo.Items.Clear();
            for (i = 0; i < m_aConf.HwIoTotal[idx]; i++)
            {
                lvItem = new ListViewItem(_HardwareIOType.DI.ToString());   //Type
                lvItem.SubItems.Add(i.ToString());      //Ch
                if (m_usStart > 40000)
                {                 //modbus address
                    string szTmp = Convert.ToString(m_usStart + i / 16) + "[" + Convert.ToString(i % 16) + "]";
                    lvItem.SubItems.Add(szTmp);
                }
                else
                {
                    string szTmp = string.Format("{0:D5}", m_usStart + i);
                    lvItem.SubItems.Add(szTmp);
                }
                lvItem.SubItems.Add("*****");           //Value
                lvItem.SubItems.Add("BOOL");            //Mode
                //listViewChInfo.Items.Add(lvItem);
            }
            //listViewChInfo.EndUpdate();
        }

      
        public static bool RefreshData(ref bool[] bDIVals)
        {
            int iChannelTotal = 0;
            bool[] bVal;
            iChannelTotal = m_aConf.HwIoTotal[m_tmpidx];
            if (!m_adamSocket.DigitalInput().GetValues(m_idxID, iChannelTotal, out bVal))
            {
                StatusBar_IO += "ApiErr:" + m_adamSocket.Modbus().LastError.ToString() + " ";//StatusBar_IO.Text += "ApiErr:" + m_adamSocket.Modbus().LastError.ToString() + " ";
                return false;
            }
           
            bVal.CopyTo(bDIVals,0);
            return true;
        }

       /// <summary>
        /// When change tab, refresh status, timer, counter related informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void tabControl1_SelectedIndexChanged()
        {
            uint uiWidth;
            bool bEnable = true;
            string strSelPageName = "DI";//string strSelPageName = tabControl1.TabPages[tabControl1.SelectedIndex].Text;
            StatusBar_IO = "";//StatusBar_IO.Text = "";

            m_adamSocket.Disconnect();
            m_adamSocket.Connect(m_adamSocket.GetIP(), ProtocolType.Tcp, 502);

            if (strSelPageName == "Module Information")
            {
                m_iFailCount = 0;
                m_iScanCount = 0;
            }
            else if (strSelPageName == "DI")
            {
                //Get DI Filter value
                if (m_adamSocket.DigitalInput().GetDigitalFilterMiniSignalWidth(m_idxID, out uiWidth, out bEnable))
                {
                    txtCntMin = uiWidth.ToString();//txtCntMin.Text = uiWidth.ToString();
                    chkBoxDiFilterEnable = bEnable;
                }
            }

          
        }

        ///// <summary>
        ///// Set DI filter -> Minimum low signal width
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
       
        public static void RefreshModbusAddressSetting()
        {
            bool bFixed;

            m_adamSocket.Configuration().GetModbusAddressMode(out bFixed);
            if (bFixed)
            {
                if (m_aConf.wDevType == (ushort)_DeviceType.DigitalInput || m_aConf.wDevType == (ushort)_DeviceType.DigitalOutput)
                {
                    m_usStart = Convert.ToUInt16(64 * m_idxID + 1); //64(Coils, bit) is Slot shift, please reference Modbus/TCP address mapping table(0x)
                    m_usLength = m_aConf.byChTotal;
                }
                else if (m_aConf.wDevType == (ushort)_DeviceType.AnalogInput || m_aConf.wDevType == (ushort)_DeviceType.AnalogOutput)
                {
                    m_usStart = Convert.ToUInt16(32 * m_idxID + 40001); //32(Registers, 2 bytes) is Slot shift, please reference Modbus/TCP address mapping table(4x)
                    m_usLength = m_aConf.byChTotal;
                }
                else if (m_aConf.wDevType == (ushort)_DeviceType.Counter || m_aConf.wDevType == (ushort)_DeviceType.PWM)
                {
                    m_usStart = Convert.ToUInt16(32 * m_idxID + 40001);
                    m_usLength = Convert.ToUInt16(m_aConf.byHwIoTotal_2 * 2 + 1); //per counter channel use 2 Registers(= 4 bytes)
                }
            }
            else
            {
                int o_iStartAddr, o_iLength;
                if (m_adamSocket.Configuration().GetModbusAddressConfig(m_idxID, out o_iStartAddr, out o_iLength))
                {
                    m_usStart = Convert.ToUInt16(o_iStartAddr);
                    m_usLength = Convert.ToUInt16(o_iLength);
                }
            }
        }

        public static void Start()
        {
            string strbtnStatus = "Start";
            if ((string.Compare(strbtnStatus, "Start", true) == 0))
            {
                // Was Stop, Then Start
                if (!StartRemote())
                {
                    return;
                }
                m_bStartFlag = true;
                
            }
            else
            {
                StatusBar_IO = ("Start to demo "
                        + APAX_INFO_NAME + "-"
                        + DVICE_TYPE + " by clicking 'Start'button.");
                FreeResource();
                //this.btnStart.Text = "Start";
            }
        }

       //************************** End of DI module(APAX-5040) **************************//
    }
}
