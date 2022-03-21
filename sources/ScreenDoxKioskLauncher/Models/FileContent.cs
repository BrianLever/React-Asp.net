using System.IO;

namespace ScreenDoxKioskInstallApi.Models
{
    /// <summary>
    /// File info
    /// </summary>
    public class FileContent
    {
        /// <summary>
        /// File name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Binary file content
        /// </summary>
        public byte[] Content{ get; set; }
    }
}