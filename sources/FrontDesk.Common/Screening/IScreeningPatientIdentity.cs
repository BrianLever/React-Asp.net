using System;

namespace FrontDesk
{
    public interface IScreeningPatientIdentity : IPersonName
    {
        DateTime Birthday { get; set; }
    }



    public interface IPersonName
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string MiddleName { get; set; }
    }
}