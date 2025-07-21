using DataAccessLayer.Tests;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer.Specific
{
    public class clsTestTypesData
    {
        static public DataTable GetTestTypesList()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT * FROM [dbo].[TestTypes]";

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

        public static bool Find(int TestTypeID, out string Title, out string Descritption, out float Fee)
        {
            Title = default;
            Fee = default;
            Descritption = default;

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT * FROM [dbo].[TestTypes]
                            where TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            bool isfound = false;
            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.Read())
                {
                    isfound = true;

                    Title = ((string)Reader["TestTypeTitle"]).Trim();
                    Descritption = ((string)Reader["TestTypeDescription"]).Trim();
                    Fee = Convert.ToSingle(Reader["TestTypeFees"]);
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


        public static bool Update(int applicationTypeID, string Description, string Title, float Fee)
        {
            int RowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"update dbo.TestTypes
            set TestTypeTitle = @TestTypeTitle 
                ,TestTypeDescription= @TestTypeDescription
                ,TestTypeFees= @TestTypeFees

            where TestTypeID = @TestTypeID
               ";

            SqlCommand Command = new SqlCommand(Query, connection);
            Command.Parameters.AddWithValue("@TestTypeTitle", Title);
            Command.Parameters.AddWithValue("@TestTypeFees", Fee);
            Command.Parameters.AddWithValue("@TestTypeDescription", Description);
            Command.Parameters.AddWithValue("@TestTypeID", applicationTypeID);


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
