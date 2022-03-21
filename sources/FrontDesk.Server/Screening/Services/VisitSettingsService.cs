using FrontDesk.Common.InfrastructureServices;
using FrontDesk.Configuration;
using FrontDesk.Server.Data.Configuration;
using FrontDesk.Server.Screening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening.Services
{
    public interface IVisitSettingsService
    {
        List<VisitSettingItem> Get(string filterById);
        List<VisitSettingItem> GetAll();
        void Update(List<VisitSettingItem> settings);
    }

    public class VisitSettingsService : IVisitSettingsService
    {
        private readonly IVisitSettingsRepository _repository;
        private readonly ITimeService _timeService;

        public VisitSettingsService(IVisitSettingsRepository repository, ITimeService timeService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
        }


        public VisitSettingsService() : this(new VisitSettingsDatabase(), new TimeService()) { }

        public void Update(List<VisitSettingItem> settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            foreach(var item in settings)
            {
                item.LastModifiedDateUTC = _timeService.GetUtcNow();
            }

            _repository.Update(settings);
        }

        public List<VisitSettingItem> GetAll()
        {
            var results = _repository.GetAll();

            foreach (var item in results.Where(x => x.Id == VisitSettingsDescriptor.Depression))
            {
                item.Name = FrontDesk.Properties.Resources.ScreeningFrequency_PHQ9A_Name;
            }

            return results;
        }

        public List<VisitSettingItem> Get(string filterById)
        {
            return _repository.Get(filterById);
        }

    }
}
