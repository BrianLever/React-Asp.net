using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using IndianHealthService.BMXNet;
using RPMS.Common;
using RPMS.Common.Models;
using RPMS.Common.Security;
using RPMS.Data.BMXNet.Builders;
using RPMS.Data.BMXNet.Framework;

namespace RPMS.Data.BMXNet
{
    public class BmxScreeningResultsRepository : BMXNetDatabase, IScreeningResultsRepository
    {
        private const char fieldDelimeter = (char) 30;

        private static readonly Dictionary<ExamSystemFields, string> _bmxExamSystemFieldIds =
            new Dictionary<ExamSystemFields, string>
            {
                {ExamSystemFields.PATIENT_NAME, ".02"},
                {ExamSystemFields.VISIT, ".03"},
                {ExamSystemFields.EXAM, ".01"},
                {ExamSystemFields.RESULT, ".04"},
                {ExamSystemFields.COMMENTS, "81101"},
            };

        #region Health factors

        private static readonly Dictionary<HealthFactorsSystemFields, string> _bmxHealthRecordSystemFieldIds =
            new Dictionary<HealthFactorsSystemFields, string>
            {
                {HealthFactorsSystemFields.PATIENT_NAME, ".02"},
                {HealthFactorsSystemFields.VISIT, ".03"},
                {HealthFactorsSystemFields.HEALTH_FACTOR, ".01"},
                {HealthFactorsSystemFields.COMMENTS, "81101"},
            };

        public virtual void ExportHealthFactors(int patientID, int visitID, List<HealthFactor> healthFactors)
        {
            Connect();
            try
            {
                foreach (HealthFactor hf in healthFactors)
                {
                    CreateHealthFactor(patientID, visitID, hf);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
        }

        private int CreateHealthFactor(int patientID, int visitID, HealthFactor healthFactor)
        {
            int affectedRows = 0;
            var sql = new StringBuilder(@"UPDATE 9000010.23^"); /// File #9000010.23 (V HEALTH FACTORS)

            //sql.Append(""); //append empty row ID - INSERT

            sql.AppendFormat("{2}{0}|{1}", _bmxHealthRecordSystemFieldIds[HealthFactorsSystemFields.PATIENT_NAME],
                patientID, "^");

            sql.AppendFormat("{2}{0}|{1}", _bmxHealthRecordSystemFieldIds[HealthFactorsSystemFields.VISIT], visitID,
                fieldDelimeter);

            sql.AppendFormat("{2}{0}|{1}", _bmxHealthRecordSystemFieldIds[HealthFactorsSystemFields.HEALTH_FACTOR],
                healthFactor.FactorID, fieldDelimeter);

            sql.AppendFormat("{2}{0}|{1}", _bmxHealthRecordSystemFieldIds[HealthFactorsSystemFields.COMMENTS],
                healthFactor.Comment, fieldDelimeter);

            string sqlStr = sql.ToString();
            Debug.WriteLine("Health factor insert SQL: " + sqlStr);

            affectedRows = RunNonSelectQuery(sqlStr);

            return affectedRows;
        }

        internal enum HealthFactorsSystemFields
        {
            PATIENT_NAME,
            VISIT,
            HEALTH_FACTOR,
            COMMENTS
        }

        #endregion

        public BmxScreeningResultsRepository()
        {
        }

        public BmxScreeningResultsRepository(IRpmsCredentialsService credentialsService) : base(credentialsService)
        {
        }

        public virtual void ExportExams(int patientID, int visitID, List<Exam> exams)
        {
            Connect();
            try
            {
                foreach (Exam exam in exams)
                {
                    CreateExam(patientID, visitID, exam);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
        }


        private int CreateExam(int patientID, int visitID, Exam exam)
        {
            int affectedRows = 0;
            var sql = new StringBuilder(@"UPDATE 9000010.13^"); /// File #9000010.13 (V EXAM)

            //sql.Append(""); //append empty row ID - INSERT

            sql.AppendFormat("{2}{0}|{1}", _bmxExamSystemFieldIds[ExamSystemFields.PATIENT_NAME], patientID, "^");

            sql.AppendFormat("{2}{0}|{1}", _bmxExamSystemFieldIds[ExamSystemFields.VISIT], visitID, fieldDelimeter);

            sql.AppendFormat("{2}{0}|{1}", _bmxExamSystemFieldIds[ExamSystemFields.EXAM], exam.ExamID, fieldDelimeter);

            sql.AppendFormat("{2}{0}|{1}", _bmxExamSystemFieldIds[ExamSystemFields.RESULT], exam.Result, fieldDelimeter);

            sql.AppendFormat("{2}{0}|{1}", _bmxExamSystemFieldIds[ExamSystemFields.COMMENTS], exam.Comment,
                fieldDelimeter);

            string sqlStr = sql.ToString();
            Debug.WriteLine("Exam Insert SQL: " + sqlStr);

            affectedRows = RunNonSelectQuery(sqlStr);

            return affectedRows;
        }

        #region Crisis Alerts

        public virtual void ExportCrisisAlerts(int patientID, int visitID, List<CrisisAlert> crisisAlerts)
        {
            var builder = new CrisisAlertCommandBuilder();
            Connect();
            try
            {
                foreach (CrisisAlert alert in crisisAlerts)
                {
                    CreateCrisisAlert(patientID, visitID, alert, builder);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
        }

        private int CreateCrisisAlert(int patientID, int visitID, CrisisAlert alert, CrisisAlertCommandBuilder builder)
        {
            if (alert == null) throw new ArgumentNullException("alert");

            int affectedRows = 0;
            int id = 0;

            string sqlStr = builder.GetInsertTIUDocumentCommandText(patientID, visitID, alert);

            Debug.WriteLine("TIU DOCUMENT Insert SQL: " + sqlStr);
            affectedRows = RunNonSelectQuery(sqlStr);

            CrisisAlert insertedAlert = GetJustInsertedRecord(patientID, visitID, alert, builder);

            if (insertedAlert != null)
            {
                id = insertedAlert.ID;
            }

            if (id > 0)
            {
                affectedRows += InsertReportComment(id, alert.Details, builder);
            }
            else
            {
                throw new BMXNetException("Failed to get ID of just inserted crisis alert.");
            }

            //Debug.Assert(affectedRows == 2, "Number of inserted rows is not equal to 2");

            return affectedRows;
        }


        private CrisisAlert CreateCrisisAlertFromReader(IDataReader reader)
        {
            return new CrisisAlert
            {
                ID = Convert.ToInt32(reader["ID"]),
                PatientID = Convert.ToInt32(reader["PatientID"]),
                VisitID = Convert.ToInt32(reader["VisitID"]),
                DocumentTypeID = Convert.ToInt32(reader["DocumentTypeID"]),
                EntryDate = Convert.ToDateTime(reader["EntryDate"]),
                DateOfNote = Convert.ToDateTime(reader["DateOfNote"]),
                Details = Convert.ToString(reader["ReportText"]),
                Author = Convert.ToString(reader["Author"]),
            };
        }

        public virtual CrisisAlert GetJustInsertedRecord(int patientID, int visitID, CrisisAlert alert)
        {
            if (alert == null) throw new ArgumentNullException("alert");
            var builder = new CrisisAlertCommandBuilder();

            return GetJustInsertedRecord(patientID, visitID, alert, builder);
        }


        private CrisisAlert GetJustInsertedRecord(int patientID, int visitID, CrisisAlert alert,
            CrisisAlertCommandBuilder builder)
        {
            int lastRecordId = 0;
            var resultList = new List<CrisisAlert>();

            Connect();
            try
            {
                string sqlStr = builder.GetLastInsertedAlertCommandText(patientID, visitID, alert);

                Debug.WriteLine("TIU DOCUMENT GET SQL: " + sqlStr);
                using (IDataReader reader = RunSelectQuery(sqlStr))
                {
                    while (reader.Read())
                    {
                        resultList.Add(CreateCrisisAlertFromReader(reader));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Disconnect();
            }

            if (resultList.Count > 0)
            {
                try
                {
                    lastRecordId =
                        resultList.Where(x => x.DateOfNote == alert.DateOfNote && x.EntryDate == alert.EntryDate)
                            .Max(x => x.ID);
                }
                catch (InvalidOperationException) //where returns no elements
                {
                    lastRecordId = 0;
                }
            }

            return resultList.FirstOrDefault(x => x.ID == lastRecordId);
            ;
        }

        private int InsertReportComment(int crisisAlertId, string reportText, CrisisAlertCommandBuilder builder)
        {
            int affectedRows = 0;

            string sqlStr = builder.GetInsertReportTextCommand(crisisAlertId, reportText);

            Debug.WriteLine("REPORT TEXT Insert SQL: " + sqlStr);
            affectedRows = RunNonSelectQuery(sqlStr);
            return affectedRows;
        }

        #endregion

        internal enum ExamSystemFields
        {
            PATIENT_NAME,
            VISIT,
            EXAM,
            RESULT,
            COMMENTS
        }

        public void ExportScreeningData(ScreeningResultRecord screeningResultRecord)
        {
            throw new NotSupportedException();
        }
    }
}