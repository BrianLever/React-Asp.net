using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPMS.Common.Models;

namespace RPMS.Common
{
    public interface IScreeningResultsRepository
    {
        void ExportHealthFactors(int patientID, int visitID, List<Models.HealthFactor> list);

        void ExportExams(int patientID, int visitID, List<Models.Exam> list);

        void ExportCrisisAlerts(int patientID, int visitID, List<Models.CrisisAlert> list);

        void ExportScreeningData(ScreeningResultRecord screeningResultRecord);
    }
}
