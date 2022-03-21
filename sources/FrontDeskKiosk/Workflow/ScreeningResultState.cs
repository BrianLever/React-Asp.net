using Common.Logging;

using FrontDesk.Common.Bhservice.Import;
using FrontDesk.Common.Screening;

using System;

namespace FrontDesk.Kiosk.Workflow
{
    public class ScreeningResultState : IScreeningResultState
    {
        private readonly ILog _logger = LogManager.GetLogger<ScreeningResultState>();

        private ScreeningResultState()
        {
            _logger.Debug("Initialized ScreeningResultState.");
        }

        private static readonly object _lockObject = new object();

        private static ScreeningResultState _instance = null;
        /// <summary>
        /// Get object single instance
        /// </summary>
        /// <remarks>Thread-safe</remarks>
        public static ScreeningResultState Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        if (_instance == null) _instance = new ScreeningResultState();
                    }
                }
                return _instance;
            }
        }


        #region Screening Result

        private object _resultSyncObject = new object();

        private ScreeningResult _result = null;
        public bool? IsPositiveScoreScreening { get; set; }

        private PatientDemographicsKioskResult _demographics = new PatientDemographicsKioskResult();
        private readonly ScreeningTimeLog _timelog = new ScreeningTimeLog();

        /// <summary>
        /// Flag that indicates wheather user's enetered name and DOB has been successfully validated through EHR as ready for export. 
        /// False means patient has not been found.
        /// </summary>
        public bool? PatientNameValidatedOnServer { get; set; }

        /// <summary>
        /// True if user repeating name and DOB questions after initial patient name validation has been failed.
        /// False is default value when patient validation has not been completed yet or has been successful
        /// </summary>
        public bool IsRepeatingPatientNameAfterValidationFailed { get; set; } = false;

        /// <summary>
        /// Get or set the screening result
        /// </summary>
        public ScreeningResult Result
        {
            get
            {
                if (_result == null)
                {
                    lock (_resultSyncObject)
                    {
                        if (_result == null)
                        {

                            _result = new ScreeningResult();
                            var screening = ScreeningMetaData;
                            if (screening != null)
                            {
                                _result.ScreeningID = screening.ScreeningID;
                                _result.KioskID = Settings.AppSettings.KioskID;
                            }
                            else
                            {
                                throw new ApplicationException(Properties.Resources.EmptyQuestionnaireMessage);
                            }
                        }
                    }
                }
                return _result;
            }
            set { _result = value; }
        }

        public ScreeningTimeLog ScreeningTimeLog
        {
            get
            {
                return _timelog; ;
            }
        }


        public PatientDemographicsKioskResult Demographics
        {
            get
            {
                return _demographics;
            }
        }

        /// <summary>
        /// Clear screening result data
        /// </summary>
        public void ResetScreeningResult()
        {
            _result = null;
            PatientNameValidatedOnServer = null;
            IsRepeatingPatientNameAfterValidationFailed = false;
        }

        /// <summary>
        /// Clear screening result data
        /// </summary>
        public void ResetResult()
        {
            _result = null;
            IsPositiveScoreScreening = null;
            PatientNameValidatedOnServer = null;
            IsRepeatingPatientNameAfterValidationFailed = false;
            _timelog.Reset();
            _demographics = new PatientDemographicsKioskResult();
        }

        #endregion

        #region Screening Data

        private Screening _screeningMetaData = null;
        /// <summary>
        /// Screening Sections and Questions
        /// </summary>
        public Screening ScreeningMetaData
        {
            get
            {
                Screening tmp = _screeningMetaData;
                if (tmp == null)
                {
                    tmp = KioskScreening.Get();
                    _screeningMetaData = tmp;
                }
                return tmp;
            }
        }

        public void ResetScreeningMetaData()
        {
            _screeningMetaData = null;
        }


        #endregion

    }
}
