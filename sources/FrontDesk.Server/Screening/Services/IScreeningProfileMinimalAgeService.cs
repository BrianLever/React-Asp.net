using System;
using System.Collections.Generic;

namespace FrontDesk.Server.Screening.Services
{
    public interface IScreeningProfileMinimalAgeService
    {
        IList<ScreeningSectionAge> GetSectionMinimalAgeSettings(int screeningProfileId);
        IList<ScreeningSectionAge> GetSectionMinimalAgeSettings(int screeningProfileId, string screeningSectionID);
        void UpdateMinimalAgeSettings(int screeningProfileId, ICollection<ScreeningSectionAge> ageSettings);
    }
    
}