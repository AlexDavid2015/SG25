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
    public partial class LogEventAuto : Form
    {
        public LogEventAuto()
        {
            InitializeComponent();
        }

        private void LogEventAuto_Load(object sender, EventArgs e)
        {
            Class1.DGVMain = DGVLogList;
        }

        private void DGVLogList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtLogEventLog.Text = DGVLogList.Rows[e.RowIndex].Cells[0].Value.ToString() + " " + DGVLogList.Rows[e.RowIndex].Cells[1].Value.ToString() + " " + DGVLogList.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}
