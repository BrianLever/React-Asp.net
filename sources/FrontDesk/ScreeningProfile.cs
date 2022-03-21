using FrontDesk.Common.Extensions;

using System.Data;

namespace FrontDesk
{
    public class ScreeningProfile
    {

        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// The id of the default screening profile that is pre-built and cannot be deleted. 
        /// </summary>
        public static int DefaultProfileID = 1;

    }


    public static class ScreeningProfileFactory
    {
        public static ScreeningProfile Create(IDataReader reader)
        {
            return new ScreeningProfile()
            {
                ID = reader.Get<int>("ID"),
                Name = reader.Get<string>("Name"),
                Description = reader.Get<string>("Description") ?? string.Empty,
            };
        }
    }
}
