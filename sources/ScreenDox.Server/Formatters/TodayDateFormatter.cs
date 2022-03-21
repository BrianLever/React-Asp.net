using FrontDesk.Common.Extensions;
using FrontDesk.Common.InfrastructureServices;

using System;

namespace ScreenDox.Server.Formatters
{
    /// <summary>
    /// Formatter for date columns in grids
    /// </summary>
    public class TodayDateFormatter : IPropertyFormatter<DateTimeOffset>
    {
        private readonly ITimeService _timeService;

        public TodayDateFormatter(ITimeService timeService)
        {
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
        }

        /// <summary>
        /// Format date. Show only time when date is equal to 'Today' date on the server.
        /// </summary>
        /// <param name="date">Date to format</param>
        /// <param name="useTodayLabel">When "true", return "Today" label instead of date.</param>
        /// <returns>Formatted date as string</returns>
        public string Format(DateTimeOffset? date, bool useTodayLabel = false)
        {
            var today = _timeService.GetDateTimeOffsetNow().Date;

            if (!date.HasValue) return string.Empty;

            if (date?.Date == today)
            {
                if(useTodayLabel)
                {
                    return "Today";
                }

                return string.Format("{0:HH:mm}", date);
            }
            else
            {
                return string.Format("{0:MM/dd/yyyy, HH:mm}", date);
            }
        }

        /// <summary>
        /// Format string as date w/o time
        /// </summary>
        /// <param name="date"></param>
        /// <param name="useTodayLabel">When "true", return "Today" label instead of date.</param>
        /// <returns></returns>
        public string FormatAsDate(DateTimeOffset? date, bool useTodayLabel = false)
        {
            var today = _timeService.GetDateTimeOffsetNow().Date;

            if (!date.HasValue) return string.Empty;

            if (date?.Date == today)
            {
                if (useTodayLabel)
                {
                    return "Today";
                }
            }

            return date.FormatAsDate();
        }
    }
}
