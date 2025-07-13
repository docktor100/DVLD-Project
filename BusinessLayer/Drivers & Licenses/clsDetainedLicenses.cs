using BusinessLayer.Applications;
using BusinessLayer.Licenses;
using DataAccessLayer.Drivers___Licenses;
using System;
using System.Data;

namespace BusinessLayer.Drivers___Licenses
{
    public class clsDetainedLicenses
    {
        enum enMode { AddNew, Update }
        enMode Mode;

        public int DetainID { get; set; }
        public clsLicenses License { get; set; }
        public DateTime DetainDate { get; set; }
        public float FineFees { get; set; }
        public clsUsers CreatedByUser { get; set; }

        public bool IsReleased { get; set; }
        public DateTime ReleaseDate { get; set; }
        public clsUsers ReleasedByUser { get; set; }
        public clsApplication ReleaseApplication { get; set; }

        public clsDetainedLicenses(int detainID, int licenseID, DateTime detainDate, float fineFees,
                            int createdByUserID, bool isReleased, DateTime releaseDate,
                            int releasedByUserID, int releaseApplicationID)
        {
            DetainID = detainID;
            License = clsLicenses.FindByLicenseID(licenseID);
            DetainDate = detainDate;
            FineFees = fineFees;
            CreatedByUser = clsUsers.Find(createdByUserID);
            IsReleased = isReleased;
            ReleaseDate = releaseDate;
            ReleasedByUser = (releasedByUserID == -1) ? null : clsUsers.Find(releasedByUserID);
            ReleaseApplication = (releaseApplicationID == -1) ? null : clsApplication.Find(releaseApplicationID);

            Mode = enMode.Update;
        }

        public clsDetainedLicenses()
        {
            DetainID = default;
            License = default;
            DetainDate = default;
            FineFees = default;
            CreatedByUser = default;
            IsReleased = default;
            ReleaseDate = default;
            ReleasedByUser = default;
            ReleaseApplication = default;

            Mode = enMode.AddNew;
        }


        public static clsDetainedLicenses Find(int LicenseID)
        {
            if (clsDetainedLicensesData.Find(LicenseID, out int detainID, out DateTime detainDate,
                            out float fineFees, out int createdByUserID, out bool isReleased,
                            out DateTime releaseDate, out int releasedByUserID, out int releaseApplicationID))
            {
                return new clsDetainedLicenses(detainID, LicenseID, detainDate, fineFees,
                                               createdByUserID, isReleased, releaseDate,
                                               releasedByUserID, releaseApplicationID);
            }
            else
            {
                return null;
            }
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            return clsDetainedLicensesData.IsLicenseDetained(LicenseID);
        }

        public static DataTable List()
        {
            return clsDetainedLicensesData.List();
        }

        private bool _AddNew()
        {
            DetainID = clsDetainedLicensesData.AddNew(License.LicenseID, DetainDate,
                                                      FineFees, CreatedByUser.UserID);

            return DetainID != -1;
        }
        private bool _Update()
        {
            return clsDetainedLicensesData.Update(DetainID, IsReleased, ReleaseDate,
                                  CreatedByUser.UserID, ReleaseApplication.ApplicationID);
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
