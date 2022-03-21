using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace FrontDesk
{
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    [DataContract(Name = "ScreeningSectionAge", Namespace = "http://www.frontdeskhealth.com")]
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed
    public class ScreeningSectionAge : ScreeningParameterBase
    {
        #region properties
        [DataMember]
        public string ScreeningSectionID { get; set; }




        private byte? _minimalAge = null; //default age for each section 0 (no limitations)
        /// <summary>
        /// Minimal age for the patient to be screened with questions from this section
        /// </summary>
        /// <remarks>Automatically tracks minimal value poperty changes and updates LastModifiedDateUTC</remarks>
        [DataMember]
        public byte MinimalAge
        {
            get
            {

                return _minimalAge ?? 0;
            }
            set
            {
                if (!_minimalAge.HasValue) { _minimalAge = value; } //initial evaluation
                //update record if value has been changed and change the last modified date
                else { if (_minimalAge.Value != value) { _minimalAge = value; _lastModifiedDateUTC = DateTime.UtcNow; } }

            }
        }

        private bool? _isEnabled = null;
        
        /// <summary>
        /// If Section turned On or Off
        /// </summary>
        [DataMember]
        public bool IsEnabled
        {
            get
            {
                return _isEnabled ?? true;
            }
            set
            {
                if (!_isEnabled.HasValue) { _isEnabled = value; } //initial evaluation
                //update record if value has been changed and change the last modified date
                else { if (_isEnabled.Value != value) { _isEnabled = value; _lastModifiedDateUTC = DateTime.UtcNow; } }
            }
        }

        #endregion

        #region constructors

        public ScreeningSectionAge() { }

        /// <summary>
        /// init minimal age parameter from database
        /// </summary>
        /// <param name="screeningSectionID"></param>
        /// <param name="minimalAge"></param>
        /// <param name="lastModifiedDateUTC"></param>
        public ScreeningSectionAge(string screeningSectionID, byte minimalAge, bool isEnabled, DateTime lastModifiedDateUTC)
            : this(screeningSectionID, string.Empty, minimalAge, isEnabled, false, lastModifiedDateUTC)
        {

        }

        public ScreeningSectionAge(string screeningSectionID, string screeningSectionLabel, byte minimalAge, bool isEnabled, bool ageIsNotConfigurable, DateTime lastModifiedDateUTC)
        {
            if (string.IsNullOrEmpty(screeningSectionID)) throw new ArgumentNullException("screeningSectionID");
            this.ScreeningSectionID = screeningSectionID;
            this.ScreeningSectionLabel = screeningSectionLabel;
            this.MinimalAge = minimalAge;
            this.IsEnabled = isEnabled;
            this.LastModifiedDateUTC = lastModifiedDateUTC;
            this.AgeIsNotConfigurable = ageIsNotConfigurable;
        }

        #endregion


        #region UI Help Properties
        /// <summary>
        /// Item Label
        /// </summary>
        public string ScreeningSectionLabel { get; set; }
        [IgnoreDataMember]
        public bool AgeIsNotConfigurable { get; set; } = false;

        /// <summary>
        /// True if item should allow managing minimum age settings. Otherwise it's hidden. 
        /// Applicable for different modes of the same tools (i.e. GAD 2/7 and GAD-7).
        /// </summary>
        public bool IsAgeEditControlVisible { get { return !AgeIsNotConfigurable; } }

        #endregion



        //groups / dependencies
        public static ScreeningMinimalAgeGroup[] GetScreeningMinimalAgeGroups()
        {
            return new ScreeningMinimalAgeGroup[]
            {
                new ScreeningMinimalAgeGroup
                {
                    PrimarySectionID = ScreeningSectionDescriptor.Depression,
                    AlternativeSectionID = ScreeningSectionDescriptor.DepressionAllQuestions,
                    DependentSectionIDs = new string[]
                    {
                        ScreeningSectionDescriptor.DepressionAllQuestions
                    }
                },
                new ScreeningMinimalAgeGroup
                {
                    PrimarySectionID = ScreeningSectionDescriptor.Anxiety,
                    AlternativeSectionID = ScreeningSectionDescriptor.AnxietyAllQuestions,
                    DependentSectionIDs = new string[]
                    {
                        ScreeningSectionDescriptor.AnxietyAllQuestions
                    }
                },

                new ScreeningMinimalAgeGroup
                {
                    PrimarySectionID = ScreeningSectionDescriptor.SubstanceAbuse,
                    AlternativeSectionID = string.Empty,
                    DependentSectionIDs = new string[]
                    {
                        ScreeningSectionDescriptor.DrugOfChoice
                    }
                }
};
        }
    }


    [DataContract(Name = "ScreeningMinimalAgeGroup", Namespace = "http://www.frontdeskhealth.com")]
    public struct ScreeningMinimalAgeGroup
    {
        [DataMember]
        public string PrimarySectionID { get; set; }

        /// <summary>
        /// Only one section can be enabled, Primary or Alternative or both disabled
        /// </summary>
        [DataMember]
        public string AlternativeSectionID { get; set; }

        /// <summary>
        /// Depedent section should not have own ScreeningSection parameter. It need to be overriten from Primary
        /// </summary>
        [DataMember]
        public string[] DependentSectionIDs { get; set; }
    }



}
