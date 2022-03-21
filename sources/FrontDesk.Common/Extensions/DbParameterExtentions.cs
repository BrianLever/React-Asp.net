using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace FrontDesk.Common.Extensions
{
    public static class DbParameterExtentions
    {
        public static IDbDataParameter AddParameter(this IDbCommand CommandObject,string sName, DbType Type, int iSize)
        {
            var dbParameter = CommandObject.CreateParameter();
            dbParameter.DbType = Type;
            dbParameter.ParameterName = sName;
            dbParameter.Size = iSize;
            CommandObject.Parameters.Add(dbParameter);
            return dbParameter;
        }

        public static IDbDataParameter AddParameter(this IDbCommand CommandObject, string sName, DbType Type)
        {
            var dbParameter = CommandObject.CreateParameter();
            dbParameter.DbType = Type;
            dbParameter.ParameterName = sName;
            CommandObject.Parameters.Add(dbParameter);
            return dbParameter;
        }

        public static IDbDataParameter AddParameter(this IDbCommand CommandObject, string sName, DbType Type, ParameterDirection pmDirection)
        {
            var dbParameter = CommandObject.CreateParameter();
            dbParameter.DbType = Type;
            dbParameter.ParameterName = sName;
            dbParameter.Direction = pmDirection;
            CommandObject.Parameters.Add(dbParameter);
            return dbParameter;
        }

    }
}
