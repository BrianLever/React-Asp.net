namespace ScreenDoxKioskLauncher.Infrastructure
{
    using Newtonsoft.Json;
    using System.IO;
    using RestSharp.Serializers;
    using RestSharp.Deserializers;


    public class NewtonsoftRestSharpJsonSerializer : ISerializer, IDeserializer
    {
        private Newtonsoft.Json.JsonSerializer serializer;

        public NewtonsoftRestSharpJsonSerializer(JsonSerializer serializer)
        {
            this.serializer = serializer ?? throw new System.ArgumentNullException(nameof(serializer));
        }

        public string ContentType
        {
            get { return "application/json"; }
            set { }
        }

        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

        public string Serialize(object obj)
        {
            using (var sw = new StringWriter())
            {
                var writer = new JsonTextWriter(sw);

                serializer.Serialize(writer, obj);

                return sw.ToString();
            }

        }

        public T Deserialize<T>(RestSharp.IRestResponse response)
        {
            var content = response.Content;

            using (var sr = new StringReader(content))
            {
                var jsonTextReader = new JsonTextReader(sr);

                return serializer.Deserialize<T>(jsonTextReader);
            }
        }

        public static NewtonsoftRestSharpJsonSerializer Default
        {
            get
            {
                return new NewtonsoftRestSharpJsonSerializer(new Newtonsoft.Json.JsonSerializer()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                });
            }
        }

        public static string SupportedContentType => "application/json";

    }
}
