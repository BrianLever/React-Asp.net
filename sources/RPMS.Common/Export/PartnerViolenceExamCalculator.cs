using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk;
using RPMS.Common.Models;
using ScreenDox.EHR.Common.Properties;
using RPMS.Common.Configuration;

namespace RPMS.Common.Export
{
    public class PartnerViolenceExamCalculator : AbstractCalculator<Exam>, IExamCalculator
    {
        public override IEnumerable<FrontDesk.ScreeningSectionResult> FilterSupportedSections(IEnumerable<FrontDesk.ScreeningSectionResult> sectionResults)
        {
            return sectionResults.Where(x => x.ScreeningSectionID == ScreeningSectionDescriptor.PartnerViolence);
        }


        protected override IList<Exam> CalculateFilteredResults(IEnumerable<FrontDesk.ScreeningSectionResult> filteredSections)
        {
            List<Exam> examList = new List<Exam>();
            Exam exam = null;
            var result = filteredSections.First();
            if (result.ScoreLevel.HasValue)
            {
                var cfg = RpmsExportConfiguration.GetConfiguration().Exams[ScreeningSectionDescriptor.PartnerViolence];
                exam = new Exam
                {
                    ExamName = cfg.Name,
                    ExamID = cfg.Id,
                    Code = cfg.Code,
                    Comment = Resources.ExamCommentTemplate.FormatWith(result.Score, result.ScoreLevelLabel)
                };

                if (result.ScoreLevel.Value > 0)
                {
                    exam.Result = ResultKeys.Present;
                }
                else
                {
                    exam.Result = ResultKeys.NormalNegative;
                }

                exam.ResultLabel = RpmsExportConfiguration.GetConfiguration().Results[exam.Result].Value;

                examList.Add(exam);
            }
            return examList;
        }
    }
}
