using DVLD_Project.Global;
using DVLD_Project.Small_Forms;
using DVLD_Project.Small_Forms.Applications;
using DVLD_Project.Small_Forms.Applications.International_License;
using DVLD_Project.Small_Forms.Applications.License_Applications;
using DVLD_Project.Small_Forms.Applications.Local_Driving_License_Applications;
using DVLD_Project.Small_Forms.People___Users;
using System;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmMainForm : Form
    {
        public bool IsSignOut = false;

        public frmMainForm()
        {
            InitializeComponent();
        }

        private void peopleListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPeopleList frm = new frmPeopleList();
            frm.ShowDialog();
        }
        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUsersList frm = new frmUsersList();
            frm.ShowDialog();
        }

        private void currentInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBasicUserCard frm = new frmBasicUserCard(clsGlobal.CurrentUser.UserID);
            frm.ShowDialog();
        }
        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangeUserPassword frm = new frmChangeUserPassword(clsGlobal.CurrentUser.UserID);
            frm.ShowDialog();
        }
        private void signgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsGlobal.CurrentUser = null;
            IsSignOut = true;
            this.Close();
        }


        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageApplicationTypes frm = new frmManageApplicationTypes();
            frm.ShowDialog();
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frmManageTestTypes frm = new frmManageTestTypes();
            frm.ShowDialog();
        }




        private void NewlocalDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddNew_LDLA frm = new frmAddNew_LDLA();
            frm.ShowDialog();
        }
        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRenew_LDL frm = new frmRenew_LDL();
            frm.ShowDialog();
        }
        private void replacementForLostOrDamagedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReplaceLicense frm = new frmReplaceLicense();
            frm.ShowDialog();
        }

        private void ManagelocalDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_LDLA_List frm = new frm_LDLA_List();
            frm.ShowDialog();
        }
        private void internToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_Int_DL_List frm = new frm_Int_DL_List();
            frm.ShowDialog();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDriversList frm = new frmDriversList();
            frm.ShowDialog();
        }

        private void globalDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmIssueInternationalLicense frm = new frmIssueInternationalLicense();
            frm.ShowDialog();
        }


        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense();
            frm.ShowDialog();
        }
        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense();
            frm.ShowDialog();
        }
        private void mangaeDetainedLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageDetainedLicenses frm = new frmManageDetainedLicenses();
            frm.ShowDialog();
        }
    }
}
