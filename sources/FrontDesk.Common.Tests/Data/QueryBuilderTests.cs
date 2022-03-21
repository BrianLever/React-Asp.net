using FluentAssertions;
using FrontDesk.Common.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDesk.Common.Tests.Data
{
    [TestClass]
    public class QueryBuilderTests
    {

        protected QueryBuilder sut()
        {
            return new QueryBuilder("SELECT t1.ID FROM dbo.Table1 t1 ");
        }

        [TestMethod]
        public void WhenCreateSimpleQuery()
        {
            var qb = sut();
            qb.AppendJoinStatement("LEFT JOIN dbo.Table2 t2 ON t1.Table2ID = t2.ID");
            qb.AppendWhereCondition("t1.Name=@Name1", ClauseType.And);
            qb.AppendWhereCondition("t2.Name=@Name2", ClauseType.And);
            qb.AppendOrderCondition("t1.ID", OrderType.Asc);


            qb.ToString().Should().Be(@"  SELECT t1.ID FROM dbo.Table1 t1  LEFT JOIN dbo.Table2 t2 ON t1.Table2ID = t2.ID where  t1.Name=@Name1  and t2.Name=@Name2    order by t1.ID asc");
        }

        [TestMethod]
        public void WhenWhereWithGroups()
        {
            var qb = sut();
            qb.AppendJoinStatement("LEFT JOIN dbo.Table2 t2 ON t1.Table2ID = t2.ID");

            qb.BeginWhereConditionGroup(ClauseType.And);
            qb.AppendWhereCondition("t1.Name=@Name1", ClauseType.And);
            qb.AppendWhereCondition("t2.Name=@Name2", ClauseType.Or);

            qb.EndWhereConditionGroup();

            qb.AppendWhereCondition("t2.ID=@ID", ClauseType.And);

            qb.ToString().Should().Be(@"  SELECT t1.ID FROM dbo.Table1 t1  LEFT JOIN dbo.Table2 t2 ON t1.Table2ID = t2.ID where  ( t1.Name=@Name1  or t2.Name=@Name2 )  and t2.ID=@ID    ");

        }


        [TestMethod]
        public void WhenNestedWhereGroups()
        {
            var qb = sut();
            qb.AppendWhereCondition("t1.ID=@ID", ClauseType.And);

            qb.BeginWhereConditionGroup(ClauseType.And);
            qb.AppendWhereCondition("t1.Name=@Name1", ClauseType.And);

            qb.BeginWhereConditionGroup(ClauseType.Or);
            qb.AppendWhereCondition("t2.Name=@Name2", ClauseType.And);
            qb.AppendWhereCondition("t3.Name=@Name3", ClauseType.And);
            qb.EndWhereConditionGroup();



            qb.EndWhereConditionGroup();


            qb.ToString().Should().Be(@"  SELECT t1.ID FROM dbo.Table1 t1   where  t1.ID=@ID  and ( t1.Name=@Name1  or ( t2.Name=@Name2  and t3.Name=@Name3 ) )    ");

        }


        [TestMethod]
        public void WhenStartsTwoWhereGroups()
        {
            var qb = sut();

            qb.BeginWhereConditionGroup(ClauseType.And);
            qb.BeginWhereConditionGroup(ClauseType.Or);
            qb.AppendWhereCondition("t2.Name=@Name2", ClauseType.And);
            qb.AppendWhereCondition("t3.Name=@Name3", ClauseType.And);
            qb.EndWhereConditionGroup();

            qb.EndWhereConditionGroup();


            qb.ToString().Should().Be(@"  SELECT t1.ID FROM dbo.Table1 t1   where  ( ( t2.Name=@Name2  and t3.Name=@Name3 ) )    ");

        }
    }
}