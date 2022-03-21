using System;
using System.Collections.Generic;
using FrontDesk.Server.Screening;

namespace FrontDesk.Server.Tests.Helpers
{
    public static class ScreeningScoreLevelRepositoryFactory
    {
        public static IList<ScreeningScoreLevel> CreateCageItems()
        {
            return
            new ScreeningScoreLevel[]{ 
                new ScreeningScoreLevel{
                    ScreeningSectionID = "CAGE",
                    ScoreLevel = 0,
                    Name = "NEGATIVE"
                },
                new ScreeningScoreLevel{
                    ScreeningSectionID = "CAGE",
                    ScoreLevel = 1,
                    Name = "Evidence of AT RISK"
                },
                new ScreeningScoreLevel{
                    ScreeningSectionID = "CAGE",
                    ScoreLevel = 2,
                    Name = "Evidence of CURRENT PROBLEM"
                },
                 new ScreeningScoreLevel{
                    ScreeningSectionID = "CAGE",
                    ScoreLevel = 3,
                    Name = "Evidence of DEPENDENCE until ruled out"
                },
            };
        }


        public static IList<ScreeningScoreLevel> CreateDastItems()
        {
            return
            new ScreeningScoreLevel[]{ 
                new ScreeningScoreLevel{
                    ScreeningSectionID = "DAST",
                    ScoreLevel = 0,
                    Name = "NEGATIVE"
                },
                new ScreeningScoreLevel{
                    ScreeningSectionID = "DAST",
                    ScoreLevel = 1,
                    Name = "LOW LEVEL degree of problem related to drug use"
                },
                new ScreeningScoreLevel{
                    ScreeningSectionID = "DAST",
                    ScoreLevel = 2,
                    Name = "MODERATE LEVEL degree of problem related to drug use"
                },
                new ScreeningScoreLevel{
                    ScreeningSectionID = "DAST",
                    ScoreLevel = 3,
                    Name = "SUBSTANTIAL LEVEL degree of problem related to drug use"
                },
                new ScreeningScoreLevel{
                    ScreeningSectionID = "DAST",
                    ScoreLevel = 4,
                    Name = "SEVERE LEVEL degree of problem related to drug use"
                },
            };
        }

        

        public static IList<ScreeningScoreLevel> CreateHitsItems()
        {
            return
            new ScreeningScoreLevel[]{ 
                new ScreeningScoreLevel{
                    ScreeningSectionID = "HITS",
                    ScoreLevel = 0,
                    Name = "NEGATIVE"
                },
                new ScreeningScoreLevel{
                    ScreeningSectionID = "HITS",
                    ScoreLevel = 1,
                    Name = "Evidence of CURRENT PROBLEM"
                },
            };
        }

        public static IList<ScreeningScoreLevel> CreatePhq9Items()
        {
            return
            new ScreeningScoreLevel[]{ 
                new ScreeningScoreLevel{
                    ScreeningSectionID = "PHQ-9",
                    ScoreLevel = 0,
                    Name = "NONE-MINIMAL depression severity"
                },
                new ScreeningScoreLevel{
                    ScreeningSectionID = "PHQ-9",
                    ScoreLevel =2,
                    Name = "MILD depression severity"
                },
                new ScreeningScoreLevel{
                    ScreeningSectionID = "PHQ-9",
                    ScoreLevel = 3,
                    Name = "MODERATE depression severity"
                },
                new ScreeningScoreLevel{
                    ScreeningSectionID = "PHQ-9",
                    ScoreLevel = 4,
                    Name = "MODERATELY SEVERE depression severity"
                },
                new ScreeningScoreLevel{
                    ScreeningSectionID = "PHQ-9",
                    ScoreLevel = 5,
                    Name = "SEVERE depression severity"
                },
            };
        }


        internal static IList<ScreeningScoreLevel> CreateAnxietyItems()
        {
            return
            new ScreeningScoreLevel[]{
                new ScreeningScoreLevel{
                    ScreeningSectionID = ScreeningSectionDescriptor.Anxiety,
                    ScoreLevel = 0,
                    Name = "NONE-MINIMAL anxiety severity"
                },
                new ScreeningScoreLevel{
                    ScreeningSectionID = ScreeningSectionDescriptor.Anxiety,
                    ScoreLevel = 1,
                    Name = "MILD anxiety severity"
                },
                new ScreeningScoreLevel{
                    ScreeningSectionID = ScreeningSectionDescriptor.Anxiety,
                    ScoreLevel = 2,
                    Name = "MODERATE anxiety severity"
                },
                new ScreeningScoreLevel{
                    ScreeningSectionID = ScreeningSectionDescriptor.Anxiety,
                    ScoreLevel = 3,
                    Name = "SEVERE anxiety severity"
                }
            };
        }

        public static IList<ScreeningScoreLevel> CreateSihItems()
        {
            return
            new ScreeningScoreLevel[]{ 
                new ScreeningScoreLevel{
                    ScreeningSectionID = "SIH",
                    ScoreLevel = 0,
                    Name = "NEGATIVE"
                },
                new ScreeningScoreLevel{
                    ScreeningSectionID = "PHQ-9",
                    ScoreLevel =1,
                    Name = "POSITIVE"
                },
            };
        }

        public static IList<ScreeningScoreLevel> CreateTccItems()
        {
            return
            new ScreeningScoreLevel[]{ 
                new ScreeningScoreLevel{
                    ScreeningSectionID = "TCC",
                    ScoreLevel = 0,
                    Name = "NEGATIVE"
                },
                new ScreeningScoreLevel{
                    ScreeningSectionID = "TCC",
                    ScoreLevel =1,
                    Name = "POSITIVE"
                },
            };
        }
    }
}
