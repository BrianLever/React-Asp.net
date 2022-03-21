using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrontDesk.Common;

namespace FrontDesk.Kiosk.Services
{
    public interface ILookupValuesService
    {
        DateTime? GetLastestDataModifiedDateUTC();
        void UpdateValues(Dictionary<string, LookupValue[]> values);
        void DeleteValues(Dictionary<string, LookupValue[]> changes);

    }
}
