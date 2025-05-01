namespace BloodShadow.CoreGame.ProfileSystem
{
    using Newtonsoft.Json;
    using R3;

    public class Profile
    {
        public string Name { get; private set; }
        public ReactiveProperty<byte[]> Icon { get; private set; }
        public IDictionary<Type, object> CustomDatas => _customDatas;
        [JsonIgnore] public bool Busy => File.Exists(Path.Combine(ParentProfileSystem.TempProfilesPath, ProfileSystem.GenerateFileName(this)));
        [JsonIgnore] public ProfileSystem ParentProfileSystem { get; set; }
        private readonly Dictionary<Type, object> _customDatas = [];

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
        public Profile(string name, byte[] icon, ProfileSystem parent, IDictionary<Type, object> dict) : this(name, icon, parent) { _customDatas = new(dict); }

        public void AddCustomData<T>(T data) { if (!_customDatas.ContainsKey(typeof(T))) { _customDatas[typeof(T)] = data; } }
        public T GetCustomData<T>() where T : new()
        {
            if (_customDatas.ContainsKey(typeof(T))) { return (T)_customDatas[typeof(T)]; }
            else
            {
                _customDatas[typeof(T)] = new T();
                return (T)_customDatas[typeof(T)];
            }
        }
    }
}
