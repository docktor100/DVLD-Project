using DataAccessLayer;
using System.Data;

namespace BusinessLayer
{
    public class clsApplicationTypes
    {
        public int ApplicationTypeID { get; set; }
        public string Title { get; set; }
        public float Fee { get; set; }

        public clsApplicationTypes(int applicationTypeID, string title, float fee)
        {
            ApplicationTypeID = applicationTypeID;
            Title = title;
            Fee = fee;
        }
        public clsApplicationTypes()
        {
            ApplicationTypeID = default;
            Title = default;
            Fee = default;
        }

        static public DataTable GetApplicationsList()
        {
            return clsApplicationTypesData.GetApplicationTypesList();
        }
        static public clsApplicationTypes Find(int applicationTypeID)
        {
            if (clsApplicationTypesData.Find(applicationTypeID, out string Title, out float Fee))
            {
                return new clsApplicationTypes(applicationTypeID, Title, Fee);
            }
            else
            {
                return null;
            }
        }

        public bool Update()
        {
            return (clsApplicationTypesData.Update(ApplicationTypeID, Title, Fee));
        }
    }
}
