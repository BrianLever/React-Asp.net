using System;
using System.Data;
using ScreenDox.Server.Common.Models;
using ScreenDox.Server.Models;

namespace ScreenDox.Server.Common.Services
{
    public interface IKioskService
    {
        Kiosk GetByID(short kioskID);
        DataSet GetAllWithFiltering(short? kioskID, string filterByName, int? branchLocationID, int? screeningProfileID, bool showDisabled, int? userID, int startRowIndex, int maximumRows, string orderBy);
        int GetKioskCount(short? kioskID, string filterByName, int? branchLocationID, int? screeningProfileID, bool showDisabled, int? userID);
        bool ValidateKiosk(short kioskID, string secret);
        bool ValidateKiosk(string kioskKey, string secret);
        int GetNotDisabledCount();
        bool ChangeLastActivityDate(KioskPingMessage message, DateTimeOffset now);
        bool ChangeLastActivityDate(short kioskID, DateTimeOffset now);

        DateTimeOffset? GetLastActivityDate(short kioskID);

        bool CheckKioskIsExistsAndNotDisabled(short kioskID);

        short Add(Kiosk kiosk, int maxKioskCountAllowed);
        void Delete(short kioskID);

        void Update(Kiosk kiosk);
        void SetDisabledStatus(short kioskID, bool isDisabled, int maxKioskCountAllowed);

        bool KioskNameIsAlreadyUsed(string kioskName);

        bool TestKioskInstallation(short kioskID);
        bool TestKioskInstallation(string kioskKey);
        SearchResponse<Kiosk> GetAll(KioskSearchFilter filter, int? filterForUserId);
    }
}