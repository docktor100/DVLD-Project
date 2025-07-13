using BusinessLayer;
using DVLD_Project.Small_Forms;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmPeopleList : Form
    {
        //to save the original datagridview without any filtering
        private static DataTable _DT = new DataTable();

        public frmPeopleList()
        {
            InitializeComponent();
        }

        private void LoadLDLAListToGrid()
        {

            _DT = clsPerson.PeopleListForDataGrid();

            dataGridView1.DataSource = _DT;
        }
        private void AdjustDataGridViewColumns()
        {
            //##

            dataGridView1.Columns[0].Width = 70;
            dataGridView1.Columns[1].Width = 70;
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[4].Width = 100;
            dataGridView1.Columns[5].Width = 100;
            dataGridView1.Columns[6].Width = 60;
            dataGridView1.Columns[7].Width = 120;
            dataGridView1.Columns[8].Width = 90;
            dataGridView1.Columns[9].Width = 120;
            dataGridView1.Columns[10].Width = 170;

        }
        private void frmPeopleList_Load(object sender, EventArgs e)
        {
            LoadLDLAListToGrid();
            comboBox1.SelectedIndex = 0;

            lblNumberOfRecords.Text = _DT.Rows.Count.ToString();


            if (_DT.Rows.Count > 0) AdjustDataGridViewColumns();
        }


        private void mbFilter_TextChanged(object sender, EventArgs e)
        {
            string column = comboBox1.Text;
            string filterText = mbFilter.Text.Trim();

            DataView dv = _DT.DefaultView;
            dv.RowFilter = $"Convert([{column}], 'System.String') LIKE '{filterText}%'";
            //Convert([{column}], 'System.String') for numeric columns to treat them as strings for filtering

            dataGridView1.DataSource = dv;

        }
        private void SetMask()
        {
            switch (comboBox1.Text)
            {
                case "PersonID":
                    mbFilter.Mask = "000000";
                    break;

                case "Gendor":
                    mbFilter.Mask = "?";
                    break;


                default:
                    mbFilter.Mask = "";
                    break;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            mbFilter.Text = "";

            if (comboBox1.SelectedIndex == 0)
            {
                mbFilter.Visible = false;
                dataGridView1.DataSource = _DT;
            }
            else
            {
                mbFilter.Visible = true;
                mbFilter.Focus();
            }

            SetMask();
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frm_UpdateAddNewPerson frm = new frm_UpdateAddNewPerson();
            frm.ShowDialog();

            if (frm.DoUpdatGridView) { LoadLDLAListToGrid(); }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = (int)dataGridView1.CurrentRow.Cells[0].Value;
            frmPersonDetails frm = new frmPersonDetails(id);
            frm.ShowDialog();
            LoadLDLAListToGrid();
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int CurrentID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            frm_UpdateAddNewPerson frm = new frm_UpdateAddNewPerson(CurrentID);
            frm.ShowDialog();

            if (frm.DoUpdatGridView) { LoadLDLAListToGrid(); }
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int currentPersonID = (int)dataGridView1.CurrentRow.Cells[0].Value;


            if (clsPerson.DoesHaveRelatedData(currentPersonID))
            {
                MessageBox.Show("Cannot delete this peron because he has related data to him",
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }


            DialogResult Result = MessageBox.Show($"Are You Sure You Want to Delete Person With PersonID = {currentPersonID} ?", ""
                                                    , MessageBoxButtons.YesNo);

            if (Result == DialogResult.No)
            {
                return;
            }


            if (clsPerson.DeletePerson(currentPersonID))
            {
                MessageBox.Show("Person Deleted Successfully");
                LoadLDLAListToGrid();

                DeletePersonImage(clsPerson.Find(currentPersonID).ImageName);
            }
            else
            {
                MessageBox.Show("Failed To Delete Person");
            }

        }
        private void DeletePersonImage(string imageName)
        {
            string imagePath = Path.Combine(Application.StartupPath, "DVLD People Images", imageName);
            try
            {
                File.Delete(imagePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        { // from gbt to enhance the UI
            if (e.Value == null || e.Value == DBNull.Value) // if the value was null 
            {
                e.Value = "NULL";         // Display string null
                e.FormattingApplied = true; // Tells DataGridView you handled it
            }
        }
    }
}
