using FluentAssertions;

using FrontDesk.Common.InfrastructureServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using ScreenDoxKioskLauncher.Infrastructure;
using ScreenDoxKioskLauncher.Models;
using ScreenDoxKioskLauncher.Services;

using System;
using System.IO;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ScreenDoxKioskLauncher.Tests.Services
{

    [DeploymentItem(@"Data\upgrade-job.yaml", "Data")]
    [DeploymentItem(@"Data\upgrade-job_version_9.yaml", "Data")]
    [DeploymentItem(@"Data\configuration.yaml", "Data")]
    [TestClass]
    public class UpgradeScheduleServiceTests
    {
        private Mock<EnvironmentProvider> _environmentProvider = new Mock<EnvironmentProvider>(() => new EnvironmentProvider());
        private Mock<ITimeService> _timeServiceMock = new Mock<ITimeService>();

        public UpgradeScheduleServiceTests()
        {
            var configFilePath = @"Data\configuration.yaml";
            _environmentProvider.SetupGet(x => x.AppConfigurationFileFullPath).Returns(configFilePath);
            _environmentProvider.SetupGet(x => x.InstallationTimeIntervalInMinutes).Returns(5); // 5 minutes from scheduled time
        }

        protected UpgradeScheduleService Sut(string alternativeYamlFilePath = null)
        {
            var yamlFilePath = string.IsNullOrWhiteSpace(alternativeYamlFilePath) ? @"Data\upgrade-job.yaml" : alternativeYamlFilePath;
            _environmentProvider.SetupGet(x => x.JobScheduleFileFullPath).Returns(yamlFilePath);

            return new UpgradeScheduleService(_environmentProvider.Object, _timeServiceMock.Object);
        }


        [TestMethod]
        public void CheckVersionIsReadyToInstall_WhenMatch_Returns_True()
        {
            var version = new Version("9.1.2.3");

            var result = Sut(@"Data\upgrade-job_version_9.yaml").CheckVersionIsReadyToInstall(version);

            result.Should().BeTrue();
        }

        [TestMethod]
        public void CheckVersionIsReadyToInstall_WhenNotMatch_Returns_False()
        {
            var version = new Version("8.0.0.3");

            var result = Sut(@"Data\upgrade-job_version_9.yaml").CheckVersionIsReadyToInstall(version);

            result.Should().BeFalse();
        }

        [TestMethod]
        public void CheckVersionIsReadyToInstall_WhenEmpty_Returns_False()
        {
            var version = new Version("8.0.0.3");

            var result = Sut().CheckVersionIsReadyToInstall(version);

            result.Should().BeFalse();
        }


        [TestMethod]
        public void RegisterInstallationJob_CanWrite_NoException()
        {
            InstallationPackageInfo model = new InstallationPackageInfo
            {
                Version = new Version("9.0.0.1"),
                InstallOn = new DateTime(2020, 6, 1, 2, 0, 0)
            };

            Action action = () => Sut().RegisterInstallationJob(model);

            action.Should().NotThrow();
        }

        [TestMethod]
        public void RegisterInstallationJob_CanWrite_And_Read()
        {
            InstallationPackageInfo expected = new InstallationPackageInfo
            {
                Version = new Version("9.0.0.1"),
                InstallOn = new DateTime(2020, 6, 1, 2, 0, 0)
            };

            var sut = Sut();

            sut.RegisterInstallationJob(expected);


            var serializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            InstallationPackageInfo actual = null;

            using (var sr = File.OpenText(_environmentProvider.Object.JobScheduleFileFullPath))
            {
                actual = serializer.Deserialize<InstallationPackageInfo>(sr);
            }

            actual.Should().NotBeNull();
            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void GetRegisteredVersionToInstall_WhenScheduleEarlier_Return_Result()
        {
            // assign
            var currentTime = DateTime.Now;
            var installOn = currentTime.AddMinutes(-30);

            var versionToInstall = new InstallationPackageInfo
            {
                Version = new Version("9.0.0.1"),
                InstallOn = installOn
            };

            _timeServiceMock.Setup(x => x.GetLocalNow()).Returns(currentTime);

            var sut = Sut();
            sut.RegisterInstallationJob(versionToInstall);


            // act
            var result = sut.GetRegisteredVersionToInstall();


            // assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void GetRegisteredVersionToInstall_WhenExactMatch_Return_Value()
        {
            // assign
            var currentTime = DateTime.Now;
            var installOn = currentTime.AddMinutes(-5);

            var versionToInstall = new InstallationPackageInfo
            {
                Version = new Version("9.0.0.1"),
                InstallOn = installOn
            };

            _timeServiceMock.Setup(x => x.GetLocalNow()).Returns(currentTime);

            var sut = Sut();
            sut.RegisterInstallationJob(versionToInstall);


            // act
            var result = sut.GetRegisteredVersionToInstall();


            // assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void GetRegisteredVersionToInstall_WhenInInterval_Return_Value()
        {
            // assign
            var currentTime = DateTime.Now;
            var installOn = currentTime;

            var versionToInstall = new InstallationPackageInfo
            {
                Version = new Version("9.0.0.1"),
                InstallOn = installOn
            };

            _timeServiceMock.Setup(x => x.GetLocalNow()).Returns(currentTime);

            var sut = Sut();
            sut.RegisterInstallationJob(versionToInstall);


            // act
            var result = sut.GetRegisteredVersionToInstall();

            // assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void GetRegisteredVersionToInstall_WhenMatchUpperInterval_Return_Value()
        {
            // assign
            var currentTime = DateTime.Now;
            var installOn = currentTime.AddMinutes(5);

            var versionToInstall = new InstallationPackageInfo
            {
                Version = new Version("9.0.0.1"),
                InstallOn = installOn
            };

            _timeServiceMock.Setup(x => x.GetLocalNow()).Returns(currentTime);

            var sut = Sut();
            sut.RegisterInstallationJob(versionToInstall);


            // act
            var result = sut.GetRegisteredVersionToInstall();

            // assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void GetRegisteredVersionToInstall_WhenLater_Return_Null()
        {
            // assign
            var currentTime = DateTime.Now;
            var installOn = currentTime.AddMinutes(10);

            var versionToInstall = new InstallationPackageInfo
            {
                Version = new Version("9.0.0.1"),
                InstallOn = installOn
            };

            _timeServiceMock.Setup(x => x.GetLocalNow()).Returns(currentTime);

            var sut = Sut();
            sut.RegisterInstallationJob(versionToInstall);


            // act
            var result = sut.GetRegisteredVersionToInstall();

            // assert
            result.Should().BeNull();
        }
    }
}
