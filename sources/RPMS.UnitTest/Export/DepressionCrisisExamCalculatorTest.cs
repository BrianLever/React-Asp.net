using RPMS.Common.Export;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FrontDesk;
using System.Collections.Generic;
using RPMS.Common.Models;
using FluentAssertions;

namespace RPMS.UnitTest.Export
{


	/// <summary>
	///This is a test class for DepressionCrisisAlertCalculatorTest and is intended
	///to contain all DepressionCrisisAlertCalculatorTest Unit Tests
	///</summary>
	[DeploymentItem(@"Configuration\rpmsExportConfiguration.config", "Configuration")]
	[TestClass()]
	public class DepressionCrisisExamCalculatorTest
	{

		private DepressionExamCalculator Sut()
		{
			return new DepressionExamCalculator(); ;
		}


		[TestMethod()]
		public void Calculate_should_return_null_for_non_asked()
		{
			ScreeningSectionResult sectionResult = ScreeningResultHelper.GetDepressionHurtYouselfNotAtAll();
			var sut = Sut();


			var actual = sut.Calculate(new ScreeningSectionResult[] { sectionResult });
			actual.Should().NotBeNull();
			actual.Should().BeEmpty();
		}

		[TestMethod()]
		public void Calculate_should_return_null_for_no()
		{
			ScreeningResult sectionResult = ScreeningResultHelper.GetAllNoAnswers();
			var sut = Sut();

			var actual = sut.Calculate(sectionResult.SectionAnswers);

			actual.Should().NotBeNull();
			actual.Should().BeEmpty();
		}


		[TestMethod()]
		public void Calculate_should_return_null_for_never()
		{
			ScreeningSectionResult sectionResult = ScreeningResultHelper.GetDepressionHurtYouselfNotAtAll();
			var sut = Sut();


			var actual = sut.Calculate(new ScreeningSectionResult[] { sectionResult });

			actual.Should().NotBeNull();
			actual.Should().BeEmpty();

		}

		[TestMethod()]
		public void Calculate_should_return_suicidal_for_rarery_hurt_youself()
		{
			ScreeningSectionResult sectionResult = ScreeningResultHelper.GetDepressionHurtYouselfSeveralDaysPlusModerate();
			var sut = Sut();


			var actual = sut.Calculate(new ScreeningSectionResult[] { sectionResult });

			actual.Should().NotBeNull();
			actual.Should().NotBeEmpty();
			actual.Count.Should().Be(1);

			var actualExam = actual[0];

			actualExam.Comment.Should()
				.Be("Score: 11. Level of Risk: SUICIDAL ideation + MODERATE depression severity. Exported from Screendox.");

		}


		[TestMethod()]
		public void Calculate_should_return_moderate_without_suicidal_when_hurt_not_at_all()
		{
			ScreeningSectionResult sectionResult = ScreeningResultHelper.GetDepressionHurtYouseNotAtAllPlusModerate();
			var sut = Sut();


			var actual = sut.Calculate(new ScreeningSectionResult[] { sectionResult });

			actual.Should().NotBeNull();
			actual.Should().NotBeEmpty();
			actual.Count.Should().Be(1);

			var actualExam = actual[0];

			actualExam.Comment.Should()
				.EndWith("Score: 11. Level of Risk: MODERATE depression severity. Exported from Screendox.");

		}
	}
}
