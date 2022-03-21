using Common.Logging;

using FrontDesk.Common.Configuration;
using FrontDesk.Common.Debugging;

using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace FrontDesk.Common.Screening
{
    public class ScreeningAgesSettingsProvider : IScreeningAgesSettingsProvider
    {

        private readonly ILog _logger = LogManager.GetLogger<ScreeningAgesSettingsProvider>();

        protected string _ageGroupsSettingString;

        public string[] AgeGroupsLabels { get; private set; }

        public int[] AgeGroups { get; private set; }

        public static string RegexValidationExpression
        {
            get { return @"^((\d+\s?-\s?\d+\s?)[;,]?|(\d+\s?or\s+[oO]lder\s?)[',;]?)+$"; }
        }
        public static string RegexMatchExpression
        {
            get { return @"(\d+\s?-\s?\d+\s?)[',]?|(\d+\s?or\s+[oO]lder\s?)[',]?"; }
        }


        public ScreeningAgesSettingsProvider()
        {
            _ageGroupsSettingString = AppSettingsProxy.GetStringValue("IndicatorReport_AgeGroups", "0 - 14; 15 - 17; 18 - 25; 26 - 54; 55 or Older");
            
            ParseAgeGroups();
        }

        public ScreeningAgesSettingsProvider(string ageGroupsSettingValue)
        {
            if (string.IsNullOrWhiteSpace(ageGroupsSettingValue))
            {
                throw new ArgumentNullException(nameof(ageGroupsSettingValue));
            }

            _ageGroupsSettingString = ageGroupsSettingValue;

            ParseAgeGroups();
        }

        public virtual void Refresh()
        {
            ParseAgeGroups();
        }

        protected void ParseAgeGroups()
        {
            _logger.TraceFormat("Age group settings: {0}", _ageGroupsSettingString);

            Regex regexp = new Regex(RegexMatchExpression, RegexOptions.IgnoreCase);

            AgeGroupsLabels = regexp.Matches(_ageGroupsSettingString)
                .Cast<Match>()
                .Select(x => x.Value)
                .ToArray();

            AgeGroups = AgeGroupsLabels.Select(x =>
            {
                var value = x;
                try
                {
                    var spaceIndex = value.IndexOfAny(new[] { ' ', '-' });
                    if (spaceIndex > 0)
                    {
                        return Int32.Parse(value.Substring(0, spaceIndex));
                    }
                }
                catch (FormatException ex)
                {
                    _logger.WarnFormat("Cannot parse Age number from Label string {0}", ex, value);
                }
                return 0;
            }).ToArray();
        }


    }
}
