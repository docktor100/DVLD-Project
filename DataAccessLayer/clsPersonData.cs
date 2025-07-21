using DataAccessLayer.Tests;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public class clsPersonData
    {
        static public bool FindByID(int id, out string firstName, out string secondName, out string thirdName,
                        out string lastName, out string nationalNO, out DateTime dateOfBirth, out bool gendor,
                        out string address, out string phone, out string email, out int countryID, out string imageName)
        {
            firstName = "";
            secondName = "";
            thirdName = "";
            lastName = "";
            nationalNO = "";
            dateOfBirth = DateTime.Now;
            gendor = false;
            address = "";
            phone = "";
            email = "";
            countryID = 0;
            imageName = "";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"USE [DVLD];
                            SELECT *
                              FROM [dbo].[People]
                            where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@PersonID", id);

            bool isfound = false;
            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.Read())
                {
                    isfound = true;

                    firstName = ((string)Reader["FirstName"]).Trim();
                    secondName = ((string)Reader["secondname"]).Trim();
                    thirdName = ((Reader["thirdname"] == DBNull.Value) ? "" : (string)Reader["thirdname"]).Trim();
                    lastName = ((string)Reader["lastName"]).Trim();
                    nationalNO = ((string)Reader["nationalNO"]).Trim();
                    dateOfBirth = (DateTime)Reader["dateOfBirth"];
                    gendor = (Convert.ToByte(Reader["Gendor"]) == 1);
                    phone = ((string)Reader["phone"]).Trim();
                    countryID = (int)Reader["NationalityCountryID"];
                    address = ((string)Reader["address"]).Trim();

                    imageName = ((Reader["ImageName"] == DBNull.Value) ? "" : (string)Reader["ImageName"]);
                    email = ((Reader["email"] == DBNull.Value) ? "" : (string)Reader["email"]).Trim();
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
        static public bool FindByNationalNo(string nationalNO, out int id, out string firstName, out string secondName, out string thirdName,
                        out string lastName, out DateTime dateOfBirth, out bool gendor,
                        out string address, out string phone, out string email, out int countryID, out string imageName)
        {
            id = 0;
            firstName = "";
            secondName = "";
            thirdName = "";
            lastName = "";
            dateOfBirth = DateTime.Now;
            gendor = false;
            address = "";
            phone = "";
            email = "";
            countryID = 0;
            imageName = "";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"USE [DVLD];
                            SELECT *
                              FROM [dbo].[People]
                            where nationalNO = @nationalNO";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@nationalNO", nationalNO);

            bool isfound = false;
            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.Read())
                {
                    isfound = true;

                    id = (int)Reader["PersonID"];
                    firstName = (string)Reader["FirstName"];
                    secondName = (string)Reader["secondname"];
                    thirdName = ((Reader["thirdname"] == DBNull.Value) ? "" : (string)Reader["thirdname"]);
                    lastName = (string)Reader["lastName"];
                    dateOfBirth = (DateTime)Reader["dateOfBirth"];
                    gendor = Convert.ToByte(Reader["Gendor"]) == 1;
                    phone = (string)Reader["phone"];
                    countryID = (int)Reader["NationalityCountryID"];
                    address = (string)Reader["address"];

                    imageName = ((Reader["ImageName"] == DBNull.Value) ? "" : (string)Reader["ImageName"]);
                    email = ((Reader["email"] == DBNull.Value) ? "" : (string)Reader["email"]);
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

        static public bool IsExist(int ID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT top 1 1 FROM [dbo].[People]
                            where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@PersonID", ID);

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
        static public bool IsExist(string NationalNo)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT top 1 1 FROM [dbo].[People]
                            where nationalNO = @nationalNO";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@nationalNO", NationalNo);

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

        static public DataTable PeopleListForDataGrid()
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT * FROM PeopleForDataGrid";

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


        static public bool UpdatePerson(int id, string firstName, string secondName, string thirdName,
                         string lastName, string nationalNO, DateTime dateOfBirth, bool gendor,
                         string address, string phone, string email, int countryID, string imageName)
        {
            int RowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
          update dbo.People
            set FirstName = @FirstName 
                ,SecondName = @SecondName
                ,ThirdName = @ThirdName
               ,LastName = @LastName
               ,Email = @Email
               ,Gendor = @Gendor
               ,NationalNo= @NationalNo
               ,Phone = @Phone
               ,Address = @Address
               ,DateOfBirth = @DateOfBirth
               ,NationalityCountryID = @NationalityCountryID
               ,imageName = @ImageName

            where PersonID = @PersonID
               ";

            SqlCommand Command = new SqlCommand(Query, connection);
            Command.Parameters.AddWithValue("@PersonID", id);
            Command.Parameters.AddWithValue("@FirstName", firstName);
            Command.Parameters.AddWithValue("@secondName", secondName);
            Command.Parameters.AddWithValue("@LastName", lastName);
            Command.Parameters.AddWithValue("@Gendor", gendor);
            Command.Parameters.AddWithValue("@Address", address);
            Command.Parameters.AddWithValue("@Phone", phone);
            Command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
            Command.Parameters.AddWithValue("@NationalityCountryID", countryID);
            Command.Parameters.AddWithValue("@NationalNo", nationalNO);


            if (email != "")
                Command.Parameters.AddWithValue("@Email", email);
            else
                Command.Parameters.AddWithValue("@Email", DBNull.Value);


            if (thirdName != "")
                Command.Parameters.AddWithValue("@ThirdName", thirdName);
            else
                Command.Parameters.AddWithValue("@ThirdName", DBNull.Value);


            if (imageName != "")
                Command.Parameters.AddWithValue("@ImageName", imageName);
            else
                Command.Parameters.AddWithValue("@ImageName", DBNull.Value);

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
        static public int AddNewPerson(string firstName, string secondName, string thirdName,
                         string lastName, string nationalNO, DateTime dateOfBirth, bool gendor,
                         string address, string phone, string email, int countryID, string imageName)
        {

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
          INSERT INTO dbo.People
                     (FirstName, SecondName, ThirdName, LastName, Email ,Phone, Address, Gendor, NationalNo, DateOfBirth, NationalityCountryID, ImageName)
               VALUES   
                     (@FirstName, @SecondName, @ThirdName, @LastName, @Email ,@Phone, @Address, @Gendor, @NationalNo, @DateOfBirth, @NationalityCountryID, @ImageName);
          select SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, connection);
            Command.Parameters.AddWithValue("@FirstName", firstName);
            Command.Parameters.AddWithValue("@SecondName", secondName);
            Command.Parameters.AddWithValue("@LastName", lastName);

            Command.Parameters.AddWithValue("@Address", address);
            Command.Parameters.AddWithValue("@Phone", phone);
            Command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
            Command.Parameters.AddWithValue("@NationalityCountryID", countryID);
            Command.Parameters.AddWithValue("@Gendor", gendor);
            Command.Parameters.AddWithValue("@NationalNo", nationalNO);

            if (email != "")
                Command.Parameters.AddWithValue("@Email", email);
            else
                Command.Parameters.AddWithValue("@Email", DBNull.Value);


            if (thirdName != "")
                Command.Parameters.AddWithValue("@ThirdName", thirdName);
            else
                Command.Parameters.AddWithValue("@ThirdName", DBNull.Value);


            if (imageName != "")
                Command.Parameters.AddWithValue("@ImageName", imageName);
            else
                Command.Parameters.AddWithValue("@ImageName", DBNull.Value);


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
        static public bool DeletePerson(int ID)
        {
            int RowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"DELETE FROM People
                            WHERE PersonID = @PersonID";

            SqlCommand Command = new SqlCommand(Query, connection);
            Command.Parameters.AddWithValue("@PersonID", ID);

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

        static public DataTable GetCountriesList()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"USE [DVLD];
                  SELECT * FROM [dbo].[Countries]";

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
        static public string GetCountryName(int countryID)
        {

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"USE [DVLD];
                            SELECT *
                              FROM [dbo].[Countries]
                            where CountryID = @CountryID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@CountryID", countryID);
            string CountryName = "";
            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.Read())
                {
                    CountryName = (string)Reader["CountryName"];
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

            return CountryName;
        }
        static public int GetCountryID(string countryName)
        {

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"USE [DVLD];
                            SELECT *
                              FROM [dbo].[Countries]
                            where countryName = @countryName";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@countryName", countryName);
            int countryID = -1;
            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.Read())
                {
                    countryID = (int)Reader["CountryID"];
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

            return countryID;
        }
    }
}
