using BusinessLayer.Specific;
using BusinessLayer.Tests;
using DVLD_Project.Properties;
using DVLD_Project.Small_Forms.Applications.Tests;
using System;
using System.ComponentModel;

using System.Data;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms.Applications
{
    public partial class frmScheduleTest : Form
    {
        clsTestType TestType { get; set; }
        cls_LDLA LDLA { get; set; }
        public bool DoUpdate_LDLA_List { get; set; } = false;
        DataTable DT;

        public frmScheduleTest(int lDLA_ID, int testTypeID)
        {
            InitializeComponent();
            TestType = clsTestType.Find(testTypeID);
            LDLA = cls_LDLA.Find(lDLA_ID);
        }

        private void FillDataGridView()
        {
            DT = clsTestAppointments.TestAppointmentsList(LDLA.LDLA_ID, TestType.TestTypeID);
            dataGridView1.DataSource = DT;
            lblRecords.Text = DT.Rows.Count.ToString();


            //if the grid view was not null, and does not have a primary key; make one
            if (DT.Rows.Count != 0 && DT.PrimaryKey != null)
            {
                DT.PrimaryKey = new DataColumn[] { DT.Columns[0] }; // for getting the index of the record for number of trials
            }

        }
        private void SetTitle()
        {

            switch (TestType.TestTypeID)
            {
                case 1:
                    lblTitle.Text = "Vision Test Appointments";
                    pbTitle.Image = Resources.Vision_512;
                    break;

                case 2:
                    lblTitle.Text = "Written Test Appointments";
                    pbTitle.Image = Resources.Written_Test_512;
                    break;

                case 3:
                    lblTitle.Text = "Driving Test Appointments";
                    pbTitle.Image = Resources.driving_test_512;
                    break;

                default:
                    pbTitle.Image = null;
                    break;
            }
        }
        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            ctr_LDL_ApplicationInfo1.LoadDataToForm(LDLA);
            SetIfPassed();
            FillDataGridView();

            SetTitle();

            bool islocked = false;
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Descending);

                islocked = (bool)DT.Rows[DT.Rows.Count - 1]["islocked"];
            }

            takeTestToolStripMenuItem.Enabled = !islocked;
        }


        private void SetIfPassed()
        {
            bool IsPassed = clsTests.IsThereAnyPassedTest(LDLA.LDLA_ID, TestType.TestTypeID);

            if (IsPassed)
            {
                lblPassed.Visible = true;
                pbIsPassed.Visible = true;

                btnAddNewAppointment.Visible = false;


                ctr_LDL_ApplicationInfo1.SetIfPassed(IsPassed);
            }
        }
        private bool IsThereUnLockedAppointments()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!(bool)row.Cells["IsLocked"].Value) // if not locked
                {
                    return true;
                }
            }
            return false;
        }
        private void btnAddNewAppointment_Click(object sender, EventArgs e)
        {
            if (IsThereUnLockedAppointments())
            {
                MessageBox.Show("Cannot Add A New Appointment When There are Incomplete Ones", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmAddNewTestAppointment frm = new frmAddNewTestAppointment(LDLA, TestType, DT.Rows.Count);

            if (frm.ShowDialog() == DialogResult.OK) // when save is pressed, else close is pressed
            {
                FillDataGridView();
                SetIfPassed();
            }
        }


        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int testAppID = (int)dataGridView1.CurrentRow.Cells[0].Value;

            int Trials = DT.Rows.IndexOf(DT.Rows.Find(testAppID)) + 1;

            frmAddNewTestAppointment frm = new frmAddNewTestAppointment(testAppID, Trials);

            if (frm.ShowDialog() == DialogResult.OK) // OK after saving any changes
                FillDataGridView();
        }
        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int testAppID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            clsTestAppointments testApp = clsTestAppointments.Find(testAppID);

            frmTakeTest frm = new frmTakeTest(testAppID, DT.Rows.Count);

            if (frm.ShowDialog() == DialogResult.Cancel)// no saving done
            {
                return;
            }


            FillDataGridView();

            if (frm.IsPassed) // the test result
            {
                SetIfPassed();
                DoUpdate_LDLA_List = true; // to update the number of passed tests in the gridview
            }
            else
            {
                DoUpdate_LDLA_List = false;
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            bool isLocked = (bool)dataGridView1.CurrentRow.Cells["IsLocked"].Value;
            takeTestToolStripMenuItem.Enabled = !isLocked;


            editToolStripMenuItem.Text = (isLocked) ? "Appointment Details" : "Edit";
        }
    }
}
