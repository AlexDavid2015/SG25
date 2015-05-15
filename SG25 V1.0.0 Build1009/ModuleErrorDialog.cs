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
    public partial class ModuleErrorDialog : Form
    {
        public ModuleErrorDialog()
        {
            InitializeComponent();
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void ModuleErrorDialog_Load(object sender, EventArgs e)
        {
            if (AvantechAIs.m_iFailCount > 1)
            {
                lblAIErr.Text = "-1";
            }
            if (AvantechDOs.m_iFailCount > 1)
            {
                lblDOErr.Text = "-1";
            }
            if (AvantechDIs.m_iFailCount > 1)
            {
                lblDIErr.Text = "-1";
            }
            if (AvantechAOs.m_iFailCount > 1)
            {
                lblAOErr.Text = "-1";
            }
            if (AvantechDIOs.m_iFailCount > 1)
            {
                lblDIOErr.Text = "-1";
            }
        }
    }
}
