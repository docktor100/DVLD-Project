using BusinessLayer;
using BusinessLayer.Applications;
using BusinessLayer.Drivers___Licenses;
using BusinessLayer.Licenses;
using DVLD_Project.Global;
using DVLD_Project.Small_Forms.Applications.Local_Driving_License_Applications;
using System;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms.Applications
{
    public partial class frmIssueInternationalLicense : Form
    {
        clsInternationalLicense I_License;
        public bool DidAdd { get; set; } = false; // For updating datagridview in the list form


        public frmIssueInternationalLicense()
        {
            InitializeComponent();
        }

        private void frmIssueInternationalLicense_Load(object sender, EventArgs e)
        {

        }


        private void CheckIfHas_I_License(clsLicenses License)
        {
            llblShowLicenseHistory.Enabled = true;

            if (clsInternationalLicense.IsExist_Active(License.LicenseID))
            {
                I_License = clsInternationalLicense.FindBy_LDL_ID(License.LicenseID);

                MessageBox.Show("This Local License already has an international License", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                lbl_IL_ApplicationID.Text = I_License.Application.ApplicationID.ToString();
                lbl_IL_LicenseID.Text = I_License.I_LicenseID.ToString();

                llblShowLicenseInfo.Enabled = true;
                btnIssue.Enabled = false;

            }
            else
            {
                lbl_IL_ApplicationID.Text = "[???]";
                lbl_IL_LicenseID.Text = "[???]";

                llblShowLicenseInfo.Enabled = false;
                btnIssue.Enabled = true;
            }
        }
        private bool IsValid()
        {
            clsLicenses L_License = I_License.IssuedUsing_LDL;

            if (!L_License.IsActive)
            {
                MessageBox.Show("Cannot issue an international license from inactive Local License",
                                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return false;
            }

            if (L_License.LicenseClass.LicenseClassID < 3)
            {
                MessageBox.Show("Cannot issue an international license from a low-tier License Class",
                                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return false;
            }

            if (DateTime.Compare(L_License.ExpirationDate, DateTime.Now) < 0)
            {
                MessageBox.Show("Cannot issue an international license from an expired License",
                           "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return false;
            }


            return true;
        }
        private void ctrFindLicense1_OnSearch(clsLicenses License)
        {

            if (License == null)
            {
                ResetForm();
                return;
            }

            CheckIfHas_I_License(License);


            lblFees.Text = clsApplicationTypes.Find(6).Fee.ToString(); //6 for new international license
            lblLocalLicenseID.Text = License.LicenseID.ToString();
            lblCreatedBy.Text = License.CreatedByUser.UserName;
            lblApplicationDate.Text = License.Application.Date.ToShortDateString();
            lblIssueDate.Text = DateTime.Now.ToShortDateString();
            lblExpirationDate.Text = DateTime.Now.AddYears(1).ToShortDateString();

            FillInternationalLicenseInfo();

            if (!IsValid())
            {
                btnIssue.Enabled = false;
            }
        }

        private void ResetForm()
        {
            lblFees.Text = "[???]";
            lblLocalLicenseID.Text = "[???]";
            lblCreatedBy.Text = "[???]";
            lblApplicationDate.Text = "[???]";
            lblIssueDate.Text = "[???]";
            lblExpirationDate.Text = "[???]";
            lbl_IL_LicenseID.Text = "[???]";
            lbl_IL_ApplicationID.Text = "[???]";

            llblShowLicenseHistory.Enabled = false;
            llblShowLicenseInfo.Enabled = false;
            btnIssue.Enabled = false;
        }
        private void FillInternationalLicenseInfo()
        {
            if (I_License == null)
            {
                I_License = new clsInternationalLicense();
            }

            I_License.IssuedUsing_LDL = ctrFindLicense1.License;
            I_License.CreatedByUser = clsGlobal.CurrentUser;
            I_License.ExpirationDate = DateTime.Now.AddYears(1);
            I_License.IssueDate = DateTime.Now;

            int driverID = I_License.IssuedUsing_LDL.Driver.DriverID;
            I_License.Driver = clsDrivers.FindByDriverID(driverID);
            I_License.IsActive = true;

            //I_License.Application = this is filled internally when saving

        }
        private void btnIssue_Click(object sender, System.EventArgs e)
        {
            if (I_License.Save())
            {
                MessageBox.Show($"License Saved Successfully with PersonID = {I_License.I_LicenseID}",
                                "Save", MessageBoxButtons.OK);

                lbl_IL_LicenseID.Text = I_License.I_LicenseID.ToString();
                lbl_IL_ApplicationID.Text = I_License.Application.ApplicationID.ToString();

                llblShowLicenseInfo.Enabled = true;
                DidAdd = true;
            }
            else
            {
                MessageBox.Show($"Failed to save data", "Save", MessageBoxButtons.OK, MessageBoxIcon.Error);

                int applicationID = I_License.Application.ApplicationID;

                if (!clsApplication.Delete(applicationID))
                {
                    MessageBox.Show($"Failed to delete the application of the process, with PersonID = {applicationID}",
                               "Save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                llblShowLicenseInfo.Enabled = false;
            }
        }

        private void llblShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonsLicenseHistory frm = new frmPersonsLicenseHistory(I_License.Driver.Person.NationalNO);
            frm.ShowDialog();
        }
        private void llblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frm_I_LicenseInfo frm = new frm_I_LicenseInfo(I_License.I_LicenseID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
