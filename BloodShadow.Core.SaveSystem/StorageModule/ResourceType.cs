namespace BloodShadow.Core.SaveSystem.StorageModule
{
    [Flags]
    public enum ResourceType : byte
    {
        Unknown = 0,
        Resource = 1 << 0,
        Collection = 1 << 1,
    }
}
