using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Frontdesk.Kiosk.Simulator.Utils;

namespace Frontdesk.Kiosk.Simulator.Tests
{
    [DeploymentItem(@"Scenarios\1_MacyHurt_all_positive.txt", "Scenarios")]
    [DeploymentItem(@"Scenarios\2_MacyHurt_negative.txt", "Scenarios")]
    [DeploymentItem(@"Scenarios\1_Garerd_allpositive.txt", "Scenarios")]

    [TestClass]
    public class TestDataReaderTest
    {
        protected TestDataReader Sut()
        {
            return new TestDataReader()
            {
                
            };
        }

        [TestMethod]
        public void TestDataReader_Parse_SingleFile()
        {
            var result = Sut().GetTestSequence(@".\Scenarios\1_MacyHurt_all_positive.txt");

            result.Should().NotBeEmpty();
            result.Count.Should().Be(1, "should parse one file");
        }

        [TestMethod]
        public void TestDataReader_Parse_Directory()
        {
            var result = Sut().GetTestSequence(@".\Scenarios");

            result.Should().NotBeEmpty();
            result.Count.Should().BeGreaterThan(1, "should parse all files in directory");
        }
    }
}
