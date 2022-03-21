using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FrontDesk.Server.Web.Formatters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontDesk.Server.Tests.Web
{
    /// <summary>
    /// Summary description for IndicatorReportMainQuestionFormatterTest
    /// </summary>
    [TestClass]
    public class IndicatorReportMainQuestionFormatterTest
    {

        public IndicatorReportMainQuestionFormatterTest()
        {
        }
        
        [TestMethod]
        public void SkipsModificationWhenNoPreamble()
        {
            var actual = "Do you drink alcohol?".FormatAsQuestionWithPreamble();

            actual.Should().Be("Do you drink alcohol?", "String should not be changed");
        }

        [TestMethod]
        public void UnderstandsPreamble()
        {
            var actual = "Over the LAST 12 MONTHS:|Have you used drugs other than those required for medical reasons?".FormatAsQuestionWithPreamble();

            actual.Should().Be("<span>Over the LAST 12 MONTHS: Have you used drugs other than those required for medical reasons?</span>", "String should be formatted");
        }
    }
}
