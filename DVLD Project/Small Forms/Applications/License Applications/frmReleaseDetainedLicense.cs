using BusinessLayer;
using BusinessLayer.Applications;
using BusinessLayer.Drivers___Licenses;
using BusinessLayer.Licenses;
using DVLD_Project.Global;
using DVLD_Project.Small_Forms.Applications.Local_Driving_License_Applications;
using System;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms.Applications.License_Applications
{
    public partial class frmReleaseDetainedLicense : Form
    {
        clsLicenses License = null;
        clsDetainedLicenses DetainInfo = null;

        public bool DoUpdateDataGridView { get; set; } = false; //for frmManageDetainedLicenses datagrid

        public frmReleaseDetainedLicense()
        {
            InitializeComponent();
        }
        public frmReleaseDetainedLicense(int licenseID)
        {
            InitializeComponent();

            License = clsLicenses.FindByLicenseID(licenseID);
            DetainInfo = clsDetainedLicenses.Find(licenseID);
        }

        private void frmReleaseDetainedLicense_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;

            if (License != null) // in case of the use of the second constructor
            {
                ctrFindLicense1.LoadInfo(License.LicenseID);
                FillFormInfo();

                btnRelease.Enabled = true;
                llblShowNewLiecenseInfo.Enabled = true;
            }
        }

        private void CheckValedity()
        {
            btnRelease.Enabled = false;


            if (!clsDetainedLicenses.IsLicenseDetained(License.LicenseID))
            {
                MessageBox.Show("This license is not detained",
                               "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            btnRelease.Enabled = true;
        }
        private void ResetForm()
        {
            btnRelease.Enabled = false;
            llblShowNewLiecenseInfo.Enabled = false;
            llblShowLicensesHistory.Enabled = false;

            lblDetainID.Text = "[???]";
            lblApplicationFees.Text = "[???]";
            lblDetainDate.Text = "[???]";
            lblFineFees.Text = "[???]";
            lblReleaseApplicationID.Text = "[???]";
            lblTotalFees.Text = "[???]";

        }
        private void FillFormInfo()
        {
            lblDetainID.Text = DetainInfo.DetainID.ToString();
            lblDetainDate.Text = DetainInfo.DetainDate.ToShortDateString();

            float applicationFees = clsApplicationTypes.Find(5).Fee;
            lblApplicationFees.Text = applicationFees.ToString();
            lblFineFees.Text = DetainInfo.FineFees.ToString();

            lblTotalFees.Text = (applicationFees + DetainInfo.FineFees).ToString();
        }
        private void ctrFindLicense1_OnSearch(clsLicenses license)
        {
            License = license;

            if (License != null)
            {
                CheckValedity();

                DetainInfo = clsDetainedLicenses.Find(License.LicenseID);

                if (DetainInfo != null) FillFormInfo();

                llblShowLicensesHistory.Enabled = true;
            }
            else
            {
                ResetForm();
            }
        }

        private void DeleteApplicationAfterFail()
        {
            int applicationID = DetainInfo.ReleaseApplication.ApplicationID;

            if (!clsApplication.Delete(applicationID))
            {
                MessageBox.Show($"Failed to delete the application, delete it manually by PersonID {applicationID} ", "Error",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool AddNewApplication()
        {
            clsApplication application = new clsApplication();
            clsApplicationTypes applicationType = clsApplicationTypes.Find(5); // 5 for Release Detained License

            application.ApplicantPerson = License.Driver.Person;
            application.ApplicationType = applicationType;
            application.CreatedByUser = clsGlobal.CurrentUser;
            application.Date = DateTime.Now;
            application.LastStatusDate = DateTime.Now;
            application.PaidFees = applicationType.Fee;
            application.Status = clsApplication.enStatus.Completed;

            if (!application.Save())
            {
                MessageBox.Show("Failed to save the application for the new license",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }


            DetainInfo.ReleaseApplication = application;


            return true;
        }
        private bool ActivateLicense()
        {
            if (!License.Activate())
            {
                MessageBox.Show($"Failed to activate the license, activate it manually by" +
                    $" PersonID = {License.LicenseID}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            return true;
        }
        private bool IsSure()
        {
            DialogResult result = MessageBox.Show("Are you sure you want to Release this license?",
                                                  "Release License", MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Information);

            return (result == DialogResult.Yes);
        }
        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (!IsSure())
            {
                return;
            }

            if (!AddNewApplication())
            {
                return;
            }

            DetainInfo.IsReleased = true;
            DetainInfo.ReleaseDate = DateTime.Now;
            DetainInfo.ReleasedByUser = clsGlobal.CurrentUser;

            if (DetainInfo.Save())
            {
                ActivateLicense();

                MessageBox.Show("License Released Successfully", "Relaese License",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                lblReleaseApplicationID.Text = DetainInfo.ReleaseApplication.ApplicationID.ToString();

                btnRelease.Enabled = false;
                llblShowNewLiecenseInfo.Enabled = true;

                DoUpdateDataGridView = true;
            }
            else
            {
                MessageBox.Show("Failed To Release The License", "Release License",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);


                DeleteApplicationAfterFail();
            }
        }

        private void llblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonsLicenseHistory frm = new frmPersonsLicenseHistory(License.Driver.Person.NationalNO);
            frm.ShowDialog();
        }
        private void llblShowNewLiecenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo(License.Application.ApplicationID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
