using BusinessLayer;
using System;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms.Applications
{
    public partial class frmEditApplicationType : Form
    {
        int ApplicationTypeID;
        clsApplicationTypes ApplicationType = null;

        public frmEditApplicationType(int applicationtypeid)
        {
            InitializeComponent();
            ApplicationTypeID = applicationtypeid;
        }

        private void frmEditApplicationType_Load(object sender, EventArgs e)
        {
            ApplicationType = clsApplicationTypes.Find(ApplicationTypeID);

            lblApplicationTypeID.Text = ApplicationType.ApplicationTypeID.ToString();
            tbTitle.Text = ApplicationType.Title;
            mbFees.Text = ApplicationType.Fee.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ApplicationType.Title = tbTitle.Text.Trim();
            ApplicationType.Fee = Convert.ToSingle(mbFees.Text);

            if (ApplicationType.Update())
            {
                MessageBox.Show("Application Type Updated Successfully", "Update Application Type");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
