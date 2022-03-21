using FrontDesk.Common.Configuration;
using FrontDesk.Common.Screening;

using ScreenDox.Server.Common.Configuration;

namespace FrontDesk.Server.Configuration
{
    public class ScreeningAgesDbProvider : ScreeningAgesSettingsProvider
    {

        private readonly ISystemSettingService _systemSettingService;

        public ScreeningAgesDbProvider(ISystemSettingService systemSettingService): base()
        {
            _systemSettingService = systemSettingService ?? throw new System.ArgumentNullException(nameof(systemSettingService));

            LoadData();

            ParseAgeGroups();

        }

        public ScreeningAgesDbProvider() : this (new SystemSettingService())
        {

        }
        

        protected void LoadData()
        {
           
            _ageGroupsSettingString = _systemSettingService.GetByKey("IndicatorReport_AgeGroups", string.Empty);

            if (string.IsNullOrEmpty(_ageGroupsSettingString))
            {
                //read from config file
                _ageGroupsSettingString = AppSettingsProxy.GetStringValue("IndicatorReport_AgeGroups", "0 - 14; 15 - 17; 18 - 25; 26 - 54; 55 or Older");
            }
        }

        public override void Refresh()
        {
            LoadData();
            base.Refresh();

        }

        public ScreeningAgesDbProvider(string ageGroupsSettingValue) : base(ageGroupsSettingValue)
        {
        }
    }
}
