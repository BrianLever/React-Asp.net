using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk
{
    public abstract class Screening
    {


        #region properties

        public string ScreeningID { get; set; }
        public string ScreeningName { get; set; }

        #endregion

        public List<ScreeningSection> Sections = new List<ScreeningSection>();

        /// <summary>
        /// Find section in the Section collection by id
        /// </summary>
        /// <param name="sectionID"></param>
        /// <returns>Section object if found or null.</returns>
        public ScreeningSection FindSectionByID(string sectionID)
        {
            ScreeningSection section = null;
            if (this.Sections.Count > 0)
            {
                int index = 0;
                for (; index < this.Sections.Count; index++)
                {
                    if (this.Sections[index].ScreeningSectionID == sectionID)
                    {
                        section = this.Sections[index];
                        break;
                    }
                }
            }
            return section;
        }
    }
}
