namespace BloodShadow.Core.OrGraph
{
    public interface IReadOnlyTree<out T>
    {
        IEnumerable<T> Elements { get; }
        IEnumerable<IReadOnlyPair<T, T>> Pairs { get; }
        IEnumerable<IEnumerable<T>> Layers { get; }
        IEnumerable<T> EndElements { get; }
        int GetDeep();
    }
}
