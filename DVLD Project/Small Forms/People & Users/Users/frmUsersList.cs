using BusinessLayer;
using System.Data;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms
{
    public partial class frmUsersList : Form
    {
        //to save the original datagridview without any filtering
        private static DataTable _DT = new DataTable();

        public frmUsersList()
        {
            InitializeComponent();
        }

        private void FillDataGridView(DataTable table)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();


            foreach (DataRow row in table.Rows)
            {
                dataGridView1.Rows.Add(row.ItemArray);
            }
        }
        private void LoadUsersListToGrid()
        {
            cbFilter.SelectedIndex = 0;

            _DT = clsUsers.GetUsersListForDataGrid();
            FillDataGridView(_DT);
        }
        private void frmUsersList_Load(object sender, System.EventArgs e)
        {
            LoadUsersListToGrid();
            lblNumberOfRecords.Text = _DT.Rows.Count.ToString();
        }


        private void mbFilter_TextChanged(object sender, System.EventArgs e)
        {
            string column = cbFilter.Text;
            string filterText = mbFilter.Text.Trim();


            if (filterText != "")
                _DT.DefaultView.RowFilter = $"Convert([{column}], 'System.String') LIKE '{filterText}%'";
            else
                _DT.DefaultView.RowFilter = "";

            FillDataGridView(_DT.DefaultView.ToTable());
        }

        private void cbFilter_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (cbFilter.SelectedIndex == 0)
            {
                mbFilter.Text = "";
                mbFilter.Visible = false;
                FillDataGridView(_DT);
            }
            else if (cbFilter.SelectedIndex != 5)
            {

                cbIsActive.Visible = false;

                mbFilter.Visible = true;
                mbFilter.Focus();
            }
            else
            {
                mbFilter.Visible = false;

                cbIsActive.Visible = true;
                cbIsActive.SelectedIndex = 0;
            }


            switch (cbFilter.Text)
            {
                case "UserID":
                    mbFilter.Mask = "000000";
                    break;

                case "PersonID":
                    mbFilter.Mask = "000000";
                    break;

                default:
                    mbFilter.Mask = "";
                    break;
            }
        }


        private void btnAddNew_Click(object sender, System.EventArgs e)
        {
            frm_UpdateAddNewUser frm = new frm_UpdateAddNewUser();
            frm.ShowDialog();

            if (frm.DoUpdateGridView) LoadUsersListToGrid();
        }



        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            showDetailsToolStripMenuItem_Click(sender, e);
        }
        private void showDetailsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            int currentID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            frmBasicUserCard frm = new frmBasicUserCard(currentID);
            frm.ShowDialog();
        }
        private void addNewUserToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            frm_UpdateAddNewUser frm = new frm_UpdateAddNewUser();
            frm.ShowDialog();
            if (frm.DoUpdateGridView) LoadUsersListToGrid();
        }
        private void editToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            int currentID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            frm_UpdateAddNewUser frm = new frm_UpdateAddNewUser(currentID);
            frm.ShowDialog();

            if (frm.DoUpdateGridView) LoadUsersListToGrid();
        }
        private void deleteToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            int currentUserID = (int)dataGridView1.CurrentRow.Cells[0].Value;


            if (clsUsers.DoesHaveRelatedData(currentUserID))
            {
                MessageBox.Show("Cannot delete this user because there are related data to it",
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }


            string userName = dataGridView1.CurrentRow.Cells["UserName"].Value.ToString();

            DialogResult result = MessageBox.Show($"Are You Sure You Want To Delete User {userName}?", "Delete User", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }


            if (clsUsers.DeleteUser(currentUserID))
            {
                MessageBox.Show("User Deleted Successfully", "Delete User");
            }

            LoadUsersListToGrid();
        }
        private void changePasswordToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            int currentID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            frmChangeUserPassword frm = new frmChangeUserPassword(currentID);
            frm.ShowDialog();
        }

        private void cbIsActive_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string ColumnName = cbFilter.Text;

            switch (cbIsActive.Text)
            {
                case "yes":
                    _DT.DefaultView.RowFilter = $"{ColumnName} = 1";
                    break;

                case "No":
                    _DT.DefaultView.RowFilter = $"{ColumnName} = 0";
                    break;

                default:
                    _DT.DefaultView.RowFilter = "";

                    break;
            }

            FillDataGridView(_DT.DefaultView.ToTable());
        }

    }
}
