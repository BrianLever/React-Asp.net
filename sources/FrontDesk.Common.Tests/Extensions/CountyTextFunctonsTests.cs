using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FrontDesk.Common.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontDesk.Common.Tests.Extensions
{
    [TestClass]
    public class CountyTextFunctonsTests
    {
        [TestMethod]
        public void CanParseNativeVillage()
        {
            var countyName = "Native Village of Unalakleet";

            CountyTextFunctions.ParseCounty(countyName)
                .Name.Should().Be(countyName);
        }
    }
}
