using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDesk.Server.Screening.Models
{
    public class ScreeningsByScoreLevelCountResult
    {
        public List<SectionScoreLevelCountResult> Results { get; set; } = new List<SectionScoreLevelCountResult>();

        public SectionScoreLevelCountResult GetSection(string sectionId)
        {
            return Results.FirstOrDefault(x => x.SectionID == sectionId);
        }

        public ScreeningsByScoreLevelCountResult Insert(string sectionId, int scoreLevel, int count)
        {
            var item = Results.LastOrDefault(x => x.SectionID == sectionId); 

            if (item == null)
            {
                item = new SectionScoreLevelCountResult()
                {
                    SectionID = sectionId,

                };
                Results.Add(item);
            }

            try
            {
                item.ScoreLevelCount.Add(scoreLevel, count);
            }
            catch(ArgumentException)
            {
                item.ScoreLevelCount[scoreLevel] = count;
            }


            return this;
        }
    }
}
