using FrontDesk;
using FrontDesk.Configuration;

using System.Runtime.Serialization;

namespace ScreenDox.Server.Models
{
    [DataContract(Namespace = "http://www.screendox.com")]
    public class ScreeningSectionAgeItemViewModel : ScreeningSectionAge
    {
        /// <summary>
        /// Measure Tool name for UI
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// When True, the Screening Tool should not be allowed to change and should be hidden.
        /// This values is configured at installation time.
        /// </summary>
        [DataMember]
        public bool IsHidden { get; set; } = false;
    }
}
