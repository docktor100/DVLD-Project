using BusinessLayer.Applications;
using BusinessLayer.Specific;
using DVLD_Project.Small_Forms.Applications.Local_Driving_License_Applications;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms.Applications
{
    public partial class frm_LDLA_List : Form
    {
        private static DataTable _DT = new DataTable();

        public frm_LDLA_List()
        {
            InitializeComponent();
        }

        private void LoadTable(DataTable table)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();


            foreach (DataRow row in table.Rows)
            {
                dataGridView1.Rows.Add(row.ItemArray);
            }
        }
        private void Load_LDLA_ListToGrid()
        {
            cbFilter.SelectedIndex = 0;

            int currentRowIndex = -1;
            if (dataGridView1.Rows.Count > 0)
            {
                currentRowIndex = dataGridView1.CurrentRow.Index;
            }

            _DT = cls_LDLA.LDLA_List();
            LoadTable(_DT);

            lblNumberOfRecords.Text = _DT.Rows.Count.ToString();

            if (currentRowIndex != -1)
            {
                dataGridView1.Rows[currentRowIndex].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[currentRowIndex].Cells[0];
            }
        }
        private void frmLocalDrivingLicenseApplicationsList_Load(object sender, EventArgs e)
        {
            cbFilter.SelectedIndex = 0;
            //comboBox1_SelectedIndexChanged(dataGridView1, EventArgs.Empty); 
            //this function will be triggered by the line above
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbFilter.Text)
            {
                case "None":
                    mbFilter.Visible = false;
                    mbFilter.Text = "";

                    cbStatus.Visible = false;

                    Load_LDLA_ListToGrid();
                    break;

                case "Status":
                    mbFilter.Visible = false;
                    mbFilter.Text = "";

                    cbStatus.Visible = true;
                    cbStatus.SelectedIndex = 0;
                    break;

                case "LocalDrivingLicenseApplicationID":
                    mbFilter.Visible = true;
                    mbFilter.Text = "00000";
                    mbFilter.Focus();

                    cbStatus.Visible = false;
                    break;

                default:
                    mbFilter.Visible = true;
                    mbFilter.Text = "";
                    mbFilter.Focus();

                    cbStatus.Visible = false;
                    break;
            }


        }
        private void mbFilter_TextChanged(object sender, EventArgs e)
        {
            string column = cbFilter.Text;
            string filterText = mbFilter.Text.Trim();

            DataView dv = _DT.DefaultView;

            if (column == "ClassName")
                dv.RowFilter = $"Convert([{column}], 'System.String') LIKE '%{filterText}%'";
            else if (column != "None")
                dv.RowFilter = $"Convert([{column}], 'System.String') LIKE '{filterText}%'";
            else
                dv.RowFilter = "";

            LoadTable(dv.ToTable());
        }
        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string column = cbFilter.Text;
            string filterText = cbStatus.Text.Trim();

            DataView dv = _DT.DefaultView;

            if (filterText != "None")
                dv.RowFilter = $"{column} = '{filterText}'";
            else
                dv.RowFilter = "";

            LoadTable(dv.ToTable());
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddNew_LDLA frm = new frmAddNew_LDLA();
            frm.ShowDialog();
            Load_LDLA_ListToGrid();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = (int)dataGridView1.CurrentRow.Cells[0].Value;
            frm_LDLA_Info frm = new frm_LDLA_Info(id);
            frm.ShowDialog();
        }
        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDLA_ID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            cls_LDLA LDLA = cls_LDLA.Find(LDLA_ID);


            if (LDLA.Status == clsApplication.enStatus.Completed)
            {
                MessageBox.Show("Can not cancel this application because it is already completed", "Status Change");
                return;
            }


            DialogResult ConfirmResult = MessageBox.Show("Are you sure you want to cancel this application?", "Cancel Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ConfirmResult == DialogResult.No)
            {
                return;
            }


            LDLA.Status = clsApplication.enStatus.Cancelled;

            if (LDLA.Save())
            {
                MessageBox.Show("Application Cancelled Successfully", "Status Change");
            }
            else
            {
                MessageBox.Show("Failed to save status changing!", "Status Change");
            }

        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete this local" +
                " driving application?, you wont be able to redo this move", "Warning",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                return;
            }

            int LDLA_ID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            if (cls_LDLA.Delete(LDLA_ID)) // deletes all data related to it
            {
                MessageBox.Show("Data Deleted Successfully", "Delete");
            }
            else
            {
                MessageBox.Show("Delete failed due to a fail in deleting LDLA data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Load_LDLA_ListToGrid();
        }


        private void ScheduleTest(int TestTypeID)
        {
            int LDLA_ID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            frmScheduleTest frm = new frmScheduleTest(LDLA_ID, TestTypeID);
            frm.ShowDialog();
            if (frm.DoUpdate_LDLA_List) // occures after saving a test result to pass in action (not by default)
            {
                Load_LDLA_ListToGrid();
            }
        }
        private void tsmScheduleVisionTest_Click(object sender, EventArgs e)
        {
            ScheduleTest(1);
        }
        private void tsmScheduleWrittentest_Click(object sender, EventArgs e)
        {
            ScheduleTest(2);
        }
        private void tsmScheduleDrivingTest_Click(object sender, EventArgs e)
        {
            ScheduleTest(3);
        }

        private void SetSchedulesText(sbyte PassedTests)
        {
            tsmScheduleVisionTest.Text = (PassedTests >= 1) ? "Vision Tests" : "Schedule Vision Test";

            tsmScheduleWrittentest.Text = (PassedTests >= 2) ? "Written Tests" : "Schedule Written Test";

            if (PassedTests == 3)
            {
                tsmScheduleDrivingTest.Text = "Driving Tests";
                tsmScheduleTests.Text = "Taken Tests";
            }
            else
            {
                tsmScheduleDrivingTest.Text = "Schedule Driving Test";
                tsmScheduleTests.Text = "Schedule Tests";
            }
        }
        private void SetScheduleTests_Visible(sbyte PassedTests)
        {
            if (PassedTests == -1)
            {
                tsmScheduleTests.Enabled = false;
                return;
            }

            tsmScheduleVisionTest.Enabled = true;
            tsmScheduleWrittentest.Enabled = true;
            tsmScheduleDrivingTest.Enabled = true;


            if (PassedTests == 0)
            {
                tsmScheduleWrittentest.Enabled = false;
                tsmScheduleDrivingTest.Enabled = false;
            }
            if (PassedTests == 1)
            {
                tsmScheduleDrivingTest.Enabled = false;
            }

            SetSchedulesText(PassedTests);
        }
        private void SetContextMenueStrip_Enable(string status, sbyte passedTests)
        {
            //defaults:
            tsmIssueDriverLicense.Visible = false;
            tsmCancelApplication.Visible = false;
            tsmShowLicense.Visible = false;
            tsmDelete.Visible = true;


            if (status == "New")
            {
                SetScheduleTests_Visible(passedTests);


                tsmCancelApplication.Visible = true;
                tsmIssueDriverLicense.Visible = (passedTests == 3);

            }
            else if (status == "Completed")
            {
                SetScheduleTests_Visible(3); // inables all three

                tsmDelete.Visible = false;

                tsmShowLicense.Visible = true;
            }
            else if (status == "Cancelled")
            {
                SetScheduleTests_Visible(-1); //disables all three
            }
        }
        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string Status = (dataGridView1.CurrentRow.Cells["Status"].Value).ToString();
            sbyte passedTests = Convert.ToSByte(dataGridView1.CurrentRow.Cells["PassedTestCount"].Value);

            SetContextMenueStrip_Enable(Status, passedTests);
        }



        private void tsmIssueDriverLicense_Click(object sender, EventArgs e)
        {
            int LDLA_ID = (int)dataGridView1.CurrentRow.Cells[0].Value;

            frmIssueLicense_FirstTime frm = new frmIssueLicense_FirstTime(LDLA_ID);
            frm.ShowDialog();
            if (frm.DoUpdate_LDLA_List)
            {
                Load_LDLA_ListToGrid();
            }
        }
        private void tsmShowLicense_Click(object sender, EventArgs e)
        {
            int LDLA_ID = (int)dataGridView1.CurrentRow.Cells[0].Value;

            cls_LDLA LDLA = cls_LDLA.Find(LDLA_ID);
            frmLicenseInfo frm = new frmLicenseInfo(LDLA.ApplicationID);
            frm.ShowDialog();
        }


        private void showPersonsLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string nationalNo = dataGridView1.CurrentRow.Cells["NationalNo"].Value.ToString();

            frmPersonsLicenseHistory frm = new frmPersonsLicenseHistory(nationalNo);
            frm.ShowDialog();
        }

    }
}
