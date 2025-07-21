using DataAccessLayer.Tests;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer.Drivers___Licenses
{
    public class clsDetainedLicensesData
    {
        public static bool Find(int licenseID, out int detainID, out DateTime detainDate,
                            out float fineFees, out int createdByUserID, out bool isReleased,
                            out DateTime releaseDate, out int releasedByUserID,
                             out int releaseApplicationID)
        {
            detainID = default;
            detainDate = default;
            fineFees = default;
            createdByUserID = default;
            isReleased = default;
            releaseDate = default;
            releasedByUserID = default;
            releaseApplicationID = default;

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT * FROM [dbo].[DetainedLicenses]
                            where LicenseID = @LicenseID and IsReleased = 0";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@LicenseID", licenseID);

            bool isfound = false;
            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.Read())
                {
                    isfound = true;

                    detainID = (int)Reader["DetainID"];
                    detainDate = (DateTime)Reader["detainDate"];
                    fineFees = Convert.ToSingle(Reader["FineFees"]);
                    createdByUserID = (int)Reader["createdByUserID"];
                    isReleased = (bool)Reader["IsReleased"];

                    if (isReleased)
                    {
                        releaseDate = (DateTime)Reader["releaseDate"];
                        releasedByUserID = (int)Reader["releasedByUserID"];
                        releaseApplicationID = (int)Reader["releaseApplicationID"];
                    }
                    else
                    {
                        releaseDate = default;
                        releasedByUserID = -1;
                        releaseApplicationID = -1;
                    }
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

        public static bool IsLicenseDetained(int LicenseID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT top 1 1 FROM [dbo].[DetainedLicenses]
                            where LicenseID = @LicenseID and IsReleased = 0";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            bool isfound = false;
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                isfound = (result != null);
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

        public static int AddNew(int licenseID, DateTime detainDate,
                            float fineFees, int createdByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
          INSERT INTO dbo.DetainedLicenses
                     (licenseID, detainDate, createdByUserID, fineFees)
               VALUES   
                     (@licenseID, @detainDate, @createdByUserID, @fineFees);
          select SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, connection);
            Command.Parameters.AddWithValue("@licenseID", licenseID);
            Command.Parameters.AddWithValue("@detainDate", detainDate);
            Command.Parameters.AddWithValue("@fineFees", fineFees);
            Command.Parameters.AddWithValue("@createdByUserID", createdByUserID);

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

        public static bool ReleaseDetainedLicense(int detainID, int releasedByUserID
                                 , int PersonID, float PaidFees, out int releaseApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
                            BEGIN TRANSACTION
                            BEGIN TRY
                            
                                INSERT INTO dbo.Applications
                                   (ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID)
                                VALUES
                                   (@ApplicantPersonID, @ApplicationDate, @ApplicationTypeID, @ApplicationStatus, @LastStatusDate, @PaidFees, @CreatedByUserID);
                            
                                Declare @releaseApplicationID int = SCOPE_IDENTITY();


                                UPDATE dbo.DetainedLicenses
                                SET isReleased = @isReleased,
                                    releaseDate = @releaseDate,
                                    releasedByUserID = @releasedByUserID,
                                    releaseApplicationID = @releaseApplicationID
                                WHERE detainID = @detainID;
                            
                                COMMIT TRANSACTION

                                select @releaseApplicationID AS releaseApplicationID;
                            
                            END TRY
                            BEGIN CATCH
                                ROLLBACK TRANSACTION
                                THROW;  -- Re-throw the error so it can be caught in C#
                            END CATCH
                            ";

            SqlCommand Command = new SqlCommand(Query, connection);

            Command.Parameters.AddWithValue("@detainID", detainID);
            Command.Parameters.AddWithValue("@isReleased", true);
            Command.Parameters.AddWithValue("@releaseDate", DateTime.Now);
            //Command.Parameters.AddWithValue("@releaseApplicationID", releaseApplicationID); declared inside the query
            Command.Parameters.AddWithValue("@releasedByUserID", releasedByUserID);


            // The parameters for INSERT application :
            Command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
            Command.Parameters.AddWithValue("@ApplicationDate", DateTime.Now);
            Command.Parameters.AddWithValue("@ApplicationTypeID", 5); // stands for release detained license
            Command.Parameters.AddWithValue("@ApplicationStatus", 3); // stands for completed
            Command.Parameters.AddWithValue("@LastStatusDate", DateTime.Now);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", releasedByUserID);

            releaseApplicationID = -1;
            bool isReleased = false;
            try
            {
                connection.Open();
                releaseApplicationID = (int)Command.ExecuteScalar();

                isReleased = true;
            }
            catch (Exception ex)
            {
                clsEventLog.LogError(ex.Message);
                isReleased = false;
            }
            finally
            {
                connection.Close();
            }

            return isReleased;
        }

        public static DataTable List()
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT * FROM DetainedLicenses_View";

            SqlCommand command = new SqlCommand(Query, connection);

            DataTable DT = new DataTable();
            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.HasRows)
                {
                    DT.Load(Reader);
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

            return DT;
        }
    }
}
