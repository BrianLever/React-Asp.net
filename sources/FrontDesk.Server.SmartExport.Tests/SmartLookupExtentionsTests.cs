using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RPMS.Common.Models;
using RPMS.UnitTest.Factory;
using RPMS.Tests.MotherObjects;

using ScreenDox.EHR.Common.SmartExport;

using System.Collections.Generic;

namespace FrontDesk.Server.SmartExport.Tests
{
    [TestClass]
    public class SmartLookupExtentionsTests
    {
        protected List<Patient> Sut()
        {
            return new List<Patient>
            {
                PatientFactory.CreateANGELA(),
                PatientFactory.CreateCHRIS(),
            };
        }

        [TestMethod]
        public void WhenFoundExactMatch_BestResult_ShouldNotBeNull()
        {
            var bestMatch = Sut().FindBestMatch();

            bestMatch.BestResult.Should().NotBeNull();
        }


        [TestMethod]
        public void WhenFoundGeminiMatch_BestResult_ShouldReturnBestMatch()
        {
           
        }
    }
}
