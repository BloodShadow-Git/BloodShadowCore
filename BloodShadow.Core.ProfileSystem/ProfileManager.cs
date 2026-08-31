using BloodShadow.Core.SaveSystem.StorageModule;
using ObservableCollections;
using R3;

namespace BloodShadow.Core.ProfileSystem
{
    public class ProfileManager
    {
        private const string EXTENSION = ".json";
        private const string EXTENSIONFILTER = $"*{EXTENSION}";
        public readonly StorageKey ProfilesPath;
        public readonly StorageKey TempProfilesPath;

        public Subject<Unit> OnSave;
        public Subject<Unit> OnStartSwitchProfile;
        public Subject<Unit> OnEndSwitchProfile;

        public ReadOnlyReactiveProperty<string> Active => _active;
        public IObservableCollection<KeyValuePair<string, Profile>> Profiles => _profiles;
        public IObservableCollection<string> BusyProfiles => _busyProfiles;
        public IDictionary<string, Profile> Items => _profiles;

        public Profile this[string key] => _profiles[key];

        private readonly ReactiveProperty<string> _active;
        private readonly ObservableDictionary<string, Profile> _profiles;
        private readonly ObservableList<string> _busyProfiles;
        private readonly StorageWatcher _profileWatcher;
        private readonly StorageWatcher _tempWatcher;
        private readonly SaveSystem.SaveSystem _saveSystem;

        public ProfileManager(StorageKey profilesDir, StorageKey tempProfilesDir, SaveSystem.SaveSystem saveSystem) : base()
        {
            OnSave = new();
            OnStartSwitchProfile = new();
            OnEndSwitchProfile = new();

            _saveSystem = saveSystem;
            _active = new ReactiveProperty<string>();
            _profiles = [];
            _busyProfiles = [];

            ProfilesPath = profilesDir;
            TempProfilesPath = tempProfilesDir;
            _saveSystem.VerifyCollection(ProfilesPath, true);
            _saveSystem.VerifyCollection(TempProfilesPath, true);

            _profileWatcher = saveSystem.CreateWatcher(ProfilesPath);
            _profileWatcher.Filter = EXTENSIONFILTER;
            _profileWatcher.OnChanged.Subscribe(_ => UpdateProfiles());
            _profileWatcher.OnDeleted.Subscribe(_ => UpdateProfiles());
            _profileWatcher.OnCreated.Subscribe(_ => UpdateProfiles());
            _profileWatcher.OnRenamed.Subscribe(_ => UpdateProfiles());

            _tempWatcher = saveSystem.CreateWatcher(TempProfilesPath);
            _tempWatcher.Filter = EXTENSIONFILTER;
            _tempWatcher.OnChanged.Subscribe(_ => UpdateBusyProfiles());
            _tempWatcher.OnDeleted.Subscribe(_ => UpdateBusyProfiles());
            _tempWatcher.OnCreated.Subscribe(_ => UpdateBusyProfiles());
            _tempWatcher.OnRenamed.Subscribe(_ => UpdateBusyProfiles());

            UpdateProfiles();
            UpdateBusyProfiles();
        }

        public bool SelectProfile(Profile profile) => SelectProfile(profile.ID);
        public bool SelectProfile(string name)
        {
            if (!_profiles.ContainsKey(name)) { return false; }
            if (IsBusy(_profiles[name])) { return false; }
            if (_active.Value != null) { _saveSystem.RemoveResource(TempProfilesPath + GenerateFileName(_active.Value)); }
            OnStartSwitchProfile.OnNext(Unit.Default);
            _active.Value = _profiles[name].ID;
            OnEndSwitchProfile.OnNext(Unit.Default);
            _saveSystem.CreateResource(TempProfilesPath + GenerateFileName(_active.CurrentValue));
            return true;
        }

        public static StorageKey GenerateFileName(Profile profile) => new($"{profile.ID}{EXTENSION}");
        public static StorageKey GenerateFileName(string profileID) => new($"{profileID}{EXTENSION}");

        public bool CreateProfile(string name, out Profile profile) => CreateProfile(name, null, null, out profile);
        public bool CreateProfile(string name, byte[]? icon, out Profile profile) => CreateProfile(name, icon, null, out profile);
        public bool CreateProfile(string name, IDictionary<string, object>? dict, out Profile profile) => CreateProfile(name, null, dict, out profile);
        public bool CreateProfile(string name, byte[]? icon, IDictionary<string, object>? dict, out Profile profile)
        {
            profile = new Profile(name, icon, dict);
            if (!ValidProfile(profile)) { return false; }
            _saveSystem.Save(ProfilesPath + GenerateFileName(profile), (ProfileData)profile);
            return true;
        }

        private bool ValidProfile(Profile profile)
        {
            if (string.IsNullOrEmpty(profile.ID) || _profiles.ContainsKey(profile.ID)) { return false; }
            return true;
        }

        public bool RemoveProfile(Profile profile)
        {
            if (string.IsNullOrEmpty(profile.ID) ||
                _busyProfiles.Contains(profile.ID) ||
                string.Equals(_active.CurrentValue, profile.ID))
            { return false; }
            _saveSystem.RemoveResource(ProfilesPath + GenerateFileName(profile));
            return true;
        }

        public bool NameAvailable(string name) => !_profiles.ContainsKey(name) && !string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(name);
        private void UpdateProfiles(object sender, FileSystemEventArgs args) => UpdateProfiles();
        private void UpdateBusyProfiles(object sender, FileSystemEventArgs args) => UpdateBusyProfiles();

        public void UpdateProfiles()
        {
            IEnumerable<Resource> profilesEntries = _saveSystem.EnumerateCollection(ProfilesPath)
                    .Where(path => string.Equals(Path.GetExtension(path.Name), EXTENSIONFILTER))
                    .Where(resource => resource.ResourceType.HasFlag(ResourceType.Resource));
            Dictionary<string, Profile> profiles = [];
            foreach (Resource profileFile in profilesEntries)
            {
                ProfileData? profileData = _saveSystem.Load<ProfileData>(new(profileFile.Name));
                if (profileData != null && profileData.ID == Path.GetFileNameWithoutExtension(profileFile.Name)) { profiles.Add(profileData.ID, profileData); }
            }

            _profiles.Clear();
            foreach (var item in profiles) { _profiles.Add(item); }
        }
        public void UpdateBusyProfiles()
        {
            IEnumerable<Resource> profilesEntries = _saveSystem.EnumerateCollection(ProfilesPath)
                    .Where(path => string.Equals(Path.GetExtension(path.Name), EXTENSIONFILTER))
                    .Where(resource => resource.ResourceType.HasFlag(ResourceType.Resource));
            List<string> busyProfiles = [];
            foreach (Resource profileFile in profilesEntries) { busyProfiles.Add(Path.GetFileNameWithoutExtension(profileFile.Name)); }

            _busyProfiles.Clear();
            _busyProfiles.AddRange(busyProfiles);
        }

        public void Save()
        {
            OnSave.OnNext(Unit.Default);
            foreach (KeyValuePair<string, Profile> kvp in _profiles)
            { _saveSystem.Save(ProfilesPath + GenerateFileName(kvp.Value), (ProfileData)kvp.Value); }
        }

        public bool IsBusy(Profile profile) => _busyProfiles.Contains(profile.ID);
        public bool Contains(Profile item) => Contains(item.ID);
        public bool Contains(string key) => _profiles.ContainsKey(key);

        private class ProfileData
        {
            public string ID;
            public string Name;
            public byte[] Icon;
            public Dictionary<string, object> CustomData;

            public ProfileData(string id, string name, byte[] icon, IDictionary<string, object> customData)
            {
                ID = id;
                Name = name;
                Icon = icon;
                if (customData != null) { CustomData = new Dictionary<string, object>(customData); }
                else { CustomData = []; }
            }

            public static implicit operator Profile(ProfileData data) => new(data.ID, data.Name, data.Icon, data.CustomData);
            public static implicit operator ProfileData(Profile profile) => new(profile.ID, profile.Name.CurrentValue, profile.Icon.CurrentValue, profile.CustomData);
        }
    }
}
