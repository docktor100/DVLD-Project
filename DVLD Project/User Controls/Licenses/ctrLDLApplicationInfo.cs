using BusinessLayer.Specific;
using DVLD_Project.Small_Forms;
using System.Windows.Forms;

namespace DVLD_Project.User_Controls
{
    public partial class ctr_LDL_ApplicationInfo : UserControl
    {


        public ctr_LDL_ApplicationInfo()
        {
            InitializeComponent();
        }

        cls_LDLA _LDLA;
        public cls_LDLA LDLA { get { return _LDLA; } }

        public void LoadDataToForm(int LDLA_ID)
        {
            _LDLA = cls_LDLA.Find(LDLA_ID);

            lbl_LDLA_ID.Text = LDLA_ID.ToString();
            lbl_LDLApplicationType.Text = _LDLA.LicenseClass.ClassName;
            lblPassedTests.Text = $"{_LDLA.PassedTests}/3";


            lblApplicationID.Text = _LDLA.ApplicationID.ToString();
            lblStatus.Text = ((byte)_LDLA.Status).ToString();
            lblDate.Text = _LDLA.Date.ToShortDateString();
            lblLastStatusDate.Text = _LDLA.LastStatusDate.ToShortDateString();
            lblCreatedBy.Text = _LDLA.CreatedByUser.UserName;
            lblApplicantPersonName.Text = _LDLA.ApplicantPerson.FullName;
            lblApplicationType.Text = _LDLA.ApplicationType.Title;
            lblFees.Text = _LDLA.PaidFees.ToString();

        }
        public void LoadDataToForm(cls_LDLA lDLA)
        {
            _LDLA = lDLA;


            lbl_LDLA_ID.Text = _LDLA.LDLA_ID.ToString();
            lbl_LDLApplicationType.Text = _LDLA.LicenseClass.ClassName;
            lblPassedTests.Text = $"{_LDLA.PassedTests}/3";


            lblApplicationID.Text = _LDLA.ApplicationID.ToString();
            lblStatus.Text = ((byte)_LDLA.Status).ToString();
            lblDate.Text = _LDLA.Date.ToShortDateString();
            lblLastStatusDate.Text = _LDLA.LastStatusDate.ToShortDateString();
            lblCreatedBy.Text = _LDLA.CreatedByUser.UserName;
            lblApplicantPersonName.Text = _LDLA.ApplicantPerson.FullName;
            lblApplicationType.Text = _LDLA.ApplicationType.Title;
            lblFees.Text = _LDLA.PaidFees.ToString();
        }

        public void SetIfPassed(bool isPassed)
        {
            if (isPassed)
            {
                _LDLA.RefreshPassedTests();
                lblPassedTests.Text = $"{_LDLA.PassedTests}/3";
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonDetails frm = new frmPersonDetails(_LDLA.ApplicantPerson.PersonID);
            frm.ShowDialog();
        }
    }
}
