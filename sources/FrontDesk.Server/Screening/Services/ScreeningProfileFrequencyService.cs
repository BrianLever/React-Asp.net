using FrontDesk.Common.Entity;
using FrontDesk.Common.InfrastructureServices;
using FrontDesk.Server.Data;
using FrontDesk.Server.Data.ScreenngProfile;
using FrontDesk.Server.Resources;

using System;
using System.Collections.Generic;

namespace FrontDesk.Configuration
{
    public class ScreeningProfileFrequencyService : IScreeningProfileFrequencyService
    {
        private readonly IScreeningProfileFrequencyRepository _frequencyRepository;
        private readonly ITimeService _timeService;

        public ScreeningProfileFrequencyService(
            IScreeningProfileFrequencyRepository frequencyRepository,
            ITimeService timeService)
        {
            _frequencyRepository = frequencyRepository ?? throw new ArgumentNullException(nameof(frequencyRepository));
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
        }


        public void Save(int screeningProfileId, ScreeningFrequencyItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            //validate 

            IScreeningParameterValidator<ScreeningFrequencyItem> validator = ScreeningFrequencyValidatorFactory.Create(item.ID);

            if (!validator.Validate(item))
            {
                throw new NonValidEntityException(validator.Error);
            }

            item.LastModifiedDateUTC = _timeService.GetUtcNow();

            _frequencyRepository.Save(screeningProfileId, new ScreeningFrequencyItem[] { item });
        }

        public void Save(int screeningProfileId, IEnumerable<ScreeningFrequencyItem> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (var item in items)
            {
                IScreeningParameterValidator<ScreeningFrequencyItem> validator = ScreeningFrequencyValidatorFactory.Create(item.ID);
                if (!validator.Validate(item))
                {
                    throw new NonValidEntityException(validator.Error);
                }
            }

            foreach(var item in items)
            {
                item.LastModifiedDateUTC = _timeService.GetUtcNow();
            }

            _frequencyRepository.Save(screeningProfileId, items);
        }

        public ScreeningFrequencyItem Get(int screeningProfileId, string ID)
        {
            return _frequencyRepository.Get(screeningProfileId, ID);
        }

        public IEnumerable<ScreeningFrequencyItem> GetAll(int screeningProfileId)
        {
            return _frequencyRepository.GetAll(screeningProfileId);
        }

        /// <summary>
        /// Get all supported frequency values
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetSupportedValues()
        {
            return new Dictionary<int, string> {
                { 0, TextMessages.Frequency_EveryVisit },
                { 1, TextMessages.Frequency_Daily },
                { 7, TextMessages.Frequency_Weekly },
                { 100, TextMessages.Frequency_Monthly },
                { 200, TextMessages.Frequency_BiMonthly },
                { 300, TextMessages.Frequency_Quarterly },
                { 1200, TextMessages.Frequency_Annually },
                { 240000, TextMessages.Frequency_Once }
            };
        }
    }
}
