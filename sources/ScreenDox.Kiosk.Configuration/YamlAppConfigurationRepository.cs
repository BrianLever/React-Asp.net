namespace ScreenDox.Kiosk.Configuration
{
    using Common.Logging;

    using System.IO;

    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.NamingConventions;

    public class YamlAppConfigurationRepository : IAppConfigurationRepository
    {
        private readonly ILog _logger = LogManager.GetLogger<YamlAppConfigurationRepository>();

        public AppConfiguration Read(string appConfigurationFileFullPath)
        {
            AppConfiguration result = null;

            var serializer = new DeserializerBuilder()
               .WithNamingConvention(CamelCaseNamingConvention.Instance)
               .IgnoreUnmatchedProperties()
               .Build();
            var filePath = appConfigurationFileFullPath;

            _logger.TraceFormat("Reading configuration from YAML file. File path: {0}", filePath);

            using (StreamReader sr = File.OpenText(filePath))
            {
                result = serializer.Deserialize<AppConfiguration>(sr);
            }

            return result;
        }

        public void Write(AppConfiguration configuration, string appConfigurationFileFullPath)
        {
            var serializer = new SerializerBuilder()
               .WithNamingConvention(CamelCaseNamingConvention.Instance)
               .Build();
            var filePath = appConfigurationFileFullPath;

            _logger.TraceFormat("Writing configuration to YAML file. File path: {0}", filePath);
           
            using (StreamWriter sw = File.CreateText(filePath))
            {
                serializer.Serialize(sw, configuration);
            }
        }
    }
}
