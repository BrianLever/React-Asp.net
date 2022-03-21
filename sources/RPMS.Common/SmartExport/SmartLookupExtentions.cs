using FrontDesk;

using RPMS.Common.Comparers;
using RPMS.Common.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ScreenDox.EHR.Common.SmartExport
{
    public static class SmartLookupExtentions
    {
        public static SmartLookupResult<TPatientSearch> FindBestMatch<TPatientSearch>(this List<TPatientSearch> patientList)
            where TPatientSearch : PatientSearch
        {
            TPatientSearch result = null;
            if (patientList != null && patientList.Any())
            {
                result = patientList.OrderBy(x => x.MatchRank).FirstOrDefault(x => x.MatchRank <= PatientComparer.LAST_AND_FIRST_NAME_AND_DOB_MATCH_THRESHOLD);
            }
            return new SmartLookupResult<TPatientSearch>
            {
                BestResult = result,
                AllResuls = patientList,
                Confidence = 1
            };
        }

        public static SmartLookupResult<Patient> SortByBestMatch(this List<Patient> patientList)
        {
            Patient result = null;
            if (patientList != null && patientList.Any())
            {
                result = patientList.OrderBy(x => x.MatchRank)
                    .FirstOrDefault();

            }

            return new SmartLookupResult<Patient>
            {
                BestResult = result,
                AllResuls = patientList,
                Confidence = result?.MatchRank <= PatientSearchComparer.LAST_AND_FIRST_NAME_AND_DOB_MATCH_THRESHOLD ? 1 : 0.9
            };
        }

        public static SmartLookupResult<Visit> FindBestMatch(this List<Visit> visitList, ScreeningResult screeningResult, IReadOnlyList<string> allowedVisitCategories)
        {
            if (allowedVisitCategories == null)
            {
                throw new ArgumentNullException(nameof(allowedVisitCategories));
            }
            visitList = visitList ?? new List<Visit>();

            //asumption - the list is sorted by date desc
            var visitsOnTheSameDate = visitList
                .Where(v => DateTime.Compare(v.Date.Date, screeningResult.CreatedDate.Date) == 0)
                .OrderBy(x => x.Date)
                .ToList();

            Visit selectedVisit = null;
            int categoryIndex = 0;

            while (selectedVisit == null && categoryIndex < allowedVisitCategories.Count)
            {
                var category = allowedVisitCategories[categoryIndex];

                if (category == "*")
                {
                    selectedVisit = visitsOnTheSameDate.FirstOrDefault();
                }
                else if (category == "+")
                {
                    selectedVisit = visitsOnTheSameDate.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.ServiceCategory));
                }
                else
                {
                    selectedVisit = visitsOnTheSameDate.FirstOrDefault(x => x.ServiceCategory == category);
                }

                categoryIndex++;
            }

            if(selectedVisit == null)
            {

            }

            return new SmartLookupResult<Visit>
            {
                BestResult = selectedVisit,
                AllResuls = visitsOnTheSameDate,
                Confidence = 1 - categoryIndex * 0.1
            };
        }
    }
}
