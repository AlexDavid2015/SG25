using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SG25
{
    public partial class Documentation : Form
    {
        public Documentation()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            try
            {
                p.StartInfo.FileName = Class1.sProjectPath + "\\Manuals\\Dummy.pdf";
                p.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("cannot find the requested file.");
            }
            this.Close();
            this.Dispose();

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            try
            {
                p.StartInfo.FileName = Class1.sProjectPath + "\\Manuals\\Dummy.pdf";
                p.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("cannot find the requested file.");
            }
            this.Close();
            this.Dispose();

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            try
            {
                p.StartInfo.FileName = Class1.sProjectPath + "\\Manuals\\Dummy.pdf";
                p.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("cannot find the requested file.");
            }

            this.Close();
            this.Dispose();

        }

        private void Button4_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void Documentation_Load(object sender, EventArgs e)
        {

        }
    }
}
