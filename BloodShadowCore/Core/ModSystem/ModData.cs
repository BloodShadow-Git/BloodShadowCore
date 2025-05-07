using BloodShadow.Core.OrGraph;

namespace BloodShadow.Core.ModSystem
{
    public class ModData : IReadOnlyModData
    {
        public ModHeader Header { get; set; }
        public byte[] Icon { get; set; }
        public string RunFileName { get; set; }
        public ModDependes[] Dependes { get; set; }

        IReadOnlyModHeader IReadOnlyModData.Header => Header;
        IReadOnlyModDependes[] IReadOnlyModData.Dependes => Dependes;

        public override bool Equals(object obj)
        {
            if (obj is not ModData modData) { return false; }
            return modData.Header.Equals(Header);
        }

        public override int GetHashCode() { return Header.GetHashCode(); }
    }
}
