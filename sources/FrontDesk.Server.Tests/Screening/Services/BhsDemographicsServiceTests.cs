using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrontDesk.Server.Screening.Services;
using Moq;
using FrontDesk.Server.Data.BhsVisits;
using FrontDesk.Services;
using FrontDesk.Server.Tests.MotherObjects;
using FrontDesk.Common.Bhservice;

namespace FrontDesk.Server.Tests.Screening.Services
{
    [TestClass]
    public class BhsDemographicsServiceTests
    {
        protected readonly Mock<IBhsDemographicsRepository> _demographicsReposiotyMock = new Mock<IBhsDemographicsRepository>();
        protected readonly Mock<IBhsDemographicsFactory> _factoryMock = new Mock<IBhsDemographicsFactory>();

        protected BhsDemographicsService Sut()
        {
            return new BhsDemographicsService(_demographicsReposiotyMock.Object, _factoryMock.Object);
        }

        [TestMethod()]
        public void BhsDemographicsService_CallsCreationOfDemographicsWhenNotExists()
        {

            _demographicsReposiotyMock.Setup(x => x.Exists(It.IsAny<ScreeningResult>()))
                .Returns(false);

            var sut = Sut();
            sut.Create(ScreeningResultMotherObject.GetAllYesAnswers());

            _demographicsReposiotyMock.Verify(x => x.Add(It.IsAny<BhsDemographics>()), Times.Once());

        }
    }
}
