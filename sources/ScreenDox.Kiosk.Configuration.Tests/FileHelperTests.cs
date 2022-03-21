using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace ScreenDox.Kiosk.Configuration.Tests
{
    [DeploymentItem(@"Data\App_setup.config", "Data")]
    [TestClass]
    public class FileHelperTests
    {
        [TestMethod]
        public void ReplaceParametersInFIle_Can_Rewrite_Same_File()
        {
            var filePath = @"Data\App_setup.config";

            FileHelper.ReplaceParametersInFIle(
                filePath,
                filePath,
                new System.Collections.Generic.Dictionary<string, string>
                {
                    {"KIOSK_KEY", "XXXXXX" },
                    {"KIOSK_SECRET", "**********" }
                });


            var content = File.ReadAllText(filePath);

            content.Should().Contain(@"<add key=""KioskKey"" value=""XXXXXX"" />");
            content.Should().Contain(@"<add key=""KioskSecret"" value=""**********"" />");

        }
    }
}
