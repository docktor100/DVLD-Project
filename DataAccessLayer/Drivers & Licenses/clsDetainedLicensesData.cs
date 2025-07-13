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

                //Console.WriteLine(ex.Message);
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

                //Console.WriteLine(ex.Message);
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



            Command.Parameters.AddWithValue("@isReleased", false);

            Command.Parameters.AddWithValue("@releasedByUserID", DBNull.Value);
            Command.Parameters.AddWithValue("@releaseDate", DBNull.Value);
            Command.Parameters.AddWithValue("@releaseApplicationID", DBNull.Value);

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
                ID = -1;
            }
            finally
            {
                connection.Close();
            }

            return ID;
        }

        public static bool Update(int detainID, bool isReleased,
                            DateTime releaseDate, int releasedByUserID,
                             int releaseApplicationID)
        {
            int RowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
          update dbo.DetainedLicenses
            set isReleased = @isReleased 
                ,releaseDate = @releaseDate
                ,releasedByUserID = @releasedByUserID
               ,releaseApplicationID = @releaseApplicationID

            where detainID = @detainID
               ";

            SqlCommand Command = new SqlCommand(Query, connection);
            Command.Parameters.AddWithValue("@detainID", detainID);

            Command.Parameters.AddWithValue("@isReleased", isReleased);
            Command.Parameters.AddWithValue("@releaseDate", releaseDate);
            Command.Parameters.AddWithValue("@releaseApplicationID", releaseApplicationID);
            Command.Parameters.AddWithValue("@releasedByUserID", releasedByUserID);

            try
            {
                connection.Open();
                RowsAffected = Command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return (RowsAffected > 0);
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

                //Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return DT;
        }
    }
}
