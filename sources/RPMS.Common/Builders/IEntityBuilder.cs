using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace RPMS.Common.Builders
{
    public interface IEntityBuilder<T>
        where T: new()
    {
        T CreateFromDbReader(IDataReader reader);

        T CreateFromDbReader(IDataReader reader, IDictionary<string, string> customFieldMapping);
    }
}
