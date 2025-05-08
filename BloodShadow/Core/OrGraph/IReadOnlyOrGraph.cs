using System.Collections.Generic;

namespace BloodShadow.Core.OrGraph
{
    public interface IReadOnlyOrGraph<out T> where T : notnull
    {
        IEnumerable<IReadOnlyTree<T>> Trees { get; }
        IEnumerable<T> EndElements { get; }
        IEnumerable<T> Elements { get; }
        IEnumerable<IReadOnlyPair<T, T>> Pairs { get; }
        int GetDeep();
        int GetDeep(int tree);
        IReadOnlyTree<T>? GetTree(int tree);
        int GetTreeCount();
        T[] GetItems(int index);
        T[] GetItems(int tree, int index);
    }
}
