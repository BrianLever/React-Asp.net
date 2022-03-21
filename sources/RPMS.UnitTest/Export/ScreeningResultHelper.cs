using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk;

namespace RPMS.UnitTest.Export
{
    internal static class ScreeningResultHelper
    {
        #region screeing result

        internal static ScreeningResult GetAllYesAnswers()
        {
            return new ScreeningResult
            {

            };
        }

        internal static ScreeningResult GetAllNoAnswers()
        {
            var result = new ScreeningResult();

            //depression
            result.SectionAnswers.Add(new ScreeningSectionResult
            {
                ScreeningSectionID = ScreeningSectionDescriptor.Depression,
                AnswerValue = 0
            });


            //smoker in the home
            result.SectionAnswers.Add(new ScreeningSectionResult
            {
                ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome,
                AnswerValue = 0
            });

            //Tobacco
            result.SectionAnswers.Add(new ScreeningSectionResult
            {
                ScreeningSectionID = ScreeningSectionDescriptor.Tobacco,
                AnswerValue = 0
            });

            //Alcohol
            result.SectionAnswers.Add(GetAlcoholAllNo());
           
            return result;
        }

        #endregion

        #region alcohol section

        internal static ScreeningSectionResult GetAlcoholAllNo()
        {
            return new ScreeningSectionResult
            {
                ScreeningSectionID = ScreeningSectionDescriptor.Alcohol,
                AnswerValue = 0,
                Score = 0
            };
        }

        internal static ScreeningSectionResult GetAlcoholAllYes()
        {
            var result = new ScreeningSectionResult
            {
                ScreeningSectionID = ScreeningSectionDescriptor.Alcohol,
                AnswerValue = 1,
            };
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 1));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 1));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 1));
            result.Score = 4;

            return result;
        }
        #endregion

        #region Violence
        internal static ScreeningSectionResult GetViolenceHurtNever()
        {
            var result = new ScreeningSectionResult
            {
                ScreeningSectionID = ScreeningSectionDescriptor.PartnerViolence,
                AnswerValue = 1,
            };
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 2));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 2));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 1));

            return result;
        }
        internal static ScreeningSectionResult GetViolenceHurtRarely()
        {
            var result = new ScreeningSectionResult
            {
                ScreeningSectionID = ScreeningSectionDescriptor.PartnerViolence,
                AnswerValue = 1,
            };
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, PartnerViolenceQuestionsDescriptor.RarelyAnswer));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 2));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 2));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 1));

            return result;
        }
        #endregion


        #region Depression
        internal static ScreeningSectionResult GetDepressionHurtYouselfNotAtAll()
        {
            var result = new ScreeningSectionResult
            {
                ScreeningSectionID = ScreeningSectionDescriptor.Depression,
                AnswerValue = 1,
            };
            result.AnswerValue = 1;
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 0));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 2));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 3));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 1));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 1));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 2));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, DepressionQuestionsDescriptor.NotAtAllAnswer));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 2));

            return result;
        }


        internal static ScreeningSectionResult GetDepressionHurtYouselfSeveralDays()
        {
            var result = new ScreeningSectionResult
            {
                ScreeningSectionID = ScreeningSectionDescriptor.Depression,
                AnswerValue = 1,
								ScoreLevel = 11,
            };
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 0));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 2));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 3));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 1));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 1));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 2));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, DepressionQuestionsDescriptor.SeveralDaysAnswer));
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 2));

            return result;
        }


		internal static ScreeningSectionResult GetDepressionHurtYouselfSeveralDaysPlusModerate()
		{
			var result = new ScreeningSectionResult
			{
				ScreeningSectionID = ScreeningSectionDescriptor.Depression,
				AnswerValue = 1,
				Score = 11,
				ScoreLevel = 2,
				ScoreLevelLabel = "MODERATE depression severity"
			};
			result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
			result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
			result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 2));
			result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 3));
			result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
			result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
			result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
			result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 3));
			result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, DepressionQuestionsDescriptor.SeveralDaysAnswer));
			result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 2));

			return result;
		}

		internal static ScreeningSectionResult GetDepressionHurtYouseNotAtAllPlusModerate()
		{
			var result = new ScreeningSectionResult
			{
				ScreeningSectionID = ScreeningSectionDescriptor.Depression,
				AnswerValue = 1,
				Score = 11,
				ScoreLevel = 2,
				ScoreLevelLabel = "MODERATE depression severity"
			};
			result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
			result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
			result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 2));
			result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 3));
			result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
			result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
			result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 1));
			result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 3));
			result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, DepressionQuestionsDescriptor.NotAtAllAnswer));
			result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 2));

			return result;
		}
		#endregion
	}
}
