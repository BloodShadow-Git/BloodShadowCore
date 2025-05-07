using BloodShadow.Core.OrGraph;

namespace BloodShadow.Core.ModSystem
{
    public class LoadedModData(ModData modData, string modFolder) : IReadOnlyLoadedModData, IUnitable<LoadedModData>
    {
        IReadOnlyModData IReadOnlyLoadedModData.ModData => ModData;
        public ModData ModData { get; set; } = modData;
        public string ModFolder { get; set; } = modFolder;

        public override bool Equals(object obj)
        {
            if (obj is not IReadOnlyLoadedModData lmd) { return false; }
            return lmd.ModFolder.Equals(ModFolder);
        }
        public override int GetHashCode() { return ModFolder.GetHashCode(); }
        public LoadedModData Unite(LoadedModData other) => new(other.ModData, other.ModFolder);
    }
}
