using BusinessLayer;
using BusinessLayer.Drivers___Licenses;
using BusinessLayer.Licenses;
using DVLD_Project.Properties;
using System.IO;
using System.Windows.Forms;

namespace DVLD_Project.User_Controls
{
    public partial class ctrLicenseInfo : UserControl
    {
        public ctrLicenseInfo()
        {
            InitializeComponent();
        }

        private void SetGendorImage(bool IsMale)
        {
            if (IsMale)
            {
                pbPersonImage.Image = Resources.Male_512;
            }
            else
            {
                pbPersonImage.Image = Resources.Female_512;
            }
        }

        private void ResetForm()
        {
            lblLicenseID.Text = "[???]";
            lblClass.Text = "[???]";
            lblNotes.Text = "[???]";
            lblExpirationDate.Text = "[???]";
            lblIssueDate.Text = "[???]";
            lblIssueReason.Text = "[???]";
            lblIsActive.Text = "[???]";
            lblIsDetained.Text = "[???]";

            lblName.Text = "[???]";
            lblNationalNo.Text = "[???]";
            lblDateOfBirth.Text = "[???]";
            lblGendor.Text = "[???]";
            lblDriverID.Text = "[???]";

            SetGendorImage(true);
        }
        public void LoadDataToForm(clsLicenses license)
        {
            if (license == null)
            {
                ResetForm();
                return;
            }

            clsPerson ApplicantPerson = license.Application.ApplicantPerson;

            lblLicenseID.Text = license.LicenseID.ToString();
            lblClass.Text = license.LicenseClass.ClassName;
            lblNotes.Text = (string.IsNullOrEmpty(license.Notes)) ? "No notes" : license.Notes;
            lblExpirationDate.Text = license.ExpirationDate.ToShortDateString();
            lblIssueDate.Text = license.IssueDate.ToShortDateString();
            lblIssueReason.Text = license.GetIssueReasonString();
            lblIsActive.Text = (license.IsActive) ? "Yes" : "No";
            lblIsDetained.Text = (clsDetainedLicenses.IsLicenseDetained(license.LicenseID)) ? "Yes" : "No";

            lblName.Text = ApplicantPerson.FullName;
            lblNationalNo.Text = ApplicantPerson.NationalNO;
            lblDateOfBirth.Text = ApplicantPerson.DateOfBirth.ToShortDateString();
            lblGendor.Text = (ApplicantPerson.Gendor) ? "Female" : "Male";
            lblDriverID.Text = license.Driver.DriverID.ToString();

            if (ApplicantPerson.ImageName != "")
            {
                string imagePath = Path.Combine(Application.StartupPath, "DVLD People Images", ApplicantPerson.ImageName);

                if (File.Exists(imagePath))
                    pbPersonImage.ImageLocation = imagePath;
                else
                    MessageBox.Show("Could not find this image: = " + imagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                SetGendorImage(!ApplicantPerson.Gendor);
            }
        }
    }
}
