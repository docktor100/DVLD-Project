using BusinessLayer.Drivers___Licenses;
using BusinessLayer.Licenses;
using DVLD_Project.Small_Forms.Applications.Local_Driving_License_Applications;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms.Applications.License_Applications
{
    public partial class frmManageDetainedLicenses : Form
    {
        private static DataTable _DT = new DataTable();

        public frmManageDetainedLicenses()
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

            dataGridView1.Sort(dataGridView1.Columns[0], System.ComponentModel.ListSortDirection.Descending);
        }
        private void LoadDetainedLicensesToGrid()
        {

            _DT = clsDetainedLicenses.List();
            LoadTable(_DT);

            lblRecords.Text = _DT.Rows.Count.ToString();
        }
        private void frmManageDetainedLicenses_Load(object sender, EventArgs e)
        {
            LoadDetainedLicensesToGrid();
            lblRecords.Text = _DT.Rows.Count.ToString();

            cbFilter.SelectedIndex = 0;
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbIsReleased.Visible = false;

            switch (cbFilter.Text)
            {
                case "None":
                    mbFilter.Visible = false;

                    break;

                case "DetainID":
                case "LicenseID":

                    mbFilter.Visible = true;
                    mbFilter.Text = "";
                    mbFilter.Focus();
                    mbFilter.Mask = "00000";

                    break;

                case "IsReleased":
                    cbIsReleased.Visible = true;
                    cbIsReleased.SelectedIndex = 0;

                    mbFilter.Visible = false;

                    break;

                default: // FullName and NationalNo
                    mbFilter.Visible = true;
                    mbFilter.Text = "";
                    mbFilter.Focus();
                    mbFilter.Mask = "";

                    break;
            }
        }
        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            string column = cbFilter.Text;
            string filterText = (cbIsReleased.Text == "Yes") ? "true" : "false";

            DataView dv = _DT.DefaultView;

            if (cbIsReleased.Text != "None")
                dv.RowFilter = $"{column} = {filterText}";
            else
                dv.RowFilter = "";

            LoadTable(dv.ToTable());
        }
        private void mbFilter_TextChanged(object sender, EventArgs e)
        {
            string column = cbFilter.Text;
            string filterText = mbFilter.Text.Trim();

            DataView dv = _DT.DefaultView;

            if (column != "None")
                dv.RowFilter = $"Convert([{column}], 'System.String') LIKE '{filterText}%'";
            else
                dv.RowFilter = "";

            LoadTable(dv.ToTable());
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense();
            frm.ShowDialog();

            if (frm.DoUpdateDataGridView)
            {
                LoadDetainedLicensesToGrid();
            }

        }
        private void btnReleaseLicense_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense();
            frm.ShowDialog();

            if (frm.DoUpdateDataGridView)
            {
                LoadDetainedLicensesToGrid();
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string nationalNo = dataGridView1.CurrentRow.Cells["NationalNo"].Value.ToString();
            frmPersonDetails frm = new frmPersonDetails(nationalNo);
            frm.ShowDialog();
        }
        private void tsmShowLicense_Click(object sender, EventArgs e)
        {
            int licenseID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["LicenseID"].Value);
            int appolicationID = clsLicenses.FindByLicenseID(licenseID).Application.ApplicationID;

            frmLicenseInfo frm = new frmLicenseInfo(appolicationID);
            frm.ShowDialog();
        }
        private void showPersonsLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string nationalNo = dataGridView1.CurrentRow.Cells["NationalNo"].Value.ToString();
            frmPersonsLicenseHistory frm = new frmPersonsLicenseHistory(nationalNo);
            frm.ShowDialog();
        }
        private void releaseLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int licenseID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["LicenseID"].Value);

            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense(licenseID);
            frm.ShowDialog();

            if (frm.DoUpdateDataGridView)
            {
                LoadDetainedLicensesToGrid();
                cbFilter.SelectedIndex = 0;
            }
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (_DT.Rows.Count > 0 && dataGridView1.CurrentRow != null)
            {
                bool isReleased = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["IsReleased"].Value);

                releaseLicenseToolStripMenuItem.Enabled = !isReleased;
            }

        }
    }
}
