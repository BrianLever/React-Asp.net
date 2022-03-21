using System;
using System.Linq;
using FluentAssertions;
using FrontDesk;
using FrontDesk.Server.Tests.MotherObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPMS.Common.Models.Factory;

namespace RPMS.Data.CareBridge.Tests.Models.Factory
{
    [TestClass]
    [TestCategory("CareBridge_NextGen")]
    public class ScreeningResultRecordFactoryTest
    {
        
        [TestMethod]
        public void Create_NotEmpty()
        {
            var actual = ScreeningResultRecordFactory.Create(1, 2, ScreeningResultMotherObject.GetAllNoAnswers(), ScreeningInfoMotherObject.GetFullScreening());

            actual.Should().NotBeNull();
        }

        [TestMethod]
        public void Create_EhrPatientId_Mapped()
        {
            var ehrPatientId = 1011;
            var actual = ScreeningResultRecordFactory.Create(ehrPatientId, 2, ScreeningResultMotherObject.GetAllNoAnswers(), ScreeningInfoMotherObject.GetFullScreening());

            actual.PatientID.Should().Be(ehrPatientId);
        }

        [TestMethod]
        public void Create_EhrVisitId_Mapped()
        {
            var ehrVisitId = 20022;
            var actual = ScreeningResultRecordFactory.Create(1, ehrVisitId, ScreeningResultMotherObject.GetAllNoAnswers(), ScreeningInfoMotherObject.GetFullScreening());

            actual.VisitID.Should().Be(ehrVisitId);
        }

        [TestMethod]
        public void Create_ResultID_And_Date_Mapper()
        {
            var screening = ScreeningResultMotherObject.CreateANDREA();

            var actual = ScreeningResultRecordFactory.Create(1, 2, ScreeningResultMotherObject.GetAllNoAnswers(), ScreeningInfoMotherObject.GetFullScreening());

            actual.ScreendoxRecordNo.Should().Be(screening.ID, "ScreendoxRecordNo should match");
            actual.ScreeningDate.Should().Be(screening.CreatedDate.Date, "ScreeningDate should match");
        }


        [TestMethod]
        public void Create_Sections_NotEmpty()
        {
            var actual = ScreeningResultRecordFactory.Create(1, 2, ScreeningResultMotherObject.GetAllNoAnswers(), ScreeningInfoMotherObject.GetFullScreening());

            actual.Sections.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public void Create_Sections_IncludesAll()
        {
            var actual = ScreeningResultRecordFactory.Create(1, 2, ScreeningResultMotherObject.GetAllNoAnswers(), ScreeningInfoMotherObject.GetFullScreening());

            actual.Sections.Count.Should().Be(6);
        }

        [TestMethod]
        public void Create_Section_HasScoreAndLevel()
        {
            var result = ScreeningResultMotherObject.CreateANDREA();
            result.AppendSectionAnswer(ScreeningResultMotherObject.GetDepressionHurtYouselfSeveralDaysPlusModerate());
            
            var actual = ScreeningResultRecordFactory.Create(1, 2, result, ScreeningInfoMotherObject.GetFullScreening()).Sections.First();

            actual.ScreeningSectionID.Should().Be(ScreeningSectionDescriptor.Depression);
            actual.Score.Should().Be(11);
            actual.ScoreLevel.Should().Be(2);
            actual.ScoreLevelLabel.Should().Be("MODERATE depression severity");
        }

        [TestMethod]
        public void Create_Section_HasAllAnswers()
        {
            var result = ScreeningResultMotherObject.CreateANDREA();
            result.AppendSectionAnswer(ScreeningResultMotherObject.GetDepressionHurtYouselfSeveralDaysPlusModerate());

            var actual = ScreeningResultRecordFactory.Create(1, 2, result, ScreeningInfoMotherObject.GetFullScreening()).Sections.First();

            actual.Answers.Count.Should().Be(10);
        }


        [TestMethod]
        public void Create_Section_Answer_Mapped()
        {
            var result = ScreeningResultMotherObject.CreateANDREA();
            result.AppendSectionAnswer(ScreeningResultMotherObject.GetDepressionHurtYouselfSeveralDaysPlusModerate());

            var section = ScreeningResultRecordFactory.Create(1, 2, result, ScreeningInfoMotherObject.GetFullScreening()).Sections.First();

            var answer = section.Answers[8];

            answer.QuestionID.Should().Be(9);
            answer.AnswerValue.Should().Be(DepressionQuestionsDescriptor.SeveralDaysAnswer);
        }


        [TestMethod]
        public void Create_Section_Answer_HasQuestionText()
        {
            var result = ScreeningResultMotherObject.CreateANDREA();
            result.AppendSectionAnswer(ScreeningResultMotherObject.GetDepressionHurtYouselfSeveralDaysPlusModerate());

            var section = ScreeningResultRecordFactory.Create(1, 2, result, ScreeningInfoMotherObject.GetFullScreening()).Sections.First();

            var answer = section.Answers[8];

            answer.QuestionText.Should().NotBeEmpty();
        }

        [TestMethod]
        public void Create_Section_Answer_HasAnswerText()
        {
            var result = ScreeningResultMotherObject.CreateANDREA();
            result.AppendSectionAnswer(ScreeningResultMotherObject.GetDepressionHurtYouselfSeveralDaysPlusModerate());

            var section = ScreeningResultRecordFactory.Create(1, 2, result, ScreeningInfoMotherObject.GetFullScreening()).Sections.First();

            var answer = section.Answers[8];

            answer.AnswerText.Should().Be("Several days");
        }
    }
}
