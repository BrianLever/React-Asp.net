using System.Collections.Generic;
using FrontDesk.Common;

namespace FrontDesk.Server.Data.BhsVisits
{
    public interface ILookupListsDataSource
    {
        List<LookupValue> GetDischarged();
        List<LookupValue> GetDrugOfChoice();
        List<LookupValue> GetEducationLevel();
        List<LookupValue> GetFollowUpContactOutcome();
        List<LookupValue> GetGender();
        List<LookupValue> GetLivingOnReservation();
        List<LookupValue> GetMaritalStatus();
        List<LookupValue> GetMilitaryExperience();
        List<LookupValue> GetNewVisitRecommendation();
        List<LookupValue> GetNewVisitReferralRecommendationAccepted();
        List<LookupValue> GetPatientAttendedVisit();
        List<LookupValue> GetRace();
        List<LookupValue> GetReasonNewVisitReferralRecommendationNotAccepted();
        List<LookupValue> GetSexualOrientation();
        List<LookupValue> GetTreatmentAction();

        /// <summary>
        /// Get lookup list source interface by source name Use LookupListDescriptor to get the source names
        /// </summary>
        /// <param name="sourceName">Values from LookupListDescriptor</param>
        /// <returns></returns>
        ILookupValueSource Get(string sourceName);
    }
}