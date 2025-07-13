using BusinessLayer;
using BusinessLayer.Drivers___Licenses;
using BusinessLayer.Licenses;
using DVLD_Project.Properties;
using DVLD_Project.Small_Forms;
using DVLD_Project.Small_Forms.Applications.Local_Driving_License_Applications;
using System.IO;
using System.Windows.Forms;

namespace DVLD_Project.User_Controls
{
    public partial class ctrInternationalLicenseInfo : UserControl
    {
        clsInternationalLicense I_License;


        public ctrInternationalLicenseInfo()
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
        public void LoadDataToForm(int I_LicenseID)
        {
            I_License = clsInternationalLicense.FindBy_IDL_ID(I_LicenseID);
            clsPerson person = I_License.Driver.Person;

            lblApplicationID.Text = I_License.Application.ApplicationID.ToString();
            lblName.Text = person.FullName;
            lblDateOfBirth.Text = person.DateOfBirth.ToShortDateString();
            lblDriverID.Text = I_License.Driver.DriverID.ToString();
            lblGendor.Text = (person.Gendor) ? "Female" : "Male";
            lblIsActive.Text = (I_License.IsActive) ? "Yes" : "No";
            lbl_L_LicenseID.Text = I_License.IssuedUsing_LDL.LicenseID.ToString();
            lblNationalNo.Text = person.NationalNO;
            lbl_I_LicenseID.Text = I_License.I_LicenseID.ToString();


            if (person.ImageName != "")
            {
                string imagePath = Path.Combine(Application.StartupPath, "DVLD People Images", person.ImageName);

                if (File.Exists(imagePath))
                    pbPersonImage.ImageLocation = imagePath;
                else
                    MessageBox.Show("Could not find this image: = " + imagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SetGendorImage(!person.Gendor);
            }

            lblExpirationDate.Text = I_License.ExpirationDate.ToShortDateString();
            lblIssueDate.Text = I_License.IssueDate.ToShortDateString();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int applicationID = clsLicenses.FindByAppID(I_License.IssuedUsing_LDL.Application.ApplicationID).Application.ApplicationID;

            frmLicenseInfo frm = new frmLicenseInfo(applicationID);
            frm.ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonDetails frm = new frmPersonDetails(lblNationalNo.Text);
            frm.ShowDialog();
        }
    }
}
