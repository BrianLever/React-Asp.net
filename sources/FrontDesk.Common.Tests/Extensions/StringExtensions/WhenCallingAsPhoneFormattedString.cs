using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace FrontDesk.Common.Tests.Extensions.StringExtensions
{
    [TestClass]
    public class WhenCallingAsPhoneFormattedString
    {
        [TestMethod]
        public void CanParseStringWithoutSeperators()
        {
            "2132023970".AsPhoneFormattedString().Should().Be("(213) 202-3970");
        }

        [TestMethod]
        public void CanParseStringWithThreeDashes()
        {
            "213-202-3970".AsPhoneFormattedString().Should().Be("(213) 202-3970");
        }

        [TestMethod]
        public void CanParseStringWithBracketss()
        {
            "(213) 202-3975".AsPhoneFormattedString().Should().Be("(213) 202-3975");
        }
    }
}
