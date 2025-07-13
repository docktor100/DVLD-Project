using BusinessLayer;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms
{
    public partial class frmChangeUserPassword : Form
    {
        private int UserID;
        private clsUsers User;

        public frmChangeUserPassword(int userID)
        {
            InitializeComponent();
            UserID = userID;
        }
        private void frmChangeUserPassword_Load(object sender, System.EventArgs e)
        {
            User = clsUsers.Find(UserID);
            ctrBaseUserCard1.LoadUserData(UserID);
            mbCurrentPassword.Focus();
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            if (mbCurrentPassword.Text != User.Password)
            {
                errorProvider1.SetError(mbCurrentPassword, "Wrong Password, Try Again");
                return;
            }
            else
            {
                errorProvider1.SetError(mbCurrentPassword, "");
            }

            if (mbNewPassword.Text != mbConfirmPassword.Text)
            {
                errorProvider1.SetError(mbConfirmPassword, "Password Doesn't Match!");
                return;
            }
            else
            {
                errorProvider1.SetError(mbConfirmPassword, "");
            }

            User.Password = mbNewPassword.Text;

            if (User.Save())
            {
                MessageBox.Show("Password Changed Successfully", "Change Password");
            }
            else
            {
                MessageBox.Show("Password Changed Successfully", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
