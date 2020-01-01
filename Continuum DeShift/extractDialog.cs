using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Continuum_DeShift
{
    public partial class extractDialog : Form
    {
        public extractDialog(string path)
        {
            InitializeComponent();
            string dir = path.Remove(path.Length - 4, 4);
            tbPacFile.Text = dir + "\\";
        }

        private void bBrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tbPacFile.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void bExtract_Click(object sender, EventArgs e)
        {
            if (!IsValidPath(tbPacFile.Text))
            {
                MessageBox.Show("You need to select a folder to extract to.");
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                Hide();
            }
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        public string getExtract()
        {
            return tbPacFile.Text;
        }

        static public bool IsValidPath(string path)
        {
            Regex r = new Regex(@"^(([a-zA-Z]\:)|(\\))(\\{1}|((\\{1})[^\\]([^/:*?<>""|]*))+)$");
            return r.IsMatch(path);
        }
    }
}
