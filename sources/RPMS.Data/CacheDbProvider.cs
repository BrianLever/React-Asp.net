using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk.Common.Data;
using InterSystems.Data.CacheClient;

namespace RPMS.Common.Data
{
    public class CacheDbProvider : StandardDbProvider
    {
        public override System.Data.IDbConnection CreateConnection()
        {
            return new CacheConnection();
        }

        public override System.Data.IDbCommand CreateCommand()
        {
            return new CacheCommand();
        }

        public override System.Data.IDataAdapter CreateDataAdapter(System.Data.IDbCommand selectCommand)
        {
            return new CacheDataAdapter((CacheCommand)selectCommand);
        }

        public override System.Data.IDataParameter CreateParameter()
        {
            return new CacheParameter();
        }
        public override System.Data.IDataParameter CreateParameter(int size)
        {
            var param = (CacheParameter)CreateParameter();
            param.Size = size;
            return param;
        }
    }
}
