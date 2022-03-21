using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Tests.MotherObjects
{
    public static class ScreeningInfoMotherObject
    {

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
                    GetAnxietySection(),
                }
            };
        }

        private static ScreeningSection GetViolenceSection()
        {
            return new ScreeningSection
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
        }

        private static ScreeningSection GetDepressionSecion()
        {
            return new ScreeningSection
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
        }

        private static ScreeningSection GetDrugsSecion()
        {
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
            return info;
        }

        private static ScreeningSection GetAlcoholSecion()
        {
            return new ScreeningSection
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
        }

        private static ScreeningSection GetTobaccoSecion()
        {
            return new ScreeningSection
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

        }

        public static ScreeningSection GetSmokerInHomeSecion()
        {
            return new ScreeningSection
            {
                ScreeningSectionID = "SIH",
                ScreeningSectionName = "Smoker in the Home",
                Questions = new List<ScreeningSectionQuestion>()
            };
        }



        private static ScreeningSection GetAnxietySection()
        {
            return new ScreeningSection
            {
                ScreeningSectionID = "GAD-7",
                ScreeningSectionName = "Anxiety (GAD-7)",
                Questions = new List<ScreeningSectionQuestion>
                {

                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 1,
                        QuestionText = "Feeling ner vous, anxious, or on edge",
                        AnswerScaleID = 2,
                        IsMainQuestion = true,
                        IndexOrder = 10,
                        ScreeningSectionID = "GAD-7"
                    },
                     new ScreeningSectionQuestion()
                    {
                        QuestionID = 2,
                        QuestionText = "Not being able to stop or control worrying",
                        AnswerScaleID = 2,
                        IsMainQuestion = true,
                        IndexOrder = 20,
                        ScreeningSectionID = "GAD-7"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 3,
                        AnswerScaleID = 2,
                        IndexOrder = 100,
                        ScreeningSectionID = "GAD-7"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 4,
                        AnswerScaleID = 2,
                        IndexOrder = 100,
                        ScreeningSectionID = "GAD-7"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 5,
                        AnswerScaleID = 2,
                        IndexOrder = 100,
                        ScreeningSectionID = "GAD-7"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 6,
                        AnswerScaleID = 2,
                        IndexOrder = 100,
                        ScreeningSectionID = "GAD-7"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 7,
                        AnswerScaleID = 2,
                        IndexOrder = 100,
                        ScreeningSectionID = "GAD-7"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 8,
                        QuestionText = "If you checked off ANY problems, how DIFFICULT have these problems made it for you to do your work, take care of things at home, or get along with other people?",
                        AnswerScaleID = 3,
                        IsMainQuestion = false,
                        ShowOnlyWhenPossitiveScore = true,
                        IndexOrder = 100,
                        ScreeningSectionID = "GAD-7"
                    },



                }

            };
        }
    }
}
