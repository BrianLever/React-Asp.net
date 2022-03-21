using System;
using FluentAssertions;
using FrontDesk.Common.InfrastructureServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ScreenDox.Server.Common.Services;
using ScreenDoxKioskInstallApi.Controllers;

namespace ScreenDoxKioskInstallApi.Tests.Services
{
    [TestClass]
    public class KioskHealthControllerTests
    {
        private readonly Mock<IKioskService> kioskServiceMock = new Mock<IKioskService>();
        private readonly Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        private DateTimeOffset currentTime = new DateTimeOffset(2020, 09, 25, 01, 0, 0, TimeSpan.FromHours(-7));

        public KioskHealthControllerTests()
        {
            timeServiceMock.Setup(x => x.GetDateTimeOffsetNow()).Returns(currentTime);
        }

        protected KioskHealthController Sut()
        {
            return new KioskHealthController(kioskServiceMock.Object, timeServiceMock.Object)
            {
                KioskKey = "PRUW"
            };
        }
        
        [TestMethod]
        public void GetTimeInSecondsSinceLastActivity_WhenEmptyHistory_Returns_NotNull()
        {
            kioskServiceMock.Setup(x => x.GetLastActivityDate(It.IsAny<short>()))
                .Returns((DateTimeOffset?)null);

            var result = Sut().GetTimeInSecondsSinceLastActivity();

            result.Should().NotBeNull();

            result.LastActivityUtc.Should().Be(currentTime.UtcDateTime);
            result.TimeSinceLastActivity.Should().Be(TimeSpan.FromHours(0));
        }

        [TestMethod]
        public void GetTimeInSecondsSinceLastActivity_WhenEmptyHistory_Returns_CurrentTime()
        {
            kioskServiceMock.Setup(x => x.GetLastActivityDate(It.IsAny<short>()))
                .Returns((DateTimeOffset?)null);

            var result = Sut().GetTimeInSecondsSinceLastActivity();

            result.Should().NotBeNull();

            result.LastActivityUtc.Should().Be(currentTime.UtcDateTime);
            result.TimeSinceLastActivity.Should().Be(TimeSpan.FromHours(0));
        }
    }
}
