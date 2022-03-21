using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrontDesk.Server.Web.Notification;
using FluentAssertions;

namespace FrontDesk.Server.Tests.Web.Notification
{
    [TestClass]
    public class NotificationMessagesHubTests
    {
        protected NotificationMessagesHub Sut()
        {
            return new NotificationMessagesHub();
        }

        [TestMethod]
        public void NotificationMessagesHub_ToHtml_GeneratesJsBlock()
        {
            var sut = Sut().Add("Notification message", NotificationCategory.Success);

            var result = sut.ToHtml();

            result.Should().NotBeEmpty();
            result.Should().Contain("Notification message");
        }

        [TestMethod]
        public void NotificationMessagesHub_ToHtml_ReturnsEmptyString_WhenNoMessages()
        {
            var sut = Sut(); ;

            var result = sut.ToHtml();

            result.Should().BeEmpty();
        }
    }
}
