using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models
{
    public class VisitSettingItemViewModel
    {
        /// <summary>
        /// Measure Tool Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Measure Tool Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Minimal Score to trigger creating visit
        /// </summary>
        public int CutScore { get; set; }
        /// <summary>
        /// Hint of available scores and severities for the Measure Tool
        /// </summary>
       public string SectionScoreHint { get; set; }

        /// <summary>
        /// Only Measure Tools in the “On” position will create a Visit when a patient screens positive for these problems.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Get the UTC date and time when record has been modified lat time
        /// </summary>
        public DateTime LastModifiedDateUTC { get; set; }
    }
}
