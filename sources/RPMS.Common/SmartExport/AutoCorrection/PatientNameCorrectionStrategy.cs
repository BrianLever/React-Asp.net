using RPMS.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScreenDox.EHR.Common.SmartExport.AutoCorrection
{
    public class PatientNameCorrectionStrategy : PatientCorrectionStrategyBase
    {
        public override IEnumerable<PatientSearch> Apply(PatientSearch patient)
        {
            var clone = patient.Clone();

            // add names as is
            var uniqueNames = new HashSet<string> {
                patient.LastName?.Trim() ?? string.Empty,
                patient.FirstName?.Trim() ?? string.Empty,
                patient.MiddleName?.Trim() ?? string.Empty
            };

            var originalNames = new List<string>(uniqueNames);

            foreach (var name in originalNames)
            {
                if (name.Contains(" "))
                {
                    uniqueNames.Add(name.Replace(" ", "-"));
                }
                else if (name.Contains("-"))
                {
                    uniqueNames.Add(name.Replace("-", " "));
                }

                if (name.Contains("."))
                {
                    // remove stop if following space
                    uniqueNames.Add(name.Replace(". ", " "));

                    // replace stop with space and remove double space
                    uniqueNames.Add(name.Replace(".", " ").Replace("  ", " "));
                }

                var parts = name.Split(new char[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in parts)
                {
                    uniqueNames.Add(item);
                }
            }

            clone.MiddleName = string.Empty;

            var names = uniqueNames.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            uniqueNames = null;


            for (var lastNameIndex = 0; lastNameIndex < names.Count; lastNameIndex++)
            {
                clone.LastName = names[lastNameIndex];

                for (var firstNameIndex = 0; firstNameIndex < names.Count; firstNameIndex++)
                {
                    if (firstNameIndex == lastNameIndex)
                    {
                        continue; // skips when equal
                    }

                    clone.FirstName = names[firstNameIndex];

                    //handle the case when only last and first name available
                    if (names.Count <= 2)
                    {
                        yield return clone.Clone();
                    }

                    for (var middleNameIndex = 0; middleNameIndex < names.Count; middleNameIndex++)
                    {
                        if (middleNameIndex == firstNameIndex || middleNameIndex == lastNameIndex)
                        {
                            continue; // skips when equal
                        }

                        var middleName = names[middleNameIndex] ?? string.Empty;

                        clone.MiddleName = middleName.Length > 0 ? middleName.Substring(0, 1) : string.Empty;

                        yield return clone.Clone();
                    }
                }
            }
        }

        public override IEnumerable<string> GetModificationsLogDescription()
        {
            return new string[] { "Swap last, first and middle name and try to split names." };
        }
    }
}
