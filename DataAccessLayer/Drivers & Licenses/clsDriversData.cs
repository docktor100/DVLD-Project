using DataAccessLayer.Tests;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer.Drivers___Licenses
{
    public class clsDriversData
    {
        public static bool FindByDriverID(int driverID, out int personID, out int userID, out DateTime date)
        {
            personID = default;
            userID = default;
            date = default;

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT * FROM [dbo].[Drivers]
                where driverID = @driverID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@driverID", driverID);

            bool isfound = false;
            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.Read())
                {
                    isfound = true;

                    personID = (int)Reader["personID"];
                    userID = (int)Reader["createdByUserID"];
                    date = (DateTime)Reader["CreatedDate"];
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
        public static bool FindByPersonID(int personID, out int driverID, out int userID, out DateTime date)
        {
            driverID = default;
            userID = default;
            date = default;

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT * FROM [dbo].[Drivers]
                where personID = @personID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@personID", personID);

            bool isfound = false;
            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.Read())
                {
                    isfound = true;

                    driverID = (int)Reader["driverID"];
                    userID = (int)Reader["createdByUserID"];
                    date = (DateTime)Reader["CreatedDate"];
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

        public static int AddNew(int personID, int userID, DateTime date)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
          INSERT INTO dbo.Drivers
                     (personID, CreatedByUserID, CreatedDate)
               VALUES   
                     (@personID, @CreatedByUserID, @CreatedDate);
          select SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, connection);

            Command.Parameters.AddWithValue("@personID", personID);
            Command.Parameters.AddWithValue("@CreatedByUserID", userID);
            Command.Parameters.AddWithValue("@CreatedDate", date);


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

        public static bool IsExist(int personID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT top 1 1 FROM [dbo].[Drivers]
                            where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@PersonID", personID);

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

        public static DataTable DriversList()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT * FROM [dbo].[Drivers_View]";

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
    }
}
