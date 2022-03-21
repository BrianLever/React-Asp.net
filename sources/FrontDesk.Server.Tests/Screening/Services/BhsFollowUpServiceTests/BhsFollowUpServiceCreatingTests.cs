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
    public class BhsFollowUpServiceCreatingTests
    {
        protected readonly Mock<IBhsFollowUpRepository> _repositoryMock = new Mock<IBhsFollowUpRepository>();
        protected readonly IBhsFollowUpFactory _factory = new BhsFollowUpFactory();
        protected readonly Mock<IBhsVisitRepository> _visitRepositoryMock = new Mock<IBhsVisitRepository>();

        public BhsFollowUpServiceCreatingTests()
        {
            _repositoryMock.Setup(x => x.FindByVisitId(1)).Returns((int?)null);
            _visitRepositoryMock.Setup(x => x.Get(It.IsAny<long>())).Returns(BhsVisitMotherObject.GetDefault());
        }

        protected BhsFollowUpService Sut()
        {
            return new BhsFollowUpService(_repositoryMock.Object, _factory, _visitRepositoryMock.Object);
        }

        [TestMethod]
        public void BhsFollowUpService_WhenCreating_ScreeningReportIdIsNotEmpty()
        {
            var visit = BhsVisitMotherObject.GetDefault();
            visit.FollowUpDate = DateTime.Now.AddDays(30);

            BhsFollowUp model = null; ;
            _repositoryMock.Setup(x => x.Add(It.IsAny<BhsFollowUp>())).Callback<BhsFollowUp>((x) => { model = x; });

            Sut().Create(visit);
            model.Should().NotBeNull();
            model.ScreeningResultID.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void BhsFollowUpService_WhenCreating_CreatedDateIsSet()
        {
            var visit = BhsVisitMotherObject.GetDefault();
            var nextVisitDate = DateTime.Now.Date.AddDays(30);
            visit.FollowUpDate = nextVisitDate;
            BhsFollowUp model = null; ;
            _repositoryMock.Setup(x => x.Add(It.IsAny<BhsFollowUp>())).Callback<BhsFollowUp>((x) => { model = x; });

            Sut().Create(visit);
            model.Should().NotBeNull();
            model.CreatedDate.Date.Should().Be(DateTimeOffset.Now.Date);
        }

        [TestMethod]
        public void BhsFollowUpService_WhenCreating_AndNotExists_RepositoryCreateCalled()
        {
            var visit = BhsVisitMotherObject.GetDefault();
            visit.FollowUpDate = DateTimeOffset.Now;

            Sut().Create(visit);

            _repositoryMock.Verify(x => x.Add(It.IsAny<BhsFollowUp>()), Times.Once());

        }


        [TestMethod]
        public void BhsFollowUpService_WhenCreatingFromVisit_ScheduledVisitDate_EqualsToNewVisitDate()
        {
            BhsFollowUp actual = null; ;
            var visit = BhsVisitMotherObject.GetDefault();
            visit.NewVisitDate = DateTime.Now.Date.AddDays(10);
            visit.FollowUpDate = visit.NewVisitDate.Value.AddDays(5);

            _repositoryMock.Setup(x => x.Add(It.IsAny<BhsFollowUp>())).Callback<BhsFollowUp>((x) => { actual = x; });

            Sut().Create(visit);
            actual.Should().NotBeNull();
            actual.ScheduledVisitDate.Date.Should().Be(visit.NewVisitDate.Value.Date);

        }

        [TestMethod]
        public void BhsFollowUpService_WhenCreatingFromVisit_FollowUpDate_EqualsToNewVisitDate()
        {
            BhsFollowUp model = null; ;
            var visit = BhsVisitMotherObject.GetDefault();
            visit.NewVisitDate = DateTime.Now.Date.AddDays(1);
            var expectedFollowUpDate = visit.FollowUpDate = visit.NewVisitDate.Value.AddDays(30);


            _repositoryMock.Setup(x => x.Add(It.IsAny<BhsFollowUp>())).Callback<BhsFollowUp>((x) => { model = x; });

            Sut().Create(visit);
            model.Should().NotBeNull();
            model.ScheduledFollowUpDate.Date.Should().Be(expectedFollowUpDate.Value.Date);

        }

    }
}
