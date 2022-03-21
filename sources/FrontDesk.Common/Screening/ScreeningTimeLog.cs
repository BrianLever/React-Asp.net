using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using Common.Logging;

using FrontDesk.Common.InfrastructureServices;

namespace FrontDesk.Common.Screening
{

    public class ScreeningTimeLog
    {
        private readonly ITimeService _timeService;
        private ILog _logger = LogManager.GetLogger<ScreeningTimeLog>();

        public ConcurrentDictionary<string, TimeInterval> ScreeningTimeIntervals { get; protected set; } = new ConcurrentDictionary<string, TimeInterval>();

        public const string ENTIRE_SCREENING_SECTION_ID = "ALL";

        public ScreeningTimeLog(ITimeService timeService)
        {
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
        }

        public ScreeningTimeLog() : this(new TimeService())
        {

        }

        public void StartSectionTimeRecording(string screeningSectionID)
        {
            TimeInterval interval = new TimeInterval
            {
                Started = _timeService.GetDateTimeOffsetNow()
            };

            if (!ScreeningTimeIntervals.TryAdd(screeningSectionID, interval))
            {

                if (ScreeningTimeIntervals.TryGetValue(screeningSectionID, out interval))
                {
                    interval.Started = _timeService.GetDateTimeOffsetNow();
                }
                else
                {
                    _logger.WarnFormat("[ScreeningTimeLog] Failed to start time tracking for section {0}", screeningSectionID);
                }

            }
        }

        public void StopSectionTimeRecording(string screeningSectionID)
        {
            TimeInterval interval;


            if (ScreeningTimeIntervals.TryGetValue(screeningSectionID, out interval))
            {
                interval.Ended = _timeService.GetDateTimeOffsetNow();
            }
            else
            {
                _logger.WarnFormat("[ScreeningTimeLog] Section not found in the time intervals. Section id: [{0}]. Cannot complete section time tracking.", screeningSectionID);
            }
        }


        public void StartPatientScreeningRecording()
        {
            StartSectionTimeRecording(ENTIRE_SCREENING_SECTION_ID);
        }

        public void StopPatientScreeningRecording()
        {
            StopSectionTimeRecording(ENTIRE_SCREENING_SECTION_ID);
        }

        public void Reset()
        {
            ScreeningTimeIntervals.Clear();
        }


        public List<ScreeningTimeLogRecord> GetLogRecords()
        {
            return ScreeningTimeIntervals.Where(kp => kp.Value.IsComplete).Select(kp => new ScreeningTimeLogRecord
            {
                ScreeningSectionID = kp.Key,
                Started = kp.Value.Started.Value,
                Ended = kp.Value.Ended.Value
            }).ToList();
        }

    }

    public class TimeInterval
    {
        public DateTimeOffset? Started { get; set; }
        public DateTimeOffset? Ended { get; set; }

        public bool IsComplete
        {
            get
            {
                return Started.HasValue && Ended.HasValue;
            }
        }
    }


    [DataContract(Name = "ScreeningTimeLogRecord", Namespace = "http://www.frontdeskhealth.com")]
    public class ScreeningTimeLogRecord
    {
        [IgnoreDataMember]
        public long ID { get; set; }


        [DataMember]
        public string ScreeningSectionID { get; set; }


        [DataMember]
        public DateTimeOffset Started { get; set; }

        [DataMember]
        public DateTimeOffset Ended { get; set; }

        [IgnoreDataMember]
        public TimeSpan ScreeningDuration
        {
            get
            {
                return (Ended - Started);
            }
        }

    }
}
