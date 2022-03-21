using FrontDesk.Common;
using FrontDesk.Common.Bhservice;
using FrontDesk.Server.Data;
using FrontDesk.Server.Data.BhsVisits;
using FrontDesk.Server.Extensions;
using FrontDesk.Server.Printouts.Bhs;
using FrontDesk.Server.Reports.ExcelReports;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Models.Export;

using System;
using System.Data;

namespace FrontDesk.Server.Screening.Services
{
    public class BhsExportService
    {
        private readonly IBhsDemographicsRepository _demographicsRepository;
        private readonly IBhsVisitRepository _visitRepository;
        private readonly IBhsFollowUpRepository _followUpRepository;
        private readonly IScreeningResultExportRepository _screeningRepository;


        public BhsExportService(IBhsVisitRepository bhsVisitRepository,
            IBhsDemographicsRepository bhsDemographicsRepository,
            IBhsFollowUpRepository followUpRepository,
            IScreeningResultExportRepository screeningRepository)
        {
            _visitRepository = bhsVisitRepository ?? throw new ArgumentNullException(nameof(bhsVisitRepository));
            _demographicsRepository = bhsDemographicsRepository ?? throw new ArgumentNullException(nameof(bhsDemographicsRepository));
            _followUpRepository = followUpRepository ?? throw new ArgumentNullException(nameof(followUpRepository));
            _screeningRepository = screeningRepository ?? throw new ArgumentNullException(nameof(screeningRepository));
        }

        public BhsExportService() : this(new BhsVisitDb(), new BhsDemographicsDb(), new BhsFollowUpDb(), new ScreeningResultExportDb())
        {

        }

        public BhsReportExport GetReports(BhsExportFilterModel filter)
        {
            BhsReportExport result = new BhsReportExport();

            var searchFilter = new BhsSearchFilterModel
            {
                StartDate = filter.StartDate,
                EndDate = filter.EndDate,
                LocationId = filter.LocationId
            };
            if (filter.IncludeScreenings)
            {

                Func<SimpleFilterModel, DataTable> getScreeningsMethod = filter.UniquePatientMode ?
                    (Func<SimpleFilterModel, DataTable>)_screeningRepository.GetUniqueScreeningsResultsForExcelExport :
                    _screeningRepository.GetScreeningsResultsForExcelExport;


                result.Screenings = getScreeningsMethod(new SimpleFilterModel
                {
                    StartDate = searchFilter.StartDate,
                    EndDate = searchFilter.EndDate,
                    Location = searchFilter.LocationId
                });


            }
            if (filter.IncludeVisits)
            {
                result.Visits = _visitRepository.GetAllForExport(searchFilter);
            }
            if (filter.IncludeFollowUps)
            {
                result.FollowUps = _followUpRepository.GetAllForExport(searchFilter);
            }
            if (filter.IncludeDemographics)
            {
                result.Demographics = _demographicsRepository.GetAllItems(searchFilter);
            }
            if (filter.IncludeDrugsOfChoice)
            {
                Func<SimpleFilterModel, DataTable> getDataMethod = filter.UniquePatientMode ?
                    (Func<SimpleFilterModel, DataTable>)_screeningRepository.GetUniqueDrugsOfChoiceScreeningsResultsForExcelExport :
                    _screeningRepository.GetDrugsOfChoiceScreeningsResultsForExcelExport;



                result.DrugsOfChoice = getDataMethod(new SimpleFilterModel
                {
                    StartDate = searchFilter.StartDate,
                    EndDate = searchFilter.EndDate,
                    Location = searchFilter.LocationId
                });
            }

            if (filter.IncludeCombined)
            {
                if (!filter.UniquePatientMode)
                {
                    // when it's a Total Records mode report - return warning message

                    result.Combined.Columns.Add(new DataColumn
                    {
                        ColumnName = "Warning",
                        DataType = typeof(string)
                    });
                    result.Combined.Rows.Add(Resources.TextMessages.REPORT_EXCEL_COMBINED_REQUIRED_UNIQUE_PATIENTS);
                }
                else
                {
                    result.Combined = _screeningRepository.GetUniqueCombinedResultsForExcelExport(new SimpleFilterModel
                    {
                        StartDate = searchFilter.StartDate,
                        EndDate = searchFilter.EndDate,
                        Location = searchFilter.LocationId
                    });

                }
            }


            return result;
        }

    }
}
