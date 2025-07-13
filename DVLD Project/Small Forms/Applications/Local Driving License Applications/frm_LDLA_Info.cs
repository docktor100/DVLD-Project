using System;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms.Applications
{
    public partial class frm_LDLA_Info : Form
    {
        public int LDLA_ID { get; set; }

        public frm_LDLA_Info(int _LDLA_ID)
        {
            InitializeComponent();
            LDLA_ID = _LDLA_ID;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLDLApplicationInfo_Load(object sender, EventArgs e)
        {
            ctrLDLApplicationInfo1.LoadDataToForm(LDLA_ID);
        }
    }
}
