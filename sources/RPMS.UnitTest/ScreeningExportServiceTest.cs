using RPMS.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RPMS.Common.Models;
using System.Collections.Generic;
using Moq;
using ScreenDox.EHR.Common.Properties;

namespace RPMS.UnitTest
{


    /// <summary>
    ///This is a test class for ScreeningExportServiceTest and is intended
    ///to contain all ScreeningExportServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ScreeningExportServiceTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        private ExportTask CreateExportTask()
        {
            return new ExportTask
            {
                PatientRecordModifications = new List<PatientRecordModification>
                {
                    new PatientRecordModification{ CurrentValue = "1", UpdateWithValue= "2", Field = PatientRecordExportFields.Phone}
                },
                HealthFactors = new List<HealthFactor>
                {
                    new HealthFactor{ Code = "1", Factor = "Test"}
                },
                Exams = new List<Exam>
                {
                    new Exam{ Result = "PO", ExamName = "Test Exam"}
                },
                CrisisAlerts = new List<CrisisAlert>
                {
                    new CrisisAlert{ Details = "Parner Violence Inserted from unit test", Title= "CLINICAL WARNING", DocumentTypeID = 31},
                    new CrisisAlert{ Details = "Depression Inserted from unit test", Title= "CLINICAL WARNING", DocumentTypeID = 31}

                }
            };
        }


        [TestMethod()]
        public void CommitExportTask_all_failed()
        {
            Mock<IPatientRepository> patientRepository = new Mock<IPatientRepository>();
            Mock<IScreeningResultsRepository> screeningResultsRepository = new Mock<IScreeningResultsRepository>();

            patientRepository.Setup(x => x.UpdatePatientRecordFields(It.IsAny<IEnumerable<PatientRecordModification>>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(0)
                .Verifiable();

            screeningResultsRepository.Setup(x => x.ExportHealthFactors(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<HealthFactor>>()))
                .Throws(new RpmsExportException(Resources.CommitExport_InsertHealthFactorActionName, new ExportFault(ExportFaultType.HealthFactorFault)))
                .Verifiable();

            screeningResultsRepository.Setup(x => x.ExportExams(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<Exam>>()))
                .Throws(new InvalidOperationException("Error message"))
                .Verifiable();


            ScreeningExportService target = new ScreeningExportService(patientRepository.Object, screeningResultsRepository.Object);
            int patientID = 15859;
            int visitID = 12497;

            ExportTask exportTask = CreateExportTask();


            List<ExportResult> expected = new List<ExportResult>
            {
                new ExportResult{
                    ActionName = Resources.CommitExport_UpdatePatientActionName,
                    IsSuccessful = false, 
                    Fault = new ExportFault{ FaultType = ExportFaultType.PatientRecordUpdateFault, 
                        InfoMessage = ExportFaultMessages.PatientRecordUpdateFault,
                        ErrorMessage = Resources.CommitExport_PatientRecordUpdateNoAffectedRowsError}
                },
                new ExportResult{
                    ActionName = Resources.CommitExport_InsertHealthFactorActionName,
                    IsSuccessful = false,
                    Fault = new ExportFault{ FaultType = ExportFaultType.HealthFactorFault, 
                        InfoMessage = ExportFaultMessages.HealthFactorFault
                    }
                },
                new ExportResult{
                    ActionName = Resources.CommitExport_InsertExamActionName,
                    IsSuccessful = false,
                    Fault = new ExportFault{ FaultType = ExportFaultType.ExamFault, 
                        InfoMessage = ExportFaultMessages.ExamFault,
                        ErrorMessage = "Error message"
                    }
                },
            };


            List<ExportResult> actual;
            actual = target.CommitExportTask(patientID, visitID, exportTask);

            Assert.AreEqual(expected.Count, actual.Count, "Invalid record count");

            for (int i = 0; i < expected.Count; i++)
            {
                var e = expected[i];
                var a = actual[i];

                Assert.AreEqual(e.ActionName, a.ActionName, i + ". Action name failed");
                Assert.AreEqual(e.IsSuccessful, a.IsSuccessful, i + ". IsSuccessful failed");
                Assert.AreEqual(e.Fault.FaultType, a.Fault.FaultType, i + ". Fault.FaultType failed");
                Assert.AreEqual(e.Fault.InfoMessage, a.Fault.InfoMessage, i + ". Fault.InfoMessage failed");
                Assert.AreEqual(e.Fault.ErrorMessage, a.Fault.ErrorMessage, i + ". Fault.ErrorMessage failed");
            }

        }


        [TestMethod()]
        public void CommitExportTask_all_succeed()
        {
            Mock<IPatientRepository> patientRepository = new Mock<IPatientRepository>();
            Mock<IScreeningResultsRepository> screeningResultsRepository = new Mock<IScreeningResultsRepository>();

            patientRepository.Setup(x => x.UpdatePatientRecordFields(It.IsAny<IEnumerable<PatientRecordModification>>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(1)
                .Verifiable();

            screeningResultsRepository.Setup(x => x.ExportHealthFactors(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<HealthFactor>>()))
                .Verifiable();

            screeningResultsRepository.Setup(x => x.ExportExams(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<Exam>>()))
                .Verifiable();


            ScreeningExportService target = new ScreeningExportService(patientRepository.Object, screeningResultsRepository.Object);
            int patientID = 15859;
            int visitID = 12497;

            ExportTask exportTask = CreateExportTask();

            List<ExportResult> expected = new List<ExportResult>
            {
                new ExportResult{
                    ActionName = Resources.CommitExport_UpdatePatientActionName,
                    IsSuccessful = true, 
                },
                new ExportResult{
                    ActionName = Resources.CommitExport_InsertHealthFactorActionName,
                    IsSuccessful = true,
                },
                new ExportResult{
                    ActionName = Resources.CommitExport_InsertExamActionName,
                    IsSuccessful = true,
                },
            };


            List<ExportResult> actual;
            actual = target.CommitExportTask(patientID, visitID, exportTask);

            Assert.AreEqual(expected.Count, actual.Count, "Invalid record count");

            for (int i = 0; i < expected.Count; i++)
            {
                var e = expected[i];
                var a = actual[i];

                Assert.AreEqual(e.ActionName, a.ActionName, i + ". Action name failed");
                Assert.AreEqual(e.IsSuccessful, a.IsSuccessful, i + ". IsSuccessful failed");
                Assert.AreEqual(e.Fault, a.Fault, i + ". Fault.FaultType failed");
            }

        }

        [TestMethod()]
        public void CommitExportTask_mixed()
        {
            Mock<IPatientRepository> patientRepository = new Mock<IPatientRepository>();
            Mock<IScreeningResultsRepository> screeningResultsRepository = new Mock<IScreeningResultsRepository>();

            patientRepository.Setup(x => x.UpdatePatientRecordFields(It.IsAny<IEnumerable<PatientRecordModification>>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(0)
                .Verifiable();

            screeningResultsRepository.Setup(x => x.ExportHealthFactors(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<HealthFactor>>()))
                .Throws(new InvalidOperationException("Error message"))
                .Verifiable();

            screeningResultsRepository.Setup(x => x.ExportExams(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<Exam>>()))
                .Verifiable();


            ScreeningExportService target = new ScreeningExportService(patientRepository.Object, screeningResultsRepository.Object);
            int patientID = 15859;
            int visitID = 12497;

            ExportTask exportTask = CreateExportTask();


            List<ExportResult> expected = new List<ExportResult>
            {
                new ExportResult{
                    ActionName = Resources.CommitExport_UpdatePatientActionName,
                    IsSuccessful = false, 
                    Fault = new ExportFault{ FaultType = ExportFaultType.PatientRecordUpdateFault, 
                        InfoMessage = ExportFaultMessages.PatientRecordUpdateFault,
                        ErrorMessage = Resources.CommitExport_PatientRecordUpdateNoAffectedRowsError}
                },
                new ExportResult{
                    ActionName = Resources.CommitExport_InsertHealthFactorActionName,
                    IsSuccessful = false,
                    Fault = new ExportFault{ FaultType = ExportFaultType.HealthFactorFault, 
                        InfoMessage = ExportFaultMessages.HealthFactorFault,
                        ErrorMessage = "Error message"
                    }
                },
                new ExportResult{
                    ActionName = Resources.CommitExport_InsertExamActionName,
                    IsSuccessful = true,
                },
            };


            List<ExportResult> actual;
            actual = target.CommitExportTask(patientID, visitID, exportTask);

            Assert.AreEqual(expected.Count, actual.Count, "Invalid record count");

            for (int i = 0; i < expected.Count; i++)
            {
                var e = expected[i];
                var a = actual[i];

                Assert.AreEqual(e.ActionName, a.ActionName, i + ". Action name failed");
                Assert.AreEqual(e.IsSuccessful, a.IsSuccessful, i + ". IsSuccessful failed");
                if (e.Fault != null)
                {

                    Assert.AreEqual(e.Fault.FaultType, a.Fault.FaultType, i + ". Fault.FaultType failed");
                    Assert.AreEqual(e.Fault.InfoMessage, a.Fault.InfoMessage, i + ". Fault.InfoMessage failed");
                    Assert.AreEqual(e.Fault.ErrorMessage, a.Fault.ErrorMessage, i + ". Fault.ErrorMessage failed");
                }
                else
                {
                    Assert.IsNull(a.Fault, i + ". action fault is not null");
                }
            }

        }


    }
}
