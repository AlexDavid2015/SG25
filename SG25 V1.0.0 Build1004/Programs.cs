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
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using SG25.SetupDataSetTableAdapters;
using System.Drawing.Imaging;
using System.Drawing.Design;



namespace SG25
{
    public partial class Programs : Form
    {
        public Programs()
        {
            InitializeComponent();
            Activated += Programs_Activated;
            Load += Programs_Load;
        }

        Alphapad objAlpha = new Alphapad();
        Numeric_Pad objNumAlpha = new Numeric_Pad();
        bool AddingNew;

        public delegate EventHandler Clik(System.Object sender, System.EventArgs e);

        //PCI-7856 Card ID
        int CardID = 0;
        //HSL Bus is 0
        int BusNo = 0;
        //HSL-DI32 Slave ID
        //int moduleDI = 1;
        //HSL-D032 Slave ID
        //int moduleDO = 3;
        //HSL-AO4 Slave ID
        //int moduleAO = 5;
        //HSL-AI16AO2 Slave ID
        //int moduleAI = 7;
        Label[] AI = new Label[17];
        Button[] DI = new Button[17];
        public string DOStatus;
        string DOArray;
        bool ModifingCurrentP;



        SetupDataSet setupDatasetObj = new SetupDataSet();
        StartupTableAdapter startupTAobj = new StartupTableAdapter();
        ProgramsTableAdapter programsTAobj = new ProgramsTableAdapter();


        int FirstScan = 0;
        private void Programs_Load(System.Object sender, System.EventArgs e)
        {

            this.startupTableAdapter.Fill(this.setupDataSet.Startup);
            this.programsTableAdapter.Fill(this.setupDataSet.Programs);
            // startupTAobj.Fill(setupDatasetObj.Startup);
            //programsTAobj.Fill(setupDatasetObj.Programs); 


            //this.ProgramsTableAdapter.Fill(SetupDataSet,"Programs");
            Gas1Lbl.Text = Class1.Gas1T + "-----------------------------";
            Gas2Lbl.Text = Class1.Gas2T + "-----------------------------";
            Gas3Lbl.Text = Class1.Gas3T + "-----------------------------";

            try
            {

                Class1.CurrentP = startupTableAdapter.SelectStartupProgram();
                this.programsTableAdapter.FillBy(this.setupDataSet.Programs, Class1.CurrentP);

            }
            catch
            {
            }

            Label5.Text = " This record is currently loaded in the System.";
            Label5.ForeColor = Color.White;
            //Else
            //Label5.ForeColor = Color.Red
            //Label5.Text = " Attention, the record displayed is not loaded in the System. !!"
            //End If

            if (Class1.Gas1 == false)
            {
                Gas1TB.Enabled = false;
                Gas1TB.Text = "N/A";
            }
            else
            {
                Gas1TB.Enabled = true;
            }

            if (Class1.Gas2 == false)
            {
                Gas2TB.Enabled = false;
                Gas2TB.Text = "N/A";
            }
            else
            {
                Gas2TB.Enabled = true;
            }

            if (Class1.Gas3 == false)
            {
                Gas3TB.Enabled = false;
                Gas3TB.Visible = false;
                Gas3Lbl.Visible = false;
                Gas3TB.Text = "N/A";
                Button1.Visible = true;
            }
            else
            {
                Gas3TB.Enabled = true;
                Button1.Visible = false;
            }
            ToolStripModify.Enabled = false;

            StartModifyButtons();

            if (Class1.UManual == "Denied")
            {
                ToolStripAddNew.Enabled = false;
                ToolStripDelete.Enabled = false;
            }
            ModifingCurrentP = false;
        }

        private void StartModifyButtons()
        {
            ToolStripModify.Enabled = false;
            ToolStripAddNew.Enabled = true;
            ToolStripDelete.Enabled = true;
            ToolStripSave.Enabled = false;
            ToolStripButton1.Enabled = true;
            ToolStripButton3.Enabled = true;
        }
        private void ONModifyChangeButtons()
        {
            ToolStripModify.Enabled = true;
            ToolStripAddNew.Enabled = false;
            ToolStripDelete.Enabled = false;
            ToolStripSave.Enabled = false;
            ToolStripButton1.Enabled = false;
            ToolStripButton3.Enabled = false;
        }
        private void OnAddNewChangeButtons()
        {
            ToolStripModify.Enabled = false;
            ToolStripAddNew.Enabled = false;
            ToolStripDelete.Enabled = false;
            ToolStripSave.Enabled = true;
            ToolStripButton1.Enabled = false;
            ToolStripButton3.Enabled = false;
        }


        private void TextBox1_Click(System.Object sender, System.EventArgs e)
        {
            if (TextBox1.Text != "")
                Class1.NumPadsend = Convert.ToDouble(TextBox1.Text);
            Numeric_Pad objNumAlpha1 = new Numeric_Pad();
            objNumAlpha1.ShowDialog();
            ModifingCurrentP = true;
            TextBox1.Text = Class1.NumPadret.ToString();
            if (AddingNew == false)
                ONModifyChangeButtons();

        }

        private void TextBox2_Click(System.Object sender, System.EventArgs e)
        {
            if (TextBox2.Text != "")
                Class1.NumPadsend = Convert.ToDouble(TextBox2.Text);
            Numeric_Pad objNumAlpha1 = new Numeric_Pad();
            objNumAlpha1.ShowDialog();
            ModifingCurrentP = true;
            TextBox2.Text = Class1.NumPadret.ToString();
            if (AddingNew == false)
                ONModifyChangeButtons();


        }

        private void Gas1TB_Click(System.Object sender, System.EventArgs e)
        {
            if (Gas1TB.Text != "")
                Class1.NumPadsend = Convert.ToDouble(Gas1TB.Text);
            Numeric_Pad objNumAlpha3 = new Numeric_Pad();
            objNumAlpha3.ShowDialog();
            ModifingCurrentP = true;
            this.Gas1TB.Text = Class1.NumPadret.ToString();

            if (AddingNew == false)
                ONModifyChangeButtons();

        }

        private void Gas2TB_Click(System.Object sender, System.EventArgs e)
        {
            if (Gas2TB.Text != "")
                Class1.NumPadsend = Convert.ToDouble(Gas2TB.Text);
            Numeric_Pad objNumAlpha4 = new Numeric_Pad();
            objNumAlpha4.ShowDialog();
            //Numeric_Pad.ShowDialog();
            ModifingCurrentP = true;
            this.Gas2TB.Text = Class1.NumPadret.ToString();

            if (AddingNew == false)
                ONModifyChangeButtons();

        }

        private void Gas3TB_Click(System.Object sender, System.EventArgs e)
        {
            if (Gas3TB.Text != "")
                Class1.NumPadsend = Convert.ToDouble(Gas3TB.Text);
            Numeric_Pad objNumAlpha5 = new Numeric_Pad();
            objNumAlpha5.ShowDialog();
            ModifingCurrentP = true;
            this.Gas3TB.Text = Class1.NumPadret.ToString();

            if (AddingNew == false)
                ONModifyChangeButtons();

        }

        private void TextBox6_Click(System.Object sender, System.EventArgs e)
        {
            if (TextBox6.Text != "")
                Class1.NumPadsend = Convert.ToDouble(TextBox6.Text);
            Numeric_Pad objNumAlpha6 = new Numeric_Pad();
            objNumAlpha6.ShowDialog();
            /*NumPadsend = Conversion.Val(TextBox6.Text);
            Numeric_Pad.ShowDialog();*/
            ModifingCurrentP = true;
            this.TextBox6.Text = Class1.NumPadret.ToString();

            if (AddingNew == false)
                ONModifyChangeButtons();

        }

        private void TextBox10_Click(System.Object sender, System.EventArgs e)
        {
            if (TextBox10.Text != "")
                Class1.NumPadsend = Convert.ToDouble(TextBox10.Text);
            Numeric_Pad objNumAlpha7 = new Numeric_Pad();
            objNumAlpha7.ShowDialog();
            /*NumPadsend = Conversion.Val(TextBox10.Text);
            Numeric_Pad.ShowDialog();*/
            ModifingCurrentP = true;
            this.TextBox10.Text = Class1.NumPadret.ToString();

            //this.TextBox10.Text = NumPadRet;
            if (AddingNew == false)
                ONModifyChangeButtons();

        }

        private void TextBox7_Click(System.Object sender, System.EventArgs e)
        {
            if (TextBox7.Text != "")
                Class1.NumPadsend = Convert.ToDouble(TextBox7.Text);
            Numeric_Pad objNumAlpha8 = new Numeric_Pad();
            objNumAlpha8.ShowDialog();
            ModifingCurrentP = true;
            this.TextBox7.Text = Class1.NumPadret.ToString();

            if (AddingNew == false)
                ONModifyChangeButtons();

        }

        private void TextBox8_Click(System.Object sender, System.EventArgs e)
        {
            if (TextBox8.Text != "")
                Class1.NumPadsend = Convert.ToDouble(TextBox8.Text);
            Numeric_Pad objNumAlpha9 = new Numeric_Pad();
            objNumAlpha9.ShowDialog();
            /*NumPadsend = Conversion.Val(TextBox8.Text);
            Numeric_Pad.ShowDialog();*/
            ModifingCurrentP = true;
            this.TextBox8.Text = Class1.NumPadret.ToString();

            if (AddingNew == false)
                ONModifyChangeButtons();

        }



        private void TextBox1_TextChanged(System.Object sender, System.EventArgs e)
        {
            if (FirstScan > 0)
            {
                if (string.IsNullOrEmpty(TextBox1.Text))
                    return;
                if (Convert.ToDouble(TextBox1.Text) < 0.03)
                {
                    MessageBox.Show("Please insert values between 0.03 and 1 mbar");
                    TextBox1.Text = "";
                }
                if (TextBox1.Text != "")
                {
                    if (Convert.ToDouble(TextBox1.Text) > 1)
                    {
                        TextBox1.Text = "";
                        MessageBox.Show("Please insert values between 0.03 and 1 mbar");
                    }
                }
            }
            FirstScan = FirstScan + 1;
            if (FirstScan > 1)
                FirstScan = 1;
            if (ModifingCurrentP == true)
            {
                Label5.ForeColor = Color.Red;
                Label5.Text = " Attention, the record displayed is not loaded in the System. !!";
                return;

            }
        }

        private void TextBox2_TextChanged(System.Object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox2.Text))
                return;
            decimal d = Convert.ToDecimal(TextBox2.Text);
            if ((d % 1) > 0)
            {
                MessageBox.Show("Can not enter Decimal values for TTP");
                TextBox2.Text = "";
            }
            else
                if (Convert.ToDouble(TextBox2.Text) < 1.0 | Convert.ToDouble(TextBox2.Text) > 5.0)
                {
                    MessageBox.Show("Please Enter values between 1 and 5.");

                    TextBox2.Text = "";
                }


            if (ModifingCurrentP == true)
            {
                Label5.ForeColor = Color.Red;
                Label5.Text = " Attention, the record displayed is not loaded in the System. !!";
                return;

            }
        }


        private void Gas1TB_TextChanged(System.Object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(Gas1TB.Text))
                return;
            if (Convert.ToDouble(Gas1TB.Text) < 0)
            {
                MessageBox.Show("No negative values allowed!");
                Gas1TB.Text = "0";
            }
            float maxG = 0;
            maxG = float.Parse(Class1.Gas1R.ToString()) / float.Parse(Class1.GCF1.ToString());
            if (float.Parse(Gas1TB.Text) > maxG)
            {
                Gas1TB.Text = "0";
                MessageBox.Show("Please insert values lower than " + maxG.ToString());
            }
            if (ModifingCurrentP == true)
            {
                Label5.ForeColor = Color.Red;
                Label5.Text = " Attention, the record displayed is not loaded in the System. !!";
                return;

            }
        }


        private void Gas2TB_TextChanged(System.Object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(Gas2TB.Text))
                return;
            if (Convert.ToDouble(Gas2TB.Text) < 0)
            {
                MessageBox.Show("No negative values allowed!");
                Gas2TB.Text = "0";
            }
            float maxG = 0;
            maxG = float.Parse(Class1.Gas2R.ToString()) / float.Parse(Class1.GCF2.ToString());
            if (float.Parse(Gas2TB.Text) > maxG)
            {
                Gas2TB.Text = "0";
                MessageBox.Show("Please insert values lower than " + maxG.ToString());
            }
            if (ModifingCurrentP == true)
            {
                Label5.ForeColor = Color.Red;
                Label5.Text = " Attention, the record displayed is not loaded in the System. !!";
                return;

            }
        }


        private void Gas3TB_TextChanged(System.Object sender, System.EventArgs e)
        {
            if (Class1.Gas3 == true)
            {
                if (string.IsNullOrEmpty(Gas3TB.Text))
                    return;
                if (Convert.ToDouble(Gas3TB.Text) < 0)
                {
                    MessageBox.Show("No negative values allowed!");
                    Gas3TB.Text = "0";
                }
                else
                {
                    if (Gas3TB.Text == "N/A") ;
                }




                double maxG = 0;
                maxG = Class1.Gas3R / Class1.GCF3;
                if (Convert.ToInt32(Gas3TB.Text) > maxG)
                {
                    Gas3TB.Text = "0";
                    MessageBox.Show("Please insert values lower than " + maxG.ToString());
                }
                if (ModifingCurrentP == true)
                {
                    Label5.ForeColor = Color.Red;
                    Label5.Text = " Attention, the record displayed is not loaded in the System. !!";
                    return;

                }
            }
        }

        private void TextBox6_TextChanged(System.Object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox6.Text))
                return;
            if (Convert.ToDouble(TextBox6.Text) < 0)
            {
                MessageBox.Show("No negative values allowed!");
                TextBox6.Text = "0";
            }
            if (Convert.ToDouble(TextBox6.Text) > Class1.SetupRFRange)
            {
                TextBox6.Text = "0";
                MessageBox.Show("Please insert values lower than " + Class1.SetupRFRange.ToString());
            }
            if (ModifingCurrentP == true)
            {
                Label5.ForeColor = Color.Red;
                Label5.Text = " Attention, the record displayed is not loaded in the System. !!";
                return;

            }
        }


        //private void TextBox10_TextChanged(System.Object sender, System.EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(TextBox10.Text))
        //        return;
        //    if (Convert.ToInt32(TextBox10.Text) > 900)
        //    {
        //        if (MessageBox.Show("That is a long time for RF Time. Are you sure you want to change this parameter?", "My Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //        {
        //        }
        //        else
        //        {
        //            TextBox10.Text = "";
        //        }
        //    }
        //    if (ModifingCurrentP == true)
        //    {
        //        Label5.ForeColor = Color.Red;
        //        Label5.Text = " Attention, the record displayed is not loaded in the System. !!";
        //        return;

        //    }
        //}

        private void TextBox10_TextChanged(System.Object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox10.Text))
                return;
            decimal d = Convert.ToDecimal(TextBox10.Text);
            if ((d % 1) > 0)
            {
                MessageBox.Show("Can not enter Decimal values for RFTime");
                TextBox10.Text = "";
            }
            else
                if (Convert.ToDouble(TextBox10.Text) > 900)
                {
                    if (MessageBox.Show("That is a long time for RF Time. Are you sure you want to change this parameter?", "My Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        TextBox10.Text = "";

                }
            if (ModifingCurrentP == true)
            {
                Label5.ForeColor = Color.Red;
                Label5.Text = " Attention, the record displayed is not loaded in the System. !!";
                return;

            }
        }



        private void TextBox7_TextChanged(System.Object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox7.Text))
                return;
            if (Convert.ToDouble(TextBox7.Text) < 1)
            {
                MessageBox.Show("Please insert values between 1 % and 99 %");
                TextBox7.Text = "50";
            }
            if (Convert.ToDouble(TextBox7.Text) > 99)
            {
                TextBox7.Text = "50";
                MessageBox.Show("Please insert values between 1 % and 99 %");
            }
            if (ModifingCurrentP == true)
            {
                Label5.ForeColor = Color.Red;
                Label5.Text = " Attention, the record displayed is not loaded in the System. !!";
                return;

            }
        }


        private void TextBox8_TextChanged(System.Object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox8.Text))
                return;
            if (Convert.ToDouble(TextBox8.Text) < 1)
            {
                MessageBox.Show("Please insert values between 1 % and 99 %");
                TextBox8.Text = "50";
            }
            if (Convert.ToDouble(TextBox8.Text) > 99)
            {
                TextBox8.Text = "50";
                MessageBox.Show("Please insert values between 1 % and 99 %");
            }
            if (ModifingCurrentP == true)
            {
                Label5.ForeColor = Color.Red;
                Label5.Text = " Attention, the record displayed is not loaded in the System. !!";
                return;

            }
        }

        private void Programs_Activated(object sender, System.EventArgs e)
        {
            ToolStripButton3.Text = "Fill";
            //ModifingCurrentP = False
        }
        private object DownloadProgram()
        {

            //DownLoad Program
            Class1.ProgFlag = true;
            Class1.CurrentP = TextBox9.Text;
            startupTAobj.UpdateStartupProgram(Class1.CurrentP);
            this.programsTAobj.FillBy(setupDatasetObj.Programs, Class1.CurrentP);
            Label5.Text = " This record is currently loaded in the System.";
            Label5.ForeColor = Color.White;
            ToolStripButton3.Text = "Fill";

            Class1.R_ID = TextBox9.Text;

            Class1.R_PT = Math.Round(Convert.ToDouble(TextBox1.Text), 2);

            Class1.R_TTP = Convert.ToDouble(TextBox2.Text);

            Class1.R_RFT = Convert.ToDouble(TextBox10.Text);


            Class1.R_PW = Convert.ToDouble(TextBox6.Text);

            try
            {
                //Analog Output
                Int32 ret1 = default(Int32);
                double val1 = 0;

                val1 = Convert.ToDouble(Class1.R_PW / Class1.SetupRFRange * 10);
                ret1 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAO, 0, val1);

            }
            catch (Exception ex)
            {
            }


            Class1.R_G1 = Convert.ToDouble(Gas1TB.Text);
            try
            {
                Int32 ret2 = default(Int32);
                double val2 = 0;

                val2 = Convert.ToDouble(Class1.R_G1 / (Class1.Gas1R / 10) * Class1.GCF1);
                ret2 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAO, 1, val2);
            }
            catch (Exception ex)
            {
            }


            Class1.R_G2 = Convert.ToDouble(Gas2TB.Text);
            try
            {
                Int32 ret3 = default(Int32);
                double val3 = 0;

                val3 = Convert.ToDouble(Class1.R_G2 / (Class1.Gas2R / 10) * Class1.GCF2);
                ret3 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAO, 2, val3);
            }
            catch (Exception ex)
            {
            }
            if (Gas3TB.Text != "N/A")
            {
                Class1.R_G3 = Convert.ToDouble(Gas3TB.Text);
            }
            if (Gas3TB.Text != " " && Gas3TB.Text != "N/A")
            {
                Class1.R_G3 = Convert.ToDouble(Gas3TB.Text);
            }
            try
            {
                Int32 ret4 = default(Int32);
                double val4 = 0;

                val4 = Convert.ToDouble(Class1.R_G3 / (Class1.Gas3R / 10) * Class1.GCF3);
                ret4 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAO, 3, val4);
            }
            catch (Exception ex)
            {
            }

            Class1.R_TP = Convert.ToDouble(TextBox7.Text);

            try
            {
                //Analog Output
                Int32 ret5 = default(Int32);
                double val5 = 0;

                val5 = Convert.ToDouble(Class1.R_TP / 10);
                ret5 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAI, 0, val5);

            }
            catch (Exception ex)
            {
            }

            Class1.R_LP = Convert.ToDouble(TextBox8.Text);

            try
            {
                //Analog Output
                Int32 ret6 = default(Int32);
                double val6 = 0;

                val6 = Convert.ToDouble(Class1.R_LP / 10);
                ret6 = APS168.APS_set_field_bus_a_output(CardID, BusNo, Class1.moduleAI, 1, val6);

            }
            catch (Exception ex)
            {
            }
            if (ModifingCurrentP == true)
            {
                MessageBox.Show("Modified and Downloaded to System.");
                ModifingCurrentP = false;
            }
            else
            {
                MessageBox.Show("Downloaded to System.");
            }



            //Main1 objMain1 = new Main1();
            //objMain1.Invalidate();
            //objMain1.Refresh();



            return 0;
        }

        private void BindingNavigator2_RefreshItems(object sender, EventArgs e)
        {

        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }

        private void ToolStripButton2_Click_1(object sender, EventArgs e)
        {
            AddingNew = false;
            if (ModifingCurrentP == true)
            {
                if (MessageBox.Show("Attention, You have not modified the program. The old program is still installed. After modification of one or more fields, press \"Modify\" to update the database and download the modifcation to the system. Do you wish to Exit anyway?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                {
                    //ModifingCurrentP = False
                    this.Close();
                    this.Dispose();
                }
            }
            else
            {
                //ModifingCurrentP = False
                this.Close();
                this.Dispose();
            }
            if (Class1.IOInfoSamePg == true & Class1.IOOpen == true)
            {
                Class1.openIO.Show();
            }
            this.Invalidate();

        }

        private void ToolStripModify_Click(object sender, EventArgs e)
        {
            string Gas3val;
            try
            {
                if (Gas3TB.Text != "" && Gas3TB.Text != "N/A")
                    Gas3val = Gas3TB.Text;
                else
                    Gas3val = "0";
                //    Gas3val = "0";
                //Gas3TB.Text ="0";
                programsTAobj.UpdateProgram(float.Parse(TextBox1.Text), Convert.ToInt32(TextBox2.Text), float.Parse(Gas1TB.Text), float.Parse(Gas2TB.Text), float.Parse(Gas3val), float.Parse(TextBox6.Text), Convert.ToInt32(TextBox10.Text), float.Parse(TextBox7.Text), float.Parse(TextBox8.Text), TextBox9.Text);
                //this.ProgramsTableAdapter.UpdateProgram(Convert.ToDouble(this.TextBox1.Text), Convert.ToDouble(this.TextBox2.Text), Convert.ToDouble(this.Gas1TB.Text), Convert.ToDouble(this.Gas2TB.Text), Convert.ToDouble(this.Gas3TB.Text), Convert.ToDouble(this.TextBox6.Text), Convert.ToDouble(this.TextBox10.Text), Convert.ToDouble(this.TextBox7.Text), Convert.ToDouble(this.TextBox8.Text), Convert.ToDouble(this.TextBox9.Text);
                StartModifyButtons();

                if (TextBox9.Text == Class1.CurrentP.ToUpper())
                {
                    DownloadProgram();
                    ModifingCurrentP = false;

                }
                else
                {
                    ModifySuccess Mobj = new ModifySuccess();
                    Mobj.Show();
                    Label5.Text = " Attention, the record displayed is not loaded in the System. !!";
                    Label5.ForeColor = Color.Red;
                    ModifingCurrentP = false;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot have Programs with same name!");
                AddingNew = false;

            }
            ModifingCurrentP = false;

        }

        private void ToolStripAddNew_Click(object sender, EventArgs e)
        {
            AddingNew = true;
            ProgramsBindingSource.AddNew();
            TextBox9.Enabled = true;
            TextBox9.Text = "";
            OnAddNewChangeButtons();
            Label5.Text = "";
        }

        private void ToolStripSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.TextBox9.Text))
            {
                TextBox9.Enabled = false;
                goto outout;
            }
            if (string.IsNullOrEmpty(this.TextBox1.Text))
            {
                TextBox9.Enabled = false;
                MessageBox.Show("Cannot Save programs with empty fields!");
                goto outout;
            }

            if (string.IsNullOrEmpty(this.TextBox2.Text))
            {
                TextBox9.Enabled = false;
                MessageBox.Show("Cannot Save programs with empty fields!");
                goto outout;
            }

            if (string.IsNullOrEmpty(this.Gas1TB.Text))
            {
                TextBox9.Enabled = false;
                MessageBox.Show("Cannot Save programs with empty fields!");
                goto outout;
            }
            if (string.IsNullOrEmpty(this.Gas2TB.Text))
            {
                TextBox9.Enabled = false;
                MessageBox.Show("Cannot Save programs with empty fields!");
                goto outout;
            }
            //if (Class1.DO_StatusGAS3 == true)
            //{
            //    if (string.IsNullOrEmpty(this.Gas3TB.Text))
            //    {
            //        TextBox9.Enabled = false;
            //        MessageBox.Show("Cannot Save programs with empty fields!");
            //        goto outout;
            //    }
            //}
            if (string.IsNullOrEmpty(this.TextBox6.Text))
            {
                TextBox9.Enabled = false;
                MessageBox.Show("Cannot Save programs with empty fields!");
                goto outout;
            }
            if (string.IsNullOrEmpty(this.TextBox10.Text))
            {
                TextBox9.Enabled = false;
                MessageBox.Show("Cannot Save programs with empty fields!");
                goto outout;
            }
            if (string.IsNullOrEmpty(this.TextBox7.Text))
            {
                TextBox9.Enabled = false;
                MessageBox.Show("Cannot Save programs with empty fields!");
                goto outout;
            }
            if (string.IsNullOrEmpty(this.TextBox8.Text))
            {
                TextBox9.Enabled = false;
                MessageBox.Show("Cannot Save programs with empty fields!");
                goto outout;
            }




            try
            {
                string Gas3TBbuf;
                if (Gas3TB.Text == "")
                    Gas3TBbuf = "0";
                else
                    Gas3TBbuf = Gas3TB.Text;

                float tempGas3TB = float.Parse(Gas3TBbuf);
                this.programsTableAdapter.InsertProgram(TextBox9.Text, float.Parse(TextBox1.Text), Convert.ToInt32(TextBox2.Text), float.Parse(Gas1TB.Text), float.Parse(Gas2TB.Text), tempGas3TB, float.Parse(TextBox6.Text), Convert.ToInt32(TextBox10.Text), float.Parse(TextBox7.Text), float.Parse(TextBox8.Text));

            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot have program with same name!");
            }
        outout:
            this.programsTableAdapter.Fill(setupDataSet.Programs);
            //programsTAobj.Fill(setupDatasetObj.Programs);
            StartModifyButtons();
            AddingNew = false;
            Label5.Text = " Attention, the record displayed is not loaded in the System. !!";

            ModifingCurrentP = false;


        }

        private void ToolStripDelete_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to delete this record?", "Attention!", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                programsTAobj.DeleteProgram(TextBox9.Text);
                this.programsTableAdapter.Fill(this.setupDataSet.Programs);
                // programsTAobj.Fill(setupDatasetObj.Programs);
            }
            else
            {
                return;
            }

        }

        private void ToolStripButton3_Click(object sender, EventArgs e)
        {

            if (ToolStripButton3.Text == "Fill")
            {
                ToolStripButton3.Text = "Current";
                this.programsTableAdapter.Fill(this.setupDataSet.Programs);
                //programsTAobj.Fill(setupDatasetObj.Programs);
            }
            else
            {
                ToolStripButton3.Text = "Fill";
                Class1.CurrentP = this.startupTableAdapter.SelectStartupProgram();
                this.programsTableAdapter.FillBy(this.setupDataSet.Programs, Class1.CurrentP);
                

            }
            TBGhost.Text = TextBox9.Text;



        }

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            ModifingCurrentP = false;
            DownloadProgram();
        }

        private void TextBox9_TextChanged(object sender, EventArgs e)
        {
            if (TextBox9.Text.Length > 20)
            {
                MessageBox.Show("Max 20 characters!");
                //TextBox9.Text = "" 
            }
            //If TextBox9.Text = "" Then TextBox9.Text = CurrentP

            if (TextBox9.Text.ToUpper() == Class1.CurrentP.ToUpper())
            {
                Label5.Text = " This record is currently loaded in the System.";
                Label5.ForeColor = Color.White;
            }
            else
            {
                Label5.ForeColor = Color.Red;
                Label5.Text = " Attention, the record displayed is not loaded in the System. !!";
            }




        }

        private void TextBox9_Click(System.Object sender, System.EventArgs e)
        {
            Alphapad objAlpha = new Alphapad();
            Class1.AlphaPadret = TextBox9.Text;
            objAlpha.ShowDialog();
            //AlphaPad.ShowDialog();
            ModifingCurrentP = true;
            TextBox9.Text = Class1.AlphaPadret.ToUpper();
            if (AddingNew == false)
                ONModifyChangeButtons();

        }

        private void TextBox9_Validating(object sender, CancelEventArgs e)
        {
            Class1.ProgramName = TextBox9.Text;
            Class2.progCheck();
            if (Class1.programIDFlag == true)
            {
                MessageBox.Show("Program ID" + " " + TextBox9.Text + " " + " already existed, Please select new program ID");
                TextBox9.Text = "";
                Class1.programIDFlag = false;
                TextBox9.Focus();
            }
        }









    }
}

