using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using FrontDesk.Common.Screening;
using FrontDesk.Server.Data.BhsVisits;
using FrontDesk.Server.Screening.Models;

namespace FrontDesk.Server.Screening.Services
{
    public interface IScreeningTimeLogService
    {
        bool SaveTimeLogResult(long screeningResultID, ScreeningTimeLogRecord[] timeLog);
        ScreeningTimeLogReportViewModel GetReport(SimpleFilterModel filter);
    }


    public class ScreeningTimeLogService : IScreeningTimeLogService
    {
        private readonly IScreeningTimeLogRepository _repository;
        private readonly IScreeningDefinitionService _screeningDefinitionService;
        private ILog _logger = LogManager.GetLogger<ScreeningTimeLogService>();

        public ScreeningTimeLogService(IScreeningTimeLogRepository repository, IScreeningDefinitionService screeningDefinitionService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _screeningDefinitionService = screeningDefinitionService ?? throw new ArgumentNullException(nameof(screeningDefinitionService));
        }

        public ScreeningTimeLogService() : this(new ScreeningTimeLogDb(), new ScreeningDefinitionService())
        {

        }


        public bool SaveTimeLogResult(long screeningResultID, ScreeningTimeLogRecord[] timeLog)
        {
            timeLog = timeLog ?? new ScreeningTimeLogRecord[0];

            var result = _repository.Add(screeningResultID, timeLog);

            if (_logger.IsInfoEnabled)
            {
                _logger.InfoFormat("Saved screening time log for ID: {0}.Succeed: {1}", screeningResultID, result);
            }

            return result;
        }

        public ScreeningTimeLogReportViewModel GetReport(SimpleFilterModel filter)
        {
            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            ScreeningTimeLogReportViewModel model = new ScreeningTimeLogReportViewModel();

            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date;
            }
            if (filter.StartDate.HasValue)
            {
                filter.StartDate = filter.StartDate.Value.Date;
            }

            //render all sections
            var allSections = _screeningDefinitionService.GetSections();
           
            var screeningItems = _repository.GetReport(filter);

            foreach (var sectionItem in allSections)
            {
                var item = screeningItems.FirstOrDefault(x => x.ScreeningSectionID == sectionItem.ScreeningSectionID);
                item = item ?? new ScreeningTimeLogReportItem
                {
                    ScreeningSectionID = sectionItem.ScreeningSectionID,
                    ScreeningSectionName = sectionItem.ScreeningSectionName
                };

                //rename DOCH label
                if (item.ScreeningSectionID == ScreeningSectionDescriptor.DrugOfChoice)
                {
                    item.ScreeningSectionName = ScreeningSectionDescriptor.DrugOfChoiceSectionSettingsName;
                }
                else if (item.ScreeningSectionID == ScreeningSectionDescriptor.Depression)
                {
                    item.ScreeningSectionName = Resources.Labels.ScreenTimeLog_Phq2_Name;
                }
                else if (item.ScreeningSectionID == ScreeningSectionDescriptor.Anxiety)
                {
                    item.ScreeningSectionName = Resources.Labels.ScreenTimeLog_Gad2_Name;
                }

                model.Items.Add(item);
            }

            model.Items.Add(screeningItems.FirstOrDefault(x => x.ScreeningSectionID == string.Empty) ?? new ScreeningTimeLogReportItem());

            return model;
        }
    }
}
