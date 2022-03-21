using System.Collections.Generic;

namespace FrontDesk.Configuration
{
    public interface IScreeningProfileFrequencyService
    {
        ScreeningFrequencyItem Get(int screeningProfileId, string ID);
        IEnumerable<ScreeningFrequencyItem> GetAll(int screeningProfileId);
        void Save(int screeningProfileId, IEnumerable<ScreeningFrequencyItem> items);
        void Save(int screeningProfileId, ScreeningFrequencyItem item);
        Dictionary<int, string> GetSupportedValues();
    }
}