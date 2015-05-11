using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SG25.SetupDataSetTableAdapters;
using APS_Define_W32;
using APS168_W32;



namespace SG25
{
    public partial class Setup1 : Form
    {
        public Setup1()
        {
            InitializeComponent();
        }

        SetupDataSet SUsetupDatasetObj = new SetupDataSet();
        SetupDefaultTableAdapter SetupDefaultDA = new SetupDefaultTableAdapter();
        StartupTableAdapter SUstartupTAobj = new StartupTableAdapter();
        SetupTableAdapter SUsetupTAobj = new SetupTableAdapter();
        ProgramsTableAdapter SUprogramsTAobj = new ProgramsTableAdapter();
        
        

       
      

     
        private void G1_Click(System.Object sender, System.EventArgs e)
        {
            if (Class1.Gas1 == false)
            {
                Class1.Gas1 = true;
                this.G1.BackColor = Color.PaleGreen;
                G1T.Visible = true;
                G1R.Visible = true;
                G1E.Visible = true;
            }
            else
            {
                Class1.Gas1 = false;
                this.G1.BackColor = Color.Gainsboro;
                G1T.Visible = false;
                G1R.Visible = false;
                G1E.Visible = false;


            }
        }


        private void G2_Click(System.Object sender, System.EventArgs e)
        {
            if (Class1.Gas2 == false)
            {
                Class1.Gas2 = true;
                this.G2.BackColor = Color.PaleGreen;
                G2T.Visible = true;
                G2R.Visible = true;
                G2E.Visible = true;
            }
            else
            {
                Class1.Gas2 = false;
                this.G2.BackColor = Color.Gainsboro;
                G2T.Visible = false;
                G2R.Visible = false;
                G2E.Visible = false;

            }
        }


        private void G3_Click(System.Object sender, System.EventArgs e)
        {
            if (Class1.Gas3 == false)
            {
                Class1.Gas3 = true;
                this.G3.BackColor = Color.PaleGreen;
                G3T.Visible = true;
                G3R.Visible = true;
                G3E.Visible = true;
            }
            else
            {
                Class1.Gas3 = false;
                this.G3.BackColor = Color.Gainsboro;
                G3T.Visible = false;
                G3R.Visible = false;
                G3E.Visible = false;
            }
          this.Invalidate();
            
        }


        private void G1E_Click(System.Object sender, System.EventArgs e)
        {
            Class2.EditGases(1);
            G1T.Text =Class1.Gas1T;
            G1R.Text = (Class1.Gas1R).ToString();
            LGCF1.Text = (Class1.GCF1).ToString();

        }


        private void G2E_Click(System.Object sender, System.EventArgs e)
        {
            Class2.EditGases(2);
            G2T.Text =Class1.Gas2T;
            G2R.Text =Class1.Gas2R.ToString();
            LGCF2.Text =Class1.GCF2.ToString();
        }


        private void G3E_Click(System.Object sender, System.EventArgs e)
        {
            Class2.EditGases(3);
            G3T.Text =Class1.Gas3T;
            G3R.Text =Class1.Gas3R.ToString();
            LGCF3.Text =Class1.GCF3.ToString();
        }


        private void Button13_Click(System.Object sender, System.EventArgs e)
        {

           Class1.NumPadsend =Double.Parse(VTime.Text);
           Numeric_Pad objNpad = new Numeric_Pad();
           objNpad.ShowDialog();
           Class1.Venttime =(int)Class1.NumPadret;
            this.VTime.Text =Class1.NumPadret.ToString();
        }





       

       private void Button6_Click(System.Object sender, System.EventArgs e)
        {
            this.Close();
            this.Dispose();
           if(Class1.IOInfoSamePg==true & Class1.IOOpen == true)
           {               
                Class1.openIO.Show(); 
           }

            
        }


       private void Setup1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'setupDataSet1.SetupDefault' table. You can move, or remove it, as needed.
            this.setupDefaultTableAdapter.Fill(this.setupDataSet1.SetupDefault);
            // TODO: This line of code loads data into the 'setupDataSet1.SetupDefault' table. You can move, or remove it, as needed.
           
           

            SetupDataSet SUsetupDatasetObj = new SetupDataSet();
            SetupDefaultTableAdapter SetupDefaultDA = new SetupDefaultTableAdapter();
            StartupTableAdapter SUstartupTAobj = new StartupTableAdapter();
            SetupTableAdapter SUsetupTAobj = new SetupTableAdapter();
            ProgramsTableAdapter SUprogramsTAobj = new ProgramsTableAdapter();
          


            this.SetupDefaultDA.Fill(SUsetupDatasetObj.SetupDefault);
           
    
            // TODO: This line of code loads data into the 'setupDataSet.Setup' table. You can move, or remove it, as needed.
            this.setupTableAdapter.Fill(this.setupDataSet.Setup);
            SUsetupTAobj.Fill(SUsetupDatasetObj.Setup);
           //RFtb.Text = SUsetupDatasetObj.Tables[0].Rows[0]["RFrange"].ToString();
           Class1.SetupRFRange = Convert.ToDouble(RFtb.Text.ToString());

           Class1.Gas1 = Convert.ToBoolean(SUsetupTAobj.GetSetupGas1());
           if (Class1.Gas1 == true)
           {
               this.G1.BackColor = Color.PaleGreen;
               G1T.Visible = true;
               G1R.Visible = true;
               G1E.Visible = true;
           }
           else
           {
               this.G1.BackColor = Color.Gainsboro;
               G1T.Visible = false;
               G1R.Visible = false;
               G1E.Visible = false;
           }

           Class1.Gas2 = Convert.ToBoolean(SUsetupTAobj.GetSetupGas2());

           if (Class1.Gas2 == true)
           {
               this.G2.BackColor = Color.PaleGreen;
               G2T.Visible = true;
               G2R.Visible = true;
               G2E.Visible = true;
           }
           else
           {
               this.G2.BackColor = Color.Gainsboro;
               G2T.Visible = false;
               G2R.Visible = false;
               G2E.Visible = false;

           }


           Class1.Gas3 = Convert.ToBoolean(SUsetupTAobj.GetSetupGas3());

           if (Class1.Gas3 == true)
           {
               this.G3.BackColor = Color.PaleGreen;
               G3T.Visible = true;
               G3R.Visible = true;
               G3E.Visible = true;
           }
           else
           {
               this.G3.BackColor = Color.Gainsboro;
               G3T.Visible = false;
               G3R.Visible = false;
               G3E.Visible = false;
           }
           Class1.Gas1T = SUsetupTAobj.SelectGas1Type().ToString();
           Class1.Gas2T = SUsetupTAobj.SelectGas2Type().ToString();
           Class1.Gas3T = SUsetupTAobj.SelectGas3Type().ToString();
           Class1.Gas1R =Convert.ToDouble(SUsetupTAobj.SelectGas1Range().ToString());
           Class1.Gas2R = Convert.ToDouble(SUsetupTAobj.SelectGas2Range().ToString());
           Class1.Gas3R = Convert.ToDouble(SUsetupTAobj.SelectGas2Range().ToString());
           Class1.Venttime =Convert.ToInt32(SUsetupTAobj.SelectVentTime().ToString());
           Class1.PunpDwnTime = Convert.ToInt32(SUsetupTAobj.SelectPumpDwnTime().ToString());
           

           Class1.H2Gen = Convert.ToBoolean(SUsetupTAobj.GetSetupHasH2());

           if (Class1.H2Gen == true)
           {
               this.H2Genbtn.BackColor = Color.PaleGreen;
           }
           else
           {
               this.H2Genbtn.BackColor = Color.Gainsboro;
           }


           Class1.TurboP = Convert.ToBoolean(SUsetupTAobj.GetSetupHasTurbo());

           if (Class1.TurboP == true)
           {
               this.TPbtn.BackColor = Color.PaleGreen;
           }
           else
           {
               this.TPbtn.BackColor = Color.Gainsboro;
           }
           Class1.GCF1 =Math.Round((Convert.ToDouble(SUsetupTAobj.GetGCF1())),2);
           Class1.GCF2 = Math.Round(Convert.ToDouble(SUsetupTAobj.GetGCF2()),2);
           Class1.GCF3 =Math.Round( Convert.ToDouble(SUsetupTAobj.GetGCF3()),2);
           LGCF1.Text = Class1.GCF1.ToString();
           LGCF2.Text = Class1.GCF2.ToString();
           LGCF3.Text = Class1.GCF3.ToString();

          
           Class1.Saved = false;
        }

       private void button5_Click(object sender, EventArgs e)
       {
           Class1.Saved = true;
           RFSetup objRFSetup = new RFSetup();
           objRFSetup.ShowDialog();
           RFtb.Text = Class1.SetupRFRange.ToString();

       }

       private void Button1_Click(object sender, EventArgs e)
       {
           Class1.Saved = false;
           SUsetupTAobj.UpdateSetupTable(float.Parse (Class1.SetupRFRange.ToString()), Convert.ToBoolean(Class1.Gas1), Convert.ToBoolean(Class1.Gas2), Convert.ToBoolean(Class1.Gas3), Class1.Gas1T.ToString(), Class1.Gas2T.ToString(), Class1.Gas3T.ToString(), float.Parse(Class1.GCF1.ToString()), float.Parse(Class1.GCF2.ToString()), float.Parse(Class1.GCF3.ToString()), float.Parse(Class1.Gas1R.ToString()), float.Parse(Class1.Gas2R.ToString()), float.Parse(Class1.Gas3R.ToString()),(int)Class1.PunpDwnTime,(int)Class1.Venttime,Convert.ToBoolean(Class1.H2Gen),Convert.ToBoolean(Class1.TurboP));
           this.Close();
           this.Dispose();
       }

       private void Button4_Click(object sender, EventArgs e)
       {
           /*Class1.SetupRFRange = 600.0;
           this.RFtb.Text = Class1.SetupRFRange.ToString();*/

           Class2.DefaultSetup();
           RFtb.Text = Class1.RFrangSetup.ToString();
           Class1.SetupRFRange = Convert.ToDouble(RFtb.Text);

           G1T.Text = Class1.Gastype1Setup.ToString();
           Class1.Gas1T = G1T.Text;

           G1R.Text = Class1.Gasrange1Setup.ToString();
           Class1.Gas1R = Convert.ToDouble(G1R.Text);

           G2T.Text = Class1.Gastype2Setup.ToString();
           Class1.Gas2T = G2T.Text;

           G2R.Text = Class1.Gasrange2Setup.ToString();
           Class1.Gas2R = Convert.ToDouble(G2R.Text);

           G3T.Text = Class1.Gastype3Setup.ToString();
           Class1.Gas3T = G3T.Text;

           G3R.Text = Class1.Gasrange3Setup.ToString();
           Class1.Gas3R = Convert.ToDouble(G3R.Text);

           VTime.Text = Class1.VentingTimeSetup.ToString();
           Class1.Venttime = Convert.ToInt32(VTime.Text);

           PDTime.Text = Class1.PumpdownAlarmSetup.ToString();
           Class1.PunpDwnTime = Convert.ToInt32(PDTime.Text);
           
       }

       private void TPbtn_Click(object sender, EventArgs e)
       {
           if (Class1.TurboP == false)
           {
               Class1.TurboP = true;
               this.TPbtn.BackColor = Color.PaleGreen;
           }
           else
           {
               Class1.TurboP = false;
               this.TPbtn.BackColor = Color.Gainsboro;
           }
       }

       private void H2Genbtn_Click(object sender, EventArgs e)
       {
            if (Class1.H2Gen == false)
            {
               Class1.H2Gen = true;
                this.H2Genbtn.BackColor = Color.PaleGreen;
            }
            else
            {
                Class1.H2Gen = false;
                this.H2Genbtn.BackColor = Color.Gainsboro;
            }
        }
      

       private void Button14_Click(object sender, EventArgs e)
       {
           Class1.NumPadsend = Convert.ToDouble(PDTime.Text);
           Numeric_Pad objNpad = new Numeric_Pad();
           objNpad.ShowDialog();
           Class1.PunpDwnTime = (int)Class1.NumPadret;
           PDTime.Text = Class1.NumPadret.ToString();

       }

     
     
    }
}
