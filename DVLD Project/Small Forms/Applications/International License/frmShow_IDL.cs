using System;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms.Applications
{
    public partial class frm_I_LicenseInfo : Form
    {
        int I_LicenseID;

        public frm_I_LicenseInfo(int i_LicenseID)
        {
            InitializeComponent();

            I_LicenseID = i_LicenseID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void frmShow_IDL_Load(object sender, EventArgs e)
        {
            ctrInternationalLicenseInfo1.LoadDataToForm(I_LicenseID);
        }
    }
}
