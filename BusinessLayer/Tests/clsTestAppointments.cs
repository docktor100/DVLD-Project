using BusinessLayer.Applications;
using BusinessLayer.Specific;
using DataAccessLayer.Tests;
using System;
using System.Data;

namespace BusinessLayer.Tests
{
    public class clsTestAppointments
    {
        public int TestAppointmentID { get; set; }
        public DateTime Date { get; set; }
        public float PaidFees { get; set; }
        public bool IsLocked { get; set; }
        enum enMode { Addnew, Update }
        enMode Mode;

        public clsTestType TestType { get; set; }
        public clsUsers CreatedByUsed { get; set; }
        public cls_LDLA LDLA { get; set; }
        public clsApplication RetaketestApplication { get; set; }



        clsTestAppointments(int testAppointmentID, int testTypeID, int lDLA_ID, DateTime date, float paidFees, int createdByUsedID, bool isLocked, int retaketestApplicationID)
        {
            TestAppointmentID = testAppointmentID;
            Date = date;
            PaidFees = paidFees;
            IsLocked = isLocked;
            Mode = enMode.Update;

            TestType = clsTestType.Find(testTypeID);
            LDLA = cls_LDLA.Find(lDLA_ID);
            CreatedByUsed = clsUsers.Find(createdByUsedID);
            RetaketestApplication = clsApplication.Find(retaketestApplicationID);
        }
        public clsTestAppointments()
        {
            TestAppointmentID = default;
            Date = default;
            PaidFees = default;
            IsLocked = default;
            Mode = enMode.Addnew;

            TestType = default;
            LDLA = default;
            CreatedByUsed = default;
            RetaketestApplication = default;
        }



        public static clsTestAppointments Find(int testAppointmentID)
        {
            if (clsTestAppointmentsData.Find(testAppointmentID, out int testTypeID, out int lDLA_ID,
                out DateTime date, out float paidFees, out int createdByUsedID, out bool isLocked,
                out int retaketestApplicationID))
            {
                return new clsTestAppointments(testAppointmentID, testTypeID, lDLA_ID, date, paidFees,
                                                createdByUsedID, isLocked, retaketestApplicationID);
            }
            else
            {
                return null;
            }
        }
        public static int NumberOfTestAppointments(int LDLA_ID, int TestTypeID)
        {
            return clsTestAppointmentsData.NumberOfTestAppointments(LDLA_ID, TestTypeID);
        }
        public static DataTable TestAppointmentsList(int LDLA_ID, int TestTypeID)
        {
            return clsTestAppointmentsData.TestAppointmentsList(LDLA_ID, TestTypeID);
        }

        private clsApplication FillRetakeTestApp()
        {
            clsApplication app = new clsApplication();

            app.ApplicantPerson = LDLA.ApplicantPerson;
            app.CreatedByUser = LDLA.CreatedByUser;
            app.Date = DateTime.Now;
            app.LastStatusDate = DateTime.Now;
            app.Status = clsApplication.enStatus.Completed;


            clsApplicationTypes AppType = clsApplicationTypes.Find(7); // type is retake test application
            app.ApplicationType = AppType;
            app.PaidFees = AppType.Fee;

            return app;
        }
        private int GetRetakeTestAppID()
        {
            int reTakeApplicationID = -1; //Null by default
            clsApplication app = null;
            int numOftries = NumberOfTestAppointments(LDLA.LDLA_ID, TestType.TestTypeID);

            if (numOftries > 0) // checks if a take or a retake
                app = FillRetakeTestApp();


            if (app != null) // == if app is not a retake
            {
                if (app.Save())
                {
                    reTakeApplicationID = app.ApplicationID;
                }
                else
                {
                    throw new Exception("Failed to save Retake Application");
                }
            }

            return reTakeApplicationID;
        }
        private bool _AddNew()
        {
            int RetakeApplicationID = GetRetakeTestAppID();


            TestAppointmentID = clsTestAppointmentsData.AddNew(TestType.TestTypeID, LDLA.LDLA_ID, Date,
                                                               PaidFees, CreatedByUsed.UserID, IsLocked, RetakeApplicationID);

            return TestAppointmentID != -1;
        }
        private bool _Update()
        {
            return clsTestAppointmentsData.Update(TestAppointmentID, Date, IsLocked);
        }

        public static bool Delete(int testAppID)
        {
            return clsTestAppointmentsData.Delete(testAppID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.Addnew:
                    if (_AddNew())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;


                case enMode.Update:
                    return _Update();


                default:
                    return false;
            }
        }
    }
}
