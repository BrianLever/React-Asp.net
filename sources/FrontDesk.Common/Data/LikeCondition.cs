using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Common.Data
{
    /// <summary>
    /// SQL LIKE statement condition type
    /// </summary>
    [Flags]
    public enum LikeCondition
    {
        None = 0,
        StartsWith = 1,
        Contains = 2
    }
}
