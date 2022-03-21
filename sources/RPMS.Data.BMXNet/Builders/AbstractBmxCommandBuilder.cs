using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPMS.Data.BMXNet.Builders
{
    public abstract class AbstractBmxCommandBuilder
    {
        public const char fieldDelimeter = (char)30;

        protected string FormatDataTime(DateTime dateTime)
        {
            return DateToFileMan(dateTime); // dateTime.ToString("M/d/yyyy HH:mm:ss");
        }

        /// <summary>
        /// Copy from (trac.opensourcevista.net\svn\BMXNET_RPMS_dotNET_UTILITIES-BMX\branch\BMX41000\IHS BMX Framework\IndianHealthService.BMXNet\M.cs)
        /// </summary>
        /// <param name="aDateTime"></param>
        /// <returns></returns>
        public static string DateToFileMan(DateTime aDateTime)
        {
            int yearsSince1700 = (int)aDateTime.Year - 1700;
            TimeSpan timeOfDay = aDateTime.TimeOfDay;

            if (timeOfDay.TotalSeconds == 0)
            {
                return yearsSince1700.ToString("000") + aDateTime.ToString("MMdd");
            }
            else
            {
                return yearsSince1700.ToString("000") + ((timeOfDay.Seconds == 0) ? aDateTime.ToString("MMdd.HHmm") : aDateTime.ToString("MMdd.HHmmss"));
            }
        }
    }
}
