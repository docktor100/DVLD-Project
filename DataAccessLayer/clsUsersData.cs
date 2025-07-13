using System;
using System.Data;
using System.Data.SqlClient;


namespace DataAccessLayer
{
    public class clsUsersData
    {
        public static bool Find(int userID, ref int personID, ref string userName, ref string password, ref bool isActive)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT * FROM [dbo].[Users]
                            where UserID = @UserID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@UserID", userID);

            bool isfound = false;
            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.Read())
                {
                    isfound = true;

                    userName = ((string)Reader["UserName"]).Trim();
                    password = ((string)Reader["PassWord"]).Trim();
                    isActive = (bool)Reader["IsActive"];
                    personID = (int)Reader["PersonID"];

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
        public static bool Find(string userName, ref int personID, ref int userID, ref string password, ref bool isActive)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT * FROM [dbo].[Users]
                            where UserName = @UserName";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@UserName", userName);

            bool isfound = false;
            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.Read())
                {
                    isfound = true;

                    userID = ((int)Reader["UserID"]);
                    password = ((string)Reader["PassWord"]).Trim();
                    isActive = (bool)Reader["IsActive"];
                    personID = (int)Reader["PersonID"];

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

        public static bool IsExist(string userName)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT top 1 1 FROM [dbo].[Users]
                            where UserName = @UserName";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@UserName", userName);

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

                //Console.WriteLine(ex.Message);
                isfound = false;
            }
            finally
            {
                connection.Close();
            }

            return isfound;
        }
        public static bool IsExist(int personID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT top 1 1 FROM [dbo].[Users]
                            where personID = @personID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@personID", personID);

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

                //Console.WriteLine(ex.Message);
                isfound = false;
            }
            finally
            {
                connection.Close();
            }

            return isfound;
        }

        public static DataTable GetUsersForDataGrid()
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT * FROM UsersForDataGrid";

            SqlCommand command = new SqlCommand(Query, connection);

            DataTable UsersList = new DataTable();
            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.HasRows)
                {
                    UsersList.Load(Reader);
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

            return UsersList;
        }

        static public bool UpdateUser(int userID, int personID, string userName, string password, bool isActive)
        {
            int RowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
            update dbo.Users
            Set
                personID= @personID
                ,userName= @userName
               ,password= @password
               ,IsActive = @IsActive

            where userID = @userID";

            SqlCommand Command = new SqlCommand(Query, connection);


            Command.Parameters.AddWithValue("@UserID", userID);
            Command.Parameters.AddWithValue("@personID", personID);
            Command.Parameters.AddWithValue("@userName", userName);

            Command.Parameters.AddWithValue("@password", password);
            Command.Parameters.AddWithValue("@IsActive", isActive);

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
        public static int AddNewUser(int personID, string userName, string password, bool isActive)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
          INSERT INTO dbo.Users
                     (PersonID, UserName, Password, IsActive)
               VALUES   
                     (@PersonID, @UserName, @Password, @IsActive);
          select SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, connection);
            Command.Parameters.AddWithValue("@personID", personID);
            Command.Parameters.AddWithValue("@userName", userName);

            Command.Parameters.AddWithValue("@password", password);
            Command.Parameters.AddWithValue("@isactive", isActive);

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
        public static bool DeleteUser(int UserID)
        {
            int RowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"DELETE FROM Users
                            WHERE UserID = @UserID";

            SqlCommand Command = new SqlCommand(Query, connection);
            Command.Parameters.AddWithValue("@UserID", UserID);

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
