namespace BloodShadowFramework.Utils
{
    using BloodShadowFramework.Extensions;
    using BloodShadowFramework.ModSystem;

    public class OrGraph<T>
    {
        public readonly List<Tree<T>> Trees;

        public OrGraph(IEnumerable<(T item, T parent)> pairs)
        {
            Trees = [];
            foreach ((T item, T parent) pair in pairs) { Add(pair); }
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

            Tree<T> _tree = new();
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

        public Tree<T> GetTree(int tree)
        {
            if (Trees.Count > tree) { return Trees[tree]; }
            return null;
        }
        public Tree<T> GetTree(T element)
        {
            foreach (Tree<T> tree in Trees) { if (tree.Contains(element)) { return tree; } }
            return null;
        }

        public int GetTreeCount() { return Trees.Count; }

        public T[] GetItems(int index)
        {
            T[] result = [];
            for (int i = 0; i < Trees.Count; i++)
            {
                if (Trees[i].Layers.Count < index)
                {
                    if (i == 0) { result = [.. Trees[i].Layers[index]]; }
                    else { result = [.. result.Union(Trees[i].Layers[index])]; }
                }
            }
            return result;
        }
        public T[] GetItems(int tree, int index)
        {
            if (Trees.Count > tree) { if (Trees[tree].Layers.Count > index) { return [.. Trees[tree].Layers[index]]; } }
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

    public class Tree<T>
    {
        public List<T> Elements { get; private set; }
        public List<KeyValuePair<T, T>> Pairs { get; private set; }
        public List<List<T>> Layers { get; private set; }
        public List<T> EndElements { get; private set; }

        private List<KeyValuePair<T, T>> _pairs;
        private List<T> _elements;
        private readonly Dictionary<T, List<T>> _unitDictionary;

        public Tree()
        {
            Elements = [];
            _pairs = [];
            Layers = [];
            EndElements = [];
            Pairs = [];
            _elements = [];
            _unitDictionary = [];
        }

        public Tree(Tree<T> one, Tree<T> two)
        {
            Elements = [.. one.Elements, .. two.Elements];
            _elements = [.. one._elements, .. two._elements];
            _pairs = [.. one._pairs, .. two._pairs];
            Pairs = [.. one.Pairs, .. two.Pairs];
            Layers = [];
            EndElements = [];
            _unitDictionary = [];

            _elements = [.. _elements.Distinct()];
            Elements = [.. Elements.Distinct()];
            _pairs = [.. _pairs.Distinct()];
            Pairs = [.. Pairs.Distinct()];

            Recalculate();
        }

        public void Add(KeyValuePair<T, T> pair)
        {
            foreach (KeyValuePair<T, List<T>> units in _unitDictionary)
            {
                if (units.Value.Contains(pair.Key))
                {
                    IEnumerable<KeyValuePair<T, T>> pairs = from search in _pairs
                                                            where search.Key.Equals(units.Key)
                                                            select search;
                    foreach (KeyValuePair<T, T> pairToCheck in pairs) { if (pairToCheck.Value.Equals(pair.Value)) { return; } }
                    _pairs.Add(new KeyValuePair<T, T>(units.Key, pair.Value));
                    return;
                }
            }

            Elements.Add(pair.Key);
            Elements.Add(pair.Value);
            Elements = [.. (from search in Elements where search != null select search)];
            Elements = [.. Elements.Distinct()];

            _elements.Add(pair.Key);
            _elements.Add(pair.Value);
            _elements = [.. (from search in _elements where search != null select search)];
            _elements = [.. _elements.Distinct()];

            _pairs.Add(pair);
            _pairs = [.. _pairs.Distinct()];

            Pairs.Add(pair);
            Pairs = [.. Pairs.Distinct()];

            Recalculate();
        }
        public void Add(T parent, T child) { Add(new KeyValuePair<T, T>(parent, child)); }
        public void Add((T parent, T child) pair) { Add(new KeyValuePair<T, T>(pair.parent, pair.child)); }


        private void Recalculate()
        {
            FilterCycle();
            FillLayers();
            FindEnds();
        }

        private void FilterCycle()
        {
            List<KeyValuePair<T, T>> _pairs = [.. this._pairs];
            foreach (KeyValuePair<T, T> pair in this._pairs)
            {
                if (pair.Value == null) { continue; }
                List<T> checkedElements = [];
                List<KeyValuePair<T, T>> visited = [];
                Stack<KeyValuePair<T, T>> frontier = [];
                IEnumerable<KeyValuePair<T, T>> pairs = from search in _pairs
                                                        where search.Key.Equals(pair.Value)
                                                        select search;
                foreach (KeyValuePair<T, T> keyValuePair in pairs) { frontier.Push(keyValuePair); }

                while (frontier.Count > 0)
                {
                    KeyValuePair<T, T> current = frontier.Pop();
                    if (current.Value == null) { checkedElements.Add(current.Key); continue; }
                    IEnumerable<KeyValuePair<T, T>> toCheck = from search in _pairs
                                                              where search.Key.Equals(current.Key)
                                                              select search;
                    bool shouldAdd = false;
                    foreach (KeyValuePair<T, T> check in toCheck)
                    {
                        if (checkedElements.Contains(check.Value)) { shouldAdd = true; }
                        else { shouldAdd = false; break; }
                    }
                    if (shouldAdd) { checkedElements.Add(current.Key); continue; }
                    visited.Add(current);

                    if (current.Equals(pair))
                    {
                        List<T> elements = [];
                        List<T> dependes = [];
                    newCycle:;
                        checkedElements = [.. checkedElements.Distinct()];
                        IEnumerable<IGrouping<T, KeyValuePair<T, T>>> toFix = from search in _pairs
                                                                              where checkedElements.Contains(search.Value)
                                                                              group search by search.Key;
                        List<KeyValuePair<T, T>> toRemove = [];
                        foreach (IGrouping<T, KeyValuePair<T, T>> fix in toFix)
                        {
                            IEnumerable<KeyValuePair<T, T>> pairsToCheeck = from search in _pairs
                                                                            where search.Key.Equals(fix.Key)
                                                                            select search;
                            bool needRemove = false;
                            foreach (KeyValuePair<T, T> pairToCheck in pairsToCheeck)
                            {
                                if (checkedElements.Contains(pairToCheck.Value)) { needRemove = true; }
                                else { needRemove = false; break; }
                            }
                            if (needRemove)
                            {
                                foreach (KeyValuePair<T, T> item in fix)
                                {
                                    if (!checkedElements.Contains(item.Key)) { checkedElements.Add(item.Key); goto newCycle; }
                                    toRemove.Add(item);
                                }
                            }
                        }
                        List<KeyValuePair<T, T>> checkedPairs = [.. (from search in visited
                                                                 where checkedElements.Contains(search.Key)
                                                                 select search)];
                        visited = [.. visited.Except(checkedPairs)];
                        foreach (KeyValuePair<T, T> keyValuePair in visited)
                        {
                            if (dependes.Contains(keyValuePair.Value)) { continue; }
                            if (toRemove.Contains(keyValuePair)) { dependes.Add(keyValuePair.Key); continue; }
                            elements.Add(keyValuePair.Key);
                            dependes.Add(keyValuePair.Value);
                        }
                        elements = [.. elements.Distinct()];
                        T unitedElement = default;
                        for (int i = 0; i < elements.Count; i++)
                        {
                            if (i == 0) { unitedElement = elements[i]; }
                            else
                            {
                                if (elements[i] is IUnitable<T> unit) { unitedElement = unit.Unit(unitedElement); }
                                else { throw new Exception("Cycle parents detected and orgraph type is not IUnitable"); }
                            }
                            _elements.Remove(elements[i]);
                        }
                        if (unitedElement == null) { throw new Exception("Empty united element"); }
                        _elements.Add(unitedElement);
                        _unitDictionary.Add(unitedElement, elements);
                        dependes = [.. dependes.Distinct().Except(elements)];
                        List<KeyValuePair<T, T>> pairsToRemove = [.. (from search in _pairs
                                                                  where elements.Contains(search.Key)
                                                                  select search)];
                        foreach (KeyValuePair<T, T> pairToRemove in pairsToRemove)
                        {
                            _pairs.Remove(pairToRemove);
                            dependes.Add(pairToRemove.Value);
                        }
                        dependes = [.. dependes.Distinct().Except(elements)];
                        foreach (var dep in dependes) { _pairs.Add(new KeyValuePair<T, T>(unitedElement, dep)); }
                        goto newPair;
                    }
                    IEnumerable<KeyValuePair<T, T>> _pairsQueue = from search in _pairs
                                                                  where search.Key.Equals(current.Value)
                                                                  select search;
                    foreach (KeyValuePair<T, T> _pair in _pairsQueue) { if (!visited.Contains(_pair)) { frontier.Push(_pair); } }
                }
            newPair:;
            }
            this._pairs = _pairs;
        }
        private void FillLayers()
        {
            Layers.Clear();
            Queue<T> elementsToAdd = [.. _elements];
            List<T> addedElements = [];
            while (elementsToAdd.Count > 0)
            {
                T element = elementsToAdd.Dequeue();
                List<KeyValuePair<T, T>> elementPairs = [.. (from search in _pairs
                                                         where search.Key.Equals(element)
                                                         select search)];
                if (elementPairs.Count != 0)
                {
                    if (elementPairs.Count == 1)
                    {
                        if (elementPairs[0].Value == null)
                        {
                            if (Layers.Count == 0) { Layers.Add([]); }
                            Layers[0].Add(element);
                            addedElements.Add(element);
                            continue;
                        }
                    }
                    bool addToQueue = true;
                    foreach (KeyValuePair<T, T> pair in elementPairs)
                    {
                        if (addedElements.Contains(pair.Value)) { addToQueue = false; }
                        else { addToQueue = true; break; }
                    }
                    if (addToQueue) { elementsToAdd.Enqueue(element); continue; }

                    int max = -1;
                    foreach (KeyValuePair<T, T> pair in elementPairs)
                    {
                        for (int i = 0; i < Layers.Count; i++)
                        {
                            if (Layers[i].Contains(pair.Value))
                            {
                                max = Math.Max(i, max);
                                break;
                            }
                        }
                    }
                    if (max >= Layers.Count - 1) { Layers.Add([element]); }
                    else { Layers[max + 1].Add(element); }
                    addedElements.Add(element);
                }
                else
                {
                    if (_elements.Contains(element)) { }
                    if (Layers.Count == 0) { Layers.Add([]); }
                    Layers[0].Add(element);
                    addedElements.Add(element);
                }
            }
        }
        private void FindEnds()
        {
            EndElements.Clear();
            Queue<T> queue = [.. _elements];
            while (queue.Count > 0)
            {
                T element = queue.Dequeue();
                IEnumerable<T> childs = from search in _pairs
                                        where element.Equals(search.Value)
                                        select search.Key;
                childs = childs.Distinct();
                if (!childs.Any()) { EndElements.Add(element); }
            }
        }

        public int GetDeep() { return Layers.Count; }

        public bool Contains(T element) { return Elements.Contains(element) || _elements.Contains(element); }
        public bool Contains((T item, T parent) pair) { return Contains(new KeyValuePair<T, T>(pair.item, pair.parent)); }
        public bool Contains(KeyValuePair<T, T> pair) { return Pairs.Contains(pair) || _pairs.Contains(pair); }

        public override string ToString()
        {
            bool dataAdded = false;
            string space = new('+', 15);
            string result = "";
            if (dataAdded) { result += $"{space}\n"; dataAdded = false; }
            if (Elements.Count != 0) { result += "\nElements (public property)\n\n"; }
            foreach (T element in Elements) { result += $"{element}\n"; dataAdded = true; }

            if (dataAdded) { result += $"{space}\n"; dataAdded = false; }
            if (_elements.Count != 0) { result += "\nElements (private field)\n\n"; }
            foreach (T element in _elements) { result += $"'{element}'\n"; dataAdded = true; }

            if (dataAdded) { result += $"{space}\n"; dataAdded = false; }
            if (Layers.Count != 0) { result += "\nLayers (public property)\n\n"; }
            for (int i = 0; i < Layers.Count; i++) { foreach (T element in Layers[i]) { result += $"{new string('\t', i)}{element}\n"; } dataAdded = true; }

            if (dataAdded) { result += $"{space}\n"; dataAdded = false; }
            if (Pairs.Count != 0) { result += "\nPairs (public property)\n\n"; }
            foreach (KeyValuePair<T, T> pair in Pairs) { result += $"{pair.Key} - {pair.Value}\n"; dataAdded = true; }

            if (dataAdded) { result += $"{space}\n"; dataAdded = false; }
            if (_pairs.Count != 0) { result += "\nPairs (private field)\n\n"; }
            foreach (KeyValuePair<T, T> pair in _pairs) { result += $"{pair.Key} - {pair.Value}\n"; dataAdded = true; }

            if (dataAdded) { result += $"{space}\n"; dataAdded = false; }
            if (_unitDictionary.Count != 0) { result += "\nUnits (private field)\n\n"; }
            foreach (KeyValuePair<T, List<T>> pair in _unitDictionary)
            {
                result += $"{pair.Key} - (";
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    if (i >= pair.Value.Count - 1) { result += $"{pair.Value[i]})\n"; }
                    else { result += $"{pair.Value[i]}, "; }
                }
                dataAdded = true;
            }

            if (dataAdded) { result += $"{space}\n"; }
            if (_pairs.Count != 0) { result += "\nEnd elements (public property)\n\n"; }
            foreach (T endElement in EndElements) { result += $"{endElement}\n"; dataAdded = true; }

            return result;
        }
    }

    public interface IUnitable<T> { T Unit(T other); }
}
