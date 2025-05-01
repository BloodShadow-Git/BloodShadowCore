namespace BloodShadow.CoreGame.Settings
{
    using BloodShadow.Core.SaveSystem;
    using R3;
    using System;
    using System.IO;

    public class SettingManager<TScreen>
    {
        public IDictionary<Type, SettingData<TScreen>> SettingDatas => _settingDatas;
        private Dictionary<Type, SettingData<TScreen>> _settingDatas;
        public Subject<Unit> SaveSubj { get; private set; } = new();

        public event Action OnSettingSave;
        public event Action OnSettingLoad;

        private readonly SaveSystem _saveSystem;
        public readonly string SavePath;

        public SettingManager(string saveDataPath, string systemDataPath, string settingFile) : this(saveDataPath, systemDataPath, settingFile, new JsonSaveSystem()) { }
        public SettingManager(string saveDataPath, string systemDataPath, string settingFile, SaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
            SavePath = Path.Combine(saveDataPath, systemDataPath, settingFile);
            _settingDatas = [];
            SaveSubj.Subscribe(_ => Save());
            Load();
        }

        public void Load()
        {
            _saveSystem.Load<Dictionary<Type, SettingData<TScreen>>>(SavePath, (data) =>
            {
                if (data != null) { _settingDatas = data; }
                else { _settingDatas = []; }
            });
            OnSettingLoad?.Invoke();
        }
        public void Save()
        {
            foreach (SettingData<TScreen> data in _settingDatas.Values) { data.UpdateData(); }
            _saveSystem.Save(SavePath, _settingDatas);
            OnSettingSave?.Invoke();
        }

        public bool Add<T>(SettingData<TScreen> data) where T : SettingData<TScreen>
        {
            if (_settingDatas.ContainsKey(typeof(T))) { return false; }
            else
            {
                _settingDatas.Add(typeof(T), data);
                return true;
            }
        }
        public bool Remove<T>() where T : SettingData<TScreen> { return true; }
        public T Get<T>(T fallback) where T : SettingData<TScreen> { return fallback; }
    }
}
