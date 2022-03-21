using System;
using System.Collections.Generic;
using System.Linq;
using FrontDesk.Common.Extensions;
using FrontDesk.Kiosk.Repositories;

namespace FrontDesk.Kiosk.Services
{
    public class DemographicsCountyNameService : TypeaheadDataSourceBase<string>
    {
      
        protected override string CacheKey => "countynames";

        protected override List<string> GetDataFromSource()
        {
            var repository = new TypeaheadDb("County");
            return repository.GetList().Select(x =>
            {
                return CountyTextFunctions.ParseCounty(x).Name;
            }).ToList();
            
        }

        protected override string GetTextForFilter(string inputModel)
        {
            return inputModel;
        }

    }
}
