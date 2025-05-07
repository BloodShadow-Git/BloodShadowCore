namespace BloodShadow.GameCore.ProfileSystem
{
    using Newtonsoft.Json;
    using R3;

    public class Profile
    {
        public string Name { get; private set; }
        public ReactiveProperty<byte[]> Icon { get; private set; }
        public IDictionary<(string, Type), object> CustomDatas => _customDatas;
        [JsonIgnore] public bool Busy => File.Exists(Path.Combine(ParentProfileSystem.TempProfilesPath, ProfileSystem.GenerateFileName(this)));
        [JsonIgnore] public ProfileSystem ParentProfileSystem { get; set; }
        private readonly Dictionary<(string, Type), object> _customDatas = [];

        public Profile(string name, ProfileSystem parent)
        {
            Name = name;
            Icon = new();
            ParentProfileSystem = parent;
        }
        public Profile(string name, byte[] icon, ProfileSystem parent)
        {
            Name = name;
            Icon = new(icon);
            ParentProfileSystem = parent;
        }
        public Profile(string name, byte[] icon, ProfileSystem parent, IDictionary<(string, Type), object> dict) : this(name, icon, parent) { _customDatas = new(dict); }

        public void AddCustomData<T>(string key, T data) { if (!_customDatas.ContainsKey((key, typeof(T)))) { _customDatas[((key, typeof(T)))] = data; } }
        public T GetCustomData<T>(string key)
        {
            if (_customDatas.ContainsKey((key, typeof(T)))) { return (T)_customDatas[(key, typeof(T))]; }
            else
            {
                _customDatas[(key, typeof(T))] = default;
                return (T)_customDatas[(key, typeof(T))];
            }
        }
    }
}
