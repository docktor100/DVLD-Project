using BusinessLayer.Applications;
using BusinessLayer.Drivers___Licenses;
using DataAccessLayer.Drivers___Licenses;
using System;
using System.Data;

namespace BusinessLayer.Licenses
{
    public class clsLicenses
    {
        enum enMode { AddNew, Update }
        enMode Mode;

        public int LicenseID { get; set; }
        public clsApplication Application { get; set; }
        public clsDrivers Driver { get; set; }
        public clsLicenseClass LicenseClass { get; set; }
        public clsUsers CreatedByUser { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public float Paidfees { get; set; }
        public bool IsActive { get; set; }

        public enum enIssueReason { FirstTime = 1, Renew, ReplacementForDamaged, ReplacementForLost }
        public enIssueReason IssueReason { get; set; }

        public clsLicenses(int licenseID, int applicationID, int driverID, int licenseClassID,
            int createdByUserID, DateTime issueDate, DateTime expirationDate, string notes,
            float paidfees, bool isActive, byte issueReason)
        {
            LicenseID = licenseID;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            Notes = notes;
            Paidfees = paidfees;
            IsActive = isActive;
            IssueReason = (enIssueReason)issueReason;

            Application = clsApplication.Find(applicationID);
            Driver = clsDrivers.FindByDriverID(driverID);
            LicenseClass = clsLicenseClass.Find(licenseClassID);
            CreatedByUser = clsUsers.Find(createdByUserID);

            Mode = enMode.Update;
        }

        public clsLicenses()
        {
            LicenseID = default;
            Application = default;
            Driver = new clsDrivers();
            LicenseClass = default;
            CreatedByUser = default;
            IssueDate = default;
            ExpirationDate = default;
            Notes = default;
            Paidfees = default;
            IsActive = default;
            IssueReason = default;

            Mode = enMode.AddNew;
        }

        public static clsLicenses FindByLicenseID(int licenseID)
        {
            if (clsLicensesData.FindByLicenseID(licenseID, out int applicationID, out int driverID, out int licenseClassID,
            out int createdByUserID, out DateTime issueDate, out DateTime expirationDate, out string notes,
            out float paidfees, out bool isActive, out byte issueReason))
            {
                return new clsLicenses(licenseID, applicationID, driverID, licenseClassID, createdByUserID, issueDate, expirationDate, notes, paidfees, isActive, issueReason);
            }
            else
                return null;
        }
        public static clsLicenses FindByAppID(int applicationID)
        {
            if (clsLicensesData.FindByAppID(applicationID, out int licenseID, out int driverID, out int licenseClassID,
            out int createdByUserID, out DateTime issueDate, out DateTime expirationDate, out string notes,
            out float paidfees, out bool isActive, out byte issueReason))
            {
                return new clsLicenses(licenseID, applicationID, driverID, licenseClassID,
                                        createdByUserID, issueDate, expirationDate, notes,
                                        paidfees, isActive, issueReason);
            }
            else
                return null;
        }

        public string GetIssueReasonString()
        {
            switch (IssueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";

                case enIssueReason.Renew:
                    return "Renew";

                case enIssueReason.ReplacementForDamaged:
                    return "Replacement For Damaged";

                case enIssueReason.ReplacementForLost:
                    return "Replacement For Lost";

                default:
                    return "";
            }
        }

        public static bool DoesHaveLicense(int ApplicationID)
        {
            return clsLicensesData.DoesHaveLicense(ApplicationID);
        }

        public static DataTable LicensesList()
        {
            return clsLicensesData.LicensesList();
        }

        private bool _AddNew()
        {
            LicenseID = clsLicensesData.AddNew(Application.ApplicationID, Driver.DriverID, LicenseClass.LicenseClassID,
                CreatedByUser.UserID, IssueDate, ExpirationDate, Notes, Paidfees, IsActive, (byte)IssueReason);

            return LicenseID != -1;
        }

        public bool Activate()
        {
            return clsLicensesData.Activate(LicenseID);
        }
        public bool Deactivate()
        {
            return clsLicensesData.Deactivate(LicenseID);
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

        public bool IsDetained()
        {
            return clsDetainedLicenses.IsLicenseDetained(LicenseID);
        }
    }
}
