using BloodShadow.Core.Logger;
using ObservableCollections;
using R3;

namespace BloodShadow.Core.Localization
{
    public class LocalizationManager
    {
        public IObservableCollection<string> AvailableLocalizations => _availableLocalizations;
        public Observable<string> CurrentLocalization => _currentLocalization;
        public IObservableCollection<(Type, string)> RegisteredKeys => _registeredKeys;

        protected readonly ObservableList<string> _availableLocalizations = [];
        protected readonly ReactiveProperty<string> _currentLocalization = new("ERROR");
        protected readonly ObservableHashSet<(Type, string)> _registeredKeys = [];
        private readonly Dictionary<(string, string, Type), object> _localizations = [];
        protected LoggerLabel Label;

        public LocalizationManager(params LocalizationData[] datas)
        {
            Label = new(GetType().Name);
            AddLocalization(datas);
            if (_availableLocalizations.Count > 0) { _currentLocalization.Value = _availableLocalizations[0]; }
        }

        public virtual bool RegisterKey<T>(string key)
        {
            Type type = typeof(T);
            Label.WriteLine(MessageChanel.INFO, "Register key \"{0}\" for \"{1}\"", null, null, key, type.Name);
            bool result = _registeredKeys.Add((type, key));
            if (result) { Label.WriteLine(MessageChanel.DEBUG, "Key \"{0}\" for \"{1}\" added", null, null, key, type.Name); }
            return result;
        }
        public void SetLocalization(string localization)
        {
            Label.WriteLine(MessageChanel.INFO, "Setting key \"{0}\"", null, null, localization);
            if (_availableLocalizations.Contains(localization))
            {
                Label.WriteLine(MessageChanel.DEBUG, "Key \"{0}\" setted", null, null, localization);
                _currentLocalization.Value = localization;
            }
        }
        public void SetLocalization(int index) { if (index >= 0 && index < _availableLocalizations.Count) { SetLocalization(_availableLocalizations[index]); } }
        public virtual void AddLocalization(LocalizationData data)
        {
            if (!_availableLocalizations.Contains(data.LocalizationKey)) { _availableLocalizations.Add(data.LocalizationKey); }
            foreach (LocalizationPair pair in data.Pairs)
            {
                (string lang, string key, Type type) key = (data.LocalizationKey, pair.Key, pair.Value.GetType());
                if (_localizations.ContainsKey(key)) { Label.WriteLine(MessageChanel.DEBUG, "Overriding key ({0} - {1}/{2})", null, null, key.lang, key.key, key.type.Name); }
                _localizations[key] = pair.Value;
            }
        }
        public void AddLocalization(params LocalizationData[] datas) { foreach (var data in datas) { AddLocalization(data); } }

        public T Localize<T>(string key)
        {
            if (!_registeredKeys.Contains((typeof(T), key)))
            {
                Label.WriteLine(MessageChanel.FATAL, "Key \"{0}\" for \"{1}\" not registered", new(1), null, key, typeof(T));
                throw new Exception("Key not registered");
            }
            return LocalizeInternal<T>(key);
        }
        protected virtual T LocalizeInternal<T>(string key)
        {
            if (_localizations.TryGetValue((_currentLocalization.CurrentValue, key, typeof(T)), out object? value)) { return (T)value; }
            else { return default!; }
        }
    }
}
