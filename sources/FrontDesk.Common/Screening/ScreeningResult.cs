using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml.Serialization;

using FrontDesk.Common.Data;

namespace FrontDesk
{
    /// <summary>
    /// Patient's Screening Result
    /// </summary>
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    [Serializable()]
    [DataContract(Name = "ScreeningResult", Namespace = "http://www.frontdeskhealth.com")]
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed
    public class ScreeningResult : ScreeningPatientIdentityWithAddress, IScreeningResult
    {
        #region Screening Properties
        /// <summary>
        /// ScreeningResultID
        /// </summary>
        [IgnoreDataMember]
        public long ID { get; set; }

        /// <summary>
        /// Reference to the performed Screening
        /// </summary>
        [DataMember]
        public string ScreeningID { get; set; }


        [IgnoreDataMember]
        public int Age
        {
            get
            {
                return GetAge(this.Birthday);
            }
        }

        #endregion

        #region System properties

        /// <summary>
        /// Kiosk unique key, where scrrening has been done
        /// </summary>
        [DataMember]
        public Int16 KioskID { get; set; }

        /// <summary>
        /// Kiosk label
        /// </summary>
        public string KioskLabel { get; internal set; }


        /// <summary>
        /// Branch location ID
        /// </summary>
        public int? LocationID { get; set; }

        /// <summary>
        /// Location label
        /// </summary>
        public string LocationLabel { get; internal set; }


        /// <summary>
        /// Date and time when record has been created
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public DateTimeOffset CreatedDate { get; set; }

        /// <summary>
        /// If true, record is marked as deleted and do not visible on screen
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public bool IsDeleted { get; internal set; }
        /// <summary>
        /// Date and time when record has been deleted
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public DateTimeOffset? DeletedDate { get; set; }
        /// <summary>
        /// Reference to the user's account who deleted this row
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public int? DeletedBy { get; set; }

        #endregion

        #region Export Properties

        /// <summary>
        /// Check if result contains patient info ready for export
        /// </summary>
        public bool IsContactInfoEligableForExport
        {
            get
            {
                return !this.IsEmptyContactInfo();
            }
        }

        /// <summary>
        /// The date and time when report has been exported
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public DateTimeOffset? ExportDate { get; set; }
        /// <summary>
        /// Reference to the user's account who exported this row
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public int? ExportedBy { get; set; }

        /// <summary>
        /// Patient's Row ID in the EHR database to which exported report has been linked
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public int? ExportedToPatientID { get; set; }

        /// <summary>
        /// Visit Rows ID in the EHR database to which exported report has been linked
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public int? ExportedToVisitID { get; set; }

        /// <summary>
        /// Visit's Date in the EHR database to which exported report has been linked
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public DateTime? ExportedToVisitDate { get; set; }

        /// <summary>
        /// Visit's Location in the EHR database to which exported report has been linked
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public string ExportedToVisitLocation { get; set; }


        /// <summary>
        /// True when result can be exported
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public bool IsEligible4Export
        {
            get
            {
                return !ExportDate.HasValue;
            }
        }

        [IgnoreDataMember]
        [XmlIgnore]
        public bool IsEligibleForExportWithoutAddress
        {
            get
            {
                return IsPassedAnySection && !ExportDate.HasValue;
            }
        }
        #endregion

        #region Section Screening Results

        private object _lockListObject = new object();

        [DataMember]
        Dictionary<string, ScreeningSectionResult> _sectionAnswers = new Dictionary<string, ScreeningSectionResult>();
        /// <summary>
        /// Section's question answers
        /// </summary>
        [IgnoreDataMember()]
        [XmlIgnore]
        public List<ScreeningSectionResult> SectionAnswers
        {
            get { lock (_lockListObject) { return new List<ScreeningSectionResult>(_sectionAnswers.Values); } }
        }

        /// <summary>
        /// Add new question answer to the section screening result
        /// </summary>
        /// <param name="answer"></param>
        public void AppendSectionAnswer(ScreeningSectionResult answer)
        {
            lock (_lockListObject)
            {
                //check if answer is already in the collection

                if (_sectionAnswers.ContainsKey(answer.ScreeningSectionID))
                {
                    Debug.Assert(true, "Duplicate section ID in the screening section results.");
                    _sectionAnswers.Remove(answer.ScreeningSectionID);
                }
                else
                {
                    _sectionAnswers.Add(answer.ScreeningSectionID, answer);

                    answer.ScreeningResultID = this.ID;
                }
            }
        }

        /// <summary>
        /// Import range of section answers
        /// </summary>
        /// <param name="answer"></param>
        public void ImportSectionAnswerRange(IEnumerable<ScreeningSectionResult> answers)
        {
            lock (_lockListObject)
            {
                //check if answer is already in the collection
                foreach (var answer in answers)
                {
                    if (_sectionAnswers.ContainsKey(answer.ScreeningSectionID))
                    {
                        Debug.Assert(true, "Duplicate section ID in the screening section results.");
                        _sectionAnswers.Remove(answer.ScreeningSectionID);
                    }

                    _sectionAnswers.Add(answer.ScreeningSectionID, answer);

                    answer.ScreeningResultID = this.ID;
                }

            }
        }

        /// <summary>
        /// Find section in the Section collection by id
        /// </summary>
        /// <param name="sectionID"></param>
        /// <returns>Section answer object if found or null.</returns>
        public ScreeningSectionResult FindSectionByID(string sectionID)
        {
            ScreeningSectionResult section = null;

            _sectionAnswers.TryGetValue(sectionID, out section);

            return section;
        }


        /// <summary>
        /// Get if patient has passed any screening section
        /// </summary>
        public bool IsPassedAnySection
        {
            get
            {
                return SectionAnswers.Any();
            }
        }
        #endregion

        /// <summary>
        /// Create sync object once object has been deserialized. Otherwise will be null reference exception
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            if (this._lockListObject == null) this._lockListObject = new object();
        }


        #region Constructors
        /// <summary>
        /// default parameterless constructor
        /// </summary>
        public ScreeningResult() { }

        public ScreeningResult(IDataReader reader)
        {

            ID = Convert.ToInt64(reader["ScreeningResultID"]);
            ScreeningID = Convert.ToString(reader["ScreeningID"]).TrimEnd();
            FirstName = Convert.ToString(reader["FirstName"]);
            LastName = Convert.ToString(reader["LastName"]);
            MiddleName = Convert.ToString(reader["MiddleName"]);
            Birthday = Convert.ToDateTime(reader["Birthday"]);
            StreetAddress = Convert.ToString(reader["StreetAddress"]);
            City = Convert.ToString(reader["City"]);
            StateID = Convert.ToString(reader["StateID"]);

            ZipCode = Convert.ToString(reader["ZipCode"]);
            Phone = Convert.ToString(reader["Phone"])?.TrimEnd();

            KioskID = Convert.IsDBNull(reader["KioskID"]) ? (short)0 : Convert.ToInt16(reader["KioskID"]);
            CreatedDate = (DateTimeOffset)reader["CreatedDate"];
            IsDeleted = Convert.ToBoolean(reader["IsDeleted"]);
            WithErrors = Convert.ToBoolean(reader["WithErrors"]);




            var cols = DBDatabase.GetReaderColumnNames(reader);
            if (cols.Contains("StateName"))
            {
                StateName = Convert.ToString(reader["StateName"]);
            }
            if (cols.Contains("KioskLabel"))
            {
                KioskLabel = Convert.ToString(reader["KioskLabel"]);
            }
            if (cols.Contains("LocationLabel"))
            {
                LocationLabel = Convert.ToString(reader["LocationLabel"]);
            }
            if (cols.Contains("DeletedDate"))
            {
                DeletedDate = Convert.IsDBNull(reader["DeletedDate"]) ? (DateTimeOffset?)null : (DateTimeOffset)reader["DeletedDate"];
            }
            if (cols.Contains("DeletedBy"))
            {
                DeletedBy = Convert.IsDBNull(reader["DeletedBy"]) ? (int?)null : Convert.ToInt32(reader["DeletedBy"]);
            }
            if (cols.Contains("BranchLocationID"))
            {
                LocationID = Convert.IsDBNull(reader["BranchLocationID"]) ?
                    (int?)null : Convert.ToInt32(reader["BranchLocationID"]);
            }

            #region Export related fields

            if (cols.Contains("ExportDate"))
            {
                ExportDate = Convert.IsDBNull(reader["ExportDate"]) ?
                    (DateTimeOffset?)null : (DateTimeOffset)reader["ExportDate"];
            }

            if (cols.Contains("ExportedBy"))
            {
                ExportedBy = Convert.IsDBNull(reader["ExportedBy"]) ? (int?)null : Convert.ToInt32(reader["ExportedBy"]);
            }
            if (cols.Contains("ExportedToPatientID"))
            {
                ExportedToPatientID = Convert.IsDBNull(reader["ExportedToPatientID"]) ? (int?)null : Convert.ToInt32(reader["ExportedToPatientID"]);
            }
            if (cols.Contains("ExportedToVisitID"))
            {
                ExportedToVisitID = Convert.IsDBNull(reader["ExportedToVisitID"]) ? (int?)null : Convert.ToInt32(reader["ExportedToVisitID"]);
            }
            if (cols.Contains("ExportedToHRN"))
            {
                ExportedToHRN = Convert.ToString(reader["ExportedToHRN"]);
            }
            if (cols.Contains("ExportedToVisitDate"))
            {
                ExportedToVisitDate = Convert.IsDBNull(reader["ExportedToVisitDate"]) ? (DateTime?)null : (DateTime)reader["ExportedToVisitDate"];
            }
            if (cols.Contains("ExportedToVisitLocation"))
            {
                ExportedToVisitLocation = Convert.ToString(reader["ExportedToVisitLocation"]);
            }

            #endregion

        }
        #endregion

        #region Validation

        /// <summary>
        /// Indicates whathever screen result content errros was found during validation process
        /// </summary>
        public bool WithErrors { get; set; }

        /// <summary>
        /// Get patient's age
        /// </summary>
        /// <param name="birthday"></param>
        /// <returns></returns>
        public static int GetAge(DateTime birthday)
        {
            return GetAge(birthday, DateTime.Today);
        }


        /// <summary>
        /// Get patient's age
        /// </summary>
        /// <param name="birthday"></param>
        /// <returns></returns>
        public static int GetAge(DateTime birthday, DateTime today)
        {
            //calculate 
            int age = today.Year - birthday.Year;
            if (today.Month < birthday.Month)
            {
                age--;
            }
            else if (today.Month == birthday.Month && today.Day < birthday.Day)
            {
                age--;
            }

            return age;
        }

        #endregion

    }
}
