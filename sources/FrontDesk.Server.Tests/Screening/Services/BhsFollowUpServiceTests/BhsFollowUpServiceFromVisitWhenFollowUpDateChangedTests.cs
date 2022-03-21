using FluentAssertions;
using FrontDesk.Common.Bhservice;
using FrontDesk.Server.Data.BhsVisits;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Tests.MotherObjects;
using FrontDesk.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Tests.Screening.Services.BhsFollowUpServiceTests
{
    [TestClass]
    public class BhsFollowUpServiceFromVisitWhenFollowUpDateChangedTests
    {
        protected readonly Mock<IBhsFollowUpRepository> _repositoryMock = new Mock<IBhsFollowUpRepository>();
        protected readonly IBhsFollowUpFactory _factory = new BhsFollowUpFactory();
        protected readonly Mock<IBhsVisitRepository> _visitRepositoryMock = new Mock<IBhsVisitRepository>();


        public BhsFollowUpServiceFromVisitWhenFollowUpDateChangedTests()
        {
            _repositoryMock.Setup(x => x.FindByVisitId(1)).Returns((int?)null);
            _visitRepositoryMock.Setup(x => x.Get(It.IsAny<long>())).Returns(BhsVisitMotherObject.GetDefault());
        }

        protected BhsFollowUpService Sut()
        {
            return new BhsFollowUpService(_repositoryMock.Object, _factory, _visitRepositoryMock.Object);
        }

        protected BhsVisit GetVisit()
        {
            var model = BhsVisitMotherObject.GetDefault();
            model.ID = 1005;
            model.NewVisitDate = DateTimeOffset.Now;
            model.FollowUpDate = model.NewVisitDate.Value.AddDays(5);

            return model;
        }

        [TestMethod()]
        public void BhsFollowUp_WhenCreatingFromVisit_AndExistNonCompletedFollowUpExists_AndNotCompleted_ShouldRecreateFollowUp()
        {
            var sut = Sut();
            var visitModel = GetVisit();
            var followUpModel = BhsThirtyDayFollowUpMotherObject.GetDefault();
            followUpModel.ScheduledFollowUpDate = visitModel.NewVisitDate.Value;


            _repositoryMock.Setup(x => x.FindByVisitId(visitModel.ID)).Returns(5001);
            _repositoryMock.Setup(x => x.Get(5001)).Returns(followUpModel);

            sut.Create(visitModel);

            _repositoryMock.Verify(x => x.Delete(5001), Times.Once(), "should drop existing");
            _repositoryMock.Verify(x => x.Add(It.IsAny<BhsFollowUp>()), Times.Once());

        }

        [TestMethod()]
        public void BhsFollowUp_WhenCreatingFromVisit_AndExistNonCompletedFollowUpExists_AndCompleted_ShouldCreateAnotherFollowUp()
        {
            var sut = Sut();
            var visitModel = GetVisit();
            var followUpModel = BhsThirtyDayFollowUpMotherObject.GetDefault();
            followUpModel.CompleteDate = DateTimeOffset.Now;


            _repositoryMock.Setup(x => x.FindByVisitId(visitModel.ID)).Returns(5001);
            _repositoryMock.Setup(x => x.Get(5001)).Returns(followUpModel);

            sut.Create(visitModel);

            _repositoryMock.Verify(x => x.Delete(5001), Times.Never(), "should keep existing");
            _repositoryMock.Verify(x => x.Add(It.IsAny<BhsFollowUp>()), Times.Once(), "should create new");

        }

    }
}
