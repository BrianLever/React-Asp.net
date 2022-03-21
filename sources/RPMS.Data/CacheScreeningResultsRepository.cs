using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPMS.Common;
using System.Diagnostics;
using RPMS.Data.Descriptors;
using System.Data;

namespace RPMS.Data
{
    public class CacheScreeningResultsRepository : CacheDatabase, IScreeningResultsRepository
    {
        #region Health factors

        public void ExportHealthFactors(int patientID, int visitID, List<Common.Models.HealthFactor> healthFactors)
        {
            Connect();
            var transaction = ConnectionObject.BeginTransaction();
            try
            {
                CommandObject.Parameters.Clear();
                CommandObject.Transaction = transaction;

                foreach (var hf in healthFactors)
                {
                    CreateHealthFactor(patientID, visitID, hf);
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally { Disconnect(); }
        }

        private void CreateHealthFactor(int patientID, int visitID, Common.Models.HealthFactor healthFactor)
        {
            string sqlInsert = string.Format(@"
INSERT INTO {0}.V_HEALTH_FACTORS (
    PATIENT_NAME, 
    VISIT,
    HEALTH_FACTOR,
    COMMENTS
) VALUES(
    ?,
    ?,
    ?,
    ?
)
", DbDescriptor.DatabaseSchemaName);

            if (CommandObject.Parameters.Count == 0)
            {
                //init parameters
                AddParameter("PatientID", System.Data.DbType.Int32).Value = patientID;
                AddParameter("VisitID", System.Data.DbType.Int32).Value = visitID;

                //updated each loop
                AddParameter("Factor", System.Data.DbType.Int32);
                AddParameter("Comments", System.Data.DbType.String, 245);
            }

            (CommandObject.Parameters["Factor"] as IDataParameter).Value = healthFactor.FactorID;
            (CommandObject.Parameters["Comments"] as IDataParameter).Value = healthFactor.Comment;

            RunNonSelectQuery(sqlInsert);
        }

        #endregion

        #region Exams
        public void ExportExams(int patientID, int visitID, List<Common.Models.Exam> exams)
        {
            Connect();
            var transaction = ConnectionObject.BeginTransaction();
            try
            {
                CommandObject.Parameters.Clear();
                CommandObject.Transaction = transaction;

                foreach (var exam in exams)
                {
                    CreateExam(patientID, visitID, exam);
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally { Disconnect(); }
        }

        private void CreateExam(int patientID, int visitID, Common.Models.Exam exam)
        {
            string sqlInsert = string.Format(@"
INSERT INTO {0}.V_EXAM (
    PATIENT_NAME, 
    VISIT,
    EXAM,
    RESULT,
    COMMENTS
) VALUES(
    ?,
    ?,
    ?,
    ?,
    ?
)
", DbDescriptor.DatabaseSchemaName);

            if (CommandObject.Parameters.Count == 0)
            {
                //init parameters
                AddParameter("PatientID", System.Data.DbType.Int32).Value = patientID;
                AddParameter("VisitID", System.Data.DbType.Int32).Value = visitID;

                //updated each loop
                AddParameter("Exam", System.Data.DbType.Int32);
                AddParameter("Result", System.Data.DbType.String, 245);
                AddParameter("Comments", System.Data.DbType.String, 245);
            }

            (CommandObject.Parameters["Exam"] as IDataParameter).Value = exam.ExamID;
            (CommandObject.Parameters["Result"] as IDataParameter).Value = exam.Result;
            (CommandObject.Parameters["Comments"] as IDataParameter).Value = exam.Comment;

            RunNonSelectQuery(sqlInsert);
        }

        #endregion


        #region Crisis Alerts


        public void ExportCrisisAlerts(int patientID, int visitID, List<Common.Models.CrisisAlert> list)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
