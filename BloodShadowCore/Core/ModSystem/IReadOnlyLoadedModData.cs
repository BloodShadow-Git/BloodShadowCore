namespace BloodShadow.Core.ModSystem
{
    public interface IReadOnlyLoadedModData
    {
        IReadOnlyModData ModData { get; }
        string ModFolder { get; }
    }
}
