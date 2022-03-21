using System;
using FluentAssertions;
using FrontDesk.Common.InfrastructureServices;
using FrontDesk.Common.Screening;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FrontDesk.Common.Tests.Screening
{
    [TestClass]
    public class ScreeningTimeLogTests
    {
        private Mock<ITimeService> _timeServiceMock = new Mock<ITimeService>();

        public ScreeningTimeLogTests()
        {
            _timeServiceMock.Setup(x => x.GetDateTimeOffsetNow()).Returns(DateTimeOffset.Now);
        }


        protected ScreeningTimeLog Sut()
        {
            return new ScreeningTimeLog(_timeServiceMock.Object);
        }

        [TestCategory("ScreeningTimeLog")]
        [TestMethod]
        public void Can_start_screening_recording()
        {
            var sut = Sut();
            var now = new DateTimeOffset(2019, 03, 10, 20, 0, 1, TimeSpan.FromHours(2));
            _timeServiceMock.Setup(x => x.GetDateTimeOffsetNow()).Returns(now);

            sut.StartPatientScreeningRecording();

            sut.ScreeningTimeIntervals.Count.Should().Be(1);
            sut.ScreeningTimeIntervals.Should().ContainKey(ScreeningTimeLog.ENTIRE_SCREENING_SECTION_ID);
            sut.ScreeningTimeIntervals[ScreeningTimeLog.ENTIRE_SCREENING_SECTION_ID].Started.Should().Be(now);
        }

        [TestCategory("ScreeningTimeLog")]
        [TestMethod]
        public void Can_start_section_recording()
        {
            var sut = Sut();
            var now = new DateTimeOffset(2019, 03, 10, 20, 0, 1, TimeSpan.FromHours(2));
            _timeServiceMock.Setup(x => x.GetDateTimeOffsetNow()).Returns(now);

            sut.StartSectionTimeRecording(ScreeningSectionDescriptor.Alcohol);

            sut.ScreeningTimeIntervals.Count.Should().Be(1);
            sut.ScreeningTimeIntervals.Should().ContainKey(ScreeningSectionDescriptor.Alcohol);
            sut.ScreeningTimeIntervals[ScreeningSectionDescriptor.Alcohol].Started.Should().Be(now);

          
        }

        [TestCategory("ScreeningTimeLog")]
        [TestMethod]
        public void Should_track_sections_independently()
        {
            var sut = Sut();
            var now1_start = new DateTimeOffset(2019, 03, 10, 20, 0, 1, TimeSpan.FromHours(2));
            var now2_start = new DateTimeOffset(2019, 03, 10, 20, 0, 3, TimeSpan.FromHours(2));
            var now2_end = new DateTimeOffset(2019, 03, 10, 20, 1, 0, TimeSpan.FromHours(2));
            var now1_end = new DateTimeOffset(2019, 03, 10, 20, 1, 10, TimeSpan.FromHours(2));


            _timeServiceMock.Setup(x => x.GetDateTimeOffsetNow()).Returns(now1_start);
            sut.StartPatientScreeningRecording();

            _timeServiceMock.Setup(x => x.GetDateTimeOffsetNow()).Returns(now2_start);
            sut.StartSectionTimeRecording(ScreeningSectionDescriptor.Alcohol);

            _timeServiceMock.Setup(x => x.GetDateTimeOffsetNow()).Returns(now2_end);
            sut.StopSectionTimeRecording(ScreeningSectionDescriptor.Alcohol);

            _timeServiceMock.Setup(x => x.GetDateTimeOffsetNow()).Returns(now1_end);
            sut.StopPatientScreeningRecording();

            //assert
            sut.ScreeningTimeIntervals[ScreeningTimeLog.ENTIRE_SCREENING_SECTION_ID].Started.Should().Be(now1_start);
            sut.ScreeningTimeIntervals[ScreeningSectionDescriptor.Alcohol].Started.Should().Be(now2_start);

            sut.ScreeningTimeIntervals[ScreeningSectionDescriptor.Alcohol].Ended.Should().Be(now2_end);
            sut.ScreeningTimeIntervals[ScreeningTimeLog.ENTIRE_SCREENING_SECTION_ID].Ended.Should().Be(now1_end);



        }

        [TestCategory("ScreeningTimeLog")]
        [TestMethod]
        public void GetLogRecords_should_not_return_incomplete_screenings()
        {
            var sut = Sut();

            sut.StartPatientScreeningRecording();
            sut.StartSectionTimeRecording(ScreeningSectionDescriptor.Alcohol);
            sut.StopSectionTimeRecording(ScreeningSectionDescriptor.Alcohol);

            var model = sut.GetLogRecords();

            model.Count.Should().Be(1);
        }

        [TestCategory("ScreeningTimeLog")]
        [TestMethod]
        public void GetLogRecords_should_export_timestamps_property()
        {
            var sut = Sut();
           

            
            var now1_start = new DateTimeOffset(2019, 03, 10, 20, 0, 1, TimeSpan.FromHours(2));
            var now2_start = new DateTimeOffset(2019, 03, 10, 20, 0, 3, TimeSpan.FromHours(2));
            var now2_end = new DateTimeOffset(2019, 03, 10, 20, 1, 0, TimeSpan.FromHours(2));
            var now1_end = new DateTimeOffset(2019, 03, 10, 20, 1, 10, TimeSpan.FromHours(2));

            sut.ScreeningTimeIntervals.TryAdd(ScreeningTimeLog.ENTIRE_SCREENING_SECTION_ID, new TimeInterval
            {
                Started = now1_start,
                Ended = now1_end
            });
            sut.ScreeningTimeIntervals.TryAdd(ScreeningSectionDescriptor.Alcohol, new TimeInterval
            {
                Started = now2_start,
                Ended = now2_end
            });


            var model = sut.GetLogRecords();

            model.Count.Should().Be(2);

            model[0].ScreeningSectionID.Should().Be(ScreeningTimeLog.ENTIRE_SCREENING_SECTION_ID);
            model[0].Started.Should().Be(now1_start);
            model[0].Ended.Should().Be(now1_end);

            model[1].ScreeningSectionID.Should().Be(ScreeningSectionDescriptor.Alcohol);
            model[1].Started.Should().Be(now2_start);
            model[1].Ended.Should().Be(now2_end);



        }
    }
}
