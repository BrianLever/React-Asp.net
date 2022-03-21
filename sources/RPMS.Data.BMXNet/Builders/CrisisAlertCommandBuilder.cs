using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPMS.Common.Models;

namespace RPMS.Data.BMXNet.Builders
{

    public sealed class CrisisAlertCommandBuilder : AbstractBmxCommandBuilder
    {
        internal enum CrisysAlertSystemFields
        {
            PATIENT,
            VISIT,
            DOCUMENT_TYPE,
            ENTRY_DATETIME,
            REFERENCE_DATE,
            AUTHOR,
            REPORT_TEXT,
            PARENTKEY,
            LINECOUNT
        }

        private static Dictionary<CrisysAlertSystemFields, string> _bmxCrisisAlertSystemFieldIds = new Dictionary<CrisysAlertSystemFields, string>
        {
            {CrisysAlertSystemFields.PATIENT, ".02"},
            {CrisysAlertSystemFields.VISIT, ".03"},
            {CrisysAlertSystemFields.DOCUMENT_TYPE, ".01"},
            {CrisysAlertSystemFields.ENTRY_DATETIME, "1201"},
            {CrisysAlertSystemFields.REFERENCE_DATE, "1301"},
            {CrisysAlertSystemFields.AUTHOR, "1202"},
            {CrisysAlertSystemFields.REPORT_TEXT, ".01"},
            {CrisysAlertSystemFields.LINECOUNT, "0.1"},
            {CrisysAlertSystemFields.PARENTKEY, ".0001"},

        };


        public string GetInsertTIUDocumentCommandText(int patientID, int visitID, Common.Models.CrisisAlert alert)
        {
            if (alert == null) throw new ArgumentNullException("alert");

            StringBuilder sql = new StringBuilder(@"UPDATE 8925^"); /// File #8925 (TIU DOCUMENT)
            /// TIU
            sql.AppendFormat("{2}{0}|{1}",
                _bmxCrisisAlertSystemFieldIds[CrisysAlertSystemFields.PATIENT],
                patientID,
                "^"
            );

            sql.AppendFormat("{2}{0}|{1}",
                _bmxCrisisAlertSystemFieldIds[CrisysAlertSystemFields.VISIT],
                visitID,
                fieldDelimeter
            );

            sql.AppendFormat("{2}{0}|{1}",
                _bmxCrisisAlertSystemFieldIds[CrisysAlertSystemFields.DOCUMENT_TYPE],
                alert.DocumentTypeID,
                fieldDelimeter
            );

            sql.AppendFormat("{2}{0}|{1}",
                _bmxCrisisAlertSystemFieldIds[CrisysAlertSystemFields.ENTRY_DATETIME],
                FormatDataTime(alert.EntryDate),
                fieldDelimeter
            );


            sql.AppendFormat("{2}{0}|{1}",
                _bmxCrisisAlertSystemFieldIds[CrisysAlertSystemFields.REFERENCE_DATE],
                FormatDataTime(alert.DateOfNote),
                fieldDelimeter
            );


            sql.AppendFormat("{2}{0}|{1}",
                _bmxCrisisAlertSystemFieldIds[CrisysAlertSystemFields.LINECOUNT],
                Math.Ceiling(alert.Details.Length / 74.0),
                fieldDelimeter
            );

            sql.AppendFormat("{2}{0}|{1}",
               _bmxCrisisAlertSystemFieldIds[CrisysAlertSystemFields.AUTHOR],
               alert.Author,
               fieldDelimeter
           );



            return sql.ToString();
        }



        public string GetLastInsertedAlertCommandText(int patientID, int visitID, CrisisAlert alert)
        {
            string sql = @"SELECT 
BMXIEN 'ID',
INTERNAL[PATIENT] 'PatientID',
INTERNAL[VISIT] 'VisitID',
INTERNAL[DOCUMENT_TYPE] 'DocumentTypeID',
ENTRY_DATE/TIME 'EntryDate',
AUTHOR/DICTATOR 'Author',
REPORT_TEXT 'ReportText',
REFERENCE_DATE 'DateOfNote'
FROM TIU_DOCUMENT
WHERE INTERNAL[PATIENT] = {0} AND
INTERNAL[VISIT] = {1} AND
INTERNAL[DOCUMENT_TYPE] = {2}
";

            return string.Format(sql, patientID, visitID, alert.DocumentTypeID);
        }


        /// <summary>
        /// Get command for insert report text
        /// </summary>
        internal string GetInsertReportTextCommand(int crisisAlertId, string reportText)
        {
            if (crisisAlertId == 0) throw new ArgumentException("crisisAlertId cannot be zero");

            StringBuilder sql = new StringBuilder(@"UPDATE 8925.02^"); /// File #8925.02 (REPORT TEXT)

            //sql.AppendFormat("{0}", crisisAlertId); //append row ID

            sql.AppendFormat("{2}{0}|{1}",
                _bmxCrisisAlertSystemFieldIds[CrisysAlertSystemFields.REPORT_TEXT],
               reportText.Substring(0, 40),
                "^"
            );

            sql.AppendFormat("{2}{0}|{1}",
                _bmxCrisisAlertSystemFieldIds[CrisysAlertSystemFields.PARENTKEY],
               crisisAlertId,
                fieldDelimeter
            );

            return sql.ToString();
        }

    }
}
