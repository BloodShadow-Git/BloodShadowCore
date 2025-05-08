using ObservableCollections;
using R3;

namespace BloodShadow.GameCore.Localizations
{
    public abstract class LocalizationManager
    {
        public IObservableCollection<string> AvailableLocalizations => _availableLocalizations;
        public Observable<string> CurrentLocalization => _currentLocalization;

        protected readonly ObservableList<string> _availableLocalizations;
        protected readonly ReactiveProperty<string> _currentLocalization;

        public LocalizationManager()
        {
            _availableLocalizations = new ObservableList<string>();
            _currentLocalization = new ReactiveProperty<string>("ERROR");
        }

        public void SetLocalization(string localization) { if (_availableLocalizations.Contains(localization)) { _currentLocalization.Value = localization; } }
        public void SetLocalization(int index) { if (index >= 0 && index < _availableLocalizations.Count) { SetLocalization(_availableLocalizations[index]); } }
        public abstract void AddLocalization(LocalizationData data);
        public abstract void AddLocalization(params LocalizationData[] datas);

        public abstract T Localize<T>(string key);
    }
}
