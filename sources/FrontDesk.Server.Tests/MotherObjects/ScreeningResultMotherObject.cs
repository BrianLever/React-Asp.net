using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Tests.MotherObjects
{
    internal static class ScreeningResultMotherObject
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
            return GetAllNoAnswersForSections(
                ScreeningSectionDescriptor.Depression,
                ScreeningSectionDescriptor.SmokerInHome,
                ScreeningSectionDescriptor.Tobacco,
                ScreeningSectionDescriptor.Alcohol,
                ScreeningSectionDescriptor.SubstanceAbuse,
                ScreeningSectionDescriptor.PartnerViolence
                );
        }

        internal static ScreeningResult GetAllNoAnswersForSections(params string[] includedSections)
        {
            var result = new ScreeningResult
            {
                LocationID = 1
            };
            if (includedSections.Contains(ScreeningSectionDescriptor.Depression))
            {
                //depression
                result.AppendSectionAnswer(new ScreeningSectionResult
                {
                    ScreeningSectionID = ScreeningSectionDescriptor.Depression,
                    AnswerValue = 0
                });
            }

            if (includedSections.Contains(ScreeningSectionDescriptor.SmokerInHome))
            {
                //smoker in the home
                result.AppendSectionAnswer(new ScreeningSectionResult
                {
                    ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome,
                    AnswerValue = 0
                });

                result.FindSectionByID(ScreeningSectionDescriptor.SmokerInHome)
                    .AppendQuestionAnswer(new ScreeningSectionQuestionResult
                    {
                        QuestionID = 1,
                        AnswerValue = 0
                    });
            }

            if (includedSections.Contains(ScreeningSectionDescriptor.Tobacco))
            {
                //Tobacco
                result.AppendSectionAnswer(new ScreeningSectionResult
                {
                    ScreeningSectionID = ScreeningSectionDescriptor.Tobacco,
                    AnswerValue = 0
                });

            }
            if (includedSections.Contains(ScreeningSectionDescriptor.Alcohol))
            {
                //Alcohol
                result.AppendSectionAnswer(GetAlcoholAllNo());
            }

            if (includedSections.Contains(ScreeningSectionDescriptor.SubstanceAbuse))
            {
                //Substance Abuse
                result.AppendSectionAnswer(new ScreeningSectionResult
                {
                    ScreeningSectionID = ScreeningSectionDescriptor.SubstanceAbuse,
                    AnswerValue = 0

                });
                result.FindSectionByID(ScreeningSectionDescriptor.SubstanceAbuse)
                    .AppendQuestionAnswer(new ScreeningSectionQuestionResult
                    {
                        QuestionID = 1,
                        AnswerValue = 0
                    });
            }
            if (includedSections.Contains(ScreeningSectionDescriptor.PartnerViolence))
            {
                //Partener Violence
                result.AppendSectionAnswer(new ScreeningSectionResult
                {
                    ScreeningSectionID = ScreeningSectionDescriptor.PartnerViolence,
                    AnswerValue = 0

                });
            }
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

        #region DrugOfChoice

        public static ScreeningResult WithDrugOfChoicePrimaryAndSecondary(this ScreeningResult result)
        {
            var section = new ScreeningSectionResult(ScreeningSectionDescriptor.DrugOfChoice, 1);
            section
                .AppendQuestionAnswer(new ScreeningSectionQuestionResult(DrugOfChoiceDescriptor.PrimaryQuestionId, 2))
                .AppendQuestionAnswer(new ScreeningSectionQuestionResult(DrugOfChoiceDescriptor.SecondaryQuestionId, 0));

            result.AppendSectionAnswer(section);

            return result;
        }

        public static ScreeningResult WithDrugOfChoiceAllAnswers(this ScreeningResult result)
        {
            var section = new ScreeningSectionResult(ScreeningSectionDescriptor.DrugOfChoice, 1);
            section
                .AppendQuestionAnswer(new ScreeningSectionQuestionResult(DrugOfChoiceDescriptor.PrimaryQuestionId, 2))
                .AppendQuestionAnswer(new ScreeningSectionQuestionResult(DrugOfChoiceDescriptor.SecondaryQuestionId, 4))
                .AppendQuestionAnswer(new ScreeningSectionQuestionResult(DrugOfChoiceDescriptor.TertiaryQuestionId, 8));


            result.AppendSectionAnswer(section);

            return result;
        }

        #endregion
    }
}
