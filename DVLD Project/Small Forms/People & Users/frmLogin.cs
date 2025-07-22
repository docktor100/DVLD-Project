using BusinessLayer;
using DVLD_Project.Global;
using Microsoft.Win32;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows.Forms;


namespace DVLD_Project.Small_Forms
{
    public partial class frmLogin : Form
    {
        public bool IsSignIn = false;

        //string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LastLoginInfo.txt"); the old method

        public frmLogin()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (!GetLogInInfoFromWinowsRegistry(out string userName, out string password))
            {
                tbUserName.Focus();
                return;
            }
;

            tbUserName.Text = userName;
            mbPassowrd.Text = password;

            checkBox1.Checked = true;
            btnLogin.Focus();
        }


        private bool AreBoxesEmpty()
        {
            bool isempty = false;

            if (string.IsNullOrWhiteSpace(tbUserName.Text))
            {
                errorProvider1.SetError(tbUserName, "Should not be left empty");
                isempty = true;
            }
            else
            {
                errorProvider1.SetError(tbUserName, "");
            }

            if (string.IsNullOrWhiteSpace(mbPassowrd.Text))
            {
                errorProvider1.SetError(mbPassowrd, "Should not be left empty");
                isempty = true;
            }
            else
            {
                errorProvider1.SetError(mbPassowrd, "");
            }

            return isempty;
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (AreBoxesEmpty())
            {
                return;
            }


            clsGlobal.CurrentUser = clsUsers.Find(tbUserName.Text);

            if (clsGlobal.CurrentUser == null)
            {
                MessageBox.Show("Invalid Username/Password.", "Wrong Credintials",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (!clsGlobal.CurrentUser.IsActive)
            {
                MessageBox.Show("This user is inactive, please contact your admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (mbPassowrd.Text == clsGlobal.CurrentUser.Password)
            {
                RememberLastUser(checkBox1.Checked);
                IsSignIn = true;

                this.Close();
            }
        }

        private void RememberLastUser(bool IsRememberMe)
        {
            if (!IsRememberMe)
            {
                if (!DeleteLogInInfoFromWinodwsRegistry())
                {
                    MessageBox.Show("Failed to delete last login info", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            if (!WriteLogInInfoToWindowsRegistry(tbUserName.Text.Trim(), mbPassowrd.Text.Trim()))
            {
                MessageBox.Show("Failed to save login info", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool DeleteLogInInfoFromWinodwsRegistry()
        {
            string KeyName = ConfigurationManager.AppSettings["LogInInfo_RegistryKeyName"];
            string ValueName = ConfigurationManager.AppSettings["LogInInfo_RegistryValueName"];

            try
            {
                Registry.CurrentUser.DeleteSubKey(KeyName.Substring("HKEY_CURRENT_USER/".Length));

                return true;
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
                return false;
            }
        }
        private bool WriteLogInInfoToWindowsRegistry(string UserName, string Password)
        {
            string KeyName = ConfigurationManager.AppSettings["LogInInfo_RegistryKeyName"];
            string ValueName = ConfigurationManager.AppSettings["LogInInfo_RegistryValueName"];


            try
            {
                string Value = UserName + "#//#" + Password;

                Registry.SetValue(KeyName, ValueName, Value, RegistryValueKind.String);

                return true;
            }
            catch (Exception ex)
            {
                LogError(ex.Message);

                return false;
            }

        }
        private bool GetLogInInfoFromWinowsRegistry(out string UserName, out string Password)
        {
            UserName = null;
            Password = null;

            string KeyName = ConfigurationManager.AppSettings["LogInInfo_RegistryKeyName"];
            string ValueName = ConfigurationManager.AppSettings["LogInInfo_RegistryValueName"];

            try
            {
                string Value = Registry.GetValue(KeyName, ValueName, null) as string;

                if (Value == null)
                {
                    return false;
                }

                string[] arr = Value.Split(new string[] { "#//#" }, StringSplitOptions.None);

                UserName = arr[0];
                Password = arr[1];
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to get login info", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                LogError(ex.Message);

                return false;
            }
        }


        private void LogError(string Message, string SourceName = "DVLD_Project")
        {
            if (string.IsNullOrEmpty(Message))
                return;

            try
            {
                if (EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, "Application");
                }

                // Write the error message to the event log
                EventLog.WriteEntry(SourceName, Message, EventLogEntryType.Error);
            }
            catch
            {

            }
        }
    }
}
