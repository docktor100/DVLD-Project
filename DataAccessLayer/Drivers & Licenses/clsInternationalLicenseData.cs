using DataAccessLayer.Tests;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer.Drivers___Licenses
{
    public class clsInternationalLicenseData
    {
        public static bool FindBy_IDL_ID(int internationalLicenseID, out int applicationID, out int driverID,
                                out int issuedUsing_LDL_ID, out DateTime issueDate, out DateTime expirationDate,
                                out bool isActive, out int createdByUserID)
        {
            applicationID = default;
            driverID = default;
            issuedUsing_LDL_ID = default;
            issueDate = default;
            expirationDate = default;
            isActive = default;
            createdByUserID = default;

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT top 1 * FROM [dbo].[InternationalLicenses]
                            where InternationalLicenseID = @InternationalLicenseID
                            order by InternationalLicenseID desc";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@InternationalLicenseID", internationalLicenseID);

            bool isfound = false;
            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.Read())
                {
                    isfound = true;

                    applicationID = (int)Reader["ApplicationID"];
                    driverID = (int)Reader["driverID"];
                    issuedUsing_LDL_ID = (int)Reader["issuedUsingLocalLicenseID"];
                    createdByUserID = (int)Reader["createdByUserID"];
                    issueDate = (DateTime)Reader["issueDate"];
                    expirationDate = (DateTime)Reader["expirationDate"];
                    isActive = (Convert.ToByte(Reader["isActive"]) == 1);
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

        public static bool FindBy_LDL_ID(int issuedUsing_LDL_ID, out int applicationID, out int driverID,
                                                 out int internationalLicenseID, out DateTime issueDate, out DateTime expirationDate,
                                                 out bool isActive, out int createdByUserID)
        {
            internationalLicenseID = default;
            applicationID = default;
            driverID = default;
            issueDate = default;
            expirationDate = default;
            isActive = default;
            createdByUserID = default;

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT top 1 * FROM [dbo].[InternationalLicenses]
                            where IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID
                            order by InternationalLicenseID desc";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", issuedUsing_LDL_ID);

            bool isfound = false;
            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.Read())
                {
                    isfound = true;

                    applicationID = (int)Reader["ApplicationID"];
                    driverID = (int)Reader["driverID"];
                    internationalLicenseID = (int)Reader["internationalLicenseID"];
                    createdByUserID = (int)Reader["createdByUserID"];
                    issueDate = (DateTime)Reader["issueDate"];
                    expirationDate = (DateTime)Reader["expirationDate"];
                    isActive = (Convert.ToByte(Reader["isActive"]) == 1);
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


        public static bool IsExist_Active(int LDL_ID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT top 1 1 FROM [dbo].[InternationalLicenses]
                            where IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID and GETDATE() < ExpirationDate";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", LDL_ID);

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

        public static DataTable ListForPersonHistory()
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT * FROM int_Licenses_DataGrid_View";

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

                clsEventLog.LogError(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return PeopleList;
        }
        public static DataTable ListForManage()
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT * FROM InternationalLicenses";

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

                clsEventLog.LogError(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return PeopleList;
        }



        public static int AddNew(int applicationID, int driverID, int issuedUsing_LDL_ID,
                                  DateTime issueDate, DateTime expirationDate, bool isActive,
                                  int createdByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
          INSERT INTO dbo.InternationalLicenses
                     (applicationID, driverID,IssuedUsingLocalLicenseID, issueDate, expirationDate, isActive, createdByUserID)
               VALUES   
                     (@applicationID, @driverID, @IssuedUsingLocalLicenseID, @issueDate, @expirationDate, @isActive, @createdByUserID);
          select SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, connection);
            Command.Parameters.AddWithValue("@applicationID", applicationID);
            Command.Parameters.AddWithValue("@driverID", driverID);
            Command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", issuedUsing_LDL_ID);

            Command.Parameters.AddWithValue("@expirationDate", expirationDate);
            Command.Parameters.AddWithValue("@issueDate", issueDate);
            Command.Parameters.AddWithValue("@createdByUserID", createdByUserID);
            Command.Parameters.AddWithValue("@isActive", isActive);


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
