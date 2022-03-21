using FrontDesk.Configuration;

using System.Collections.Generic;

namespace FrontDesk.Server.Data.ScreenngProfile
{
    public interface IScreeningProfileFrequencyRepository
    {
        void Save(int screeningProfileId, IEnumerable<ScreeningFrequencyItem> items);

        ScreeningFrequencyItem Get(int screeningProfileId, string ID);
        IEnumerable<ScreeningFrequencyItem> GetAll(int screeningProfileId);


    }
}
