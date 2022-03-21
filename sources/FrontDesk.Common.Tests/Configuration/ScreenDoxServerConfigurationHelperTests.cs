using System;
using FluentAssertions;
using FrontDesk.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontDesk.Common.Tests.Configuration
{
    [TestClass]
    public class ScreenDoxServerConfigurationHelperTests
    {
        [TestMethod]
        public void GetFqdnKioskEndpointAddress_Ignores_ForwardSlash_AtTheEnd()
        {
            ScreenDoxServerConfigurationHelper.GetFqdnKioskEndpointAddress("https://server.com/screendox/")
                .Should().Be("https://server.com/screendox/endpoint/KioskEndpoint.svc");
        }

        [TestMethod]
        public void GetFqdnKioskEndpointAddress_When_NoSeperatatorAtEnd_Returns_Correct()
        {
            ScreenDoxServerConfigurationHelper.GetFqdnKioskEndpointAddress("https://server.com/screendox")
                .Should().Be("https://server.com/screendox/endpoint/KioskEndpoint.svc");
        }

        [TestMethod]
        public void GetFqdnKioskEndpointAddress_When_OnlyServer_Returns_Correct()
        {
            ScreenDoxServerConfigurationHelper.GetFqdnKioskEndpointAddress("https://screendox.com")
                .Should().Be("https://screendox.com/endpoint/KioskEndpoint.svc");
        }
    }
}
