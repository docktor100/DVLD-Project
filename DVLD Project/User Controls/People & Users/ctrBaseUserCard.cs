using BusinessLayer;
using System.Windows.Forms;

namespace DVLD_Project.User_Controls
{
    public partial class ctrBaseUserCard : UserControl
    {
        private clsUsers User;

        public ctrBaseUserCard()
        {
            InitializeComponent();
        }

        private void ResetForm()
        {
            ctrBasePersonCard1.ResetForm();
            lblUserID.Text = "[???]";
            lblUserName.Text = "[???]";
            lblIsActive.Text = "[???]";
        }

        public void LoadUserData(int userID)
        {
            User = clsUsers.Find(userID);

            if (User != null)
            {
                ctrBasePersonCard1.LoadPersonInfo(User.Person);
                lblUserID.Text = User.UserID.ToString();
                lblUserName.Text = User.UserName;
                lblIsActive.Text = (User.IsActive) ? "Yes" : "No";
            }
            else
            {
                ResetForm();
            }
        }

    }
}
