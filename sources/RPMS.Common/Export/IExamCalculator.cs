using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk;

namespace RPMS.Common.Export
{
    public interface IExamCalculator
    {
        IList<Models.Exam> Calculate(IEnumerable<ScreeningSectionResult> sectionResults);
    }
}
