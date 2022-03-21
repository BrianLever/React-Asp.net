using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace RPMS.Data.BMXNet
{
    public static class BmxReaderExtensions
    {
        public static bool IsValidRecordInResult(this IDataReader bmxReader)
        {
            if (bmxReader.FieldCount == 1 && bmxReader.GetName(0) == "M_ERROR")
            {
                return false;
            }
            return true;
        }
    }
}
