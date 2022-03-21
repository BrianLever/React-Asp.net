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
    public class BhsVisitServiceUpdatingTests : BhsVisitServiceTestsBase
    {


        public BhsVisitServiceUpdatingTests()
        {
            

        }

        
        [TestMethod()]
        public void BhsVisit_WhenUpdating_ShouldCallCreatingOfFollowUp_WhenThirtyDayFollowUpIsOn()
        {
            var sut = Sut();
            var model = BhsVisitMotherObject.GetDefault();
            model.FollowUpDate = DateTimeOffset.Now;

            sut.Update(model, UserMotherObject.Create());

            _followUpServiceMock.Verify(x => x.Create(model), Times.Once());

        }



        [TestMethod()]
        public void BhsVisit_WhenUpdating_FollowUpBecomeDisabled_WhenNoNewVisitDate()
        {
            var sut = Sut();
            var model = BhsVisitMotherObject.GetDefault();
            model.FollowUpDate = DateTimeOffset.Now;
            model.NewVisitDate = null;

            BhsVisit repoModel = null;

            _bhsVisitRepositoryMock.Setup(x => x.UpdateManualEntries(It.IsAny<BhsVisit>())).Callback<BhsVisit>(x => repoModel = x);

            sut.Update(model, UserMotherObject.Create());

            repoModel.Should().NotBeNull();
            repoModel.ThirtyDatyFollowUpFlag.Should().BeFalse();
        }


        [TestMethod()]
        public void BhsVisit_WhenUpdating_FollowUpRemainsEnabled_WhenHasNewVisitDate()
        {
            var sut = Sut();
            var model = BhsVisitMotherObject.GetDefault();
            model.NewVisitDate = DateTimeOffset.Now.AddDays(30).Date;
            model.FollowUpDate = model.NewVisitDate.Value.AddDays(5);

            BhsVisit repoModel = null;

            _bhsVisitRepositoryMock.Setup(x => x.UpdateManualEntries(It.IsAny<BhsVisit>())).Callback<BhsVisit>(x => repoModel = x);

            sut.Update(model, UserMotherObject.Create());

            repoModel.Should().NotBeNull();
            repoModel.ThirtyDatyFollowUpFlag.Should().BeTrue();
            repoModel.FollowUpDate.Should().HaveValue();
        }
                
    }
}