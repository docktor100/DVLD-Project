using System;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms
{
    public partial class frmBasicUserCard : Form
    {
        int UserID;

        public frmBasicUserCard(int userID)
        {
            InitializeComponent();
            UserID = userID;
        }
        private void ctrBaseUserCard1_Load(object sender, EventArgs e)
        {
            ctrBaseUserCard1.LoadUserData(UserID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
