using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPMS.Common.Models;
using FrontDesk;
using FrontDesk.Common.Screening;
using RPMS.Common.Configuration;
using ScreenDox.EHR.Common.Properties;

namespace RPMS.Common.Export
{
    public class DepressionExamCalculator : AbstractCalculator<Exam>, IExamCalculator
    {


        public override IEnumerable<FrontDesk.ScreeningSectionResult> FilterSupportedSections(IEnumerable<FrontDesk.ScreeningSectionResult> sectionResults)
        {
            return sectionResults.Where(x => x.ScreeningSectionID == ScreeningSectionDescriptor.Depression);
        }

        protected override IList<Exam> CalculateFilteredResults(IEnumerable<FrontDesk.ScreeningSectionResult> filteredSections)
        {
            List<Exam> examList = new List<Exam>();
            Exam exam = null;
            var result = filteredSections.First();
            if (result.ScoreLevel.HasValue)
            {
                var cfg = RpmsExportConfiguration.GetConfiguration().Exams[ScreeningSectionDescriptor.Depression];
                exam = new Exam
                {
                    ExamName = cfg.Name,
                    ExamID = cfg.Id,
                    Code = cfg.Code,
                    Comment = (result.HasSuicidalIdeation()?
											Resources.ExamSuicidalCommentTemplate : 
										Resources.ExamCommentTemplate).FormatWith(result.Score, result.ScoreLevelLabel)
                };

                if (result.ScoreLevel.Value > 0)
                {
                    exam.Result = ResultKeys.Positive;
                }
                else
                {
                    exam.Result = ResultKeys.Negative;
                }
                exam.ResultLabel = RpmsExportConfiguration.GetConfiguration().Results[exam.Result].Value;

                examList.Add(exam);
            }
            return examList;
        }
    }
}
