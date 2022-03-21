using System;
using FrontDesk.Server.Screening;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontDesk.Server.Tests.Screening
{


    /// <summary>
    ///This is a test class for SubstanceAbuseScreeningScoringTest and is intended
    ///to contain all SubstanceAbuseScreeningScoringTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SubstanceAbuseScreeningScoringTest : BaseScreeningScoringTest
    {
        public ScreeningSection GetSectionInfo()
        {
            return ScreeningInfo.FindSectionByID(ScreeningSectionDescriptor.SubstanceAbuse);
        }
        protected ScreeningSectionResult GetNegativeResult(int patternID)
        {
            ScreeningSectionResult result = new ScreeningSectionResult();
            result.ScreeningSectionID = "DAST";
            if (patternID == 0)
            {
                result.AnswerValue = 0; //No for section
				result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 0));
            }
            else if (patternID == 1)
            {
				result.AnswerValue = 0; //No for section
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 0));

            }
            else if (patternID == 3)
            {
				result.AnswerValue = 0; //No for section
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 0));

            }
            return result;
        }
        protected ScreeningSectionResult GetLowLevelResult(int patternID)
        {
            ScreeningSectionResult result = new ScreeningSectionResult();
            result.ScreeningSectionID = "DAST";
			result.AnswerValue = 1; //Yes for section
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 1)); //Yes for section

            if (patternID == 0)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 0));
            }
            else if (patternID == 1)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 0));

            }
            else if (patternID == 2)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 0));

            }
            return result;
        }

        protected ScreeningSectionResult GetModerateLevelResult(int patternID)
        {
            ScreeningSectionResult result = new ScreeningSectionResult();
            result.ScreeningSectionID = "DAST";
			result.AnswerValue = 1; //Yes for section
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 1)); //Yes for section

            if (patternID == 0)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 0));
            }
            else if (patternID == 1)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 1));

            }
            else if (patternID == 2)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 0));

            }
            return result;
        }

        protected ScreeningSectionResult GetSubstantialLevelResult(int patternID)
        {
            ScreeningSectionResult result = new ScreeningSectionResult();
            result.ScreeningSectionID = "DAST";
			result.AnswerValue = 1; //Yes for section
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 1)); //Yes for section

            if (patternID == 0)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 0));
            }
            else if (patternID == 1)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 0));

            }
            else if (patternID == 2)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 1));

            }
            return result;
        }

        protected ScreeningSectionResult GetSevereLevelResult(int patternID)
        {
            ScreeningSectionResult result = new ScreeningSectionResult();
            result.ScreeningSectionID = "DAST";
			result.AnswerValue = 1; //Yes for section
            result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 1)); //Yes for section

            if (patternID == 0)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 1));
            }
            else if (patternID == 1)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 1));

            }
            else if (patternID == 2)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 1));

            }
            return result;
        }


        [TestMethod()]
        [DeploymentItem("FrontDesk.Server.dll")]
        public void SubstanceAbuse_GetScoreLevel_Negative()
        {
            SubstanceAbuseScreeningScoring target;
            ScreeningScoreLevel actual;
            int expectedLevel = 0;
            int patternID = 0;

            for (patternID = 0; patternID < 3; patternID++)
            {
                target = new SubstanceAbuseScreeningScoring(GetNegativeResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.ScoreLevel;
                Assert.IsNotNull(actual);
                Assert.AreEqual(expectedLevel, actual.ScoreLevel);
            }
        }

        [TestMethod()]
        [DeploymentItem("FrontDesk.Server.dll")]
        public void SubstanceAbuse_GetScoreLevel_Low()
        {
            SubstanceAbuseScreeningScoring target;
            //Nullable<int> score = new Nullable<int>(); // TODO: Initialize to an appropriate value
            ScreeningScoreLevel actual;
            var expectedLevel = 1;
            int patternID = 0;

            for (patternID = 0; patternID < 3; patternID++)
            {
                target = new SubstanceAbuseScreeningScoring(GetLowLevelResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.ScoreLevel;
                Assert.IsNotNull(actual);
                Assert.AreEqual<int>(expectedLevel, actual.ScoreLevel);
            }
        }

        [TestMethod()]
        [DeploymentItem("FrontDesk.Server.dll")]
        public void SubstanceAbuse_GetScoreLevel_Moderate()
        {
            SubstanceAbuseScreeningScoring target;
            ScreeningScoreLevel actual;
            var expectedLevel = 2;
            int patternID = 0;

            for (patternID = 0; patternID < 3; patternID++)
            {

                target = new SubstanceAbuseScreeningScoring(GetModerateLevelResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.ScoreLevel;
                Assert.IsNotNull(actual);
                Assert.AreEqual<int>(expectedLevel, actual.ScoreLevel);
            }
        }

        [TestMethod()]
        [DeploymentItem("FrontDesk.Server.dll")]
        public void SubstanceAbuse_GetScoreLevel_Substantial()
        {
            SubstanceAbuseScreeningScoring target;
            ScreeningScoreLevel actual;
            var expectedLevel = 3;
            int patternID = 0;

            for (patternID = 0; patternID < 3; patternID++)
            {

                target = new SubstanceAbuseScreeningScoring(GetSubstantialLevelResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.ScoreLevel;
                Assert.IsNotNull(actual);
                Assert.AreEqual<int>(expectedLevel, actual.ScoreLevel);
            }
        }


        /// <summary>
        ///A test for GetScoreLevel
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FrontDesk.Server.dll")]
        public void SubstanceAbuse_GetScoreLevel_Severe()
        {
            int patternID = 0;
            
            SubstanceAbuseScreeningScoring target;
            ScreeningScoreLevel actual;
            int expectedLevel = 4;
            
         
            for (patternID = 0; patternID < 3; patternID++)
            {
                target = new SubstanceAbuseScreeningScoring(GetSevereLevelResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.ScoreLevel;
                Assert.IsNotNull(actual);
                Assert.AreEqual<int>(expectedLevel, actual.ScoreLevel);
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
            SubstanceAbuseScreeningScoring target;
            Nullable<int> expected = new Nullable<int>();
            Nullable<int> actual;

            for (patternID = 0; patternID < 3; patternID++)
            {
                expected = 0;
                target = new SubstanceAbuseScreeningScoring(GetNegativeResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.Score;
                Assert.IsNotNull(actual);
                Assert.AreEqual(expected.Value, actual.Value);
            }

            for (patternID = 0; patternID < 3; patternID++)
            {
                expected = 1 + patternID;
                expected = expected > 2 ? 2 : expected;

                target = new SubstanceAbuseScreeningScoring(GetLowLevelResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.Score;
                Assert.IsNotNull(actual);
                Assert.AreEqual<int>(expected.Value, actual.Value);
            }
            for (patternID = 0; patternID < 3; patternID++)
            {
                expected = 3 + patternID;
                expected = expected > 5 ? 5 : expected;

                target = new SubstanceAbuseScreeningScoring(GetModerateLevelResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.Score;
                Assert.IsNotNull(actual);
                Assert.AreEqual<int>(expected.Value, actual.Value);
            }
            for (patternID = 0; patternID < 3; patternID++)
            {
                expected = 6 + patternID;
                expected = expected > 8 ? 8 : expected;

                target = new SubstanceAbuseScreeningScoring(GetSubstantialLevelResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.Score;
                Assert.IsNotNull(actual);
                Assert.AreEqual<int>(expected.Value, actual.Value);
            }
            for (patternID = 0; patternID < 3; patternID++)
            {
                expected = 9 + patternID;
                expected = expected > 10 ? 10 : expected;

                target = new SubstanceAbuseScreeningScoring(GetSevereLevelResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.Score;
                Assert.IsNotNull(actual);
                Assert.AreEqual<int>(expected.Value, actual.Value);
            }
        }
    }
}
