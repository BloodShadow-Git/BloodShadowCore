namespace BloodShadowFramework.ModSystem
{
    public struct Version : IComparable<Version>
    {
        public int Major { get; private set; }
        public int Minor { get; private set; }
        public int Patch { get; private set; }
        public VersionType VersionType { get; private set; }

        public Version(int major, int minor, int patch, VersionType versionType)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            VersionType = versionType;
        }

        public Version(int major, int minor) : this(major, minor, 0, VersionType.Release) { }
        public Version(int major, int minor, VersionType versionType) : this(major, minor, 0, versionType) { }

        public Version(string input)
        {
            Major = 0;
            Minor = 0;
            Patch = 0;
            VersionType = VersionType.Release;
            string[] first = input.Split(' ');
            if (first.Length > 2) { throw new Exception($"INCORRCT VERSION STRING: {input}"); }
            string[] versionsTypes = Enum.GetNames(typeof(VersionType));
            string toExclude = "";
            foreach (string version in first)
            {
                foreach (string versionType in versionsTypes)
                {
                    if (versionType.Equals(version, StringComparison.CurrentCultureIgnoreCase))
                    {
                        toExclude = versionType;
                        VersionType = Enum.Parse<VersionType>(versionType);
                        goto Version;
                    }
                }
            }
        Version:;
            string[] second = first.Except([toExclude]).ElementAt(0).Split('.');
            if (second.Length > 3) { throw new Exception($"INCORRCT VERSION STRING: {input}"); }
            if (int.TryParse(second[0], out int major)) { Major = major; }
            if (int.TryParse(second[0], out int minor)) { Minor = minor; }
            if (int.TryParse(second[0], out int patch)) { Patch = patch; }
        }

        public override readonly bool Equals(object obj)
        {
            if (obj == null) { return false; }
            if (obj is not Version version) return false;
            return Major == version.Major && Minor == version.Minor && Patch == version.Patch && VersionType == version.VersionType;
        }

        public override readonly int GetHashCode() => Major.GetHashCode() + Minor.GetHashCode() + Patch.GetHashCode() + VersionType.GetHashCode();
        public readonly int CompareTo(Version other)
        {
            if (other.Major < Major || other.Minor < Minor || other.Patch < Patch || other.VersionType < VersionType) { return 1; }
            else if (other.Equals(this)) { return 0; }
            else if (other.Major > Major || other.Minor > Minor || other.Patch > Patch || other.VersionType > VersionType) { return -1; }
            return -1;
        }

        public override readonly string ToString() => $"{VersionType} {Major}:{Minor}:{Patch}";
        public static bool operator ==(Version left, Version right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Version left, Version right)
        {
            return !(left == right);
        }

        public static bool operator <(Version left, Version right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(Version left, Version right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(Version left, Version right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(Version left, Version right)
        {
            return left.CompareTo(right) >= 0;
        }
    }

    public enum VersionType : byte
    {
        Alpha = 0,
        Beta = 1,
        Release = 2
    }
}
