using System;
using System.Collections.Generic;
using FrontDesk.Common;

namespace FrontDesk.Server.Data.BhsVisits
{
    public interface ILookupValueSource
    {
        List<LookupValue> GetList();
        List<LookupValue> GetModifiedValues(DateTime lastModifiedDateUTC);
    }
}