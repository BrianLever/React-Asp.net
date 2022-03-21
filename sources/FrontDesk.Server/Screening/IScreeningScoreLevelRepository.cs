using System;
using System.Collections.Generic;
namespace FrontDesk.Server.Screening
{
    public interface IScreeningScoreLevelRepository
    {
        IList<ScreeningScoreLevel> GetScoreLevelsBySectionID(string sectionID);
    }
}
