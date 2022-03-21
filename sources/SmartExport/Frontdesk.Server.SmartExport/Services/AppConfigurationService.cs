using FrontDesk.Server.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontdesk.Server.SmartExport.Services
{
    public class AppConfigurationService : IAppConfigurationService
    {
        /// <summary>
        /// True if need to simulate export
        /// </summary>
        /// <returns></returns>
        public bool GetRunIsTestModeFlag()
        {
            return AppSettings.GetIntValue("RunInTestMode", 1) == 1;
        }

        /// <summary>
        /// The filter for visit cateogies to export
        /// </summary>
        /// <returns></returns>
        /// <![CDATA[
        /// * - allow any category, including empty string
        /// + - allow any category, excluding empty string
        /// EHR filter:  AMBULATORY,CHART REVIEW, TELECOMMUNICATIONS 
        /// ]]>
        public string[] GetAllowedVisitCategories()
        {
            string value = AppSettings.GetStringValue("AllowedVisitCategories", "*");

            return value.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
        }

        public int ExportAttemptCountOnIgnore
        {
            get
            {
                return AppSettings.GetIntValue("ExportAttemptCountOnIgnore", 10);
            }
        }
        public int ExportAttemptCountOnFailure
        {
            get
            {
                return AppSettings.GetIntValue("ExportAttemptCountOnFailure", 50);
            }
        }


}
}
