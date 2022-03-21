using System;
using System.Collections.Generic;

namespace FrontDesk.Server.Data.BhsVisits
{
    public interface ITypeaheadDataSource
    {
        List<string> GetList(string query = null);
        List<string> GetModifiedValues(DateTime lastModifiedDateUTC);
    }
}