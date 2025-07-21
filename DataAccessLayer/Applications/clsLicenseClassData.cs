using DataAccessLayer.Tests;
using System;
using System.Data.SqlClient;

namespace DataAccessLayer.Applications
{
    public class clsLicenseClassData
    {
        public static bool Find(int id, out string className, out string description, out byte minimumAllowedAge, out byte defaultValidityLength, out float classFees)
        {
            className = default;
            description = default;
            minimumAllowedAge = default;
            defaultValidityLength = default;
            classFees = default;

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"SELECT *
                  FROM [dbo].[LicenseClasses]
                where LicenseClassID = @LicenseClassID";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@LicenseClassID", id);

            bool isfound = false;
            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.Read())
                {
                    isfound = true;

                    className = ((string)Reader["ClassName"]).Trim();
                    description = ((string)Reader["ClassDescription"]).Trim();
                    minimumAllowedAge = Convert.ToByte(Reader["MinimumAllowedAge"]);
                    defaultValidityLength = Convert.ToByte(Reader["DefaultValidityLength"]);
                    classFees = (float)Convert.ToDecimal(Reader["ClassFees"]);
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

    }
}
