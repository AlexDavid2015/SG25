using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Math;

namespace SG25
{
    public partial class Graph : Form
    {
        public Graph()
        {
            InitializeComponent();
        }

        System.Drawing.Drawing2D.GraphicsPath PWRPath = new System.Drawing.Drawing2D.GraphicsPath();
        //declare a new Graphic path to follow the movement
        System.Drawing.Drawing2D.GraphicsPath RevPath = new System.Drawing.Drawing2D.GraphicsPath();
        //declare a new Graphic path to follow the movement
        System.Drawing.Drawing2D.GraphicsPath TunePath = new System.Drawing.Drawing2D.GraphicsPath();
        //declare a new Graphic path to follow the movement
        System.Drawing.Drawing2D.GraphicsPath LoadPath = new System.Drawing.Drawing2D.GraphicsPath();
        //declare a new Graphic path to follow the movement



        int myAlpha = 255;
        // declare a Alpha variable
        Color myUserColor = new Color();
        //this is a color the user selects
        float myPenWidth = 2;
        //set pen width variable
        float PXP = 0;
        float PXR = 0;
        float PXT = 0;
        float PXL = 0;
        float PYRF = 0;
        float PYREV = 0;
        float PYTune = 0;
        float PYLoad = 0;


        private void PBRFFWD_Click(object sender, EventArgs e)
        {

        }

        private void Button9_Click(object sender, EventArgs e)
        {
            Timer1.Enabled = true;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (Class1.DO_RFON == true)
            {
               PYRF = Convert.ToInt16(Math.Round((Class1.AI_ARFPowerValue * 15) + 3));
               PYREV = Convert.ToInt16(Math.Round((Class1.AI_RFRefelctedValue * 15) + 3));
               PYTune = Convert.ToInt16(Math.Round((Class1.AI_TuneValue * 15) + 3));
               PYLoad = Convert.ToInt16(Math.Round((Class1.AI_LoadValue * 15) + 3));


                PXP = PXP + 5;
                PXR = PXR + 5;
                PXT = PXT + 5;
                PXL = PXL + 5;

                PWRPath.AddLine(PXP, PYRF, PXP, PYRF);
                RevPath.AddLine(PXR, PYREV, PXR, PYREV);
                TunePath.AddLine(PXT, PYTune, PXT, PYTune);
                LoadPath.AddLine(PXL, PYLoad, PXL, PYLoad);

                if (PXP > PBRFFWD.Width - 10)
                {
                    PXP = 0;
                    PWRPath.Reset();
                    PWRPath.AddLine(PXP, PYRF, PXP, PYRF);
                }
                if (PXR > PBRFREV.Width - 10)
                {
                    PXR = 0;
                    RevPath.Reset();
                    RevPath.AddLine(PXR, PYREV, PXR, PYREV);
                }
                if (PXT > PBTUNE.Width - 10)
                {
                    PXT = 0;
                    TunePath.Reset();
                    TunePath.AddLine(PXT, PYTune, PXT, PYTune);
                }
                if (PXL > PBLOAD.Width - 10)
                {
                    PXL = 0;
                    LoadPath.Reset();
                    LoadPath.AddLine(PXL, PYLoad, PXL, PYLoad);
                }
                PBRFFWD.Invalidate();
                PBRFREV.Invalidate();
                PBTUNE.Invalidate();
                PBLOAD.Invalidate();
            }

        }

        private void Graph_Load(object sender, EventArgs e)
        {
            this.Width = 938;
            this.Height = 1024;
            this.Top = 0;
            this.Left = 330;

            PWRPath.StartFigure();
            RevPath.StartFigure();
            TunePath.StartFigure();
            LoadPath.StartFigure();

            FlatButtonAppearance fa2 = Button2.FlatAppearance;
            fa2.BorderSize = 0;
            FlatButtonAppearance fa3 = Button3.FlatAppearance;
            fa3.BorderSize = 0;
            FlatButtonAppearance fa4 = Button4.FlatAppearance;
            fa4.BorderSize = 0;
            FlatButtonAppearance fa5 = Button5.FlatAppearance;
            fa5.BorderSize = 0;

            if (Class1.Gas3 == true)
            {
                Label10.Visible = true;
                Label12.Visible = true;

            }
            else
            {
                Label10.Visible = false;
                Label12.Visible = false;
            }

            if (Class1.UManual == "Denied")
            {
                Button6.Visible = false;
            }
            else
            { Button6.Visible = true; }
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            PXP = 0;
            PXR = 0;
            PXT = 0;
            PXL = 0;
            PWRPath.Reset();
            RevPath.Reset();
            LoadPath.Reset();
            TunePath.Reset();
            PBRFFWD.Invalidate();
            PBRFREV.Invalidate();
            PBTUNE.Invalidate();
            PBLOAD.Invalidate();

        }

        private void Button6_Click(object sender, EventArgs e)
        {
            Programs fq = new Programs();
            fq.ShowDialog();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            this.Close();
            this.Dispose();

        }

        private void Graph_Resize(object sender, EventArgs e)
        {
            this.Width = 938;
            this.Height = 1024;
            this.Top = 0;
            this.Left = 330;
        }
    }
}
