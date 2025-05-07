namespace BloodShadow.Core.ModSystem
{
    public class ModHeader : IReadOnlyModHeader
    {
        public string Name { get; set; }
        public Version Version { get; set; }
        public string URL { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is not ModHeader modHeader) { return false; }
            return modHeader.Name.Equals(Name);
        }
        public override int GetHashCode() { return Name.GetHashCode(); }
    }
}
