using System;
using System.Collections.Generic;

using Common.Logging;

using FluentAssertions;

using Frontdesk.Server.SmartExport.Data;
using Frontdesk.Server.SmartExport.EhrInterfaceService;
using Frontdesk.Server.SmartExport.Models;
using Frontdesk.Server.SmartExport.Services;

using FrontDesk;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Tests.MotherObjects;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using RPMS.Common.Models;

namespace ScreenDox.Rpms.SmartExport.App.Tests.Services
{
    [TestClass]
    [TestCategory("SmartExport")]
    public class SmartExportServiceTests
    {
        private Mock<ISmartExportRepository> _repository = new Mock<ISmartExportRepository>();
        private Mock<IEhrInterfaceProxy> _proxy = new Mock<IEhrInterfaceProxy>();
        private Mock<IScreeningResultService> _screeningResultServiceMock = new Mock<IScreeningResultService>();
        private Mock<IUserService> _userServiceProxy = new Mock<IUserService>();
        private Mock<IAppConfigurationService> _appConfigurationServiceMock = new Mock<IAppConfigurationService>();
        private Mock<IScreeningDefinitionService> _screeningDefServiceMock = new Mock<IScreeningDefinitionService>();

        private Mock<ILog> _logger = new Mock<ILog>();
        private ScreeningResult _result;

        public SmartExportServiceTests()
        {
            _result = ScreeningResultMotherObject.GetAllNoAnswers();
            _result.ExportedToHRN = "98765";
            _result.ExportedToPatientID = 3004;
            _result.ExportedToVisitID = 123456;
            _result.ExportedToVisitDate = new DateTime(2017, 3, 12);
            _result.CreatedDate = new DateTime(2017, 3, 12);

            _repository.Setup(x => x.GetScreeningResultsForExport(It.IsAny<int>())).Returns(
                new List<ScreeningResultInfo>{ new ScreeningResultInfo
            {
                ID = 1,
                Birthday = new DateTime(1980, 3, 12),
                CreatedDate = new DateTime(2017, 3, 12),
                PatientName = _result.FullName
            }
            });


            var patient = new Patient
            {
                ID = 3004,
                EHR = "98765",
                FirstName = _result.FullName,
                LastName = _result.LastName,
                MiddleName = _result.MiddleName,
                DateOfBirth = new DateTime(1980, 3, 12)
            };

            _proxy.Setup(x => x.GetMatchedPatients(It.IsAny<ScreeningResult>(), It.IsAny<int>(), It.IsAny<int>())).Returns(
                new List<Patient>
                {
                    new Patient
                    {
                        ID = 3004,
                        EHR = "98765",
                        FirstName = _result.FullName,
                        LastName = _result.LastName,
                        MiddleName = _result.MiddleName,
                        DateOfBirth = new DateTime(1980, 3, 12)
                    }
                });

            _proxy.Setup(x => x.FindEhrPatientRecord(It.IsAny<ScreeningResult>())).Returns(
                 new Patient
                 {
                     ID = 3004,
                     EHR = "98765",
                     FirstName = _result.FullName,
                     LastName = _result.LastName,
                     MiddleName = _result.MiddleName,
                     DateOfBirth = new DateTime(1980, 3, 12)
                 }
               );

            _proxy.Setup(x => x.GetScheduledVisitsByPatient(It.IsAny<PatientSearch>(), It.IsAny<int>(), It.IsAny<int>())).Returns(
                new List<Visit>
                {
                    new Visit
                    {
                        ID = 123456,
                        Date = new DateTime(2017, 3, 12),
                        Location = new Location(),
                        ServiceCategory = "Office Visit"
                    }
                });

            _proxy.Setup(x => x.PreviewExportResult(It.IsAny<ScreeningResult>(), It.IsAny<PatientSearch>(), It.IsAny<int>())).Returns(
               new ExportTask
               {

               });



            _proxy.Setup(x => x.CommitExportResult(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ScreeningResult>(), It.IsAny<Screening>()))
                .Returns(new List<ExportResult>());



            _screeningDefServiceMock.Setup(x => x.Get()).Returns(ScreeningInfoMotherObject.GetFullScreening());
            _appConfigurationServiceMock.Setup(x => x.GetAllowedVisitCategories()).Returns(new[] { "+" });
            _appConfigurationServiceMock.Setup(x => x.GetRunIsTestModeFlag()).Returns(true);

        }

        private SmartExportService Sut()
        {
            return new SmartExportService(
                _repository.Object,
                _proxy.Object,
                _screeningResultServiceMock.Object,
                _userServiceProxy.Object,
                _logger.Object,
                _appConfigurationServiceMock.Object,
                _screeningDefServiceMock.Object
                );
        }


        [TestMethod]
        public void When_Simulation_On_Return_True()
        {
            var sut = Sut();

            var actual = sut.ExecuteExport(_result, true);

            actual.Should().BeTrue();
        }

        [TestMethod]
        public void When_Simulation_On_Skip_Export()
        {
            var sut = Sut();

            var actual = sut.ExecuteExport(_result, true);

            _proxy.Verify(x => x.CommitExportTask(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ExportTask>()), Times.Never(), "should not call when simulation is on");
        }

        [TestMethod]
        public void When_Simulation_Off_Do_Export()
        {
            var sut = Sut();

            sut.ExecuteExport(_result, false);

            _proxy.Verify(x => x.CommitExportTask(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ExportTask>()), Times.Once(), "should not call when simulation is on");
        }

        [TestMethod]
        public void When_Simulation_Off_Clean_AddressInScreening()
        {
            var sut = Sut();
            ScreeningResult actualParameter = null;
            _proxy.Setup(x => x.PreviewExportResult(It.IsAny<ScreeningResult>(), It.IsAny<PatientSearch>(), It.IsAny<int>()))
                .Callback<ScreeningResult, PatientSearch, int>((r, p, v) => { actualParameter = r; })
                .Returns(new ExportTask());

            //act
            sut.ExecuteExport(_result, false);

            //assert
            actualParameter.Should().NotBeNull("should be assigned in callback");
            actualParameter.StreetAddress.Should().BeEmpty();
            actualParameter.StateID.Should().BeEmpty();
            actualParameter.City.Should().BeEmpty();
            actualParameter.ZipCode.Should().BeEmpty();

        }

        [TestMethod]
        public void When_Simulation_Off_Clean_PhoneInScreening()
        {
            var sut = Sut();
            ScreeningResult actualParameter = null;
            _proxy.Setup(x => x.PreviewExportResult(It.IsAny<ScreeningResult>(), It.IsAny<PatientSearch>(), It.IsAny<int>()))
                .Callback<ScreeningResult, PatientSearch, int>((r, p, v) => { actualParameter = r; })
                .Returns(new ExportTask());

            //act
            sut.ExecuteExport(_result, false);

            //assert
            actualParameter.Should().NotBeNull("should be assigned in callback");
            actualParameter.Phone.Should().BeEmpty();
        }



        [TestMethod]
        public void When_Simulation_Off_And_Succeed_Updates_PatientInfo()
        {
            var sut = Sut();
            ScreeningResult actualParameter = null;

            var originalScreening = _result.ToPatient().Clone();
            var ehrPatient = new Patient
            {
                ID = 3004,
                EHR = "98765",
                FirstName = "JOHN",
                LastName = "DOW",
                MiddleName = "JR",
                DateOfBirth = new DateTime(1980, 12, 3)
            };

            _screeningResultServiceMock.Setup(x => x.UpdateExportInfo(
                It.IsAny<ScreeningResult>(),
                It.IsAny<ExportOperationStatus>(),
                It.IsAny<Patient>(),
                It.IsAny<Visit>(),
                It.IsAny<int>()))
                .Callback<ScreeningResult, ExportOperationStatus, Patient, Visit, int>((r, e, p, v, u) =>
                {
                    actualParameter = r;
                }
                );


            _proxy.Setup(x => x.FindEhrPatientRecord(It.IsAny<ScreeningResult>())).Returns(
                ehrPatient
              );

            //act
            sut.ExecuteExport(_result, false);

            //assert
            actualParameter.Should().NotBeNull("should be assigned in callback");
            actualParameter.LastName.Should().Be(ehrPatient.LastName);
            actualParameter.FirstName.Should().Be(ehrPatient.FirstName);
            actualParameter.MiddleName.Should().Be(ehrPatient.MiddleName);
            actualParameter.Birthday.Should().Be(ehrPatient.DateOfBirth);
        }

    }
}
