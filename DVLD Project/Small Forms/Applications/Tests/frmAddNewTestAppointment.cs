using BusinessLayer;
using BusinessLayer.Applications;
using BusinessLayer.Specific;
using BusinessLayer.Tests;
using DVLD_Project.Global;
using DVLD_Project.Properties;
using System;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms
{
    public partial class frmAddNewTestAppointment : Form
    {
        public bool DoUpdateGridView { get; set; } = false;

        private enum enMode { AddNew, Update };
        enMode Mode;

        clsTestType TestType { get; set; } = null;
        cls_LDLA LDLA { get; set; } = null;
        clsTestAppointments TestAppointment { get; set; } = null;
        int Trials = 0; // retrieved either by number of rows(addnew) or row Index(udpate) of the data grid


        public frmAddNewTestAppointment(cls_LDLA lDLA, clsTestType testType, int trials)
        {
            InitializeComponent();
            TestType = testType;
            LDLA = lDLA;
            Trials = trials;
            Mode = enMode.AddNew;
        }
        public frmAddNewTestAppointment(int testAppointmentID, int trials)
        {
            InitializeComponent();
            TestAppointment = clsTestAppointments.Find(testAppointmentID);
            LDLA = TestAppointment.LDLA;
            TestType = TestAppointment.TestType;
            Trials = trials;
            Mode = enMode.Update;
        }

        private void FillFormInfo()
        {
            //shared between "edit" and "Addnew"
            lbl_DL_Class.Text = LDLA.LicenseClass.ClassName;
            lblName.Text = LDLA.ApplicantPerson.FullName;
            lblFees.Text = TestType.Fee.ToString();
            lblFees.Text = TestType.Fee.ToString();
            lblTrial.Text = Trials.ToString();
            lbl_LDLA_ID.Text = LDLA.LDLA_ID.ToString();



            float retakeTestApplicationFee = 0;
            if (Mode == enMode.AddNew)
            {
                retakeTestApplicationFee = clsApplicationTypes.Find(7).Fee;
                lbl_R_App_Fee.Text = retakeTestApplicationFee.ToString();
                lblTotalFee.Text = (retakeTestApplicationFee + TestType.Fee).ToString();

                dateTimePicker1.Text = DateTime.Now.AddDays(1).ToShortDateString();
            }
            else
            {
                dateTimePicker1.Text = TestAppointment.Date.ToShortDateString();

                if (TestAppointment.RetaketestApplication != null)
                {
                    retakeTestApplicationFee = TestAppointment.RetaketestApplication.PaidFees;
                    lbl_R_App_Fee.Text = retakeTestApplicationFee.ToString();
                    lblTotalFee.Text = (retakeTestApplicationFee + TestAppointment.PaidFees).ToString();

                    lbl_R_App_ID.Text = TestAppointment.RetaketestApplication.ApplicationID.ToString();
                }
                else
                    gbRetakeTestInfo.Enabled = false;


                if (TestAppointment.IsLocked)
                {
                    dateTimePicker1.Enabled = false;
                    btnSave.Enabled = false;
                }
            }
        }
        private void SetTitle()
        {

            switch (TestType.TestTypeID)
            {
                case 1:
                    this.Text = "Vision Test";
                    gbRetakeTestInfo.Text = "Vision Test";
                    pbTitle.Image = Resources.Vision_512;
                    break;

                case 2:
                    this.Text = "Written Test";
                    gbRetakeTestInfo.Text = "Written Test";
                    pbTitle.Image = Resources.Written_Test_512;
                    break;

                case 3:
                    this.Text = "Driving Test";
                    gbRetakeTestInfo.Text = "Driving Test";
                    pbTitle.Image = Resources.driving_test_512;
                    break;

                default:
                    pbTitle.Image = null;
                    gbRetakeTestInfo.Text = "";
                    break;
            }
        }
        private void frmAddNewTestAppointment_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MinDate = DateTime.Now;

            if (Trials == 0)
            {
                gb_RT_App_Info.Enabled = false;
            }

            FillFormInfo();

            SetTitle();

            lblTitle.Text = (Trials > 1) ? "Schedule Retake Test" : "Schedule Test";
        }

        private clsApplication AddNewRetakeApplication()
        {
            clsApplication Application = new clsApplication();

            Application.ApplicantPerson = LDLA.ApplicantPerson;
            Application.ApplicationType = LDLA.ApplicationType;
            Application.CreatedByUser = clsGlobal.CurrentUser;
            Application.Date = DateTime.Now;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationTypes.Find(7).Fee;
            Application.Status = clsApplication.enStatus.Completed;

            if (Application.Save())
            {
                return Application;
            }
            else
            {
                throw new Exception("Failed to Save Retake Application");
            }
        }
        private void Fill_TestAppointment()
        {
            TestAppointment = new clsTestAppointments();

            TestAppointment.LDLA = LDLA;
            TestAppointment.TestType = TestType;
            TestAppointment.CreatedByUsed = clsGlobal.CurrentUser;
            TestAppointment.Date = dateTimePicker1.Value;
            TestAppointment.IsLocked = false;
            TestAppointment.PaidFees = TestType.Fee;

            if (Trials > 0)
            {
                TestAppointment.RetaketestApplication = AddNewRetakeApplication();
            }
            else
            {
                TestAppointment.RetaketestApplication = null;
            }

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Mode == enMode.AddNew)
            {
                Fill_TestAppointment();
            }
            else
                TestAppointment.Date = dateTimePicker1.Value;



            if (TestAppointment.Save())
            {
                MessageBox.Show("Data Saved Successfully", "Data Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed to save data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.DialogResult = DialogResult.OK;
            DoUpdateGridView = true;
            this.Close();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
