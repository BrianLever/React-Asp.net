using System;
using System.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ScreenDoxKioskLauncher.Infrastructure;
using ScreenDoxKioskLauncher.Services;

namespace ScreenDoxKioskLauncher.Tests.Services
{
    [DeploymentItem(@"Content\App_setup.config", @"packages\9.0.0.0")]
    [TestClass]
    public class FileManagementServiceTests
    {
        private Mock<IEnvironmentProvider> environmentProviderMock = new Mock<IEnvironmentProvider>();

        public FileManagementServiceTests()
        {
            environmentProviderMock.SetupGet(x => x.PackagesRootDirectoryPath).Returns(@"packages");
            environmentProviderMock.SetupGet(x => x.KioskExeName).Returns(@"FrontDeskKiosk.exe");
            environmentProviderMock.SetupGet(x => x.KioskKey).Returns("ABCD");
            environmentProviderMock.SetupGet(x => x.KioskSecret).Returns("Secret-string-XXX");
            environmentProviderMock.SetupGet(x => x.ScreenDoxInstallationServiceBaseUrl).Returns("https://server.com/screendoxnstallationapi");
            environmentProviderMock.SetupGet(x => x.ScreendoxServerBaseUrl).Returns("https://server.com/screendox");
        }

        private FileManagementService Sut()
        {
            return new FileManagementService(environmentProviderMock.Object);
        }

        [TestMethod]
        public void UpgradeToNewVersionController_Creates_AppConfig()
        {
            // assign
            var sut = Sut();

            //act
            sut.ApplyTransformationToAppConfigurationFile("9.0.0.0");


            // assert
            File.Exists(@"packages\9.0.0.0\FrontDeskKiosk.exe.config").Should().BeTrue();
        }

        [TestMethod]
        public void UpgradeToNewVersionController_Rewrites_Params_In_AppConfig()
        {
            // assign
            var sut = Sut();
            var appConfig = @"packages\9.0.0.0\FrontDeskKiosk.exe.config";

            //act
            sut.ApplyTransformationToAppConfigurationFile("9.0.0.0");

            // assert
            var content = File.ReadAllText(appConfig);

            content.Contains("KIOSK_KEY").Should().BeFalse();
            content.Contains("KIOSK_SECRET").Should().BeFalse();
            content.Contains("{SERVICE_ADDRESS}").Should().BeFalse();
            content.Contains("{VERSION}").Should().BeFalse();

            content.Contains(@"<add key=""KioskKey"" value=""ABCD"" />").Should().BeTrue();
            content.Contains(@"endpoint address=""https://server.com/screendox/endpoint/KioskEndpoint.svc"" ").Should().BeTrue();
        }

        [TestMethod]
        public void UpgradeToNewVersionController_Rewrites_Params_In_AppConfig_Several_Times()
        {
            // assign
            var sut = Sut();

            //act
            //first try
            sut.ApplyTransformationToAppConfigurationFile("9.0.0.0");
            //second try
            Action action = () => sut.ApplyTransformationToAppConfigurationFile("9.0.0.0");

            // assert
            action.Should().NotThrow("unlock the file");
        }

        [DeploymentItem(@"Content\App_setup.config", @"app")]
        [DeploymentItem(@"Content\ScreenDoxKiosk-package-9.0.0.0.zip", @"app\data")]
        [DeploymentItem(@"Data\upgrade-job.yaml", @"app")]
        [DeploymentItem(@"Data\upgrade-job_version_9.yaml", @"app\data")]


        [TestMethod]
        public void BackupKioskApplicationDirectory_Can_Copy_Files_FirstTime()
        {
            // assign
            var sut = Sut();
            environmentProviderMock.SetupGet(x => x.KioskApplicationDirectoryPath).Returns("app");
            environmentProviderMock.SetupGet(x => x.ApplicationBackupDirectoryPath).Returns("recovery-backup");


            //act
            sut.BackupKioskApplicationDirectory();

            // assert
            Directory.Exists("recovery-backup").Should().BeTrue();
            Directory.Exists(@"recovery-backup\data").Should().BeTrue("Should copy subfolder data");

            Directory.GetFiles("recovery-backup").Length.Should().Be(2);
            Directory.GetFiles(@"recovery-backup\data").Length.Should().Be(2, "copy files into subfolder");
        }

        [DeploymentItem(@"Content\App_setup.config", @"app-3")]
        [DeploymentItem(@"Content\App_setup.config", @"recovery-backup-3")]

        [TestMethod]
        public void BackupKioskApplicationDirectory_Can_Copy_Files_When_NotEmpty()
        {
            // assign
            var sut = Sut();
            environmentProviderMock.SetupGet(x => x.KioskApplicationDirectoryPath).Returns("app-3");
            environmentProviderMock.SetupGet(x => x.ApplicationBackupDirectoryPath).Returns("recovery-backup-3");


            //act
            sut.BackupKioskApplicationDirectory();

            // assert
            Directory.Exists("recovery-backup-3").Should().BeTrue();
            Directory.GetFiles("recovery-backup-3").Length.Should().Be(1);
        }

        [DeploymentItem(@"Content\App_setup.config", @"recovery-backup-test")]
        [DeploymentItem(@"Content\App_setup.config", @"app-recovery-test\App.config")]
        [DeploymentItem(@"Data\upgrade-job.yaml", @"recovery-backup-test")]
        [DeploymentItem(@"Data\upgrade-job_version_9.yaml", @"recovery-backup-test\data")]
        [TestMethod]
        public void RestoreKioskApplicationFolderFromBackup_Can_Copy_Files()
        {
            // assign
            var sut = Sut();
            environmentProviderMock.SetupGet(x => x.KioskApplicationDirectoryPath).Returns("app-recovery-test");
            environmentProviderMock.SetupGet(x => x.ApplicationBackupDirectoryPath).Returns("recovery-backup-test");


            //act
            sut.RestoreKioskApplicationFolderFromBackup();

            // assert
            Directory.Exists("app-recovery-test").Should().BeTrue();
            Directory.Exists(@"app-recovery-test\data").Should().BeTrue("Should copy subfolder data");

            Directory.GetFiles("app-recovery-test", "App.config").Length.Should().Be(0, "File App.config should be removed");
            Directory.GetFiles("app-recovery-test").Length.Should().Be(2);
            Directory.GetFiles(@"app-recovery-test\data").Length.Should().Be(1, "copy files into subfolder");
        }

        [DeploymentItem(@"Content\App_setup.config", @"app-2")]
        [DeploymentItem(@"Content\ScreenDoxKiosk-package-9.0.0.0.zip", @"app-2\data")]
        [DeploymentItem(@"Content\App_setup.config", @"recovery-backup-2")]
        [DeploymentItem(@"Content\ScreenDoxKiosk-package-9.0.0.0.zip", @"recovery-backup-2\data")]
        [DeploymentItem(@"Data\upgrade-job.yaml", @"recovery-backup-2")]
        [DeploymentItem(@"Data\version-8.yaml", @"recovery-backup-2\sound")]
        [DeploymentItem(@"Data\upgrade-job_version_9.yaml", @"recovery-backup-2\data")]
        [DeploymentItem(@"Content\App_setup.config", @"app-2\AnotherFolder")]
        [TestMethod]
        public void RestoreKioskApplicationFolderFromBackup_Can_Replace_Files()
        {
            // assign
            var sut = Sut();
            environmentProviderMock.SetupGet(x => x.KioskApplicationDirectoryPath).Returns("app-2");
            environmentProviderMock.SetupGet(x => x.ApplicationBackupDirectoryPath).Returns("recovery-backup-2");

            //act
            sut.RestoreKioskApplicationFolderFromBackup();

            // assert
            Directory.Exists(@"app-2").Should().BeTrue();
            Directory.Exists(@"app-2\sound").Should().BeTrue("Should copy subfolder data");

            Directory.GetFiles("app-2").Length.Should().Be(2);
            Directory.GetFiles(@"app-2\data").Length.Should().Be(2, "copy files into subfolder and delete previous files");

            Directory.GetFiles(@"app-2\sound").Length.Should().Be(1);

            Directory.Exists(@"app-2\AnotherFolder").Should().BeFalse("Remove new folders not in backup");
        }
    }
}
