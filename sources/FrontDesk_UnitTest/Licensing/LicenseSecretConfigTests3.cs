using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ScreenDox.Licensing;

using System;
using System.Configuration;

namespace FrontDesk_UnitTest.Licensing
{
    [TestClass]
    [DeploymentItem("configs/screendox.Secrets.config", "configs")]
    public class LicenseSecretConfigTests3
    {
        [TestMethod]
        public void Can_Open_Configu()
        {
            var section = LicenseSecretsConfig.GetConfiguration();

            section.Should().NotBeNull();
        }

        [TestMethod]
        public void Can_Read_License_Key()
        {
            LicenseSecretsConfig.GetConfiguration().LicenseEncryptionKey.Should().NotBeEmpty();
        }

        [TestMethod]
        public void Can_Read_Activation_Key()
        {
            var section = ConfigurationManager.GetSection("screendox.Secrets") as SecretConfigSection;

            section.ActivationRequestEncryptionKey.Should().NotBeEmpty();
        }
    }
}
