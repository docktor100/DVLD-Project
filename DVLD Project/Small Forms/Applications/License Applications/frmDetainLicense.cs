using BusinessLayer.Drivers___Licenses;
using BusinessLayer.Licenses;
using DVLD_Project.Global;
using DVLD_Project.Small_Forms.Applications.Local_Driving_License_Applications;
using System;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms.Applications.License_Applications
{
    public partial class frmDetainLicense : Form
    {
        clsLicenses License;
        clsDetainedLicenses LicenseDetainInfo;

        public bool DoUpdateDataGridView { get; set; } = false; //for frmManagaeDetainedLicenses datagrid

        public frmDetainLicense()
        {
            InitializeComponent();
        }


        private void frmDetainLicense_Load(object sender, EventArgs e)
        {
            lblDetainDate.Text = DateTime.Now.ToShortDateString();
            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;

            LicenseDetainInfo = new clsDetainedLicenses();
        }


        private void CheckValedity()
        {
            btnDetain.Enabled = false;

            if (DateTime.Compare(License.ExpirationDate, DateTime.Now) < 0)
            {
                MessageBox.Show("This license is expired so cannot detain it",
                                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            if (!License.IsActive)
            {
                MessageBox.Show("This license is deactivated, so cannot detain it",
                                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            if (clsDetainedLicenses.IsLicenseDetained(License.LicenseID))
            {
                MessageBox.Show("This license is already detained",
                               "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            btnDetain.Enabled = true;
        }
        private void FillLicenseDetainInfo()
        {
            LicenseDetainInfo.CreatedByUser = clsGlobal.CurrentUser;
            LicenseDetainInfo.DetainDate = DateTime.Now;
            LicenseDetainInfo.FineFees = Convert.ToSingle(mbFineFees.Text.Trim());
            LicenseDetainInfo.License = License;

            LicenseDetainInfo.IsReleased = false;
        }
        private void ResetForm()
        {
            btnDetain.Enabled = false;
            llblShowNewLiecenseInfo.Enabled = false;
            llblShowLicensesHistory.Enabled = false;

            lblLicenseID.Text = "[???]";
        }
        private void ctrFindLicense1_OnSearch(clsLicenses license)
        {
            License = license;

            if (License != null)
            {
                lblLicenseID.Text = License.LicenseID.ToString();

                CheckValedity();

                llblShowLicensesHistory.Enabled = true;
            }
            else
            {
                ResetForm();
            }
        }

        private void DeactivateLicense()
        {
            License.IsActive = false;
            if (!License.Deactivate())
            {
                MessageBox.Show($"Failed to deactivate the old license, deactivate it" +
                    $" manually by PersonID {License.LicenseID}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool IsSure()
        {
            DialogResult result = MessageBox.Show("Are you sure you want to detain this license?",
                                                  "Detain License", MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Information);

            return (result == DialogResult.Yes);
        }
        private void btnDetain_Click(object sender, EventArgs e)
        {
            if (!IsSure())
            {
                return;
            }

            FillLicenseDetainInfo();

            if (LicenseDetainInfo.Save())
            {
                DeactivateLicense();

                MessageBox.Show("License detain saved successfully", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                lblDetainID.Text = LicenseDetainInfo.DetainID.ToString();

                llblShowNewLiecenseInfo.Enabled = true;
                btnDetain.Enabled = false;

                DoUpdateDataGridView = true;
            }
            else
            {
                MessageBox.Show("Failed to save license detain", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void llblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string nationalNo = License.Driver.Person.NationalNO;
            frmPersonsLicenseHistory frm = new frmPersonsLicenseHistory(nationalNo);
            frm.ShowDialog();
        }
        private void llblShowNewLiecenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int applicationID = License.Application.ApplicationID;
            frmLicenseInfo frm = new frmLicenseInfo(applicationID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
