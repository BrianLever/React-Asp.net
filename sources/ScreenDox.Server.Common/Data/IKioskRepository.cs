using System;
using System.Collections.Generic;
using System.Data;
using ScreenDox.Server.Common.Models;

namespace ScreenDox.Server.Common.Data
{
    public interface IKioskRepository: FrontDesk.Common.Data.ITransactionalDatabase
    {
        bool ChangeLastActivityDate(KioskPingMessage kioskPingMessage, DateTimeOffset lastActivityDate);
        DateTimeOffset? GetLastActivityDate(short kioskId);

        void Delete(short kioskID);
        DataSet GetAllWithFiltering(int? kioskID, string name, int? branchLocationId, int? screeningProfileId, bool showDisabled, int? userID, int startRowIndex, int maximumRows, string orderBy);
        List<Kiosk> GetAll(int? kioskID, string name, int? branchLocationId, int? screeningProfileId, bool showDisabled, int? userID, int startRowIndex, int maximumRows, string orderBy);

        Kiosk GetByID(short kioskID);
        int GetKioskCount(int? kioskID, string name, int? branchLocationId, int? screeningProfileId, bool showDisabled, int? userID);
        int GetNotDisabledCount();
        void SetKioskEnabledStatus(short kioskID, bool isDisabled);
        void Update(Kiosk kiosk);
        bool KioskNameIsAlreadyUsed(string kioskName);
        short Add(Kiosk kiosk);
    }
}