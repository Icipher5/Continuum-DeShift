using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Continuum_DeShift
{
    public partial class addDialog : Form
    {
        private Queue qFiles;
        private string sPath;

        public addDialog()
        {
            InitializeComponent();
            sPath = "";
            qFiles = new Queue();
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            if (!IsValidPath(tbPacFile.Text))
            {
                MessageBox.Show("You need to select a file to save as.");
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

        private void bBrowse_Click(object sender, EventArgs e)
        {
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.Filter = "pac Files|*.pac";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbPacFile.Text = saveFileDialog1.FileName;
            }
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "All Files|*.*";
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                lbSelected.Items.Clear();
                sPath = openFileDialog1.FileName;
                sPath = sPath.Replace(openFileDialog1.SafeFileName, "");
                for (int i = 0; i < openFileDialog1.FileNames.Count(); i++)
                {
                    lbSelected.Items.Add(openFileDialog1.FileNames[i]);
                }
            }
        }

        private void lbSelected_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                lbSelected.Items.RemoveAt(lbSelected.SelectedIndex);
            }
        }

        public string getSave()
        {
            return tbPacFile.Text;
        }

        public Queue getAdds()
        {
            qFiles = new Queue();
            foreach (Object obj in lbSelected.Items)
            {
                qFiles.Enqueue(obj);
            }

            return qFiles;
        }

        public string getPath()
        {
            return sPath;
        }

        public Int32 getIsText()
        {
            if (cbText.Checked)
            {
                return 0x11;
            }
            else
            {
                return 0x01;
            }
        }

        static public bool IsValidPath( string path )
        {
            Regex r = new Regex( @"^(([a-zA-Z]\:)|(\\))(\\{1}|((\\{1})[^\\]([^/:*?<>""|]*))+)$" );
            return r.IsMatch( path );
        }
    }
}
