using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening.Models
{
    public static class DischargedDescriptor
    {
        public const int No = 0;
        public const int ServiceCompleted = 1;
        public const int SymptomReduction = 2;
        public const int PatientRequestedDiscontinuationOfService = 3;
        public const int AddressChanged = 4;
        public const int CouldNotContact = 5;
        public const int TransferredToDifferentProvider = 6;
        public const int Deceased = 7;

        public static bool IsDischarged(int value)
        {
            return value > No;
        }
    }
}
