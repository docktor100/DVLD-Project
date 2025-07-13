using BusinessLayer.Specific;
using DVLD_Project.Small_Forms.Applications;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms
{
    public partial class frmManageTestTypes : Form
    {
        public frmManageTestTypes()
        {
            InitializeComponent();
        }

        private void LoadDataToGridView()
        {
            DataTable dt = clsTestType.GetApplicationsList();

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
            }

            foreach (DataRow row in dt.Rows)
            {
                dataGridView1.Rows.Add(row.ItemArray);
            }

            lblRecords.Text = dataGridView1.Rows.Count.ToString();
        }
        private void frmManageTestTypes_Load(object sender, EventArgs e)
        {
            LoadDataToGridView();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int applicationID = (int)dataGridView1.CurrentRow.Cells[0].Value;

            frmEditTestTypes frm = new frmEditTestTypes(applicationID);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadDataToGridView();
            }
        }
    }
}
