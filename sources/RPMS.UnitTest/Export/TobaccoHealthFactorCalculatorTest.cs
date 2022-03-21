using RPMS.Common.Export;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FrontDesk;
using System.Collections.Generic;
using RPMS.Common.Models;
using System.Dynamic;
using RPMS.Common.Configuration;
using FluentAssertions;

namespace RPMS.UnitTest.Export
{

    /// <summary>
    /// Test all 24 cases from Tobacco_health_factor_export_matrix_Updated_2012_12_4.docx
    /// </summary>
    [DeploymentItem(@"Configuration\rpmsExportConfiguration.config", "Configuration")]
    [TestClass()]
    public class TobaccoHealthFactorCalculatorTest
    {

        private List<IEnumerable<ScreeningSectionResult>> CreateTestCases()
        {
            List<IEnumerable<ScreeningSectionResult>> testCases = new List<IEnumerable<ScreeningSectionResult>>();

            //1
            testCases.Add(new List<ScreeningSectionResult>{
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome, AnswerValue = 1},
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.Tobacco, AnswerValue = 0},
            });

            //2
            testCases.Add(new List<ScreeningSectionResult>{
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome, AnswerValue = 0},
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.Tobacco, AnswerValue = 0},
            });


            //3
            testCases.Add(new List<ScreeningSectionResult>{
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome, AnswerValue = 0},
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.Tobacco, AnswerValue = 1,
                    }.
                    ImportQuestionAnswerRange(new List<ScreeningSectionQuestionResult>{
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.UseForCeremonyOnlyQuestionID, AnswerValue = 0},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeQuestionID, AnswerValue = 0},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeSmokelessQuestionID, AnswerValue = 0},
                    }),
            });

            //4
            testCases.Add(new List<ScreeningSectionResult>{
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome, AnswerValue = 1},
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.Tobacco, AnswerValue = 1,
                    }.
                    ImportQuestionAnswerRange(new List<ScreeningSectionQuestionResult>{
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.UseForCeremonyOnlyQuestionID, AnswerValue = 0},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeQuestionID, AnswerValue = 0},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeSmokelessQuestionID, AnswerValue = 0},
                    }),
            });

            //5
            testCases.Add(new List<ScreeningSectionResult>{
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome, AnswerValue = 0},
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.Tobacco, AnswerValue = 1,
                    }.
                    ImportQuestionAnswerRange(new List<ScreeningSectionQuestionResult>{
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.UseForCeremonyOnlyQuestionID, AnswerValue = 1},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeQuestionID, AnswerValue = 0},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeSmokelessQuestionID, AnswerValue = 0},
                    }),
            });

            //6
            testCases.Add(new List<ScreeningSectionResult>{
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome, AnswerValue = 1},
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.Tobacco, AnswerValue = 1,
                    }.
                    ImportQuestionAnswerRange(new List<ScreeningSectionQuestionResult>{
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.UseForCeremonyOnlyQuestionID, AnswerValue = 1},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeQuestionID, AnswerValue = 0},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeSmokelessQuestionID, AnswerValue = 0},
                    }),
            });

            //7
            testCases.Add(new List<ScreeningSectionResult>{
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome, AnswerValue = 0},
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.Tobacco, AnswerValue = 1,
                    }.
                    ImportQuestionAnswerRange(new List<ScreeningSectionQuestionResult>{
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.UseForCeremonyOnlyQuestionID, AnswerValue = 0},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeQuestionID, AnswerValue = 1},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeSmokelessQuestionID, AnswerValue = 0},
                    }),
            });

            //8
            testCases.Add(new List<ScreeningSectionResult>{
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome, AnswerValue = 1},
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.Tobacco, AnswerValue = 1,
                    }.
                    ImportQuestionAnswerRange(new List<ScreeningSectionQuestionResult>{
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.UseForCeremonyOnlyQuestionID, AnswerValue = 0},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeQuestionID, AnswerValue = 1},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeSmokelessQuestionID, AnswerValue = 0},
                    }),
            });


            //9
            testCases.Add(new List<ScreeningSectionResult>{
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome, AnswerValue = 0},
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.Tobacco, AnswerValue = 1,
                    }.
                    ImportQuestionAnswerRange(new List<ScreeningSectionQuestionResult>{
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.UseForCeremonyOnlyQuestionID, AnswerValue = 0},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeQuestionID, AnswerValue = 0},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeSmokelessQuestionID, AnswerValue = 1},
                    }),
            });

            //10
            testCases.Add(new List<ScreeningSectionResult>{
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome, AnswerValue = 1},
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.Tobacco, AnswerValue = 1,
                    }.
                    ImportQuestionAnswerRange(new List<ScreeningSectionQuestionResult>{
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.UseForCeremonyOnlyQuestionID, AnswerValue = 0},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeQuestionID, AnswerValue = 0},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeSmokelessQuestionID, AnswerValue = 1},
                    }),
            });

            //11
            testCases.Add(new List<ScreeningSectionResult>{
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome, AnswerValue = 0},
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.Tobacco, AnswerValue = 1,
                    }.
                    ImportQuestionAnswerRange(new List<ScreeningSectionQuestionResult>{
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.UseForCeremonyOnlyQuestionID, AnswerValue = 1},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeQuestionID, AnswerValue = 1},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeSmokelessQuestionID, AnswerValue = 0},
                    }),
            });

            //12
            testCases.Add(new List<ScreeningSectionResult>{
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome, AnswerValue = 1},
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.Tobacco, AnswerValue = 1,
                    }.
                    ImportQuestionAnswerRange(new List<ScreeningSectionQuestionResult>{
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.UseForCeremonyOnlyQuestionID, AnswerValue = 1},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeQuestionID, AnswerValue = 1},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeSmokelessQuestionID, AnswerValue = 0},
                    }),
            });

            //13
            testCases.Add(new List<ScreeningSectionResult>{
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome, AnswerValue = 0},
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.Tobacco, AnswerValue = 1,
                    }.
                    ImportQuestionAnswerRange(new List<ScreeningSectionQuestionResult>{
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.UseForCeremonyOnlyQuestionID, AnswerValue = 1},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeQuestionID, AnswerValue = 0},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeSmokelessQuestionID, AnswerValue = 1},
                    }),
            });

            //14
            testCases.Add(new List<ScreeningSectionResult>{
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome, AnswerValue = 1},
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.Tobacco, AnswerValue = 1,
                    }.
                    ImportQuestionAnswerRange(new List<ScreeningSectionQuestionResult>{
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.UseForCeremonyOnlyQuestionID, AnswerValue = 1},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeQuestionID, AnswerValue = 0},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeSmokelessQuestionID, AnswerValue = 1},
                    }),
            });

            //15
            testCases.Add(new List<ScreeningSectionResult>{
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome, AnswerValue = 0},
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.Tobacco, AnswerValue = 1,
                    }.
                    ImportQuestionAnswerRange(new List<ScreeningSectionQuestionResult>{
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.UseForCeremonyOnlyQuestionID, AnswerValue = 0},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeQuestionID, AnswerValue = 1},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeSmokelessQuestionID, AnswerValue = 1},
                    }),
            });

            //16
            testCases.Add(new List<ScreeningSectionResult>{
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome, AnswerValue = 1},
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.Tobacco, AnswerValue = 1,
                    }.
                    ImportQuestionAnswerRange(new List<ScreeningSectionQuestionResult>{
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.UseForCeremonyOnlyQuestionID, AnswerValue = 0},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeQuestionID, AnswerValue = 1},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeSmokelessQuestionID, AnswerValue = 1},
                    }),
            });

            //17
            testCases.Add(new List<ScreeningSectionResult>{
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome, AnswerValue = 0},
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.Tobacco, AnswerValue = 1,
                    }.
                    ImportQuestionAnswerRange(new List<ScreeningSectionQuestionResult>{
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.UseForCeremonyOnlyQuestionID, AnswerValue = 1},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeQuestionID, AnswerValue = 1},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeSmokelessQuestionID, AnswerValue = 1},
                    }),
            });

            //18
            testCases.Add(new List<ScreeningSectionResult>{
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.SmokerInHome, AnswerValue = 1},
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.Tobacco, AnswerValue = 1,
                    }.
                    ImportQuestionAnswerRange(new List<ScreeningSectionQuestionResult>{
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.UseForCeremonyOnlyQuestionID, AnswerValue = 1},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeQuestionID, AnswerValue = 1},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeSmokelessQuestionID, AnswerValue = 1},
                    }),
            });



            //19
            testCases.Add(new List<ScreeningSectionResult>{
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.Tobacco, AnswerValue = 0,
                    },
            });

            //20
            testCases.Add(new List<ScreeningSectionResult>{
                new ScreeningSectionResult{ ScreeningSectionID = ScreeningSectionDescriptor.Tobacco, AnswerValue = 1,
                    }.
                    ImportQuestionAnswerRange(new List<ScreeningSectionQuestionResult>{
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.UseForCeremonyOnlyQuestionID, AnswerValue = 0},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeQuestionID, AnswerValue = 0},
                        new ScreeningSectionQuestionResult{ QuestionID = TobaccoQuestionsDescriptor.DoYouSmokeSmokelessQuestionID, AnswerValue = 0},
                    }),
            });

            return testCases;
        }


        private List<string[]> CreateExpectedTestCaseResults()
        {
            return new List<string[]>{
                //1
                new string[]{HealthFactorKeys.TobaccoSmokerInHome}, 
                //2
                new string[]{HealthFactorKeys.TobaccoSmokerFreeHome}, 
                //3
                new string[]{HealthFactorKeys.TobaccoSmokerFreeHome}, 
                //4
                new string[]{HealthFactorKeys.TobaccoSmokerInHome},
                //5
                new string[]{HealthFactorKeys.TobaccoCeremonialUseOnly},
                //6
                new string[]{
                    HealthFactorKeys.TobaccoSmokerInHome,
                    HealthFactorKeys.TobaccoCeremonialUseOnly
                },
                //7
                new string[]{HealthFactorKeys.TobaccoCurrentSmoker},
                //8
                new string[]{
                        HealthFactorKeys.TobaccoSmokerInHome,
                        HealthFactorKeys.TobaccoCurrentSmoker
                },
                //9
                new string[]{HealthFactorKeys.TobaccoCurrentSmokeless},
                //10
                new string[]{
                        HealthFactorKeys.TobaccoSmokerInHome,
                    HealthFactorKeys.TobaccoCurrentSmokeless
                },
                //11
                new string[]{
                    HealthFactorKeys.TobaccoCeremonialUseOnly,
                    HealthFactorKeys.TobaccoCurrentSmoker},
                //12
                new string[]{
                        HealthFactorKeys.TobaccoSmokerInHome,
                    HealthFactorKeys.TobaccoCeremonialUseOnly,
                    HealthFactorKeys.TobaccoCurrentSmoker
                },
                //13
                new string[]{
                    HealthFactorKeys.TobaccoCeremonialUseOnly,
                    HealthFactorKeys.TobaccoCurrentSmokeless},
                //14
                new string[]{
                        HealthFactorKeys.TobaccoSmokerInHome,
                        HealthFactorKeys.TobaccoCeremonialUseOnly,
                        HealthFactorKeys.TobaccoCurrentSmokeless
                },
                //15
                new string[]{
                    HealthFactorKeys.TobaccoCurrentSmoker,
                    HealthFactorKeys.TobaccoCurrentSmokeless,
                    HealthFactorKeys.TobaccoCurrentSmokerAndSmokeless
                },
                //16
                new string[]{
                        HealthFactorKeys.TobaccoSmokerInHome,
                        HealthFactorKeys.TobaccoCurrentSmoker,
                        HealthFactorKeys.TobaccoCurrentSmokeless,
                        HealthFactorKeys.TobaccoCurrentSmokerAndSmokeless
                },
                //17
                new string[]{
                    HealthFactorKeys.TobaccoCeremonialUseOnly,
                    HealthFactorKeys.TobaccoCurrentSmoker,
                    HealthFactorKeys.TobaccoCurrentSmokeless,
                    HealthFactorKeys.TobaccoCurrentSmokerAndSmokeless
                },
                //18
                new string[]{
                        HealthFactorKeys.TobaccoSmokerInHome,
                        HealthFactorKeys.TobaccoCeremonialUseOnly,
                        HealthFactorKeys.TobaccoCurrentSmoker,
                        HealthFactorKeys.TobaccoCurrentSmokeless,

                        HealthFactorKeys.TobaccoCurrentSmokerAndSmokeless
                },
                //19
                new string[]{HealthFactorKeys.TobaccoCurrentNonSmoker},
                //20
                new string[]{HealthFactorKeys.TobaccoCurrentNonSmoker},
            };
        }

        /// <summary>
        ///A test for CalculateFilteredResults
        ///</summary>
        [TestMethod()]
        public void Can_export_all_combinations()
        {
            TobaccoHealthFactorCalculator target = new TobaccoHealthFactorCalculator();
          
            int caseNo = 0;
            IList<string> expected = null;
            IList<HealthFactor> actual;

            var cases = CreateTestCases();
            List<string[]> expectedResults = CreateExpectedTestCaseResults();


            foreach (var c in cases)
            {
                actual = target.Calculate(c);


                actual.Should().NotBeNull();

                expected = expectedResults[caseNo];
                actual.Count.Should().Be(expected.Count, "Case {0}: # of Heath factors shall match expected", caseNo + 1);

                for (int i = 0; i < expected.Count; i++)
                {
                    var expectedFactorKey = expected[i];
                    var actualFactor = actual[i];

                    var expectedFactorElement = RpmsExportConfiguration.GetConfiguration().HealthFactors[expectedFactorKey];

                    actual[i].Factor.Should().Be(expectedFactorElement.Factor, "Case {0}: Factors shall match in items # {1}", caseNo + 1, i);
                }

                caseNo++;
            }


        }

    }
}
