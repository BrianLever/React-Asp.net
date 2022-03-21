using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPMS.Common.Builders
{
    public class DataOfBirthBuilder : IEntityBuilder<DateTime>
    {
        #region IEntityBuilder<DateTime> Members

        public DateTime CreateFromDbReader(System.Data.IDataReader reader)
        {
           return Convert.ToDateTime(reader[PatientRepositoryDescriptor.DateOfBirthColumn]);
        }

        public DateTime CreateFromDbReader(System.Data.IDataReader reader, IDictionary<string, string> customFieldMapping)
        {
            return Convert.ToDateTime(reader[customFieldMapping[PatientRepositoryDescriptor.DateOfBirthColumn]]);
        }

        #endregion
    }
}
