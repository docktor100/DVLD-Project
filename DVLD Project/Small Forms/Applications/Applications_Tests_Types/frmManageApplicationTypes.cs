using BusinessLayer;
using DVLD_Project.Small_Forms.Applications;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms
{
    public partial class frmManageApplicationTypes : Form
    {
        public frmManageApplicationTypes()
        {
            InitializeComponent();
        }

        private void LoadDataToGridView()
        {
            DataTable dt = clsApplicationTypes.GetApplicationsList();
            dataGridView1.DataSource = dt;

            lblRecords.Text = dataGridView1.Rows.Count.ToString();
        }
        private void frmManageApplicationTypes_Load(object sender, EventArgs e)
        {
            LoadDataToGridView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int applicationID = (int)dataGridView1.CurrentRow.Cells[0].Value;

            frmEditApplicationType frm = new frmEditApplicationType(applicationID);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadDataToGridView();
            }
        }
    }
}
