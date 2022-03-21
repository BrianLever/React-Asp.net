using System;
using System.Data;
namespace FrontDesk.Common.Data
{
    public interface IDbProvider
    {
        System.Data.IDbCommand CreateCommand();
        System.Data.IDbConnection CreateConnection();
        System.Data.IDataAdapter CreateDataAdapter(IDbCommand selectCommand);
        IDataParameter CreateParameter();
        IDataParameter CreateParameter(int size);
    }
}
