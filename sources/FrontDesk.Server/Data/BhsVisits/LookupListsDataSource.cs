using Common.Logging;
using FrontDesk.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FrontDesk.Server.Data.BhsVisits
{
    public class LookupListsDataSource : ILookupListsDataSource
    {

        private readonly ILog _log = LogManager.GetLogger<LookupListsDataSource>();

        public List<LookupValue> GetNewVisitRecommendation()
        {
            return new LookupValueDb("NewVisitReferralRecommendation").GetList();
        }

        public List<LookupValue> GetNewVisitReferralRecommendationAccepted()
        {
            return new LookupValueDb("NewVisitReferralRecommendationAccepted").GetList();
        }

        public List<LookupValue> GetReasonNewVisitReferralRecommendationNotAccepted()
        {
            return new LookupValueDb("ReasonNewVisitReferralRecommendationNotAccepted").GetList();
        }

        public List<LookupValue> GetDischarged()
        {
            return new LookupValueDb("Discharged").GetList();
        }

        public List<LookupValue> GetTreatmentAction()
        {
            return new LookupValueDb("TreatmentAction").GetList();
        }

        public List<LookupValue> GetRace()
        {
            return new LookupValueDb("Race").GetList();
        }

        public List<LookupValue> GetGender()
        {
            return new LookupValueDb("Gender").GetList();
        }

        public List<LookupValue> GetSexualOrientation()
        {
            return new LookupValueDb("SexualOrientation").GetList();
        }

        public List<LookupValue> GetMaritalStatus()
        {
            return new LookupValueDb("MaritalStatus").GetList();
        }

        public List<LookupValue> GetEducationLevel()
        {
            return new LookupValueDb("EducationLevel").GetList();
        }

        public List<LookupValue> GetLivingOnReservation()
        {
            return new LookupValueDb("LivingOnReservation").GetList();
        }

        public List<LookupValue> GetMilitaryExperience()
        {
            return new LookupValueDb("MilitaryExperience").GetList();
        }

        public List<LookupValue> GetPatientAttendedVisit()
        {
            return new LookupValueDb("PatientAttendedVisit").GetList();
        }

        public List<LookupValue> GetFollowUpContactOutcome()
        {
            return new LookupValueDb("FollowUpContactOutcome").GetList();
        }

        public List<LookupValue> GetDrugOfChoice()
        {
            return new LookupValueDb("DrugOfChoice").GetList();
        }

        public ILookupValueSource Get(string sourceName)
        {
            try
            {
                return new LookupValueDb(sourceName);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Failed to read from Lookup table. Source Name: {0}", ex, sourceName);

                return null;
            }
        }

        public class VisitListNameDescriptor
        {
            public const string NewVisitReferralRecommendation = "NewVisitReferralRecommendation";
            public const string NewVisitReferralRecommendationAccepted = "NewVisitReferralRecommendationAccepted";
            public const string ReasonNewVisitReferralRecommendationNotAccepted = "ReasonNewVisitReferralRecommendationNotAccepted";
            public const string Discharged = "Discharged";
            public const string TreatmentAction = "TreatmentAction";
            public const string Race = "Race";
            public const string Gender = "Gender";
            public const string SexualOrientation = "SexualOrientation";
            public const string MaritalStatus = "MaritalStatus";
            public const string EducationLevel = "EducationLevel";
            public const string LivingOnReservation = "LivingOnReservation";
            public const string MilitaryExperience = "MilitaryExperience";
            public const string PatientAttendedVisit = "PatientAttendedVisit";
            public const string FollowUpContactOutcome = "FollowUpContactOutcome";
            public const string DrugOfChoice = "DrugOfChoice";

            public static string[] GetSupportedLists()
            {
                var obj = new VisitListNameDescriptor();

                return typeof(VisitListNameDescriptor).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    .Where(fi => fi.IsLiteral && !fi.IsInitOnly).Select(x => (string)x.GetValue(obj)).OrderBy(x => x).ToArray();
            }
        }
    };


       
}
