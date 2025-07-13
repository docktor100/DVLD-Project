using DataAccessLayer.Tests;

namespace BusinessLayer.Tests
{
    public class clsTests
    {
        enum enMode { AddNew, Update }
        enMode Mode;
        public int TestID { get; set; }
        public clsTestAppointments TestApointment { get; set; }
        public bool TestResult { get; set; }
        public string Notes { get; set; }
        public clsUsers User { get; set; }

        public clsTests(int testID, int testApointmentID, bool testResult, string notes, int userID)
        {
            TestID = testID;
            TestApointment = clsTestAppointments.Find(testApointmentID);
            TestResult = testResult;
            Notes = notes;
            User = clsUsers.Find(userID);
            Mode = enMode.Update;
        }
        public clsTests()
        {
            TestID = default;
            TestApointment = null;
            TestResult = default;
            Notes = default;
            User = null;
            Mode = enMode.AddNew;
        }

        public static bool IsThereAnyPassedTest(int LDLA_ID, int TestTypeID)
        {
            return clsTestsData.IsThereAnyPassedTests(LDLA_ID, TestTypeID);
        }

        private bool _AddNew()
        {
            TestApointment.IsLocked = true;

            if (!TestApointment.Save())
            {
                throw new System.Exception("Failed to Save TestAppointment Status");
            }


            TestID = clsTestsData.AddNew(TestApointment.TestAppointmentID, TestResult, Notes, User.UserID);

            return TestID != -1;
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
                    return false;


                default:
                    return false;
            }
        }

        public static bool Delete(int lDLA_ID)
        {// deletes all records related to this LDLA ID

            return clsTestsData.Delete(lDLA_ID);
        }
    }
}
