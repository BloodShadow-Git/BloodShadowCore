using System.Text;
using Newtonsoft.Json;

namespace BloodShadow.Core.SaveSystem.SerializeModule
{
    public class JsonSerializeModule : SerializeModule
    {
        private readonly JsonSerializerSettings _settings;

        public JsonSerializeModule()
        {
            _settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            };
        }

        public JsonSerializeModule(JsonSerializerSettings settings) : this() { _settings = settings; }

        protected override byte[] SerializeInternal(object obj) => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj, _settings));
        protected override Task<byte[]> SerializeAsyncInternal(object obj) => Task.Run(() => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj, _settings)));
        protected override T DeserializeInternal<T>(byte[] source) => (T)JsonConvert.DeserializeObject(Encoding.UTF8.GetString(source))!;
        protected override Task<T> DeserializeAsyncInternal<T>(byte[] source) => Task.Run(() => (T)JsonConvert.DeserializeObject(Encoding.UTF8.GetString(source))!);
    }
}
