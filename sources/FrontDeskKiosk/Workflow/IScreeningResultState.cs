using FrontDesk.Common.Bhservice.Import;
using FrontDesk.Common.Screening;

namespace FrontDesk.Kiosk.Workflow
{
    public interface IScreeningResultState
    {
        /// <summary>
        /// Clean state
        /// </summary>
        void ResetResult();
        /// <summary>
        /// Reset screening data result and section timing but keep Patient Demographics data
        /// </summary>
        void ResetScreeningResult();

        FrontDesk.ScreeningResult Result { get; set; }
        ScreeningTimeLog ScreeningTimeLog { get; }
        FrontDesk.Screening ScreeningMetaData { get; }
        PatientDemographicsKioskResult Demographics { get; }
        bool? IsPositiveScoreScreening { get; set; }
        /// <summary>
        /// Flag that indicates wheather user's enetered name and DOB has been successfully validated through EHR as ready for export. 
        /// False means patient has not been found.
        /// </summary>
        bool? PatientNameValidatedOnServer { get; set; }

        /// <summary>
        /// True if user repeating name and DOB questions after initial patient name validation has been failed.
        /// False is default value when patient validation has not been completed yet or has been successful
        /// </summary>
        bool IsRepeatingPatientNameAfterValidationFailed { get; set; }

        void ResetScreeningMetaData();
    }
}
