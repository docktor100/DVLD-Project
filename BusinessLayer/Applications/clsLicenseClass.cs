using DataAccessLayer.Applications;

namespace BusinessLayer.Applications
{
    public class clsLicenseClass
    {
        public int LicenseClassID { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }
        public byte MinimumAge { get; set; }
        public byte DefaultValidityLength { get; set; }
        public float ClassFees { get; set; }

        public clsLicenseClass(int iD, string className, string description, byte minimumAllowedAge, byte defaultValidityLength, float classFees)
        {
            LicenseClassID = iD;
            ClassName = className;
            Description = description;
            MinimumAge = minimumAllowedAge;
            DefaultValidityLength = defaultValidityLength;
            ClassFees = classFees;
        }

        public static clsLicenseClass Find(int id)
        {
            if (clsLicenseClassData.Find(id, out string className, out string description, out byte minimumAllowedAge, out byte defaultValidityLength, out float classFees))
            {
                return new clsLicenseClass(id, className, description, minimumAllowedAge, defaultValidityLength, classFees);
            }
            else
            {
                return null;
            }
        }
    }
}
