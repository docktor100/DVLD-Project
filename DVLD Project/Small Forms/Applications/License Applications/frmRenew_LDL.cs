using BusinessLayer;
using BusinessLayer.Applications;
using BusinessLayer.Licenses;
using DVLD_Project.Global;
using System;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms.Applications.Local_Driving_License_Applications
{
    public partial class frmRenew_LDL : Form
    {
        clsLicenses OldLicense;
        clsLicenses NewLicense;

        public frmRenew_LDL()
        {
            InitializeComponent();
        }

        private void frmRenew_LDL_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = DateTime.Now.ToShortDateString();
            lblApplicationFees.Text = clsApplicationTypes.Find(2).Fee.ToString(); // 2 stands for new driving license
            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;

            NewLicense = new clsLicenses();
        }

        private void CheckValedity()
        {
            if (DateTime.Compare(OldLicense.ExpirationDate, DateTime.Now) > 0)
            {
                MessageBox.Show("This license is not expired so cannot make a renewal for it",
                                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnRenew.Enabled = false;
                tbNotes.Enabled = false;
            }
            else
            {
                btnRenew.Enabled = true;
                tbNotes.Enabled = true;
            }
        }
        private void ResetFormInfo()
        {
            lblExpirationDate.Text = "[???]";
            tbNotes.Text = "";
            lblOld_LDL_ID.Text = "[???]";
            lblRenewed_LDL_ID.Text = "[???]";
            lblRenew_LDLA_ID.Text = "[???]";
            lblLicenseFees.Text = "[$$$]";
            lblTotalFees.Text = "[$$$]";

            btnRenew.Enabled = false;
            tbNotes.Enabled = false;
        }
        private void FillRenewApplicationInfo()
        {
            clsLicenseClass licenseclass = OldLicense.LicenseClass;

            lblOld_LDL_ID.Text = OldLicense.LicenseID.ToString();
            lblExpirationDate.Text = DateTime.Now.AddYears(licenseclass.MinimumAge).ToShortDateString();
            lblLicenseFees.Text = licenseclass.ClassFees.ToString();

            float totalfees = licenseclass.ClassFees + Convert.ToSingle(lblApplicationFees.Text);
            lblTotalFees.Text = totalfees.ToString();

            //lblRenewed_LDL_ID.Text = "[???]";
            //lblRenew_LDLA_ID.Text = "[???]";
        }
        private void ctrFindLicense1_OnSearch(clsLicenses license)
        {
            OldLicense = license;

            if (OldLicense != null)
            {
                FillRenewApplicationInfo();

                CheckValedity();
            }
            else
            {
                ResetFormInfo();
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
        private bool AddNewApplication()
        {
            clsApplication application = new clsApplication();
            clsApplicationTypes applicationType = clsApplicationTypes.Find(2); // 2 stands for new driving license

            application.ApplicantPerson = OldLicense.Driver.Person;
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


            NewLicense.Application = application;

            return true;
        }
        private void FillNewLicenseInfo()
        {
            clsLicenseClass licenseClass = OldLicense.LicenseClass;

            NewLicense.CreatedByUser = clsGlobal.CurrentUser;
            NewLicense.Driver = OldLicense.Driver;
            NewLicense.IsActive = true;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = DateTime.Now.AddYears(licenseClass.MinimumAge);
            NewLicense.LicenseClass = licenseClass;
            NewLicense.Paidfees = licenseClass.ClassFees;
            NewLicense.Notes = tbNotes.Text.Trim();
            NewLicense.IssueReason = clsLicenses.enIssueReason.Renew;
        }
        private void DeactivateOldLicense()
        {
            OldLicense.IsActive = false;
            if (!OldLicense.Deactivate())
            {
                MessageBox.Show($"Failed to deactivate the old license, deactivate it" +
                    $" manually by PersonID {OldLicense.LicenseID}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnRenew_Click(object sender, EventArgs e)
        {
            if (!AddNewApplication())
            {
                return;
            }

            FillNewLicenseInfo();

            if (NewLicense.Save())
            {
                DeactivateOldLicense();

                MessageBox.Show("License Added Successfully", "Renew License",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnRenew.Enabled = false;
                llblShowNewLiecenseInfo.Enabled = true;
                ctrFindLicense1.DisableSearch();

                lblRenewed_LDL_ID.Text = NewLicense.LicenseID.ToString();
                lblRenew_LDLA_ID.Text = NewLicense.Application.ApplicationID.ToString();
            }
            else
            {
                MessageBox.Show("Failed To Renew The License", "Renew License", MessageBoxButtons.OK, MessageBoxIcon.Error);


                DeleteApplicationAfterFail();
            }
        }

        private void llblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonsLicenseHistory frm = new frmPersonsLicenseHistory(NewLicense.Driver.Person.NationalNO);
            frm.ShowDialog();
        }
        private void llblShowNewLiecenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo(NewLicense.Application.ApplicationID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
