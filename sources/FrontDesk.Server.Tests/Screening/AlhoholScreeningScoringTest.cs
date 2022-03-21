using System;
using FrontDesk.Server.Screening;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontDesk.Server.Tests.Screening
{


    /// <summary>
    ///This is a test class for AlhoholScreeningScoringTest and is intended
    ///to contain all AlhoholScreeningScoringTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AlhoholScreeningScoringTest : BaseScreeningScoringTest
    {
        public ScreeningSection GetSectionInfo()
        {
            return ScreeningInfo.FindSectionByID(ScreeningSectionDescriptor.Alcohol);
        }

        protected ScreeningSectionResult GetNegativeResult(int patternID)
        {
            ScreeningSectionResult result = new ScreeningSectionResult();
            result.ScreeningSectionID = "CAGE";

            if (patternID == 0)
            {
 				result.AnswerValue = 0;
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0)); //No for section
            }
            else if (patternID == 1)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 1)); //Yes on main question
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));

            }
            return result;
        }
        protected ScreeningSectionResult GetAtRiskResult(int patternID)
        {
            ScreeningSectionResult result = new ScreeningSectionResult();
            result.ScreeningSectionID = "CAGE";
 			result.AnswerValue = 1; //Yes for section
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 1)); //Yes for section

            if (patternID == 0)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
            }
            else if (patternID == 1)
            {
               
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));

            }
            return result;
        }
        protected ScreeningSectionResult GetProblemResult(int patternID)
        {
            ScreeningSectionResult result = new ScreeningSectionResult();
            result.ScreeningSectionID = "CAGE";
            result.AnswerValue = 1; //Yes for section
			result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 1)); //Yes for section

            if (patternID == 0)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 1));
            }
            else if (patternID == 1)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));

            }
            return result;
        }

        protected ScreeningSectionResult GetDependenceResult(int patternID)
        {
            ScreeningSectionResult result = new ScreeningSectionResult();
            result.ScreeningSectionID = "CAGE";
            result.AnswerValue = 1; //Yes for section
			result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 1)); //Yes for section

            if (patternID == 0)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 1));
            }
            else if (patternID == 1)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 1));

            }
            return result;
        }

        /// <summary>
        ///A test for GetScoreLevel
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FrontDesk.Server.dll")]
        public void GetScoreLevelTest()
        {
            int patternID = 0;
            AlhoholScreeningScoring target;
            ScreeningScoreLevel actual;
            int expected;
         
            for (patternID = 0; patternID < 2; patternID++)
            {
                expected = 0;
                target = new AlhoholScreeningScoring(GetNegativeResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.ScoreLevel;
                Assert.IsNotNull(actual);
                Assert.AreEqual(expected, actual.ScoreLevel);
            }


            for (patternID = 0; patternID < 2; patternID++)
            {
                expected = 1;
                target = new AlhoholScreeningScoring(GetAtRiskResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.ScoreLevel;
                Assert.IsNotNull(actual);
                Assert.AreEqual<int>(expected, actual.ScoreLevel);
            }

            for (patternID = 0; patternID < 2; patternID++)
            {
                expected = 2;
                target = new AlhoholScreeningScoring(GetProblemResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.ScoreLevel;
                Assert.IsNotNull(actual);
                Assert.AreEqual<int>(expected, actual.ScoreLevel);
            }


            for (patternID = 0; patternID < 2; patternID++)
            {
                expected = 3;
                target = new AlhoholScreeningScoring(GetDependenceResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.ScoreLevel;
                Assert.IsNotNull(actual);
                Assert.AreEqual<int>(expected, actual.ScoreLevel);
            }


        }

        /// <summary>
        ///A test for GetScore
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FrontDesk.Server.dll")]
        public void GetScoreTest()
        {
            int patternID = 0;
            AlhoholScreeningScoring target;
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;

            for (patternID = 0; patternID < 2; patternID++)
            {
                expected = 0;
                target = new AlhoholScreeningScoring(GetNegativeResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.Score;
                Assert.AreEqual<int>(expected.Value, actual.Value);
            }


            for (patternID = 0; patternID < 2; patternID++)
            {
                expected = 1;
                target = new AlhoholScreeningScoring(GetAtRiskResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.Score;
                Assert.AreEqual<int>(expected.Value, actual.Value);
            }

            for (patternID = 0; patternID < 2; patternID++)
            {
                expected = 2;
                target = new AlhoholScreeningScoring(GetProblemResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.Score;
                Assert.AreEqual<int>(expected.Value, actual.Value);
            }


            for (patternID = 0; patternID < 2; patternID++)
            {
                if (patternID == 0) expected = 3;
                else if (patternID == 1) expected = 4;

                target = new AlhoholScreeningScoring(GetDependenceResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.Score;
                Assert.AreEqual<int>(expected.Value, actual.Value);
            }
            
           
          
        }
    }
}
