using System;
using FrontDesk.Server.Screening;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace FrontDesk.Server.Tests.Screening
{


    /// <summary>
    ///This is a test class for DepressionScreeningScoringTest and is intended
    ///to contain all DepressionScreeningScoringTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DepressionScreeningScoringTest : BaseScreeningScoringTest
    {

        public ScreeningSection GetSectionInfo()
        {
            return ScreeningInfo.FindSectionByID(ScreeningSectionDescriptor.Depression);
        }

        #region helpers

        protected ScreeningSectionResult GetNoDepressionResult(int patternID)
        {
            ScreeningSectionResult result = new ScreeningSectionResult();
            result.ScreeningSectionID = "PHQ-9";
            if (patternID == 0)
            {
                result.AnswerValue = 0;
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
            }
            else
            {
                result.AnswerValue = 1;
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));


                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 0));


                if (patternID == 1)
                {
                    result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 0));
                }
                else if (patternID == 2)
                {
                    result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 1));
                }
                else if (patternID == 3)
                {
                    result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 3));
                }
                else if (patternID == 4)
                {
                    result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 0));
                }



            }
            return result;
        }
        protected ScreeningSectionResult GetMinimalDepressionResult(int patternID)
        {
            ScreeningSectionResult result = new ScreeningSectionResult();
            result.ScreeningSectionID = "PHQ-9";
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
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 0));

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
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 2));
            }
            else if (patternID == 2)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 0));
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
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 0));
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
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 0));
            }
            return result;
        }

        protected ScreeningSectionResult GetMildDepressionResult(int patternID)
        {
            ScreeningSectionResult result = new ScreeningSectionResult();
            result.ScreeningSectionID = "PHQ-9";
            result.AnswerValue = 1; //Yes for section

            if (patternID == 0)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 2));

            }
            else if (patternID == 1)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 2));
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
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 0));
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
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 3));
            }
            else if (patternID == 4)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 3));
            }
            return result;
        }

        protected ScreeningSectionResult GetModerateDepressionResult(int patternID)
        {
            ScreeningSectionResult result = new ScreeningSectionResult();
            result.ScreeningSectionID = "PHQ-9";
            result.AnswerValue = 1;

            if (patternID == 0)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 2));

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
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 1));
            }
            else if (patternID == 2)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 3));
            }
            else if (patternID == 3)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 0));
            }
            else if (patternID == 4)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 2));
            }
            return result;
        }

        protected ScreeningSectionResult GetModeratelySevereDepressionResult(int patternID)
        {
            ScreeningSectionResult result = new ScreeningSectionResult();
            result.ScreeningSectionID = "PHQ-9";
            result.AnswerValue = 1;

            if (patternID == 0)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 2));

            }
            else if (patternID == 1)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 3));
            }
            else if (patternID == 2)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 3));
            }
            else if (patternID == 3)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 0));
            }
            else if (patternID == 4)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 0));
            }
            return result;
        }

        protected ScreeningSectionResult GetSevereDepressionResult(int patternID)
        {
            ScreeningSectionResult result = new ScreeningSectionResult();
            result.ScreeningSectionID = "PHQ-9";
            result.AnswerValue = 1;

            if (patternID == 0)
            {
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 2));

            }
            else if (patternID == 1)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 3));
            }
            else if (patternID == 2)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 1));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 0));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 0));
            }
            else if (patternID == 3)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 2));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 3));
            }
            else if (patternID == 4)
            {

                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(1, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(2, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(3, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(4, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(5, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(6, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(7, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(8, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(9, 3));
                result.AppendQuestionAnswer(new ScreeningSectionQuestionResult(10, 3));
            }
            return result;
        }
        #endregion


        /// <summary>
        ///A test for GetScore
        ///</summary>
        [TestMethod()]
        public void GetScoreTest_NoDepression()
        {
            int patternID = 0;
            DepressionScreeningScoring target;
            Nullable<int> expected = new Nullable<int>();
            Nullable<int> actual;
            for (patternID = 0; patternID < 4; patternID++)
            {
                expected = patternID == 0? 0: 1;
                target = new DepressionScreeningScoring(GetNoDepressionResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.Score;
                Assert.AreEqual<int>(expected.Value, actual.Value);
            }
        }

        [TestMethod()]
        public void GetScoreTest_Minimal()
        {
            int patternID = 0;
            DepressionScreeningScoring target;
            Nullable<int> expected = new Nullable<int>();
            Nullable<int> actual;

            for (patternID = 0; patternID < 4; patternID++)
            {
                expected = 1 + patternID;
                target = new DepressionScreeningScoring(GetMinimalDepressionResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.Score;
                actual.Value.Should().Be(expected.Value, "pattern {0}", patternID);
            }
        }

        [TestMethod()]
        public void GetScoreTest_Mild()
        {
            int patternID = 0;
            DepressionScreeningScoring target;
            Nullable<int> expected = new Nullable<int>();
            Nullable<int> actual;

            for (patternID = 0; patternID < 5; patternID++)
            {
                expected = 5 + patternID;
                target = new DepressionScreeningScoring(GetMildDepressionResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.Score;
                Assert.AreEqual<int>(expected.Value, actual.Value);
            }
        }

        [TestMethod()]
        public void GetScoreTest_Moderate()
        {
            int patternID = 0;
            DepressionScreeningScoring target;
            Nullable<int> expected = new Nullable<int>();
            Nullable<int> actual;

            for (patternID = 0; patternID < 5; patternID++)
            {
                expected = 10 + patternID;
                target = new DepressionScreeningScoring(GetModerateDepressionResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.Score;
                actual.Value.Should().Be(expected.Value, "pattern {0}", patternID);
            }
        }

        [TestMethod()]
        public void GetScoreTest_ModeratelySevere()
        {
            int patternID = 0;
            DepressionScreeningScoring target;
            Nullable<int> expected = new Nullable<int>();
            Nullable<int> actual;

            for (patternID = 0; patternID < 5; patternID++)
            {
                expected = 15 + patternID;
                target = new DepressionScreeningScoring(GetModeratelySevereDepressionResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.Score;
                Assert.AreEqual<int>(expected.Value, actual.Value);
            }

        }

        [TestMethod()]
        public void GetScoreTest_Severe()
        {
            int patternID = 0;
            DepressionScreeningScoring target;
            Nullable<int> expected = new Nullable<int>();
            Nullable<int> actual;
            
            for (patternID = 0; patternID < 2; patternID++)
            {
                expected = 20 + patternID;
                target = new DepressionScreeningScoring(GetSevereDepressionResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.Score;
                Assert.AreEqual<int>(expected.Value, actual.Value);
            }


        }

        [TestMethod()]
        public void GetScoreLevelTest_NoDepression()
        {
            int patternID;
            DepressionScreeningScoring target;
            ScreeningScoreLevel actual;
            int? expected;

            for (patternID = 0; patternID < 4; patternID++)
            {
                expected = 0;
                target = new DepressionScreeningScoring(GetNoDepressionResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.ScoreLevel;
                Assert.AreEqual<int>(expected.Value, actual.ScoreLevel);
            }
        }

        [TestMethod()]
        public void GetScoreLevelTest_Minimal()
        {
            int patternID;
            DepressionScreeningScoring target;
            ScreeningScoreLevel actual;
            int? expected;

            for (patternID = 0; patternID < 4; patternID++)
            {
                expected = 0;
                target = new DepressionScreeningScoring(GetMinimalDepressionResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.ScoreLevel;
                Assert.AreEqual<int>(expected.Value, actual.ScoreLevel, "Minimal depression result failed");
            }
        }

        [TestMethod()]
        public void GetScoreLevelTest_Mild()
        {
            int patternID;
            DepressionScreeningScoring target;
            ScreeningScoreLevel actual;
            int? expected;

            for (patternID = 0; patternID < 5; patternID++)
            {
                expected = 2;
                target = new DepressionScreeningScoring(GetMildDepressionResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.ScoreLevel;
                Assert.AreEqual<int>(expected.Value, actual.ScoreLevel, "Mild depression result failed");
            }
        }

        [TestMethod()]
        public void GetScoreLevelTest_Moderate()
        {
            int patternID;
            DepressionScreeningScoring target;
            ScreeningScoreLevel actual;
            int? expected;

            for (patternID = 0; patternID < 5; patternID++)
            {
                expected = 3;
                target = new DepressionScreeningScoring(GetModerateDepressionResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.ScoreLevel;
                actual.ScoreLevel.Should().Be(expected.Value, "pattern {0}", patternID);
            }
        }

        [TestMethod()]
        public void GetScoreLevelTest_ModeratelySevere()
        {
            int patternID;
            DepressionScreeningScoring target;
            ScreeningScoreLevel actual;
            int? expected;


            for (patternID = 0; patternID < 5; patternID++)
            {
                expected = 4;
                target = new DepressionScreeningScoring(GetModeratelySevereDepressionResult(patternID), GetSectionInfo(), repositoryMock.Object);
                actual = target.ScoreLevel;
                Assert.AreEqual<int>(expected.Value, actual.ScoreLevel, "Moderately severe depression result failed");
            }
        }

        [TestMethod()]
        public void GetScoreLevelTest_Severe()
        {
            int patternID;
            DepressionScreeningScoring target;
            ScreeningScoreLevel actual;
            int? expected;
            
            for (patternID = 0; patternID < 5; patternID++)
            {
                expected = 5;
                target = new DepressionScreeningScoring(GetSevereDepressionResult(patternID), GetSectionInfo(), repositoryMock.Object);
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

            DepressionScreeningScoring target;
            ScreeningScoreLevel actual;
            var sectionResult = new ScreeningSectionResult { ScreeningSectionID = "PHQ-9" };
            int? expected;

            expected = 0;
            target = new DepressionScreeningScoring(sectionResult, GetSectionInfo(), repositoryMock.Object);
            actual = target.GetScoreLevel(0);
            Assert.AreEqual<int>(expected.Value, actual.ScoreLevel, "Score for 0 failed");


            expected = 3;
            target = new DepressionScreeningScoring(sectionResult, GetSectionInfo(), repositoryMock.Object);
            actual = target.GetScoreLevel(13);
            Assert.AreEqual<int>(expected.Value, actual.ScoreLevel, "Score for 13 failed");


            expected = 5;
            target = new DepressionScreeningScoring(sectionResult, GetSectionInfo(), repositoryMock.Object);
            actual = target.GetScoreLevel(27);
            Assert.AreEqual<int>(expected.Value, actual.ScoreLevel, "Score for 27 failed");
        }
    }
}
