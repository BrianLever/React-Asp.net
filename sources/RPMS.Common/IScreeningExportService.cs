using System;
using RPMS.Common.Models;
using System.Collections;
using System.Collections.Generic;
namespace RPMS.Common
{
    public interface IScreeningExportService
    {
        ExportTask CreateExportTask(FrontDesk.ScreeningResult screeningResult, Patient patientRecord);

        List<ExportResult> CommitExportTask(int patientID, int visitID, ExportTask exportTask);
        List<ExportResult> ExportScreeningData(ScreeningResultRecord screeningResultRecord);
    }
}
