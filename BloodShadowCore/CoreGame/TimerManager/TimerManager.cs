namespace BloodShadow.CoreGame.TimerManager
{
    using BloodShadow.Core.SaveSystem;
    using BloodShadow.CoreGame.Systems;

    public class TimerManager : IUpdateSystem
    {
        private Dictionary<string, TimerData> _timers;
        private readonly string _savePath;
        private readonly SaveSystem _saveSystem;

        public TimerManager(string saveDataPath, string runtimeData, string timersFile) : this(saveDataPath, runtimeData, timersFile, new JsonSaveSystem()) { }
        public TimerManager(string saveDataPath, string runtimeData, string timersFile, SaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
            _savePath = Path.Combine(saveDataPath, runtimeData, timersFile);
            _saveSystem.Load<Dictionary<string, TimerData>>(_savePath, data =>
            {
                if (data != null) { _timers = data; }
                else { _timers = []; }
            });
        }

        public void Save() { _saveSystem.Save(_savePath, _timers); }
        public bool AddTimer(string name, float time, Action action, TimerMode mode = TimerMode.OneTime) => _timers.TryAdd(name, new TimerData(time, action, mode));
        public bool RemoveTimer(string name) => _timers.Remove(name);

        public void Update(in float delta)
        {
            List<string> toRemove = [];
            foreach (KeyValuePair<string, TimerData> timer in _timers)
            {
                timer.Value.EliminatedTime += delta;
                if (timer.Value.EliminatedTime >= timer.Value.Time)
                {
                    timer.Value.Action?.Invoke();
                    switch (timer.Value.Mode)
                    {
                        default:
                        case TimerMode.OneTime: toRemove.Add(timer.Key); break;
                        case TimerMode.Period: timer.Value.EliminatedTime -= timer.Value.Time; break;
                    }
                }
            }
            foreach (string remove in toRemove) { _timers.Remove(remove); }
        }

        private class TimerData(float time, Action action, TimerMode mode)
        {
            public float Time = time;
            public float EliminatedTime = 0f;
            public Action Action = action;
            public TimerMode Mode = mode;
        }
    }
}
