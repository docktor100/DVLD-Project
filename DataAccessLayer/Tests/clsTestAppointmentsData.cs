using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer.Tests
{
    public class clsTestAppointmentsData
    {
        public static bool Find(int testAppointmentID, out int testTypeID, out int lDLA_ID, out DateTime date, out float paidFees, out int createdByUsedID, out bool isLocked, out int retaketestApplicationID)
        {
            testTypeID = default;
            lDLA_ID = default;
            date = default;
            paidFees = default;
            createdByUsedID = default;
            isLocked = false;
            retaketestApplicationID = default;

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT * FROM [dbo].[TestAppointments]
                            where testAppointmentID = @testAppointmentID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@testAppointmentID", testAppointmentID);

            bool isfound = false;
            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.Read())
                {
                    isfound = true;

                    testAppointmentID = (int)Reader["testAppointmentID"];
                    testTypeID = (int)Reader["TestTypeID"];
                    lDLA_ID = (int)Reader["LocalDrivingLicenseApplicationID"];
                    date = (DateTime)Reader["AppointmentDate"];
                    paidFees = (float)Convert.ToDecimal(Reader["PAIdFees"]);
                    createdByUsedID = (int)Reader["CreatedByUserID"];
                    isLocked = (bool)Reader["IsLocked"];

                    retaketestApplicationID = ((Reader["RetakeTestApplicationID"] == DBNull.Value) ? -1 : (int)Reader["RetakeTestApplicationID"]);
                }

                Reader.Close();
            }
            catch (Exception ex)
            {

                clsEventLog.LogError(ex.Message);
                isfound = false;
            }
            finally
            {
                connection.Close();
            }

            return isfound;
        }
        public static int NumberOfTestAppointments(int LDLA_ID, int TestTypeID)
        {//for checking if it is the first time test, or a retake 

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"select count(*) as NumberOfTestAppointments from TestAppointments
                            where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                            and 
                            TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LDLA_ID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            int NumberOfTestAppointments = -1;
            try
            {
                connection.Open();
                object Result = command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int result))
                {
                    NumberOfTestAppointments = result;
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

            return NumberOfTestAppointments;
        }
        public static DataTable TestAppointmentsList(int LDLA_ID, int TestTypeID)
        {//returns TestAppointmentID, AppointmentDate, PaidFees, IsLocked for datagridview

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"select TestAppointmentID, AppointmentDate, PaidFees, IsLocked from TestAppointments
                            where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                            and 
                            TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LDLA_ID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.HasRows)
                {
                    dt.Load(Reader);
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

            return dt;
        }


        public static bool Update(int TestAppointmentID, DateTime date, bool isLocked)
        {

            int RowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"update dbo.TestAppointments
            set AppointmentDate = @AppointmentDate , IsLocked = @isLocked

            where TestAppointmentID = @TestAppointmentID
               ";

            SqlCommand Command = new SqlCommand(Query, connection);
            Command.Parameters.AddWithValue("@AppointmentDate", date);
            Command.Parameters.AddWithValue("@isLocked", isLocked);
            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

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
        public static int AddNew(int testTypeID, int lDLA_ID, DateTime date, float paidFees, int createdByUsedID, bool isLocked, int retaketestApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
          INSERT INTO dbo.TestAppointments
                     (TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees ,CreatedByUserID, IsLocked, RetakeTestApplicationID)
               VALUES   
                     (@TestTypeID, @LocalDrivingLicenseApplicationID, @AppointmentDate, @PaidFees ,@CreatedByUserID, @IsLocked, @RetakeTestApplicationID);
          select SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, connection);
            Command.Parameters.AddWithValue("@TestTypeID", testTypeID);
            Command.Parameters.AddWithValue("@AppointmentDate", date);
            Command.Parameters.AddWithValue("@CreatedByUserID", createdByUsedID);

            Command.Parameters.AddWithValue("@IsLocked", isLocked);
            Command.Parameters.AddWithValue("@PaidFees", paidFees);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", lDLA_ID);

            if (retaketestApplicationID != -1)
                Command.Parameters.AddWithValue("@RetakeTestApplicationID", retaketestApplicationID);
            else
                Command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);


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

        public static bool Delete(int lDLA_ID)
        {
            int RowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"DELETE FROM TestAppointments
                            WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

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
