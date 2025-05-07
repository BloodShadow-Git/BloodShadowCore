namespace BloodShadow.Core.ModSystem
{
    public interface IReadOnlyModHeader
    {
        string Name { get; }
        Version Version { get; }
        string URL { get; }
    }
}
