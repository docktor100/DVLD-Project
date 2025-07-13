using DataAccessLayer.Applications;
using System;
using System.Data;

namespace BusinessLayer.Applications
{
    public class clsApplication
    {
        protected enum enMode { AddNew, Update }
        protected enMode Mode { get; set; }

        public int ApplicationID { get; set; }
        public clsPerson ApplicantPerson { get; set; }
        public DateTime Date { get; set; }
        public clsApplicationTypes ApplicationType { get; set; }
        public DateTime LastStatusDate { get; set; }
        public float PaidFees { get; set; }
        public clsUsers CreatedByUser { get; set; }

        public enum enStatus { New = 1, Cancelled, Completed }
        public enStatus Status { get; set; }



        public clsApplication(int iD, int personID, DateTime date, int applicatinoTypeID,
                              DateTime lastStatusDate, float paidFees, byte status, int createdByUserID)
        {
            ApplicationID = iD;
            Date = date;
            CreatedByUser = clsUsers.Find(createdByUserID);
            ApplicantPerson = clsPerson.Find(personID);
            ApplicationType = clsApplicationTypes.Find(applicatinoTypeID);
            LastStatusDate = lastStatusDate;
            PaidFees = paidFees;
            Status = (enStatus)status;
            Mode = enMode.Update;
        }

        public clsApplication()
        {
            ApplicationID = default;
            ApplicantPerson = null;
            Date = default;
            ApplicationType = null;
            LastStatusDate = default;
            PaidFees = default;
            Status = default;
            Mode = enMode.AddNew;

        }
        public static bool IsApplicantPersonExist(int personID)
        {
            return clsApplicationsData.IsApplicantPersonExist(personID);
        }
        public static bool IsUserExist(int userID)
        {
            return clsApplicationsData.IsUserExist(userID);
        }

        public static clsApplication Find(int applicationID)
        {
            if (clsApplicationsData.Find(applicationID, out int personID, out DateTime date,
                                         out int applicatinoTypeID, out DateTime lastStatusDate,
                                         out float paidFees, out byte status, out int createdByUserID))
            {
                return new clsApplication(applicationID, personID, date, applicatinoTypeID, lastStatusDate, paidFees, status, createdByUserID);
            }
            else
            {
                return null;
            }
        }

        public static bool Delete(int applicationID)
        {
            return clsApplicationsData.Delete(applicationID);
        }


        public static DataTable LicenseClassesList()
        {
            return clsApplicationsData.LicenseClassList();
        }


        private bool _AddNew()
        {
            ApplicationID = clsApplicationsData.AddNew(ApplicantPerson.PersonID, Date, ApplicationType.ApplicationTypeID,
                                                   LastStatusDate, PaidFees, (byte)Status, CreatedByUser.UserID);

            return ApplicationID != -1;
        }
        private bool _Update()
        {
            return clsApplicationsData.Update(ApplicationID, (byte)Status);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNew())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }


                case enMode.Update:
                    return _Update();

                default:
                    return false;
            }
        }
    }
}
