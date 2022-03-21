using System.IO;

namespace ScreenDoxKioskInstallApi.Models
{
    public class InstallationPackageFile
    {
        public string FileName { get; set; }
        public Stream Content{ get; set; }
    }
}