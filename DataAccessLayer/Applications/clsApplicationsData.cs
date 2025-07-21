using DataAccessLayer.Tests;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer.Applications
{
    public class clsApplicationsData
    {
        static public bool Find(int applicationID, out int personID, out DateTime date,
                                out int applicatinoTypeID, out DateTime lastStatusDate,
                                out float paidFees, out byte status, out int createdByUserID)
        {
            createdByUserID = default;
            personID = default;
            date = default;
            applicatinoTypeID = default;
            lastStatusDate = default;
            paidFees = default;
            status = default;


            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT * FROM [dbo].[Applications]
                            where ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@ApplicationID", applicationID);

            bool isfound = false;
            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.Read())
                {
                    isfound = true;

                    personID = (int)Reader["ApplicantPersonID"];
                    applicatinoTypeID = (int)Reader["ApplicationTypeID"];
                    date = (DateTime)Reader["ApplicationDate"];
                    status = Convert.ToByte(Reader["ApplicationStatus"]);
                    lastStatusDate = (DateTime)Reader["LastStatusDate"];
                    paidFees = (float)Convert.ToDecimal(Reader["PaidFees"]);
                    createdByUserID = (int)Reader["CreatedByUserID"];
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


        public static bool Delete(int ApplicationID)
        {
            int RowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"DELETE FROM Applications
                            WHERE ApplicationID = @ApplicationID";

            SqlCommand Command = new SqlCommand(Query, connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

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


        public static int AddNew(int personID, DateTime date,
                                  int applicatinoTypeID, DateTime lastStatusDate,
                                  float paidFees, byte status, int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
          INSERT INTO dbo.Applications
                     (ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate ,PaidFees, CreatedByUserID)
               VALUES   
                     (@ApplicantPersonID, @ApplicationDate, @ApplicationTypeID, @ApplicationStatus, @LastStatusDate ,@PaidFees, @CreatedByUserID);
          select SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, connection);

            Command.Parameters.AddWithValue("@ApplicantPersonID", personID);
            Command.Parameters.AddWithValue("@ApplicationDate", date);
            Command.Parameters.AddWithValue("@ApplicationStatus", status);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@PaidFees", paidFees);
            Command.Parameters.AddWithValue("@LastStatusDate", lastStatusDate);
            Command.Parameters.AddWithValue("@ApplicationTypeID", applicatinoTypeID);



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

        public static bool Update(int applicationID, byte status)
        {// this method is only for changing application status
            int RowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            DateTime lastStatusDate = DateTime.Now;

            string Query = @"update dbo.applications
            Set
                lastStatusDate= @lastStatusDate
                ,ApplicationStatus= @ApplicationStatus

            where applicationID = @applicationID";

            SqlCommand Command = new SqlCommand(Query, connection);


            Command.Parameters.AddWithValue("@applicationID", applicationID);
            Command.Parameters.AddWithValue("@lastStatusDate", lastStatusDate);
            Command.Parameters.AddWithValue("@ApplicationStatus", status);

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


        public static DataTable LicenseClassList()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"USE [DVLD];
                  SELECT * FROM [dbo].[LicenseClasses]";

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

        public static bool IsApplicantPersonExist(int applicantPersonID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT top 1 1 FROM [dbo].[Applications]
                            where ApplicantPersonID = @ApplicantPersonID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@ApplicantPersonID", applicantPersonID);

            bool isfound = false;
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null)
                {
                    isfound = true;
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
        public static bool IsUserExist(int userID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT top 1 1 FROM [dbo].[Applications]
                            where CreatedByUserID = @CreatedByUserID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@CreatedByUserID", userID);

            bool isfound = false;
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null)
                {
                    isfound = true;
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

    }
}
