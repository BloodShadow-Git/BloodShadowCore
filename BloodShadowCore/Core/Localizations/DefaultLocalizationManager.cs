namespace BloodShadow.Core.Localizations
{
    public class DefaultLocalizationManager : LocalizationManager
    {
        private readonly Dictionary<(string, string, Type), object> _localizations;

        public DefaultLocalizationManager(params LocalizationData[] datas) : base()
        {
            _localizations = [];
            AddLocalization(datas);
            if (_availableLocalizations.Count > 0) { _currentLocalization.Value = _availableLocalizations[0]; }
        }

        public override void AddLocalization(LocalizationData data)
        {
            _availableLocalizations.Add(data.LocalizationKey);
            foreach (LocalizationPair pair in data.Pairs) { _localizations[(data.LocalizationKey, pair.Key, pair.Value.GetType())] = pair.Value; }
        }
        public override void AddLocalization(params LocalizationData[] datas) { foreach (LocalizationData data in datas) { AddLocalization(data); } }

        public override T Localize<T>(string key)
        {
            if (_localizations.TryGetValue((_currentLocalization.CurrentValue, key, typeof(T)), out object value)) { return (T)value; }
            else { return default; }
        }
    }
}
