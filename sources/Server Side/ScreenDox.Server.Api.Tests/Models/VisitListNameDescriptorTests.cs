using FluentAssertions;

using FrontDesk.Server.Data.BhsVisits;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Linq;

namespace ScreenDox.Server.Api.Tests.Models
{
    [TestClass]
    public class VisitListNameDescriptorTests
    {
        [TestMethod]
        public void VisitListNameDescriptor_ReturnsSupportedField()
        {
            var props = LookupListsDataSource.VisitListNameDescriptor.GetSupportedLists();

            props.Length.Should().Be(15);
        }


        [TestMethod]
        public void VisitListNameDescriptor_Return_Field_Value()
        {
            var props = LookupListsDataSource.VisitListNameDescriptor.GetSupportedLists();

            props.First().Should().Be("Discharged");
        }
    }
}
