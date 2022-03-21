using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;


namespace RPMS.Common.Models
{
    /// <summary>
    /// Patient Visit
    /// </summary>
    [DataContract(Name = "Visit", Namespace = "http://www.screendox.com")]
    public class Visit
    {
        /// <summary>
        /// Visit ID key
        /// </summary>
        [DataMember]
        public int ID { get; set; }

        /// <summary>
        /// Date of Visit
        /// </summary>
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// Service Category
        /// </summary>
        [DataMember]
        public string ServiceCategory { get; set; }

        /// <summary>
        /// Visit location
        /// </summary>
        [DataMember]
        public Location Location { get; set; }

        public Visit()
        {
            Location = new Location();
        }



        public static string GetServiceCategoryName(string serviceCategoryCode)
        {
            switch (serviceCategoryCode)
            {
                case "A": return "AMBULATORY";
                case "H": return "HOSPITALIZATION";
                case "I": return "IN HOSPITAL";
                case "C": return "CHART REVIEW";
                case "T": return "TELECOMMUNICATIONS";
                case "N": return "NOT FOUND";
                case "S": return "DAY SURGERY";
                case "O": return "OBSERVATION";
                case "E": return "EVENT (HISTORICAL)";
                case "R": return "NURSING HOME";
                case "D": return "DAILY HOSPITALIZATION DATA";
                case "X": return "ANCILLARY PACKAGE DAILY DATA";
                default: return String.Empty;
            }
        }

    }
}
