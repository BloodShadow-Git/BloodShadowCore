namespace BloodShadow.Core.OrGraph
{
    public interface IReadOnlyPair<out TKey, out TValue> where TKey : notnull
    {
        TKey Key { get; }
        TValue Value { get; }
    }
}
