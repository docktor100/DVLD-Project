using BusinessLayer.Drivers___Licenses;
using DVLD_Project.Small_Forms.Applications.Local_Driving_License_Applications;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms.Applications.International_License
{
    public partial class frm_Int_DL_List : Form
    {
        DataTable _DT;

        public frm_Int_DL_List()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
        private void Load_IDL_ListToGrid()
        {
            //comboBox1.SelectedIndex = 0;

            //int currentRowIndex = -1;
            //if (dataGridView1.Rows.Count > 0)
            //{
            //    currentRowIndex = dataGridView1.CurrentRow.Index;
            //}

            _DT = clsInternationalLicense.ListForManage();
            LoadTable(_DT);

            lblNumberOfRecords.Text = _DT.Rows.Count.ToString();

            //if (currentRowIndex != -1)
            //{
            //    dataGridView1.Rows[currentRowIndex].Selected = true;
            //    dataGridView1.CurrentCell = dataGridView1.Rows[currentRowIndex].Cells[0];
            //}
        }
        private void frm_Int_DL_List_Load(object sender, EventArgs e)
        {
            Load_IDL_ListToGrid();


            mbFilter.Mask = "00000";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.SelectedIndex == 0)
            {
                mbFilter.Visible = false;

                mbFilter.Text = "";
                Load_IDL_ListToGrid();
            }
            else
            {
                mbFilter.Visible = true;

                mbFilter.Focus();
            }

        }
        private void mbFilter_TextChanged(object sender, EventArgs e)
        {
            string column = cbFilterBy.Text;
            string filterText = mbFilter.Text.Trim();

            DataView dv = _DT.DefaultView;

            if (column != "None")
                dv.RowFilter = $"Convert([{column}], 'System.String') LIKE '{filterText}%'";
            else
                dv.RowFilter = "";

            LoadTable(dv.ToTable());
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmIssueInternationalLicense frm = new frmIssueInternationalLicense();
            frm.ShowDialog();

            if (frm.DidAdd)
            {
                Load_IDL_ListToGrid();
            }
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int driverID = (int)dataGridView1.CurrentRow.Cells[2].Value; // 2 is the index of driverID
            int personID = clsDrivers.FindByDriverID(driverID).Person.PersonID;

            frmPersonDetails frm = new frmPersonDetails(personID);
            frm.ShowDialog();
        }
        private void tsmShowLicense_Click(object sender, EventArgs e)
        {
            int I_licenseID = (int)dataGridView1.CurrentRow.Cells[0].Value;

            frm_I_LicenseInfo frm = new frm_I_LicenseInfo(I_licenseID);
            frm.ShowDialog();
        }
        private void showPersonsLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int driverID = (int)dataGridView1.CurrentRow.Cells[2].Value; // 2 is the index of driverID
            string nationalNo = clsDrivers.FindByDriverID(driverID).Person.NationalNO;

            frmPersonsLicenseHistory frm = new frmPersonsLicenseHistory(nationalNo);
            frm.ShowDialog();
        }
    }
}
