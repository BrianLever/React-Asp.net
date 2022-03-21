using FrontDesk;

using System;

namespace ScreenDox.Server.Models.ViewModels
{
    /// <summary>
    /// Patient search results. Includes patient info from Screendox and EHR system.
    /// </summary>
    public class PatientSearchResponse
    {
        public SearchResponse<PatientSearchInfoMatch> Screendox = new SearchResponse<PatientSearchInfoMatch>();

        public SearchResponse<PatientSearchInfoMatch> Ehr = new SearchResponse<PatientSearchInfoMatch>();

    }
}
