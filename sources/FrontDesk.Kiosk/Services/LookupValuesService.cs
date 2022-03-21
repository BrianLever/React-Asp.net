using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using FrontDesk.Common;
using FrontDesk.Common.Bhservice;
using FrontDesk.Common.InfrastructureServices;
using FrontDesk.Kiosk.Repositories;

namespace FrontDesk.Kiosk.Services
{
    public class LookupValuesService : ILookupValuesService
    {
        private readonly ILog _log = LogManager.GetLogger<LookupValuesService>();
        private readonly ITimeService _timeService = new TimeService();

        public void DeleteValues(Dictionary<string, LookupValue[]> values)
        {
      
            foreach (var dict in values)
            {
                if (LookupListDescriptor.IsKnown(dict.Key))
                {
                    var repository = new LookupValueDb(dict.Key);

                    repository.DeleteValues(dict.Value);

                    _log.InfoFormat("LookupValuesService.DeleteValues. Removed items from lookup table: {0}", dict.Key);
                }
                else if (TypeaheadListDescriptor.IsKnown(dict.Key))
                {

                    var repository = new TypeaheadDb(dict.Key);

                    repository.DeleteValues(dict.Value);

                    _log.InfoFormat("LookupValuesService.DeleteValues. Removed items from typeahead table: {0}", dict.Key);
                }
            }


        }

        public DateTime? GetLastestDataModifiedDateUTC()
        {
            return new LookupAgregateDb().GetLatestModifiedDate();
        }

        public void UpdateValues(Dictionary<string, LookupValue[]> values)
        {


            DateTime now = _timeService.GetUtcNow();

            foreach (var dict in values)
            {
                if (LookupListDescriptor.IsKnown(dict.Key))
                {
                    var repository = new LookupValueDb(dict.Key);

                    repository.UpdateValues(dict.Value, now);

                    _log.InfoFormat("LookupValuesService.UpdateValues. Updated lookup table: {0}", dict.Key);
                }
                else if (TypeaheadListDescriptor.IsKnown(dict.Key))
                {

                    var repository = new TypeaheadDb(dict.Key);

                    repository.UpdateValues(dict.Value, now);

                    _log.InfoFormat("LookupValuesService.UpdateValues. Updated typeahead table: {0}", dict.Key);
                }
            }

        }
    }
}
