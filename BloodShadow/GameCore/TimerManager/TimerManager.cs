namespace BloodShadow.GameCore.TimerManager
{
    using BloodShadow.Core.SaveSystem;
    using BloodShadow.GameCore.Systems;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public abstract class TimerManager : IUpdateSystem
    {
        private Dictionary<string, TimerData> _timers;
        private readonly string _savePath;
        private readonly SaveSystem _saveSystem;

        public TimerManager(string saveDataPath, string runtimeData, string timersFile) : this(saveDataPath, runtimeData, timersFile, new JsonSaveSystem()) { }
        public TimerManager(string saveDataPath, string runtimeData, string timersFile, SaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
            _timers = new Dictionary<string, TimerData>();
            _savePath = Path.Combine(saveDataPath, runtimeData, timersFile);
            _saveSystem.Load<Dictionary<string, TimerData>>(_savePath, data =>
            {
                if (data != null) { _timers = data; }
                else { _timers = new Dictionary<string, TimerData>(); }
            });
        }

        public void Save() { _saveSystem.Save(_savePath, _timers); }
        public bool AddTimer(string name, float time, Action action, TimerMode mode = TimerMode.OneTime) => _timers.TryAdd(name, new TimerData(time, action, mode));
        public bool RemoveTimer(string name) => _timers.Remove(name);

        public abstract void Update();

        private class TimerData
        {
            public float Time;
            public float EliminatedTime = 0f;
            public Action Action;
            public TimerMode Mode;

            public TimerData(float time, Action action, TimerMode mode)
            {
                Time = time;
                Action = action;
                Mode = mode;
            }
        }
    }
}
