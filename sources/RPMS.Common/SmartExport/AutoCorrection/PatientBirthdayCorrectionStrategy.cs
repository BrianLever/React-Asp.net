using RPMS.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScreenDox.EHR.Common.SmartExport.AutoCorrection
{
    public class PatientBirthdayCorrectionStrategy : PatientCorrectionStrategyBase
    {
        public override IEnumerable<PatientSearch> Apply(PatientSearch patient)
        {
            var originalBirthday = patient.DateOfBirth;
            var day = originalBirthday.Day;
            var month = originalBirthday.Month;
            var year = originalBirthday.Year;

            HashSet<DateTime> birthdays = new HashSet<DateTime>();

            birthdays.Add(originalBirthday);
            AppendDateIfValid(birthdays, year, SwapDigitsInMonth(month), day); // swap digits in month
            AppendDateIfValid(birthdays, year, month, SwapDigitsInDay(day, month, year)); // swap digits in month
            AppendDateIfValid(birthdays, year, day, month); // swap day and month


            foreach(var date in birthdays)
            {
                yield return patient.Clone().SetBirthday(date);
            }
        }

        private ICollection<DateTime> AppendDateIfValid(ICollection<DateTime> listDates, int year, int month, int day)
        {
            if(IsValidateDate(year, month, day))
            {
                listDates.Add(new DateTime(year, month, day));
            }

            return listDates;
        }


        private bool IsValidateDate(int year, int month, int day)
        {
            DateTime dateTime;

            if (DateTime.TryParse($"{month}/{day}/{year}", out dateTime))
            {
                return true;
            }

            return false;
        }

        private int SwapDigits(int number)
        {
            int lower = number % 10;
            int upper = number / 10;

            return lower * 10 + upper;
        }

        private int SwapDigitsInDay(int day, int month, int year)
        {
            var newDay = SwapDigits(day);

            //validating new date
            DateTime dateTime;
            if (DateTime.TryParse($"{month}/{newDay}/{year}", out dateTime))
            {
                return newDay;
            }
            else
            {
                return day;
            }
        }

        private int SwapDigitsInMonth(int month)
        {
            if (month > 10) return month;

            return SwapDigits(month);
        }

        public override IEnumerable<string> GetModificationsLogDescription()
        {
            return new string[] { "Swap month and day and swaped digits in day and month." };
        }
    }
}
