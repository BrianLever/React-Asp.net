using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrontDesk;

namespace FrontDesk.Common.Tests.Extensions.StringExtensions
{
    [TestClass]
    public class WhenCallingAsMaskedPasswordTest
    {
        [TestMethod]
        public void WithPasswordMoreThan4Symbols()
        {
            "ABCDE".AsMaskedPassword().Should().Be("AB*DE");
        }

        [TestMethod]
        public void WithPasswordWithFourSymbols()
        {
            "ABCD".AsMaskedPassword().Should().Be("A**D");
        }

        [TestMethod]
        public void WithPasswordLessThanFourSymbols()
        {
            "ABC".AsMaskedPassword().Should().Be("A**");
        }

        [TestMethod]
        public void WithPasswordIsOneSymbol()
        {
            "A".AsMaskedPassword().Should().Be("A");
        }
    }
}
