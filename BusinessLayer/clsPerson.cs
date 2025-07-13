using BusinessLayer.Applications;
using DataAccessLayer;
using System;
using System.Data;

namespace BusinessLayer
{
    public class clsPerson
    {
        public enum enMode { AddNew, Update }
        enMode Mode;

        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                if (!string.IsNullOrEmpty(ThirdName))
                    ThirdName += " ";

                return $"{FirstName} {SecondName} {ThirdName}{LastName}";
            }
        }
        public DateTime DateOfBirth { get; set; }
        public bool Gendor { get; set; } // 0 stands for male, 1 stands for female
        public string NationalNO { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ImageName { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }

        public clsPerson()
        {
            PersonID = -1;
            FirstName = "";
            SecondName = "";
            ThirdName = "";
            LastName = "";
            DateOfBirth = DateTime.Now;
            Gendor = false;
            NationalNO = "";
            Address = "";
            Phone = "";
            Email = "";
            ImageName = "";
            CountryID = -1;
            CountryName = "";

            Mode = enMode.AddNew;
        }
        public clsPerson(int id, string firstName, string secondName, string thirdName,
        string lastName, string nationalNO, DateTime dateOfBirth, bool gendor,
        string address, string phone, string email, int countryID, string imageName)
        {
            PersonID = id;
            FirstName = firstName;
            SecondName = secondName;
            ThirdName = thirdName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            NationalNO = nationalNO;
            Address = address;
            Gendor = gendor;
            Phone = phone;
            Email = email;
            ImageName = imageName;
            CountryID = countryID;
            CountryName = clsPersonData.GetCountryName(countryID);

            Mode = enMode.Update;
        }


        protected bool _AddNewPerson()
        {
            PersonID = clsPersonData.AddNewPerson(FirstName, SecondName, ThirdName, LastName,
                                        NationalNO, DateOfBirth, Gendor, Address, Phone,
                                        Email, CountryID, ImageName);

            return (PersonID != -1);
        }
        protected bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(PersonID, FirstName, SecondName, ThirdName, LastName,
                                              NationalNO, DateOfBirth, Gendor, Address, Phone,
                                              Email, CountryID, ImageName);
        }
        public static bool DeletePerson(int ID)
        {
            return clsPersonData.DeletePerson(ID);
        }

        static public clsPerson Find(int ID)
        {
            string firstName; string secondName; string thirdName;
            string lastName; string nationalNO; DateTime dateOfBirth; bool gendor;
            string address; string phone; string email; int countryID; string imageName;


            if (
                clsPersonData.FindByID(ID, out firstName, out secondName, out thirdName, out lastName,
                                        out nationalNO, out dateOfBirth, out gendor, out address,
                                        out phone, out email, out countryID, out imageName))
            {

                return new clsPerson(ID, firstName, secondName, thirdName, lastName,
                                         nationalNO, dateOfBirth, gendor, address,
                                         phone, email, countryID, imageName);
            }
            else
                return null;
        }
        static public clsPerson Find(string nationalNO)
        {
            int id;
            string firstName; string secondName; string thirdName;
            string lastName; DateTime dateOfBirth; bool gendor;
            string address; string phone; string email; int countryID; string imagePath;


            if (
                clsPersonData.FindByNationalNo(nationalNO, out id, out firstName, out secondName, out thirdName, out lastName,
                                        out dateOfBirth, out gendor, out address,
                                        out phone, out email, out countryID, out imagePath)
               )
                return new clsPerson(id, firstName, secondName, thirdName, lastName,
                                         nationalNO, dateOfBirth, gendor, address,
                                         phone, email, countryID, imagePath);
            else
                return null;
        }
        static public bool IsExist(int ID)
        {
            return clsPersonData.IsExist(ID);
        }
        static public bool IsExist(string NationalNo)
        {
            return clsPersonData.IsExist(NationalNo);
        }
        static public DataTable PeopleListForDataGrid()
        {
            return clsPersonData.PeopleListForDataGrid();
        }

        static public string GetCountryName(int id)
        {
            return clsPersonData.GetCountryName(id);
        }
        static public int GetCountryID(string countryName)
        {
            return clsPersonData.GetCountryID(countryName);
        }

        static public bool DoesHaveRelatedData(int personID)
        {
            if (clsUsers.IsExist(personID) || clsApplication.IsApplicantPersonExist(personID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static public DataTable ListCountries()
        {
            return clsPersonData.GetCountriesList();
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdatePerson();

                default:
                    return false;
            }
        }
    }
}
