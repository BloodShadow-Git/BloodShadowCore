namespace BloodShadow.Core.ModSystem
{
    public class ModDependes : IReadOnlyModDependes
    {
        IReadOnlyModHeader IReadOnlyModDependes.Header => Header;
        public ModHeader Header { get; set; }
        public ModVersionDependesType ModVersionDependesType { get; set; }
    }
}
