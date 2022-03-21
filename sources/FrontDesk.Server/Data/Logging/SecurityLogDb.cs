using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using FrontDesk.Common.Data;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Logging;

using ScreenDox.Server.Models.ViewModels;

namespace FrontDesk.Server.Data.Logging
{
    public interface ISecurityLogRepository
    {
        void Add(SecurityLog logItem);
        List<SecurityEventCategory> GetCategories(bool isSA);
        List<SecurityEventView> GetEvents(int categoryID);
        int GetReportItemsCount(int? categoryID, int? eventID,
            DateTime? startDate, DateTime? endDate, bool isSA, int userID);
        void SetEventEnabledStatus(int eventID, bool enabled);

        bool IsEventEnabled(SecurityEvents securityEvent);
        List<SecurityLogEventSettingResponse> GetEventsSettings(int? categoryID);
    }
    public class SecurityLogDb : DBDatabase, ISecurityLogRepository
    {
        #region Constructors

        // TODO: 1st connection string is used. Any other way to specify connection string, with parameterless constructor?
        public SecurityLogDb() : base(0) { }

        public SecurityLogDb(DbConnection sharedConnection) : base(sharedConnection)
        {
        }

        #endregion

        /// <summary>
        /// Add new security log record
        /// </summary>
        public void Add(SecurityLog logItem)
        {
            string sql = @"
INSERT INTO SecurityLog(SecurityEventID, PKID, LogDate, Metadata, RelatedBranchID)
VALUES(@SecurityEventID, @PKID, @LogDate, @Metadata, @RelatedBranchID)";

            #region Add parameters

            CommandObject.Parameters.Clear();
            AddParameter("@SecurityEventID", DbType.Int32).Value = (int)logItem.LoggedEvent.Event;
            AddParameter("@PKID", DbType.Int32).Value = logItem.UserID;
            AddParameter("@LogDate", DbType.DateTimeOffset).Value = logItem.LogDate;
            AddParameter("@Metadata", DbType.Object).Value = SqlParameterSafe(logItem.LoggedEvent.Metadata);
            AddParameter("@RelatedBranchID", DbType.Int32).Value = SqlParameterSafe(logItem.RelatedBranchLocationID);
            #endregion

            try
            {
                Connect();
                RunNonSelectQuery(sql);
            }
            catch
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Get list of available security event categories
        /// </summary>
        public List<SecurityEventCategory> GetCategories(bool isSA)
        {
            string sql = "";

            if (isSA)
            {
                sql = "select * from dbo.[SecurityEventCategory] order by CategoryName";
            }
            else
            {
                sql = "select * from dbo.[SecurityEventCategory] where SecurityEventCategoryID = 2 order by CategoryName";
            }

            List<SecurityEventCategory> categories = new List<SecurityEventCategory>();

            try
            {
                Connect();

                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        categories.Add(new SecurityEventCategory(reader));
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                Disconnect();
            }

            return categories;
        }

        /// <summary>
        /// Get the list of events by category
        /// </summary>
        public List<SecurityEventView> GetEvents(int categoryID)
        {
            string sql = @"
select SecurityEventID, Description 
from SecurityEvent se 
where se.SecurityEventCategoryID = @SecurityEventCategoryID
order by [Description]";

            CommandObject.Parameters.Clear();
            AddParameter("@SecurityEventCategoryID", DbType.Int32).Value = categoryID;

            List<SecurityEventView> events = new List<SecurityEventView>();

            try
            {
                Connect();

                using (IDataReader reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        events.Add(
                            new SecurityEventView()
                            {
                                ID = Convert.ToInt32(reader["SecurityEventID"]),
                                Description = Convert.ToString(reader["Description"])
                            });
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                Disconnect();
            }

            return events;
        }


        /// <summary>
        /// Get items count for paging
        /// </summary>
        public int GetReportItemsCount(int? categoryID, int? eventID,
            DateTime? startDate, DateTime? endDate, bool isSA, int userID)
        {
            QueryBuilder qb = null;

            if (isSA)
            {
                qb = new QueryBuilder(@"
select COUNT(*)
from SecurityLog sl
    inner join SecurityEvent se on se.SecurityEventID = sl.SecurityEventID
    inner join SecurityEventCategory sec on sec.SecurityEventCategoryID = se.SecurityEventCategoryID
");
            }
            else
            {
                qb = new QueryBuilder(@"
select COUNT(*)
from SecurityLog sl
    inner join SecurityEvent se on se.SecurityEventID = sl.SecurityEventID
    inner join SecurityEventCategory sec on sec.SecurityEventCategoryID = se.SecurityEventCategoryID
    inner join BranchLocation bl on bl.BranchLocationID = sl.RelatedBranchID
    inner join Users_BranchLocation ubl on ubl.BranchLocationID = bl.BranchLocationID");
            }

            CommandObject.Parameters.Clear();

            AddSearchConditions(qb, categoryID, eventID, startDate, endDate, isSA, userID);

            try
            {
                Connect();
                return Convert.ToInt32(RunScalarQuery(qb.ToString()));
            }
            catch
            {
                throw;
            }
            finally
            {
                Disconnect();
            }



        }

        /// <summary>
        /// Get paged report
        /// </summary>
        public DataSet GetReport(int? categoryID, int? eventID,
            DateTime? startDate, DateTime? endDate, bool isSA, int userID,
            int startRowIndex, int maximumRows, string orderBy)
        {
            if (String.IsNullOrEmpty(orderBy))
            {
                orderBy = "LogDate desc";
            }


            QueryBuilder search = null;

            if (isSA)
            {
                search = new QueryBuilder(@"
select	sl.PKID,
        sl.LogDate,
        sl.SecurityEventID,
        sl.Metadata,
        ROW_NUMBER() over (order by {0}) as RowNumber,
        se.[Description],
        sec.CategoryName,
        u.FullName
from SecurityLog sl
    inner join SecurityEvent se on se.SecurityEventID = sl.SecurityEventID
    inner join SecurityEventCategory sec on se.SecurityEventCategoryID = sec.SecurityEventCategoryID
    inner join UserDetails u on u.UserID = sl.PKID");
            }
            else
            {
                search = new QueryBuilder(@"
select	sl.PKID,
        sl.LogDate,
        sl.SecurityEventID,
        sl.Metadata,
        ROW_NUMBER() over (order by {0}) as RowNumber,
        se.[Description],
        sec.CategoryName,
        u.FullName
from SecurityLog sl
    inner join SecurityEvent se on se.SecurityEventID = sl.SecurityEventID
    inner join SecurityEventCategory sec on sec.SecurityEventCategoryID = se.SecurityEventCategoryID
    inner join BranchLocation bl on bl.BranchLocationID = sl.RelatedBranchID
    inner join Users_BranchLocation ubl on ubl.BranchLocationID = bl.BranchLocationID
    inner join UserDetails u on u.UserID = sl.PKID");
            }

            CommandObject.Parameters.Clear();

            AddSearchConditions(search, categoryID, eventID, startDate, endDate, isSA, userID);

            //integrate with CTE
            string cte = String.Format(
@"with res(PKID, LogDate, SecurityEventID, Metadata, RowNumber, [Description], CategoryName, FullName) AS ({0})",
String.Format(search.ToString(), orderBy));


            QueryBuilder main = new QueryBuilder(String.Format(@"
select  se.PKID,
        se.LogDate,
        se.SecurityEventID,
        se.Metadata,
        se.RowNumber,
        se.[Description],
        se.CategoryName,
        se.FullName,
        CAST('' as nvarchar(255)) as HTMLDescription
from res se
where se.RowNumber between @StartRowIndex and (@StartRowIndex + @MaxRows)
order by {0}", orderBy));

            main.AddSqlStatement(cte);

            AddParameter("@StartRowIndex", DbType.Int32).Value = startRowIndex;
            AddParameter("@MaxRows", DbType.Int32).Value = maximumRows;


            try
            {
                Connect();
                return GetDataSet(main.ToString());
            }
            catch
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
        }

        public DataSet GetReport(int? categoryID, int? eventID,
            DateTime? startDate, DateTime? endDate, bool isSA, int userID, string orderBy)
        {
            if (String.IsNullOrEmpty(orderBy))
            {
                orderBy = "LogDate desc";
            }


            QueryBuilder search = null;

            if (isSA)
            {
                search = new QueryBuilder(@"
select	sl.PKID,
        sl.LogDate,
        sl.SecurityEventID,
        sl.Metadata,
        se.[Description],
        sec.CategoryName,
        u.FullName,
        CAST('' as nvarchar(255)) as LocalDescription
from SecurityLog sl
    inner join SecurityEvent se on se.SecurityEventID = sl.SecurityEventID
    inner join SecurityEventCategory sec on se.SecurityEventCategoryID = sec.SecurityEventCategoryID
    inner join UserDetails u on u.UserID = sl.PKID");
            }
            else
            {
                search = new QueryBuilder(@"
select	sl.PKID,
        sl.LogDate,
        sl.SecurityEventID,
        sl.Metadata,
        se.[Description],
        sec.CategoryName,
        u.FullName,
        CAST('' as nvarchar(255)) as LocalDescription
from SecurityLog sl
    inner join SecurityEvent se on se.SecurityEventID = sl.SecurityEventID
    inner join SecurityEventCategory sec on sec.SecurityEventCategoryID = se.SecurityEventCategoryID
    inner join BranchLocation bl on bl.BranchLocationID = sl.RelatedBranchID
    inner join Users_BranchLocation ubl on ubl.BranchLocationID = bl.BranchLocationID
    inner join UserDetails u on u.UserID = sl.PKID");
            }

            CommandObject.Parameters.Clear();
            AddSearchConditions(search, categoryID, eventID, startDate, endDate, isSA, userID);

            search.AppendOrderCondition(orderBy);

            try
            {
                Connect();
                return GetDataSet(search.ToString());
            }
            catch
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
        }


        private void AddSearchConditions(QueryBuilder qb, int? categoryID, int? eventID,
            DateTime? startDate, DateTime? endDate, bool isSA, int userID)
        {
            if (categoryID.HasValue)
            {
                qb.AppendWhereCondition("se.SecurityEventCategoryID=@SecurityEventCategoryID", ClauseType.And);
                AddParameter("@SecurityEventCategoryID", DbType.Int32).Value = categoryID.Value;
            }

            if (eventID.HasValue)
            {
                qb.AppendWhereCondition("se.SecurityEventID=@SecurityEventID", ClauseType.And);
                AddParameter("@SecurityEventID", DbType.Int32).Value = eventID.Value;
            }

            if (startDate.HasValue)
            {
                qb.AppendWhereCondition("sl.LogDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.Date).Value = startDate.Value;
            }
            if (endDate.HasValue)
            {
                qb.AppendWhereCondition("sl.LogDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.Date).Value = endDate.Value.AddDays(1);
            }

            if (!isSA)
            {
                qb.AppendWhereCondition("se.SecurityEventCategoryID = 2", ClauseType.And);
                AddParameter("@UserID", DbType.Int32).Value = SqlParameterSafe(userID);
                qb.AppendWhereCondition("ubl.UserID = @UserID", ClauseType.And);
            }
        }

        [Obsolete("Used in legacy only")]
        /// <summary>
        /// Get DataSet with two tables: EventCatefories and Events
        /// </summary>
        public DataSet GetCategoriesWithEvents()
        {
            string sql = @"
select * from dbo.[SecurityEventCategory] order by [CategoryName]
select * from dbo.[SecurityEvent] order by [Description]";

            DataSet ds = null;
            try
            {
                Connect();
                ds = GetDataSet(sql);
                ds.Relations.Add(new DataRelation("SecEventToCategory",
                    ds.Tables[0].Columns["SecurityEventCategoryID"],
                    ds.Tables[1].Columns["SecurityEventCategoryID"]));

            }
            catch
            {
                throw;
            }
            finally
            {
                Disconnect();
            }

            return ds;
        }

        /// <summary>
        /// Get events with enable status and category
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public List<SecurityLogEventSettingResponse> GetEventsSettings(int? categoryID)
        {
            var qb = new QueryBuilder(@"
select SecurityEventID, Description, SecurityEventCategoryID, Enabled 
from SecurityEvent se");

            ClearParameters();

            if (categoryID.HasValue)
            {
                qb.AppendWhereCondition("se.SecurityEventCategoryID = @SecurityEventCategoryID", ClauseType.And);
                AddParameter("@SecurityEventCategoryID", DbType.Int32).Value = categoryID;

            }

            qb.AppendOrderCondition("[SecurityEventCategoryID]", OrderType.Asc);
            qb.AppendOrderCondition("[Description]", OrderType.Asc);


            List<SecurityLogEventSettingResponse> events = new List<SecurityLogEventSettingResponse>();

            try
            {
                Connect();

                using (IDataReader reader = RunSelectQuery(qb.ToString()))
                {
                    while (reader.Read())
                    {
                        events.Add(
                            new SecurityLogEventSettingResponse()
                            {
                                ID = reader["SecurityEventID"].As<int>(),
                                Description = reader["Description"].As<string>(),
                                CategoryID = reader["SecurityEventCategoryID"].As<int>(),
                                IsEnabled = reader["Enabled"].As<bool>()
                            }); ;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                Disconnect();
            }

            return events;
        }

        /// <summary>
        /// Set the status of security log event
        /// </summary>
        public void SetEventEnabledStatus(int eventID, bool enabled)
        {
            string sql = @"
update SecurityEvent 
    SET Enabled=@Enabled 
where SecurityEventID=@SecurityEventID";

            CommandObject.Parameters.Clear();
            AddParameter("@Enabled", DbType.Boolean).Value = enabled;
            AddParameter("@SecurityEventID", DbType.Int32).Value = eventID;

            try
            {
                Connect();
                RunSelectQuery(sql);
            }
            catch
            {
                throw;
            }
            finally
            {
                Disconnect();
            }

        }

        /// <summary>
        /// Check for security event enabled status
        /// </summary>
        public bool IsEventEnabled(SecurityEvents securityEvent)
        {
            string sql = "select Enabled from SecurityEvent where SecurityEventID=@SecurityEventID";

            CommandObject.Parameters.Clear();
            AddParameter("@SecurityEventID", DbType.Int32).Value = (int)securityEvent;

            try
            {
                Connect();
                return Convert.ToBoolean(RunScalarQuery(sql));
            }
            catch
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
        }

       
    }
}
