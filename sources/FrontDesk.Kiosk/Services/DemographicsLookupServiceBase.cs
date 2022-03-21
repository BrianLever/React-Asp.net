using System.Collections.Generic;
using System.Linq;
using FrontDesk.Common;
using FrontDesk.Kiosk.Repositories;

namespace FrontDesk.Kiosk.Services
{
    public abstract class DemographicsLookupServiceBase : TypeaheadDataSourceBase<LookupValue>, ILookupDataSourceBase
    {
        private readonly string _screenName;

        public string ScreenName { get { return _screenName; } }

        public DemographicsLookupServiceBase(string screenName)
        {
            if (string.IsNullOrEmpty(screenName))
            {
                throw new System.ArgumentException("message", nameof(screenName));
            }

            _screenName = screenName;
        }
        protected override string CacheKey => _screenName;

        protected override List<LookupValue> GetDataFromSource()
        {
            var repository = new LookupValueDb(_screenName);
            return repository.GetList();
        }

        protected override string GetTextForFilter(LookupValue inputModel)
        {
            return inputModel?.Name;
        }

    }

    public class DemographicsGenderService : DemographicsLookupServiceBase
    {
        public DemographicsGenderService() : base("Gender")
        {

        }
    }

    public class DemographicsRaceService : DemographicsLookupServiceBase
    {
        public DemographicsRaceService() : base("Race")
        {

        }
    }

    public class DemographicsEducationLevelService : DemographicsLookupServiceBase
    {
        public DemographicsEducationLevelService() : base("EducationLevel")
        {

        }
    }

    public class DemographicsMaritalStatusService : DemographicsLookupServiceBase
    {
        public DemographicsMaritalStatusService() : base("MaritalStatus")
        {

        }
    }

    public class DemographicsSexualOrientationService : DemographicsLookupServiceBase
    {
        public DemographicsSexualOrientationService() : base("SexualOrientation")
        {

        }
    }

    public class DemographicsMilitaryExperienceService : DemographicsLookupServiceBase
    {
        public DemographicsMilitaryExperienceService() : base("MilitaryExperience")
        {

        }
    }

    public class DemographicsLivingOnReservationService : DemographicsLookupServiceBase, ILookupDataSourceBase
    {
        public DemographicsLivingOnReservationService() : base("LivingOnReservation")
        {

        }
    }


    public class DemographicsMilitaryCombatService : DemographicsLookupServiceBase, ILookupDataSourceBase
    {
        public DemographicsMilitaryCombatService() : base("MilitaryExperience")
        {

        }

        protected override string CacheKey => "militarycombat";

        protected override List<LookupValue> GetDataFromSource()
        {
            var values = base.GetDataFromSource();

            LookupValue result = values.First(x => x.Id == 4);
            result.Name = "YES";

            return new List<LookupValue>
            {
                result,
                new LookupValue{Id = 0, Name = "NO"}
            };
        }
    }
}
