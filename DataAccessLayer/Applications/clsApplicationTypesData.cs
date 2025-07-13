using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public class clsApplicationTypesData
    {
        static public DataTable GetApplicationTypesList()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT * FROM [dbo].[ApplicationTypes]";

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

                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static bool Find(int applicationTypeID, out string Title, out float Fee)
        {
            Title = default;
            Fee = default;

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT *
                              FROM [dbo].[ApplicationTypes]
                            where ApplicationTypeID = @ApplicationTypeID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeID);

            bool isfound = false;
            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.Read())
                {
                    isfound = true;

                    Title = ((string)Reader["ApplicationTypeTitle"]).Trim();
                    Fee = Convert.ToSingle(Reader["ApplicationFees"]);
                }

                Reader.Close();
            }
            catch (Exception ex)
            {

                //Console.WriteLine(ex.Message);
                isfound = false;
            }
            finally
            {
                connection.Close();
            }

            return isfound;
        }

        public static bool Update(int applicationTypeID, string Title, float Fee)
        {
            int RowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"update dbo.ApplicationTypes
            set ApplicationTypeTitle = @ApplicationTypeTitle 
                ,ApplicationFees = @ApplicationFees

            where ApplicationTypeID = @ApplicationTypeID
               ";

            SqlCommand Command = new SqlCommand(Query, connection);
            Command.Parameters.AddWithValue("@ApplicationTypeTitle", Title);
            Command.Parameters.AddWithValue("@ApplicationFees", Fee);
            Command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeID);


            try
            {
                connection.Open();
                RowsAffected = Command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return (RowsAffected > 0);
        }
    }

}
