using FrontDesk.Common.Data;
using System.Collections.Generic;

namespace FrontDesk.Server.Data
{
    public interface IBranchLocationDb : ITransactionalDatabase
    {
        int Add(BranchLocation branchLocation);
        int CountAll(string filterByName, bool showDisabled, int? screeningProfileId);
        bool Delete(int id);
        BranchLocation Get(int id);
        List<BranchLocation> GetAll();
        List<BranchLocation> GetAll(string filterByName, bool showDisabled, int? screeningProfileId, int startRowIndex, int maximumRows, string orderBy);
        List<BranchLocation> GetForUserID(int userID);
        int GetNotDisabledCount();
        bool HasActiveKiosk(int branchLocationID);
        bool Update(BranchLocation branchLocation);
        void SetBranchLocationDisabledStatus(int branchLocationID, bool isDisabled);
        int? GetScreeningProfileByKioskID(short kiokID);
    }
}