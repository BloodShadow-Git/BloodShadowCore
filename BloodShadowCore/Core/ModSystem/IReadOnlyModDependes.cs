namespace BloodShadow.Core.ModSystem
{
    public interface IReadOnlyModDependes
    {
        IReadOnlyModHeader Header { get; }
        ModVersionDependesType ModVersionDependesType { get; }
    }
}
