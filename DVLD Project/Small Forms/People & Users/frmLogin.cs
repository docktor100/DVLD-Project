using BusinessLayer;
using DVLD_Project.Global;
using System;
using System.IO;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms
{
    public partial class frmLogin : Form
    {
        public bool IsSignIn = false;

        string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LastLoginInfo.txt");

        public frmLogin()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            string Content = File.ReadAllText(filepath);
            if (!string.IsNullOrWhiteSpace(Content))
            {
                string[] arrContent = Content.Split(new[] { "#//#" }, StringSplitOptions.None);
                tbUserName.Text = arrContent[0].Trim();
                mbPassowrd.Text = arrContent[1].Trim();
                checkBox1.Checked = true;

                btnLogin.Focus();
            }
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
                this.DialogResult = DialogResult.OK;
                IsSignIn = true;

                this.Close();
            }
        }

        private void RememberLastUser(bool IsRememberMe)
        {
            if (!IsRememberMe)
            {
                if (File.Exists(filepath))
                {
                    File.WriteAllText(filepath, "");
                }
                return;
            }


            string[] Content = { tbUserName.Text, mbPassowrd.Text };
            string UserName_Passoword = string.Join("#//#", Content);

            if (File.Exists(filepath))
            {
                File.WriteAllText(filepath, UserName_Passoword);
            }
        }

    }
}
