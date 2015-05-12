using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using APS_Define_W32;
using APS168_W32;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
namespace SG25
{
    public static class Class1
    {
        //SG25
        #region Slot Num
        public static int DISlotNum = 1;// 5040 DI slot num = 1
        public static int DOSlotNum = 3;// 5046 DO slot num = 3
        public static int AOSlotNum = 5;// 5028 AO slot num = 5
        public static int AISlotNum = 7;// 5017H AI slot num = 7
        public static int DIOSlotNum = 9;// 5045 DI/O slot num = 9
        #endregion
        
        #region DIDO Array
        public static bool[] DOArrayValues = new bool[24];
        public static bool[] DOIArrayValues = new bool[36];
        public static bool[] DIArrayValues = new bool[24];
        public static bool[] DIOArrayValues = new bool[36];
        #endregion

        #region DB Connection String
        public static string Connectionstring = ConfigurationManager.ConnectionStrings["SG25ConnectionString"].ConnectionString;
        public static SqlConnection conn;
 #endregion

        #region AutoPage
        public static DataGridView DGVMain;
        public static bool MainLoadFlg;
        public static bool IsContinueClicked = false;
        public static Button btnAutoStart;
        public static LogEventAuto objLogevent;
        #endregion

        #region ManualPage
        public static RichTextBox GBEventLogtxt;
        #endregion
        #region Form Objects & Connect Page Object & Variable
        public static ConnectPage RetForm;
        public static Form RetFormManual;
        //public static bool IOInfoForm;
        public static Form IOShowInfoForm;
        public static Form openIO = IOInfo.Instance();
        public static Leak objLeak;
        public static ConnectPage Connobj;
        public static bool connectedObjFlg=false;
        public static FirstRun FirstRunObj;
        public static int DisconnectTimeElapsedSecs = 0;
        public static System.Windows.Forms.Timer DOOutputTimerGlobal;
        public static bool TunePageConnected;
        
       
        #endregion

        #region Proj Path, Version and Date
        public static string VersionNo;
        public static string sProjectPath;
        public static string NowDate;
        public static string NowTime;
        #endregion

        #region EventCodeTable Global Variables
        public static string ConnectionOkCode;
        public static string ConnectionOkDesc;
        public static string ConnectionFailCode;
        public static string ConnectionFailDesc;
        public static string LoginSuccessfullyCode;
        public static string LoginSuccessfullyDesc;
        public static string LoginFailedCode;
        public static string LoginFailedDesc;
        public static string AutoPageStartedCode;
        public static string AutoPageStartedDesc;
        public static string SetupLoadedCode;
        public static string SetupLoadedDesc;
        public static string RecipeLoadedCode;
        public static string RecipeLoadedDesc;
        

        #endregion



        #region IO Page
        public static bool IOStopped;
        public static bool IOOpen;
        public static bool IOInfoSamePg;
        public static bool IOFlg;
        #endregion

        #region DODI - DO Channel Bit
        //public static int DO24 = AvantechDIOs.m_iDoOffset;
        //public static int DO25 = AvantechDIOs.m_iDoOffset+1;
        //public static int DO26 = AvantechDIOs.m_iDoOffset+2;
        //public static int DO27 = AvantechDIOs.m_iDoOffset+3;
        //public static int DO28 = AvantechDIOs.m_iDoOffset+4;
        //public static int DO29 = AvantechDIOs.m_iDoOffset+5;
        //public static int DO30 = AvantechDIOs.m_iDoOffset+6;
        //public static int DO31 = AvantechDIOs.m_iDoOffset+7;
        //public static int DO32 = AvantechDIOs.m_iDoOffset+8;
        //public static int DO33 = AvantechDIOs.m_iDoOffset+9;
        //public static int DO34 = AvantechDIOs.m_iDoOffset + 10;
        //public static int DO35 = AvantechDIOs.m_iDoOffset + 11;
       

        public static int DO24 = 12;
        public static int DO25 = 13;
        public static int DO26 = 14;
        public static int DO27 = 15;
        public static int DO28 = 16;
        public static int DO29 = 17;
        public static int DO30 = 18;
        public static int DO31 = 19;
        public static int DO32 = 20;
        public static int DO33 = 21;
        public static int DO34 = 22;
        public static int DO35 = 23;
       

        #endregion

        #region AutoCycle
        public static bool RFFlag;// indicate whether RF has already been on before
        public static int RFTimeAuto = 10;
        public static bool IsLoginedIn;
        #endregion

        #region Analog output
        public static Label[] AOHighVals = new Label[8];// AvantechAOs.m_aConf.HwIoTotal[AvantechAOs.m_tmpidx]
        public static Label[] AOLowVals = new Label[8];// AvantechAOs.m_aConf.HwIoTotal[AvantechAOs.m_tmpidx]
        public static Label[] AOOutputVals = new Label[8];// AvantechAOs.m_aConf.HwIoTotal[AvantechAOs.m_tmpidx]
        public static TextBox[] txtAOOutputVals = new TextBox[8];// AvantechAOs.m_aConf.HwIoTotal[AvantechAOs.m_tmpidx]
        public static TrackBar[] AOTrackBarVals = new TrackBar[8];// 8 AOs
        public static CheckBox[] AOChkRanges = new CheckBox[8];// 8 AO ranges
        #endregion



        public static bool LoginOK;
            public static int ApsRet;
            public static bool openAlphapad;
            public static bool Auto;
            public static bool ManualP;
            public static bool SetupPage;
            public static string DOStatus2;
            public static string RetInput;
        
       
            public static bool ManualCycleStarted;
            public static bool Graph;
            public static bool User;
            public static bool Program;
            public static string UManual;
            public static string USetupPage;
            public static string UUser;
           
            public static string[,] Pass = new string[100, 100];
            public static int PassIndex;
            public static string newpass;
           
            public static bool LeakOpen;
            public static Button btnManualStart, btnIOManual, btnManualMode, btnManualLeak,btnManualProg,btnManualSetup,btnMaualSMEMA,btnManualTuner,btnManualStop;
             
            
            public static GroupBox GBManualDoors;
           
            public static string[,] Setup = new string[100, 100];
            public static string[,] Users = new string[100, 100];
            public static string[,] Manual = new string[100, 100];
            public static string[,] Programs = new string[100, 100];
           
            public static string UProgram;
            public static double NumPadret;
            public static double NumPadsend;
            public static string AlphaPadret;
            public static string AlphaPadsend;
            public static  int Gasedit;
            public static string AllowedDenied;
            public static int UserIndex;
           
            public static string CurrentP;
           
            public static bool FVent;
            
            public static System.DateTime FileStartTime;
            public static string FPath;
            public static bool Saved;
           
            public static bool AutoCycle;
            public static bool AutoRFCycle;
            public static bool ManualCycle;
            public static int PumpTick;
            public static bool PumpOn;
            public static int MotorLapseTime;
            public static int MotorLapseTSetup;
            public static bool Intlk;
            public static int RunStep;
           
            public static double Inc;
            public static bool FatalError;
            public static bool ManualStop;
            
            public static string NewArray;
            public static bool BackDoorNotUp;
            public static bool FrontDoorNotUp;
            public static bool BothDoorsNotUp;
            public static bool ManualGas;
            public static bool StartCounting;
            public static bool Stopping;
            public static string DnStreamStatus;
            public static bool Venting;
            public static bool ACBool;
            public static bool ByPass;
            public static bool ByPassMode;

            #region Recipe Values

            public static string R_ID;
            public static double R_PT;
            public static double R_TTP;
            public static double R_G1;
            public static double R_G2;
            public static double R_G3;
            public static double R_PW;
            public static double R_RFT;
            public static double R_TP;
            public static double R_LP;
            

            public static string progIDDB;
            public static double PressTrigDB;
            public static double TTPDB;
            public static double Gas1DB;
            public static double Gas2DB;
            public static double Gas3DB;
            public static double RFPWRDB;
            public static double RFTimeDB;
            public static double TunePosDB;
            public static double LoadposDB;
            public static Boolean DidCloseDoors;
            public static Boolean DidOpenDoors;
            public static bool FrontDrUpOK;
            public static bool BackDrUpOK;
            public static bool closeDrOK;
            public static bool OpenDrOK;
            public static bool ScloseDrOK;
            public static bool SOpenDrOK;
            public static bool Stopped;
            #endregion 

            public static bool OpenfromManual;
            public static string DOReadVal;
            public static double SetupRFRange;
            public static Int16 Autostart;
           

           
            public static string[] AI = new string[17];
            public static Boolean connFlag;
            public static Boolean chkFlag;
            public static bool ProgFlag;
            public static int SelectNegVal;
            public static bool Autocycle;
            public static string DONewArray;
       
            public static string NewDoorArray; 
            public static bool DORunning;
            public static bool TLDid1;
            public static bool TLDid2;
            public static bool FirstCycle;
            public static bool Add200msec;
            public static Int16 ErrorCounter;
            public static string ManCycleTime;

            #region RealTime Variables

            public static bool CycleStart;
            public static Int32 ShiftCycles;
            public static double RealPressure = 0;
            public static double RealGas1 = 0;
            public static double RealGas2 = 0;
            public static double RealGas3 = 0;
            public static double RealRFPWR = 0;
            public static double RealRFREV = 0;
            public static double RealBias = 0;
            public static double RealTune = 0;
            public static double RealLoad = 0;
            public static double RealGAS1PS = 0;
            public static double RealGAS2PS = 0;
            public static double RealGAS3PS = 0;
            public static double RealPurge = 0;
            #endregion
           
                 
              
            #region AI
            public static double AI_PressureValue;
            public static double AI_GAS1Value;
            public static double AI_GAS2Value;
            public static double AI_GAS3Value;
            public static double AI_VentValue;
            public static double AI_ARFPowerValue;
            public static double AI_RFRefelctedValue;
            public static double AI_BiasValue;
            public static double AI_TuneValue;
            public static double AI_LoadValue;
            public static double AI_GAS1PSValue;
            public static double AI_GAS2PSValue;
            public static double AI_GAS3PSValue;
            public static double AI_PURGEPSValue;
            #endregion
            #region AO
            public static double AO_SetRfValue;
            public static double AO_SetGAS1Value;
            public static double AO_SetGAS2Value;
            public static double AO_SetGAS3Value;
            public static double AO_SetTuneValue;
            public static double AO_SetLoadValue;
            #endregion

            //End RealTime Variables


            #region Process variables
            public static bool CycleStarted;
            public static double CycleTime;
            public static bool DiDPumpDown;
            public static bool DiDTTP;
            public static bool DiDPlasma;
            public static bool TriggerError;
            public static bool HoldBuzzer;
            public static Int32 ClockTick;
            public static Int32 RFTick;
            public static Int32 MRFTick;
            public static Int32 MGasTick;
            public static Int32 PDTick;
            public static Int32 TTPTick;
            public static Int32 DelayTick;
            public static Int32 PlasmaTick;
            public static Int32 VentTick;
            public static bool StopClick;
            public static bool ThreadSleeping;
            public static string LeaktestResult;
            public static bool DoingPumpOn;
            public static bool DoingWaitForPressvalve;
            public static bool DoingWaitForPT;
            public static bool DoingTTP;
            public static bool DoneOnce;
            public static bool DoingPlasma;

            #endregion

            #region Setup Globals
            //public static double SetupRFRange;
            public static bool Gas1;
            public static bool Gas2;
            public static bool Gas3;
            public static string Gas1T;
            public static string Gas2T;
            public static string Gas3T;

            public static string GasCT;
            public static double Gas1R;
            public static double Gas2R;
            public static double Gas3R;
            public static int Venttime;
            public static int PunpDwnTime;
            public static bool H2Gen;

            public static bool TurboP;
            public static string TheUser;

            public static string Password;
            #endregion


            #region Gas Correction Factors
            public static double GCF1;
            public static double GCF2;
            public static double GCF3;

            public static double GCFC;

            #endregion



            #region Program Parameters
            public static string PID;
            public static float PPressTrig;
            public static int PTTP;
            public static float PGas1;
            public static float PGas2;
            public static float PGas3;
            public static float PRFPWR;
            public static int PRFTime;
            public static float PTunePos;

            public static float PLoadPos;
            #endregion


            #region Alarm Golbals
            public static bool AlarmActive;
            public static bool AlarmPause;
            public static int SendAlarm;
            public static string AlarmMsg;

            #endregion


            //PCI 7856 Globals
            //Card number for setting.
            public static int CardID = 0;
            //Bus number for setting,  Motion Net BusNo is 0.
            public static int BusNo = 0;
            public static int DICard;
            public static int DOCard;
            public static int AI_AOCard;

          
            public static bool Connected;

            public static int AutoC;
            public static Int32 ErrorCode;
            public static Int32 InBits;

            public static Int32 DI_Value;
            public static string DIArray;
            public static string DOArray;
         
            
            public static double PciPressure;
            
            public static int moduleDI;
            public static int moduleDO;
            public static int moduleAO;
            public static int moduleAI;

            public static string DOStatus;
            public static bool DO_StatusMainpurge;
            public static bool DoingManualCycle;
            public static bool CMP;
            public static Int32[,] DiType;
            public static Int32[] ditypeDB = new Int32[32];

            #region Setup Variables
            public static double RFrangSetup;
            public static string Gastype1Setup;
            public static double Gasrange1Setup;
            public static string Gastype2Setup;
            public static double Gasrange2Setup;
            public static string Gastype3Setup;
            public static double Gasrange3Setup;
            public static int VentingTimeSetup;
            public static int PumpdownAlarmSetup;

            public static string ProgramName;
            public static bool programIDFlag;
            public static string PreconnectionString;
            #endregion

            #region DI Global Variables

        

            public static int DI_Air;
            public static int DI_EStop;
            public static int DI_PumpOK;
            public static int DI_ChamberUP;
            public static int DI_ChamberDn;
            public static int DI_ShutterDrUP;
            public static int DI_ShutterDrDn;
            public static int DI_TrolClampRightLock;
            public static int DI_TrolClampLeftLock;
            public static int DI_ConveyorUP;
            public static int DI_ConveyorDn;
            public static int DI_TravellerHome;
            public static int DI_TravellerCenter;
            public static int DI_TravellerRemote;
            public static int DI_TravellerEndSnr;
            public static int DI_TravellerClearSnr;
            public static int DI_TrayPosSnrRight;
            public static int DI_TrayPosSnrLeft;
            public static int DI_TrolleySnrRight;
            public static int DI_TrolleySnrLeft;
            public static int DI_ByPassKeySwitch;
            public static int DI_DoorCoverSwitch;
            public static int DI_ChillerAlarm1;
            public static int DI_ChillerAlarm2;
            public static int DI_ChillerAlarm3;
            public static int DI_Lamp26;
            public static int DI_Lamp27;
            public static int DI_Lamp28;
            public static int DI_Lamp29;
            public static int DI_Lamp30;
            public static int DI_Lamp31;
            public static int DI_Lamp32;
            public static int DI_Lamp33;
            public static int DI_Lamp34;
            public static int DI_Lamp35;

        //End New DIs-----------------------------------

            public static int BO;

            public static string GBin;
            public static int Milliseconds;
            public static int[] DI = new int[32];

            #endregion


            public static void GetDI(bool[] arr)
            {
                DI_Air = Convert.ToInt16(arr[0]);
                DI_EStop = Convert.ToInt16(arr[1]);
                DI_PumpOK = Convert.ToInt16(arr[2]);
                DI_ChamberUP = Convert.ToInt16(arr[3]);
                DI_ChamberDn = Convert.ToInt16(arr[4]);
                DI_ShutterDrUP = Convert.ToInt16(arr[5]);
                DI_ShutterDrDn = Convert.ToInt16(arr[6]);
                DI_TrolClampRightLock = Convert.ToInt16(arr[7]);
                DI_TrolClampLeftLock = Convert.ToInt16(arr[8]);
                DI_ConveyorUP = Convert.ToInt16(arr[9]);
                DI_ConveyorDn = Convert.ToInt16(arr[10]);
                DI_TravellerHome = Convert.ToInt16(arr[11]);
                DI_TravellerCenter = Convert.ToInt16(arr[12]);
                DI_TravellerRemote = Convert.ToInt16(arr[13]);
                DI_TravellerEndSnr = Convert.ToInt16(arr[14]);
                DI_TravellerClearSnr = Convert.ToInt16(arr[15]);
                DI_TrayPosSnrRight = Convert.ToInt16(arr[16]);
                DI_TrayPosSnrLeft = Convert.ToInt16(arr[17]);
                DI_TrolleySnrRight = Convert.ToInt16(arr[18]);
                DI_TrolleySnrLeft = Convert.ToInt16(arr[19]);
                DI_ByPassKeySwitch = Convert.ToInt16(arr[20]);
                DI_DoorCoverSwitch = Convert.ToInt16(arr[21]);
                DI_ChillerAlarm1 = Convert.ToInt16(arr[22]);
                DI_ChillerAlarm2 = Convert.ToInt16(arr[23]);
                DI_ChillerAlarm3 = Convert.ToInt16(arr[24]);

                //DI_Lamp24 = arr[23];
                //DI_Lamp25 = arr[24];
                //DI_Lamp26 = arr[25];
                //DI_Lamp27 = arr[26];
                //DI_Lamp28 = arr[27];
                //DI_Lamp29 = arr[28];
                //DI_Lamp30 = arr[29];
                //DI_Lamp31 = arr[30];
                //DI_Lamp32 = arr[31];
            }

            #region DO Global Variables

            public static bool DO_RFON;
            public static bool DO_PumpON;
            public static bool DO_ManualTuner;
            public static bool DO_PresetAutoTuner;
            public static bool DO_TravelMotorFW;
            public static bool DO_TravelMotorBW;
            public static bool DO_TravelMotorBrake1;
            public static bool DO_ConvMotor2_3FW;
            public static bool DO_ConvMotor2_3BW;
            public static bool DO_ConvMotorBr2_3;
            public static bool DO_StandBy_10;
            public static bool DO_StandBy_11;
            public static bool DO_ChamberUP;
            public static bool DO_ChamberDown;
            public static bool DO_ShutterDoorUP;
            public static bool DO_ShutterDoorDown;
            public static bool DO_TrolleyClampRightLock;
            public static bool DO_TrolleyClampLeftLock;
            public static bool DO_ConveyorUP;
            public static bool DO_ConveyorDown;
            public static bool DO_PressureON;
            public static bool DO_VentON;
            public static bool DO_VacuumON;
            public static bool DO_StandBy_23;
            public static bool DO_Gas1ON;
            public static bool DO_Gas2ON;
            public static bool DO_GreenLight;
            public static bool DO_YellowLight;
            public static bool DO_RedLight;
            public static bool DO_BuzzerON;
            public static bool DO_SafetySwitch;
            public static bool DO_StandBy_31;
            public static bool DO_SPARE_32;
            public static bool DO_SPARE_33;
            public static bool DO_SPARE_34;
            public static bool DO_SPARE_35;
           
            #endregion


            public static void GetDO(bool[] arr)
            {

                DO_RFON = arr[0]; 
                DO_PumpON = arr[1]; 
                DO_ManualTuner = arr[2]; 
                DO_PresetAutoTuner = arr[3]; 
                DO_TravelMotorFW = arr[4]; 
                DO_TravelMotorBW = arr[5];
                DO_TravelMotorBrake1 = arr[6]; 
                DO_ConvMotor2_3FW = arr[7]; 
                DO_ConvMotor2_3BW = arr[8]; 
                DO_ConvMotorBr2_3 = arr[9];
                DO_StandBy_10 = arr[10]; 
                DO_StandBy_11 = arr[11];
                DO_ChamberUP = arr[12];
                DO_ChamberDown = arr[13];
                DO_ShutterDoorUP = arr[14];
                DO_ShutterDoorDown = arr[15];
                DO_TrolleyClampRightLock = arr[16];
                DO_TrolleyClampLeftLock = arr[17];
                DO_ConveyorUP = arr[18];
                DO_ConveyorDown = arr[19];
                DO_PressureON = arr[20];
                DO_VentON = arr[21];
                DO_VacuumON = arr[22];
                DO_StandBy_23 = arr[23];
                DO_Gas1ON = arr[24];
                DO_Gas2ON = arr[25];
                DO_GreenLight = arr[26];
                DO_YellowLight = arr[27];
                DO_RedLight = arr[28];
                DO_BuzzerON = arr[29];
                DO_SafetySwitch = arr[30];
                DO_StandBy_31 = arr[31];
                DO_SPARE_32 = arr[32];
                DO_SPARE_33 = arr[33];
                DO_SPARE_34 = arr[34];
                DO_SPARE_35 = arr[35];
            
              
            }

          
        
        }

      
}
