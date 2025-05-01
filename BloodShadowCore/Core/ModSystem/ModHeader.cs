namespace BloodShadow.Core.ModSystem
{
    public class ModData
    {
        public ModHeader Header { get; set; }
        public ModDependes[] Dependes { get; set; }
        public int DefaultLocalizationIndex { get; set; }

        public string[] ScriptsFiles { get; set; }
        public string[] AssetBundles { get; set; }

        public string ItemsFolderName { get; set; }
        public KeyValuePair<string, string>[] SceneShortcuts { get; set; }
    }

    public class ModHeader
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public Version Version { get; set; }
    }

    public class ModDependes
    {
        public ModHeader Header { get; set; }
        public ModVersionDependesType ModVersionDependesType { get; set; }
    }

    public enum ModVersionDependesType : byte
    {
        More = 0,
        MoreOrEquals = 1,
        Less = 2,
        LessOrEquals = 3,
        Equals = 4
    }
}
