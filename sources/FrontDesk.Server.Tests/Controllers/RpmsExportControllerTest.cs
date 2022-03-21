using System;
using Moq;
using FluentAssertions;
using FrontDesk.Server.Controllers;
using RPMS.Common.Security;
using RPMS.Common.GlobalConfiguration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontDesk.Server.Tests.Controllers
{
    public abstract class RpmsExportControllerTest
    {
        protected Mock<IDateService> mockDateService = new Mock<IDateService>();
        protected Mock<IRpmsCredentialsService> mockConfigurationService = new Mock<IRpmsCredentialsService>();
        protected Mock<IGlobalSettingsService>globalSettingsServiceMock = new Mock<IGlobalSettingsService>();


        protected RpmsCredentials CreateCredentials(DateTime expireOn)
        {
            return new RpmsCredentials {ExpireAt = expireOn};
        }

        public RpmsExportControllerTest()
        {
            globalSettingsServiceMock.SetupGet(x => x.IsRpmsMode).Returns(true);
            globalSettingsServiceMock.SetupGet(x => x.IsNextGenMode).Returns(false);

        }

        
        [TestClass]
        public class WhenGettingRemoteSettings : RpmsExportControllerTest
        {
           
            [TestMethod]
            public void GetCredentialsExpirationDate_calls_configService()
            {
                mockConfigurationService.Setup(x => x.GetCredentials()).Returns(CreateCredentials(new DateTime(2013, 6, 1)));

                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                controller.GetCredentialsExpirationDate();
                Action action = () => mockConfigurationService.Verify(x => x.GetCredentials(), Times.Exactly(1));

                action.Should().NotThrow();

            }

            [TestMethod]
            public void GetCredentialsExpirationDate_return_correctValue()
            {
                DateTime expirationDate = new DateTime(2013, 6, 1);
                mockConfigurationService.Setup(x => x.GetCredentials()).Returns(CreateCredentials(expirationDate));

                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                DateTime? value = controller.GetCredentialsExpirationDate();

                value.Should().Be(expirationDate);
            }

        }

        [TestClass]
        public class WhenCredentialsAreEmpty : RpmsExportControllerTest
        {
            public WhenCredentialsAreEmpty()
            {
                mockConfigurationService.Setup(x => x.GetCredentials()).Returns((RpmsCredentials)null);
                mockDateService.Setup(x => x.GetCurrentDate()).Returns(new DateTime(2013, 3, 1));
            }

            [TestMethod]
            public void GetDaysToCredentialsExpiration_notHaveValue()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                int? value = controller.GetDaysToCredentialsExpiration();

                value.Should().NotHaveValue();
            }

            [TestMethod]
            public void ShouldDisplayAlertMessage_returns_false()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                bool value = controller.ShouldDisplayAlertMessage();

                value.Should().BeTrue();
            }

            [TestMethod]
            public void RpmsCredentialsAreExpired_returns_false()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                bool? value = controller.RpmsCredentialsAreExpired();

                value.Should().BeTrue();
            }
            [TestMethod]
            public void GetAlertMessageText_returns_message()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                string value = controller.GetAlertMessageText();

                value.Should().NotBeNullOrEmpty();
            }

        }


        [TestClass]
        public class WhenPasswordIsNotExpiring : RpmsExportControllerTest
        {
            public WhenPasswordIsNotExpiring()
            {
                mockConfigurationService.Setup(x => x.GetCredentials()).Returns(CreateCredentials(new DateTime(2013, 6, 1)));
                mockDateService.Setup(x => x.GetCurrentDate()).Returns(new DateTime(2013, 3, 1));
            }

            [TestMethod]
            public void GetDaysToCredentialsExpiration_returns_correctvalue()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                int? value = controller.GetDaysToCredentialsExpiration();

                value.Should().Be(92);
            }

            [TestMethod]
            public void ShouldDisplayAlertMessage_returns_false()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                bool value = controller.ShouldDisplayAlertMessage();

                value.Should().BeFalse();
            }

            [TestMethod]
            public void RpmsCredentialsAreExpired_returns_false()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                bool? value = controller.RpmsCredentialsAreExpired();

                value.Should().BeFalse();
            }
            [TestMethod]
            public void GetAlertMessageText_returns_emptyString()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                string value = controller.GetAlertMessageText();

                value.Should().BeEmpty();
            }

        }

        [TestClass]
        public class WhenPasswordExpiringIn7Days : RpmsExportControllerTest
        {
            public WhenPasswordExpiringIn7Days()
            {
                mockConfigurationService.Setup(x => x.GetCredentials()).Returns(CreateCredentials(new DateTime(2013, 6, 1)));
                mockDateService.Setup(x => x.GetCurrentDate()).Returns(new DateTime(2013, 5, 25));
            }

            [TestMethod]
            public void GetDaysToCredentialsExpiration_returns_correctvalue()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                int? value = controller.GetDaysToCredentialsExpiration();

                value.Should().Be(7);
            }

            [TestMethod]
            public void ShouldDisplayAlertMessage_returns_true()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                bool value = controller.ShouldDisplayAlertMessage();

                value.Should().BeTrue();
            }

            [TestMethod]
            public void RpmsCredentialsAreExpired_returns_false()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                bool? value = controller.RpmsCredentialsAreExpired();

                value.Should().BeFalse();
            }
            [TestMethod]
            public void GetAlertMessageText_returns_correctString()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                string value = controller.GetAlertMessageText();

                value.Should().Be(FrontDesk.Server.Resources.TextMessages.RPMSCredentialsExpirationAlertMessage.FormatWith(7));
            }

        }

        [TestClass]
        public class WhenPasswordExpiringIn3Days : RpmsExportControllerTest
        {
            public WhenPasswordExpiringIn3Days()
            {
                mockConfigurationService.Setup(x => x.GetCredentials()).Returns(CreateCredentials(new DateTime(2013, 6, 1)));
                mockDateService.Setup(x => x.GetCurrentDate()).Returns(new DateTime(2013, 5, 29));
            }

            [TestMethod]
            public void GetDaysToCredentialsExpiration_returns_correctvalue()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                int? value = controller.GetDaysToCredentialsExpiration();

                value.Should().Be(3);
            }

            [TestMethod]
            public void ShouldDisplayAlertMessage_returns_true()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                bool value = controller.ShouldDisplayAlertMessage();

                value.Should().BeTrue();
            }

            [TestMethod]
            public void RpmsCredentialsAreExpired_returns_false()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                bool? value = controller.RpmsCredentialsAreExpired();

                value.Should().BeFalse();
            }
            [TestMethod]
            public void GetAlertMessageText_returns_correctString()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                string value = controller.GetAlertMessageText();

                value.Should().Be(FrontDesk.Server.Resources.TextMessages.RPMSCredentialsExpirationAlertMessage.FormatWith(3));
            }

        }

        [TestClass]
        public class WhenPasswordExpiringInZeroDays : RpmsExportControllerTest
        {
            public WhenPasswordExpiringInZeroDays()
            {
                mockConfigurationService.Setup(x => x.GetCredentials()).Returns(CreateCredentials(new DateTime(2013, 6, 1)));
                mockDateService.Setup(x => x.GetCurrentDate()).Returns(new DateTime(2013, 6, 1));
            }

            [TestMethod]
            public void GetDaysToCredentialsExpiration_returns_correctvalue()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                int? value = controller.GetDaysToCredentialsExpiration();

                value.Should().Be(0);
            }

            [TestMethod]
            public void ShouldDisplayAlertMessage_returns_true()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                bool value = controller.ShouldDisplayAlertMessage();

                value.Should().BeTrue();
            }

            [TestMethod]
            public void RpmsCredentialsAreExpired_returns_false()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                bool? value = controller.RpmsCredentialsAreExpired();

                value.Should().BeFalse();
            }
            [TestMethod]
            public void GetAlertMessageText_returns_correctString()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                string value = controller.GetAlertMessageText();

                value.Should().Be(FrontDesk.Server.Resources.TextMessages.RPMSCredentialsExpirationAlertMessage.FormatWith(0));
            }

        }
        
        [TestClass]
        public class WhenPasswordExpired : RpmsExportControllerTest
        {
            public WhenPasswordExpired()
            {
                mockConfigurationService.Setup(x => x.GetCredentials()).Returns(CreateCredentials(new DateTime(2013, 6, 1)));
                mockDateService.Setup(x => x.GetCurrentDate()).Returns(new DateTime(2013, 6, 2));
            }

            [TestMethod]
            public void GetDaysToCredentialsExpiration_returns_correctvalue()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                int? value = controller.GetDaysToCredentialsExpiration();

                value.Should().Be(-1);
            }

            [TestMethod]
            public void ShouldDisplayAlertMessage_returns_true()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                bool value = controller.ShouldDisplayAlertMessage();

                value.Should().BeTrue();
            }

            [TestMethod]
            public void RpmsCredentialsAreExpired_returns_true()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                bool? value = controller.RpmsCredentialsAreExpired();

                value.Should().BeTrue();
            }
            [TestMethod]
            public void GetAlertMessageText_returns_correctString()
            {
                var controller = new RpmsExportController(mockDateService.Object, mockConfigurationService.Object, globalSettingsServiceMock.Object);
                string value = controller.GetAlertMessageText();

                value.Should().Be(FrontDesk.Server.Resources.TextMessages.RPMSCredentialsExpiredMessage);
            }

        }
    }
}
