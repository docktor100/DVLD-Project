using DataAccessLayer.Specific;
using System.Data;

namespace BusinessLayer.Specific
{
    public class clsTestType
    {
        public int TestTypeID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Fee { get; set; }

        public clsTestType(int testTypeID, string title, string description, float fee)
        {
            TestTypeID = testTypeID;
            Description = description;
            Title = title;
            Fee = fee;
        }

        static public DataTable GetApplicationsList()
        {
            return clsTestTypesData.GetTestTypesList();
        }
        static public clsTestType Find(int testTypeID)
        {
            if (clsTestTypesData.Find(testTypeID, out string title, out string description, out float Fee))
            {
                return new clsTestType(testTypeID, title, description, Fee);
            }
            else
            {
                return null;
            }
        }

        public bool Update()
        {
            return (clsTestTypesData.Update(TestTypeID, Description, Title, Fee));
        }
    }
}
