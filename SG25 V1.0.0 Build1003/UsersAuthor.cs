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
    public partial class UsersAuthor : Form
    {
        public UsersAuthor()
        {
            InitializeComponent();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Class1.AllowedDenied = this.Button3.Text;
            this.Close();

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Class1.AllowedDenied = this.Button1.Text;
            this.Close();

        }
    }
}
