using System;
using System.Data.SqlClient;

namespace DataAccessLayer.Tests
{
    public class clsTestsData
    {
        public static int AddNew(int testAppointmentID, bool testResult, string notes, int userID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
          INSERT INTO dbo.Tests
                     (testAppointmentID, testResult, notes, CreatedByUserID)
               VALUES   
                     (@testAppointmentID, @testResult, @notes, @CreatedByUserID);
          select SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, connection);

            Command.Parameters.AddWithValue("@testAppointmentID", testAppointmentID);
            Command.Parameters.AddWithValue("@CreatedByUserID", userID);
            Command.Parameters.AddWithValue("@testResult", testResult);

            if (notes != "")
                Command.Parameters.AddWithValue("@notes", notes);
            else
                Command.Parameters.AddWithValue("@notes", DBNull.Value);




            int ID = -1;

            try
            {
                connection.Open();

                object result = Command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                {
                    ID = InsertedID;
                }
            }
            catch (Exception ex)
            {
                clsEventLog.LogError(ex.Message);
                ID = -1;
            }
            finally
            {
                connection.Close();
            }

            return ID;
        }

        public static bool IsThereAnyPassedTests(int LDLA_ID, int TestTypeID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            //this view returns a table with LDLA_ID, TestTypeID, TestResult
            string Query = @"select top 1 testResult from TestResultsBy_LDLA_TestType_View

						 where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                         and
                         TestTypeID = @TestTypeID
                         and 
                         TestResult = 1";


            SqlCommand Command = new SqlCommand(Query, connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LDLA_ID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            bool isfound = false;

            try
            {
                connection.Open();

                object result = Command.ExecuteScalar();

                if (result != null)
                {
                    isfound = (bool)result; // the query returns only passed tests
                }
            }
            catch (Exception ex)
            {
                clsEventLog.LogError(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return isfound;
        }

        public static bool Delete(int lDLA_ID)
        {
            int RowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"delete from Tests
                            where TestID in
                            (select testID from LDLA_Tests_View
                            where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)"; // this View returns LDLA_ID and TestID Columns

            SqlCommand Command = new SqlCommand(Query, connection);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", lDLA_ID);

            try
            {
                connection.Open();
                RowsAffected = Command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                clsEventLog.LogError(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return (RowsAffected > 0);
        }

    }
}
