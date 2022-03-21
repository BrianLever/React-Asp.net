using System.Runtime.Serialization;

namespace FrontDesk.Configuration
{
    public class VisitSettingItem : ScreeningParameterBase
    {
        [DataMember]
        /// <summary>
        /// Measure Tool Id
        /// </summary>
        public string Id { get; set; }

        [IgnoreDataMember]
        public string Name { get; set; }

        [IgnoreDataMember]
        private int _cutScore = 1;
        
        [DataMember]
        /// <summary>
        /// Minimal Score to trigger creating visit
        /// </summary>
        public int CutScore
        {
            get
            {
                return _cutScore;
            }
            set
            {
                if (value <= 1)
                {
                    _cutScore = 1;
                }
                else
                {
                    _cutScore = value;
                }
            }
        }

        [DataMember]
        /// <summary>
        /// Only Measure Tools in the “On” position will create a Visit when a patient screens positive for these problems.
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}
