using DataAccessLayer.Drivers___Licenses;
using System;
using System.Data;

namespace BusinessLayer.Drivers___Licenses
{
    public class clsDrivers
    {
        enum enMode { AddNew, Update }
        enMode Mode;
        public int DriverID { get; set; }
        public clsPerson Person { get; set; }
        public clsUsers CreatedByUser { get; set; }
        public DateTime CreateDate { get; set; }

        public clsDrivers(int driverID, int personID, int userID, DateTime date)
        {
            DriverID = driverID;
            Person = clsPerson.Find(personID);
            CreatedByUser = clsUsers.Find(userID);
            CreateDate = date;
            Mode = enMode.Update;
        }
        public clsDrivers()
        {
            DriverID = default;
            Person = default;
            CreatedByUser = default;
            CreateDate = default;
            Mode = enMode.AddNew;
        }


        public static clsDrivers FindByDriverID(int driverID)
        {
            if (clsDriversData.FindByDriverID(driverID, out int personID, out int userID, out DateTime date))
            {
                return new clsDrivers(driverID, personID, userID, date);
            }
            else
            {
                return null;
            }
        }
        public static clsDrivers FindByPersonID(int personID)
        {
            if (clsDriversData.FindByPersonID(personID, out int driverID, out int userID, out DateTime date))
            {
                return new clsDrivers(driverID, personID, userID, date);
            }
            else
            {
                return null;
            }
        }


        public static DataTable DriversList()
        {
            return clsDriversData.DriversList();
        }

        public static bool IsExist(int personID)
        {
            return clsDriversData.IsExist(personID);
        }

        private bool _AddNew()
        {
            DriverID = clsDriversData.AddNew(Person.PersonID, CreatedByUser.UserID, CreateDate);

            return DriverID != -1;
        }
        public bool Save()
        {
            if (_AddNew())
            {
                Mode = enMode.Update;
                return true;
            }
            else
                return false;
        }

    }
}
