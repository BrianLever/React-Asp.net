using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Common.Screening
{
    public static class ScreeningSectionResultExtensions
    {
        public static bool HasSuicidalIdeation(this ScreeningSectionResult sectionResult)
        {
            if (sectionResult == null || sectionResult.ScreeningSectionID != ScreeningSectionDescriptor.Depression)
            {
                return false;
            }

            if (!sectionResult.Answers.Any())
            {
                return false;
            }

            var answers =
                sectionResult.GetAnswersOnSpecificQuestions(new[] { ScreeningSectionDescriptor.DepressionThinkOfDeathQuestionID })
                    .FirstOrDefault();
            if (answers != null && answers.AnswerValue > 0)
            {
                return true;
            }

            return false;

        }

        public static DepressionScreeningSectionResult AsDepressionSection(this ScreeningSectionResult sectionResult)
        {

            if (sectionResult == null || sectionResult.ScreeningSectionID != ScreeningSectionDescriptor.Depression)
            {
                return null; ;
            }

            return new DepressionScreeningSectionResult(sectionResult);
        }

        public static AnxietyScreeningSectionResult AsAnxietySection(this ScreeningSectionResult sectionResult)
        {

            if (sectionResult == null || sectionResult.ScreeningSectionID != ScreeningSectionDescriptor.Anxiety)
            {
                return null; ;
            }

            return new AnxietyScreeningSectionResult(sectionResult);
        }
    }
}
