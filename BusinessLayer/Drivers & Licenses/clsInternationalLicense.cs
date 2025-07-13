using BusinessLayer.Applications;
using BusinessLayer.Licenses;
using DataAccessLayer.Drivers___Licenses;
using System;
using System.Data;

namespace BusinessLayer.Drivers___Licenses
{
    public class clsInternationalLicense
    {
        public int I_LicenseID { get; set; }
        public clsApplication Application { get; set; }
        public clsDrivers Driver { get; set; }
        public clsLicenses IssuedUsing_LDL { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public clsUsers CreatedByUser { get; set; }

        clsInternationalLicense(int internationalLicenseID, int applicationID, int driverID,
                                int issuedUsing_LDL_ID, DateTime issueDate, DateTime expirationDate,
                                bool isActive, int createdByUserID)
        {
            I_LicenseID = internationalLicenseID;
            Application = clsApplication.Find(applicationID);
            Driver = clsDrivers.FindByDriverID(driverID);
            IssuedUsing_LDL = clsLicenses.FindByLicenseID(issuedUsing_LDL_ID);
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            IsActive = isActive;
            CreatedByUser = clsUsers.Find(createdByUserID);

        }

        public clsInternationalLicense()
        {
            I_LicenseID = default;
            Application = new clsApplication();
            Driver = default;
            IssuedUsing_LDL = default;
            IssueDate = default;
            ExpirationDate = default;
            IsActive = default;
            CreatedByUser = default;
        }


        public static clsInternationalLicense FindBy_IDL_ID(int internationalLicenseID)
        {
            if (clsInternationalLicenseData.FindBy_IDL_ID(internationalLicenseID, out int applicationID, out int driverID,
                                                  out int issuedUsing_LDL_ID, out DateTime issueDate, out DateTime expirationDate,
                                                  out bool isActive, out int createdByUserID))
            {
                return new clsInternationalLicense(internationalLicenseID, applicationID,
                                                   driverID, issuedUsing_LDL_ID, issueDate,
                                                   expirationDate, isActive, createdByUserID);
            }
            else
            {
                return null;
            }
        }
        public static clsInternationalLicense FindBy_LDL_ID(int issuedUsing_LDL_ID)
        {
            if (clsInternationalLicenseData.FindBy_LDL_ID(issuedUsing_LDL_ID, out int applicationID, out int driverID,
                                                  out int internationalLicenseID, out DateTime issueDate, out DateTime expirationDate,
                                                  out bool isActive, out int createdByUserID))
            {
                return new clsInternationalLicense(internationalLicenseID, applicationID,
                                                   driverID, issuedUsing_LDL_ID, issueDate,
                                                   expirationDate, isActive, createdByUserID);
            }
            else
            {
                return null;
            }
        }

        public static DataTable ListForPersonHistory()
        {
            return clsInternationalLicenseData.ListForPersonHistory();
        }
        public static DataTable ListForManage()
        {
            return clsInternationalLicenseData.ListForManage();
        }

        public static bool IsExist_Active(int LDL_ID)
        {
            return clsInternationalLicenseData.IsExist_Active(LDL_ID);
        }

        private void AddNewApplication()
        {
            Application.ApplicantPerson = IssuedUsing_LDL.Application.ApplicantPerson;
            Application.CreatedByUser = CreatedByUser;
            Application.Date = DateTime.Now;
            Application.LastStatusDate = DateTime.Now;

            clsApplicationTypes appType = clsApplicationTypes.Find(6); // for new international License
            Application.ApplicationType = appType;
            Application.PaidFees = appType.Fee;
            Application.Status = clsApplication.enStatus.Completed;

            if (!Application.Save())
            {
                throw new Exception("Failed to save the application");
            }
        }
        public bool _AddNew()
        {
            AddNewApplication();


            I_LicenseID = clsInternationalLicenseData.AddNew(Application.ApplicationID,
                                     Driver.DriverID, IssuedUsing_LDL.LicenseID, IssueDate,
                                     ExpirationDate, IsActive, CreatedByUser.UserID);


            return I_LicenseID != -1;
        }


        public bool Save()
        {
            return _AddNew();
        }
    }
}
