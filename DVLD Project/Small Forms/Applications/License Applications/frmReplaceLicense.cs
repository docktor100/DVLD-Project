using BusinessLayer;
using BusinessLayer.Applications;
using BusinessLayer.Licenses;
using DVLD_Project.Global;
using DVLD_Project.Small_Forms.Applications.Local_Driving_License_Applications;
using System;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms.Applications.License_Applications
{
    public partial class frmReplaceLicense : Form
    {
        clsLicenses OldLicense;
        clsLicenses NewLicense;
        clsApplicationTypes ApplicationType; //for either lost or damaged

        public frmReplaceLicense()
        {
            InitializeComponent();
        }

        private void frmReplaceLicense_Load(object sender, EventArgs e)
        {
            NewLicense = new clsLicenses();
            rbLostLicense_CheckedChanged(rbLostLicense, EventArgs.Empty);

            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();

        }

        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            int ApplicationTypeID = (rbLostLicense.Checked) ? 3 : 4; // 3 for lost, 4 for damaged
            ApplicationType = clsApplicationTypes.Find(ApplicationTypeID);

            lblFees.Text = ApplicationType.Fee.ToString();
        }

        private void CheckIfValid()
        {
            btnIssue.Enabled = false;

            if (!OldLicense.IsActive)
            {
                MessageBox.Show("This license is already inactive, can not issue a replacement for it",
                                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            if (DateTime.Compare(OldLicense.ExpirationDate, DateTime.Now) < 0)
            {
                MessageBox.Show("This license is expired, can not issue a replacement for it",
                           "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            //if (OldLicense.IsDetained())
            //{
            //    MessageBox.Show("This license Detained, can not issue a replacement for it",
            //              "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //    return;
            //} this condition is checked within the first condition

            btnIssue.Enabled = true;
        }
        private void ResetForm()
        {
            lblOldLicenseID.Text = "[???]";
            lblReplacedLicenseID.Text = "[???]";
            lbl_LR_ApplicationID.Text = "[???]";

            llblShowLicenseHistory.Enabled = false;
            llblShowLicenseInfo.Enabled = false;
            btnIssue.Enabled = false;
        }
        private void ctrFindLicense1_OnSearch(clsLicenses license)
        {
            OldLicense = license;

            if (OldLicense != null)
            {
                CheckIfValid();

                lblOldLicenseID.Text = OldLicense.LicenseID.ToString();
                llblShowLicenseHistory.Enabled = true;
            }
            else
            {
                ResetForm();
            }
        }


        private void DeleteApplicationAfterFail()
        {
            int applicationID = NewLicense.Application.ApplicationID;

            if (!clsApplication.Delete(applicationID))
            {
                MessageBox.Show($"Failed to delete the application, delete it manually by PersonID {applicationID} ", "Error",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FillNewLicenseInfo()
        {
            clsLicenseClass licenseClass = OldLicense.LicenseClass;

            NewLicense.CreatedByUser = clsGlobal.CurrentUser;
            NewLicense.Driver = OldLicense.Driver;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = OldLicense.ExpirationDate;
            NewLicense.LicenseClass = licenseClass;
            NewLicense.Paidfees = licenseClass.ClassFees;
            NewLicense.Notes = OldLicense.Notes;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = (rbLostLicense.Checked) ? clsLicenses.enIssueReason.ReplacementForLost :
                                                              clsLicenses.enIssueReason.ReplacementForDamaged;
        }
        private bool AddNewApplication()
        {
            clsApplication application = new clsApplication();

            application.ApplicantPerson = OldLicense.Driver.Person;
            application.ApplicationType = ApplicationType;
            application.CreatedByUser = clsGlobal.CurrentUser;
            application.Date = DateTime.Now;
            application.LastStatusDate = DateTime.Now;
            application.PaidFees = ApplicationType.Fee;
            application.Status = clsApplication.enStatus.Completed;

            if (!application.Save())
            {
                MessageBox.Show("Failed to save the application for the new license",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }


            NewLicense.Application = application;

            return true;
        }
        private void DeactivateOldLicense()
        {
            if (!OldLicense.Deactivate())
            {
                MessageBox.Show($"Failed to deactivate the old license", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (!AddNewApplication())
            {
                return;
            }

            FillNewLicenseInfo();


            if (NewLicense.Save())
            {
                DeactivateOldLicense();

                MessageBox.Show($"New License Added Successfully with PersonID = {NewLicense.LicenseID}", "Add New License");

                lblReplacedLicenseID.Text = NewLicense.LicenseID.ToString();
                lbl_LR_ApplicationID.Text = NewLicense.Application.ApplicationID.ToString();
                llblShowLicenseInfo.Enabled = true;
            }
            else
            {
                MessageBox.Show("Failed to add the new license", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                DeleteApplicationAfterFail();
            }

        }

        private void llblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int applicationID = NewLicense.Application.ApplicationID;

            frmLicenseInfo frm = new frmLicenseInfo(applicationID);
            frm.ShowDialog();
        }
        private void llblShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string nationalNo = OldLicense.Driver.Person.NationalNO;

            frmPersonsLicenseHistory frm = new frmPersonsLicenseHistory(nationalNo);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
