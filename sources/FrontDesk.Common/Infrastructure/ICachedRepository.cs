using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Common.Infrastructure
{
    public interface ICachedRepository<TKey, TValue>
    {
        TValue Get(TKey key, TimeSpan timeout);
    }
}
