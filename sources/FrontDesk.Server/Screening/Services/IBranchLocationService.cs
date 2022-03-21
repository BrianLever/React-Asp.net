using ScreenDox.Server.Models;

using System.Collections.Generic;

namespace FrontDesk.Server.Screening.Services
{
    public interface IBranchLocationService
    {
        int Add(BranchLocation model);
        SearchResponse<BranchLocation> GetAll(BranchLocationSearchFilter filter);
        //int CountAll();
        //int CountAll(string filterByName, bool showDisabled, int? screeningProfileID);
        bool Delete(int id);
        BranchLocation Get(int id);
        List<BranchLocation> GetAccesibleCheckInLocationsForCurrentUser();
        List<BranchLocation> GetAll();
        //List<BranchLocation> GetAll(int startRowIndex, int maximumRows, string orderBy);
        //List<BranchLocation> GetAll(string filterByName, bool showDisabled, int? screeningProfileID, int startRowIndex, int maximumRows, string orderBy);
        List<BranchLocation> GetAllForDisplay();
        List<BranchLocation> GetForUserID(int userID);
        int GetNotDisabledCount();
        bool HasActiveKiosk(int branchLocationID);
        void SetDisabledStatus(int branchLocationID, bool makeDisabled);
        bool Update(BranchLocation model);
        int GetScreeningProfileByKioskID(short kioskID);
    }
}