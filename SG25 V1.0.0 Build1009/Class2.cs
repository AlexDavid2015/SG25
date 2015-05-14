using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Text;
using APS_Define_W32;
using APS168_W32;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using SG25.SetupDataSetTableAdapters;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Net.Sockets;
using Advantech.Adam;
using Apax_IO_Module_Library;
namespace SG25
{
    public static class Class2
    {
        public static int CardID = 0;        //PCI-7858 Card ID
        public static int BusNo = 0;         //HSL Bus is 0
        public static string DOArray = "";
        public static string DoorDOArray;
        public static string DOStatus;
        public static Boolean SetDoorsAlarm;
        //public static string RetArray;

        private static UsersTableAdapter userTAobj = new UsersTableAdapter();

        public static void SetDO(int iSlot, int iChannel, bool bState)
        {
            if (Avantech.bModbusConnected)
            { 
            string errorMsg = "";
            errorMsg = string.Format("Set digital output failed! at Channel {0} in iSlot {1}", iSlot, iChannel);
            if (iSlot == 3)// 5046 DOs
            {
                try { 
                if (!AvantechDOs.CheckControllable())
                    return;
                Avantech.DOEnabled = false;
                int iDOChannelTotal = AvantechDOs.m_aConf.HwIoTotal[AvantechDOs.m_tmpidx];
                bool[] DOvalues = new bool[iDOChannelTotal];
                if (AvantechDOs.m_adamSocket.DigitalOutput().SetValue(iSlot, iChannel, bState))
                {
                    AvantechDOs.RefreshData(ref DOvalues);
                }
                else
                    MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                Avantech.DOEnabled = true;
                }
                catch { }
            }
            else if (iSlot == 9)// 5045 DIO's DOs
            {
                try { 
                if (!AvantechDIOs.CheckControllable())
                    return;
                Avantech.DIOEnabled = false;
                int iDIChannelTotal = AvantechDIOs.m_aConf.HwIoTotal[AvantechDIOs.m_DIidx];
                int iDOChannelTotal = AvantechDIOs.m_aConf.HwIoTotal[AvantechDIOs.m_DOidx];
                bool[] DIvalues = new bool[iDIChannelTotal];
                bool[] DOvalues = new bool[iDOChannelTotal];
                if (AvantechDIOs.m_adamSocket.DigitalOutput().SetValue(iSlot, iChannel, bState))
                {
                    AvantechDIOs.Refresh_DOData(ref DOvalues);//AvantechDIOs.RefreshData(ref DIvalues, ref DOvalues);
                }
                else
                    MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                Avantech.DIOEnabled = true;
           }catch{} }
            else
            {
                MessageBox.Show("Set unknown digital output!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }

            }
        }
        public static void SetMultiDO(int[] iChannel, bool[] bState)
        {
            if (Avantech.bModbusConnected)
            {
                string errorMsg = "";
                //errorMsg = string.Format("Set digital output failed! at Channel {0} in iSlot {1}", iChannel);
                bool ChkDOControl;
                bool ChkDIOControl;
                try
                {
                    ChkDOControl = AvantechDOs.CheckControllable();
                    if (ChkDOControl == true)
                    {
                        Avantech.DOEnabled = false;
                        for (int i = 0; i < iChannel.Length; i++)
                        {
                            errorMsg = "Set digital output failed! at Slot - 3   " + iChannel[i];
                            int DOChannelNo = iChannel[i];
                            bool DOState = bState[i];

                            //Class1.DOOutputTimerGlobal.Enabled = false;

                            try
                            {

                                int iDOChannelTotal = AvantechDOs.m_aConf.HwIoTotal[AvantechDOs.m_tmpidx];
                                bool[] DOvalues = new bool[iDOChannelTotal];
                                if (AvantechDOs.m_adamSocket.DigitalOutput().SetValue(3, DOChannelNo, DOState))
                                {
                                    AvantechDOs.RefreshData(ref DOvalues);
                                }
                                else
                                    MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);

                            }
                            catch { }


                            //Class1.DOOutputTimerGlobal.Enabled = true;
                        }
                        Avantech.DOEnabled = true;
                    }
                    else
                    {
                        return;
                    }

                }
                catch { }
            }

        }
        public static void SetMultiDIO(int[] iChannel, bool[] bState)
        {
            if (Avantech.bModbusConnected)
            {
                string errorMsg = "";
                //errorMsg = string.Format("Set digital output failed! at Channel {0} in iSlot {1}",  iChannel);
                bool ChkDOControl;
                bool ChkDIOControl;
                try
                {
                    ChkDIOControl = AvantechDIOs.CheckControllable();
                    if (ChkDIOControl == true)
                    {
                        Avantech.DIOEnabled = false;
                        for (int i = 0; i < iChannel.Length; i++)
                        {
                            errorMsg = errorMsg = "Set digital output failed! at Slot - 9   Channel  " + iChannel[i];
                            int DOChannelNo = iChannel[i];
                            bool DOState = bState[i];

                            //Class1.DOOutputTimerGlobal.Enabled = false;
                            try
                            {
                                int iDIChannelTotal = AvantechDIOs.m_aConf.HwIoTotal[AvantechDIOs.m_DIidx];
                                int iDOChannelTotal = AvantechDIOs.m_aConf.HwIoTotal[AvantechDIOs.m_DOidx];
                                bool[] DIvalues = new bool[iDIChannelTotal];
                                bool[] DOvalues = new bool[iDOChannelTotal];
                                if (AvantechDIOs.m_adamSocket.DigitalOutput().SetValue(9, DOChannelNo, DOState))
                                {
                                    AvantechDIOs.Refresh_DOData(ref DOvalues);
                                }
                                else
                                    MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);

                            }
                            catch { }
                        }


                        //Class1.DOOutputTimerGlobal.Enabled = true;

                    }
                    Avantech.DIOEnabled = true;

                }
                catch { }
            }

        }

        public static object SetDoors()
        {
          //  Thread.Sleep(10);
          //  Int32 ret = default(Int32);
          //  Int32 buf = 0;

          ////  string ans = BinaryToDecimal(DoorDOArray);

          //  buf = Convert.ToInt32(ans, 10);
          //  try
          //  {
          //      ret = APS168.APS_field_bus_d_set_output(CardID, BusNo, Class1.moduleDO, buf);
          //      Class1.ApsRet = ret;
          //  }
          //  catch (Exception e) { SetDoorsAlarm = true; }
            return 0;
        }
       
        public static void MotorRunTUpdate(int MotorT)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = Class1.Connectionstring;
            try
            {
                conn.Open();
                string query = "Update Setup set MotorT='" + MotorT + "'";
                SqlCommand com = new SqlCommand(query, conn);
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            { }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

        }
        public static string Read(string Field, string Table)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = Class1.Connectionstring;
            conn.Open();
            string query = "SELECT " + Field + " FROM " + Table;
            SqlCommand cmdRead = new SqlCommand(query, conn);
            SqlDataReader reader = cmdRead.ExecuteReader();
            reader.Read();
            string returnAuto = (reader[Field]).ToString();
            reader.Close();
            conn.Close();
            return returnAuto;
        }
        public static void Update(string Value,string Table,string Field)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = Class1.Connectionstring;
            try
            {
                conn.Open();
                string query = "Update "+ Table +" set "+ Field + " = " + "'" + Value + "'";
                SqlCommand com = new SqlCommand(query, conn);
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occurred in data update");
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

        }
        public static int EditGases(int i)
        {
            Class1.Gasedit = i;
            GasSetup objGasSetup = new GasSetup();
            objGasSetup.ShowDialog();
            return 0;
        }
        public static void LoadSetup()
        {
            try { 
            SetupDataSet SUsetupDatasetObj = new SetupDataSet();
            StartupTableAdapter SUstartupTAobj = new StartupTableAdapter();
            SetupTableAdapter SUsetupTAobj = new SetupTableAdapter();
            ProgramsTableAdapter SUprogramsTAobj = new ProgramsTableAdapter();

            Class1.SetupRFRange = Convert.ToDouble(SUsetupTAobj.SelectRFRange());
            Class1.Gas1 =Convert.ToBoolean(SUsetupTAobj.GetSetupGas1());
            Class1.Gas2 =Convert.ToBoolean(SUsetupTAobj.GetSetupGas2());
            Class1.Gas3 =Convert.ToBoolean(SUsetupTAobj.GetSetupGas3());
            Class1.Gas1T = SUsetupTAobj.SelectGas1Type().ToString();
            Class1.Gas2T = SUsetupTAobj.SelectGas2Type().ToString();
            Class1.Gas3T = SUsetupTAobj.SelectGas3Type().ToString();
            Class1.Gas1R = Convert.ToDouble(SUsetupTAobj.SelectGas1Range());
            Class1.Gas2R = Convert.ToDouble(SUsetupTAobj.SelectGas2Range());
            Class1.Gas3R = Convert.ToDouble(SUsetupTAobj.SelectGas3Range());
            Class1.Venttime = (int)SUsetupTAobj.SelectVentTime();
            Class1.PunpDwnTime = (int)SUsetupTAobj.SelectPumpDwnTime();
            Class1.H2Gen =Convert.ToBoolean(SUsetupTAobj.GetSetupHasH2());
            Class1.TurboP = Convert.ToBoolean(SUsetupTAobj.GetSetupHasTurbo());
            Class1.UserIndex = (int)SUsetupTAobj.SelectUserIDIndex();
            Class1.GCF1 =Convert.ToDouble(SUsetupTAobj.GetGCF1());
            Class1.GCF2 = Convert.ToDouble(SUsetupTAobj.GetGCF2());
            Class1.GCF3 = Convert.ToDouble(SUsetupTAobj.GetGCF3());
            //---------------------To be Removed--------------------
            Class1.CurrentP = SUstartupTAobj.SelectStartupProgram();
           //--------------------------------------------------------
            }
            catch
            {
                MessageBox.Show("An error has occurred in Loading Setup");
                return;
            }
          

        }
        public static int[] DItype()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = Class1.Connectionstring;
            int[] Ditype = new int[32];
            SqlDataReader reader=null;
             try
            {
            SqlCommand cmd = new SqlCommand("SELECT DIType FROM dbo.GetDIType", conn);
            conn.Open();
            reader = cmd.ExecuteReader();
            
            int i=0;
           
                while (reader.Read())
                {
                    Ditype[i] = Convert.ToInt32(reader["DIType"].ToString());
                    i++;
                }
            }
            catch (Exception ex)
            { }
           return Ditype;
           conn.Close();
           reader.Close();
           
        }
    
        public static void RecipeUpload()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = Class1.Connectionstring;
            try
            {
                 conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //throw;
            }
            SqlCommand command = new SqlCommand("SELECT Programs.ID,Programs.PressTrig, Programs.TTP, Programs.Gas1, Programs.Gas2, Programs.Gas3, Programs.RFPWR, Programs.RFTime, Programs.TunePos, Programs.Loadpos FROM Programs INNER JOIN Startup ON Programs.ID = Startup.CurrentProgram", conn);
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    Class1.R_ID = reader["ID"].ToString();
                    Class1.R_PT = Math.Round(float.Parse(reader["PressTrig"].ToString()), 2);
                    Class1.R_TTP = float.Parse(reader["TTP"].ToString());
                    Class1.R_G1 = float.Parse(reader["Gas1"].ToString());
                    Class1.R_G2 = float.Parse(reader["Gas2"].ToString());
                    Class1.R_G3 = float.Parse(reader["Gas3"].ToString());
                    Class1.R_PW = float.Parse(reader["RFPWR"].ToString());
                    Class1.R_RFT = float.Parse(reader["RFTime"].ToString());
                    Class1.R_TP = float.Parse(reader["TunePos"].ToString());
                    Class1.R_LP = float.Parse(reader["Loadpos"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occurred in Loading Recipe");
                return;
            }
            finally
            { if(reader!=null)
              {
                reader.Close();
                
              }
            if (conn != null)
            { 
                conn.Close();
            }
       
            }
        }
        public static void ProgRead()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = Class1.Connectionstring;
                    conn.Open();
                    using (SqlCommand command = new SqlCommand("SELECT Programs.ID, Programs.PressTrig, Programs.TTP, Programs.Gas1, Programs.Gas2, Programs.Gas3, Programs.RFPWR, Programs.TunePos, Programs.RFTime, Programs.Loadpos FROM Programs INNER JOIN Startup ON Programs.ID = Startup.CurrentProgram", conn))
                    {

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Class1.progIDDB = reader["ID"].ToString();
                                Class1.PressTrigDB = Math.Round(float.Parse(reader["PressTrig"].ToString()), 2);
                                Class1.TTPDB = float.Parse(reader["TTP"].ToString());
                                Class1.Gas1DB = float.Parse(reader["Gas1"].ToString());
                                Class1.Gas2DB = float.Parse(reader["Gas2"].ToString());
                                Class1.Gas3DB = float.Parse(reader["Gas3"].ToString());
                                Class1.RFPWRDB = float.Parse(reader["RFPWR"].ToString());
                                Class1.RFTimeDB = float.Parse(reader["RFTime"].ToString());
                                Class1.TunePosDB = float.Parse(reader["TunePos"].ToString());
                                Class1.LoadposDB = float.Parse(reader["Loadpos"].ToString());

                            }
                        }
                    }

                }
            }
            catch (Exception)
            {

                MessageBox.Show("An error has occurred in Reading Program");
            }
          
        }
        public static void DefaultSetup()
         {
           
           SqlConnection conn = new SqlConnection();
           conn.ConnectionString = Class1.Connectionstring;
           string query = "SELECT RFrange, Gas1, Gas2, Gas3, Gastype1, Gastype2, Gastype3, GCF1, GCF2, GCF3, PumpdownAlarm, VentingTime, Gasrange1, Gasrange2, Gasrange3 FROM SetupDefault";
            SqlDataReader reader=null;
           try {
           conn.Open();
           SqlCommand cmdRead = new SqlCommand(query, conn);
           reader = cmdRead.ExecuteReader();
           while(reader.Read())
           { 
           Class1.RFrangSetup =Convert.ToDouble(reader["RFrange"].ToString());
           Class1.Gastype1Setup= reader["Gastype1"].ToString();
           Class1.Gasrange1Setup=Convert.ToDouble(reader["Gasrange1"].ToString());
           Class1.Gastype2Setup= reader["Gastype2"].ToString();
           Class1.Gasrange2Setup =Convert.ToDouble(reader["Gasrange2"].ToString());
           Class1.Gastype3Setup = reader["Gastype3"].ToString();
           Class1.Gasrange3Setup =Convert.ToDouble(reader["Gasrange3"].ToString());
           Class1.VentingTimeSetup =Convert.ToInt32(reader["VentingTime"].ToString());
           Class1.PumpdownAlarmSetup =Convert.ToInt32(reader["PumpdownAlarm"].ToString());
          
           }
           }
             catch(Exception ex)
           {
               MessageBox.Show("An error has occurred in Loading Default Setup");
             }
             finally
               {
            if(reader!=null)
            { 
               reader.Close();
            }
               if(conn!=null)
               { 
                conn.Close();
               }
               }
                    
         }
          
        public static void progCheck()
         {
            
             string query = "SELECT ID FROM Programs where ID='"+ Class1.ProgramName +"'";
             try
             {
                 using (SqlConnection conn = new SqlConnection())
                 {
                     conn.ConnectionString = Class1.Connectionstring;
                     conn.Open();
                     using (SqlCommand cmdRead = new SqlCommand(query, conn))
                     {
                         using (SqlDataReader reader = cmdRead.ExecuteReader())
                         {
                             if (reader.HasRows)
                             {
                                 Class1.programIDFlag = true;
                             }
                         }
                     }
                 }
             }
             catch (Exception)
             {

                 MessageBox.Show("An error has occurred in Program Check");
             }
            

         }
        public static void WriteEventLog(string EventCode,string EventDesc)
        {
            StreamWriter FS;
            Class1.NowDate = null;
            Class1.NowDate = DateTime.Now.ToShortDateString();
            Class1.NowDate = Class1.NowDate.Replace("/", "-");
            Class1.NowTime=null;
            Class1.NowTime = DateTime.Now.ToLongTimeString();
            string MainPath = Class1.sProjectPath + " \\LogEvent\\LogEvent" + Class1.NowDate + ".txt";
            try
            {

            
            if(!File.Exists(MainPath))
            {
                using(FS=File.CreateText(MainPath))
                {
                    FS.WriteLine(Class1.NowTime + " > "+ EventCode + " : " + EventDesc + Environment.NewLine);
                }
            }else
            {
                using(FS=File.AppendText(MainPath))
                {
                    FS.WriteLine(Class1.NowTime + " > " + EventCode + " : " + EventDesc + Environment.NewLine);
                }
            }
            }
            catch (Exception)
            {

                MessageBox.Show("An error has occurred in Writing EventLog File");
            }
               
        }
        public static void WriteManualEventLog(string ManualEventCode, string ManualEventDesc)
         {
        StreamWriter FS;
        Class1.NowDate = null;
        Class1.NowDate = DateTime.Now.ToShortDateString();
        Class1.NowDate = Class1.NowDate.Replace("/", "-");
        Class1.NowTime = null;
        Class1.NowTime = DateTime.Now.ToLongTimeString();
        string MainPath = Class1.sProjectPath + " \\ManualLogEvent\\ManualLogEvent" + Class1.NowDate + ".txt";
        try
        {


            if (!File.Exists(MainPath))
            {
                using (FS = File.CreateText(MainPath))
                {
                    FS.WriteLine(Class1.NowTime + " > " + ManualEventCode + " : " + ManualEventDesc + Environment.NewLine);
                }
            }
            else
            {
                using (FS = File.AppendText(MainPath))
                {
                    FS.WriteLine(Class1.NowTime + " > " + ManualEventCode + " : " + ManualEventDesc + Environment.NewLine);
                }
            }
        }
        catch (Exception)
        {

            MessageBox.Show("An error has occurred in Writing EventLog File");
        }


    }
        public static void WriteAlarmLog(string AlarmCodeDesc)
        {
        StreamWriter FS;
        Class1.NowDate = null;
        Class1.NowDate = DateTime.Now.ToShortDateString();
        Class1.NowDate = Class1.NowDate.Replace("/", "-");
        Class1.NowTime = null;
        Class1.NowTime = DateTime.Now.ToLongTimeString();
        string MainPath = Class1.sProjectPath + " \\AlarmLog\\AlarmLog" + Class1.NowDate + ".txt";
        try
        {

        if (!File.Exists(MainPath))
        {
            using (FS = File.CreateText(MainPath))
            {
                FS.WriteLine(Class1.NowTime + " > " + AlarmCodeDesc + Environment.NewLine);
            }
        }
        else
        {
            using (FS = File.AppendText(MainPath))
            {
                FS.WriteLine(Class1.NowTime + " > " + AlarmCodeDesc + Environment.NewLine);
            }
        }
        }
        catch (Exception ex)
        {

            MessageBox.Show("An error has occurred in Writing AlarmLog File");
        }

    }

        public static void WriteManualAlarmLog(string ManualAlarmCodeDesc)
        {
        StreamWriter MFS;
        Class1.NowDate = null;
        Class1.NowDate = DateTime.Now.ToShortDateString();
        Class1.NowDate = Class1.NowDate.Replace("/", "-");
        Class1.NowTime = null;
        Class1.NowTime = DateTime.Now.ToLongTimeString();
        string MainPath = Class1.sProjectPath + " \\ManualAlarmLog\\ManualAlarmLog" + Class1.NowDate + ".txt";
        try
        {

            if (!File.Exists(MainPath))
            {
                using (MFS = File.CreateText(MainPath))
                {
                    MFS.WriteLine(Class1.NowTime + " > " + ManualAlarmCodeDesc + Environment.NewLine);
                }
            }
            else
            {
                using (MFS = File.AppendText(MainPath))
                {
                    MFS.WriteLine(Class1.NowTime + " > " + ManualAlarmCodeDesc + Environment.NewLine);
                }
            }
        }
        catch (Exception ex)
        {

            MessageBox.Show("Error in Writing AlarmLog File");
        }

    }

       
       public static void GetEventCode()
        {
            string[] Code = new string[20];
            string[] Desc = new string[20];
            DataTable EventLogCode = new DataTable();
            int count;
            string query = "SELECT EventLogCode, EventLogDesc FROM tblEventLogCode";
            try
            {
                if (Class1.conn.State == ConnectionState.Closed)
                {
                    Class1.conn.Open();
                }
             using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = Class1.Connectionstring;
                conn.Open();
                SqlDataAdapter DA = new SqlDataAdapter(query,conn);
                DataSet DS = new DataSet();
                DA.Fill(DS, "tblEventLogCode");
                EventLogCode = DS.Tables["tblEventLogCode"];
                count = EventLogCode.Rows.Count;
                for(int i=0;i<count;i++)
                {
                    Code[i] = EventLogCode.Rows[i]["EventLogCode"].ToString();
                    Desc[i] = EventLogCode.Rows[i]["EventLogDesc"].ToString();
                }
            }
            }
            catch (Exception Ex)
            {

                MessageBox.Show("An error has occurred in Event Code Load");
            }
            Class1.ConnectionOkCode = Code[0];
            Class1.ConnectionOkDesc = Desc[0];
            Class1.ConnectionFailCode = Code[1];
            Class1.ConnectionFailDesc = Desc[1];
            Class1.LoginSuccessfullyCode = Code[2];
            Class1.LoginSuccessfullyDesc = Desc[2];
            Class1.LoginFailedCode = Code[3];
            Class1.LoginFailedDesc = Code[3];
            Class1.SetupLoadedCode = Code[4];
            Class1.SetupLoadedDesc = Desc[4]; 
            Class1.RecipeLoadedCode = Code[5];
            Class1.RecipeLoadedDesc = Desc[5];
            Class1.AutoPageStartedCode = Code[6];
            Class1.AutoPageStartedDesc = Desc[6];
               

               
           }

       public static void LogEventDB(string EventCode, string EventDesc)
       {
           StreamWriter FS;
           string EventLogDateTime;
           int RowCount;
           int Rows;
           EventLogDateTime = DateTime.Now.ToString();
           DataTable EventLog = new DataTable();
           EventLogDateTime = EventLogDateTime.Replace("/", "-");
           string cmdString = "INSERT INTO tblEventLog(ELCode, ELDateTime, ELDescription) VALUES ('" + EventCode + "','" + EventLogDateTime + "','" + EventDesc + "')";
           string selectQuery = "select count(*) from tblEventLog";
          
           try {
               if (Class1.conn.State == ConnectionState.Closed)
               {
                   Class1.conn.Open();
               }
           if(Class1.conn.State==ConnectionState.Open)
          {
             using (SqlCommand cmd = new SqlCommand(cmdString, Class1.conn))
              {
                  cmd.ExecuteNonQuery(); 
              }
              using (SqlCommand cmdRead = new SqlCommand(selectQuery, Class1.conn))
               {
                   RowCount =Convert.ToInt16(cmdRead.ExecuteScalar());
               }

               if (RowCount > 200)
               {
                   Rows = RowCount - 200;
                   string queryDel = "DELETE top("+Rows+") FROM tblEventLog where  ELDateTime in (SELECT TOP ("+Rows+") ELDateTime FROM tblEventLog ORDER BY ELDateTime)";
                   using (SqlCommand cmdDel = new SqlCommand(queryDel, Class1.conn))
                   {
                       cmdDel.ExecuteNonQuery();
                   }
                   
               }
               EventAccess();
               if(Class1.MainLoadFlg==true)
               {
                   Thread.Sleep(100);
                   DGVDataAccess();
               }
            }
        }
           catch(SystemException ex)
           {
               MessageBox.Show("An error has occurred in LogEventDB");
               
           }
      }

       public static void ManualLogEventDB(string ManualEventCode, string ManualEventDesc)
       {
           StreamWriter MFS;
           string ManualEventLogDateTime;
           int ManualRowCount;
           int ManualRows;
           ManualEventLogDateTime = DateTime.Now.ToString();
           ManualEventLogDateTime = ManualEventLogDateTime.Replace("/", "-");
           string cmdString = "INSERT INTO tblManualEventLog(MELCode, MELDateTime, MELDescription) VALUES ('" + ManualEventCode + "','" + ManualEventLogDateTime + "','" + ManualEventDesc + "')";
           try
           {
               if(Class1.conn.State == ConnectionState.Closed)
               {
                   Class1.conn.Open();
               }
               if (Class1.conn.State == ConnectionState.Open)
               {
                   using (SqlCommand cmdManual = new SqlCommand(cmdString, Class1.conn))
                   {
                       cmdManual.ExecuteNonQuery();
                   }

               }
           }
           catch (SystemException ex)
           {
               MessageBox.Show("An error has occurred in Manual LogEventDB");
               
           }

           string ManualEventLogSelectQuery = "SELECT TOP (1) MELDateTime, MELCode, MELDescription FROM tblManualEventLog ORDER BY MELDateTime DESC";
           Class1.GBEventLogtxt.Text = null;
           try
           {
               if(Class1.conn.State == ConnectionState.Closed)
               {
                   Class1.conn.Open();
               }
               if (Class1.conn.State == ConnectionState.Open)
               {
                   using (SqlCommand cmdManual = new SqlCommand(ManualEventLogSelectQuery, Class1.conn))
                   {
                       using (SqlDataReader reader = cmdManual.ExecuteReader())
                       {
                           if (reader.HasRows)
                           {
                               reader.Read();
                               Class1.GBEventLogtxt.Text = reader["MELDateTime"].ToString() + Environment.NewLine+  reader["MELCode"].ToString() + "  > " + reader["MELDescription"].ToString();
                           }
                       }
                   }
               }
           }
           catch (Exception)
           {

               MessageBox.Show("An error has occurred in Manual LogEventDB Load");
           }

       }
       public static void ManualELMaintenance()
       {
           string selectQuery = "select count(*) from tblManualEventLog";
           int RowCount,Rows;
           try
           {
               if (Class1.conn.State == ConnectionState.Closed)
                   {
                       Class1.conn.Open();
                   }
               if (Class1.conn.State == ConnectionState.Open)
                {
                    using (SqlCommand cmdRead = new SqlCommand(selectQuery, Class1.conn))
                    {
                        RowCount = Convert.ToInt16(cmdRead.ExecuteScalar());
                    }

                    if (RowCount > 200)
                    {
                        Rows = RowCount - 200;
                        string queryDel = "DELETE top(" + Rows + ") FROM tblManualEventLog where  MELDateTime in (SELECT TOP (" + Rows + ") MELDateTime FROM tblManualEventLog ORDER BY MELDateTime)";
                        using (SqlCommand cmdDel = new SqlCommand(queryDel, Class1.conn))
                        {
                            cmdDel.ExecuteNonQuery();
                        }

                    }
                }
           }
           catch (Exception)
           {

               MessageBox.Show("An error has occurred in Manual Event Log Maintenance");
           }
          
       }
        public static void ConnOpen()
       {
           try
           { Class1.conn = new SqlConnection();
                 if (Class1.conn.State == ConnectionState.Closed)
                   {
                      
                       Class1.conn.ConnectionString = Class1.Connectionstring;
                       Class1.conn.Open();
                   }
                   else
                   {
                       Class1.conn.Close();
                       Class1.conn.Open();
                   }
               
                             
           }
           catch (Exception)
           {

               MessageBox.Show("An error has occurred in Openning Database Connection");
               if(Class1.conn.State==ConnectionState.Open)
               {
                   Class1.conn.Close();
               }
               
           }
       }

        public static void ConnClose()
        {
            try
            {
                if(Class1.conn!=null)
                {
                    if(Class1.conn.State==ConnectionState.Open)
                    {
                        Class1.conn.Close();
                    }
                }
            }
            catch (Exception)
            {

                Class1.conn.Close();
                Class1.conn = null;
                MessageBox.Show("An error has occurred in Openning Database Connection");
            }
        }
        public static void EventAccess()
        {
            string EventSelectQuery = "DELETE FROM tblEventLogAccess";
            try
            {
                if (Class1.conn.State == ConnectionState.Closed)
                {
                    Class1.conn.Open();
                }
                if(Class1.conn.State==ConnectionState.Open)
            {
                using(SqlCommand cmdSelect=new SqlCommand(EventSelectQuery,Class1.conn))
                {
                    cmdSelect.ExecuteNonQuery();
                }
            }
                
            }
            catch (Exception)
            {

                MessageBox.Show("An error has occurred in Event Access Delete");
            }
            try
            {
                if (Class1.conn.State == ConnectionState.Closed)
                {
                    Class1.conn.Open();
                }
                if (Class1.conn.State == ConnectionState.Open)
                { 
            string cmdStringEventAccess = "INSERT INTO tblEventLogAccess SELECT * FROM tblEventLog";
            using (SqlCommand cmdAccess = new SqlCommand(cmdStringEventAccess,Class1.conn))
            {
                cmdAccess.ExecuteNonQuery();
            }
            }
            }
            catch (Exception)
            {

                MessageBox.Show("An error has occurred in data insert in Event Access");
            }
           
        }

        public static void DGVDataAccess()
        {
            DataTable DataAccess = new DataTable();
            
            try
            {
                if (Class1.conn.State == ConnectionState.Closed)
                {
                    Class1.conn.Open();
                }
                if(Class1.conn.State==ConnectionState.Open)
                {
                    
                    string query = "SELECT  ELDateTime, ELCode, ELDescription FROM tblEventLogAccess ORDER BY ELDateTime DESC";
                    
                          using( SqlDataAdapter DA = new SqlDataAdapter(query, Class1.conn))
                          {
                            DataSet DS = new DataSet();
                            DA.Fill(DS, "tblEventLogAccess");
                            DataAccess = DS.Tables["tblEventLogAccess"];
                            if(Class1.DGVMain.InvokeRequired)
                            {
                                Class1.DGVMain.Invoke((MethodInvoker)delegate { Class1.DGVMain.DataSource = DataAccess; Class1.DGVMain.Columns[0].Width = 180; Class1.DGVMain.Columns[0].HeaderText = "Date/Time"; Class1.DGVMain.Columns[1].Width = 80; Class1.DGVMain.Columns[1].HeaderText = "Code"; Class1.DGVMain.Columns[2].Width = 235; Class1.DGVMain.Columns[2].HeaderText = "EventLog"; });
                                //Class1.DGVMain.Invoke((MethodInvoker)delegate { Class1.DGVMain.DataSource = DataAccess;  Class1.DGVMain.Columns[0].HeaderText = "Date/Time";  Class1.DGVMain.Columns[1].HeaderText = "Code";  Class1.DGVMain.Columns[2].HeaderText = "EventLog"; });
                            }
                            else
                            {
                                Class1.DGVMain.DataSource = DataAccess; Class1.DGVMain.Columns[0].Width = 180; Class1.DGVMain.Columns[0].HeaderText = "Date/Time"; Class1.DGVMain.Columns[1].Width = 80; Class1.DGVMain.Columns[1].HeaderText = "Code"; Class1.DGVMain.Columns[2].Width = 235; Class1.DGVMain.Columns[2].HeaderText = "EventLog";
                                //Class1.DGVMain.DataSource = DataAccess; Class1.DGVMain.Columns[0].HeaderText = "Date/Time"; Class1.DGVMain.Columns[1].HeaderText = "Code";  Class1.DGVMain.Columns[2].HeaderText = "EventLog";
                            }
                            
                            
                          }
                            
                        }
                   }
            catch 
            {
                MessageBox.Show("An error has occurred in Loading EventLog in datagridview");
        
            }
        }

        public static void DBAutoPPstatusRead()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = Class1.Connectionstring;
            string DBAutoPlasmaPrcessQuery = "Select RFFlag from tblPlasmaProcess";
            try
            {
                conn.Open();
                using (SqlCommand cmdPP = new SqlCommand(DBAutoPlasmaPrcessQuery, Class1.conn))
                {
                    using (SqlDataReader reader = cmdPP.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Class1.RFFlag = Convert.ToBoolean(reader["RFFlag"]);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
      


        public static bool LastUserLoginReload()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = Class1.Connectionstring;
            SqlCommand command = new SqlCommand("SELECT * FROM Users Where Users.IsLoginedIn = 'True'", conn);// only one user can be logined in at each time
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    Class1.TheUser = reader["UUser"].ToString();
                    Class1.Password = reader["Password"].ToString();
                    Class1.UManual = reader["Manual"].ToString();
                    Class1.UUser = reader["Users"].ToString();
                    Class1.USetupPage = reader["Setup"].ToString();
                    Class1.UProgram = reader["Programs"].ToString();
                    Class1.IsLoginedIn = Convert.ToBoolean(reader["IsLoginedIn"].ToString());
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();

                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return false;
        }

        public static void UpdateUserLoginedIn(string user, bool isLoginedIn)
        {
            try
            {
                userTAobj.UpdateLoginInfo_User(isLoginedIn, user);
            }
            catch (Exception ex)
            {
                MessageBox.Show("UpdateUserLoginedIn failed!!");
            }
        }

        public static void setAOValues(string value, int idx)
        {
            if (Avantech.bModbusConnected)
            { 
            AvantechAOs.m_bAOValueModified[idx] = false;// b_AOValueModified = false;
            if (!AvantechAOs.CheckControllable())
            {
                //lblAOErr.Text = "-1";
                return;
            }
            Class1.txtAOOutputVals[idx].Text = value;
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
                    RefreshAnalogOutputValues(fHigh, fLow, fVal, idx);// refresh special index of analog output panel

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
            RefreshAODataValues();//RefreshData();
            string strInfo = string.Format("Set output AO_{0} value done!", idx);
           // MessageBox.Show(strInfo, "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);

            Avantech.AOEnabled = true;//timer1.Enabled = true;
            }
        }

        public static void RefreshAnalogOutputValues(float fHigh, float fLow, float fOutputVal, int idx)
        {
            Class1.AOHighVals[idx].Text = fHigh.ToString();
            Class1.AOLowVals[idx].Text = fLow.ToString();
            Class1.txtAOOutputVals[idx].Text = fOutputVal.ToString("0.000");// textbox refresh
            Class1.AOOutputVals[idx].Text = fOutputVal.ToString("0.000");// output label refresh
            Class1.AOTrackBarVals[idx].Value = Convert.ToInt32(Class1.AOTrackBarVals[idx].Minimum + (Class1.AOTrackBarVals[idx].Maximum - Class1.AOTrackBarVals[idx].Minimum) * (fOutputVal - fLow) / (fHigh - fLow));
        }

        public static bool RefreshAODataValues()
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
                    RefreshAnalogOutputValues(fHighVals[j], fLowVals[j],
                    AnalogOutput.GetScaledValue(AvantechAOs.m_usRanges[j], usVal[j]), j);
                    //RefreshOutputPanel(fHighVals[j], fLowVals[j], AnalogOutput.GetScaledValue(m_usRanges[j], usVal[j]));
                }
            }

            return true;
        }

       

       
    }

}
