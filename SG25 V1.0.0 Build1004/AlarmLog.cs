using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using SG25.SetupDataSetTableAdapters;
using System.IO;


namespace SG25
{
    public partial class AlarmLog : Form
    {
        public AlarmLog()
        {
            InitializeComponent();
        }

        private void AlarmLog_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            Timer1.Enabled = false;
        }

        SetupDataSet AlarmSetupDSobj = new SetupDataSet();
        AlarmLogTableTableAdapter AlarmTblAdpObj = new AlarmLogTableTableAdapter();

        

        private void AlarmLog_Load(System.Object sender, System.EventArgs e)
        {
            Timer1.Enabled = true;
            // TODO: This line of code loads data into the 'setupDataSet.AlarmLogTable' table. You can move, or remove it, as needed.
            this.alarmLogTableTableAdapter.Fill(this.setupDataSet.AlarmLogTable);
            // TODO: This line of code loads data into the 'setupDataSet.AlarmLogTable' table. You can move, or remove it, as needed.
            this.alarmLogTableTableAdapter.Fill(this.setupDataSet.AlarmLogTable);
            AlarmTblAdpObj.Fill(AlarmSetupDSobj.AlarmLogTable);
            
            DGV.Sort(alarmDateTimeDataGridViewTextBoxColumn, System.ComponentModel.ListSortDirection.Descending);
        }




        private void AlarmHandler()
        {

            AlarmTblAdpObj.Fill(AlarmSetupDSobj.AlarmLogTable);

            switch (Class1.SendAlarm)
            {
                case 0:
                    break; // TODO: might not be correct. Was : Exit Select

                    break;
                case 1:
                    Class1.AlarmMsg = "# 1 - No Air";
                    AlarmTblAdpObj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 1, "No Air", "Check the Air connection, valve and sensor.");
                    break;
                case 2:
                    Class1.AlarmMsg = "# 2 - Pump Error";
                    AlarmTblAdpObj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now , 2, "Pump Error", "Check the Vacuum Pump connection and temperature.");
                    break;
                case 3:
                    Class1.AlarmMsg = "# 3 - No Plasma";
                    AlarmTblAdpObj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 3, "No Plasma", "Check the Program setting, Tuner and RF generator.");
                    break;
                case 4:
                    Class1.AlarmMsg = "# 4 - High Reflected Power";
                    AlarmTblAdpObj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 4, "High Reflective Power", "Check the Program setting, Tuner and RF generator.");
                    break;
                case 5:
                    Class1.AlarmMsg = "# 5 - No Gas 1";
                    AlarmTblAdpObj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 5, "No Gas 1", "Check the Gas 1 supply line and bottle.");
                    break;
                case 6:
                    Class1.AlarmMsg = "# 6 - No Gas 2";
                    AlarmTblAdpObj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 6, "No Gas 2", "Check the Gas 2 supply line and bottle.");
                    break;
                case 7:
                    Class1.AlarmMsg = "# 7 - No Gas 3";
                    AlarmTblAdpObj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 7, "No Gas 3", "Check the Gas 3 supply line and bottle.");
                    break;
                case 8:
                    Class1.AlarmMsg = "# 8 - PumpDown Error";
                    AlarmTblAdpObj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 8, "Pump Down Error", "Check the program settings,the chamber O-Rings and the Pump.");
                    break;
                case 9:
                    Class1.AlarmMsg = "# 9 - Door Sensor Error";
                    AlarmTblAdpObj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 9, "Door Sensor Error", "You cannot Start with the door open; or, check the door sensor.");
                    break;
                case 10:
                    Class1.AlarmMsg = "# 10 - E-Stop";
                    AlarmTblAdpObj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 10, "E-Stop", "The E-Stop is engaged.");
                    break;
                case 11:
                    Class1.AlarmMsg = "# 11 - MFC 1";
                    AlarmTblAdpObj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 11, "MFC 1", "Check Gas Valve 1 Or, Check the mass flow Controller.");
                    break;
                case 12:
                    Class1.AlarmMsg = "# 12 - MFC 2";
                    AlarmTblAdpObj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 12, "MFC 2", "TCheck Gas Valve 1 Or, Check the mass flow Controller.");
                    break;
                case 13:
                    Class1.AlarmMsg = "# 13 - MFC 3";
                    AlarmTblAdpObj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 13, "MFC 3", "Check Gas Valve 1 Or, Chec the mass flow Controller.");
                    break;
                case 14:
                    Class1.AlarmMsg = "# 14 - Pres.Trig. Timeout";
                    AlarmTblAdpObj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 14, "Pres.Trig. Timeout", "The program Settings have too mach gas and too low Pressure Trigger.");
                    break;
                case 15:
                    Class1.AlarmMsg = "# 15 - Venting Time Timeout";
                    AlarmTblAdpObj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 15, "Venting Time Timeout", "Check the Venting Time in Setup, Or the Purge supply is empty or blocked");
                    break;
                case 16:
                    Class1.AlarmMsg = "# 16 - Reaching Plasma Pressure Timeout";
                    AlarmTblAdpObj.InsertNewAlarmLog(Class1.CurrentP, DateTime.Now, 16, "Reaching Plasma Pressure Timeoutt", "The System Cannot reach the Pressure trigger Point set in your recipe. Please check if the Gas value set in the recipe is correct, or check the gas lines.");

                    break;
            }
            //fileAlarm = System.IO.File.Create("c:\Filelog" & Now.ToString & ".txt")

        }


        private void TextBox2_TextChanged(System.Object sender, System.EventArgs e)
        {
            //This textbox must be changed to the routine that gets the alarm status from the machine
            // SendAlarm = Val(TextBox2.Text)
            //RaiseAlarm(SendAlarm)

        }

        private object RaiseAlarm(int Alarm)
        {

            AlarmHandler();
            try
            {
                if (Class1.AutoCycle == true)
                {
                    Main1 objMain1 = new Main1();
                    objMain1.AlarmThread.RunWorkerAsync();
                    //Main.AlarmThread.RunWorkerAsync();
                }
                if (Class1.ManualCycle == true)
                {
                    Manual objManual = new Manual();
                    objManual.AlarmThread.RunWorkerAsync();
                 }

                Main1 objMain = new Main1();
                objMain.WirteLogThread.RunWorkerAsync();
            }
            catch
            {
            }
            return 0;
        }

        private void ToolStripButton2_Click(System.Object sender, System.EventArgs e)
        {
            this.Close();
            this.Dispose();

            Mode objMode = new Mode();
            objMode.ShowDialog();
        }


        private void ToolStripButton3_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                DGV.FirstDisplayedScrollingRowIndex = DGV.FirstDisplayedScrollingRowIndex - 20;
            }
            catch
            {
            }

        }


        private void ToolStripButton4_Click(System.Object sender, System.EventArgs e)
        {

            try
            {
                DGV.FirstDisplayedScrollingRowIndex = DGV.FirstDisplayedScrollingRowIndex + 20;
                
            }
            catch
            {
            }

        }


        private void ToolStripButton5_Click(System.Object sender, System.EventArgs e)
        {
            OpenFileDialog MyDialog = new OpenFileDialog();
            Stream myStream = null;

            MyDialog.Title = "Open Alarm Log File";
            MyDialog.InitialDirectory = "C:/Program Files/QML-EX/AlarmLog";
           // MyDialog.Filter = "txt files (*.txt)|*.txt";
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

        private void ToolStripButton7_Click(System.Object sender, System.EventArgs e)
        {
            TextBox1.Text = "";
        }

        private void Timer1_Tick(System.Object sender, System.EventArgs e)
        {
            int y = 0;
            int sum = 0;
            if (Convert.ToInt32(BN1.CountItem.Text.Substring(3))>250)
             {
                int ret = 0;
                ret = AlarmTblAdpObj.Fill(AlarmSetupDSobj.AlarmLogTable);
                if (ret > 250)
                {
                    sum = AlarmTblAdpObj.Fill(AlarmSetupDSobj.AlarmLogTable);
                    try
                    {
                        for (y = sum; y >= 200; y += -1)
                        {
                            DGV.Sort(alarmDateTimeDataGridViewTextBoxColumn, System.ComponentModel.ListSortDirection.Ascending);
                            BN1.MoveFirstItem.Select();
                            BN1.MoveLastItem.Select();
                            AlarmTblAdpObj.DeleteTop1FromAlarmlogtable();
                            DGV.Rows.Remove(DGV.CurrentRow);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        DGV.Sort(alarmDateTimeDataGridViewTextBoxColumn, System.ComponentModel.ListSortDirection.Descending);
                        this.Invalidate();
                    }
                }
            }
        }

              

    }
}
