using System;
using System.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScreenDoxKioskInstallApi.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ScreenDoxKioskInstallApi.Tests.Models
{
    [DeploymentItem(@"Data\kiosk-installation-config.yaml", "Data")]
    [TestClass]
    public class InstallationPackageInfoSerialization
    {
        protected InstallationPackageInfo Sut()
        {
            return new InstallationPackageInfo
            {
                Version = "8.0.0.1",
                InstallOn = new DateTime(2020, 6, 1, 3, 1, 0),
                Kiosks = new System.Collections.Generic.List<KioskInstallationInfo>()
                {
                    new KioskInstallationInfo
                    {
                        Key = "PRUW",
                        InstallOn = new DateTime (2020, 6, 10, 3, 1, 0),
                    },
                    new KioskInstallationInfo
                    {
                        Key = "8ZAR",
                        InstallOn = new DateTime (2020, 6, 11, 3, 1, 0),
                    }
                }
            };
        }


        [TestMethod]
        public void CanSerializeToYaml()
        {
            var serializer = new SerializerBuilder().Build();

            var yaml = serializer.Serialize(Sut());

            yaml.Should().NotBeEmpty();
        }

        [TestMethod]
        public void CanDeserializeFromYaml()
        {
            var serializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var expected = Sut();
            InstallationPackageInfo actual;

            using (var sr = File.OpenText(@"data\kiosk-installation-config.yaml"))
            {
                actual = serializer.Deserialize<InstallationPackageInfo>(sr);
            }

            actual.Should().NotBeNull();
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
