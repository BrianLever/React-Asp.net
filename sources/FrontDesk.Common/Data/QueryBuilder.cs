using System;
using System.Collections.Generic;
using System.Text;

namespace FrontDesk.Common.Data
{
    /// <summary>
    /// SQL SELECT Where Condition
    /// </summary>
    public enum ClauseType
    {
        And,
        Or
    }
    /// <summary>
    /// SQL SELECT Order type
    /// </summary>
    public enum OrderType
    {
        Asc,
        Desc
    }
    /// <summary>
    /// SQL SELECT Query Builder
    /// </summary>
    public class QueryBuilder
    {
        private StringBuilder declaredVariables = null;
        private StringBuilder tSqlStatments = null;
        private StringBuilder query = null;
        private List<string> joins = new List<string>();
        private StringBuilder whereClause = null;
        private StringBuilder groupByClause = null;
        private StringBuilder orderByClause = null;

        private int topValue;
        /// <summary>
        /// Get or Set SELECT TOP (X) value
        /// </summary>
        public int TopValue
        {
            get { return topValue; }
            set { topValue = value; }
        }

        private string space = " ";
        private string coma = ",";

        /// <summary>
        /// Default constructor
        /// </summary>
        public QueryBuilder()
        {
            topValue = 0;

            declaredVariables = new StringBuilder();
            tSqlStatments = new StringBuilder();
            query = new StringBuilder();
            whereClause = new StringBuilder();
            groupByClause = new StringBuilder();
            orderByClause = new StringBuilder();
        }
        /// <summary>
        /// Constructor with SELECT clause text
        /// </summary>
        /// <param name="selectQuery"></param>
        public QueryBuilder(string selectQuery)
        {
            topValue = 0;

            declaredVariables = new StringBuilder();
            tSqlStatments = new StringBuilder();
            query = new StringBuilder(selectQuery);

            whereClause = new StringBuilder();
            groupByClause = new StringBuilder();
            orderByClause = new StringBuilder();
        }


        /// <summary>
        /// Append JOIN statements
        /// </summary>
        /// <param name="whereCondition"></param>
        /// <param name="clauseType"></param>
        public void AppendJoinStatement(string joinClause)
        {
            if (!joins.Contains(joinClause))
            {
                joins.Add(joinClause);
            }
            //query.Append(joinClause);
        }

        /// <summary>
        /// Append Where statement
        /// </summary>
        /// <param name="whereCondition"></param>
        /// <param name="clauseType"></param>
        public void AppendWhereCondition(string whereCondition, ClauseType clauseType)
        {
            if (whereClause.Length != 0 && !IsWhereConditionGroupStarted)
            {
                whereClause.Append(space);
                whereClause.Append(clauseType == ClauseType.And ? "and" : "or");
            }

            whereClause.Append(space);
            whereClause.Append(whereCondition);
            whereClause.Append(space);

            IsWhereConditionGroupStarted = false;
        }

        public bool HasWhereConditions
        {
            get
            {
                return whereClause.Length > 0;
            }
        }

        private bool IsWhereConditionGroupStarted = false;

        public void BeginWhereConditionGroup(ClauseType clauseType)
        {
            if (whereClause.Length != 0 && !IsWhereConditionGroupStarted)
            {
                whereClause.Append(space);
                whereClause.Append(clauseType == ClauseType.And ? "and" : "or");
            }

            whereClause.Append(space);
            whereClause.Append("(");

            IsWhereConditionGroupStarted = true;
        }


        public void EndWhereConditionGroup()
        {
            whereClause.Append(")");
            whereClause.Append(space);

            IsWhereConditionGroupStarted = false;
        }



        /// <summary>
        /// Append Order By statement
        /// </summary>
        /// <param name="orderCondition"></param>
        /// <param name="orderType"></param>
        public void AppendOrderCondition(string orderCondition, OrderType orderType)
        {
            if (orderByClause.Length > 0)
            {
                orderByClause.Append(space);
                orderByClause.Append(coma);
                orderByClause.Append(space);
            }

            orderByClause.Append(orderCondition);
            orderByClause.Append(space);
            orderByClause.Append(orderType.ToString().ToLowerInvariant());
        }
        /// <summary>
        /// Append Order by statement with specified direction
        /// </summary>
        /// <param name="orderCondition"></param>
        public void AppendOrderCondition(string orderCondition)
        {
            if (orderByClause.Length > 0)
            {
                orderByClause.Append(space);
                orderByClause.Append(coma);
                orderByClause.Append(space);
            }

            orderByClause.Append(orderCondition);
        }
        /// <summary>
        /// Append GROUP BY statement
        /// </summary>
        /// <param name="groupCondition"></param>
        public void AppendGroupCondition(string groupCondition)
        {
            if (groupByClause.Length > 0)
            {
                groupByClause.Append(space);
                groupByClause.Append(coma);
                groupByClause.Append(space);
            }

            groupByClause.Append(groupCondition);
            groupByClause.Append(space);

        }
        /// <summary>
        /// Declare local variable
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public void AddVariableDeclaration(string name, string type)
        {
            declaredVariables.Append(space);
            declaredVariables.Append("declare");
            declaredVariables.Append(space);
            declaredVariables.Append(name);
            declaredVariables.Append(space);
            declaredVariables.Append(type);
            declaredVariables.Append(space);
        }

        /// <summary>
        /// Append TSQL statement before main query
        /// </summary>
        /// <param name="statment"></param>
        public void AddSqlStatement(string statment)
        {
            tSqlStatments.Append(space);
            tSqlStatments.Append(statment);
            tSqlStatments.Append(space);
        }
        /// <summary>
        /// Get complete SQL query text
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var variables = declaredVariables.ToString();
            var tSql = tSqlStatments.ToString();
            var selectFrom = query.ToString();
            var selectFromJoins = string.Join(" ", joins);


            var where = whereClause.Length > 0 ? "where " + whereClause.ToString() : String.Empty;
            var groupBy = groupByClause.Length > 0 ? "group by " + groupByClause.ToString() : String.Empty;
            var orderBy = orderByClause.Length > 0 ? "order by " + orderByClause.ToString() : String.Empty;


            #region calculate and insert top value into select clause if defined
            if (topValue != 0)
            {
                var firstDistinctOrSelectStatmentIndex = selectFrom.IndexOf("distinct");
                if (firstDistinctOrSelectStatmentIndex > 0)
                {
                    // insert top clause after distinct
                    firstDistinctOrSelectStatmentIndex += 8; // add length of 'distinct'
                }
                else
                {
                    //inser top clause after select
                    firstDistinctOrSelectStatmentIndex = selectFrom.IndexOf("select");
                    if (firstDistinctOrSelectStatmentIndex > 0)
                    {
                        firstDistinctOrSelectStatmentIndex += 6; //add length of 'select'
                    }
                }

                // if position of top clause was defined then add it to select clause
                if (firstDistinctOrSelectStatmentIndex > 0)
                {
                    selectFrom = selectFrom.Insert(firstDistinctOrSelectStatmentIndex, String.Format(" top {0}", topValue));
                }
            }
            #endregion


            return String.Format("{0} {1} {2} {3} {4} {5}  {6}", variables, tSql, selectFrom, selectFromJoins, where, groupBy, orderBy);
        }
    }
}
