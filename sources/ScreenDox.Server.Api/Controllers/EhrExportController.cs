using AutoMapper;

using CacheCow.Server.WebApi;

using Common.Logging;

using Frontdesk.Server.SmartExport.EhrInterfaceService;

using FrontDesk;
using FrontDesk.Common.Entity;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Screening.Mappers;
using FrontDesk.Server.Screening.Services;

using RPMS.Common.Models;

using ScreenDox.Server.Api.Infrastructure;
using ScreenDox.Server.Logging;
using ScreenDox.Server.Models;
using ScreenDox.Server.Models.ViewModels;
using ScreenDox.Server.Security;

using System;
using System.Linq;
using System.Web.Http;

namespace ScreenDox.Server.Api.Controllers
{
    public class EhrExportController : ApiController
    {
        private readonly ILog _logger = LogManager.GetLogger<EhrExportController>();
        private readonly IScreeningResultService _screenService;
        private readonly IScreeningDefinitionService _screeningInfoService;
        private readonly IUserPrincipalService _userService;
        private readonly IEhrInterfaceProxy _proxy;
        private readonly ISecurityLogService _securityLogService;

        public EhrExportController(IScreeningResultService screenService,
                                   IEhrInterfaceProxy proxy,
                                   IUserPrincipalService userService,
                                   ISecurityLogService securityLogService,
                                   IScreeningDefinitionService screeningInfoService)
        {
            _screenService = screenService ?? throw new ArgumentNullException(nameof(screenService));
            _proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _securityLogService = securityLogService ?? throw new ArgumentNullException(nameof(securityLogService));
            _screeningInfoService = screeningInfoService ?? throw new ArgumentNullException(nameof(screeningInfoService));
        }


        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpPost]
        [Route("api/ehrexport/patient/{screeningResultId}")]
        public SearchResponse<PatientInfoMatch> FindMatchedPatients(long screeningResultId, [FromBody] PagedSearchFilter filter)
        {
            filter = filter ?? new PagedSearchFilter();
            if (filter.MaximumRows == 0)
            {
                filter.MaximumRows = 10;
            };

            var result = new SearchResponse<PatientInfoMatch>();


            var screeningResult = _screenService.Get(screeningResultId);

            screeningResult.ShouldNotBeNull();
            try
            {
                result.TotalCount = _proxy.GetPatientCount(screeningResult);

                result.Items = _proxy.GetMatchedPatients(screeningResult, filter.StartRowIndex, filter.MaximumRows)
                    .Select(x =>
                    {
                        var item = Mapper.Map<PatientInfoMatch>(x);
                        item.SetNotMatchesFields(screeningResult);
                        return item;
                    }).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error("[Find Address] Failed to Find Patients in EHR", ex);

                ResponseDataFactory.DependencyError("EHR service is unavailable. Please try again later or contact Administrator.");
            }

            return result;
        }


        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpPost]
        [Route("api/ehrexport/visit/{patientId}")]
        public SearchResponse<VisitMatch> FindPatientVisits(int patientId, [FromBody] PagedSearchFilter filter)
        {
            filter = filter ?? new PagedSearchFilter();
            if (filter.MaximumRows == 0)
            {
                filter.MaximumRows = 10;
            };

            patientId.ShouldNotBeDefault();

            var result = new SearchResponse<VisitMatch>();


            try
            {
                var ehrPatient = _proxy.GetPatientRecord(new RPMS.Common.Models.PatientSearch
                {
                    ID = patientId
                });

                var visits = _proxy.GetScheduledVisitsByPatient(ehrPatient, filter.StartRowIndex, filter.MaximumRows);

                result.TotalCount = _proxy.GetScheduledVisitsByPatientCount(ehrPatient);

                result.Items = _proxy.GetScheduledVisitsByPatient(ehrPatient, filter.StartRowIndex, filter.MaximumRows)
                    .Select(x => Mapper.Map<VisitMatch>(x)).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error("[Find Visit] Failed to Find Scheduled Visits in EHR", ex);

                ResponseDataFactory.DependencyError("EHR service is unavailable. Please try again later or contact Administrator.");
            }

            return result;
        }

        [HttpPost]
        [Route("api/ehrexport/result/{screeningResultId}")]
        public EhrExportResponse ExportToEhr([FromUri] long screeningResultId, [FromBody] EhrExportRequest request)
        {
            screeningResultId.ShouldNotBeDefault();
            var principle = _userService.GetCurrent();


            var result = new EhrExportResponse
            {
                Status = ExportOperationStatus.Unknown
            };
            // Check if record has been exported already

            try
            {
                var patientSearch = new PatientSearch
                {
                    ID = request.PatientId
                };


                var ehrPatient = _proxy.GetPatientRecord(patientSearch);
                var ehrVisit = _proxy.GetScheduledVisit(request.VisitId, patientSearch);
                var screeningResult = _screenService.Get(screeningResultId);

                ehrPatient.ShouldNotBeNull(request.PatientId);
                ehrVisit.ShouldNotBeNull(request.VisitId);
                screeningResult.ShouldNotBeNull(screeningResultId);

                result.ExportScope = _proxy.PreviewExportResult(screeningResult, ehrPatient, ehrVisit.ID);

                // apply export scope
                result.ExportResults = _proxy.CommitExportTask(ehrPatient.ID, ehrVisit.ID, result.ExportScope);

                // export screening result payload
                result.ExportResults.AddRange(_proxy.CommitExportResult(ehrPatient.ID, ehrVisit.ID,
                    screeningResult, _screeningInfoService.Get()));

                result.Status = result.ExportResults.GetExportOperationStatus();


                _screenService.UpdateExportInfo(screeningResult, result.Status, ehrPatient, ehrVisit, principle.UserID);

                if (result.Status == ExportOperationStatus.AllSucceed || result.Status == ExportOperationStatus.SomeOperationsFailed)
                {

                    BhsPatientAddressmapper.ImportPatientAddressFromEhr(screeningResult, ehrPatient);
                    new BhsVisitService().FullfilPatientAddress(screeningResult);


                    _securityLogService.Add(new SecurityLog(SecurityEvents.EditPatientContactInformation,
                        String.Format("#{0}, {1} => EHR:{2}, Address: {3}",
                            screeningResult.ID,
                            screeningResult.FullName,
                            screeningResult.ExportedToHRN,
                            screeningResult.AddressToString()
                            ),
                        screeningResult.LocationID));
                }


            }
            catch (NonValidEntityException ex)
            {

                result.Errors.Add(ErrorLog.AddServerException(ScreenDox.Server.Resources.TextMessages.FailedToUpdateFDExportDetails, ex).ErrorMessage);


                result.Status = result.Status == ExportOperationStatus.Unknown ? //exception happned before getting export status
                    ExportOperationStatus.AllFailed :
                    ExportOperationStatus.SomeOperationsFailed;

            }
            catch (Exception ex)
            {
                result.Errors.Add(ErrorLog.AddServerException(ScreenDox.Server.Resources.TextMessages.ExportFailed, ex).ErrorMessage);

                result.Status = result.Status == ExportOperationStatus.Unknown ? //exception happned before getting export status
                   ExportOperationStatus.AllFailed :
                   ExportOperationStatus.SomeOperationsFailed;

            }

            return result;
        }

    }
}