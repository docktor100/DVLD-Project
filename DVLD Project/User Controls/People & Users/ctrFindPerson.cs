using BusinessLayer;
using DVLD_Project.Small_Forms;
using System;
using System.Windows.Forms;

namespace DVLD_Project.User_Controls
{
    public partial class ctrFindPerson : UserControl
    {
        public int PersonID { get; set; }

        public clsPerson SelectedPersonInfo
        {
            get { return ctrBasePersonCard1.Person; }
        }
        //##

        private bool _ShowAddPerson = true; //##
        public bool ShowAddPerson
        {
            get
            {
                return _ShowAddPerson;
            }
            set
            {
                _ShowAddPerson = value;
                btnAddNew.Visible = _ShowAddPerson;
            }
        }

        private bool _FilterEnabled = true;//##
        public bool FilterEnabled
        {
            get
            {
                return _FilterEnabled;
            }
            set
            {
                _FilterEnabled = value;
                mbFind.Enabled = _FilterEnabled;
            }
        }

        public event Action<int> OnPersonSelected;

        public ctrFindPerson()
        {
            InitializeComponent();
        }

        private void ctrFindPerson_Load(object sender, EventArgs e)
        {
            cbFilter.SelectedIndex = 0;
            mbFind.Focus();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.SelectedIndex == 1)
                mbFind.Mask = "0000000";
            else
                mbFind.Mask = "";

            mbFind.Text = "";
            mbFind.Focus();
        }


        private void AddNewDataBackHandler(clsPerson Person)
        {
            mbFind.Text = Person.PersonID.ToString();
            ctrBasePersonCard1.LoadPersonInfo(Person.PersonID);
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frm_UpdateAddNewPerson frm = new frm_UpdateAddNewPerson(-1);
            frm.dataBack += AddNewDataBackHandler;
            frm.ShowDialog();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(mbFind.Text.Trim()))
            {
                MessageBox.Show("Please enter a value to search for.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //if there are a selected person, and the searched perosn is the same as the current person
            if (mbFind.Text == SelectedPersonInfo?.PersonID.ToString() || mbFind.Text == SelectedPersonInfo?.NationalNO)
            {
                return;
            }

            if (cbFilter.Text == "Person ID")
            {
                PersonID = int.Parse(mbFind.Text.Trim());
                ctrBasePersonCard1.LoadPersonInfo(PersonID);
            }
            else
            {
                ctrBasePersonCard1.LoadPersonInfo(mbFind.Text.Trim());
            }

            mbFind.Focus();

            OnPersonSelected?.Invoke(PersonID);
        }

        public void LoadPersonInfo(int personID)
        {
            PersonID = personID;
            ctrBasePersonCard1.LoadPersonInfo(personID);

            mbFind.Text = personID.ToString();
            groupBox1.Enabled = false;
        }

        public void ResetForm()
        {
            ctrBasePersonCard1.ResetForm();
            cbFilter.SelectedIndex = 0;
            mbFind.Text = default;
        }

        public void FilterFocus()
        {
            mbFind.Focus();
        }
        private void mbFind_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btnSearch.PerformClick();
            }
        }
    }
}
