using System;
using System.Diagnostics;
using System.Windows.Forms;
using Milkitic.OsbLib;

namespace Milkitic.OsbTool
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnAdj_Click(object sender, EventArgs e)
        {
            try
            {
                string osb = tbSource.Text;
                int timing = int.Parse(tbTiming.Text);
                float x = float.Parse(tbX.Text);
                float y = float.Parse(tbY.Text);
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSOfg_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "osb files (*.osb)|*.osb|All files (*.*)|*.*";
            var result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                tb_Spath.Text = ofd.FileName;
            }
        }

        private void btnTOfg_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "osb files (*.osb)|*.osb|All files (*.*)|*.*";
            var result = sfd.ShowDialog();
            if (result == DialogResult.OK)
            {
                tb_Tpath.Text = sfd.FileName;
            }
        }

        private void btn_ExeComp_Click(object sender, EventArgs e)
        {
            string sPath = tb_Spath.Text;
            string tPath = tb_Tpath.Text;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string text = System.IO.File.ReadAllText(sPath);
            ElementManager em = new ElementManager();
            try
            {
                var parsed = ElementGroup.Parse(text, 0);
                parsed.Compress();
                em.Add(parsed);
                em.Save(tPath);
                sw.Stop();
                MessageBox.Show("Finished work in " + sw.ElapsedMilliseconds + "ms.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
