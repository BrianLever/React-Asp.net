using System;

namespace FrontDesk.Common.InfrastructureServices
{
    public class TimeService : ITimeService
    {
        public DateTimeOffset GetDateTimeOffsetNow()
        {
            return DateTimeOffset.Now;
        }

        public DateTime GetLocalNow()
        {
            return DateTime.Now; ;
        }

        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
