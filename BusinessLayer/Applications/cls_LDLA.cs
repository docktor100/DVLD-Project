using BusinessLayer.Applications;
using BusinessLayer.Tests;
using DataAccessLayer.Specific;
using System;
using System.Data;

namespace BusinessLayer.Specific
{
    public class cls_LDLA : clsApplication
    {
        public clsLicenseClass LicenseClass { get; set; }
        public int LDLA_ID { get; set; }
        public sbyte PassedTests { get; set; }

        public cls_LDLA(int local_DL_ApplicationID, int applicationID, int licenseClassID)
        {
            clsApplication application = clsApplication.Find(applicationID);
            LDLA_ID = local_DL_ApplicationID;
            ApplicationID = application.ApplicationID;
            ApplicantPerson = application.ApplicantPerson;
            Date = application.Date;
            ApplicationType = application.ApplicationType;
            LastStatusDate = application.LastStatusDate;
            PaidFees = application.PaidFees;
            Status = application.Status;
            PassedTests = GetPassedTestsNumber();
            CreatedByUser = application.CreatedByUser;
            base.Mode = enMode.Update;


            LicenseClass = clsLicenseClass.Find(licenseClassID);
        }
        public cls_LDLA()
        {
            clsApplication application = new clsApplication();
            ApplicationID = application.ApplicationID;
            ApplicantPerson = application.ApplicantPerson;
            Date = application.Date;
            ApplicationType = application.ApplicationType;
            LastStatusDate = application.LastStatusDate;
            PaidFees = application.PaidFees;
            Status = application.Status;
            base.Mode = enMode.AddNew;

            LicenseClass = null;
        }

        public static new cls_LDLA Find(int Local_DL_ApplicationID)
        {
            if (cls_LDLA_Data.Find(Local_DL_ApplicationID, out int applicationID, out int licenseClassID))
            {
                return new cls_LDLA(Local_DL_ApplicationID, applicationID, licenseClassID);
            }
            else
            {

                return null;
            }
        }
        private sbyte GetPassedTestsNumber()
        {
            if (cls_LDLA_Data.FindPassedTestsNumber(LDLA_ID, out sbyte passedTests))
            {
                return passedTests;
            }
            else
            {
                return -1;
            }

        }
        public void RefreshPassedTests()
        {
            PassedTests = GetPassedTestsNumber();
        }


        //returns a table with the application ID, PersonID, LicenseClassID, status  (for checking person's Local Licenses)
        public static DataTable PersonLDLApplicationsList(int ApplicantPersonID)
        {
            return cls_LDLA_Data.PersonLDLApplicationsList(ApplicantPersonID);
        }
        public static bool DoesHaveNoneCancelledLicense(int ApplicantPersonID, int licenseClassID, out int FoundApplicationID)
        {
            FoundApplicationID = -1;

            DataTable dtPersonLDLApplications = PersonLDLApplicationsList(ApplicantPersonID);

            foreach (DataRow row in dtPersonLDLApplications.Rows)
            {
                if ((int)row["LicenseClassID"] == licenseClassID
                    &&
                    (byte)row["ApplicationStatus"] != 2) // 1 = new, 2 = cancelled, 3 = completed
                {
                    FoundApplicationID = (int)row["ApplicationID"];
                    return true;
                }
            }

            return false;
        }

        public static DataTable LDLA_List()
        {
            return cls_LDLA_Data.LocalDrivingLicenseApplicationsList();
        }


        new public static bool Delete(int lDLA_ID)
        {//new because the same method exists in the base calss
            cls_LDLA lDLA = cls_LDLA.Find(lDLA_ID);

            if (!clsTests.Delete(lDLA_ID))
                throw new Exception("❌ Failed to delete tests related to this Local Driving License Application.");

            if (!clsTestAppointments.Delete(lDLA_ID))
                throw new Exception($"❌ Failed to delete test appointments related to this LDLA after tests were deleted.\n➡ Manually delete by LDLA_ID = {lDLA_ID}.");

            if (!cls_LDLA_Data.Delete(lDLA_ID))
                throw new Exception($"❌ Failed to delete the LDLA after deleting related tests and appointments.\n➡ Manually delete by ID = {lDLA_ID}.");

            if (!clsApplication.Delete(lDLA.ApplicationID))
                throw new Exception($"❌ Failed to delete the application record after deleting LDLA and related data.\n➡ Manually delete by ApplicationID = {lDLA.ApplicationID}.");

            return true;

        }

        private bool _AddNew()
        {
            LDLA_ID = cls_LDLA_Data.AddNew(ApplicationID, LicenseClass.LicenseClassID);

            return LDLA_ID != -1;
        }

        public new bool Save()
        {
            switch (base.Mode)
            {
                case enMode.AddNew:

                    if (base.Save())
                    {
                        if (_AddNew()) { base.Mode = enMode.Update; return true; }
                        else { return false; }
                    }
                    else
                    {
                        return false;
                    }


                case enMode.Update:
                    return base.Save();


                default:
                    return false;
            }

        }
    }
}
