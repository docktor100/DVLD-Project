using BusinessLayer;
using BusinessLayer.Drivers___Licenses;
using BusinessLayer.Licenses;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms.Applications.Local_Driving_License_Applications
{
    public partial class frmPersonsLicenseHistory : Form
    {
        int ApplicantPersonID;
        DataTable _DT;

        public frmPersonsLicenseHistory(string nationalNo)
        {
            InitializeComponent();
            ApplicantPersonID = clsPerson.Find(nationalNo).PersonID;
        }

        private void frmPersonsLicenseHistory_Load(object sender, EventArgs e)
        {
            Load_L_LicensesListToGrid();
            lbl_L_LicensesRecords.Text = dgv_L_Licenses.Rows.Count.ToString();

            Load_I_LicensesListToGrid();
            lbl_I_LicensesRecords.Text = dgv_I_Licenses.Rows.Count.ToString();


            ctrBasePersonCard1.LoadPersonInfo(ApplicantPersonID);
        }


        private void Fill_L_LicensesDataGridView(DataTable table)
        {
            dgv_L_Licenses.DataSource = null;
            dgv_L_Licenses.Rows.Clear();


            foreach (DataRow row in table.Rows)
            {
                //starting from index 1 because the first item is the id
                object[] array = new object[row.ItemArray.Length - 1];
                Array.Copy(row.ItemArray, 1, array, 0, row.ItemArray.Length - 1);

                dgv_L_Licenses.Rows.Add(array);
            }
        }
        private void Fill_I_LicensesDataGridView(DataTable table)
        {
            dgv_I_Licenses.DataSource = null;
            dgv_I_Licenses.Rows.Clear();


            foreach (DataRow row in table.Rows)
            {
                //starting from index 1 because the first item is the id
                object[] array = new object[row.ItemArray.Length - 1];
                Array.Copy(row.ItemArray, 1, array, 0, row.ItemArray.Length - 1);

                dgv_I_Licenses.Rows.Add(array);
            }
        }

        private void Load_L_LicensesListToGrid()
        {
            _DT = clsLicenses.LicensesList();

            DataView view = _DT.DefaultView;
            view.RowFilter = $"ApplicantPersonID = {ApplicantPersonID}";
            view.Sort = "ClassName desc, LicenseID desc";

            Fill_L_LicensesDataGridView(view.ToTable());
        }
        private void Load_I_LicensesListToGrid()
        {
            _DT = clsInternationalLicense.ListForPersonHistory();

            DataView view = _DT.DefaultView;

            if (_DT.Rows.Count > 0)
            {
                view.RowFilter = $"ApplicantPersonID = {ApplicantPersonID}";
                view.Sort = "InternationalLicenseID desc";

                Fill_I_LicensesDataGridView(view.ToTable());
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                int applicationID = (int)dgv_L_Licenses.CurrentRow.Cells[1].Value;

                frmLicenseInfo frm = new frmLicenseInfo(applicationID);
                frm.ShowDialog();
            }
            else if (dgv_I_Licenses.Rows.Count > 0)
            {
                int I_LicenseID = (int)dgv_I_Licenses.CurrentRow.Cells[0].Value;

                frm_I_LicenseInfo frm = new frm_I_LicenseInfo(I_LicenseID);
                frm.ShowDialog();

            }
        }
    }
}
