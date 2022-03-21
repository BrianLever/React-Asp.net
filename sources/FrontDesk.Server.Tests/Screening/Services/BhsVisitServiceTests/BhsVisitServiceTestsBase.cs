using FrontDesk.Common.Bhservice;
using FrontDesk.Server.Controllers;
using FrontDesk.Server.Data;
using FrontDesk.Server.Data.BhsVisits;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Services;

using Moq;

using System;

namespace FrontDesk.Server.Tests.Screening.Services.BhsVisitServiceTests
{
    public abstract class BhsVisitServiceTestsBase
    {
        protected Mock<IBhsVisitRepository> _bhsVisitRepositoryMock;
        protected Mock<IBhsVisitFactory> _visitFactoryMock;
        protected Mock<IBhsDemographicsService> _demographicsServiceMock;
        protected Mock<IBhsFollowUpService> _followUpServiceMock;
        protected Mock<IScreenerResultReadRepository> _resultReaderRepositoryMock;
        protected Mock<IBhsHistoryRepository> _historyRepositoryMock;


        public BhsVisitServiceTestsBase()
        {
            _bhsVisitRepositoryMock = new Mock<IBhsVisitRepository>();
            _visitFactoryMock = new Mock<IBhsVisitFactory>();
            _demographicsServiceMock = new Mock<IBhsDemographicsService>();
            _followUpServiceMock = new Mock<IBhsFollowUpService>();
            _historyRepositoryMock = new Mock<IBhsHistoryRepository>();
            _resultReaderRepositoryMock = new Mock<IScreenerResultReadRepository>();

            _visitFactoryMock.Setup(x => x.Create(It.IsAny<ScreeningResult>(), It.IsAny<FrontDesk.Screening>()))
                .Returns<ScreeningResult, FrontDesk.Screening>((r, info) => new BhsVisit
                {
                    ID = DateTime.UtcNow.Ticks,
                    ScreeningResultID = r.ID
                });

            _bhsVisitRepositoryMock.Setup(x => x.Add(It.IsAny<BhsVisit>())).Returns<BhsVisit>((x) => x.ID);

            _demographicsServiceMock.Setup(x => x.Create(It.IsAny<ScreeningResult>())).Returns(2);
        }


        protected BhsVisitService Sut()
        {
            return new BhsVisitService(
                _bhsVisitRepositoryMock.Object, 
                _visitFactoryMock.Object, new VisitCreator(), _demographicsServiceMock.Object, 
                _followUpServiceMock.Object,
                _historyRepositoryMock.Object,
                _resultReaderRepositoryMock.Object);
        }
    }
}
