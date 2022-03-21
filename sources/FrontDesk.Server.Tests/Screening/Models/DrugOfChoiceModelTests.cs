using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Tests.MotherObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontDesk.Server.Tests.Screening.Models
{
    [TestClass]
    public class DrugOfChoiceModelTests
    {

        protected DrugOfChoiceModel Sut(ScreeningResult result)
        {
            return new DrugOfChoiceModel(result);
        }

        [TestMethod]
        public void Should_Get_PrimaryDrugOfChoice_When_NotEmpty()
        {
            var screening = ScreeningResultMotherObject.GetAllYesAnswers().WithDrugOfChoicePrimaryAndSecondary();
            var sut = Sut(screening);

            sut.Primary.Should().Be(2);
        }

        [TestMethod]
        public void Should_Get_Zero_SecondaryDrugOfChoice_When_NotEmpty()
        {
            var screening = ScreeningResultMotherObject.GetAllYesAnswers().WithDrugOfChoicePrimaryAndSecondary();
            var sut = Sut(screening);

            sut.Secondary.Should().Be(0);
        }

        [TestMethod]
        public void Should_Get_SecondaryDrugOfChoice_When_NotEmpty()
        {
            var screening = ScreeningResultMotherObject.GetAllYesAnswers().WithDrugOfChoiceAllAnswers();
            var sut = Sut(screening);

            sut.Secondary.Should().Be(4);
        }


        [TestMethod]
        public void Should_Get_Zero_TertiaryDrugOfChoice_When_NotEmpty()
        {
            var screening = ScreeningResultMotherObject.GetAllYesAnswers().WithDrugOfChoicePrimaryAndSecondary();
            var sut = Sut(screening);

            sut.Tertiary.Should().Be(0);
        }


        [TestMethod]
        public void Should_Get_TertiaryDrugOfChoice_When_NotEmpty()
        {
            var screening = ScreeningResultMotherObject.GetAllYesAnswers().WithDrugOfChoiceAllAnswers();
            var sut = Sut(screening);

            sut.Tertiary.Should().Be(8);
        }


        [TestMethod]
        public void Should_Get_Primary_When_Empty()
        {
            var screening = ScreeningResultMotherObject.GetAllYesAnswers();
            var sut = Sut(screening);

            sut.Primary.Should().Be(0);
        }


        [TestMethod]
        public void Should_Set_And_Read_When_Empty()
        {
            var screening = ScreeningResultMotherObject.GetAllYesAnswers();
            var sut = Sut(screening);

            sut.Primary = 10;
            sut.Secondary = 5;
            sut.Tertiary = 1;

            sut.Primary.Should().Be(10);
            sut.Secondary.Should().Be(5);
            sut.Tertiary.Should().Be(1);

        }


        [TestMethod]
        public void Should_Get_Modified_Section_With_All_Answers_When_NotEmpty()
        {
            var screening = ScreeningResultMotherObject.GetAllYesAnswers().WithDrugOfChoicePrimaryAndSecondary();
            var sut = Sut(screening);

            sut.Primary = 10;
            sut.Secondary = 5;
            sut.Tertiary = 1;

            var section = sut.GetSection();

            section.Answers.Count.Should().Be(3, "Section should include all 3 answers");
            section.Answers[0].AnswerValue.Should().Be(10, "Primary should match");
            section.Answers[1].AnswerValue.Should().Be(5, "Secondary should match");
            section.Answers[2].AnswerValue.Should().Be(1, "Tertiary should match");

        }

        [TestMethod]
        public void Should_Get_Populated_PrimaryAnswer_In_Section_When_Empty()
        {
            var screening = ScreeningResultMotherObject.GetAllYesAnswers();
            var sut = Sut(screening);

            sut.Primary = 10;
            sut.Secondary = 0;
            sut.Tertiary = 0;

            var answer = sut.GetSection().Answers[0];

            answer.ScreeningSectionID.Should().Be(ScreeningSectionDescriptor.DrugOfChoice, "Section ID should match");
            answer.QuestionID.Should().Be(DrugOfChoiceDescriptor.PrimaryQuestionId, "Question ID should match");
            answer.AnswerValue.Should().Be(10);
        }


        [TestMethod]
        public void Should_Return_Same_Section_When_NonEmpty()
        {
            var screening = ScreeningResultMotherObject.GetAllYesAnswers().WithDrugOfChoicePrimaryAndSecondary();
            var sut = Sut(screening);

            sut.Primary = 10;

            sut.GetSection().Should().BeSameAs(screening.FindSectionByID(ScreeningSectionDescriptor.DrugOfChoice));
        }


        [TestMethod]
        public void Should_Reset_Secondary_When_Primary_Changed_To_None()
        {
            var screening = ScreeningResultMotherObject.GetAllYesAnswers().WithDrugOfChoiceAllAnswers();
            var sut = Sut(screening);

            sut.Primary = 0;

            var section = sut.GetSection();

            section.Answers[0].AnswerValue.Should().Be(0, "Primary should match");
            section.Answers[1].AnswerValue.Should().Be(0, "Secondary should match");
            section.Answers[2].AnswerValue.Should().Be(0, "Tertiary should match");
        }
}

}
