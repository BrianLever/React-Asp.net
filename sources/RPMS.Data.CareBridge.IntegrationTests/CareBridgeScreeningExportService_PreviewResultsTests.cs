using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RPMS.Common;
using RPMS.Tests.MotherObjects;
using FluentAssertions;

namespace RPMS.Data.CareBridge.IntegrationTests
{
    [TestCategory("CareBridge_Integration")]
    [TestCategory("CareBridge_NextGen")]
    [TestClass]
    public class CareBridgeScreeningExportService_PreviewResultsTests
    {
        private readonly Mock<IPatientRepository> _patientRepositoryMock = new Mock<IPatientRepository>();
        private readonly Mock<IScreeningResultsRepository> _screeningResultsRepository = new Mock<IScreeningResultsRepository>();

        public CareBridgeScreeningExportService_PreviewResultsTests()
        {

        }


        protected CareBridgeScreeningExportService Sut()
        {
            return new CareBridgeScreeningExportService(_patientRepositoryMock.Object, _screeningResultsRepository.Object);
        }



        [TestMethod]
        public void WhenComparingAddress_IgnoresCase()
        {
            var screeningResult = ScreeningResultMotherObject.GetMaryTest();
            var patient = PatientMotherObject.GetMaryTest();

            var result = Sut().CreateExportTask(screeningResult, patient);

            result.PatientRecordModifications.Count.Should().Be(0);
            result.Errors.Should().BeEmpty("No errors");
        }
    }
}
