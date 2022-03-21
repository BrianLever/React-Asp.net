
using FrontDesk;
using FrontDesk.Common.Extensions;

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ScreenDox.Server.Models
{
    [KnownType(typeof(PatientInfo))]
    [DataContract(Name = "ScreeningResultResponse", Namespace = "http://www.screendox.com")]
    public class ScreeningResultResponse : IScreeningResult
    {
        [DataMember]
        public long ID { get; set; }

        /// <summary>
        /// Patient details with an address.
        /// </summary>
        [DataMember]
        public PatientInfo PatientInfo { get; set; }

        /// <summary>
        /// Patient's HRN in the EHR database to which exported report has been linked
        /// </summary>
        [DataMember]
        public string ExportedToHRN { get; set; }

        /// <summary>
        /// Visit ID key
        /// </summary>
        [DataMember]
        public int VisitID { get; set; }

       
        [DataMember]
        public DateTimeOffset CreatedDate { get; set; }

        [DataMember]
        public string CreatedDateLabel => CreatedDate.FormatAsDateWithTime();

        [DataMember]
        public DateTimeOffset? ExportDate { get; set; }

        [DataMember]
        public string ExportDateLabel => ExportDate.FormatAsDateWithTime();

        [DataMember]
        public string ExportedByFullName { get; set; }

        [DataMember]
        public int? ExportedToPatientID { get; set; }

        [DataMember]
        public DateTime? ExportedToVisitDate { get; set; }

        [DataMember]
        public int? ExportedToVisitID { get; set; }

        [DataMember]
        public string ExportedToVisitLocation { get; set; }

        [DataMember]
        public bool IsContactInfoEligableForExport { get; set; }

        [DataMember]
        public bool IsEligible4Export { get; set; }
        [DataMember]
        public bool IsPassedAnySection { get; set; }
        [DataMember]
        public short KioskID { get; set; }

        [DataMember]
        public int? LocationID { get; set; }
        [DataMember]
        public string LocationLabel { get; set; }

        /// <summary>
        /// Label whether Visit exists and can be navitated to or create operation need to be performed.
        /// </summary>
        [DataMember]
        public string BhsVisitStatus { get; set; }

        /// <summary>
        /// ID of the linked Screendox Visit record.
        /// </summary>
        [DataMember]
        public long? BhsVisitID { get; set; }

        [DataMember]
        /// <summary>
        /// True if records has been created with validation errors.
        /// </summary>
        public bool WithErrors { get; set; }

        [DataMember]
        /// <summary>
        /// Contains warning message text if record has been created with errors.
        /// </summary>
        public string WithErrorsMessage { get; set; }

        [DataMember]
        /// <summary>
        /// ID of Screendox Patient Record (Patient Demographics)
        /// </summary>
        public long? PatientDemographicsID { get; set; }

        [DataMember]
        public List<ScreeningResultSectionResponse> Sections = new List<ScreeningResultSectionResponse>();
    }
}
