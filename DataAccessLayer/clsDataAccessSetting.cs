using System.Configuration;

namespace DataAccessLayer
{
    static class clsDataAccessSetting
    {
        //public static string ConnectionString = "Server=.;Database=DVLD;User Id=sa; Password=sa123456"; the old method

        public static string ConnectionString = ConfigurationManager.AppSettings["DB_ConnectionString"];
    }
}
