using Common.Logging;

using FrontDesk.Common;
using FrontDesk.Common.Bhservice;
using FrontDesk.Common.Bhservice.Import;
using FrontDesk.Common.Screening;
using FrontDesk.Server.Licensing.Services;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Services;
using ScreenDox.Server.Common.Models;
using ScreenDox.Server.Common.Services;

using System;
using System.Linq;

namespace FrontDesk.Server.Controllers
{
    [Obsolete("Migrated to ScreeDox.Server")]
    public class ScreeningResultCreator
    {
        private readonly IScreeningResultService _screeningResultService;
        private readonly BhsVisitService _bhsVisitService;
        private readonly IBhsDemographicsService _demographicsService;
        private readonly IScreeningTimeLogService _screeningTimeLogService;
        private readonly IBranchLocationService _branchLocationService;
        private readonly ILicenseService _licenseService;
        private readonly IErrorLoggerService _errorLoggerService;
        private readonly IKioskService _kioskService;

        private readonly ILog _logger = LogManager.GetLogger<ScreeningResultCreator>();


        public ScreeningResultCreator() :
            this(new ScreeningResultService(), new BhsVisitService(), new BhsDemographicsService(),
                LicenseService.Current,
                new ErrorLoggerService(),
                new ScreeningTimeLogService(),
                new BranchLocationService(),
                new KioskService()
                )
        {

        }

        public ScreeningResultCreator(
            IScreeningResultService screeningResultService,
            BhsVisitService bhsVisitService,
            IBhsDemographicsService demographicsService,
            ILicenseService licenseService,
            IErrorLoggerService errorLoggerService,
            IScreeningTimeLogService screeningTimeLogService,
            IBranchLocationService branchLocationService,
            IKioskService kioskService
            )
        {
            _screeningResultService = screeningResultService ?? throw new ArgumentNullException(nameof(screeningResultService));
            _bhsVisitService = bhsVisitService ?? throw new ArgumentNullException(nameof(bhsVisitService));
            _demographicsService = demographicsService ?? throw new ArgumentNullException(nameof(demographicsService));
            _licenseService = licenseService ?? throw new ArgumentNullException(nameof(licenseService));
            _errorLoggerService = errorLoggerService ?? throw new ArgumentNullException(nameof(errorLoggerService));
            _screeningTimeLogService = screeningTimeLogService ?? throw new ArgumentNullException(nameof(screeningTimeLogService));
            _branchLocationService = branchLocationService ?? throw new ArgumentNullException(nameof(branchLocationService));
            _kioskService = kioskService ?? throw new ArgumentNullException(nameof(kioskService));
        }

        /// <summary>
        /// Returns true of possitive result, false if negative, null if failed to save
        /// </summary>
        /// <param name="result"></param>
        /// <param name="timeLog"></param>
        /// <returns></returns>
        public bool? SaveScreeningResult(ScreeningResult result, ScreeningTimeLogRecord[] timeLog)
        {
            bool succeed = false;
            long screeningResultId = 0;
            bool? isPositiveIndicator = null;
            try
            {

                if (ValidateKiosk(result.KioskID))
                {
                    // save result if kiosks count has not been exceeded 
                    screeningResultId = _screeningResultService.InsertScreeningResult(result);

                    isPositiveIndicator = result.SectionAnswers.Any(x => x.ScoreLevel > 0);

                    succeed = true;

                    //Save time log
                    if (timeLog != null)
                    {
                        _screeningTimeLogService.SaveTimeLogResult(screeningResultId, timeLog);
                    }

                    //Update kiosk last activity time
                    _kioskService.ChangeLastActivityDate(result.KioskID, DateTimeOffset.Now);

                }
            }
            catch (FrontDesk.Common.Entity.NonValidEntityException ex)
            {
                _errorLoggerService.Add(ex.Message, ex.StackTrace, result.KioskID);
                succeed = false;
            }
            catch (Exception ex)
            {
                succeed = false;
                _errorLoggerService.Add(ex.Message, ex.StackTrace, result.KioskID == 0 ? (short?)null : result.KioskID);
            }
            if (succeed && screeningResultId > 0)
            {
                try
                {
                    OnScreeningResultCreated(screeningResultId);
                }
                catch (Exception ex)
                {
                    _errorLoggerService.Add("Error occured during on Screening Result Created event processing", ex.StackTrace, result.KioskID == 0 ? (short?)null : result.KioskID);

                    _errorLoggerService.Add(ex.Message, ex.StackTrace, result.KioskID == 0 ? (short?)null : result.KioskID);

                    _logger.Error("Failed Save Screening Result.", ex);
                }
            }


            bool? operationResult = succeed ? isPositiveIndicator : (bool?)null;
            return operationResult;
        }


        /// <summary>
        /// Save demographics results from the kiosk
        /// </summary>
        /// <param name="result"></param>
        /// <param name="timeLog"></param>
        /// <returns>Return true of succeed, otherwise false </returns>
        public bool SaveDemographicsResult(PatientDemographicsKioskResult result, ScreeningTimeLogRecord[] timeLog)
        {
            bool succeed = false;
            try
            {
                if (ValidateKiosk(result.KioskID))
                {
                    //get latest screening results for the patient and this kiosk
                    long? screeningResultID = _screeningResultService.GetLatestForKiosk(result.KioskID, (ScreeningPatientIdentity)result);

                    if (!screeningResultID.HasValue)
                    {
                        _errorLoggerService.Add("Error occured during patient demographics saving. Screening results not found.", string.Empty, result.KioskID);
                        return false;
                    }


                    //if demographics does not exists, create it
                    long? demographicsId = _demographicsService.Find(result);

                    if (!demographicsId.HasValue)
                    {
                        demographicsId = _demographicsService.Create(_screeningResultService.Get(screeningResultID.Value));
                    }

                    //update it

                    BhsDemographics demographicsModel = _demographicsService.Get(demographicsId.Value);

                    demographicsModel.Race = new LookupValue { Id = result.RaceId };
                    demographicsModel.Gender = new LookupValue { Id = result.GenderId };
                    demographicsModel.SexualOrientation = new LookupValue { Id = result.SexualOrientationId };
                    demographicsModel.TribalAffiliation = result.TribalAffiliation?.Trim();
                    demographicsModel.MaritalStatus = new LookupValue { Id = result.MaritalStatusId };
                    demographicsModel.EducationLevel = new LookupValue { Id = result.EducationLevelId };
                    demographicsModel.LivingOnReservation = new LookupValue { Id = result.LivingOnReservationId };
                    demographicsModel.CountyOfResidence = result.CountyOfResidence;
                    demographicsModel.MilitaryExperience = result.MilitaryExperience.Select(x => new Common.LookupValue { Id = x }).ToList();

                    var kioskUserAccount = FDUser.FindUsersByName("kiosk");


                    _demographicsService.Update(demographicsModel, kioskUserAccount);

                    succeed = true;

                    //Save time log
                    if (timeLog != null)
                    {
                        _screeningTimeLogService.SaveTimeLogResult(screeningResultID.Value, timeLog);
                    }

                    //Update kiosk last activity time
                    _kioskService.ChangeLastActivityDate(result.KioskID, DateTimeOffset.Now);

                }
            }
            catch (Exception ex)
            {
                succeed = false;
                _errorLoggerService.Add(ex.Message, ex.StackTrace, result.KioskID == 0 ? (short?)null : result.KioskID);
                _logger.Error("Failed .Saving Demographics", ex);
            }
            
            return succeed;
        }


        private bool ValidateKiosk(short kioskID)
        {
            bool succeed = true;

            var cert = _licenseService.GetActivatedLicense();

            if (cert == null)
            {
                _errorLoggerService.Add(Resources.TextMessages.KioskEndpoint_ActivationLicenseErrorMesage, null, kioskID);
                succeed = false;
            }
            else if (_kioskService.GetNotDisabledCount() > cert.License.MaxKiosks)
            {
                _errorLoggerService.Add(Resources.TextMessages.KioskEndpoint_KioskCountLicenseErrorMesage, null, kioskID);
                succeed = false;
            }
            else if (_branchLocationService.GetNotDisabledCount() > cert.License.MaxBranchLocations)
            {
                _errorLoggerService.Add(Resources.TextMessages.KioskEndpoint_LocationCountLicenseErrorMesage, null, kioskID);
                succeed = false;
            }
            else if (!_kioskService.CheckKioskIsExistsAndNotDisabled(kioskID))
            {

                string ipAddress = "Unknown";
                if (System.Web.HttpContext.Current != null)
                {
                    ipAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
                }
                //kiosk not found or has been disabled
                _errorLoggerService.Add(string.Format(Resources.TextMessages.KioskEndpoint_KioskNotFoundOrDisabled,
                    Kiosk.GetKioskIDAsString(kioskID), ipAddress), null, kioskID);

                succeed = false;
            }

            return succeed;

        }


        private void OnScreeningResultCreated(long screeningResultId)
        {
            //create bhs visit if required
            var result = ScreeningResultHelper.GetScreeningResult(screeningResultId);
            var info = ServerScreening.GetByID(result.ScreeningID);

            var visit = _bhsVisitService.Create(result, info);

            /* 
             * According to the last requirements C-SSRS should not be created automatically.
             * Keep here this block before the release
            if(visit != null)
            {
                var thinkOfDeathQuestion = visit.DepressionThinkOfDeathAnswer;

                if(string.IsNullOrEmpty(thinkOfDeathQuestion) || string.Compare(thinkOfDeathQuestion, "Not at all", true) == 0)
                {
                    return; // do not create Columbia SSRS - negative answer.
                }

                // create new C-SSRS
                _columbiaSuicideReportService.Create(visit);
            }
            */
        }
    }
}
