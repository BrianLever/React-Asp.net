using System.Collections.Generic;

namespace RPMS.Common.GlobalConfiguration
{
    public interface IGlobalSettingsRepository
    {
        GlobalSettingsItem Get(string key);
        List<GlobalSettingsItem> GetAll();
    }
}