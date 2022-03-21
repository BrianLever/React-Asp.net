using System.Collections.Generic;
using FrontDesk;

namespace FrontdDeskKiosk.Tests.MotherObjects
{
    public static class ScreeningMotherObject
    {
        public static Screening GetFullScreening()
        {
            return new KioskScreening
            {
                ScreeningID = "BHS",
                Sections = new List<ScreeningSection>
                {
                    GetSmokerInHomeSecion(),
                    GetTobaccoSecion(),
                    GetAlcoholSecion(),
                    GetDrugsSecion(),
                    GetDepressionSecion(),
                    GetViolenceSecion(),
                }
            };
        }

        private static ScreeningSection GetViolenceSecion()
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
                        QuestionText = "Did a partner, family member, or caregiver\n hurt, insult, threaten, or scream at you?",
                        PreambleText = "Over the <b>LAST 12 MONTHS</b>:",
                        AnswerScaleID = 1,
                        IsMainQuestion = true,
                        IndexOrder = 10,
                        ScreeningSectionID = "HITS"
                    },

                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 1,
                        QuestionText = "Physically HURT you?",
                        PreambleText = "Over the LAST 12 MONTHS, how often did your partner, family member, or caregiver:",
                        AnswerScaleID = 4,
                        IsMainQuestion = false,
                        IndexOrder = 100,
                        ScreeningSectionID = "HITS"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 2,
                        QuestionText = "INSULT or talk down to you?",
                        PreambleText = "Over the LAST 12 MONTHS, how often did your partner, family member, or caregiver:",
                        AnswerScaleID = 4,
                        IsMainQuestion = false,
                        IndexOrder = 100,
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
                        PreambleText = "Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems?",
                        AnswerScaleID = 2,
                        IsMainQuestion = true,
                        IndexOrder = 10,
                        ScreeningSectionID = "PHQ-9"
                    },
                     new ScreeningSectionQuestion()
                    {
                        QuestionID = 2,
                        QuestionText = "Feeling down, depressed, or hopeless",
                         PreambleText = "Over the LAST 2 WEEKS, how often have you been bothered by any of the following problems?",
                         AnswerScaleID = 2,
                        IsMainQuestion = true,
                        IndexOrder = 20,
                        ScreeningSectionID = "PHQ-9"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 3,
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
            return new ScreeningSection
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
                    },

                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 1,
                        QuestionText = "Do you abuse more than one drug at a time?",
                        AnswerScaleID = 1,
                        IsMainQuestion = false,
                        IndexOrder = 100,
                        ScreeningSectionID = "DAST"
                    },
                    new ScreeningSectionQuestion()
                    {
                        QuestionID = 2,
                        QuestionText = "Are you always able to stop using drugs when you want to?",
                        AnswerScaleID = 1,
                        ScreeningSectionID = "DAST"
                    },

                }

            };
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
    }
}
