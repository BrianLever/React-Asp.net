using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using FrontDesk.Services;
using FluentAssertions;
using FrontDesk.Common.Bhservice;
using FrontDesk.Server.Data.BhsVisits;
using FrontDesk.Server.Tests.MotherObjects;

namespace FrontDesk.Server.Tests.Screening.Services.BhsVisitServiceTests
{
    [TestClass()]
    public class BhsVisitServiceCreatingTests : BhsVisitServiceTestsBase
    {
       
        public BhsVisitServiceCreatingTests()
        {
           
        }

        [TestMethod()]
        [TestCategory("Integration")]
        public void BhsVisit_Saved()
        {

            var sut = Sut();
            var visit1 = sut.Create(1);

            _bhsVisitRepositoryMock.Verify(x => x.Add(It.IsAny<BhsVisit>()), Times.Once());
        }

        [TestMethod()]
        [TestCategory("Integration")]
        public void BhsVisit_CreatedOnlyOnceForScreening()
        {

            var sut = Sut();
            var visit1 = sut.Create(1);
            _bhsVisitRepositoryMock.Setup(x => x.FindByScreeningResultId(1)).Returns(visit1.ID);
            _bhsVisitRepositoryMock.Setup(x => x.Get(visit1.ID)).Returns(visit1);

            var visit2 = sut.Create(1);

            visit2.ID.Should().Be(visit1.ID);

        }

        [TestMethod()]
        [TestCategory("Integration")]
        public void BhsVisit_WhenCreating_CallsCreationOfDemographic()
        {

            _demographicsServiceMock.Setup(x => x.Exists(It.IsAny<ScreeningResult>()))
                .Returns(false);

            var sut = Sut();
            sut.Create(1);

            _demographicsServiceMock.Verify(x => x.Create(It.IsAny<ScreeningResult>()), Times.Once());

        }


    }
}