using System;

namespace ScreenDox.EHR.Common.Models.PatientValidation
{
    public class PatientNameCorrectionLogItem
    {
        public string OriginalPatientName { get; set; }
        public DateTime OriginalBirthday { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public string CorrectedPatientName { get; set; }
        public DateTime CorrectedBirthday { get; set; }
        public string Comments { get; set; }
    }
}
