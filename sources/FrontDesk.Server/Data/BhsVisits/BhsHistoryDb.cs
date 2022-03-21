using FrontDesk.Common.Data;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Screening.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Data.BhsVisits
{
    public interface IBhsHistoryRepository : ITransactionalDatabase
    {
        List<BhsScreeningHistoryItem> GetLastNotEmptyScreenings(ScreeningPatientIdentity patient, DateTimeOffset currentTime, int limit);
        List<BhsHistoryItem> GetVisitsAndFollowUps(ScreeningPatientIdentity patient, DateTimeOffset startDate, DateTimeOffset endDate);
    }

    public class BhsHistoryDb : DBDatabase, IBhsHistoryRepository
    {
        public BhsHistoryDb() : base(0) { }

        internal BhsHistoryDb(DbConnection sharedConnection) : base(sharedConnection) { }

        public List<BhsScreeningHistoryItem> GetLastNotEmptyScreenings(ScreeningPatientIdentity patient, DateTimeOffset currentTime, int limit)
        {
            var result = new List<BhsScreeningHistoryItem>(limit);
            const string sql = "[dbo].[uspGetHistoryOfPreviousNotEmptyScreenings]";

            ClearParameters();
            AddParameter("@FirstName", DbType.String, 128).Value = SqlParameterSafe(patient.FirstName);
            AddParameter("@LastName", DbType.String, 128).Value = SqlParameterSafe(patient.LastName);
            AddParameter("@MiddleName", DbType.String, 128).Value = SqlParameterSafe(patient.MiddleName);
            AddParameter("@Birthday", DbType.Date).Value = patient.Birthday;
            AddParameter("@EndDate", DbType.DateTimeOffset).Value = currentTime;
            AddParameter("@Limit", DbType.Int32).Value = limit;


            try
            {
                using (var reader = RunProcedureSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        result.Add(new BhsScreeningHistoryItem
                        {
                            ScreeningResultID = reader.Get<long>("ScreeningResultID"),
                            CreatedDate = reader.Get<DateTimeOffset>("CreatedDate"),
                            IsPositive = reader.Get<int>("Positive") > 0,
                            HasAnyScreening = reader.Get<int>("HasAnyScreening") > 0
                        });
                    }
                }
                return result;
            }
            finally
            {
                Disconnect();
            }
        }

        public List<BhsHistoryItem> GetVisitsAndFollowUps(ScreeningPatientIdentity patient, DateTimeOffset startDate, DateTimeOffset endDate)
        {


            var result = new List<BhsHistoryItem>();
            const string sql = "[dbo].[uspGetHistoryOfVisitsAndFollowUps]";

            ClearParameters();
            AddParameter("@FirstName", DbType.String, 128).Value = SqlParameterSafe(patient.FirstName);
            AddParameter("@LastName", DbType.String, 128).Value = SqlParameterSafe(patient.LastName);
            AddParameter("@MiddleName", DbType.String, 128).Value = SqlParameterSafe(patient.MiddleName);
            AddParameter("@Birthday", DbType.Date).Value = patient.Birthday;
            AddParameter("@StartDate", DbType.DateTimeOffset).Value = startDate;
            AddParameter("@EndDate", DbType.DateTimeOffset).Value = endDate;


            try
            {
                using (var reader = RunProcedureSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        result.Add(new BhsHistoryItem
                        {
                            ID = reader.Get<long>("ID"),
                            CompletedDate = reader.Get<DateTimeOffset>("CompleteDate"),
                            DischargeID = reader.GetNullable<int>("DischargedID"),
                            NewVisitReferralRecommendationAcceptedID = reader.GetNullable<int>("NewVisitReferralRecommendationAcceptedID")
                        });
                    }
                }
                return result;
            }
            finally
            {
                Disconnect();
            }
        }
    }
}
