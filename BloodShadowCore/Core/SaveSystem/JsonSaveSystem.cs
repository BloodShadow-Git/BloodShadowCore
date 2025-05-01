namespace BloodShadow.Core.SaveSystem
{
    using BloodShadow.Core.Logger;
    using Newtonsoft.Json;
    using System;
    using System.IO;

    public class JsonSaveSystem : SaveSystem
    {
        private readonly JsonSerializerSettings _settings;
        private readonly Logger _logger;

        public JsonSaveSystem()
        {
            _settings = new()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            };
        }

        public JsonSaveSystem(JsonSerializerSettings settings) : this() { _settings = settings; }
        public JsonSaveSystem(Logger logger) : this() { _logger = logger; }
        public JsonSaveSystem(JsonSerializerSettings settings, Logger logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public override void Save(string key, object data, Action<bool> callback = null, bool useBuildPath = true, bool useCheckPath = true)
        {
            try
            {
                string path = key;
                if (useBuildPath) { path = BuildPath(key); }
                if (useCheckPath) { CheckFile(path); }
                using (StreamWriter writer = new(path)) { writer.Write(JsonConvert.SerializeObject(data, _settings)); }
                callback?.Invoke(true);
            }
            catch (Exception ex)
            {
                if (_logger != null)
                {
                    _logger.WriteLineWarning(ex);
                    _logger.WriteLineWarning("Save wrong");
                }
                else
                {
                    Console.WriteLine(ex);
                    Console.WriteLine("Save wrong");
                }
                callback?.Invoke(false);
            }
        }

        public override async void SaveAsync(string key, object data, Action<bool> callback = null, bool useBuildPath = true, bool useCheckPath = true)
        {
            try
            {
                string path = key;
                if (useBuildPath) { path = BuildPath(key); }
                if (useCheckPath) { CheckFile(path); }
                using (StreamWriter writer = new(path)) { await writer.WriteAsync(JsonConvert.SerializeObject(data, _settings)); }
                callback?.Invoke(true);
            }
            catch (Exception ex)
            {
                if (_logger != null)
                {
                    _logger.WriteLineWarning(ex);
                    _logger.WriteLineWarning("Save wrong");
                }
                else
                {
                    Console.WriteLine(ex);
                    Console.WriteLine("Save wrong");
                }
                callback?.Invoke(false);
            }
        }

        public override void SaveToString(object data, Action<bool, string> callback = null)
        {
            try { callback?.Invoke(true, JsonConvert.SerializeObject(data, _settings)); }
            catch (Exception ex)
            {
                if (_logger != null)
                {
                    _logger.WriteLineWarning(ex);
                    _logger.WriteLineWarning("Save wrong");
                }
                else
                {
                    Console.WriteLine(ex);
                    Console.WriteLine("Save wrong");
                }
                callback?.Invoke(false, "");
            }
        }

        public override void Load<T>(string key, Action<T> callback, bool useBuildPath = true, bool useCheckPath = true)
        {
            try
            {
                string path = key;
                if (useBuildPath) { path = BuildPath(key); }
                if (useCheckPath) { CheckFile(path); }
                using StreamReader fileStream = new(path);
                callback?.Invoke(JsonConvert.DeserializeObject<T>(fileStream.ReadToEnd(), _settings));
            }
            catch (Exception ex)
            {
                if (_logger != null)
                {
                    _logger.WriteLineWarning(ex);
                    _logger.WriteLineWarning("Load wrong");
                }
                else
                {
                    Console.WriteLine(ex);
                    Console.WriteLine("Load wrong");
                }
                callback?.Invoke(default);
            }
        }

        public override async void LoadAsync<T>(string key, Action<T> callback, bool useBuildPath = true, bool useCheckPath = true)
        {
            try
            {
                string path = key;
                if (useBuildPath) { path = BuildPath(key); }
                if (useCheckPath) { CheckFile(path); }
                using StreamReader fileStream = new(path);
                string data = await fileStream.ReadToEndAsync();
                callback?.Invoke(JsonConvert.DeserializeObject<T>(data, _settings));
            }
            catch (Exception ex)
            {
                if (_logger != null)
                {
                    _logger.WriteLineWarning(ex);
                    _logger.WriteLineWarning("Load wrong");
                }
                else
                {
                    Console.WriteLine(ex);
                    Console.WriteLine("Load wrong");
                }
                callback?.Invoke(default);
            }
        }

        public override void LoadFromString<T>(string objectString, Action<T> callback)
        {
            try { callback?.Invoke(JsonConvert.DeserializeObject<T>(objectString)); }
            catch (Exception ex)
            {
                if (_logger != null)
                {
                    _logger.WriteLineWarning(ex);
                    _logger.WriteLineWarning("Load wrong");
                }
                else
                {
                    Console.WriteLine(ex);
                    Console.WriteLine("Load wrong");
                }
                callback?.Invoke(default);
            }
        }

        public override void CheckFile(string path)
        {
            if (!Directory.Exists(path)) { Directory.CreateDirectory(Path.GetDirectoryName(path) ?? ""); }
            if (!File.Exists(path)) { File.Create(path).Close(); }
        }

        public override void CheckFolder(string path) { if (!Directory.Exists(path)) { Directory.CreateDirectory(Path.GetDirectoryName(path) ?? ""); } }

        private static string BuildPath(string key) => Path.Combine("./", key);
    }
}