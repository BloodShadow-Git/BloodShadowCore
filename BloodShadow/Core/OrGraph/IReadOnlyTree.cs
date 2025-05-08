using System.Collections.Generic;

namespace BloodShadow.Core.OrGraph
{
    public interface IReadOnlyTree<out T> where T : notnull
    {
        IEnumerable<T> Elements { get; }
        IEnumerable<IReadOnlyPair<T, T>> Pairs { get; }
        IEnumerable<IEnumerable<T>> Layers { get; }
        IEnumerable<T> EndElements { get; }
        int GetDeep();
    }
}
