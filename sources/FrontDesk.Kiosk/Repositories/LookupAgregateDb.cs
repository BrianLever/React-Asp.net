using System;
using System.Collections.Generic;
using System.Data;

using FrontDesk.Common;
using FrontDesk.Common.Data;
using FrontDesk.Common.Extensions;

namespace FrontDesk.Kiosk.Repositories
{
    public class LookupAgregateDb : DBCompactDatabase
    { 

        public LookupAgregateDb() : base()
        {
        }

        public DateTime? GetLatestModifiedDate()
        {
            DateTime? result = null ;

            ClearParameters();

            var sql = $"SELECT MAX([LastModifiedDateUTC]) FROM[LookupValue]";
            try
            {
                result = RunScalarQuery<DateTime>(sql);
               
            }
            finally
            {
                Disconnect();
            }
            return result;
        }

    }
}
