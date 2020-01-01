using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Continuum_DeShift
{
    public partial class SettingsDialog : Form
    {
        bool bOpenChanged, bContextChanged;

        public SettingsDialog()
        {
            InitializeComponent();

            // check if the entries in the registry exist and check settings accordingly
            RegistryKey akey = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey bkey = akey.OpenSubKey("Classes");
            RegistryKey rkey = bkey.OpenSubKey(".pac");
            if (rkey != null)
            {
                cbOpenWith.Checked = true;
                string extstring = rkey.GetValue("").ToString();
                if (extstring != null)
                {
                    if (extstring.Length > 0)
                    {
                        rkey.Close();
                        rkey = bkey.OpenSubKey(extstring, false);
                        if (rkey != null)
                        {
                            string strkey = "shell\\Continuum DeShift";
                            RegistryKey subky = rkey.OpenSubKey(strkey, false);
                            if (subky != null)
                            {
                                cbEnableContext.Checked = true;
                                subky.Close();
                            }
                            rkey.Close();
                        }
                    }
                }
            }
            bkey.Close();
            akey.Close();

            bOpenChanged = false;
            bContextChanged = false;
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            if (bOpenChanged == true)
            {
                if (cbOpenWith.Checked == true)
                {
                    AddAssociateWith();
                }
                else
                {
                    RemoveAssociateWith();
                }
            }
            if (bContextChanged == true)
            {
                if (cbEnableContext.Checked == true)
                {
                    AddContextMenuItem(".pac", "Continuum DeShift",
                        "Extract Here", "\"" + Application.ExecutablePath + "\" \"extract\" \"" + "%1" + "\"");
                }
                else
                {
                    RemoveContextMenuItem(".pac", "Continuum DeShift");
                }
            }
            Close();
        }

        // This function is based off of code from Nish's Blog, http://blog.voidnish.com/?p=17
        //Extension - Extension of the file (.zip, .txt etc.)
        //MenuName - Name for the menu item (Play, Open etc.)
        //MenuDescription - The actual text that will be shown
        //MenuCommand - Path to executable
        private bool AddContextMenuItem(string Extension, string MenuName, string MenuDescription, string MenuCommand)
        {
            bool ret = false;
            RegistryKey akey = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey bkey = akey.OpenSubKey("Classes");
            RegistryKey rkey = bkey.OpenSubKey(Extension);
            if (rkey != null)
            {
                string extstring = rkey.GetValue("").ToString();
                rkey.Close();
                if (extstring != null)
                {
                    if (extstring.Length > 0)
                    {
                        rkey = bkey.OpenSubKey(extstring, true);
                        if (rkey != null)
                        {
                            string strkey = "shell\\" + MenuName + "\\command";
                            RegistryKey subky = rkey.CreateSubKey(strkey);
                            if (subky != null)
                            {
                                subky.SetValue("", MenuCommand);
                                subky.Close();
                                subky = rkey.OpenSubKey("shell\\" + MenuName, true);
                                if (subky != null)
                                {
                                    subky.SetValue("", MenuDescription);
                                    subky.Close();
                                }
                                ret = true;
                            }
                            rkey.Close();
                        }
                    }
                }
            }
            bkey.Close();
            akey.Close();
            return ret;
        }

        private bool RemoveContextMenuItem(string Extension, string MenuName)
        {
            bool ret = false;
            RegistryKey akey = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey bkey = akey.OpenSubKey("Classes");
            RegistryKey rkey = bkey.OpenSubKey(Extension);
            if (rkey != null)
            {
                string extstring = rkey.GetValue("").ToString();
                rkey.Close();
                if (extstring != null)
                {
                    if (extstring.Length > 0)
                    {
                        rkey = bkey.OpenSubKey(extstring, true);
                        if (rkey != null)
                        {
                            string strkey = "shell\\";
                            RegistryKey subky = rkey.OpenSubKey(strkey, true);
                            if (subky != null)
                            {
                                subky.DeleteSubKeyTree(MenuName);
                                subky.Close();
                                ret = true;
                            }
                            rkey.Close();
                        }
                    }
                }
            }
            bkey.Close();
            akey.Close();
            return ret;
        }

        private bool AddAssociateWith()
        {
            bool ret = false;
            RegistryKey akey = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey bkey = akey.OpenSubKey("Classes", true);
            RegistryKey subkey = bkey.CreateSubKey(".pac");
            subkey.SetValue("", "CDS.File");
            subkey.Close();

            subkey = bkey.CreateSubKey("CDS.File");
            subkey.SetValue("", "");

            RegistryKey subkey2 = subkey.CreateSubKey("DefaultIcon");
            subkey2.SetValue("", "\"" + Application.ExecutablePath + "\"");

            subkey2.Close();
            subkey2 = subkey.CreateSubKey("shell");
            subkey2.SetValue("", "");

            RegistryKey subkey3 = subkey2.CreateSubKey("open");
            subkey3.SetValue("", "");

            RegistryKey subkey4 = subkey3.CreateSubKey("command");
            subkey4.SetValue("", "\"" + Application.ExecutablePath + "\" \"" + "%1" + "\"");

            subkey4.Close();
            subkey3.Close();
            subkey2.Close();
            subkey.Close();
            bkey.Close();
            akey.Close();
            return ret;
        }

        private bool RemoveAssociateWith()
        {
            bool ret = false;
            RegistryKey akey = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey bkey = akey.OpenSubKey("Classes", true);
            RegistryKey rkey = bkey.OpenSubKey(".pac");
            if (rkey != null)
            {
                rkey.Close();
                bkey.DeleteSubKeyTree(".pac");
                bkey.DeleteSubKeyTree("CDS.File");
                ret = true;
            }
            bkey.Close();
            akey.Close();
            return ret;
        }

        private void cbEnableContext_CheckedChanged(object sender, EventArgs e)
        {
            bContextChanged = !bContextChanged;
        }

        private void cbOpenWith_CheckedChanged(object sender, EventArgs e)
        {
            bOpenChanged = !bOpenChanged;
            if (cbOpenWith.Checked == true)
            {
                cbEnableContext.Enabled = true;
            }
            else
            {
                cbEnableContext.Enabled = false;
            }

        }
    }
}
