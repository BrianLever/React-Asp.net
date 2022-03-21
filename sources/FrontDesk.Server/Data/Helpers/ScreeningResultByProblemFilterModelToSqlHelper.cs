using FrontDesk.Common.Data;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Screening.Models;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Data.Helpers
{
    public static class ScreeningResultByProblemFilterModelToSqlHelper
    {
        private static QueryBuilder CreateUnionAllBuilder()
        {
            return new QueryBuilder(@"
SELECT v.ScreeningResultID, v.PatientName, v.Birthday, v.CreatedDate, v.BranchLocationID
FROM dbo.vScreeningResults v
");
        }

        public static void TranslateToScreeningDetailsSql(ScreeningResultByProblemFilter filter, StringBuilder unionSql, IDbCommand commandObject, ClauseType linkRule = ClauseType.And)
        {
            if (filter == null || !filter.Filters.Any())
            {
                unionSql.AppendLine(CreateUnionAllBuilder().ToString());
                return;
            }

            if (filter.Filters.Count == 1 && filter.Filters.First().ScreeningSection == ScreeningResultByProblemFilter.AnySectionName)
            {
                var sqlQb = CreateUnionAllBuilder();
                sqlQb.AppendWhereCondition("v.ScoreLevel>=@MinScoreLevel", ClauseType.And);
                commandObject.AddParameter("@MinScoreLevel", DbType.Int32).Value = filter.Filters.First().MinScoreLevel;

                unionSql.AppendLine(sqlQb.ToString());

                return;
            }


            int filterNo = 0;

            foreach (var filterQuery in filter.Filters)
            {
                var sqlQb = CreateUnionAllBuilder();

                if (filterQuery.ScreeningSection == VisitSettingsDescriptor.SubstanceAbuse 
                    || filterQuery.ScreeningSection == VisitSettingsDescriptor.PartnerViolence 
                    || filterQuery.ScreeningSection == VisitSettingsDescriptor.Alcohol
                    || filterQuery.ScreeningSection == VisitSettingsDescriptor.ProblemGambling
                    )
                {
                    sqlQb.AppendWhereCondition(
                        @"v.ScreeningSectionID=@ScreeningSectionID_{0} AND v.ScoreLevel=@ScoreLevel_{0}".FormatWith(filterNo), ClauseType.And);

                    commandObject.AddParameter("@ScreeningSectionID_" + filterNo, DbType.AnsiString, 5).Value = filterQuery.ScreeningSection;
                    commandObject.AddParameter("@ScoreLevel_" + filterNo, DbType.Int32).Value = filterQuery.MinScoreLevel;
                }
                else if (filterQuery.ScreeningSection == VisitSettingsDescriptor.Depression || filterQuery.ScreeningSection == VisitSettingsDescriptor.DepressionThinkOfDeath)
                {
                    commandObject.AddParameter("@ScreeningSectionID_" + filterNo, DbType.AnsiString, 5).Value = ScreeningSectionDescriptor.Depression;

                    if (filterQuery.ScreeningSection == VisitSettingsDescriptor.Depression)
                    {
                        sqlQb.AppendWhereCondition(
                        @"v.ScreeningSectionID=@ScreeningSectionID_{0} AND v.ScoreLevel=@ScoreLevel_{0}".FormatWith(filterNo), ClauseType.And);

                        commandObject.AddParameter("@ScoreLevel_" + filterNo, DbType.Int32).Value = filterQuery.MinScoreLevel;
                    }
                    else
                    {
                        //filter by question

                        sqlQb.AppendWhereCondition(
                       @"
v.ScreeningSectionID=@ScreeningSectionID_{0} AND (v.QuestionID=@DepressionQuestionID_{0} AND v.AnswerValue=@DepressionAnswerID_{0})"
.FormatWith(filterNo), ClauseType.And);

                        //qb.AppendWhereCondition("", ClauseType.And);
                        commandObject.AddParameter("@DepressionQuestionID_" + filterNo, DbType.Int32).Value = ScreeningSectionDescriptor.DepressionThinkOfDeathQuestionID;
                        commandObject.AddParameter("@DepressionAnswerID_" + filterNo, DbType.Int32).Value = filterQuery.MinScoreLevel;
                    }
                }
                else if (filterQuery.ScreeningSection == ScreeningSectionDescriptor.AnxietyAllQuestions)
                {
                    commandObject.AddParameter("@ScreeningSectionID_" + filterNo, DbType.AnsiString, 5).Value = ScreeningSectionDescriptor.Anxiety;

                    sqlQb.AppendWhereCondition(
                    @"v.ScreeningSectionID=@ScreeningSectionID_{0} AND v.ScoreLevel=@ScoreLevel_{0}".FormatWith(filterNo), ClauseType.And);

                    commandObject.AddParameter("@ScoreLevel_" + filterNo, DbType.Int32).Value = filterQuery.MinScoreLevel;

                }
                else if (filterQuery.ScreeningSection == ScreeningSectionDescriptor.Tobacco)
                {
                    //any positive answer in tobacco section
                    sqlQb.AppendWhereCondition(
                     @"v.ScreeningSectionID IN (@TobaccoScreeningSectionID)  AND v.ScoreLevel > 0", ClauseType.And);

                    commandObject.AddParameter("@TobaccoScreeningSectionID", DbType.AnsiString, 5).Value = ScreeningSectionDescriptor.Tobacco;
                }
                else if (filterQuery.ScreeningSection == VisitSettingsDescriptor.TobaccoUseCeremony
                    || filterQuery.ScreeningSection == VisitSettingsDescriptor.TobaccoUseSmoking
                    || filterQuery.ScreeningSection == VisitSettingsDescriptor.TobaccoUseSmokeless)
                {

                    sqlQb.AppendWhereCondition(
                   @"v.ScreeningSectionID=@ScreeningSectionID_{0} AND (v.QuestionID=@TobaccoQuestionID_{0} AND v.AnswerValue=1)
".FormatWith(filterNo), ClauseType.And);


                    commandObject.AddParameter("@ScreeningSectionID_" + filterNo, DbType.AnsiString, 5).Value = ScreeningSectionDescriptor.Tobacco;
                    commandObject.AddParameter("@TobaccoQuestionID_" + filterNo, DbType.Int32).Value = VisitSettingsDescriptor.MapTobaccoSettingToTobaccoQuestion(filterQuery.ScreeningSection);
                }
                else if (filterQuery.ScreeningSection == VisitSettingsDescriptor.DrugOfChoice)
                {

                    sqlQb.AppendWhereCondition(@"v.ScreeningSectionID=@ScreeningSectionID_{0}".FormatWith(filterNo), ClauseType.And);

                    sqlQb.AppendWhereCondition(@"EXISTS(SELECT 1 FROM dbo.ScreeningSectionQuestionResult qr
WHERE qr.ScreeningSectionID = v.ScreeningSectionID AND qr.ScreeningResultID = v.ScreeningResultID
AND qr.AnswerValue = @Drug_{0})".FormatWith(filterNo), ClauseType.And);

                    commandObject.AddParameter("@ScreeningSectionID_" + filterNo, DbType.AnsiString, 5).Value = ScreeningSectionDescriptor.DrugOfChoice;
                    commandObject.AddParameter("@Drug_" + filterNo, DbType.Int32).Value = filterQuery.MinScoreLevel;


                }

                if (filterNo > 0)
                {
                    unionSql.AppendLine(" UNION ALL ");
                }

                unionSql.AppendLine(sqlQb.ToString());

                filterNo++;
            }

        }

    }
}
