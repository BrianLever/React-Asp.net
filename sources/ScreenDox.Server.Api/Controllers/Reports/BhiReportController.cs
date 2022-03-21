
using CacheCow.Server.WebApi;

using Common.Logging;

using FrontDesk.Common.Screening;
using FrontDesk.Server;
using FrontDesk.Server.Data.BhsVisits;
using FrontDesk.Server.Printouts.Bhs;
using FrontDesk.Server.Reports;
using FrontDesk.Server.Reports.ExcelReports;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Models.Export;
using FrontDesk.Server.Screening.Services;

using ScreenDox.Server.Api.Infrastructure;
using ScreenDox.Server.Logging;
using ScreenDox.Server.Models.FilterParams;
using ScreenDox.Server.Models.SearchFilters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ScreenDox.Server.Api.Controllers.Reports
{
    /// <summary>
    /// BHI Reports
    /// </summary>
    public class BhiReportController : ApiController
    {
        private readonly IIndicatorReportService _indicatorReportService;
        private readonly IScreeningDefinitionService _screeningInfoService;
        private readonly IScreeningAgesSettingsProvider _ageGroupsProvider;
        private readonly IScreeningResultService _screeningResultService;
        private readonly LookupListsDataSource lookupListsDataSource = new LookupListsDataSource();
        private readonly IBranchLocationService _branchLocationService;
        private readonly BranchLocationService branchLocationService1 = new BranchLocationService();
        private readonly ISecurityLogService _securityLogService;
        private readonly BhsDemographicsIndicatorReportService bhsDemographics=new BhsDemographicsIndicatorReportService();
        private readonly BhsFollowUpIndicatorReportService bhsFollowUpIndicatorReportService = new BhsFollowUpIndicatorReportService();
        private readonly ScreeningTimeLogService screeningTimeLogService = new ScreeningTimeLogService();       
        private readonly BhsVisitIndicatorReportService bhsVisitIndicatorReportService = new BhsVisitIndicatorReportService();
        private readonly BhsExportService bhsExportService = new BhsExportService();
        private readonly ScreeningResultService screeningResultService1 = new ScreeningResultService();
        private readonly ILog _logger = LogManager.GetLogger<BhiReportController>();


        /// <summary>
        /// Default constructor
        /// </summary>
        public BhiReportController(IIndicatorReportService indicatorReportService,
                                   IScreeningDefinitionService screeningInfoService,
                                   IScreeningAgesSettingsProvider ageGroupsProvider,
                                   ISecurityLogService securityLogService,
                                   IScreeningResultService screeningResultService,
                                   IBranchLocationService branchLocationService)
        {
            _indicatorReportService = indicatorReportService ?? throw new ArgumentNullException(nameof(indicatorReportService));
            _securityLogService = securityLogService ?? throw new ArgumentNullException(nameof(securityLogService));
            _screeningInfoService = screeningInfoService ?? throw new ArgumentNullException(nameof(screeningInfoService));
            _ageGroupsProvider = ageGroupsProvider ?? throw new ArgumentNullException(nameof(ageGroupsProvider));
            _screeningResultService = screeningResultService ?? throw new ArgumentNullException(nameof(screeningResultService));
            _branchLocationService = branchLocationService ?? throw new ArgumentNullException(nameof(branchLocationService));
          
        }       

        /// <summary>
        /// Get screening report by age
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/bhireport/byage")]
        public IndicatorReportByAgeViewModel GetByAge([FromBody] BhiReportFilter filter)
        {
            filter = filter ?? new BhiReportFilter();

            var screeningInfo = _screeningInfoService.Get();
            if (filter.EndDate != null)
            {
                filter.EndDate = filter.EndDate.Value.AddDays(1); // adds 1 day to include current date into results
            }


            var model = _indicatorReportService.GetBhsIndicatorReportByAge(
                screeningInfo,
               filter,
                _ageGroupsProvider.AgeGroups,
                filter.RenderUniquePatientsReportType);

            model.ShouldNotBeNull();

            return model;
        }

        /// <summary>
        /// Get pdf version of report by age
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Pdf file.</returns>
        [Route("api/bhireport/byage/print")]
        [HttpPost]
        public HttpResponseMessage PrintByAge([FromBody] BhiReportFilter filter)
        {
            filter = filter ?? new BhiReportFilter();


            //Print report into context
            var pdfReport = new BhsIndicatorByAgePdfReport(
                filter,
                filter.RenderUniquePatientsReportType,
                _screeningResultService,
                _screeningInfoService,
                _branchLocationService,
                _ageGroupsProvider
                );

            var response = Request.CreateResponse(HttpStatusCode.OK);

            pdfReport.CreatePDF(response, "IR_ScreeningResultsByAge.pdf");

            return response;
        }

        /// <summary>
        /// Get screening report by problem
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/bhireport/byproblem")]
        public IndicatorReportViewModel GetByProblem([FromBody] BhiReportFilter filter)
        {
            Console.WriteLine(filter.EndDate);
            filter = filter ?? new BhiReportFilter();            
            var screeningInfo = _screeningInfoService.Get();
            if (filter.EndDate != null)
            {
                filter.EndDate = filter.EndDate.Value.AddDays(1); // adds 1 day to include current date into results
            }


            var model = _indicatorReportService.GetBhsIndicatorReportByProblem(
                screeningInfo,
               filter,
               filter.RenderUniquePatientsReportType);

            model.ShouldNotBeNull();

            return model;
        }

        /// <summary>
        /// Get pdf version of report by problem
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Pdf file.</returns>
        [Route("api/bhireport/byproblem/print")]
        [HttpPost]
        public HttpResponseMessage PrintByProblem([FromBody] BhiReportFilter filter)
        {
            filter = filter ?? new BhiReportFilter();            

            //Print report into context
            var pdfReport = new BhsIndicatorPdfReport(
                filter,
                filter.RenderUniquePatientsReportType,
                _screeningResultService,
                _screeningInfoService,
                _branchLocationService
                );

            var response = Request.CreateResponse(HttpStatusCode.OK);

            pdfReport.CreatePDF(response, "IR_ScreeningResultsByProblem.pdf");

            return response;
        }

        /// <summary>
        /// Get data for Drugs Use by Age report
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/bhireport/drugusebyage")]
        public IndicatorReportByAgeViewModel GetDrugByAge([FromBody] BhiReportFilter filter)
        {
            filter = filter ?? new BhiReportFilter();

            var screeningInfo = _screeningInfoService.Get();
            if (filter.EndDate != null)
            {
                filter.EndDate = filter.EndDate.Value.AddDays(1); // adds 1 day to include current date into results
            }


            var model = _indicatorReportService.GetDrugsOfChoiceByAge(
                screeningInfo,
               filter,
                _ageGroupsProvider.AgeGroups,
                filter.RenderUniquePatientsReportType);

            model.ShouldNotBeNull();

            return model;
        }

        /// <summary>
        /// Get pdf version of drug use by age
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Pdf file.</returns>
        [HttpCache(DefaultExpirySeconds = 0)] // cache disabled
        [Route("api/bhireport/drugusebyage/print")]
        [HttpPost]
        public HttpResponseMessage PrintDrugByAge([FromBody] BhiReportFilter filter)
        {
            filter = filter ?? new BhiReportFilter();


            //Print report into context
            var pdfReport = new DrugsOfChoiceByAgePdfReport(
                filter,
                filter.RenderUniquePatientsReportType,
                _screeningResultService,
                _screeningInfoService,
                _branchLocationService
                );

            var response = Request.CreateResponse(HttpStatusCode.OK);

            pdfReport.CreatePDF(response, "IR_DrugUseResultsByAge.pdf");

            return response;
        }

        /// <summary>
        /// Get data for Patient Demographics by Age report
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/bhireport/patientdemographics")]
        public BhsDemographicsReportByAgeViewModel GetPatientDemographicsReport([FromBody] BhiReportFilter filter)
        {
            filter = filter ?? new BhiReportFilter();
          
            if (filter.EndDate != null)
            {
                filter.EndDate = filter.EndDate.Value.AddDays(1); // adds 1 day to include current date into results
            }

            var model = bhsDemographics.GetDemographicsReportByAge(           
                 filter,
                _ageGroupsProvider.AgeGroups,
                 filter.RenderUniquePatientsReportType);
                 model.ShouldNotBeNull();

            return model;
        }

        /// <summary>
        /// Get pdf version of Patient Demographics by age
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Pdf file.</returns>
        [HttpCache(DefaultExpirySeconds = 0)] // cache disabled
        [Route("api/bhireport/patientdemographics/print")]
        [HttpPost]
        public HttpResponseMessage PrintPatientDemographics([FromBody] BhiReportFilter filter)
        {
            filter = filter ?? new BhiReportFilter();

            //Print report into context
            var pdfReport = new BhsDemopgraphicsVisitsPdfReport(
                filter,
                filter.RenderUniquePatientsReportType,
                _screeningResultService,
                _screeningInfoService,
                branchLocationService1
                );

            var response = Request.CreateResponse(HttpStatusCode.OK);

            pdfReport.CreatePDF(response, "IR_PatientDemographics-BHSVisits.pdf");

            return response;
        }

        /// <summary>
        /// Get data for FollowUp Outcomes by Age report
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/bhireport/followupOutcomes")]
        public BhsFollowUpOutcomesViewModel GetFollowUpOutcomesReport([FromBody] BhiReportFilter filter)
        {
            filter = filter ?? new BhiReportFilter();

            if (filter.EndDate != null)
            {
                filter.EndDate = filter.EndDate.Value.AddDays(1); // adds 1 day to include current date into results
            }

            var model = bhsFollowUpIndicatorReportService.GetOutcomesReportByAge(
                 filter,
                _ageGroupsProvider.AgeGroups,
                 filter.RenderUniquePatientsReportType);
            model.ShouldNotBeNull();

            return model;
        }

        /// <summary>
        /// Get pdf version of followupOutcomes by age
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Pdf file.</returns>
        [HttpCache(DefaultExpirySeconds = 0)] // cache disabled
        [Route("api/bhireport/followupOutcomes/print")]
        [HttpPost]
        public HttpResponseMessage PrintfollowupOutcomes([FromBody] BhiReportFilter filter)
        {
            filter = filter ?? new BhiReportFilter();

            //Print report into context
            var pdfReport = new BhsFollowUpOutcomesPdfReport(
                filter,
                filter.RenderUniquePatientsReportType,
                _screeningResultService,
                _screeningInfoService,
                branchLocationService1
                );

            var response = Request.CreateResponse(HttpStatusCode.OK);

            pdfReport.CreatePDF(response, "IR_BhsFollowUpOutcomes.pdf");

            return response;
        }

        /// <summary>
        /// Get data for Screen Time Log
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/bhireport/screentimelog")]
        public ScreeningTimeLogReportViewModel GetScreenTimeLog([FromBody] BhiReportFilter filter)
        {
            filter = filter ?? new BhiReportFilter();

            if (filter.EndDate != null)
            {
                filter.EndDate = filter.EndDate.Value.AddDays(1); // adds 1 day to include current date into results
            }

            var model = screeningTimeLogService.GetReport(filter);
            model.ShouldNotBeNull();

            return model;
        }

        /// <summary>
        /// Get pdf version of Screen Time Log
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Pdf file.</returns>
        [HttpCache(DefaultExpirySeconds = 0)] // cache disabled
        [Route("api/bhireport/ScreenTimeLog/print")]
        [HttpPost]
        public HttpResponseMessage PrintScreenTimeLog([FromBody] BhiReportFilter filter)
        {
            filter = filter ?? new BhiReportFilter();

            //Print report into context
            var pdfReport = new ScreenTimeLogPdfReport( filter);

            var response = Request.CreateResponse(HttpStatusCode.OK);

            pdfReport.CreatePDF(response, "IR_ScreenTimeLog.pdf");

            return response;
        }

        /// <summary>
        /// Get data for Visits Outcomes by Age report
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/bhireport/visitsOutcomes")]
        public BhsVisitOutcomesViewModel GetVisitsOutcomesReport([FromBody] BhiReportFilter filter)
        {           
            filter = filter ?? new BhiReportFilter();

            if (filter.EndDate != null)
            {
                filter.EndDate = filter.EndDate.Value.AddDays(1); // adds 1 day to include current date into results
            }

            var model = bhsVisitIndicatorReportService.GetTreatmentOutcomesReportByAge(
                 filter,
                _ageGroupsProvider.AgeGroups,
                 filter.RenderUniquePatientsReportType);
            model.ShouldNotBeNull();

            return model;
        }

        /// <summary>
        /// Get pdf version of Visits Outcomes by age
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Pdf file.</returns>
        [HttpCache(DefaultExpirySeconds = 0)] // cache disabled
        [Route("api/bhireport/visitsOutcomes/print")]
        [HttpPost]
        public HttpResponseMessage PrintVisitsOutcomes([FromBody] BhiReportFilter filter)
        {
            filter = filter ?? new BhiReportFilter();

            //Print report into context
            var pdfReport = new BhsVisitOutcomesPdfReport(
                filter,
                filter.RenderUniquePatientsReportType,
                _screeningResultService,
                _screeningInfoService,
                branchLocationService1
                );

            var response = Request.CreateResponse(HttpStatusCode.OK);

            pdfReport.CreatePDF(response, "IR_BhsVisitsOutcomes.pdf");

            return response;
        }

        /// <summary>
        /// Get excel export 
        /// </summary>
        /// <returns>Excel file.</returns>
        [Route("api/bhireport/export")]
        [HttpPost]
        public HttpResponseMessage Export([FromBody] BhsExportFilterModel filter) {            
            var report = bhsExportService.GetReports(filter);           
            BhsReportExcelReport excelReport = new BhsReportExcelReport(report);

            var response = Request.CreateResponse(HttpStatusCode.OK);

            excelReport.Create(
                response, "Export_Reports.xlsx");

            return response;
        }

        /// <summary>
        /// Get screening result by sort
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/bhireport/bysort")]
        public List<BhsCheckInItemPrintoutModel> GetBySort([FromBody] ScreeningResultFilterModel filter) {
           
            filter = filter ?? new ScreeningResultFilterModel();
            // Console.WriteLine(filter.ProblemScoreFilter);
            var model = screeningResultService1.GetPatientScreeningsBySort(filter);
            model.ShouldNotBeNull();
            return model;
        }

        /// <summary>
        /// Get screening result
        /// Get pdf version of report by sort
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Pdf file.</returns>
        [Route("api/bhireport/bysort/print")]
        [HttpPost]
        public HttpResponseMessage PrintBySort([FromBody] ScreeningResultFilterModel filter)
        {
            filter = filter ?? new ScreeningResultFilterModel();

            //Print report into context           
            var pdfReport = new BhsCheckInBySortPdfPrintout(                            
               _screeningResultService,
               lookupListsDataSource,
               filter,              
               branchLocationService1
               );
            var response = Request.CreateResponse(HttpStatusCode.OK);
            pdfReport.CreatePDF(response, "IR_ScreeningResultsBySort.pdf");
            return response;
        }


        /// <summary>
        /// Get screening result by score level
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/bhireport/byscorelevel")]
        public ScreeningsByScoreLevelCountResult GetByScoreLevel([FromBody] SimpleFilterModel filter)        {

            filter = filter ?? new SimpleFilterModel();
            var model = screeningResultService1.GetScreeningsCountByScoreLevel(filter);
            model.ShouldNotBeNull();
            return model;
        }

        /// <summary>
        /// Get screening result by problemSort
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/bhireport/byproblemsort")]
        public List<PatientCheckInViewModel> GetByProblemSort([FromBody] ScreeningResultFilterModel filter){

            filter = filter ?? new ScreeningResultFilterModel();
            var model = screeningResultService1.GetRelatedPatientScreeningsByProblemSort(10,filter);
            model.ShouldNotBeNull();
            return model;
        }
    }
}
