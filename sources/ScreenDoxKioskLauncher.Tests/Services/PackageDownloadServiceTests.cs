using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using ScreenDoxKioskInstallApi.Models;

using ScreenDoxKioskLauncher.Infrastructure;
using ScreenDoxKioskLauncher.Models;
using ScreenDoxKioskLauncher.Services;

using System;
using System.IO;

namespace ScreenDoxKioskLauncher.Tests.Services
{
    [DeploymentItem(@"Data\version-8.yaml", "Data")]
    [DeploymentItem(@"Data\upgrade-job.yaml", "Data")]
    [DeploymentItem(@"Data\configuration.yaml", "Data")]
    [DeploymentItem(@"Content\ScreenDoxKiosk-package-9.0.0.0.zip", "Content")]
    [TestClass]
    public class PackageDownloadServiceTests
    {
        private IEnvironmentProvider _environmentProvider = new EnvironmentProvider();
        private Mock<IKioskInstallApiClient> _clientMock = new Mock<IKioskInstallApiClient>();

        public PackageDownloadServiceTests()
        {
            File.Copy(@"Data\version-8.yaml",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                @"ScreenDox\version.yaml"), true);

            File.Copy(@"Data\upgrade-job.yaml",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), 
                @"ScreenDox\upgrade-job.yaml"), true);

        }

        protected PackageDownloadService Sut()
        {
            return new PackageDownloadService(_environmentProvider, _clientMock.Object);
        }


        [TestMethod]
        public void CanReadCurrentVersion()
        {
            var sut = Sut();

            sut.GetCurrentVersion().ToString().Should().Be("8.0.0.0");
        }

        [TestMethod]
        public void CheckNewVersionAvailable_WhenNewVersion_Returns_Result()
        {
            var sut = Sut();

            _clientMock.Setup(x => x.GetAvailableVersionInfo()).Returns(new InstallationPackageInfo
            {
                InstallOn = DateTime.Now.AddDays(1),
                Version = new Version("9.0.0.0")
            });

            var result = sut.CheckNewVersionAvailable();

            result.Should().NotBeNull();
        }

        [TestMethod]
        public void CheckNewVersionAvailable_WhenSameVersion_Returns_Null()
        {
            var sut = Sut();

            _clientMock.Setup(x => x.GetAvailableVersionInfo()).Returns(new InstallationPackageInfo
            {
                InstallOn = DateTime.Now.AddDays(1),
                Version = new Version("8.0.0.0")
            });

            var result = sut.CheckNewVersionAvailable();

            result.Should().BeNull();
        }

        [TestMethod]
        public void CheckNewVersionAvailable_WhenPreviousVersion_Returns_Null()
        {
            var sut = Sut();

            _clientMock.Setup(x => x.GetAvailableVersionInfo()).Returns(new InstallationPackageInfo
            {
                InstallOn = DateTime.Now.AddDays(1),
                Version = new Version("7.9.0.0")
            });

            var result = sut.CheckNewVersionAvailable();

            result.Should().BeNull();
        }

        [TestMethod]
        public void CheckNewVersionAvailable_WhenMinorVersion_Returns_Result()
        {
            var sut = Sut();

            _clientMock.Setup(x => x.GetAvailableVersionInfo()).Returns(new InstallationPackageInfo
            {
                InstallOn = DateTime.Now.AddDays(1),
                Version = new Version("8.0.0.1")
            });

            var result = sut.CheckNewVersionAvailable();

            result.Should().NotBeNull();
        }


        [TestMethod]
        public void CheckNewVersionAvailable_WhenNewVersion_Returns_CorrectVersion()
        {
            var info = new InstallationPackageInfo
            {
                InstallOn = DateTime.Now.AddDays(1),
                Version = new Version("9.0.0.0")
            };

            _clientMock.Setup(x => x.GetAvailableVersionInfo()).Returns(info);

            var result = Sut().CheckNewVersionAvailable();

            result.Should().BeEquivalentTo(info);
        }


        
        [TestMethod]
        public void DownloadPackage_When_Correct()
        {
            var version = new Version("9.0.0.0");

            _clientMock.Setup(x => x.DownloadPackage(version)).Returns(new FileContent
            {
                FileName = "ScreenDoxKiosk-package-9.0.0.0.zip",
                Content = System.IO.File.ReadAllBytes(@"Content\ScreenDoxKiosk-package-9.0.0.0.zip")
            });

            Action action = () => Sut().DownloadPackage(version);

            action.Should().NotThrow<DownloadPackageException>();
        }
    }
}
