using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer.Drivers___Licenses
{
    public class clsLicensesData
    {
        public static bool FindByLicenseID(int licenseID, out int applicationID, out int driverID, out int licenseClassID,
            out int createdByUserID, out DateTime issueDate, out DateTime expirationDate, out string notes,
            out float paidfees, out bool isActive, out byte issueReason)
        {
            issueDate = default;
            expirationDate = default;
            notes = default;
            paidfees = default;
            isActive = default;
            issueReason = default;

            applicationID = default;
            driverID = default;
            licenseClassID = default;
            createdByUserID = default;

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT * FROM [dbo].[Licenses]
                            where LicenseID = @LicenseID";

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

                    applicationID = (int)Reader["applicationID"];
                    driverID = (int)Reader["driverID"];
                    createdByUserID = (int)Reader["createdByUserID"];
                    licenseClassID = (int)Reader["LicenseClass"];
                    issueDate = (DateTime)Reader["IssueDate"];
                    expirationDate = (DateTime)Reader["expirationDate"];
                    paidfees = (float)Convert.ToDecimal(Reader["PaidFees"]);
                    isActive = (Convert.ToByte(Reader["IsActive"]) == 1);
                    issueReason = Convert.ToByte(Reader["issueReason"]);


                    notes = ((Reader["Notes"] == DBNull.Value) ? "" : (string)Reader["Notes"]);
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

        public static bool FindByAppID(int applicationID, out int licenseID, out int driverID, out int licenseClassID,
            out int createdByUserID, out DateTime issueDate, out DateTime expirationDate, out string notes,
            out float paidfees, out bool isActive, out byte issueReason)
        {
            licenseID = default;
            issueDate = default;
            expirationDate = default;
            notes = default;
            paidfees = default;
            isActive = default;
            issueReason = default;

            driverID = default;
            licenseClassID = default;
            createdByUserID = default;

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT * FROM [dbo].[Licenses]
                            where applicationID = @applicationID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@applicationID", applicationID);

            bool isfound = false;
            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.Read())
                {
                    isfound = true;

                    licenseID = (int)Reader["licenseID"];
                    driverID = (int)Reader["driverID"];
                    createdByUserID = (int)Reader["createdByUserID"];
                    licenseClassID = (int)Reader["LicenseClass"];
                    issueDate = (DateTime)Reader["IssueDate"];
                    expirationDate = (DateTime)Reader["expirationDate"];
                    paidfees = (float)Convert.ToDecimal(Reader["PaidFees"]);
                    isActive = (Convert.ToByte(Reader["IsActive"]) == 1);
                    issueReason = Convert.ToByte(Reader["issueReason"]);


                    notes = ((Reader["Notes"] == DBNull.Value) ? "" : (string)Reader["Notes"]);
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

        public static int AddNew(int applicationID, int driverID, int licenseClassID,
            int createdByUserID, DateTime issueDate, DateTime expirationDate, string notes,
            float paidfees, bool isActive, byte issueReason)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
          INSERT INTO dbo.Licenses
                     (applicationID, driverID, LicenseClass, createdByUserID, issueDate ,expirationDate, notes, paidfees, isActive, issueReason)
               VALUES   
                     (@applicationID, @driverID, @LicenseClass, @createdByUserID, @issueDate ,@expirationDate, @notes, @paidfees, @isActive, @issueReason);
          select SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, connection);
            Command.Parameters.AddWithValue("@applicationID", applicationID);
            Command.Parameters.AddWithValue("@driverID", driverID);
            Command.Parameters.AddWithValue("@createdByUserID", createdByUserID);
            Command.Parameters.AddWithValue("@expirationDate", expirationDate);
            Command.Parameters.AddWithValue("@issueReason", issueReason);
            Command.Parameters.AddWithValue("@paidfees", paidfees);
            Command.Parameters.AddWithValue("@isActive", isActive);
            Command.Parameters.AddWithValue("@issueDate", issueDate);
            Command.Parameters.AddWithValue("@LicenseClass", licenseClassID);

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
                ID = -1;
            }
            finally
            {
                connection.Close();
            }

            return ID;
        }

        public static bool Deactivate(int LicenseID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"update Licenses
                             set IsActive = @IsActive
                             where LicenseID = @LicenseID";

            SqlCommand Command = new SqlCommand(Query, connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            Command.Parameters.AddWithValue("@IsActive", false);

            int RowsAffected = 0;
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
        public static bool Activate(int LicenseID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"update Licenses
                             set IsActive = @IsActive
                             where LicenseID = @LicenseID";

            SqlCommand Command = new SqlCommand(Query, connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            Command.Parameters.AddWithValue("@IsActive", true);

            int RowsAffected = 0;
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


        public static DataTable LicensesList()
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT * FROM L_Licenses_DataGrid_View";

            SqlCommand command = new SqlCommand(Query, connection);

            DataTable PeopleList = new DataTable();
            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.HasRows)
                {
                    PeopleList.Load(Reader);
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

            return PeopleList;
        }

        public static bool DoesHaveLicense(int ApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"select 1
                            where exists 
                            (select top 1 1 
                            from Licenses
                            where ApplicationID = @ApplicationID)";


            SqlCommand Command = new SqlCommand(Query, connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            bool isfound = false;

            try
            {
                connection.Open();

                object result = Command.ExecuteScalar();

                if (result != null)
                {
                    isfound = true;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return isfound;
        }

    }
}
