using BusinessLayer;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace DVLD_Project.Small_Forms
{
    public partial class frm_UpdateAddNewPerson : Form
    {
        int PersonID = -1;
        public clsPerson Person { get; set; } = null;
        string CurrentImagePath = default;

        bool _DoUpdatGridView = false;
        public bool DoUpdatGridView { get; } = false;

        enum enMode { AddNew, Update } //##
        enMode Mode;

        public delegate void DataBackEventHandler(clsPerson person);
        public event DataBackEventHandler dataBack;



        public frm_UpdateAddNewPerson()
        {
            InitializeComponent();
            PersonID = -1;
            Mode = enMode.AddNew;
        }
        public frm_UpdateAddNewPerson(int personID)
        {
            InitializeComponent();
            PersonID = personID;
            Mode = enMode.Update;
        }

        private void frm_Update_AddNew_Load(object sender, EventArgs e)
        {
            FillComboBoxWithCountries();


            if (Mode == enMode.Update)
            {
                LoadPersonData();
            }
            else
            {
                Person = new clsPerson();
                SetGendorImage(false);
            }


            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);


            if (Mode == enMode.AddNew)
            {
                this.Text = "Add New Person";
                lblTitle.Text = "Add New Person";
            }
            else
            {
                this.Text = "Update Person Info";
                lblTitle.Text = "Update Person Info";
            }

        }

        private void SetGendorImage(bool IsFemale)
        {
            if (IsFemale)
            {
                pbPersonImage.Image = Properties.Resources.Female_512;
            }
            else
            {
                pbPersonImage.Image = Properties.Resources.Male_512;
            }
        }


        //For Updating
        public void LoadPersonData()
        {
            Person = clsPerson.Find(PersonID);

            if (Person == null)
            {
                MessageBox.Show($"Person with ID = {PersonID} Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            SetGendorImage(true);

            llblRemoveImage.Visible = (Person.ImageName != "");

            LoadDataToForm();
        }
        private void LoadDataToForm()
        {
            lblPersonID.Text = Person.PersonID.ToString();
            tbFirstName.Text = Person.FirstName;
            tbSecondName.Text = Person.SecondName;
            tbThirdName.Text = (Person.ThirdName != "") ? Person.ThirdName : "";
            tbLastName.Text = Person.LastName;
            tbPhone.Text = Person.Phone;
            tbEmail.Text = (Person.Email != "") ? Person.Email : "";
            tbAddress.Text = Person.Address;
            dtpDateOfBirth.Text = Person.DateOfBirth.ToString();

            tbNationalNO.Text = Person.NationalNO;


            cbCountry.SelectedIndex = cbCountry.FindString(Person.CountryName);


            if (Person.ImageName != "")
            {
                CurrentImagePath = Path.Combine(Application.StartupPath, "DVLD People Images", Person.ImageName);

                if (File.Exists(CurrentImagePath))
                    pbPersonImage.ImageLocation = CurrentImagePath;
                else
                    MessageBox.Show("Could not find this image: = " + CurrentImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                llblRemoveImage.Visible = true;
            }
            else
            {
                CurrentImagePath = null;
                SetGendorImage(Person.Gendor);
            }

            if (!Person.Gendor)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;

        }
        //Fore Saving

        private void FillComboBoxWithCountries()
        {
            DataTable dtCountries = clsPerson.ListCountries();

            foreach (DataRow Row in dtCountries.Rows)
            {
                cbCountry.Items.Add(Row["CountryName"]);
            }

            if (Person != null)
                cbCountry.SelectedIndex = cbCountry.FindString(Person.CountryName);
        }



        private void Basics_Leave(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                ErrorProvider.SetError(tb, "Should Not be empty");
            }
        }
        private void Basics_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text.Length > 0)
            {
                ErrorProvider.SetError(tb, "");
            }


            if (tb == tbNationalNO && tb.Text != Person.NationalNO && clsPerson.IsExist(tb.Text))
            {
                ErrorProvider.SetError(tb, "User With This National Number Already Exists!.");
            }
        }

        private void CheckEmailValedity(object sender)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                ErrorProvider.SetError(tb, "");
                return;
            }

            if (!tb.Text.EndsWith(".Com", StringComparison.OrdinalIgnoreCase))
            {
                ErrorProvider.SetError(tb, "Invalid Email Address Format");
            }
            else
            {
                ErrorProvider.SetError(tb, "");
            }
        }
        private void tbEmail_Leave(object sender, EventArgs e)
        {
            CheckEmailValedity(sender);
        }
        private void tbEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                CheckEmailValedity(sender);
        }


        string NewImagePath = default;
        string selectedImagePath = default;
        private bool IsThereErrors()
        {
            foreach (Control c in gbPersonInformation.Controls)
            {
                if (!string.IsNullOrEmpty(ErrorProvider.GetError(c)))
                {
                    return true; // At least one error is set
                }
            }
            return false;
        }
        public void FillPersonData()
        {
            Person.FirstName = tbFirstName.Text;
            Person.SecondName = tbSecondName.Text;
            Person.ThirdName = tbThirdName.Text;
            Person.LastName = tbLastName.Text;
            Person.Email = tbEmail.Text;
            Person.Phone = tbPhone.Text;
            Person.Address = tbAddress.Text;
            Person.NationalNO = tbNationalNO.Text;
            Person.DateOfBirth = dtpDateOfBirth.Value;

            Person.CountryID = clsPerson.GetCountryID(cbCountry.Text);
            Person.CountryName = cbCountry.Text;

            if (NewImagePath != null)
            {
                Person.ImageName = Path.GetFileName(NewImagePath);
            }

        }
        private void HandlePersonImage()
        {
            // If the image is not changed, or the image got deleted, else do nothing
            if (NewImagePath != null || pbPersonImage.ImageLocation == null)
            {
                // if there was an image before, delete the old image file
                if (CurrentImagePath != null)
                {
                    try
                    {
                        File.Delete(CurrentImagePath);
                    }
                    catch (IOException)
                    {
                        // We could not delete the file.
                        //log it later   
                    }
                }


                // in case of not removing the image 
                if (selectedImagePath != null)
                {
                    File.Copy(selectedImagePath, NewImagePath, true);
                    Person.ImageName = Path.GetFileName(NewImagePath); // Save the new image name
                    CurrentImagePath = NewImagePath; // Update the current image path
                }
            }

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsThereErrors())
            {
                MessageBox.Show("Check Your Input", "Error");
                return;
            }

            FillPersonData();

            if (Person.Save())
            {
                MessageBox.Show("Save Succeeded");
                lblPersonID.Text = Person.PersonID.ToString();

                lblTitle.Text = "Update Person Info";
                dataBack?.Invoke(Person);

                HandlePersonImage();

                NewImagePath = null;
                selectedImagePath = null;
                if (Person.ImageName == "")
                {
                    CurrentImagePath = null;
                    llblRemoveImage.Visible = false;
                }

                Mode = enMode.Update;
                _DoUpdatGridView = true;
            }
            else
            {
                MessageBox.Show("Failed To Save Data", "ERROR");

            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void llblSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;

            string ImageExtension = default;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                selectedImagePath = openFileDialog1.FileName;

                // Prepare destination path
                ImageExtension = Path.GetExtension(selectedImagePath);
                string newFileName = Guid.NewGuid() + ImageExtension;

                string folderPath = Path.Combine(Application.StartupPath, "DVLD People Images");
                string newFilePath = Path.Combine(folderPath, newFileName);

                NewImagePath = newFilePath;

                // Load the image into the PictureBox 
                pbPersonImage.ImageLocation = selectedImagePath;
            }
        }
        private void llblRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;
            SetGendorImage(rbFemale.Checked);

            Person.ImageName = "";

            llblRemoveImage.Visible = false;
        }


        private void rbGendor_CheckedChanged(object sender, EventArgs e)
        {
            if (pbPersonImage.ImageLocation == null)
            {
                bool IsFemale = (!rbMale.Checked);
                SetGendorImage(IsFemale);
            }
        }
    }
}
