using BusinessLayer;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms
{
    public partial class frm_UpdateAddNewUser : Form
    {
        clsUsers User { get; set; }

        public bool DoUpdateGridView = false;

        enum enMode { AddNew, Update } //##
        enMode Mode;

        public frm_UpdateAddNewUser()
        {
            InitializeComponent();
            User = new clsUsers();
            Mode = enMode.AddNew;
        }
        public frm_UpdateAddNewUser(int userID)
        {
            InitializeComponent();
            User = clsUsers.Find(userID);
            Mode = enMode.Update;
        }


        private void frmAddNewUser_Load(object sender, System.EventArgs e)
        {
            if (Mode == enMode.AddNew)
            {
                lbl_Update_Addnew_Subject.Text = "Add New User";
                this.Text = "Add New User";
            }
            else
            {
                this.Text = "Update User";
                lbl_Update_Addnew_Subject.Text = "Update User";

                LoadDataToForm();
            }

            ctrFindPerson1.FilterFocus();


        }
        private void LoadDataToForm()
        {
            if (User == null)
            {
                MessageBox.Show("No User with ID = " + User.UserID, "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();

                return;
            }

            ctrFindPerson1.LoadPersonInfo(User.Person.PersonID);
            lblUserID.Text = User.UserID.ToString();

            tbUserName.Text = User.UserName;
            mbPassword.Text = User.Password;
            mbConfirmPassword.Text = User.Password;
            cbIsActive.Checked = User.IsActive;
        }
        public void FillUserData()
        {
            User.Person = ctrFindPerson1.SelectedPersonInfo;
            User.UserName = tbUserName.Text;
            User.Password = mbPassword.Text;
            User.IsActive = cbIsActive.Checked;
        }
        private void ResetForm()
        {
            ctrFindPerson1.ResetForm();

            lblUserID.Text = "[???]";

            tbUserName.Text = "";
            mbPassword.Text = "";
            mbConfirmPassword.Text = "";
            cbIsActive.Checked = false;

        }


        private void btnNext_Click(object sender, System.EventArgs e)
        {
            //if (IsValidToLoginInfo()) //this function is raised in the line 179
            tabControl1.SelectedIndex = 1;
        }
        private void btnPrev_Click(object sender, System.EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            if (IsThereErrors())
            {
                MessageBox.Show("Check Your Input", "Error");
                return;
            }
            FillUserData();

            if (!User.Save())
            {
                MessageBox.Show("Failed To Save Data", "ERROR");
            }
            else
            {
                MessageBox.Show("Save Succeeded");
                lblUserID.Text = User.UserID.ToString();

                lbl_Update_Addnew_Subject.Text = "Update User";
                Mode = enMode.Update;

                DoUpdateGridView = true;
            }
        }
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private bool IsThereErrors()
        {
            foreach (Control c in tcUserInfo.Controls)
            {
                if (!string.IsNullOrEmpty(errorProvider1.GetError(c)))
                {
                    return true; // At least one error is set
                }
            }
            return false;
        }
        private bool IsValidToLoginInfo()
        {
            if (ctrFindPerson1.SelectedPersonInfo == null)
            {
                MessageBox.Show("Select a person first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            else if (clsUsers.IsExist(ctrFindPerson1.PersonID) && Mode != enMode.Update)
            {
                MessageBox.Show("This person is already a user, Choose another person", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }

            else
                return true;

        }


        private void tbUserName_TextChanged(object sender, System.EventArgs e)
        {
            if (tbUserName.Text != User.UserName && clsUsers.IsExist(tbUserName.Text))
            {
                errorProvider1.SetError(tbUserName, "User With This User Name Already Exists!");
            }
            else
            {
                errorProvider1.SetError(tbUserName, "");
            }
        }
        private void mbConfirmPassword_Leave(object sender, System.EventArgs e)
        {
            if (mbConfirmPassword.Text != mbPassword.Text)
            {
                errorProvider1.SetError(mbConfirmPassword, "Password Does Not Match!");
            }
            else
            {
                errorProvider1.SetError(mbConfirmPassword, "");
            }
        }


        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tcUserInfo && !IsValidToLoginInfo())
            {
                e.Cancel = true;
                MessageBox.Show("Choose a person first", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ctrFindPerson1_OnPersonSelected(int obj)
        {
            if (clsUsers.IsExist(obj))
            {
                MessageBox.Show("This person is already a user", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
    }
}

