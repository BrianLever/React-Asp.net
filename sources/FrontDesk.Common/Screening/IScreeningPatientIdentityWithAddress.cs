namespace FrontDesk
{
    public interface IScreeningPatientIdentityWithAddress: IScreeningPatientIdentity
    {
        string City { get; set; }
        string FullName { get; }
        string Phone { get; set; }
        string StateID { get; set; }
        string StateName { get; set; }
        string StreetAddress { get; set; }
        string ZipCode { get; set; }
    }


    public static class ScreeningPatientIdentityWithAddressExtensiosns
    {
        /// <summary>
        /// Check of current result contains patient's contact info. Returns True if contact questions have been omitted because of contact screening frequency specification.
        /// </summary>
        public static bool IsEmptyContactInfo(this IScreeningPatientIdentityWithAddress model)
        {
          
                return string.IsNullOrEmpty(model.StreetAddress) || string.IsNullOrEmpty(model.StateID) || string.IsNullOrEmpty(model.City)
                    || string.IsNullOrEmpty(model.ZipCode) || string.IsNullOrEmpty(model.Phone);
            
        }
        public static string AddressToString(this IScreeningPatientIdentityWithAddress model)
        {
            return "{0}, {1}, {2} {3}".FormatWith(model.StreetAddress, model.City, model.StateID, model.ZipCode);
        }
    }

}