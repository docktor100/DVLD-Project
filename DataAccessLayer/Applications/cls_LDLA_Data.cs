using DataAccessLayer.Tests;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer.Specific
{
    public class cls_LDLA_Data
    {
        public static bool Find(int local_DL_ApplicationID, out int applicationID, out int licenseClassID)
        {
            applicationID = default;
            licenseClassID = default;

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"USE [DVLD];
                SELECT *
                  FROM [dbo].[LocalDrivingLicenseApplications]
                where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", local_DL_ApplicationID);

            bool isfound = false;
            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.Read())
                {
                    isfound = true;

                    applicationID = (int)Reader["ApplicationID"];
                    licenseClassID = (int)Reader["LicenseClassID"];
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
        public static bool FindPassedTestsNumber(int Local_DL_ApplicationID, out sbyte PassedTests)
        {
            PassedTests = default;

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT        COUNT(dbo.TestAppointments.TestTypeID) AS PassedTestCount
                               FROM            dbo.Tests INNER JOIN
                                                         dbo.TestAppointments ON dbo.Tests.TestAppointmentID = dbo.TestAppointments.TestAppointmentID
                               WHERE        (dbo.TestAppointments.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) AND (dbo.Tests.TestResult = 1)";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", Local_DL_ApplicationID);

            bool isfound = false;
            try
            {
                connection.Open();
                object Result = command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int numberOfPassedTests))
                {
                    isfound = true;

                    PassedTests = (sbyte)numberOfPassedTests;
                }

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


        static public DataTable PersonLDLApplicationsList(int ApplicantPersonID)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"select * from PersonLDLApplications_View
                                where ApplicantPersonID = @ApplicantPersonID";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);

            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.HasRows)
                {
                    dt.Load(Reader);
                }

                Reader.Close();
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
        static public DataTable LocalDrivingLicenseApplicationsList()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"select * from LocalDrivingLicenseApplications_View";

            SqlCommand command = new SqlCommand(Query, connection);


            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.HasRows)
                {
                    dt.Load(Reader);
                }

                Reader.Close();
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

        public static bool Delete(int lDLA_ID)
        {
            int RowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"DELETE FROM LocalDrivingLicenseApplications
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
        public static bool Delete(int lDLA_ID, int ApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"begin transaction 
                             begin try
                             
                             	delete from Tests
                                                         where TestID in
                                                         (select testID from LDLA_Tests_View
                                                         where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID); --this View returns LDLA_ID and TestID Columns
                             
                             	DELETE FROM TestAppointments
                                                         WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                             
                             	DELETE FROM LocalDrivingLicenseApplications
                                                         WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                             
                             	DELETE FROM Applications
                                                         WHERE ApplicationID = @ApplicationID
                             
                             	
                             	commit

                             end try
                             
                             begin catch
                             	rollback
                             end catch";

            SqlCommand Command = new SqlCommand(Query, connection);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", lDLA_ID);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            bool IsDeleted;
            try
            {
                connection.Open();
                Command.ExecuteNonQuery();
                IsDeleted = true;
            }
            catch (Exception ex)
            {

                clsEventLog.LogError(ex.Message);
                IsDeleted = false;
            }
            finally
            {
                connection.Close();
            }

            return IsDeleted;
        }

        public static int AddNew(int applicationID, int licenseClassID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
          INSERT INTO dbo.LocalDrivingLicenseApplications
                     (ApplicationID, licenseClassID)
               VALUES   
                     (@ApplicationID, @licenseClassID);
          select SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, connection);
            Command.Parameters.AddWithValue("@ApplicationID", applicationID);
            Command.Parameters.AddWithValue("@licenseClassID", licenseClassID);


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
    }
}
