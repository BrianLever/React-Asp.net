using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FrontDesk.Common.Extensions;
using FrontDesk.Kiosk.Repositories;

namespace FrontDesk.Kiosk.Services
{
    public class DemographicsCountyStateService : TypeaheadDataSourceBase<string>
    {
        private string _selectedCountyName = string.Empty;

        public string SelectedCountyName { get => _selectedCountyName; set => _selectedCountyName = value?.Trim(); }


        protected override string CacheKey => "countystates_" + SelectedCountyName ?? String.Empty;

        protected override List<string> GetDataFromSource()
        {
            var repository = new TypeaheadDb("County");
            return repository
                .GetList()
                .Where(x => x.StartsWith(SelectedCountyName))
                .Select(x =>
            {
                return CountyTextFunctions.ParseCounty(x).State;
            }).ToList();

        }

        public override List<string> GetByPartOfName(string startsWithInput)
        {
            return base.GetByPartOfName(startsWithInput);
        }

        protected override string GetTextForFilter(string inputModel)
        {
            return inputModel;
        }

        public override void Refresh()
        {
            base.Refresh();
        }

    }
}
