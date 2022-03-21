using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Tests.MotherObjects
{
    public static class ScreeningInfoMotherObject
    {

        public static List<AnswerScaleOption> GetAllAnswerOptions()
        {
            return new List<AnswerScaleOption>
            {
                //'Yes / No
                new AnswerScaleOption
                {
                    AnswerScaleOptionID = 1,
                    AnswerScaleID = 1,
                    Text = "Yes",
                    Value = 1
                },
                new AnswerScaleOption
                {
                    AnswerScaleOptionID = 2,
                    AnswerScaleID = 1,
                    Text = "No",
                    Value = 0
                },
                //PHQ-9
                new AnswerScaleOption
                {
                    AnswerScaleOptionID= 3,
                    AnswerScaleID  = 2,
                    Text = "Not at all",
                    Value = 0
                },
                 new AnswerScaleOption
                {
                    AnswerScaleOptionID = 4,
                    AnswerScaleID = 2,
                    Text = "Several days",
                    Value = 1
                },
                  new AnswerScaleOption
                {
                    AnswerScaleOptionID = 5,
                    AnswerScaleID = 2,
                    Text = "More than half the days",
                    Value = 2
                },
                   new AnswerScaleOption
                {
                    AnswerScaleOptionID = 6,
                    AnswerScaleID = 2,
                    Text = "Nearly every day",
                    Value = 3
                },

                //PHQ-9 Difficulty
                new AnswerScaleOption
                {
                    AnswerScaleOptionID = 7,
                    AnswerScaleID = 3,
                    Text = "Not difficult at all",
                    Value = 0
                },

                new AnswerScaleOption
                {
                    AnswerScaleOptionID = 7,
                    AnswerScaleID = 3,
                    Text = "Not difficult at all",
                    Value = 0
                },
                new AnswerScaleOption
                {
                    AnswerScaleOptionID = 8,
                    AnswerScaleID = 3,
                    Text = "Somewhat difficult",
                    Value = 1
                },
                new AnswerScaleOption
                {
                    AnswerScaleOptionID = 9,
                    AnswerScaleID = 3,
                    Text = "Very difficult",
                    Value = 2
                },
                new AnswerScaleOption
                {
                    AnswerScaleOptionID = 10,
                    AnswerScaleID = 3,
                    Text = "Extremely difficult",
                    Value = 3
                },

                //HITS
                                             
                new AnswerScaleOption
                {
                    AnswerScaleOptionID = 11,
                    AnswerScaleID = 4,
                    Text = "Never",
                    Value = 1
                },
                new AnswerScaleOption
                {
                    AnswerScaleOptionID = 12,
                    AnswerScaleID = 4,
                    Text = "Rarely",
                    Value = 2
                },
                new AnswerScaleOption
                {
                    AnswerScaleOptionID = 13,
                    AnswerScaleID = 4,
                    Text = "Sometimes",
                    Value = 3
                },
                new AnswerScaleOption
                {
                    AnswerScaleOptionID = 14,
                    AnswerScaleID = 4,
                    Text = "Fairly Often",
                    Value = 4
                },
                new AnswerScaleOption
                {
                    AnswerScaleOptionID = 15,
                    AnswerScaleID = 4,
                    Text = "Frequently",
                    Value = 5
                }
            };
        }


        public static FrontDesk.Screening GetFullScreening()
        {
            return new ServerScreening
            {
                ScreeningID = "BHS",
                Sections = new List<ScreeningSection>
                {
                    GetSmokerInHomeSecion(),
                    GetTobaccoSecion(),
                    GetAlcoholSecion(),
                    GetDrugsSecion(),
                    GetDepressionSecion(),
                    GetViolenceSection(),
                }
            };
        }

        private static ScreeningSection GetViolenceSection()
        {
            var answerOptions = GetAllAnswerOptions();

            var section = new ScreeningSection
            {
                ScreeningSectionID = "HITS",
                ScreeningSectionName = "Intimate Partner/Domestic Violence (HITS)",
                Questions = new List<ScreeningSectionQuestion>
                {
                     new ScreeningSectionQuestion()
                    {
                        QuestionID = 5,
                        AnswerScaleID = 1,
                        IsMainQuestion = true,
                        IndexOrder = 10,
                        ScreeningSectionID = "HITS"
                    },

                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 1,
                        QuestionText = "Physically HURT you?",
                        AnswerScaleID = 4,
                        ScreeningSectionID = "HITS"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 2,
                        QuestionText = "INSULT or talk down to you?",
                        AnswerScaleID = 4,
                        ScreeningSectionID = "HITS"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 3,
                        AnswerScaleID = 4,
                        ScreeningSectionID = "HITS"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 4,
                        AnswerScaleID = 4,
                        ScreeningSectionID = "HITS"
                    },

                }

            };

            foreach (var q in section.Questions)
            {
                q.AnswerOptions = answerOptions.Where(x => x.AnswerScaleID == q.AnswerScaleID).ToList();
            }

            return section;
        }

        private static ScreeningSection GetDepressionSecion()
        {
            var answerOptions = GetAllAnswerOptions();


            var section = new ScreeningSection
            {
                ScreeningSectionID = "PHQ-9",
                ScreeningSectionName = "Depression (PHQ-9)",
                Questions = new List<ScreeningSectionQuestion>
                {

                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 1,
                        QuestionText = "Little interest or pleasure in doing things",
                        AnswerScaleID = 2,
                        IsMainQuestion = true,
                        IndexOrder = 10,
                        ScreeningSectionID = "PHQ-9"
                    },
                     new ScreeningSectionQuestion()
                    {
                        QuestionID = 2,
                        QuestionText = "Feeling down, depressed, or hopeless",
                        AnswerScaleID = 2,
                        IsMainQuestion = true,
                        IndexOrder = 20,
                        ScreeningSectionID = "PHQ-9"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 3,
                        AnswerScaleID = 2,
                        IndexOrder = 100,
                        ScreeningSectionID = "PHQ-9"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 4,
                        AnswerScaleID = 2,
                        IndexOrder = 100,
                        ScreeningSectionID = "PHQ-9"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 5,
                        AnswerScaleID = 2,
                        IndexOrder = 100,
                        ScreeningSectionID = "PHQ-9"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 6,
                        AnswerScaleID = 2,
                        IndexOrder = 100,
                        ScreeningSectionID = "PHQ-9"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 7,
                        AnswerScaleID = 2,
                        IndexOrder = 100,
                        ScreeningSectionID = "PHQ-9"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 8,
                        AnswerScaleID = 2,
                        IndexOrder = 100,
                        ScreeningSectionID = "PHQ-9"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 9,
                        QuestionText = "Thoughts that you would be better off dead or of hurting yourself in some way?",
                        AnswerScaleID = 2,
                        IndexOrder = 100,
                        ScreeningSectionID = "PHQ-9"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 10,
                        QuestionText = "If you checked off ANY problems, how DIFFICULT have these problems made it for you to do your work, take care of things at home, or get along with other people?",
                        AnswerScaleID = 3,
                        IsMainQuestion = true,
                        IndexOrder = 100,
                        ScreeningSectionID = "PHQ-9"
                    },


                }

            };

            foreach (var q in section.Questions)
            {
                q.AnswerOptions = answerOptions.Where(x => x.AnswerScaleID == q.AnswerScaleID).ToList();
            }

            return section;
        }

        private static ScreeningSection GetDrugsSecion()
        {
            var answerOptions = GetAllAnswerOptions();

            var info = new ScreeningSection
            {
                ScreeningSectionID = "DAST",
                ScreeningSectionName = "Non-Medical Drug Use (DAST-10)",
                Questions = new List<ScreeningSectionQuestion>
                {
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 10,
                        PreambleText = "Over the <b>LAST 12 MONTHS</b>:",
                        QuestionText = "Have you used drugs other than those required for medical reasons?",
                        AnswerScaleID = 1,
                        IsMainQuestion = true,
                        IndexOrder = 10,
                        ScreeningSectionID = "DAST"
                    }
                }
            };

            for (int i = 1; i <= 9; i++)
            {
                info.Questions.Add(new ScreeningSectionQuestion()
                {
                    QuestionID = i,
                    AnswerScaleID = 1,
                    IsMainQuestion = false,
                    IndexOrder = 100,
                    ScreeningSectionID = "DAST"
                });
            }

            foreach (var q in info.Questions)
            {
                q.AnswerOptions = answerOptions.Where(x => x.AnswerScaleID == q.AnswerScaleID).ToList();
            }
            return info;
        }

        private static ScreeningSection GetAlcoholSecion()
        {
            var answerOptions = GetAllAnswerOptions();

            var section = new ScreeningSection
            {
                ScreeningSectionID = "CAGE",
                ScreeningSectionName = "Alcohol Use (CAGE)",
                Questions = new List<ScreeningSectionQuestion>
                {
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 5,
                        QuestionText = "Do you drink alcohol?",
                        AnswerScaleID = 1,
                        IsMainQuestion = true,
                        IndexOrder = 10,
                        ScreeningSectionID = "CAGE"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 1,
                        QuestionText = "Have you ever felt you should CUT down on your drinking?",
                        AnswerScaleID = 1,
                        IsMainQuestion = false,
                        IndexOrder = 100,
                        ScreeningSectionID = "CAGE"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 2,
                        QuestionText = "Have people ANNOYED you by criticizing your drinking?",
                        AnswerScaleID = 1,
                        IsMainQuestion = false,
                        IndexOrder = 100,
                        ScreeningSectionID = "CAGE"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 3,
                        QuestionText = "Have you ever felt bad or GUILTY about your drinking?",
                        AnswerScaleID = 1,
                        IsMainQuestion = false,
                        IndexOrder = 100,
                        ScreeningSectionID = "CAGE"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 4,
                        QuestionText = "Have you ever had a drink first thing in the morning to steady your nerves or get rid of a hangover (EYE-OPENER)?",
                        AnswerScaleID = 1,
                        IsMainQuestion = false,
                        IndexOrder = 100,
                        ScreeningSectionID = "CAGE"
                    },

                }

            };

            foreach (var q in section.Questions)
            {
                q.AnswerOptions = answerOptions.Where(x => x.AnswerScaleID == q.AnswerScaleID).ToList();
            }

            return section;
        }

        private static ScreeningSection GetTobaccoSecion()
        {
            var answerOptions = GetAllAnswerOptions();

            var section = new ScreeningSection
            {
                ScreeningSectionID = "TCC",
                ScreeningSectionName = "Tobacco Cessation Counseling",
                Questions = new List<ScreeningSectionQuestion>
                {
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 4,
                        QuestionText = "Do you use tobacco?",
                        AnswerScaleID = 1,
                        IsMainQuestion = true,
                        ScreeningSectionID = "TCC"
                    },

                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 1,
                        QuestionText = "Do you use tobacco for ceremony?",
                        AnswerScaleID = 1,
                        ScreeningSectionID = "TCC"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 2,
                        QuestionText = "Do you smoke tobacco (such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)?",
                        AnswerScaleID = 1,
                        ScreeningSectionID = "TCC"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 3,
                        QuestionText = "Do you use smokeless tobacco?",
                        AnswerScaleID = 1,
                        ScreeningSectionID = "TCC"
                    }
                }

            };

            foreach (var q in section.Questions)
            {
                q.AnswerOptions = answerOptions.Where(x => x.AnswerScaleID == q.AnswerScaleID).ToList();
            }

            return section;
        }

        public static ScreeningSection GetSmokerInHomeSecion()
        {
            return new ScreeningSection
            {
                ScreeningSectionID = "SIH",
                ScreeningSectionName = "Smoker in the Home",
                Questions = new List<ScreeningSectionQuestion>()
                {
                    new ScreeningSectionQuestion
                    {
                        QuestionID = 1,
                        QuestionText = "Does anyone in the home smoke tobacco\n(such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)?",
                        IsMainQuestion = true,
                        AnswerScaleID = 1,
                        ScreeningSectionID = "SIH",
                        AnswerOptions = GetAllAnswerOptions().Where(x => x.AnswerScaleID == 1).ToList()
                    }
                }
            };
        }
    }
}
