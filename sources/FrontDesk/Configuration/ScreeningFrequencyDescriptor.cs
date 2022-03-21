using System;

namespace FrontDesk.Configuration
{
    public static class ScreeningFrequencyDescriptor
    {
        public const string ContactFrequencyID = "CIF";

        public static string GetName(string id)
        {
            switch(id)
            {
                case ContactFrequencyID:
                    return Properties.Resources.ScreeningFrequency_Contact_Name;
                case ScreeningSectionDescriptor.SmokerInHome:
                    return Properties.Resources.ScreeningFrequency_SmokerInHome_Name;
                case ScreeningSectionDescriptor.Tobacco:
                    return Properties.Resources.ScreeningFrequency_Tobacco_Name;
                case ScreeningSectionDescriptor.Alcohol:
                    return Properties.Resources.ScreeningFrequency_Alcohol_Name;
                case ScreeningSectionDescriptor.Depression:
                    return Properties.Resources.ScreeningFrequency_Depression_Name;
                case ScreeningSectionDescriptor.DepressionAllQuestions:
                    return Properties.Resources.ScreeningFrequency_PHQ9A_Name;
                case ScreeningSectionDescriptor.PartnerViolence:
                    return Properties.Resources.ScreeningFrequency_PartnerViolence_Name;
                case ScreeningSectionDescriptor.SubstanceAbuse:
                    return Properties.Resources.ScreeningFrequency_SubstanceAbuse_Name;
                case ScreeningSectionDescriptor.Demographics:
                    return Properties.Resources.ScreeningFrequency_Demographics_Name;
                case ScreeningSectionDescriptor.Anxiety:
                    return Properties.Resources.ScreeningFrequency_Anxiety_Name;
                case ScreeningSectionDescriptor.AnxietyAllQuestions:
                    return Properties.Resources.ScreeningFrequency_GAD7A_Name;
                case ScreeningSectionDescriptor.ProblemGambling:
                    return Properties.Resources.ScreeningFrequency_ProblemGambling_Name;
                default:
                    throw new ArgumentOutOfRangeException(id  +" id is not known");

            }
        }

       
    }
}
