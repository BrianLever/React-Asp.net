using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FrontDesk.Server.Screening.Mappers;
using System.Linq;
using FluentAssertions;
using FrontDesk.Server.Screening.Models;

namespace FrontDesk.Server.Tests.Screening.Mappers
{
    [TestClass]
    public class BhsVisitListItemDtoModelMapperTests
    {

        protected IList<BhsVisitListItemDtoModel> CreateTestData()
        {
            return new List<BhsVisitListItemDtoModel>()
            {
                new BhsVisitListItemDtoModel //1
                {
                    PatientName = "DEMO, BONNIE",
                    Birthday = new DateTime(1985,01,01),
                    ScreeningResultID = 30349,
                    ID = 30180,
                    CreatedDate = DateTimeOffset.Parse("2018-02-04 17:46:47.0506872 +02:00"),
                    ScreeningDate = DateTimeOffset.Parse("2018-02-04 17:46:46.9726882 +02:00"),
                    CompletedDate = null,
                    IsVisitRecordType = true,
                    DemographicsID = 20010,
                    DemographicsScreeningDate = DateTimeOffset.Parse("2017-12-13 01:16:02.9488740 +02:00"),
                    DemographicsCreatedDate = DateTimeOffset.Parse("2017-12-13 01:16:03.3776852 +02:00"),
                    DemographicsCompleteDate = null,

                },
                new BhsVisitListItemDtoModel
                {
                    PatientName = "DEMO, PATIENT Drugs Low", //2
                    Birthday = new DateTime(1960,12, 20),
                    ScreeningResultID = 30334,
                    ID = 30168,
                    CreatedDate = DateTimeOffset.Parse("2018-02-04 15:08:39.7578119 +02:00"),
                    ScreeningDate = DateTimeOffset.Parse("2018-02-04 15:08:39.7003808 +02:00"),
                    CompletedDate = DateTimeOffset.Parse("2018-02-04 15:20:23.3403685 +02:00"),
                    IsVisitRecordType = true,
                    DemographicsID = 20017,
                    DemographicsScreeningDate = DateTimeOffset.Parse("2018-01-02 00:16:23.6760551 +02:00"),
                    DemographicsCreatedDate = DateTimeOffset.Parse("2018-01-02 00:16:23.6920547 +02:00"),
                    DemographicsCompleteDate = DateTimeOffset.Parse("2018-02-04 02:27:05.0149768 +02:00"),
                },
                new BhsVisitListItemDtoModel //1
                {
                    PatientName = "DEMO, BONNIE",
                    Birthday = new DateTime(1985,01,01),
                    ScreeningResultID = 20264,
                    ID = 20143,
                    CreatedDate = DateTimeOffset.Parse("2017-12-25 01:25:33.1200550 +02:00"),
                    ScreeningDate = DateTimeOffset.Parse("2017-12-25 01:25:33.0695569 +02:00"),
                    CompletedDate = DateTimeOffset.Parse("2018-01-13 00:18:02.4561514 +02:00"),
                    IsVisitRecordType = true,
                    DemographicsID = 20010,
                    DemographicsScreeningDate = DateTimeOffset.Parse("2017-12-13 01:16:02.9488740 +02:00"),
                    DemographicsCreatedDate = DateTimeOffset.Parse("2017-12-13 01:16:03.3776852 +02:00"),
                    DemographicsCompleteDate = null,
                },
                new BhsVisitListItemDtoModel
                {
                    PatientName = "DEMO, BONNIE", //1
                    Birthday = new DateTime(1985,01,01),
                    ScreeningResultID = 20259,
                    ID = 20138,
                    CreatedDate = DateTimeOffset.Parse("2017-12-25 01:13:53.7523759 +02:00"),
                    ScreeningDate = DateTimeOffset.Parse("2017-12-25 01:13:53.3759121 +02:00"),
                    CompletedDate = null,
                    IsVisitRecordType = true,
                    DemographicsID = 20010,
                    DemographicsScreeningDate = DateTimeOffset.Parse("2017-12-13 01:16:02.9488740 +02:00"),
                    DemographicsCreatedDate = DateTimeOffset.Parse("2017-12-13 01:16:03.3776852 +02:00"),
                    DemographicsCompleteDate = null,
                },
                new BhsVisitListItemDtoModel
                {
                    PatientName = "DEMO, BONNIE", //3
                    Birthday = new DateTime(1985,10,01),
                    ScreeningResultID = 20227,
                    ID = 20106,
                    CreatedDate = DateTimeOffset.Parse("2017-12-13 01:16:03.3596432 +02:00"),
                    ScreeningDate = DateTimeOffset.Parse("2017-12-13 01:16:02.9488740 +02:00"),
                    CompletedDate = null,
                    IsVisitRecordType = true
                },


            };
        }

        [TestMethod]
        public void BhsVisitListItemDtoModelMapper_IsNotNull()
        {
            var actual = CreateTestData().ToViewModel().ToList();

            actual.Should().NotBeNull();
        }

        [TestMethod]
        public void BhsVisitListItemDtoModelMapper_CreatesPatientGroups()
        {
            var actual = CreateTestData().ToViewModel().ToList();

            actual.Count.Should().Be(3);
        }

        [TestMethod]
        public void BhsVisitListItemDtoModelMapper_Should_MostRecent_IsFirst()
        {
            var actual = CreateTestData().ToViewModel().First();

            actual.PatientName.Should().Be("DEMO, BONNIE");
            actual.Birthday.Should().Be(new DateTime(1985, 01, 01));
        }

        [TestMethod]
        public void BhsVisitListItemDtoModelMapper_Patient_Should_IncudeAllRelatedRecords()
        {
            var actual = CreateTestData().ToViewModel().First();

            actual.ReportItems.Where(x => x.IsVisitRecordType).Count().Should().Be(3);
        }

        [TestMethod]
        public void BhsVisitListItemDtoModelMapper_Patient_Should_MostRecentItemOnTop()
        {
            var actual = CreateTestData().ToViewModel().First();

            actual.ReportItems.First(x => x.IsVisitRecordType).ID.Should().Be(30180);
        }

        [TestMethod]
        public void BhsVisitListItemDtoModelMapper_Should_NewPatient_WhenBirthdayIsDifferent()
        {
            var actual = CreateTestData().ToViewModel().ToList().Last();


            actual.PatientName.Should().Be("DEMO, BONNIE");
            actual.Birthday.Should().Be(new DateTime(1985, 10, 01));
        }


        [TestMethod]
        public void BhsVisitListItemDtoModelMapper_Should_CreateDemographicsRecord()
        {
            var actual = CreateTestData().ToViewModel().ToList().First();

            actual.ReportItems.First().IsVisitRecordType.Should().BeFalse("First record should be Demographics");
            actual.ReportItems.First().ID.Should().Be(20010);
        }



    }
}
