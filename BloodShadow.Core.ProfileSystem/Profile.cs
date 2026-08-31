using R3;

namespace BloodShadow.Core.ProfileSystem
{
    public class Profile
    {
        public string ID { get; private set; }
        public ReactiveProperty<string> Name { get; private set; }
        public ReactiveProperty<byte[]> Icon { get; private set; }
        public IDictionary<string, object> CustomData => _customData;

        private readonly Dictionary<string, object> _customData;
        public Profile(string name) : this(Guid.NewGuid().ToString(), name, [], null) { }
        public Profile(string name, byte[]? icon) : this(Guid.NewGuid().ToString(), name, icon, null) { }
        public Profile(string name, byte[]? icon, IDictionary<string, object>? dict) : this(Guid.NewGuid().ToString(), name, icon, dict) { }
        public Profile(string name, IDictionary<string, object>? dict) : this(Guid.NewGuid().ToString(), name, [], dict) { }
        public Profile(string id, string name, byte[]? icon, IDictionary<string, object>? dict)
        {
            ID = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            Name = new(name);
            if (icon != null) { Icon = new(icon); }
            else { Icon = new(); }
            if (dict != null) { _customData = new(dict); }
            else { _customData = []; }
        }

        public void AddCustomData(string key, object data) { if (!_customData.ContainsKey(key)) { _customData[key] = data; } }
        public T GetCustomData<T>(string key)
        {
            if (_customData.TryGetValue(key, out object? value)) { return (T)value; }
            else
            {
                _customData[key] = default!;
                return (T)_customData[key];
            }
        }

    }
}
