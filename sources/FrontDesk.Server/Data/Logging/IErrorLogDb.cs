using FrontDesk.Server.Logging;

using System;
using System.Collections.Generic;
using System.Data;

namespace FrontDesk.Server.Data.Logging
{
    public interface IErrorLogRepository
    {
        long Add(ErrorLog logItem);
        int Delete(IEnumerable<long> errorLogIds);
        int Delete(long errorLogID);
        List<ErrorLog> Get(DateTimeOffset startDate, DateTimeOffset endDate, int startRowIndex, int maximumRows);
        ErrorLog Get(long errorLogID);
        DataSet GetAsDataSet(DateTimeOffset startDate, DateTimeOffset endDate, int startRowIndex, int maximumRows);
        int GetCount(DateTimeOffset startDate, DateTimeOffset endDate);
        void DeleteAll();
    }
}