using BusinessLayer;
using DVLD_Project.Properties;
using DVLD_Project.Small_Forms;
using System.IO;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class ctrBasePersonCard : UserControl
    {
        public clsPerson Person;
        public int ID;
        public string NationalNo;



        public ctrBasePersonCard()
        {
            InitializeComponent();
        }


        private void SetGendorImage(bool IsFemale)
        {
            if (IsFemale)
            {
                pbPersonImage.Image = Resources.Female_512;
            }
            else
            {
                pbPersonImage.Image = Resources.Male_512;
            }
        }
        private void FillFormInfo()
        {
            if (Person == null)
                return;

            lblPersonID.Text = Person.PersonID.ToString();
            lblName.Text = Person.FullName;
            lblPhone.Text = Person.Phone;
            lblEmail.Text = (Person.Email != "") ? Person.Email : "Null";
            lblAddress.Text = Person.Address;
            lblDateOfBirth.Text = Person.DateOfBirth.ToShortDateString();
            lblNationalNo.Text = Person.NationalNO;

            lblGendor.Text = (!Person.Gendor) ? "Male" : "Female";
            lblCountry.Text = Person.CountryName;

            if (Person.ImageName != "")
            {
                string imagePath = Path.Combine(Application.StartupPath, "DVLD People Images", Person.ImageName);

                if (File.Exists(imagePath))
                    pbPersonImage.ImageLocation = imagePath;
                else
                    MessageBox.Show("Could not find this image: = " + imagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SetGendorImage(Person.Gendor);
            }

            linkLabel1.Enabled = true;
        }
        public void ResetForm()
        {
            lblPersonID.Text = "[???]";
            lblName.Text = "[???]";
            lblPhone.Text = "[???]";
            lblEmail.Text = "[???]";
            lblAddress.Text = "[???]";
            lblDateOfBirth.Text = "[???]";
            lblNationalNo.Text = "[???]";

            lblGendor.Text = "[???]";
            lblCountry.Text = "[???]";

            linkLabel1.Enabled = false;

            SetGendorImage(false);
        }

        public void LoadPersonInfo(clsPerson person)
        {
            if (person != null)
            {
                Person = person;
                FillFormInfo();
            }
            else
            {
                ResetForm();
            }
        }
        public void LoadPersonInfo(int id)
        {
            Person = null;
            ID = id;

            Person = clsPerson.Find(ID);

            if (Person != null)
            {
                FillFormInfo();
            }
            else
            {
                ResetForm();
            }
        }
        public void LoadPersonInfo(string naitionalno)
        {
            Person = null;
            NationalNo = naitionalno;

            if (naitionalno != "")
            {
                Person = clsPerson.Find(naitionalno);
            }

            if (Person != null)
            {
                FillFormInfo();
            }
            else
            {
                ResetForm();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frm_UpdateAddNewPerson frm = new frm_UpdateAddNewPerson(Person.PersonID);
            frm.dataBack += frm_Update_AddNew_DataBack;

            frm.ShowDialog();
            //Person = frm.Person;
        }
        private void frm_Update_AddNew_DataBack(clsPerson person)
        {
            Person = person;
            FillFormInfo();
        }

    }
}
