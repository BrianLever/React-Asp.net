using System;
using FrontDesk.Server.Screening;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace FrontDesk.Server.Tests.Screening
{
    /// <summary>
    ///This is a test class for AnxietyScreeningScoringTest and is intended
    ///to contain all AnxietyScreeningScoringTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AnxietyScreeningScoringTest : BaseScreeningScoringTest
    {

        public ScreeningSection GetSectionInfo()
        {
            return ScreeningInfo.FindSectionByID(ScreeningSectionDescriptor.Anxiety);
        }

        #region helpers

        protected ScreeningSectionResult GetNoAnxietyResult(int patternID)
        {
            ScreeningSectionResult result = new ScreeningSectionResult();
            result.ScreeningSectionID = "GAD-7";
            if (patternID == 0)
            {
                result.AnswerValue = 0;
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
            }
            else if (patternID == 1)
            {
                result.AnswerValue = 0;
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 1));
            }
            else if (patternID == 2)
            {
                result.AnswerValue = 0;
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 2));
            }
            else if (patternID == 3)
            {
                result.AnswerValue = 0;
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 3));
            }
            else
            {
                result.AnswerValue = 1;
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 3));

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 0));
            }
            return result;
        }
        protected ScreeningSectionResult GetMinimalAnxietyResult(int patternID)
        {
            ScreeningSectionResult result = new ScreeningSectionResult();
            result.ScreeningSectionID = "GAD-7";
            result.AnswerValue = 1; //Yes for section

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
            }
            else if (patternID == 1)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 2));
            }
            else if (patternID == 2)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 0));
            }
            else if (patternID == 3)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 2));
            }
            else if (patternID == 4)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 0));
            }
            return result;
        }

        protected ScreeningSectionResult GetMildAmxietyResult(int patternID)
        {
            ScreeningSectionResult result = new ScreeningSectionResult();
            result.ScreeningSectionID = "GAD-7";
            result.AnswerValue = 1; //Yes for section

            if (patternID == 0)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 2));
            }
            else if (patternID == 1)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 2));
            }
            else if (patternID == 2)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 0));
            }
            else if (patternID == 3)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 3));
            }
            else if (patternID == 4)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 3));
            }
            return result;
        }

        protected ScreeningSectionResult GetModerateAnxietyResult(int patternID)
        {
            ScreeningSectionResult result = new ScreeningSectionResult();
            result.ScreeningSectionID = "GAD-7";
            result.AnswerValue = 1;

            if (patternID == 0)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 2));

            }
            else if (patternID == 1)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 1));
            }
            else if (patternID == 2)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 3));
            }
            else if (patternID == 3)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 0));
            }
            else if (patternID == 4)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 2));
            }
            return result;
        }

        protected ScreeningSectionResult GetSevereAnxietyResult(int patternID)
        {
            ScreeningSectionResult result = new ScreeningSectionResult();
            result.ScreeningSectionID = "GAD-7";
            result.AnswerValue = 1;

            if (patternID == 0)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 2));
            }
            else if (patternID == 1)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 3));
            }
            else if (patternID == 2)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 2));
            }
            else if (patternID == 3)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 3));
            }
            else if (patternID == 4)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 3));
            }
            return result;
        }

        #endregion


        /// <summary>
        ///A test for GetScore
        ///</summary>
        [TestMethod()]
        public void GetScoreTest_None()
        {
            AnxietyScreeningScoring target;
            int? actual;
            for (var patternID = 0; patternID < 4; patternID++)
            {
                var expected = patternID;
                target = new AnxietyScreeningScoring(GetNoAnxietyResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.Score;
                actual.Value.Should().Be(expected, "pattern {0}", patternID);
            }
        }

        [TestMethod()]
        public void GetScoreTest_Minimal()
        {
            int patternID = 0;
            AnxietyScreeningScoring target;
            int? expected = new Nullable<int>();
            int? actual;

            for (patternID = 0; patternID < 4; patternID++)
            {
                expected = 1 + patternID;
                target = new AnxietyScreeningScoring(GetMinimalAnxietyResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.Score;
                actual.Value.Should().Be(expected.Value, "pattern {0}", patternID);
            }
        }

        [TestMethod()]
        public void GetScoreTest_Mild()
        {
            int patternID = 0;
            AnxietyScreeningScoring target;
            int? expected = new Nullable<int>();
            int? actual;

            for (patternID = 0; patternID < 5; patternID++)
            {
                expected = 5 + patternID;
                target = new AnxietyScreeningScoring(GetMildAmxietyResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.Score;
                actual.Value.Should().Be(expected.Value, "pattern {0}", patternID);
            }
        }

        [TestMethod()]
        public void GetScoreTest_Moderate()
        {
            int patternID = 0;
            AnxietyScreeningScoring target;
            int? expected;
            int? actual;

            for (patternID = 0; patternID < 5; patternID++)
            {
                expected = 10 + patternID;
                target = new AnxietyScreeningScoring(GetModerateAnxietyResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.Score;
                actual.Value.Should().Be(expected.Value, "pattern {0}", patternID);
            }
        }

        [TestMethod()]
        public void GetScoreTest_Severe()
        {
            int patternID = 0;
            AnxietyScreeningScoring target;
            int? actual;

            for (patternID = 0; patternID < 2; patternID++)
            {
                var expected = 15 + patternID;
                target = new AnxietyScreeningScoring(GetSevereAnxietyResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.Score;
                Assert.AreEqual<int>(expected, actual.Value);
            }


        }

        [TestMethod()]
        public void GetScoreLevelTest_NoAnxiety()
        {
            int patternID;
            AnxietyScreeningScoring target;
            ScreeningScoreLevel actual;
            int? expected;

            for (patternID = 0; patternID < 4; patternID++)
            {
                expected = 0;
                target = new AnxietyScreeningScoring(GetMinimalAnxietyResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.ScoreLevel;
                Assert.AreEqual<int>(expected.Value, actual.ScoreLevel);
            }
        }

        [TestMethod()]
        public void GetScoreLevelTest_Minimal()
        {
            int patternID;
            AnxietyScreeningScoring target;
            ScreeningScoreLevel actual;
            int? expected;

            for (patternID = 0; patternID < 4; patternID++)
            {
                expected = 0;
                target = new AnxietyScreeningScoring(GetMinimalAnxietyResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.ScoreLevel;
                Assert.AreEqual<int>(expected.Value, actual.ScoreLevel, "Minimal depression result failed");
            }
        }

        [TestMethod()]
        public void GetScoreLevelTest_Mild()
        {
            int patternID;
            AnxietyScreeningScoring target;
            ScreeningScoreLevel actual;
            int? expected;

            for (patternID = 0; patternID < 5; patternID++)
            {
                expected = 1;
                target = new AnxietyScreeningScoring(GetMildAmxietyResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.ScoreLevel;
                Assert.AreEqual<int>(expected.Value, actual.ScoreLevel, "Mild depression result failed");
            }
        }

        [TestMethod()]
        public void GetScoreLevelTest_Moderate()
        {
            int patternID;
            AnxietyScreeningScoring target;
            ScreeningScoreLevel actual;
            int? expected;

            for (patternID = 0; patternID < 5; patternID++)
            {
                expected = 2;
                target = new AnxietyScreeningScoring(GetModerateAnxietyResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.ScoreLevel;
                actual.ScoreLevel.Should().Be(expected.Value, "pattern {0}", patternID);
            }
        }

        [TestMethod()]
        public void GetScoreLevelTest_Severe()
        {
            int patternID;
            AnxietyScreeningScoring target;
            ScreeningScoreLevel actual;
            int? expected;

            for (patternID = 0; patternID < 5; patternID++)
            {
                expected = 3;
                target = new AnxietyScreeningScoring(GetSevereAnxietyResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.ScoreLevel;
                Assert.AreEqual<int>(expected.Value, actual.ScoreLevel, "Failed pattern {0}. expected <{1}>, actual <{2}>", patternID, expected.Value, actual.ScoreLevel);
            }
        }



        /// <summary>
        ///A test for GetScoreLevel
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FrontDesk.Server.dll")]
        public void Can_calculate_scoreLevel()
        {

            AnxietyScreeningScoring target;
            ScreeningScoreLevel actual;
            var sectionResult = new ScreeningSectionResult { ScreeningSectionID = "GAD-7" };
            int? expected;

            expected = 0;
            target = new AnxietyScreeningScoring(sectionResult, GetSectionInfo(), repositoryMock.Object);
            actual = target.GetScoreLevel(0);
            Assert.AreEqual<int>(expected.Value, actual.ScoreLevel, "Score for 0 failed");


            expected = 2;
            target = new AnxietyScreeningScoring(sectionResult, GetSectionInfo(), repositoryMock.Object);
            actual = target.GetScoreLevel(13);
            Assert.AreEqual<int>(expected.Value, actual.ScoreLevel, "Score for 13 failed");


            expected = 3;
            target = new AnxietyScreeningScoring(sectionResult, GetSectionInfo(), repositoryMock.Object);
            actual = target.GetScoreLevel(20);
            Assert.AreEqual<int>(expected.Value, actual.ScoreLevel, "Score for 20 failed");
        }
    }
}
