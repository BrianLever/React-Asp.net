using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace RPMS.Common.Configuration
{
    public class RpmsExportConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("healthFactors")]
        [ConfigurationCollection(typeof(RpmsElementCollection<HealthFactorElement>))]
        public RpmsElementCollection<HealthFactorElement> HealthFactors
        {
            get
            {
                return (RpmsElementCollection<HealthFactorElement>)base["healthFactors"];
            }
        }

        [ConfigurationProperty("exams")]
        [ConfigurationCollection(typeof(RpmsElementCollection<ExamElement>))]
        public RpmsElementCollection<ExamElement> Exams
        {
            get
            {
                return (RpmsElementCollection<ExamElement>)base["exams"];
            }
        }

        [ConfigurationProperty("results")]
        [ConfigurationCollection(typeof(NameValueConfigurationCollection))]
        public NameValueConfigurationCollection Results
        {
            get
            {
                return (NameValueConfigurationCollection)base["results"];
            }
        }

        [ConfigurationProperty("crisisAlerts")]
        [ConfigurationCollection(typeof(RpmsElementCollection<CrisisAlertElement>))]
        public RpmsElementCollection<CrisisAlertElement> CrisisAlerts
        {
            get
            {
                return (RpmsElementCollection<CrisisAlertElement>)base["crisisAlerts"];
            }
        }

        private static Lazy<RpmsExportConfiguration> _lazyInstance = new Lazy<RpmsExportConfiguration>(() => (RpmsExportConfiguration)ConfigurationManager.GetSection("rpmsExportConfiguration"));

        public static RpmsExportConfiguration GetConfiguration()
        {
            return _lazyInstance.Value;
        }

    }

}
