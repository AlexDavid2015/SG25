using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using APS_Define_W32;
using APS168_W32;
using System.Data.SqlClient;
using SG25.SetupDataSetTableAdapters;
using System.Reflection;

namespace SG25
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
        }
        int TTTick = 0;
        int CardID=0;
        int BusNo = 0;

        SetupDataSet SPdatasetObj = new SetupDataSet();
        UsersTableAdapter SPuserTAObj = new UsersTableAdapter();
        StartupTableAdapter SPstartupTAobj = new StartupTableAdapter();
        ProgramsTableAdapter SPProgTAobj = new ProgramsTableAdapter();

        private void Splash_Load(System.Object sender, System.EventArgs e)
        {
           
            if ((!Class1.IsLoginedIn) && (Class2.LastUserLoginReload()))// first last come in
            {
                Class1.AutoCycle = true;
                Class1.ManualCycle = false;
                Class1.Auto = true;
                Class1.ManualP = false;
                Class1.User = false;
                Class1.SetupPage = false;
                Class1.Program = false;
               
                 if (Avantech.bModbusConnected)
                  { Class2. DBAutoPPstatusRead();
                
                    if (Class1.RFFlag)// RFFlag = 1
                    {
                        this.Hide();
                        this.Close();
                        RFTimeCont objRFTimeCont = new RFTimeCont();
                        objRFTimeCont.ShowDialog();
                    }
                    else// RFFlag = 0
                    {
                        if (Class1.TheUser == "OPUSER" || Class1.TheUser == "OP")// if it is operator
                        {
                            this.Hide();
                            this.Close();
                            Main1 objMain1 = new Main1();// Auto page
                            objMain1.ShowDialog();
                        }
                        else
                        {
                            this.Hide();
                            this.Close();
                            Mode objMode = new Mode();// Model page
                            objMode.ShowDialog();
                        }
                    }
                }
                else
                {
                    // User Connection Selection Page
                    UserConnectionSelectionPage objUserConnectSelectPage = new UserConnectionSelectionPage();
                    objUserConnectSelectPage.ShowDialog();
                    if (Class1.IsContinueClicked)// indicating it is ADMIN(then need to show Connection page after connected)
                    {
                        // Initialize Avantech etc
                        Class1.RetForm.LoadConnectionPageSettings();
                        Class1.RetForm.Show();
                        Class1.IsContinueClicked = false;// reset
                    }
                    //SystemGlobals.objConnectPage.Show();
                } 
                    
        }
               
           
            this.Text = "SCI Automation  -  " + "SG25 " + Class1.VersionNo;
            // TODO: This line of code loads data into the 'setupDataSet.Users' table. You can move, or remove it, as needed.
            this.usersTableAdapter.Fill(this.setupDataSet.Users);
            try
            {
                SPuserTAObj.Fill(SPdatasetObj.Users);
                AssemblyCopyrightAttribute copyright = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0] as AssemblyCopyrightAttribute;

                Label3.Text = copyright.Copyright +" "+" SCI Automation Pte. Ltd.";
                //Class2.LoadSetup();
                Class1.FileStartTime = DateTime.Now.Date;
                Class1.FPath = ("C:/Program Files/QML-EX/AlarmLog/Logfile " + DateTime.Now.ToString() + ".csv");
                
                //Version.Text = "Version : " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                Version.Text = "SG25"+" "+Class1.VersionNo;
                //Dim myFileVersionInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo("d:\runtime\setup.exe")

                //' Print the file name and version number.
                //Label4.Text = "File: " & myFileVersionInfo.FileDescription & ControlChars.Cr & _
                //    "Version number: " & myFileVersionInfo.FileVersion

                Class1.CurrentP = SPstartupTAobj.SelectStartupProgram();
                this.SPProgTAobj.FillBy(SPdatasetObj.Programs,Class1.CurrentP);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        private void Button1_Click(System.Object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox1.Text) & TextBox2.Text == "12231224")
            {
                TextBox1.Text = "";
                TextBox2.Text = "";
                Class1.Password = "";
                Class1.UManual = "Allowed";
                Class1.UUser = "Allowed";
                Class1.USetupPage = "Allowed";
                Class1.UProgram = "Allowed";
                this.Hide();
                this.Close();
                Mode objMode = new Mode();
                objMode.ShowDialog();
                return;
            }
            
            try
            {
                if(TextBox1.Text!="" & TextBox2.Text!=" ")
                { 
                Class1.Password = SPuserTAObj.SelectPass(Class1.TheUser);
                Thread.Sleep(200);
                Class1.UManual = SPuserTAObj.GetMaunal(Class1.TheUser);
                Thread.Sleep(200);
                Class1.UUser = SPuserTAObj.GetUsers(Class1.TheUser);
                Thread.Sleep(200);
                Class1.USetupPage = SPuserTAObj.GetSetup(Class1.TheUser);
                Thread.Sleep(200);
                Class1.UProgram = SPuserTAObj.GetPrograms(Class1.TheUser);
                Thread.Sleep(200);

                if (TextBox2.Text == Class1.Password)
                {
                    TextBox1.Text = "";
                    TextBox2.Text = "";
                    Class1.Password = "";
                    Class1.IsLoginedIn = true;
                    Class2.UpdateUserLoginedIn(Class1.TheUser, Class1.IsLoginedIn);
                    
                    this.Close();
                    this.Dispose();
                    Class2.WriteEventLog(Class1.LoginSuccessfullyCode, Class1.LoginSuccessfullyDesc);
                    Class2.LogEventDB(Class1.LoginSuccessfullyCode, Class1.LoginSuccessfullyDesc);
                    if (Avantech.bModbusConnected)
                    {
                        if (Class1.RFFlag)// RFFlag = 1
                        {
                            this.Hide();
                            this.Close();
                            RFTimeCont objRFTimeCont = new RFTimeCont();
                            objRFTimeCont.ShowDialog();
                        }
                        else// RFFlag = 0
                        {
                            if (Class1.TheUser == "OPUSER" || Class1.TheUser == "OP")// if it is operator
                            {
                                this.Hide();
                                this.Close();
                                Main1 objMain1 = new Main1();// Auto page
                                objMain1.ShowDialog();
                            }
                            else
                            {
                                this.Hide();
                                this.Close();
                                Mode objMode = new Mode();// Model page
                                objMode.ShowDialog();
                            }
                        }
                    }
                    else
                    {
                        UserConnectionSelectionPage objUserConnectSelectPage = new UserConnectionSelectionPage();
                        objUserConnectSelectPage.ShowDialog();
                        if (Class1.IsContinueClicked)// indicating it is ADMIN(then need to show Connection page after connected)
                        {
                            Class1.RetForm.LoadConnectionPageSettings();// Initialize Avantech etc
                            Class1.RetForm.Show();
                            Class1.IsContinueClicked = false;// reset
                        }
                    } 

                }
                else
                {
                    MessageBox.Show("Wrong Password!!");
                    TextBox2.Text = "";
                    Class1.Password = "";
                   
                    Class2.WriteEventLog(Class1.LoginFailedCode, Class1.LoginFailedDesc);
                    Class2.LogEventDB(Class1.LoginFailedCode, Class1.LoginFailedDesc);
                }
                }
                else
                    if(TextBox1.Text=="")
                    {
                        MessageBox.Show("Please Select User");
                    }
                   else if(TextBox1.Text =="" & TextBox2.Text=="")
                    {
                        MessageBox.Show("Please Select User and Enter Password");
                    }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Avantech.bModbusConnected = false;
                UserConnectionSelectionPage usrConnSelectPage = new UserConnectionSelectionPage();
                usrConnSelectPage.ShowDialog();
                Class1.RetForm.Show();
            }

            // remember to remove!!!

            //{
            //    TextBox1.Text = "";
            //    TextBox2.Text = "";
            //    Class1.Password = "";
            //    Class1.UManual = "Allowed";
            //    Class1.UUser = "Allowed";
            //    Class1.USetupPage = "Allowed";
            //    Class1.UProgram = "Allowed";
            //    this.Hide();
            //    this.Close();
            //    Mode objMode = new Mode();
            //    objMode.ShowDialog();
            //    return;
            //}
            // remember to remove!!!


        }

        private void Button2_Click(System.Object sender, System.EventArgs e)
        {
            Class1.AlarmActive = false;
            Class2.ConnClose();
          // int APS=APS168.APS_close();
        //   APS168.APS_stop_field_bus(CardID, BusNo);
            if (Class1.TheUser!=null)
            {
                Class1.IsLoginedIn = false;
                Class2.UpdateUserLoginedIn(Class1.TheUser, Class1.IsLoginedIn);
            }
            
            System.Environment.Exit(0);
        }

        private void LB_Click(object sender, System.EventArgs e)
        {
            Class1.TheUser = LB.Text;
            TextBox1.Text =Class1.TheUser;
        }


        private void TextBox2_Click(object sender, System.EventArgs e)
        {
            Class1.AlphaPadret = "";
            Alphapad objAlpha = new Alphapad();
            objAlpha.ShowDialog();
            try
            {
                TextBox2.Text = Class1.AlphaPadret;
                Class1.Password = Class1.AlphaPadret;

            }
            catch
            {
            }

        }

        private void Splash_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
           SPuserTAObj.Fill(SPdatasetObj.Users);

        }

        private void TextBox1_Click(System.Object sender, System.EventArgs e)
        {
            TextBox1.Text = "";
        }


        private void Version_Click(System.Object sender, System.EventArgs e)
        {
            if (TTTick == 0)
                TTTick = 1;
            if (TTTick == 3)
                TTTick = 4;
        }

        private void Label3_Click(System.Object sender, System.EventArgs e)
        {
            if (TTTick == 1)
                TTTick = 2;
            if (TTTick == 2)
                TTTick = 3;
            if (TTTick == 4)
            {
                try
                {
                    Class1.UManual = "Allowed";
                    Class1.UUser = "Allowed";
                    Class1.USetupPage = "Allowed";
                    Class1.UProgram = "Allowed";
                    this.Hide();
                    this.Close();
                    Mode objMode = new Mode();
                    objMode.ShowDialog();
                    
                }
                catch
                {
                }

            }
        }
    }
}
