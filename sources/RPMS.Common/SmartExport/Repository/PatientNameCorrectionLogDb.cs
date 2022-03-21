using FrontDesk.Common.Data;
using FrontDesk.Common.Extensions;

using ScreenDox.EHR.Common.Models.PatientValidation;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace ScreenDox.EHR.Common.SmartExport.Repository
{
    public interface IPatientNameCorrectionLogRepository
    {
        void Add(PatientNameCorrectionLogItem logItem);
        List<PatientNameCorrectionLogItem> Get(DateTimeOffset? startDate, DateTimeOffset? endDate, string nameFilter, string orderBy, int startRowIndex, int maximumRows);
        int GetCount(DateTimeOffset? startDate, DateTimeOffset? endDate, string nameFilter);
    }

    public class PatientNameCorrectionLogDb : DBDatabase, IPatientNameCorrectionLogRepository
    {
        public PatientNameCorrectionLogDb() : base()
        {
            this.ConnectionString = ConfigurationManager.ConnectionStrings["LocalConnection"].ConnectionString;
        }


        public void Add(PatientNameCorrectionLogItem logItem)
        {
            string sql = "[export].[uspAddPatientNameCorrectionLog]";

            ClearParameters();

            AddParameter("@OriginalPatientName", DbType.String, 400).Value = SqlParameterSafe(logItem.OriginalPatientName);
            AddParameter("@OriginalBirthday", DbType.Date).Value = logItem.OriginalBirthday;
            AddParameter("@CreatedDate", DbType.DateTimeOffset).Value = logItem.CreatedDate;

            AddParameter("@CorrectedPatientName", DbType.String, 400).Value = SqlParameterSafe(logItem.CorrectedPatientName);
            AddParameter("@CorrectedBirthday", DbType.Date).Value = logItem.CorrectedBirthday;
            AddParameter("@Comments", DbType.String).Value = SqlParameterSafe(logItem.Comments);

            try
            {
                RunProcedureNonSelectQuery(sql);
            }
            finally
            {
                Disconnect();
            }
        }

        public List<PatientNameCorrectionLogItem> Get(DateTimeOffset? startDate, DateTimeOffset? endDate, string nameFilter, string orderBy, int startRowIndex, int maximumRows)
        {
            string innerOrderBy = string.Empty;
            string totalOrderBy = string.Empty;

            var result = new List<PatientNameCorrectionLogItem>();


            QueryBuilder qbInnerSql = new QueryBuilder(string.Format(@"
SELECT TOP(@startRowIndex + @maxRows) 
    ROW_NUMBER() OVER ( ORDER BY CorrectedPatientName) as RowNumber, ID
    FROM [export].[PatientNameCorrectionLog] l
", innerOrderBy));

            string outerSql = @"
WITH tblList(RowNumber, ID)
AS ({0})
SELECT 
l.[OriginalPatientName]
,l.[OriginalBirthday]
,l.[CreatedDate]
,l.[CorrectedPatientName]
,l.[CorrectedBirthday]
,l.[Comments]
FROM tblList t
	INNER JOIN export.PatientNameCorrectionLog l ON t.ID = l.ID
WHERE RowNumber BETWEEN @startRowIndex AND (@startRowIndex + @maxRows)
ORDER BY {1}";


            ClearParameters();

            //parameters for pagination
            AddParameter("@startRowIndex", DbType.Int32).Value = startRowIndex + 1;
            AddParameter("@maxRows", DbType.Int32).Value = maximumRows;

            // sorting

            if (string.IsNullOrEmpty(orderBy)) orderBy = "l.CreatedDate DESC"; // default sort order

            innerOrderBy = totalOrderBy = orderBy;

            //map user field names to the query field names
            if (orderBy.Contains("CreatedDate"))
            {
                innerOrderBy = orderBy.Replace("LastCreatedDate", "MAX(r.CreatedDate)");
                totalOrderBy = orderBy;
            }
            //else if (orderBy.Contains("FullName"))
            //{
            //    innerOrderBy = orderBy.Replace("FullName", "l.PatientName");
            //    totalOrderBy = orderBy;
            //}
            //else if (orderBy.Contains("Birthday"))
            //{
            //    innerOrderBy = orderBy.Replace("Birthday", "r.Birthday");
            //    totalOrderBy = orderBy;
            //}

            // filter
            if (!string.IsNullOrEmpty(nameFilter))
            {
                qbInnerSql.AppendWhereCondition("(l.CorrectedPatientName LIKE @NameFilter OR l.OriginalPatientName LIKE @NameFilter)", ClauseType.And);
                AddParameter("@NameFilter", DbType.String, 128).Value = SqlLikeStringPrepeare(nameFilter, LikeCondition.Contains);
            }

            if (startDate.HasValue)
            {
                qbInnerSql.AppendWhereCondition("l.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTimeOffset).Value = startDate.Value;
            }
            if (endDate.HasValue)
            {
                qbInnerSql.AppendWhereCondition("l.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTimeOffset).Value = endDate.Value;
            }


            try
            {
                using (var reader = RunSelectQuery(string.Format(outerSql, qbInnerSql.ToString(), totalOrderBy)))
                {
                    while (reader.Read())
                    {
                        result.Add(new PatientNameCorrectionLogItem
                        {
                            OriginalPatientName = reader.GetString(0),
                            OriginalBirthday = reader.Get<DateTime>(1).Value,
                            CreatedDate = reader.Get<DateTimeOffset>(2).Value,
                            CorrectedPatientName = reader.GetString(3),
                            CorrectedBirthday = reader.Get<DateTime>(4).Value,
                            Comments = reader.GetString(5)
                        });
                    }
                }
            }
            finally
            {
                Disconnect();
            }

            return result;
        }

        public int GetCount(DateTimeOffset? startDate, DateTimeOffset? endDate, string nameFilter)
        {
            var qb = new QueryBuilder(@"SELECT COUNT(ID) FROM [export].[PatientNameCorrectionLog] l");

            ClearParameters();

            // filter
            if (!string.IsNullOrEmpty(nameFilter))
            {
                qb.AppendWhereCondition("(l.CorrectedPatientName LIKE @NameFilter OR l.OriginalPatientName LIKE @NameFilter)", ClauseType.And);
                AddParameter("@NameFilter", DbType.String, 128).Value = SqlLikeStringPrepeare(nameFilter, LikeCondition.Contains);
            }

            if (startDate.HasValue)
            {
                qb.AppendWhereCondition("l.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTimeOffset).Value = startDate.Value;
            }
            if (endDate.HasValue)
            {
                qb.AppendWhereCondition("l.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTimeOffset).Value = endDate.Value;
            }

            try
            {
                return RunScalarQuery<int>(qb.ToString()) ?? 0;
            }
            finally
            {
                Disconnect();
            }
        }
    }
}
