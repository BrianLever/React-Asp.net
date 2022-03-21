using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

namespace FrontDeskKioskUILocker
{
    public partial class MainForm : Form
    {
        private const string AUTO_ADMIN_LOGIN_KEY = "AutoAdminLogon";
        private const string AUTO_LOGIN_USERNAME_KEY = "DefaultUserName";
        private const string AUTO_LOGIN_PWD_KEY = "DefaultPassword";
        private const string AUTO_LOGIN_DOMAIN_KEY = "DefaultDomainName";
        private const string SHELL_KEY = "Shell";

        private const string SHELL_DEFAULT_VALUE = "Explorer.exe";
        private readonly string SHELL_CUSTOM_VALUE;//C:\\Program Files\\ScreenDox\\Launcher\ScreenDoxKioskLauncher.exe";

        RegistryKey lm_winlogon;

        RegistryKey cu_winlogon;




        public MainForm()
        {
            InitializeComponent();
            SHELL_CUSTOM_VALUE = Path.Combine(
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                "Launcher",
                "ScreenDoxKioskLauncher.exe");

            this.Load += new EventHandler(MainForm_Load);


        }

        void MainForm_Load(object sender, EventArgs e)
        {
            BindUI();
        }

        private void BindUI()
        {
            try
            {
                lm_winlogon = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", true);
                cu_winlogon = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", true);
            }
            catch (System.Security.SecurityException ex)
            {
                Trace.WriteLine(ex.Message);
                MessageBox.Show("You must have administrative permissions.", "Access denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                btnOk.Enabled = false;

                this.Close();
                return;

            }
            if (lm_winlogon == null)
            {
                MessageBox.Show("You must have administrative permissions.", "Access denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                btnOk.Enabled = false;

                this.Close();
                return;
            }

            string[] lm_keys = lm_winlogon.GetValueNames();
            string[] cu_keys = cu_winlogon.GetValueNames();

            #region set auto-logon current settings

            if (lm_keys.Contains(AUTO_ADMIN_LOGIN_KEY))
            {
                rbEnableAutoLogin.Checked =
                    Convert.ToInt32(lm_winlogon.GetValue(AUTO_ADMIN_LOGIN_KEY)) == 1;
            }
            rbDisableAutoLogin.Checked = !rbEnableAutoLogin.Checked;

            if (lm_keys.Contains(AUTO_LOGIN_USERNAME_KEY))
            {
                txtUserName.Text = lm_winlogon.GetValue(AUTO_LOGIN_USERNAME_KEY).ToString();
                if (lm_keys.Contains(AUTO_LOGIN_DOMAIN_KEY))
                {
                    txtUserName.Text = lm_winlogon.GetValue(AUTO_LOGIN_DOMAIN_KEY).ToString() + "\\" +
                        txtUserName.Text;
                }
            }

            if (lm_keys.Contains(AUTO_LOGIN_PWD_KEY))
            {
                txtPassword.Text = lm_winlogon.GetValue(AUTO_LOGIN_PWD_KEY).ToString();
                txtConfirmPassword.Text = txtPassword.Text;
            }
            #endregion

            #region current shell

            rbStansard.Checked = true;

            if (lm_keys.Contains(SHELL_KEY) && lm_winlogon.GetValue(SHELL_KEY).ToString() == SHELL_DEFAULT_VALUE)
            {
                rbStansard.Checked = true;
            }
            else if (lm_keys.Contains(SHELL_KEY) && lm_winlogon.GetValue(SHELL_KEY).ToString() == SHELL_CUSTOM_VALUE)
            {
                rbForAll.Checked = true;
            }
            else if (cu_keys.Contains(SHELL_KEY) && cu_winlogon.GetValue(SHELL_KEY).ToString() == SHELL_CUSTOM_VALUE)
            {
                rbForCurrentOnly.Checked = true;
            }

            #endregion


            BindHotkeysStatus();

        }

        private void BindHotkeysStatus()
        {
            chkCtrlAltDel.Checked = HotKeyManager.GetStateCtrlAltDelete();
            
        }

        private void SetHotkeyStatus()
        {
            HotKeyManager.SetStateCtrlAltDelete(chkCtrlAltDel.Checked);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            MakeChanges();
        }




        private void MakeChanges()
        {
            if (rbEnableAutoLogin.Checked)
            {
                if (IsValid)
                {

                    string enteredName = txtUserName.Text.Trim();
                    string userName = "", domainName = "";
                    if (enteredName.Contains("\\"))
                    {
                        //user name with domain name
                        int separatorIndex = enteredName.IndexOf('\\');
                        domainName = enteredName.Substring(0, separatorIndex);
                        userName = enteredName.Substring(separatorIndex + 1, enteredName.Length - (separatorIndex + 1));
                    }
                    else
                    {
                        userName = txtUserName.Text.Trim();
                    }



                    lm_winlogon.SetValue(AUTO_ADMIN_LOGIN_KEY, "1");
                    lm_winlogon.SetValue(AUTO_LOGIN_USERNAME_KEY, userName);
                    if (domainName != "")
                    {
                        lm_winlogon.SetValue(AUTO_LOGIN_DOMAIN_KEY, domainName);
                    }
                    else
                    {
                        lm_winlogon.SetValue(AUTO_LOGIN_DOMAIN_KEY, "");
                    }
                    lm_winlogon.SetValue(AUTO_LOGIN_PWD_KEY, txtPassword.Text);
                }
                else
                {
                    return;
                }
            }
            else
            {
                lm_winlogon.SetValue(AUTO_ADMIN_LOGIN_KEY, 0);
            }


            if (rbStansard.Checked)
            {
                lm_winlogon.SetValue(SHELL_KEY, SHELL_DEFAULT_VALUE);
                cu_winlogon.SetValue(SHELL_KEY, "");
            }
            else if (rbForCurrentOnly.Checked)
            {
                cu_winlogon.SetValue(SHELL_KEY, SHELL_CUSTOM_VALUE); 
                lm_winlogon.SetValue(SHELL_KEY, SHELL_DEFAULT_VALUE);
            }
            else if (rbForAll.Checked)
            {
                lm_winlogon.SetValue(SHELL_KEY, SHELL_CUSTOM_VALUE);
                cu_winlogon.SetValue(SHELL_KEY, "");
            }

            lm_winlogon.Close();
            cu_winlogon.Close();


            SetHotkeyStatus();

            Close();
        }

        private bool IsValid
        {
            get
            {
                if (String.IsNullOrEmpty(txtUserName.Text.Trim()))
                {
                    txtUserName.Focus();
                    MessageBox.Show("User name is required");
                    return false;
                }
                else if (String.IsNullOrEmpty(txtPassword.Text))
                {
                    MessageBox.Show("Password is required");
                    return false;
                }
                else if (String.IsNullOrEmpty(txtConfirmPassword.Text))
                {
                    MessageBox.Show("Password confirmation is required");
                    return false;
                }
                else if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    MessageBox.Show("Password and password confirmation must match");
                    return false;
                }

                return true;
            }
        }

        private void rbEnableAutoLogin_CheckedChanged(object sender, EventArgs e)
        {
            pnlLogin.Enabled = (sender as RadioButton).Checked;
        }


    }
}
