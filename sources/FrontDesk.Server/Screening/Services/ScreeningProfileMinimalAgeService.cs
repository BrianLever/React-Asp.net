namespace FrontDesk.Server.Screening.Services
{
    using FrontDesk.Common.InfrastructureServices;
    using FrontDesk.Configuration;
    using FrontDesk.Server.Data.ScreeningProfile;

    using System;
    using System.Collections.Generic;
    using System.Linq;


    public class ScreeningProfileMinimalAgeService : IScreeningProfileMinimalAgeService
    {
        private IScreeningProfileMinimalAgeRepository _repository;
        private readonly ITimeService _timeService;

        public ScreeningProfileMinimalAgeService(IScreeningProfileMinimalAgeRepository repository,
                                                 ITimeService timeService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
        }

        public ScreeningProfileMinimalAgeService() : 
            this(new ScreeningProfileMinimalAgeDatabase(),
                new TimeService()) { }

        /// <summary>
        /// Save changes to the Section Minimal Age parameters
        /// </summary>
        public void UpdateMinimalAgeSettings(int screeningProfileId, 
            ICollection<ScreeningSectionAge> ageSettings)
        {
            if (ageSettings == null) return;

            //validate alternatives
            ValidateAndFixEnableStatusForAlternatives(ageSettings);
            ValidateAndFixMinimalAgeStatusForDepentencies(ageSettings);

            //set last modified date
            foreach( var item in ageSettings)
            {
                item.LastModifiedDateUTC = _timeService.GetUtcNow();
            }

            _repository.UpdateMinimalAgeSettings(screeningProfileId, ageSettings);
        }

        /// <summary>
        /// Get Minimal Age settings for all screening section
        /// </summary>
        public IList<ScreeningSectionAge> GetSectionMinimalAgeSettings(int screeningProfileId)
        {
            return GetSectionMinimalAgeSettings(screeningProfileId, null);
        }

        /// <summary>
        /// Get Minimal Age settings for particular screening section
        /// </summary>
        public IList<ScreeningSectionAge> GetSectionMinimalAgeSettings(int screeningProfileId, string screeningSectionID)
        {
            var result = _repository.GetSectionMinimalAgeSettings(screeningProfileId, screeningSectionID);

            foreach (var item in result)
            {
                switch (item.ScreeningSectionID)
                {
                    //rename DOCH label
                    case ScreeningSectionDescriptor.DrugOfChoice:
                        item.ScreeningSectionLabel = ScreeningSectionDescriptor.DrugOfChoiceSectionSettingsName;
                        break;
                    case ScreeningSectionDescriptor.Depression:
                    case ScreeningSectionDescriptor.DepressionAllQuestions:
                        item.ScreeningSectionLabel = ScreeningFrequencyDescriptor.GetName(item.ScreeningSectionID);
                        break;
                    case ScreeningSectionDescriptor.Anxiety:
                    case ScreeningSectionDescriptor.AnxietyAllQuestions:
                        item.ScreeningSectionLabel = ScreeningFrequencyDescriptor.GetName(item.ScreeningSectionID);
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        private void ValidateAndFixEnableStatusForAlternatives(ICollection<ScreeningSectionAge> ageSettings)
        {
            var alternatives = ScreeningSectionAge.GetScreeningMinimalAgeGroups();

            foreach (var item in alternatives)
            {
                if (string.IsNullOrEmpty(item.AlternativeSectionID)) continue;

                var primaryAgeSetting = ageSettings
                    .FirstOrDefault(x => x.ScreeningSectionID == item.PrimarySectionID);

                if (primaryAgeSetting == null) continue;

                var alternativeAgeSetting = ageSettings
                .FirstOrDefault(x => x.ScreeningSectionID == item.AlternativeSectionID);

                if (alternativeAgeSetting == null) continue;

                if (primaryAgeSetting.IsEnabled && alternativeAgeSetting.IsEnabled)
                {
                    alternativeAgeSetting.IsEnabled = false;
                }
            }
        }

        private void ValidateAndFixMinimalAgeStatusForDepentencies(ICollection<ScreeningSectionAge> ageSettings)
        {
            var groups = ScreeningSectionAge.GetScreeningMinimalAgeGroups();

            foreach (var item in groups)
            {

                var primaryAgeSetting = ageSettings
                    .FirstOrDefault(x => x.ScreeningSectionID == item.PrimarySectionID);

                if (primaryAgeSetting == null) continue;

                foreach (var dependentID in item.DependentSectionIDs)
                {

                    var depententAgeSetting = ageSettings
                    .FirstOrDefault(x => x.ScreeningSectionID == dependentID);

                    if (depententAgeSetting == null) continue;

                    depententAgeSetting.MinimalAge = primaryAgeSetting.MinimalAge;

                }
            }
        }
    }
}
