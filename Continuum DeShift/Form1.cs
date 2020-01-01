using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Continuum_DeShift
{
    public partial class Form1 : Form
    {
        private string sCurrfile;
        private string sPath, sAddPath;
        private string sSave, sExtract;
        private Int32 iText;
        private FileHeader fileHeader;
        private ListViewColumnSorter lvwColumnSorter;
        private Queue qFiles;

        public Form1()
        {
            InitializeComponent();
            lvwColumnSorter = new ListViewColumnSorter();
            this.listView1.ListViewItemSorter = lvwColumnSorter;
        }

        public Form1(string[] args)
        {
            InitializeComponent();
            lvwColumnSorter = new ListViewColumnSorter();
            this.listView1.ListViewItemSorter = lvwColumnSorter;
            contextMenuStrip1.Items[0].Enabled = true;
            resetEverything();
            if (args[0] == "extract")
            {
                for (int i = 1; i < args.Length; i++)
                {
                    sCurrfile = args[i];
                    int index = sCurrfile.LastIndexOf("\\");
                    sPath = sCurrfile.Remove(index + 1);
                    readHeader();
                    extract(sPath);
                }
                Environment.Exit(0);
            }
            else
            {
                sCurrfile = args[0];
                int index = sCurrfile.LastIndexOf("\\");
                sPath = sCurrfile.Remove(index + 1);
                readHeader();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About ab = new About();
            ab.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "pac Files|*.pac";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                contextMenuStrip1.Items[0].Enabled = true;
                resetEverything();
                sCurrfile = openFileDialog1.FileName;
                sPath = sCurrfile.Remove(sCurrfile.Length - openFileDialog1.SafeFileName.Length, openFileDialog1.SafeFileName.Length);
                Form1.ActiveForm.Text = "Continuum DeShift - " + openFileDialog1.SafeFileName;
                readHeader();
            }
        }

        private void readHeader()
        {
            FileStream fs = null;
            BinaryReader br = null;
            try
            {
                fs = File.OpenRead(sCurrfile);
                br = new BinaryReader(fs);

                //read the identifier and check if it's a pac file
                string identifier = new string(br.ReadChars(4));
                if (identifier != "FPAC")
                {
                    MessageBox.Show("Invalid .pac file!");
                    return;
                }

                //skips ahead to find the number of files
                br.ReadInt64();
                fileHeader = new FileHeader(br.ReadInt32());
                //resets the read postion
                br.BaseStream.Position = 4;

                //read data from header
                fileHeader.iDataStart = br.ReadInt32();
                fileHeader.iFileSize = br.ReadInt32();
                fileHeader.iNumFiles = br.ReadInt32();
                //this dword is always = to 0x01 except for in language files where it's 0x11
                br.ReadInt32();
                //this is proabaly supposed to be reading 8 bytes instead of 4, but no current pac file uses more than 4
                fileHeader.iFileNameLength = br.ReadInt32();
                br.ReadInt64();

                ListViewItem lviTemp;
                for (int i = 0; i < fileHeader.iNumFiles; i++)
                {
                    fileHeader.sFileNames[i] = new string(br.ReadChars(fileHeader.iFileNameLength)).TrimEnd('\0');
                    fileHeader.iFileNumber[i] = br.ReadInt32();
                    fileHeader.iDataOffset[i] = fileHeader.iDataStart + br.ReadInt32();
                    fileHeader.iFileSizes[i] = br.ReadInt32();
                    br.ReadInt32();
                    //need to read number of bytes that will get the read position
                    //to the next multiple of 16
                    br.ReadBytes(calculatePadding((int)br.BaseStream.Position, 16));

                    //displays info in the listview
                    lviTemp = new ListViewItem(fileHeader.sFileNames[i], 0);
                    lviTemp.SubItems.Add(fileHeader.iFileSizes[i].ToString());
                    lviTemp.Tag = fileHeader.iDataOffset[i];
                    listView1.Items.Add(lviTemp);
                }
                extractToolStripMenuItem.Enabled = true;
                extractAllButton.Enabled = true;
            }
            catch (System.IO.IOException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (br != null)
                    br.Close();

                if (fs != null)
                    fs.Close();
            }
        }

        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            extractDialog extractForm = new extractDialog(sCurrfile);
            if (extractForm.ShowDialog() == DialogResult.OK)
            {
                sExtract = extractForm.getExtract();
                extractForm.Close();

                extract(sExtract);
            }
        }

        private void extract(string path)
        {
            FileStream fr = null;
            BinaryReader br = null;
            try
            {
                fr = File.OpenRead(sCurrfile);
                br = new BinaryReader(fr);

                if (listView1.SelectedItems.Count == 0)
                {
                    for (int i = 0; i < fileHeader.iNumFiles; i++)
                    {
                        br.BaseStream.Position = fileHeader.iDataOffset[i];

                        //make sure the dir exists
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        FileStream fw = File.Create(path + "\\" + fileHeader.sFileNames[i]);
                        BinaryWriter bw = new BinaryWriter(fw);

                        bw.Write(br.ReadBytes(fileHeader.iFileSizes[i]));
                        bw.Close();
                        fw.Close();
                    }
                }
                else
                {
                    for (int i = 0; i < listView1.SelectedItems.Count; i++)
                    {
                        br.BaseStream.Position = (int)listView1.SelectedItems[i].Tag;

                        //make sure the dir exists
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        FileStream fw = null;
                        try
                        {
                            fw = File.Create(path + "\\" + listView1.SelectedItems[i].SubItems[0].Text);
                        }
                        catch (System.IO.IOException e)
                        {
                            MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        BinaryWriter bw = new BinaryWriter(fw);

                        int iBytes = Convert.ToInt32(listView1.SelectedItems[i].SubItems[1].Text);
                        bw.Write(br.ReadBytes(iBytes));
                        bw.Close();
                        fw.Close();
                    }
                }
            }
            catch (System.IO.IOException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (br != null)
                    br.Close();

                if (fr != null)
                    fr.Close();
            }
        }

        private int calculatePadding(int bytes, int multiple)
        {
            if (bytes % multiple == 0)
            {
                return 0;
            }
            else
            {
                return (multiple - (int)(bytes % multiple));
            }
        }

        private void resetEverything()
        {
            listView1.Items.Clear();
            fileHeader = new FileHeader(0);
            sCurrfile = "";
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.listView1.Sort();
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addDialog addForm = new addDialog();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                sSave       = addForm.getSave();
                sAddPath    = addForm.getPath();
                iText       = addForm.getIsText();
                qFiles      = addForm.getAdds();
                addForm.Close();

                this.createHeader();
                this.createFile();
            }
        }

        private void createHeader()
        {
            fileHeader = new FileHeader(qFiles.Count);
            fileHeader.iLanguage = iText;
            int iNameLength = 0;
            int iFileSize = 0;
            for (int i = 0; i < fileHeader.iNumFiles; i++)
            {
                //remove path from filenames
                string sName = qFiles.Dequeue().ToString();
                fileHeader.sFileNames[i] = sName.Replace(sAddPath, "");
                //find the longest file name
                if (iNameLength < fileHeader.sFileNames[i].Length)
                {
                    iNameLength = fileHeader.sFileNames[i].Length;
                }
                fileHeader.iFileNumber[i] = i;

                //gets the length of the file
                byte[] bFile = File.ReadAllBytes(sAddPath + fileHeader.sFileNames[i]);
                fileHeader.iFileSizes[i] = bFile.Length;
                iFileSize += fileHeader.iFileSizes[i] + calculatePadding(fileHeader.iFileSizes[i], 16);

                //dataoffset is previous file's offset + file Size of previous file + padding of previous file
                if (i == 0)
                {
                    fileHeader.iDataOffset[i] = 0;
                }
                else
                {
                    fileHeader.iDataOffset[i] = fileHeader.iDataOffset[i - 1] + fileHeader.iFileSizes[i - 1] + calculatePadding(fileHeader.iFileSizes[i - 1], 16);
                }
            }
            //calculte the fileNameLength field
            fileHeader.iFileNameLength = iNameLength + calculatePadding(iNameLength, 4);
            fileHeader.iDataStart = calculateHeaderSize(fileHeader);
            fileHeader.iFileSize = fileHeader.iDataStart + iFileSize;
        }

        private int calculateHeaderSize(FileHeader fh)
        {
            //there's always 32 bytes
            int iHeaderSize = 32;
            //calculate the length of one entry in the file list and it's padding
            int iFileList = fh.iFileNameLength + 12;
            iFileList += calculatePadding(iFileList, 16);
            //multiply by the total number of files
            iHeaderSize += (iFileList * fh.iNumFiles);
            return iHeaderSize;
        }

        private void createFile()
        {
            FileStream fs = null;
            BinaryWriter bw = null;
            try
            {
                fs = new FileStream(sSave, FileMode.Create);
                bw = new BinaryWriter(fs);

                //standard header
                bw.Write(fileHeader.iPacIdentifier);
                bw.Write(fileHeader.iDataStart);
                bw.Write(fileHeader.iFileSize);
                bw.Write(fileHeader.iNumFiles);
                bw.Write(fileHeader.iLanguage);
                bw.Write(fileHeader.iFileNameLength);
                bw.Write(createPadding(8));

                //file list
                for (int i = 0; i < fileHeader.iNumFiles; i++)
                {
                    bw.Write(fileHeader.sFileNames[i].ToCharArray());
                    bw.Write(createPadding(fileHeader.iFileNameLength - fileHeader.sFileNames[i].Length));
                    bw.Write(fileHeader.iFileNumber[i]);
                    bw.Write(fileHeader.iDataOffset[i]);
                    bw.Write(fileHeader.iFileSizes[i]);
                    bw.Write(createPadding(calculatePadding(fileHeader.iFileNameLength + 12, 16)));
                }

                //data
                for (int i = 0; i < fileHeader.iNumFiles; i++)
                {
                    FileStream fsr = new FileStream(sAddPath + fileHeader.sFileNames[i], FileMode.Open);
                    BinaryReader br = new BinaryReader(fsr);

                    bw.Write(br.ReadBytes(fileHeader.iFileSizes[i]));
                    br.Close();
                    fsr.Close();
                    bw.Write(createPadding(calculatePadding(fileHeader.iFileSizes[i], 16)));
                }
            }
            catch (System.IO.IOException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (bw != null)
                    bw.Close();

                if (fs != null)
                    fs.Dispose();
            }
        }

        private byte[] createPadding(int numBytes)
        {
            byte[] temp = new byte[numBytes];
            temp.Initialize();
            return temp;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsDialog sd = new SettingsDialog();
            sd.ShowDialog();
        }
    }
}
