namespace BloodShadowCore.ModSystem
{
    public abstract class ModBase
    {
        private readonly string Name;
        private readonly Version Version;
        private readonly ModManager LoadingModManager;

        public ModBase(string name, Version version, ModManager loadingModManager)
        {
            Name = name;
            Version = version;
            LoadingModManager = loadingModManager;

            OnLoad();
        }

        protected abstract void OnLoad();

        public override string ToString() => $"{Name} v. {Version} loaded by {LoadingModManager}";
    }
}
