using BusinessLayer.Licenses;
using System;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms.Applications.Local_Driving_License_Applications
{
    public partial class frmLicenseInfo : Form
    {
        private clsLicenses License;

        public frmLicenseInfo(int applicationID)
        {
            InitializeComponent();
            License = clsLicenses.FindByAppID(applicationID);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLicenseInfo_Load(object sender, EventArgs e)
        {
            ctrLicenseInfo1.LoadDataToForm(License);
        }
    }
}
