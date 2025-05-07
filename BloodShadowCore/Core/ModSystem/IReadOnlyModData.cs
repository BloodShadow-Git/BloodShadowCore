namespace BloodShadow.Core.ModSystem
{
    public interface IReadOnlyModData
    {
        IReadOnlyModHeader Header { get; }
        byte[] Icon { get; }
        string RunFileName { get; }
        IReadOnlyModDependes[] Dependes { get; }
    }
}
