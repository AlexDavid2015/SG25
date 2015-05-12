using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using APS_Define_W32;
using APS168_W32;
using System.Threading;


namespace SG25
{
    public partial class TunerPage : Form
    {
        public TunerPage()
        {
            InitializeComponent();
        }
        
        int CardID = 0;     //PCI-7856 Card ID
        int BusNo = 0;     //HSL Bus is 0
 
        Label[] AI = new Label[17];
        Button[] DI = new Button[17];
        public string DOStatus;
       // int moduleDIDO = 3;
        internal System.Windows.Forms.Button Button5;
        internal System.Windows.Forms.Button Button9;
        internal System.Windows.Forms.Button Button11;
        internal System.Windows.Forms.Button Button12;
        internal System.Windows.Forms.Button Button13;

        string DOArray;
        //double RealTune;
       
        internal System.Windows.Forms.Label Label12;
        //double RealLoad;//this is local
        bool SelectTune;
        bool SelectLoad;
        internal System.ComponentModel.BackgroundWorker BackgroundWorker1;
        internal System.Windows.Forms.Button BBuzzer;

        double RealPressure = 0;
        double RealGas1 = 0;
        double RealGas2 = 0;
        double RealGas3 = 0;
        double RealRFPWR = 0;
        double RealRFREV = 0;
        double RealBias = 0;

        Thread TunerPgTh;
        

        private void TunerPage_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
       
        {if(TunerPgTh!=null)
        { TunerPgTh.Abort();}
          }




        private void TunerPage_Load(System.Object sender, System.EventArgs e)
        {
           
            TunerPgTh = new Thread(new System.Threading.ThreadStart(TunerTimerTh));
            TunerPgTh.Start();
            G1T.Text =Class1.Gas1T;
            G2T.Text =Class1.Gas2T;
            G3T.Text =Class1.Gas3T;
            KnobControl1.Value =(int)Class1.R_TP * 10;
            KnobControl2.Value =(int)Class1.R_LP * 10;
            Class1.Inc = 1;
            SelectTune = true;
            KnobControl1.BackColor = Color.DarkOliveGreen;

        }



        private void KnobControl1_ValueChanged(System.Object ValueChangedEventHandler)
        {
            double knobVal1=KnobControl1.Value;
            Label1.Text = String.Format("{0:0.0}", knobVal1 / 10)+"%";
            if (KnobControl1.Value < 0)
                Label1.Text = (0.0).ToString();
            if (KnobControl1.Value > 1000)
                Label1.Text = (100.0).ToString(); ;

            if (KnobControl1.Value < 100)
            {
                Label1.ForeColor = Color.Red;
            }
            else if (KnobControl1.Value > 900)
            {
                Label1.ForeColor = Color.Red;
            }
            else
            {
                Label1.ForeColor = Color.White;
            }

            Label1.Refresh();

            try
            {
                //Analog Output
                Int32 ret = default(Int32);
                double val1 = 0;
                Double knobValue1 = Convert.ToDouble(KnobControl1.Value);

                val1 = knobValue1 / 100;
               // ret = APS168.APS_set_field_bus_a_output(CardID, BusNo,Class1.moduleAI, 0, val1);
                Class2.setAOValues(Convert.ToString(val1), 1);
                
              }
            catch (Exception ex)
            {
            }
        }

        private void KnobControl2_ValueChanged(System.Object ValueChangedEventHandler)
        {
            double knobVal2 = KnobControl2.Value;
            Label2.Text = String.Format("{0:0.0}", knobVal2 / 10) + "%";

            if (KnobControl2.Value < 0)
                Label2.Text = (0.0).ToString();
            if (KnobControl2.Value > 1000)
                Label2.Text = (100.0).ToString();

            if (KnobControl2.Value < 100)
            {
                Label2.ForeColor = Color.Red;
            }
            else if (KnobControl2.Value > 900)
            {
                Label2.ForeColor = Color.Red;
            }
            else
            {
                Label2.ForeColor = Color.White;
            }

            Label2.Refresh();


            try
            {
                //Analog Output
                Int32 ret = default(Int32);
                double val1 = 0;
                double knobValue2 = Convert.ToDouble(KnobControl2.Value);

                val1 = knobValue2 / 100;
                //ret =APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAI, 1, val1);
                Class2.setAOValues(Convert.ToString(val1), 2);
                

            }
            catch (Exception ex)
            {
            }
        }

        private void Button3_Click(System.Object sender, System.EventArgs e)
        {
            Class1.TunePageConnected = false;
            Class1.btnAutoStart.Enabled = true;
            if (TunerPgTh != null)
            { TunerPgTh.Abort(); }
            Class1.ProgFlag =true;

            if (Class1.DO_ManualTuner == false & Class1.DO_RFON == false)
            {
               // Class2.Create10DOArray(18, 1, 19,0);

                
                int[] DOChannelArr = { 2,3 }; //manual tuner,auto tuner
                bool[] DOStateArr = { true, false };
                Class2.SetMultiDO(DOChannelArr, DOStateArr);

                //Class2.SetDO(Class1.DOSlotNum, 2, true);
                //Class2.SetDO(Class1.DOSlotNum, 3, false);
                //Thread.Sleep(200);
                //Class2.Create10DOArray(19, 0);

                Double val5, val6;
                Int32 ret5, ret6;
                Class2.RecipeUpload();
                val5 = Convert.ToDouble(Class1.R_TP / 10);
                //ret5 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAI, 0, val5);
                Class2.setAOValues(Convert.ToString(val5), 1);

                Class2.RecipeUpload();
                val6 = Convert.ToDouble(Class1.R_LP / 10);
                //ret6 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAI, 1, val6);
                Class2.setAOValues(Convert.ToString(val6), 2);
               
                if(Class1.IOShowInfoForm!=null)
                {  Class1.IOShowInfoForm.SendToBack();}
               
                MessageBox.Show("Tune and Load set to Recipe values");
                
            }

            this.Close();
            this.Dispose();

        }


        private void Button8_Click(System.Object sender, System.EventArgs e)
        {
            Programs objProg = new Programs();
            objProg.ShowDialog();
            
        }
        private void TunerTimerTh()
        {
            do
            {
                Thread.Sleep(200);
                Label13.Invoke((MethodInvoker)delegate { Label13.Text = (Class1.Inc / 10).ToString(); });
                //Read DO

                if (Class1.DO_RFON == false)
                {
                    Button6.BackColor = Color.Red;
                }
                else
                {
                    Button6.BackColor = Color.Green;
                }
                if (Class1.DO_ManualTuner == false)
                {
                    Button7.Invoke((MethodInvoker)delegate { Button7.BackColor = Color.LightGreen; Button7.Text = "Auto Tuning"; });
             
                }
                else
                {
                    Button7.Invoke((MethodInvoker)delegate { Button7.BackColor = Color.SteelBlue; Button7.Text = "Preset Tuning"; });
                }

                if (Class1.Gas3 == false)
                {
                    G3T.Invoke((MethodInvoker)delegate { G3T.Visible = false; Label15.Visible = false; });
                 }
                else
                {
                    G3T.Invoke((MethodInvoker)delegate { G3T.Visible = true; Label15.Visible = true; });
               
                }

                //End Read DO

                //Read AI

                //Set Interlock threshold
                if (Class1.Intlk == false)
                {
                    Button4.Invoke((MethodInvoker)delegate { Button4.BackColor = Color.Red; });
                    
                }
                else
                {
                    Button4.Invoke((MethodInvoker)delegate { Button4.BackColor = Color.Green; });
                    
                }


                //Set Interlock threshold



                //Calculate the Pressure in mbar
                //RealPressure = Math.Pow(10, (Class1.AI_PressureValue - 6));
                //RealPressure = RealPressure + 0.02;
                RealPressure = Convert.ToDouble(Class1.AI_PressureValue / 10);

                if (RealPressure < 100)
                {
                    PressureLbl.Invoke((MethodInvoker)delegate { PressureLbl.Text = (Math.Round(RealPressure, 2)).ToString(); });
                }
                else
                {
                    PressureLbl.Invoke((MethodInvoker)delegate { PressureLbl.Text = "Over Range"; });
                    
                }
                //End Calculate the Pressure in mbar

                //Calculate Gas1
                RealGas1 = ((Class1.Gas1R / 10) * Class1.AI_GAS1Value);
                //* GCF1
                Label11.Invoke((MethodInvoker)delegate { Label11.Text = (Math.Round(RealGas1, 2)).ToString(); });

                if (Class1.DO_Gas1ON == false)
                    Label11.Invoke((MethodInvoker)delegate { Label11.Text = "0"; });
                   
                //End Calculate Gas1

                //Calculate Gas2
                RealGas2 = Class1.Gas2R / 10 * Class1.AI_GAS2Value;
                //* GCF2
                Label9.Invoke((MethodInvoker)delegate { Label9.Text = (Math.Round(RealGas2, 2)).ToString(); });

                if (Class1.DO_Gas2ON == false)
                    Label9.Invoke((MethodInvoker)delegate { Label9.Text = "0"; });
                   
                //End Calculate Gas2

                //Calculate Gas3
                RealGas3 = Class1.Gas3R / 10 * Class1.AI_GAS3Value;
                //* GCF3
                Label15.Invoke((MethodInvoker)delegate { Label15.Text = (Math.Round(RealGas3, 2)).ToString(); });
                
                //if (Class1.DO_StatusGAS3 == false)
                //    Label15.Invoke((MethodInvoker)delegate { Label15.Text = "0"; });
                   
                ////End Calculate Gas3

                ////Calculate AI_ARFPowerValue
                //RealRFPWR = Class1.SetupRFRange / 10 * Class1.AI_ARFPowerValue;
                //Label16.Invoke((MethodInvoker)delegate {  Label16.Text = (Math.Round(RealRFPWR, 1)).ToString();});

                if (Class1.DO_RFON == false)
                    Label16.Invoke((MethodInvoker)delegate { Label16.Text = "0"; });
                    
                //End Calculate AI_ARFPowerValue

                //Calculate AI_RFRefelctedValue
                RealRFREV = Class1.SetupRFRange / 10 / 3 * Class1.AI_RFRefelctedValue;
                Label10.Invoke((MethodInvoker)delegate { Label10.Text = (Math.Round(RealRFREV, 1)).ToString(); });

                if (Class1.DO_RFON == false)
                    Label10.Invoke((MethodInvoker)delegate { Label10.Text = "0"; });
                    
                //End Calculate AI_RFRefelctedValue

                //Calculate AI_BiasValue
                RealBias = 100 * Class1.AI_BiasValue;
                Label27.Invoke((MethodInvoker)delegate { Label27.Text = (Math.Round(RealBias, 2)).ToString(); });

                if (Class1.DO_RFON == false)
                    Label27.Invoke((MethodInvoker)delegate { Label27.Text = "0"; });
                    
                //End Calculate AI_BiasValue

                //Calculate AI_TuneValue
                Class1.RealTune = 10 * Class1.AI_TuneValue;
                KnobControl3.Invoke((MethodInvoker)delegate { KnobControl3.Value = (int)Class1.RealTune; });
                Label8.Invoke((MethodInvoker)delegate { Label8.Text = (Math.Round(Class1.RealTune, 2)).ToString() + "%"; });
               
                if (KnobControl3.Value < 10)
                {
                    Label8.ForeColor = Color.Red;
                }
                else if (KnobControl3.Value > 90)
                {
                    Label8.ForeColor = Color.Red;
                }
                else
                {
                    Label8.ForeColor = Color.White;
                }
                //End Calculate AI_TuneValue

                //Calculate AI_LoadValue
                Class1.RealLoad = 10 * Class1.AI_LoadValue;
                KnobControl4.Invoke((MethodInvoker)delegate { KnobControl4.Value = (int)Class1.RealLoad; });
                Label7.Invoke((MethodInvoker)delegate { Label7.Text = (Math.Round(Class1.RealLoad, 2)).ToString() + "%"; });
                
                if (KnobControl4.Value < 10)
                {
                    Label7.ForeColor = Color.Red;
                }
                else if (KnobControl4.Value > 90)
                {
                    Label7.ForeColor = Color.Red;
                }
                else
                {
                    Label7.ForeColor = Color.White;
                }
            } while (true);
        }

      private void RFON_Click(System.Object sender, System.EventArgs e)
        {
            if (Class1.Intlk == false)
            {
                MessageBox.Show("Sorry, RF Interlock not satisfied!");
                return;
            }

            if (Class1.AutoRFCycle == true)
            {
                MessageBox.Show("You cannot use this button when RF Auto Cycle is On");
                return;
            }
            if (Class1.Intlk == true & Class1.DoingManualCycle == true)
            {
                if (Class1.DO_RFON == false)
                {
                   // Class2.Create10DOArray(17, 1);
                    Class2.SetDO(Class1.DOSlotNum, 0, true);//RFON  
                           
                }
                else
                {
                    //Class2.Create10DOArray(17, 0);
                    Class2.SetDO(Class1.DOSlotNum, 0, false);//RF OFF
                    
                }
            }
        }


        private void Button1_Click(System.Object sender, System.EventArgs e)
        {
          //  Class2.Create10DOArray(18, 1);
            Class2.SetDO(Class1.DOSlotNum, 2, true);//Manual tuner on
            GroupBox1.Enabled = true;
            Thread.Sleep(200);
           // Class2.Create10DOArray(19, 0);
            Class2.SetDO(Class1.DOSlotNum, 3, false);//auto tuner off

            try
            {
                //Analog Output
                Int32 ret = default(Int32);
                double val1 = 0;

                val1 = Convert.ToDouble(KnobControl1.Value / 100);
               // ret = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAI, 0, val1);
                Class2.setAOValues(Convert.ToString(val1), 1);
               


            }
            catch (Exception ex)
            {
            }

            try
            {
                //Analog Output
                Int32 ret = default(Int32);
                double val2 = 0;

                val2 = Convert.ToDouble(KnobControl2.Value / 100);
                //ret = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAI, 1, val2);
                Class2.setAOValues(Convert.ToString(val2), 2);

            }
            catch (Exception ex)
            {
            }
        }


        private void Button2_Click(System.Object sender, System.EventArgs e)
        {
           // Class2.Create10DOArray(18, 0);
            Class2.SetDO(Class1.DOSlotNum, 2, true);//Manual tuner on
            GroupBox1.Enabled = true;
            Thread.Sleep(200);
          //  Class2.Create10DOArray(19, 1);
            Class2.SetDO(Class1.DOSlotNum, 3, false);//auto tuner off
        }




      


        private void Button5_Click(System.Object sender, System.EventArgs e)
        {
            KnobControl1.Value =(int)Class1.R_TP * 10;
            KnobControl2.Value =(int)Class1.R_LP * 10;
        }

        private void Button9_Click(System.Object sender, System.EventArgs e)
        {

            KnobControl1.Value = Convert.ToInt32(Class1.RealTune * 10);
            //KnobControl1.Value = Convert.ToInt32(RealTune);
            Label1.Text = Class1.RealTune.ToString();
           // Label1.Text = KnobControl1.Value.ToString()+".0%";
            KnobControl2.Value = Convert.ToInt32(Class1.RealLoad * 10);
            // KnobControl2.Value = Convert.ToInt32(RealLoad);
            Label2.Text = Class1.RealLoad.ToString();
           // Label2.Text = KnobControl2.Value.ToString() + ".0%";
        }

        private void Button11_Click(System.Object sender, System.EventArgs e)
        {
            if (Class1.Inc <= 0)
            { Class1.Inc = 1; }

            if (Class1.Inc == 1)
            {
                Class1.Inc = 10;
                return;
            }
            else if (Class1.Inc == 10)
            {
                Class1.Inc = 50;
            }
            else if (Class1.Inc == 50)
            {
                Class1.Inc = 1;
                return;
            }
        }

        private void Button10_Click(System.Object sender, System.EventArgs e)
        {
            if (SelectTune == false)
            {
                SelectTune = true;
                SelectLoad = false;
                Button10.Text = "Press to Select Load";
                KnobControl1.BackColor = Color.DarkOliveGreen;
                KnobControl2.BackColor = Color.FromArgb(64, 64, 64);
            }
            else
            {
                SelectTune = false;
                SelectLoad = true;
                Button10.Text = "Press to Select Tune";
                KnobControl1.BackColor = Color.FromArgb(64, 64, 64);
                KnobControl2.BackColor = Color.DarkOliveGreen;
            }


        }

        private void Button12_Click(System.Object sender, System.EventArgs e)
        {
            //double PresetTune;
            if (SelectTune == true)
            {
                Class1.RealTune = KnobControl1.Value - Class1.Inc;
                if (Class1.RealTune <= 0.0)
                    Class1.RealTune = 0.0;
                if (Class1.RealTune >= 1000.0)
                    Class1.RealTune = 1000.0;

                try
                {
                  //Analog Output
                    Int32 ret = default(Int32);
                   double Tval1 = 0;

                   Tval1 = Convert.ToDouble(Class1.RealTune / 100);
                   // ret =APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAI, 0, Tval1);
                   Class2.setAOValues(Convert.ToString(Tval1), 1);
                    
                    KnobControl1.Value = (int)Class1.RealTune;

                }
                catch (Exception ex)
                {
                }

            }
            else if (SelectLoad == true)
            {
                Class1.RealLoad = KnobControl2.Value - Class1.Inc;
                if (Class1.RealLoad <= 0.0)
                    Class1.RealLoad = 0.0;
                if (Class1.RealLoad >= 1000.0)
                    Class1.RealLoad = 1000.0;


                try
                {
                    //Analog Output
                    Int32 ret = default(Int32);
                    double Lval1 = 0;

                    Lval1 = Convert.ToDouble(Class1.RealLoad / 100);
                    //ret =APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAI, 1, Lval1);
                    Class2.setAOValues(Convert.ToString(Lval1), 2);

                    KnobControl2.Value = (int)Class1.RealLoad;

                }
                catch (Exception ex)
                {
                }
            }


        }

        private void Button13_Click(System.Object sender, System.EventArgs e)
        {
            if (SelectTune == true)
            {
                Class1.RealTune = KnobControl1.Value + Class1.Inc;
                if (Class1.RealTune <= 0.0)
                    Class1.RealTune = 0.0;
                if (Class1.RealTune >= 1000.0)
                    Class1.RealTune = 1000.0;

                try
                {
                    //Analog Output
                    Int32 ret = default(Int32);
                    double val1 = 0;

                    val1 = Convert.ToDouble(Class1.RealTune / 100);
                    //ret =APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAI, 0, val1);
                    //ret = ret;
                    Class2.setAOValues(Convert.ToString(val1), 1);
                    KnobControl1.Value = (int)Class1.RealTune;

                }
                catch (Exception ex)
                {
                }

            }
            else if (SelectLoad == true)
            {
                Class1.RealLoad = KnobControl2.Value + Class1.Inc;
                if (Class1.RealLoad <= 0.0)
                    Class1.RealLoad = 0.0;
                if (Class1.RealLoad >= 1000.0)
                    Class1.RealLoad = 1000.0;


                try
                {
                    //Analog Output
                    Int32 ret = default(Int32);
                    double val1 = 0;

                    val1 = Convert.ToDouble(Class1.RealLoad / 100);
                    //ret =APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAI, 1, val1);
                    //ret = ret;
                    Class2.setAOValues(Convert.ToString(val1), 2);
                    KnobControl2.Value = (int)Class1.RealLoad;

                }
                catch (Exception ex)
                {
                }
            }


        }

    

        private void BBuzzer_Click(System.Object sender, System.EventArgs e)
        {
            // Reset Buzzer
            Class1.HoldBuzzer = true;
            Class1.FatalError = false;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Class1.TunePageConnected = true;
            Class1.btnAutoStart.Enabled = false;
            Class1.RetForm.Show();
        }
     

    }
}
