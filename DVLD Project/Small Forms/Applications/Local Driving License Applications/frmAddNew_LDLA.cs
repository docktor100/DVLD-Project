using BusinessLayer;
using BusinessLayer.Applications;
using BusinessLayer.Specific;
using DVLD_Project.Global;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms.Applications
{
    public partial class frmAddNew_LDLA : Form
    {
        public frmAddNew_LDLA()
        {
            InitializeComponent();
        }

        public cls_LDLA LDLA { get; set; }

        private void FillLicenseClass()
        {
            DataTable dtLicenseClass = clsApplication.LicenseClassesList();

            foreach (DataRow Row in dtLicenseClass.Rows)
            {
                cbLiceseClass.Items.Add(Row["ClassName"]);
            }

            cbLiceseClass.SelectedIndex = 2;
        }
        private void frmAddNewLocal_DL_Applicatino_Load(object sender, System.EventArgs e)
        {
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;

            float New_LDLA_Fee = clsApplicationTypes.Find(1).Fee;
            lblFees.Text = New_LDLA_Fee.ToString();


            LDLA = new cls_LDLA();

            FillLicenseClass();

            ctrFindPerson1.FilterFocus();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }
        private void btnPrev_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }


        private bool CheckIfSelectedLicenseExists(out int ID)
        {
            ID = -1;

            //DataTable dtPersonLDLApplications = cls_LDLA.PersonLDLApplicationsList(LDLA.ApplicantPerson.PersonID);
            ////automatically brings local driving license applications for specified person
            ////applicationtypeID will be 1 automatically


            //foreach (DataRow row in dtPersonLDLApplications.Rows)
            //{
            //    if ((int)row["LicenseClassID"] == LDLA.LicenseClass.LicenseClassID
            //        &&
            //        (byte)row["ApplicationStatus"] != 2)
            //    {
            //        ID = (int)row["ApplicationID"];
            //        return true;
            //    }
            //}

            //return false;


            if (cls_LDLA.DoesHaveNoneCancelledLicense(LDLA.ApplicantPerson.PersonID, LDLA.LicenseClass.LicenseClassID, out int id))
            {
                ID = id;
                return true;
            }
            else return false;
        }
        private void Fill_LDLA_Info()
        {
            LDLA.ApplicantPerson = ctrFindPerson1.SelectedPersonInfo;
            LDLA.CreatedByUser = clsGlobal.CurrentUser;
            LDLA.ApplicationType = clsApplicationTypes.Find(1); //1 is new LDLA

            LDLA.Date = DateTime.Now;
            LDLA.PaidFees = Convert.ToSingle(lblFees.Text);
            LDLA.LastStatusDate = LDLA.Date;
            LDLA.Status = clsApplication.enStatus.New;


            //DataRow licencseRecord = dtLicenseClass.Select($"ClassName = '{cbLiceseClass.Text}'")[0];
            //int licenseID = (int)licencseRecord["LicenseClassID"];
            //LDLApplication.LicenseClass = clsLicenseClass.Find(licenseID);

            LDLA.LicenseClass = clsLicenseClass.Find(cbLiceseClass.SelectedIndex + 1);


        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Fill_LDLA_Info();

            if (CheckIfSelectedLicenseExists(out int ID))
            {
                //errorProvider1.SetError(cbLiceseClass, "This Applicant Person Already Has Such Local Driving License");
                MessageBox.Show($"Choose another license class, The selected person already have an active application for the selected class with id = {ID}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (LDLA.Save())
            {
                MessageBox.Show("Data saved successfully", "Add New Local Driving Licesne");
                lblApplicationID.Text = LDLA.ApplicationID.ToString();
            }
            else
            {
                MessageBox.Show("Failed to save data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbLiceseClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(cbLiceseClass, "");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tabPage2 && ctrFindPerson1.SelectedPersonInfo == null)
            {
                e.Cancel = true;
            }
        }
    }
}
