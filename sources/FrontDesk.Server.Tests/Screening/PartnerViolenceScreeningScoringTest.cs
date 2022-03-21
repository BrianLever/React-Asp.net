using System;
using FrontDesk.Server.Screening;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontDesk.Server.Tests.Screening
{
    
    
    /// <summary>
    ///This is a test class for PartnerViolenceScreeningScoringTest and is intended
    ///to contain all PartnerViolenceScreeningScoringTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PartnerViolenceScreeningScoringTest : BaseScreeningScoringTest
    {

        public ScreeningSection GetSectionInfo()
        {
            return ScreeningInfo.FindSectionByID(ScreeningSectionDescriptor.PartnerViolence);
        }

        protected ScreeningSectionResult GetNegativeResult(int patternID)
        {
            ScreeningSectionResult result = new ScreeningSectionResult();
            result.ScreeningSectionID = "HITS";
            if (patternID == 0)
            {
				result.AnswerValue = 0;
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0)); //No on main question
            }
            else if (patternID == 1)
            {
				result.AnswerValue = 1;
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 1)); //yes on main question
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 1));

            }
            else if (patternID == 2)
            {
				result.AnswerValue = 1;
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 1)); //yes on main question
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 5));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 2));

            }
            return result;
        }
        protected ScreeningSectionResult GetPositiveResult(int patternID)
        {
            ScreeningSectionResult result = new ScreeningSectionResult();
            result.ScreeningSectionID = "HITS";
			result.AnswerValue = 1; //Yes for section
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 1)); //yes on main question


            if (patternID == 0)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 5));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 1));
            }
            else if (patternID == 1)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 4));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 4));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 4));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 1));

            }
            else if (patternID == 2)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 5));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 5));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 5));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 5));

            }
            return result;
        }

        /// <summary>
        ///A test for GetScore
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FrontDesk.Server.dll")]
        public void GetScoreTest()
        {
             PartnerViolenceScreeningScoring target;
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
          
             expected = 0;
             target = new PartnerViolenceScreeningScoring(GetNegativeResult(0), GetSectionInfo(), repositoryMock.Object);
             actual = target.Score;
             Assert.AreEqual<int>(expected.Value, actual.Value);

             expected = 4;
             target = new PartnerViolenceScreeningScoring(GetNegativeResult(1), GetSectionInfo(), repositoryMock.Object);
             actual = target.Score;
             Assert.AreEqual<int>(expected.Value, actual.Value);

             expected = 10;
             target = new PartnerViolenceScreeningScoring(GetNegativeResult(2), GetSectionInfo(), repositoryMock.Object);
             actual = target.Score;
             Assert.AreEqual<int>(expected.Value, actual.Value);

             expected = 11;
             target = new PartnerViolenceScreeningScoring(GetPositiveResult(0), GetSectionInfo(), repositoryMock.Object);
             actual = target.Score;
             Assert.AreEqual<int>(expected.Value, actual.Value);

             expected = 13;
             target = new PartnerViolenceScreeningScoring(GetPositiveResult(1), GetSectionInfo(), repositoryMock.Object);
             actual = target.Score;
             Assert.AreEqual<int>(expected.Value, actual.Value);


             expected = 20;
             target = new PartnerViolenceScreeningScoring(GetPositiveResult(2), GetSectionInfo(), repositoryMock.Object);
             actual = target.Score;
             Assert.AreEqual<int>(expected.Value, actual.Value);
            
        }

        /// <summary>
        ///A test for GetScoreLevel
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FrontDesk.Server.dll")]
        public void GetScoreLevelTest()
        {
            PartnerViolenceScreeningScoring target;
            int expected;
            ScreeningScoreLevel actual;

            for (int patternID = 0; patternID < 3; patternID++)
            {
                expected = 0;
                target = new PartnerViolenceScreeningScoring(GetNegativeResult(patternID), GetSectionInfo(), repositoryMock.Object);

                actual = target.GetScoreLevel(target.Score);
                Assert.AreEqual(expected, actual.ScoreLevel);
            }

            for (int patternID = 0; patternID < 3; patternID++)
            {
                expected = 1;
                target = new PartnerViolenceScreeningScoring(GetPositiveResult(patternID), GetSectionInfo(), repositoryMock.Object);

                actual = target.GetScoreLevel(target.Score);
                Assert.AreEqual(expected, actual.ScoreLevel);
            }
        }
    }
}
