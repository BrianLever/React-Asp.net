using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk.Common;
using RPMS.Common.Models;
using RPMS.Common.Export;
using RPMS.Common.Export.Factories;
using ScreenDox.EHR.Common.Properties;
using FrontDesk.Common.Debugging;
using System.Diagnostics;
using Common.Logging;
using FrontDesk;

namespace RPMS.Common
{
    public class CareBridgeScreeningExportService : ScreeningExportService
    {
        private readonly ILog _logger = LogManager.GetLogger<CareBridgeScreeningExportService>();

        public CareBridgeScreeningExportService(
            IPatientRepository patientRepository, 
            IScreeningResultsRepository screeningResultsRepository)
            : base(patientRepository, screeningResultsRepository)
        {

        }


        


        public override ExportTask CreateExportTask(FrontDesk.ScreeningResult screeningResult, Patient patientRecord)
        {
            if (screeningResult == null)
            {
                throw new ArgumentNullException(nameof(screeningResult));
            }

            ExportTask exportTask = new ExportTask();


            //custom validation for patient fields over known field length restrictions in EHR
            if (screeningResult.IsContactInfoEligableForExport)
            {

                if (patientRecord != null)
                {
                    exportTask.PatientRecordModifications = CalculatePatientRecordModification(screeningResult, patientRecord);
                }

            }

            foreach(var section in screeningResult.SectionAnswers)
            {
                if(section.ScreeningSectionID == ScreeningSectionDescriptor.DrugOfChoice)
                {
                    continue;
                }
                exportTask.ScreeningSections.Add(new ExportScreeningSectionPreview
                {
                    ScoreLevelLabel = section.ScoreLevelLabel,
                    ScreeningSectionID = section.ScreeningSectionID
                }
                );
            }

            

            return exportTask;
        }

        #region IScreeningExportService Members


        public override List<ExportResult> CommitExportTask(int patientID, int visitID, ExportTask exportTask)
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




            return result;
        }

        public override List<ExportResult> ExportScreeningData(ScreeningResultRecord screeningResultRecord)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            List<ExportResult> result = new List<ExportResult>();

            if (screeningResultRecord != null)
            {
                result.Add(SafeActionExecute(
                                () =>
                                {
                                    try
                                    {
                                        _screeningResultsRepository.ExportScreeningData(screeningResultRecord);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.ErrorFormat("Failed to execute ExportScreeningData. Patient id: {0}", ex, screeningResultRecord.PatientID);

                                        throw new RpmsExportException(Resources.CommitExport_ScreeningDataActionName, new ExportFault(ExportFaultType.ScreeningDataExportFault, Resources.CommitExport_ScreeningDataExportError));
                                    }
                                },
                                Resources.CommitExport_ScreeningDataActionName,
                                ExportFaultType.ScreeningDataExportFault
                ));
            }
            
            return result;
        }

        #endregion

    }
}
