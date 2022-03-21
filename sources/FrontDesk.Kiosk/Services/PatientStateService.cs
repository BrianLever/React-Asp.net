using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Caching;

namespace FrontDesk.Kiosk.Services
{
    /// <summary>
    /// Manage cached list of states. Singletone.
    /// Note: this class must only be used on Kiosk App
    /// </summary>
    public class PatientStateService : TypeaheadDataSourceBase<State>
    {
      
        protected override string CacheKey => "states";

        protected override List<State> GetDataFromSource()
        {
            FrontDesk.State.UseLocalDatabaseConnection = true;
            return State.GetList();
        }

        protected override string GetTextForFilter(State inputModel)
        {
            return inputModel?.Name;
        }

    }
}
