namespace BloodShadow.Core.OrGraph
{
    public class OrGraph<T> : IReadOnlyOrGraph<T>
    {
        IEnumerable<IReadOnlyTree<T>> IReadOnlyOrGraph<T>.Trees => Trees.Cast<IReadOnlyTree<T>>();
        public IEnumerable<T> EndElements => Trees.SelectMany(tree => tree.EndElements);
        public IEnumerable<T> Elements => Trees.SelectMany(tree => tree.Elements);
        public IEnumerable<IReadOnlyPair<T, T>> Pairs => Trees.SelectMany(tree => ((IReadOnlyTree<T>)tree).Pairs);

        public readonly List<Tree<T>> Trees;
        private readonly Func<T, T, T> _uniteFunction;

        public OrGraph() { Trees = []; }
        public OrGraph(IEnumerable<(T item, T parent)> pairs) : this(pairs, null) { }

        public OrGraph(IEnumerable<(T item, T parent)> pairs, Func<T, T, T> uniteFunc)
        {
            _uniteFunction = uniteFunc;
            Trees = [];
            foreach ((T, T) pair in pairs) { Add(pair); }
        }

        public void Add((T item, T parent) pair)
        {
            List<Tree<T>> toRemove = [];
            List<Tree<T>> toAdd = [];
            bool needNewTree = true;
            foreach (Tree<T> first in Trees)
            {
                foreach (Tree<T> second in Trees)
                {
                    if (first.Equals(second)) { continue; }
                    if (first.Contains(pair.item) && second.Contains(pair.parent) ||
                        first.Contains(pair.parent) && second.Contains(pair.item))
                    {
                        Tree<T> tree = new(first, second);
                        tree.Add(pair);
                        toRemove.Add(first);
                        toRemove.Add(second);
                        toAdd.Add(tree);
                        needNewTree = false;
                        goto exit;
                    }
                }
                if (first.Contains(pair.item) || first.Contains(pair.parent))
                {
                    first.Add(pair);
                    needNewTree = false;
                    break;
                }
            }

        exit:;
            foreach (Tree<T> tree in toRemove) { Trees.Remove(tree); }
            foreach (Tree<T> tree in toAdd) { Trees.Add(tree); }
            toRemove.Clear();
            toAdd.Clear();
            if (!needNewTree) { return; }

            Tree<T> _tree = new(_uniteFunction);
            _tree.Add(pair);
            Trees.Add(_tree);
        }
        public void Add(KeyValuePair<T, T> pair) { Add((pair.Key, pair.Value)); }
        public void Add(T parent, T child) { Add(new KeyValuePair<T, T>(parent, child)); }

        public int GetDeep()
        {
            int max = -1;
            foreach (Tree<T> tree in Trees) { max = Math.Max(max, tree.GetDeep()); }
            return max;
        }
        public int GetDeep(int tree)
        {
            if (Trees.Count > tree) { return Trees[tree].GetDeep(); }
            return -1;
        }

        public Tree<T> GetTreeWriteable(int tree)
        {
            if (Trees.Count > tree) { return Trees[tree]; }
            return null;
        }
        public Tree<T> GetTreeWriteable(T element)
        {
            foreach (Tree<T> tree in Trees) { if (tree.Contains(element)) { return tree; } }
            return null;
        }
        public IReadOnlyTree<T> GetTree(int tree) => GetTreeWriteable(tree);
        public IReadOnlyTree<T> GetTree(T element) => GetTreeWriteable(element);

        public int GetTreeCount() { return Trees.Count; }

        public T[] GetItems(int index)
        {
            T[] result = [];
            for (int i = 0; i < Trees.Count; i++)
            {
                if (Trees[i].Layers.Count < index)
                {
                    if (i == 0) { result = [.. Trees[i].Layers.ElementAt(index)]; }
                    else { result = [.. result.Union(Trees[i].Layers.ElementAt(index))]; }
                }
            }
            return result;
        }
        public T[] GetItems(int tree, int index)
        {
            if (Trees.Count > tree) { if (Trees[tree].Layers.Count > index) { return [.. Trees[tree].Layers.ElementAt(index)]; } }
            return [];
        }

        public bool Contains(T element)
        {
            foreach (Tree<T> tree in Trees) { if (tree.Contains(element)) { return true; } }
            return false;
        }
        public bool Contains((T item, T parent) pair) { return Contains(new KeyValuePair<T, T>(pair.item, pair.parent)); }
        public bool Contains(KeyValuePair<T, T> pair)
        {
            foreach (Tree<T> tree in Trees) { if (tree.Contains(pair)) { return true; } }
            return false;
        }

        public override string ToString()
        {
            string result = "";
            if (Trees.Count == 1) { return Trees[0].ToString(); }
            for (int i = 0; i < Trees.Count; i++)
            {
                if (i >= Trees.Count - 1) { result += Trees[i].ToString(); }
                else { result += $"{Trees[i]}\n{new string('-', 10)}\n"; }
            }
            return result;
        }
    }
}
