using System;
using System.Collections.Generic;

namespace FrontDesk.Data
{
    public interface IScreeningKioskDb
    {
        Dictionary<int, AnswerScale> GetAnswerOptions();
        List<AnswerScaleOption> GetAnswerOptions(int answerScaleID);

        void UpdateAgeSettings(ICollection<ScreeningSectionAge> ageSettings);

        ScreeningSectionAge GetMinimalAgeForScreeningSection(string screeningSectionID);

        DateTime? GetMaxAgeSettingsModifiedDateUTC();
    }
}