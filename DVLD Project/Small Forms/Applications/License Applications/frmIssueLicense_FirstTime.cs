using BusinessLayer.Applications;
using BusinessLayer.Drivers___Licenses;
using BusinessLayer.Licenses;
using BusinessLayer.Specific;
using DVLD_Project.Global;
using System;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms.Applications.Local_Driving_License_Applications
{
    public partial class frmIssueLicense_FirstTime : Form
    {
        cls_LDLA LDLA;
        clsLicenses NewLicense;

        public bool DoUpdate_LDLA_List { get; set; } = false;

        public frmIssueLicense_FirstTime(int LDLA_ID)
        {
            InitializeComponent();
            LDLA = cls_LDLA.Find(LDLA_ID);
        }
        public frmIssueLicense_FirstTime(cls_LDLA lDLA)
        {
            InitializeComponent();
            LDLA = lDLA;
        }

        private void frmIssueLicense_FirstTime_Load(object sender, EventArgs e)
        {
            ctrLDLApplicationInfo1.LoadDataToForm(LDLA);
            NewLicense = new clsLicenses();
        }

        private void FillLicenseInfo(clsLicenses NewLicense)
        {
            NewLicense.Application = clsApplication.Find(LDLA.ApplicationID);
            NewLicense.CreatedByUser = clsGlobal.CurrentUser;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = DateTime.Now.AddYears(LDLA.LicenseClass.DefaultValidityLength);
            NewLicense.IsActive = true;
            NewLicense.LicenseClass = LDLA.LicenseClass;
            NewLicense.Notes = tbNotes.Text.Trim();
            NewLicense.Paidfees = LDLA.LicenseClass.ClassFees;
            NewLicense.IssueReason = clsLicenses.enIssueReason.FirstTime;
        }
        private bool AddNewDriverIfNotExist()
        {
            clsDrivers driver = clsDrivers.FindByPersonID(LDLA.ApplicantPerson.PersonID);

            if (driver != null)
            {
                NewLicense.Driver = driver;
                return true;
            }
            else
            {
                NewLicense.Driver.CreateDate = DateTime.Now;
                NewLicense.Driver.CreatedByUser = clsGlobal.CurrentUser;
                NewLicense.Driver.Person = LDLA.ApplicantPerson;
            }


            if (!NewLicense.Driver.Save())
            {
                MessageBox.Show("Failed to add the driver to the system", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            return true;
        }
        private void tbnIssue_Click(object sender, EventArgs e)
        {
            FillLicenseInfo(NewLicense);

            if (!AddNewDriverIfNotExist())
            {
                return;
            }

            if (NewLicense.Save())
            {
                LDLA.Status = clsApplication.enStatus.Completed;
                LDLA.Save();//for updating status

                MessageBox.Show($"License issued Successfully with PersonID = {NewLicense.LicenseID}", "License Issue");
                DoUpdate_LDLA_List = true;

                this.Close();
            }
            else
            {
                MessageBox.Show("Something Went wrong", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
