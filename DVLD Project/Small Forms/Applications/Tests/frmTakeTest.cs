using BusinessLayer.Tests;
using DVLD_Project.Global;
using System;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms.Applications.Tests
{
    public partial class frmTakeTest : Form
    {
        clsTestAppointments TestAppointment;
        clsTests Test;
        int Trial;
        public bool IsPassed { get; set; } = false;

        public frmTakeTest(int testAppointmentID, int trial)
        {
            InitializeComponent();
            TestAppointment = clsTestAppointments.Find(testAppointmentID);
            Trial = trial;
        }


        private void FillFormInfo()
        {
            lbl_LDLA_ID.Text = TestAppointment.LDLA.LDLA_ID.ToString();
            lbl_DL_Class.Text = TestAppointment.LDLA.LicenseClass.ClassName;
            lblName.Text = TestAppointment.LDLA.ApplicantPerson.FullName;
            lblFees.Text = TestAppointment.TestType.Fee.ToString();
            lblTrial.Text = Trial.ToString();
            dateTimePicker1.Text = TestAppointment.Date.ToShortDateString();
            dateTimePicker1.Enabled = false;
        }
        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            FillFormInfo();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Test = new clsTests();

            Test.TestResult = radioButton1.Checked;
            Test.Notes = textBox1.Text.Trim();
            Test.TestApointment = TestAppointment;
            Test.User = clsGlobal.CurrentUser;



            DialogResult result = MessageBox.Show("Are You Sure You Want To Save? After This Operation" +
            " You Cannot Change The Result", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (result == DialogResult.Yes)
            {
                if (Test.Save())
                {
                    MessageBox.Show("Data Saved Successfully", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (Test.TestResult == true)
                        IsPassed = true;

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to save data", "Save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmTakeTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Test == null) //if no saving done, do not update the grid view outside
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }
    }
}
