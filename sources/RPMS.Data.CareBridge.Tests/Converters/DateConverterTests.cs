using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RPMS.Data.CareBridge.Converters;

namespace RPMS.Data.CareBridge.Tests.Converters
{
    [TestCategory("CareBridge_NextGen")]
    [TestClass]
    public class DateConverterTests
    {

        protected DateConverter Sut()
        {
            return new DateConverter();
        }

        [TestMethod]
        public void CanParseDate()
        {
            string testJson = "19480819";

            var value = JsonConvert.DeserializeObject<DateTime>(testJson, Sut());

            value.Should().Be(new DateTime(1948, 08, 19));
        }

        [TestMethod]
        public void CanParseDateWithTime()
        {
            string testJson = "20180630140418";

            var value = JsonConvert.DeserializeObject<DateTime>(testJson, Sut());

            value.Should().Be(new DateTime(2018, 06, 30, 14, 04, 18));
        }
    }
}
