using System;

namespace FrontDesk.Common.InfrastructureServices
{
    public interface ITimeService
    {
        DateTimeOffset GetDateTimeOffsetNow();
        DateTime GetUtcNow();
        DateTime GetLocalNow();
    }
}