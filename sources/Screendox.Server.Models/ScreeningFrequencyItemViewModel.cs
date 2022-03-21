using FrontDesk.Configuration;

using System.Runtime.Serialization;

namespace ScreenDox.Server.Models
{
    [DataContract(Namespace = "http://www.screendox.com")]
    public class ScreeningFrequencyItemViewModel : ScreeningFrequencyItem
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
