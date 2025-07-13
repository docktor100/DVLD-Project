using BusinessLayer.Drivers___Licenses;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms.People___Users
{
    public partial class frmDriversList : Form
    {
        DataTable _DT;
        public frmDriversList()
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
        private void frmDriversList_Load(object sender, EventArgs e)
        {
            _DT = clsDrivers.DriversList();
            //LoadTable(_DT); //this is done in the select index change of the 

            comboBox1.SelectedIndex = 0;

            lblNumberOfRecords.Text = _DT.Rows.Count.ToString();
        }


        private void mbFilter_TextChanged(object sender, EventArgs e)
        {
            string column = comboBox1.Text;
            string filterText = mbFilter.Text.Trim();

            DataView dv = _DT.DefaultView;
            dv.RowFilter = $"Convert([{column}], 'System.String') LIKE '{filterText}%'";

            LoadTable(dv.ToTable());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            mbFilter.Text = "";

            if (comboBox1.SelectedIndex == 0)
            {
                mbFilter.Visible = false;
                LoadTable(_DT);
            }
            else
            {
                mbFilter.Visible = true;
                mbFilter.Focus();
            }


            if (comboBox1.Text == "DriverID" || comboBox1.Text == "PersonID")
            {
                mbFilter.Mask = "0000";
            }
            else if (comboBox1.Text == "NumberOfActiveLicenses")
            {
                mbFilter.Mask = "0";
            }
            else
            {
                mbFilter.Mask = "";
            }

        }
    }
}
