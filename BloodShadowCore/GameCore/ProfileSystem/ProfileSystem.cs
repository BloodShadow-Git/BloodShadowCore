namespace BloodShadow.GameCore.ProfileSystem
{
    using BloodShadow.Core.SaveSystem;
    using ObservableCollections;
    using R3;
    using System.Linq;

    public class ProfileSystem
    {
        private const string EXTENSION = ".json";
        private const string EXTENSIONFILTER = "*.json";
        public readonly string ProfilesPath;
        public readonly string TempProfilesPath;

        public event Action OnSave;
        public event Action OnStartSwitchProfile;
        public event Action OnEndSwitchProfile;

        public ReadOnlyReactiveProperty<Profile> Active => _active;
        public IObservableCollection<Profile> Profiles => _profiles;
        public IEnumerable<string> ProfileNames => _profiles.Select(input => input.Name);

        private readonly ReactiveProperty<Profile> _active;
        private readonly ObservableList<Profile> _profiles;
        private readonly FileSystemWatcher _profileWatcher;
        private readonly FileSystemWatcher _tempWatcher;
        private readonly SaveSystem _saveSystem;

        public ProfileSystem(string profilesDir, string tempProfilesDir) : this(profilesDir, tempProfilesDir, new JsonSaveSystem()) { }
        public ProfileSystem(string profilesDir, string tempProfilesDir, SaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
            _active = new();
            _profiles = [];

            ProfilesPath = profilesDir;
            TempProfilesPath = tempProfilesDir;
            _saveSystem.CheckDirectory(ProfilesPath);
            _saveSystem.CheckDirectory(TempProfilesPath);

            _profileWatcher = new(ProfilesPath) { IncludeSubdirectories = false };
            _profileWatcher.Changed += UpdateProfiles;
            _profileWatcher.Deleted += UpdateProfiles;
            _profileWatcher.Created += UpdateProfiles;
            _profileWatcher.Renamed += UpdateProfiles;

            _tempWatcher = new(TempProfilesPath) { IncludeSubdirectories = false };
            _tempWatcher.Changed += UpdateProfiles;
            _tempWatcher.Deleted += UpdateProfiles;
            _tempWatcher.Created += UpdateProfiles;
            _tempWatcher.Renamed += UpdateProfiles;
            UpdateProfiles();
        }

        public bool SelectProfile(int index)
        {
            if (index < 0 || index >= _profiles.Count) { return false; }
            if (_profiles[index].Busy) { return false; }
            if (_active.Value != null) { File.Delete(Path.Combine(TempProfilesPath, GenerateFileName(_active.Value))); }
            OnStartSwitchProfile?.Invoke();
            _active.Value = _profiles[index];
            OnEndSwitchProfile?.Invoke();
            File.Create(Path.Combine(TempProfilesPath, GenerateFileName(_active.Value)));
            return true;
        }

        public static string GenerateFileName(Profile profile) => $"{profile.Name}{EXTENSION}";

        public bool CreateProfile(string name, byte[] icon, out Profile profile)
        {
            profile = new(name, icon, this);
            if (!ValidProfile(profile)) { return false; }
            SaveProfile(profile);
            return true;
        }

        public bool CreateProfile(string name, byte[] icon, IDictionary<(string, Type), object> dict, out Profile profile)
        {
            profile = new(name, icon, this, dict);
            if (!ValidProfile(profile)) { return false; }
            SaveProfile(profile);
            return true;
        }

        private bool ValidProfile(Profile profile)
        {
            UpdateProfiles();
            if (string.IsNullOrEmpty(profile.Name)) { return false; }
            if (ProfileNames.Contains(profile.Name)) { return false; }
            return true;
        }

        private void SaveProfile(Profile profile)
        {
            _saveSystem.Save(Path.Combine(ProfilesPath, GenerateFileName(profile)), profile);
            UpdateProfiles();
        }

        public bool RemoveProfile(Profile profile)
        {
            UpdateProfiles();
            if (string.IsNullOrEmpty(profile.Name)) { return false; }
            if (!ProfileNames.Contains(profile.Name)) { return false; }
            File.Delete(Path.Combine(ProfilesPath, GenerateFileName(profile)));
            UpdateProfiles();
            return true;
        }

        public bool NameAvailable(string name) => !ProfileNames.Contains(name) && !string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(name);

        private void UpdateProfiles(object sender, FileSystemEventArgs args) => UpdateProfiles();
        public void UpdateProfiles()
        {
            _profiles.Clear();
            IEnumerable<FileInfo> profilesFiles = Directory.EnumerateFiles(ProfilesPath, EXTENSIONFILTER).Select(input => new FileInfo(input));
            foreach (FileInfo profileFile in profilesFiles)
            {
                _saveSystem.Load<Profile>(profileFile.FullName, data =>
                {
                    if (data != null) { if (data.Name == Path.GetFileNameWithoutExtension(profileFile.Name)) { _profiles.Add(data); } }
                });
            }
        }

        public void Save()
        {
            OnSave?.Invoke();
            foreach (Profile profile in _profiles) { _saveSystem.Save(Path.Combine(ProfilesPath, GenerateFileName(profile)), profile); }
        }
    }
}
