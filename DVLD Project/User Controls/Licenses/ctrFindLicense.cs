using BusinessLayer.Licenses;
using System;
using System.Windows.Forms;

namespace DVLD_Project.User_Controls
{
    public partial class ctrFindLicense : UserControl
    {
        public clsLicenses License { get; set; }

        public ctrFindLicense()
        {
            InitializeComponent();
        }

        public event Action<clsLicenses> OnSearch;

        public void DisableSearch()
        {
            gbFilter.Enabled = false;
        }
        public void LoadInfo(int licenseID)
        {
            License = clsLicenses.FindByLicenseID(licenseID);

            mbFilter.Text = licenseID.ToString();

            btnSearch.PerformClick();

            DisableSearch();
        }

        public void FilterFocus()
        {
            mbFilter.Focus();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (mbFilter.Text.Trim() == "")
            {
                return;
            }

            int licenseID = Convert.ToInt32(mbFilter.Text.Trim());
            License = clsLicenses.FindByLicenseID(licenseID);


            ctrLicenseInfo1.LoadDataToForm(License);

            OnSearch?.Invoke(License);
        }
    }
}
