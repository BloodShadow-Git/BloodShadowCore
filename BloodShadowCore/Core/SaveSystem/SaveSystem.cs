namespace BloodShadow.Core.SaveSystem
{
    using System;

    public abstract class SaveSystem
    {
        public abstract void Save(string key, object data, Action<bool> callback = null, bool useBuildPath = true, bool useCheckPath = true);
        public abstract void SaveAsync(string key, object data, Action<bool> callback = null, bool useBuildPath = true, bool useCheckPath = true);
        public abstract void SaveToString(object data, Action<bool, string> callback = null);

        public abstract void Load<T>(string key, Action<T> callback, bool useBuildPath = true, bool useCheckPath = true);
        public abstract void LoadAsync<T>(string key, Action<T> callback, bool useBuildPath = true, bool useCheckPath = true);
        public abstract void LoadFromString<T>(string objectString, Action<T> callback);

        public abstract void CheckFile(string path);
        public abstract void CheckFolder(string path);
    }
}