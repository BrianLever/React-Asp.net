using FrontDesk.Common;
using FrontDesk.Common.Debugging;

using RPMS.Common.Export.Factories;
using RPMS.Common.Models;

using ScreenDox.EHR.Common.Properties;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RPMS.Common
{
    public class ScreeningExportService : IScreeningExportService
    {
        protected IScreeningResultProcessorFactory _factory;
        protected IPatientRepository _patientRepository;
        protected IScreeningResultsRepository _screeningResultsRepository;

        public ScreeningExportService(IPatientRepository patientRepository, IScreeningResultsRepository screeningResultsRepository)
        {
            _factory = new ScreeningResultProcessorFactory();
            _patientRepository = patientRepository;

            _screeningResultsRepository = screeningResultsRepository;
        }


        public virtual ExportTask CreateExportTask(FrontDesk.ScreeningResult screeningResult, Patient patientRecord)
        {
            ExportTask exportTask = new ExportTask();

            int maxCityLength = 15;
            //custom validation for patient fields over known field length restrictions in EHR
            if (screeningResult.IsContactInfoEligableForExport)
            {
                if (!string.IsNullOrWhiteSpace(screeningResult.City) && screeningResult.City.Length > maxCityLength)
                {
                    exportTask.Errors.Add(string.Format(Resources.PatientExceededStringFieldLengthTemplate, "City", maxCityLength, screeningResult.City, screeningResult.City.Length));
                }
                else if (string.IsNullOrWhiteSpace(screeningResult.StateName))
                {
                    exportTask.Errors.Add(Resources.StateNameIsEmpty);
                }
                else
                {

                    if (patientRecord != null)
                    {
                        exportTask.PatientRecordModifications = CalculatePatientRecordModification(screeningResult, patientRecord);
                    }
                }
            }
            exportTask.HealthFactors = CalculateHealthFactors(screeningResult.SectionAnswers);
            exportTask.Exams = CalculateExams(screeningResult.SectionAnswers);


            return exportTask;
        }



        /// <summary>
        /// Calculate health factors from screening results
        /// </summary>
        /// <param name="sectionResults">screening answers</param>
        /// <returns>Health factor list</returns>
        private List<HealthFactor> CalculateHealthFactors(List<FrontDesk.ScreeningSectionResult> sectionResults)
        {
            List<HealthFactor> factors = new List<HealthFactor>();

            IHealthFactorCalculatorFactory factory = _factory.CreateHealthFactorCalculatorFactory();

            foreach (var calculator in factory.GetHealthFactorCalculators())
            {
                IList<HealthFactor> calculatedFactors = calculator.Calculate(sectionResults);

                factors.AddRange(calculatedFactors);
            }
            return factors;
        }


        /// <summary>
        /// Calculate exams from screening results
        /// </summary>
        /// <param name="sectionResults">screening answers</param>
        /// <returns>Exam list</returns>
        private List<Exam> CalculateExams(List<FrontDesk.ScreeningSectionResult> sectionResults)
        {
            List<Exam> exams = new List<Exam>();

            IExamCalculatorFactory factory = _factory.CreateExamCalculatorFactory();

            foreach (var calc in factory.GetExamCalculators())
            {
                IList<Exam> calculatedExams = calc.Calculate(sectionResults);
                exams.AddRange(calculatedExams);
            }
            return exams;
        }

        /// <summary>
        /// Calculate crisis alerts from screening results
        /// </summary>
        /// <param name="sectionResults">screening answers</param>
        /// <returns>EHR crisis alert list</returns>
        private List<CrisisAlert> CalculateCrisisAlerts(List<FrontDesk.ScreeningSectionResult> sectionResults)
        {
            List<CrisisAlert> alerts = new List<CrisisAlert>();

            ICrisisAlertCalculatorFactory factory = _factory.CreateCrisisAlertCalculatorFactory();

            foreach (var calculator in factory.GetCrisisAlertCalculators())
            {
                IList<CrisisAlert> calculatedAlerts = calculator.Calculate(sectionResults);
                alerts.AddRange(calculatedAlerts);
            }
            return alerts;
        }


        protected virtual List<PatientRecordModification> CalculatePatientRecordModification(FrontDesk.ScreeningResult screeningResult, Patient patientRecord)
        {
            List<PatientRecordModification> modifications = new List<PatientRecordModification>();

            if (screeningResult.IsContactInfoEligableForExport)
            {
                //City
                if (!string.Equals(screeningResult.City, patientRecord.City, StringComparison.OrdinalIgnoreCase))
                {
                    modifications.Add(new PatientRecordModification { Field = PatientRecordExportFields.City, CurrentValue = patientRecord.City, UpdateWithValue = screeningResult.City });
                }
                ///phone
                if (!string.Equals(screeningResult.Phone.AsRawPhoneNumber(), patientRecord.PhoneHome.AsRawPhoneNumber(), StringComparison.OrdinalIgnoreCase)
                        && !string.Equals(screeningResult.Phone.AsRawPhoneNumber(), patientRecord.PhoneOffice.AsRawPhoneNumber(), StringComparison.OrdinalIgnoreCase)
                        )
                {
                    modifications.Add(new PatientRecordModification { Field = PatientRecordExportFields.Phone, CurrentValue = patientRecord.PhoneHome, UpdateWithValue = screeningResult.Phone });
                }

                ///zip code
                if (!string.Equals(screeningResult.ZipCode, patientRecord.ZipCode, StringComparison.OrdinalIgnoreCase))
                {
                    modifications.Add(new PatientRecordModification { Field = PatientRecordExportFields.ZipCode, CurrentValue = patientRecord.ZipCode, UpdateWithValue = screeningResult.ZipCode });
                }

                ///state id
                if (!string.Equals(screeningResult.StateID, patientRecord.StateID, StringComparison.OrdinalIgnoreCase))
                {
                    modifications.Add(new PatientRecordModification { Field = PatientRecordExportFields.StateID, CurrentValue = patientRecord.StateID, UpdateWithValue = screeningResult.StateName });
                }


                ///address 
                if (!string.Equals(screeningResult.StreetAddress, patientRecord.StreetAddress, StringComparison.OrdinalIgnoreCase))
                {

                    List<string> screeningAddressLines = screeningResult.StreetAddress.SplitTo3AddressLines(35, 30, 30).ToList();
                    while (screeningAddressLines.Count < 3)
                    {
                        screeningAddressLines.Add(string.Empty);
                    }


                    if (!string.Equals(screeningAddressLines[0], patientRecord.StreetAddressLine1, StringComparison.OrdinalIgnoreCase))
                    {
                        modifications.Add(new PatientRecordModification { Field = PatientRecordExportFields.AddressLine1, CurrentValue = patientRecord.StreetAddressLine1, UpdateWithValue = screeningAddressLines[0] });
                    }
                    if (!string.Equals(screeningAddressLines[1], patientRecord.StreetAddressLine2, StringComparison.OrdinalIgnoreCase))
                    {
                        modifications.Add(new PatientRecordModification { Field = PatientRecordExportFields.AddressLine2, CurrentValue = patientRecord.StreetAddressLine2, UpdateWithValue = screeningAddressLines[1] });
                    }
                    if (!string.Equals(screeningAddressLines[2], patientRecord.StreetAddressLine3, StringComparison.OrdinalIgnoreCase))
                    {
                        modifications.Add(new PatientRecordModification { Field = PatientRecordExportFields.AddressLine3, CurrentValue = patientRecord.StreetAddressLine3, UpdateWithValue = screeningAddressLines[2] });
                    }
                }

            }
            return modifications;

        }





        #region IScreeningExportService Members


        public virtual List<ExportResult> CommitExportTask(int patientID, int visitID, ExportTask exportTask)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            List<ExportResult> result = new List<ExportResult>();
            if (exportTask == null)
            {
                result.Add(new ExportResult
                {
                    ActionName = Resources.CommitExport_DataValidationActionName,
                    IsSuccessful = false,
                    Fault = new ExportFault(ExportFaultType.ExportTaskIsEmpty)
                });
            }
            if (exportTask.PatientRecordModifications != null && exportTask.PatientRecordModifications.Count > 0)
            {
                result.Add(SafeActionExecute(
                                () =>
                                {
                                    int affectedRows = _patientRepository.UpdatePatientRecordFields(exportTask.PatientRecordModifications, patientID, visitID);
                                    if (affectedRows == 0)
                                    {
                                        throw new RpmsExportException(Resources.CommitExport_UpdatePatientActionName, new ExportFault(ExportFaultType.PatientRecordUpdateFault, Resources.CommitExport_PatientRecordUpdateNoAffectedRowsError));
                                    }
                                },
                                Resources.CommitExport_UpdatePatientActionName,
                                ExportFaultType.PatientRecordUpdateFault
                ));
            }

            if (exportTask.HealthFactors != null && exportTask.HealthFactors.Count > 0)
            {
                result.Add(SafeActionExecute(
                             () => _screeningResultsRepository.ExportHealthFactors(patientID, visitID, exportTask.HealthFactors),
                             Resources.CommitExport_InsertHealthFactorActionName,
                             ExportFaultType.HealthFactorFault
             ));
            }

            if (exportTask.Exams != null && exportTask.Exams.Count > 0)
            {
                result.Add(SafeActionExecute(
                        () => _screeningResultsRepository.ExportExams(patientID, visitID, exportTask.Exams),
                        Resources.CommitExport_InsertExamActionName,
                        ExportFaultType.ExamFault
                ));

            }


            return result;
        }

        #endregion

        #region Save export execute

        /// <summary>
        /// Execute action and wrap action execution result into ExportResult class
        /// </summary>
        /// <param name="action">Action to execute</param>
        /// <param name="actionName">Action name for ExportResult</param>
        /// <param name="faultType">Fault type for ExportResult</param>
        /// <returns>ExportResult object</returns>
        protected ExportResult SafeActionExecute(Action action, string actionName, ExportFaultType faultType)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                //execute action
                action();

                DebugLogger.WriteTraceMessage(string.Format("Executed action {0}. Duration: {1} seconds. Succeed.", actionName, stopwatch.Elapsed.TotalSeconds));

                stopwatch.Stop();
                return new ExportResult
                {
                    ActionName = actionName,
                    IsSuccessful = true,
                };
            }
            catch (RpmsExportException ex)
            {
                DebugLogger.WriteTraceMessage(string.Format("Executed action {0}. Duration: {1} seconds. Failed.", actionName, stopwatch.Elapsed.TotalSeconds));
                stopwatch.Stop();
                return new ExportResult
                {
                    ActionName = ex.ActionName,
                    IsSuccessful = false,
                    Fault = ex.Fault
                };
            }
            catch (Exception ex)
            {
                DebugLogger.TraceException(ex);

                DebugLogger.WriteTraceMessage(string.Format("Executed action {0}. Duration: {1} seconds. Failed.", actionName, stopwatch.Elapsed.TotalSeconds));
                stopwatch.Stop();

                return new ExportResult
                {
                    ActionName = actionName,
                    IsSuccessful = false,
                    Fault = new ExportFault(faultType, ex.Message)
                };

            }
        }

        public virtual List<ExportResult> ExportScreeningData(ScreeningResultRecord screeningResultRecord)
        {
            return new List<ExportResult>();
        }
        #endregion
    }
}
