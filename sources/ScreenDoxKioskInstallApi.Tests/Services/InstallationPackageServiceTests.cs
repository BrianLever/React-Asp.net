using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ScreenDoxKioskInstallApi.Infrastructure;
using ScreenDoxKioskInstallApi.Services;

namespace ScreenDoxKioskInstallApi.Tests.Services
{
    [DeploymentItem(@"Data\kiosk-installation-config.yaml", "Data")]
    [TestClass]
    public class InstallationPackageServiceTests
    {
        private readonly Mock<IAppSettingsProvider> _appSettingsMock = new Mock<IAppSettingsProvider>();

        public InstallationPackageServiceTests()
        {
            _appSettingsMock.SetupGet(x => x.KioskInstallationDirectoryRoot).Returns(@"Data");
        }

        private InstallationPackageService Sut()
        {
            return new InstallationPackageService(_appSettingsMock.Object);
        }

        [TestMethod]
        public void InstallationPackageService_Get_Can_Return_NotEmpty_KioskSettings()
        {
            var result = Sut().Get();

            result.Should().NotBeNull();
            result.Kiosks.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public void InstallationPackageService_Get_Returns_Default_InstallOn()
        {
            var result = Sut().Get();

            result.InstallOn.Should().Be(DateTime.Parse("2020-06-01T03:01:00"));
        }


        [TestMethod]
        public void InstallationPackageService_Get_KioskKey_Returns_Kiosk_InstallOn()
        {
            var result = Sut().Get("8ZAR");

            result.InstallOn.Should().Be(DateTime.Parse("2020-06-11T03:01:00"));
        }
    }
}
