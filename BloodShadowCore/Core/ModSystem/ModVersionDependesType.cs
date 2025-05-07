namespace BloodShadow.Core.ModSystem
{
    public enum ModVersionDependesType : byte
    {
        More = 0,
        MoreOrEquals = 1,
        Less = 2,
        LessOrEquals = 3,
        Equals = 4
    }
}
