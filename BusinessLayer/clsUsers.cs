using BusinessLayer.Applications;
using DataAccessLayer;
using System.Data;

namespace BusinessLayer
{
    public class clsUsers
    {
        public enum enMode { AddNew, Update }
        enMode Mode = enMode.AddNew;

        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public clsPerson Person { get; set; }

        public clsUsers()
        {
            UserID = default;
            UserName = default;
            Password = default;
            IsActive = default;

            Person = null;
            Mode = enMode.AddNew;
        }
        public clsUsers(int userID, int personID, string userName, string password, bool isActive)
        {
            UserID = userID;
            UserName = userName;
            Password = password;
            IsActive = isActive;

            Person = clsPerson.Find(personID);
            Mode = enMode.Update;
        }

        private bool _UpdatePerson()
        {
            return clsUsersData.UpdateUser(UserID, Person.PersonID, UserName, Password, IsActive);
        }

        private bool _AddNewUser()
        {
            int userID = clsUsersData.AddNewUser(Person.PersonID, UserName, Password, IsActive);

            return (userID != -1);
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
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

        public static bool DeleteUser(int userID)
        {
            return clsUsersData.DeleteUser(userID);
        }
        public static bool IsExist(int personID)
        {
            return clsUsersData.IsExist(personID);
        }

        public static bool DoesHaveRelatedData(int userID)
        {
            return clsApplication.IsUserExist(userID);
        }

        public static clsUsers Find(int userID)
        {
            int personID = default;
            string userName = default, passWord = default;
            bool IsActive = default;

            if (clsUsersData.Find(userID, ref personID, ref userName, ref passWord, ref IsActive))
            {
                return new clsUsers(userID, personID, userName, passWord, IsActive);
            }
            else
                return null;
        }
        public static clsUsers Find(string userName)
        {
            int personID = default;
            int userID = default;
            string passWord = default;
            bool IsActive = default;

            if (clsUsersData.Find(userName, ref personID, ref userID, ref passWord, ref IsActive))
            {
                return new clsUsers(userID, personID, userName, passWord, IsActive);
            }
            else
                return null;
        }

        public static bool IsExist(string userName)
        {
            return clsUsersData.IsExist(userName);
        }
        public static DataTable GetUsersListForDataGrid()
        {
            return clsUsersData.GetUsersForDataGrid();
        }
    }
}
