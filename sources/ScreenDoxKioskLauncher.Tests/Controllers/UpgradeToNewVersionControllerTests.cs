using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using ScreenDoxKioskLauncher.Controllers;
using ScreenDoxKioskLauncher.Infrastructure;
using ScreenDoxKioskLauncher.Models;
using ScreenDoxKioskLauncher.Services;

using System;

namespace ScreenDoxKioskLauncher.Tests.Controllers
{

    [TestClass]
    public class UpgradeToNewVersionControllerTests
    {
        private Mock<IEnvironmentProvider> environmentProviderMock = new Mock<IEnvironmentProvider>();
        private Mock<IUpgradeScheduleService> upgradeScheduleServiceMock = new Mock<IUpgradeScheduleService>();
        private readonly Mock<IKioskAppManagerService> kioskAppManagerServiceMock = new Mock<IKioskAppManagerService>();
        private readonly Mock<IFileManagementService> fileManagementServiceMock = new Mock<IFileManagementService>();
        private readonly Mock<IKioskHealthService> healthServiceMock = new Mock<IKioskHealthService>();
        private readonly Mock<IApplicationStateService> applicationStateServiceMock = new Mock<IApplicationStateService>();

        public UpgradeToNewVersionControllerTests()
        {
            environmentProviderMock.SetupGet(x => x.CheckUpgradeTimeInterval).Returns(TimeSpan.FromMinutes(10));
            environmentProviderMock.SetupGet(x => x.PackagesRootDirectoryPath).Returns(@"packages");
            environmentProviderMock.SetupGet(x => x.ScreenDoxInstallationServiceBaseUrl).Returns("https://server.com/screendox");

            var version = new InstallationPackageInfo
            {
                Version = new Version("9.0.0.0"),
                InstallOn = DateTime.Now
            };
            upgradeScheduleServiceMock.Setup(x => x.GetRegisteredVersionToInstall()).Returns(version);
            healthServiceMock.Setup(x => x.GetKioskLastActivity()).Returns(() =>
            {
                return new KioskLastActivity
                {
                    LastActivityUtc = DateTime.UtcNow.AddMilliseconds(-50),
                    TimeSinceLastActivity = TimeSpan.FromMilliseconds(50)
                };
            });
        }

        protected UpgradeToNewVersionController Sut()
        {
            return new UpgradeToNewVersionController(
                upgradeScheduleServiceMock.Object,
                environmentProviderMock.Object,
                kioskAppManagerServiceMock.Object,
                fileManagementServiceMock.Object,
                healthServiceMock.Object,
                applicationStateServiceMock.Object
                );
        }

        [TestMethod]
        public void UpgradeToNewVersionController_Reschedules_Installation_Job()
        {
            // assign
            var currentTime = DateTime.Now;

            var version = new InstallationPackageInfo
            {
                Version = new Version("9.0.0.0"),
                InstallOn = currentTime
            };

            InstallationPackageInfo rescheduledVersion = null;

            upgradeScheduleServiceMock.Setup(x => x.GetRegisteredVersionToInstall())
                .Returns(version);

            upgradeScheduleServiceMock
               .Setup(x => x.RegisterInstallationJob(It.IsAny<InstallationPackageInfo>()))
               .Callback<InstallationPackageInfo>((x) => rescheduledVersion = x);

            var sut = Sut();

            //act
            sut.OnTimerTickAction();

            // assert
            upgradeScheduleServiceMock
                .Verify(x => x.RegisterInstallationJob(It.IsAny<InstallationPackageInfo>()), Times.Once);

            rescheduledVersion.Version.Should().Be(version.Version);
            rescheduledVersion.InstallOn.Should().BeAfter(version.InstallOn);
        }


        [TestMethod]
        public void UpgradeToNewVersionController_Creates_AppConfig()
        {
            // assign
            var sut = Sut();

            //act
            sut.OnTimerTickAction();

            // assert
            fileManagementServiceMock.Verify(x => x.ApplyTransformationToAppConfigurationFile("9.0.0.0"), Times.Once);
        }
    }
}
