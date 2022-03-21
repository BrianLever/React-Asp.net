using System;
using System.Collections.Generic;
using System.Linq;
using FrontDesk.Kiosk.Repositories;

namespace FrontDesk.Kiosk.Services
{
    public class DemographicsTribeService : TypeaheadDataSourceBase<string>
    {
        private TextSearchWordStartsWith _matchPattern = new TextSearchWordStartsWith();
        protected override string CacheKey => "tribes";

        protected override List<string> GetDataFromSource()
        {
            var repository = new TypeaheadDb("Tribe");
            return repository.GetList().OrderBy(x => x).ToList();
        }

        protected override string GetTextForFilter(string inputModel)
        {
            return inputModel;
        }


        public override List<string> GetByPartOfName(string startsWithInput)
        {
            //find matched that contains the whole word


            if (String.IsNullOrEmpty(startsWithInput))
            {
                return new List<string>();
            }

            return GetDataFromCache().FindAll(s => _matchPattern.IsMatched(startsWithInput, s));
        }
    }
}
