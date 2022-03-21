using System;

namespace FrontDesk.Common.Bhservice
{
    public static  class LookupListDescriptor
    {
        public const string EducationLevel = "EducationLevel";
        public const string Gender = "Gender";
        public const string LivingOnReservation = "LivingOnReservation";
        public const string MaritalStatus = "MaritalStatus";
        public const string MilitaryExperience = "MilitaryExperience";
        public const string Race = "Race";
        public const string SexualOrientation = "SexualOrientation";

        public static bool IsKnown(string name)
        {
            return string.Compare(name, EducationLevel, true) == 0
                 || string.Compare(name, Gender, true) == 0
                 || string.Compare(name, LivingOnReservation, true) == 0
                 || string.Compare(name, MaritalStatus, true) == 0
                 || string.Compare(name, MilitaryExperience, true) == 0
                 || string.Compare(name, Race, true) == 0
                 || string.Compare(name, SexualOrientation, true) == 0;
        }
    }

    public static class TypeaheadListDescriptor
    {
        public const string Tribe = "Tribe";
        public const string County = "County";

        public static bool IsKnown(string name)
        {
            return string.Compare(name, Tribe, true) == 0
                 || string.Compare(name, Tribe, true) == 0;
        }
    }
}