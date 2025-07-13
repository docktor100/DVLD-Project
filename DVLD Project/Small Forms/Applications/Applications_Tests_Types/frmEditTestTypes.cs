using BusinessLayer.Specific;
using System;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms.Applications
{
    public partial class frmEditTestTypes : Form
    {
        clsTestType TestType;
        int TestTypeID;
        public frmEditTestTypes(int testTypeID)
        {
            InitializeComponent();
            TestTypeID = testTypeID;
        }

        private void frmEditTestTypes_Load(object sender, EventArgs e)
        {
            TestType = clsTestType.Find(TestTypeID);

            lblTestTypeID.Text = TestType.TestTypeID.ToString();
            tbTitle.Text = TestType.Title;
            tbDescription.Text = TestType.Description;
            mbFees.Text = TestType.Fee.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            TestType.Title = tbTitle.Text.Trim();
            TestType.Fee = Convert.ToSingle(mbFees.Text);

            if (TestType.Update())
            {
                MessageBox.Show("Test Type Updated Successfully", "Update Test Type");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
