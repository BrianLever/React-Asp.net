using FrontDesk;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Models;

using ScreenDox.Server.Models;

using System;
using System.Collections.Generic;

namespace ScreenDox.Server.Services
{
    public interface IScreenService
    {
        DateTimeOffset? GetMinDate();
        List<PatientCheckInViewModel> GetRelatedPatientScreenings(long mainRowID, ScreeningResultFilterModel filter);
        ScreeningResultResponse GetScreeningResultView(long screeningResultID);
        SearchResponse<UniquePatientScreenViewModel> GetUniquePatientScreens(PagedScreeningResultFilterModel filter);

        ScreeningResult GetScreeningResult(long screeningResultID);
        bool Delete(long id);
    }
}