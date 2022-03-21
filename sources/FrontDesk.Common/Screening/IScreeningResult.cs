using System;

namespace FrontDesk
{
    /// <summary>
    /// Screening Result model
    /// </summary>
    public interface IScreeningResult
    {
        long ID { get; set; }
        DateTimeOffset CreatedDate { get; set; }
        DateTimeOffset? ExportDate { get; set; }
        int? ExportedToPatientID { get; set; }
        DateTime? ExportedToVisitDate { get; set; }
        int? ExportedToVisitID { get; set; }
        string ExportedToVisitLocation { get; set; }
        
        bool IsContactInfoEligableForExport { get; }
        bool IsEligible4Export { get; }
        bool IsPassedAnySection { get; }
        short KioskID { get; set; }
        int? LocationID { get; set; }
        string LocationLabel { get; }
    }
}