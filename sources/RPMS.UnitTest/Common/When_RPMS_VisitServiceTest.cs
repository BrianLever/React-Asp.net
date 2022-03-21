using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RPMS.Common;
using RPMS.Common.GlobalConfiguration;
using RPMS.Common.Models;

namespace RPMS.UnitTest.Common
{
    [TestClass]
    public class When_RPMS_VisitServiceTest
    {
        private const string PatientNameWithApostrophe = "MC'DONNALD, DUCK";
        private const string PatientName = "DOE, JOHN JR.";



        private readonly Mock<IVisitRepository> _visitRepositoryMock = new Mock<IVisitRepository>();
        private readonly Mock<IGlobalSettingsService> _globalSettingsMock = new Mock<IGlobalSettingsService>();

        private VisitService Sut()
        {
            _globalSettingsMock.SetupGet(x => x.IsRpmsMode).Returns(true);
            _globalSettingsMock.SetupGet(x => x.IsNextGenMode).Returns(false);

            return new VisitService(_visitRepositoryMock.Object, _globalSettingsMock.Object);
        }

        private PatientSearch GetPatientSearch(int id)
        {
            return new PatientSearch
            {
                ID = id,
                LastName = "DOE",
                FirstName = "JOHN",
                MiddleName = "JR."
            };
        }

        [TestCategory("RpmsMode")]
        [TestCategory("EHR_Interface")]
        [TestMethod]
        public void WhenPatientNameWithoutApostrophe_SearchByPatientName()
        {
            var patient = GetPatientSearch(1001);

            Sut().GetVisitsByPatient(patient, 0, 50);
            _visitRepositoryMock.Verify(x => x.GetVisitsByPatient(patient, 0, 50), Times.Once());
        }

        [TestCategory("RpmsMode")]
        [TestCategory("EHR_Integration")]
        [TestMethod]
        public void WhenPatientNameHasApostrophe_SearchByPatientId()
        {
            var patient = GetPatientSearch(1001);
            patient.LastName = PatientNameWithApostrophe;

            Sut().GetVisitsByPatient(patient, 0, 50);
            _visitRepositoryMock.Verify(x => x.GetVisitsByPatient(1001, 0, 50), Times.Once());
        }

        
        [TestCategory("RpmsMode")]
        [TestCategory("EHR_Interface")]
        [TestMethod]
        public void WhenPatientNameWithoutApostrophe_CountByPatientName()
        {
            var patient = GetPatientSearch(1001);

            Sut().GetVisitsByPatientCount(patient);
            _visitRepositoryMock.Verify(x => x.GetVisitsByPatientCount(patient), Times.Once());
        }

        [TestCategory("Rpms")]
        [TestMethod]
        public void WhenPatientNameHasApostrophe_CountByPatientId()
        {
            var patient = GetPatientSearch(1001);
            patient.LastName = PatientNameWithApostrophe;

            Sut().GetVisitsByPatientCount(patient);
            _visitRepositoryMock.Verify(x => x.GetVisitsByPatientCount(1001), Times.Once());
        }
    }
}
