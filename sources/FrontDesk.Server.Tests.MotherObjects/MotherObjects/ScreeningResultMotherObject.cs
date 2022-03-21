using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Tests.MotherObjects
{
    public static class ScreeningResultMotherObject
    {
        #region screeing result

        public static ScreeningResult GetEmptyGarerd()
        {
            return CreateGARERD();
        }

        public static ScreeningResult GetAllNoAnswers()
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

        
        public static ScreeningResult GetAllNoAnswersForSections(params string[] includedSections)
        {
            var result = CreateANDREA();

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

        public static ScreeningSectionResult GetAlcoholAllNo()
        {
            return new ScreeningSectionResult
            {
                ScreeningSectionID = ScreeningSectionDescriptor.Alcohol,
                AnswerValue = 0,
                Score = 0
            };
        }

        public static ScreeningSectionResult GetAlcoholAllYes()
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
        public static ScreeningSectionResult GetViolenceHurtNever()
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
        public static ScreeningSectionResult GetViolenceHurtRarely()
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
        public static ScreeningSectionResult GetDepressionHurtYouselfNotAtAll()
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


        public static ScreeningSectionResult GetDepressionHurtYouselfSeveralDays()
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


        public static ScreeningSectionResult GetDepressionHurtYouselfSeveralDaysPlusModerate()
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

        public static ScreeningSectionResult GetDepressionHurtYouseNotAtAllPlusModerate()
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


        #region Patients


        public static ScreeningResult CreateANDREA()
        {
            return new ScreeningResult
            {
                ID = 16009,
                LastName = "DEMO",
                FirstName = "ANDREA",
                MiddleName = "Chris",
                Birthday = new DateTime(1972, 2, 22),
                StreetAddress = "6385 WEST OTTAWA",
                Phone = "(775) 219-8620",
                StateID = "NV",
                ZipCode = "89436",
                City = "SPANISH SPRINGS",
                LocationID = 1,
            };
        }

        public static ScreeningResult CreateGARERD()
        {
            return new ScreeningResult
            {
                LastName = "GARERD",
                FirstName = "ADELA",
                MiddleName = "Don Sr.",
                Birthday = new DateTime(1965, 9, 9),
                StreetAddress = "Fake Street",
                Phone = "111-111-11111",
                StateID = "CA",
                ZipCode = "92061",
                City = "AMADOR COUNTY",
                LocationID = 1

            };
        }

        #endregion

    }
}
