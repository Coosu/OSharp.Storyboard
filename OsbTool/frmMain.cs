using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibOsb;

namespace OsbTool
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnAdj_Click(object sender, EventArgs e)
        {
            try
            {
                string osb = tbSource.Text;
                int timing = int.Parse(tbTiming.Text);
                double x = double.Parse(tbX.Text);
                double y = double.Parse(tbY.Text);
                var before = ElementGroup.Parse(osb, 0);
                //tbSource.Text = before.ToString();
                var after = ElementManager.Adjust(before, x, y, timing);
                tbHandled.Text = after.ToString();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    MessageBox.Show(ex.Message + "\r\n" + ex.InnerException.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
