using System;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms
{
    public partial class frmPersonDetails : Form
    {
        bool _DoUdpateGridView = false;
        public bool DoUdpateGridView { get { return _DoUdpateGridView; } }

        public frmPersonDetails(int ID)
        {
            InitializeComponent();
            ctrBasePersonCard1.LoadPersonInfo(ID);
        }
        public frmPersonDetails(string NationlNo)
        {
            InitializeComponent();
            ctrBasePersonCard1.LoadPersonInfo(NationlNo);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
