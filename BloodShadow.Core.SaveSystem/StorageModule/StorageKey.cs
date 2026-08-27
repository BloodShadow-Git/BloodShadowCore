namespace BloodShadow.Core.SaveSystem.StorageModule
{
    public struct StorageKey
    {
        public string[] Path { get; private set; }

        public StorageKey() { Path = []; }
        public StorageKey(params string[] path)
        {
            List<string> temp = new(path.Length);
            foreach (string pathPart in path) { temp.AddRange(pathPart.Split(['/', '\\'], StringSplitOptions.RemoveEmptyEntries)); }
            Path = [.. temp];
        }

        public static StorageKey operator /(StorageKey key, string path)
        {
            string[] temp = new string[key.Path.Length + 1];
            Array.Copy(key.Path, temp, key.Path.Length);
            temp[^1] = path;
            return new(temp);
        }
    }
}