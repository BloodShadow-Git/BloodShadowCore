namespace BloodShadow.Core.ModSystem
{
    public interface IReadOnlyModManagerProperty<out T>
    {
        T Value { get; }
        IEnumerable<T> Overrides { get; }
    }
}
