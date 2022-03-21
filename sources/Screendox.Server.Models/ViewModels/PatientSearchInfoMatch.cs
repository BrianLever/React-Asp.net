using FrontDesk;
using FrontDesk.Common;
using FrontDesk.Common.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels
{
    /// <summary>
    /// DTO for rendering Matched Patient list search
    /// </summary>
    public class PatientSearchInfoMatch : PatientInfoMatch
    {
        /// <summary>
        /// When True, the patient info origins from EHR system. Otherwise it was found in Screendox database
        /// </summar
        [DataMember]
        public bool IsEhrSource { get; set; }

        /// <summary>
        /// Screen result Id. Null in case of EHR origins.
        /// </summary>
        [DataMember]
        public long? ScreeningResultID { get; set; }

        /// <summary>
        /// Demographics profile Id. Null in case of EHR origins.
        /// </summary>
        [DataMember]
        public long? DemographicsID { get; set; }

        /// <summary>
        /// Ehr patient ID. Null in case of Screendox origins
        /// </summary>
        [DataMember]
        public long? EhrPatientId { get; set; }
    }
}
