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
    public class BhsFollowUpServiceUpdatingTests
    {
        protected readonly Mock<IBhsFollowUpRepository> _repositoryMock = new Mock<IBhsFollowUpRepository>();
        protected readonly IBhsFollowUpFactory _factory = new BhsFollowUpFactory();
        protected readonly Mock<IBhsVisitRepository> _visitRepositoryMock = new Mock<IBhsVisitRepository>();


        public BhsFollowUpServiceUpdatingTests()
        {
            _repositoryMock.Setup(x => x.FindByVisitId(1)).Returns((int?)null);
            _visitRepositoryMock.Setup(x => x.Get(It.IsAny<long>())).Returns(BhsVisitMotherObject.GetDefault());
        }

        protected BhsFollowUpService Sut()
        {
            return new BhsFollowUpService(_repositoryMock.Object, _factory, _visitRepositoryMock.Object);
        }

        [TestMethod()]
        public void BhsThirtyDayFollowUp_WhenUpdating_ShouldCallCreatingOfFollowUp_WhenThirtyDayFollowUpIsOn()
        {
            var sut = Sut();
            var model = BhsThirtyDayFollowUpMotherObject.GetDefault();
            model.FollowUpDate = DateTimeOffset.Now;
            model.NewVisitDate = DateTimeOffset.Now;
          
            sut.Update(model, UserMotherObject.Create());

            _repositoryMock.Verify(x => x.Add(It.IsAny<BhsFollowUp>()), Times.Once());

        }

        [TestMethod()]
        public void BhsThirtyDayFollowUp_WhenUpdating_FollowUpBecomeDisabled_WhenNoNewVisitDate()
        {
            var sut = Sut();
            var model = BhsThirtyDayFollowUpMotherObject.GetDefault();
            model.FollowUpDate = DateTimeOffset.Now;
            model.NewVisitDate = null;
            BhsFollowUp repoModel = null;
            _repositoryMock.Setup(x => x.UpdateManualEntries(It.IsAny<BhsFollowUp>())).Callback< BhsFollowUp>(x => repoModel = x);

            sut.Update(model, UserMotherObject.Create());

            repoModel.Should().NotBeNull();
            repoModel.ThirtyDatyFollowUpFlag.Should().BeFalse();
            repoModel.FollowUpDate.Should().NotHaveValue();
        }

        [TestMethod()]
        public void BhsThirtyDayFollowUp_WhenUpdating_ShouldNotCreateFollowUp_WhenThirtyDayFollowUpIsOff()
        {
            var sut = Sut();
            var model = BhsThirtyDayFollowUpMotherObject.GetDefault();
            model.FollowUpDate = null;
            var today = DateTimeOffset.Now.Date;

            sut.Update(model, UserMotherObject.Create());

            _repositoryMock.Verify(x => x.Add(It.IsAny<BhsFollowUp>()), Times.Never());

        }

        [TestMethod()]
        public void BhsThirtyDayFollowUp_WhenUpdating_CallRepository()
        {
            var sut = Sut();
            var model = BhsThirtyDayFollowUpMotherObject.GetDefault();
            model.FollowUpDate = null;
            var today = DateTimeOffset.Now.Date;

            sut.Update(model, UserMotherObject.Create());

            _repositoryMock.Verify(x => x.Add(It.IsAny<BhsFollowUp>()), Times.Never());

        }

        [TestMethod()]
        public void BhsThirtyDayFollowUp_WhenUpdating__NewFollowUp_ShouldHas_FollowUpDate_In30Days_FromNewVisitDate()
        {
            BhsFollowUp creatingFollowUp = null;
            var sut = Sut();
            var model = BhsThirtyDayFollowUpMotherObject.GetDefault();

            model.NewVisitDate = DateTimeOffset.Now.Date.AddDays(30);
            var thirtyDateFromVisit = model.NewVisitDate.Value.Date.AddDays(30);
            model.FollowUpDate = thirtyDateFromVisit;

            _repositoryMock.Setup(x => x.Add(It.IsAny<BhsFollowUp>()))
                .Callback<BhsFollowUp>(x => creatingFollowUp = x);

            sut.Update(model, UserMotherObject.Create());

            creatingFollowUp.Should().NotBeNull();
            creatingFollowUp.ScheduledFollowUpDate.Should().Be(thirtyDateFromVisit);
        }

        [TestMethod()]
        public void BhsThirtyDayFollowUp_WhenUpdating_NewFollowUp_ShouldHas_ScheduledVisitDate_IsEqualToNewVisitDate()
        {
            BhsFollowUp creatingFollowUp = null;
            var sut = Sut();
            var model = BhsThirtyDayFollowUpMotherObject.GetDefault();
            model.NewVisitDate = DateTimeOffset.Now.Date.AddDays(30);
            model.FollowUpDate = model.NewVisitDate.Value.AddDays(30);

            _repositoryMock.Setup(x => x.Add(It.IsAny<BhsFollowUp>()))
                .Callback<BhsFollowUp>(x => creatingFollowUp = x);

            sut.Update(model, UserMotherObject.Create());

            creatingFollowUp.Should().NotBeNull();
            creatingFollowUp.ScheduledVisitDate.Date.Should().Be(model.NewVisitDate.Value.Date);
        }


        [TestMethod()]
        public void BhsThirtyDayFollowUp_WhenUpdating_DoNotCreateAnotherFollowUp_WhenAlreadyCreated_AndWhenThirtyDayFollowUpIsOn()
        {
            var sut = Sut();

            var model = BhsThirtyDayFollowUpMotherObject.GetDefault();
            model.FollowUpDate = DateTimeOffset.Now;

            _repositoryMock.Setup(x => x.FindByParentFollowUpId(It.IsAny<long>())).Returns(5);

            sut.Update(model, UserMotherObject.Create());

            _repositoryMock.Verify(x => x.Add(It.IsAny<BhsFollowUp>()), Times.Never(), "should not call creating follow-up report when there is one with ParentFollowUpID");

        }


        [TestMethod()]
        public void BhsThirtyDayFollowUp_WhenUpdating_TryToFindCorrectParentFollowUpID_WhenThirtyDayFollowUpIsOn()
        {
            var sut = Sut();

            var model = BhsThirtyDayFollowUpMotherObject.GetDefault();
            model.FollowUpDate = DateTimeOffset.Now;
            model.NewVisitDate = DateTimeOffset.Now;
            model.ID = 1021;

            sut.Update(model, UserMotherObject.Create());

            _repositoryMock.Verify(x => x.FindByParentFollowUpId(It.Is<long>(p => p == model.ID)), Times.Once(), "Should try to find if follow-up report created");
        }

        [TestMethod()]
        public void BhsThirtyDayFollowUp_WhenUpdating_SetsValidParentFollowUpId_WhenCreatingFollowUpReport()
        {
            var sut = Sut();
            BhsFollowUp newFollowUpModel = null;
            var model = BhsThirtyDayFollowUpMotherObject.GetDefault();
            model.FollowUpDate = DateTimeOffset.Now;
            model.NewVisitDate = DateTimeOffset.Now;
            model.ID = 1021;

            _repositoryMock.Setup(x => x.Add(It.IsAny<BhsFollowUp>())).Callback<BhsFollowUp>(x => newFollowUpModel = x);


            sut.Update(model, UserMotherObject.Create());

            newFollowUpModel.Should().NotBeNull("New follow-up should be created");
            newFollowUpModel.ParentFollowUpID.Should().Be(model.ID);
        }
    }
}
