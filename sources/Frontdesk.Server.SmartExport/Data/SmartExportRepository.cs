using FrontDesk.Common.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Frontdesk.Server.SmartExport.Data
{
    public interface ISmartExportRepository: ITransactionalDatabase
    {
        List<long> GetScreeningResultsForExport();
    }

    public class SmartExportDb : DBDatabase, ISmartExportRepository
    {
        public SmartExportDb() : base(0) { }

        internal SmartExportDb(DbConnection sharedConnection) : base(sharedConnection) { }


        public List<long> GetScreeningResultsForExport()
        {
            const string sql = "export.uspGetScreeningResultsForExport";
            List<long> result = new List<long>();

            ClearParameters();
            try
            {
                using (var reader = RunProcedureSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetInt64(0));
                    }
                }
            }
            finally
            {
                Disconnect();
            }

            return result;
        }
    }
}
