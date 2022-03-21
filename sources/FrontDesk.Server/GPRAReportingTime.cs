using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FrontDesk.Server.Extensions;

namespace FrontDesk.Server
{
    public class GPRAReportingTime
    {
        #region constants

        //range STARTING with July 01 of each year and ENDING with June 30 of each year. The Government Performance Results Act (GPRA) reporting period is from July 01 to June 30 of each year.
        public const int PeriodStartMonth = 10;
        public const int PeriodStartDay = 1;

        #endregion

        #region Properties
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Label
        {
            get
            {
                return string.Format("{0:MM/dd/yyyy} - {1:MM/dd/yyyy}", this.StartDate, this.EndDate);
            }
        }

        public int Year
        {
            get
            {
                return this.StartDate.Year;
            }
        }

        #endregion

        public GPRAReportingTime() { }

        public GPRAReportingTime(int year)
        {
            StartDate = new DateTime(year, PeriodStartMonth, PeriodStartDay);
            EndDate = StartDate.AddYears(1).AddDays(-1);

        }
        /// <summary>
        /// Get the list of GPRA periods starting from given date and till today
        /// </summary>
        /// <param name="starteDate">The the most late date to cover</param>
        /// <returns>The list of GPRA periods</returns>
        public static List<GPRAReportingTime> GetPeriodsSince(DateTime startedDate)
        {
            return GetPeriodsSince(startedDate, DateTime.Today);
        }

        /// <summary>
        /// Get the list of GPRA periods starting from given date and till today
        /// </summary>
        /// <param name="starteDate">The the most late date to cover</param>
        /// <param name="today">Today</param>
        /// <returns>The list of GPRA periods</returns>
        public static List<GPRAReportingTime> GetPeriodsSince(DateTime starteDate, DateTime today)
        {
            starteDate = starteDate.Date; //cut off time

            LinkedList<GPRAReportingTime> periodList = new LinkedList<GPRAReportingTime>();

            int thisYear = today.Year;
            int startYear = starteDate.Year;

            var startPeriod = new GPRAReportingTime(startYear);

            //periodList.AddFirst();

            if (starteDate < startPeriod.StartDate)
            {
                periodList.AddFirst(new GPRAReportingTime(startYear - 1));
            }


            for (int year = startYear; year < thisYear; year++)
            {
                periodList.AddFirst(new GPRAReportingTime(year));
            }

            var first = periodList.FirstOrDefault();
            if (first == null || today > first.EndDate)
            {
                periodList.AddFirst(new GPRAReportingTime(thisYear));
            }

            return periodList.ToList();
        }

        /// <summary>
        /// Get current GPRA year
        /// </summary>
        public static GPRAReportingTime Current { get { return GetCurrent(DateTime.Today); } }

        /// <summary>
        /// Get current GPRA year
        /// </summary>
        /// <param name="today"></param>
        /// <returns></returns>
        public static GPRAReportingTime GetCurrent(DateTime today)
        {
            int year = today.Year;
            if (today.Month < GPRAReportingTime.PeriodStartMonth ||
                (today.Month == GPRAReportingTime.PeriodStartMonth && today.Day < GPRAReportingTime.PeriodStartDay))
            {
                --year; //still previous GPRA year
            }
            return new GPRAReportingTime(year);

        }


        #region Frequency related methods
        /// <summary>
        /// Get the effective date for current GPRA screening period considering screening frequency settings
        /// </summary>
        /// <param name="currentDate"></param>
        /// <param name="screeningFrequencyInMonths">Screening interval in months, 0 - screen every visit, 1 - 1 per months, etc.</param>
        /// <returns></returns>
        public DateTime GetGPRAFrequencyEffectiveDateInMonths(DateTime currentDate, int screeningFrequencyInMonths)
        {
            if (screeningFrequencyInMonths < 0 || screeningFrequencyInMonths > 12)
            {
                throw new ArgumentOutOfRangeException("screeningFrequencyInMonths is a number of monthes in the GPRA year and could be from 0 to 12");
            }

            var currentGpraYear = GPRAReportingTime.GetCurrent(currentDate);
            if (screeningFrequencyInMonths < 12 && screeningFrequencyInMonths > 0)
            {

                int monthsFromYStart = currentDate.GetDiffInMonths(currentGpraYear.StartDate);

                return currentGpraYear.StartDate.AddMonths((monthsFromYStart / screeningFrequencyInMonths) * screeningFrequencyInMonths);
            }
            else
            {
                return currentGpraYear.StartDate;
            }
        }


        /// <summary>
        /// Get the effective date for current GPRA screening period considering screening frequency settings
        /// </summary>
        /// <param name="currentDate"></param>
        /// <param name="screeningFrequencyInDays">Screening interval in months, 0 - screen every visit, 1 - 1 per months, etc.</param>
        /// <returns></returns>
        public DateTime GetGPRAFrequencyEffectiveDateInDays(DateTime currentDate, int screeningFrequencyInDays)
        {
            if (screeningFrequencyInDays <= 0 || screeningFrequencyInDays > 30)
            {
                throw new ArgumentOutOfRangeException("screeningFrequencyInDays is a number of days in the GPRA year and could be from 1 to 30");
            }

            var currentGpraYear = GPRAReportingTime.GetCurrent(currentDate);

            DateTime today = currentDate.Date;

            var startDate = today.AddDays(-1 * (screeningFrequencyInDays - 1));

            return startDate > currentGpraYear.StartDate ? startDate : currentGpraYear.StartDate;

        }
        #endregion
    }
}
