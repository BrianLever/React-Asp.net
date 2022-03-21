using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPMS.Common;
using RPMS.Common.Models;

namespace RPMS.Data.FakeObjects
{
    public class FakeScreeningResultsRepository : IScreeningResultsRepository
    {
        #region IScreeningResultsRepository Members

        public void ExportHealthFactors(int patientID, int visitID, List<Common.Models.HealthFactor> list)
        {
            
        }

        public void ExportExams(int patientID, int visitID, List<Common.Models.Exam> list)
        {
            
        }

        #endregion

        #region IScreeningResultsRepository Members


        public void ExportCrisisAlerts(int patientID, int visitID, List<Common.Models.CrisisAlert> list)
        {
            throw new NotImplementedException();
        }

        public void ExportScreeningData(ScreeningResultRecord screeningResultRecord)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
