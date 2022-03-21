using System;

namespace FrontDesk.Server.Screening
{
    public static class ScreeningScoringFactory
    {
        private static IScreeningScoreLevelRepository repository = new ScreeningScoreLevel();

        /// <summary>
        /// Create Scoring object for specified Screening Section
        /// </summary>
        /// <param name="screeningSectionResult"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">screeningSectionResult argument is null</exception>
        /// <exception cref="System.ArgumentException">Unknown value in screeningSectionResult.ScreeningSectionID. Cannot create ScreeningScoring derived class</exception>
        public static ScreeningScoring CreateScreeningScoring(ScreeningSectionResult screeningSectionResult, ScreeningSection sectionInfo)
        {
            if (screeningSectionResult == null)
            {
                throw new ArgumentNullException("screeningSectionResult");
            }

            return CreateScoringObjectForSection(screeningSectionResult.ScreeningSectionID, screeningSectionResult, sectionInfo);
        }


        private static ScreeningScoring CreateScoringObjectForSection(string screeningSectionID, ScreeningSectionResult screeningSectionResult, ScreeningSection sectionInfo)
        {
            ScreeningScoring scoring;
            if (screeningSectionID == ScreeningSectionDescriptor.Tobacco || screeningSectionID == ScreeningSectionDescriptor.SmokerInHome) //add Smoker in the Home to the tobacco. They have the same scoring
            {
                scoring = new TobaccoScreeningScoring(screeningSectionResult, sectionInfo, repository);
            }
            else if (screeningSectionID == ScreeningSectionDescriptor.Alcohol)
            {
                scoring = new AlhoholScreeningScoring(screeningSectionResult, sectionInfo, repository);
            }
            else if (screeningSectionID == ScreeningSectionDescriptor.SubstanceAbuse)
            {
                scoring = new SubstanceAbuseScreeningScoring(screeningSectionResult, sectionInfo, repository);
            }
            else if (screeningSectionID == ScreeningSectionDescriptor.Depression)
            {
                scoring = new DepressionScreeningScoring(screeningSectionResult, sectionInfo, repository);
            }
            else if (screeningSectionID == ScreeningSectionDescriptor.PartnerViolence)
            {
                scoring = new PartnerViolenceScreeningScoring(screeningSectionResult, sectionInfo, repository);
            }
            else if (screeningSectionID == ScreeningSectionDescriptor.DrugOfChoice)
            {
                scoring = new PartnerViolenceScreeningScoring(screeningSectionResult, sectionInfo, repository);
            }
            else if (screeningSectionID == ScreeningSectionDescriptor.Anxiety)
            {
                scoring = new AnxietyScreeningScoring(screeningSectionResult, sectionInfo, repository);
            }
            else if (screeningSectionID == ScreeningSectionDescriptor.ProblemGambling)
            {
                scoring = new ProblemGamblingScreeningScoring(screeningSectionResult, sectionInfo, repository);
            }
            else
            {
                throw new ArgumentException(string.Concat("Unknown Screening Section '", screeningSectionID, "'"));
            }
            return scoring;
        }



    }
}
