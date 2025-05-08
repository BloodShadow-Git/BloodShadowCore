namespace BloodShadow.Core.OrGraph
{
    using Extensions;

    public class Tree<T> : IReadOnlyTree<T>
    {
        IEnumerable<T> IReadOnlyTree<T>.Elements => Elements;
        IEnumerable<IReadOnlyPair<T, T>> IReadOnlyTree<T>.Pairs => Pairs.Select(key => (IReadOnlyPair<T, T>)new Pair<T, T>(key));
        IEnumerable<IEnumerable<T>> IReadOnlyTree<T>.Layers => Layers;
        IEnumerable<T> IReadOnlyTree<T>.EndElements => EndElements;

        public List<T> Elements;
        public List<KeyValuePair<T, T>> Pairs;
        public readonly List<List<T>> Layers;
        public readonly List<T> EndElements;

        private readonly Dictionary<T, List<T>> _unitDictionary;
        private readonly Func<T, T, T> _uniteFunction;

        public Tree() : this(null) { }

        public Tree(Func<T, T, T> unitFunc)
        {
            _uniteFunction = unitFunc;

            Pairs = [];
            Layers = [];
            EndElements = [];
            Elements = [];
            _unitDictionary = [];
        }

        public Tree(Tree<T> one, Tree<T> two)
        {
            if (one._uniteFunction != two._uniteFunction) { throw new Exception("Invalid unite function in first and second trees"); }

            _uniteFunction = one._uniteFunction;
            Elements = [.. one.Elements, .. two.Elements];
            Pairs = [.. one.Pairs, .. two.Pairs];
            Layers = [];
            EndElements = [];
            _unitDictionary = [];

            Elements = [.. Elements.Distinct()];
            Pairs = [.. Pairs.Distinct()];

            Recalculate();
        }

        public void Add(KeyValuePair<T, T> pair)
        {
            foreach (KeyValuePair<T, List<T>> units in _unitDictionary)
            {
                if (units.Value.Contains(pair.Key))
                {
                    IEnumerable<KeyValuePair<T, T>> pairs = from search in Pairs
                                                            where search.Key.Equals(units.Key)
                                                            select search;
                    foreach (KeyValuePair<T, T> pairToCheck in pairs) { if (pairToCheck.Value.Equals(pair.Value)) { return; } }
                    Pairs.Add(new KeyValuePair<T, T>(units.Key, pair.Value));
                    return;
                }
            }

            Elements.Add(pair.Key);
            Elements.Add(pair.Value);
            Elements = [.. from search in Elements where search != null select search];
            Elements = [.. Elements.Distinct()];

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
            List<KeyValuePair<T, T>> _pairs = [.. this.Pairs];
            foreach (KeyValuePair<T, T> pair in this.Pairs)
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
                        List<T> depends = [];
                    newCycle:;
                        checkedElements = [.. checkedElements.Distinct()];
                        IEnumerable<IGrouping<T, KeyValuePair<T, T>>> toFix = from search in _pairs
                                                                              where checkedElements.Contains(search.Value)
                                                                              group search by search.Key;
                        List<KeyValuePair<T, T>> toRemove = [];
                        foreach (IGrouping<T, KeyValuePair<T, T>> fix in toFix)
                        {
                            IEnumerable<KeyValuePair<T, T>> pairsToCheck = from search in _pairs
                                                                           where search.Key.Equals(fix.Key)
                                                                           select search;
                            bool needRemove = false;
                            foreach (KeyValuePair<T, T> pairToCheck in pairsToCheck)
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
                        List<KeyValuePair<T, T>> checkedPairs = [.. from search in visited
                                                                 where checkedElements.Contains(search.Key)
                                                                 select search];
                        visited = [.. visited.Except(checkedPairs)];
                        foreach (KeyValuePair<T, T> keyValuePair in visited)
                        {
                            if (depends.Contains(keyValuePair.Value)) { continue; }
                            if (toRemove.Contains(keyValuePair)) { depends.Add(keyValuePair.Key); continue; }
                            elements.Add(keyValuePair.Key);
                            depends.Add(keyValuePair.Value);
                        }
                        elements = [.. elements.Distinct()];
                        T unitedElement = default;
                        for (int i = 0; i < elements.Count; i++)
                        {
                            if (i == 0) { unitedElement = elements[i]; }
                            else
                            {
                                if (elements[i] is IUnitable<T> unit) { unitedElement = unit.Unite(unitedElement); }
                                else if (_uniteFunction != null) { unitedElement = _uniteFunction(elements[i], unitedElement); }
                                else { throw new Exception($"Cycle parents detected and or graph type is not {nameof(IUnitable<T>)} or graph don`t have unite function"); }
                            }
                            Elements.Remove(elements[i]);
                        }
                        if (unitedElement == null) { throw new Exception("Empty united element"); }
                        Elements.Add(unitedElement);
                        _unitDictionary.Add(unitedElement, elements);
                        depends = [.. depends.Distinct().Except(elements)];
                        List<KeyValuePair<T, T>> pairsToRemove = [.. from search in _pairs
                                                                  where elements.Contains(search.Key)
                                                                  select search];
                        foreach (KeyValuePair<T, T> pairToRemove in pairsToRemove)
                        {
                            _pairs.Remove(pairToRemove);
                            depends.Add(pairToRemove.Value);
                        }
                        depends = [.. depends.Distinct().Except(elements)];
                        foreach (var dep in depends) { _pairs.Add(new KeyValuePair<T, T>(unitedElement, dep)); }
                        goto newPair;
                    }
                    IEnumerable<KeyValuePair<T, T>> _pairsQueue = from search in _pairs
                                                                  where search.Key.Equals(current.Value)
                                                                  select search;
                    foreach (KeyValuePair<T, T> _pair in _pairsQueue) { if (!visited.Contains(_pair)) { frontier.Push(_pair); } }
                }
            newPair:;
            }
            this.Pairs = _pairs;
        }
        private void FillLayers()
        {
            Layers.Clear();
            Queue<T> elementsToAdd = [.. Elements];
            List<T> addedElements = [];
            while (elementsToAdd.Count > 0)
            {
                T element = elementsToAdd.Dequeue();
                List<KeyValuePair<T, T>> elementPairs = [.. from search in Pairs
                                                         where search.Key.Equals(element)
                                                         select search];
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
                                //break;
                            }
                        }
                    }
                    if (max >= Layers.Count - 1) { Layers.Add([element]); }
                    else { Layers[max + 1].Add(element); }
                    addedElements.Add(element);
                }
                else
                {
                    if (Layers.Count == 0) { Layers.Add([]); }
                    Layers[0].Add(element);
                    addedElements.Add(element);
                }
            }
        }
        private void FindEnds()
        {
            EndElements.Clear();
            Queue<T> queue = [.. Elements];
            while (queue.Count > 0)
            {
                T element = queue.Dequeue();
                IEnumerable<T> child = from search in Pairs
                                       where element.Equals(search.Value)
                                       select search.Key;
                child = child.Distinct();
                if (!child.Any()) { EndElements.Add(element); }
            }
        }

        public int GetDeep() { return Layers.Count; }

        public bool Contains(T element) { return Elements.Contains(element); }
        public bool Contains((T item, T parent) pair) { return Contains(new KeyValuePair<T, T>(pair.item, pair.parent)); }
        public bool Contains(KeyValuePair<T, T> pair) { return Pairs.Contains(pair); }

        public override string ToString()
        {
            bool dataAdded = false;
            string space = new('+', 15);
            string result = "";

            if (dataAdded) { result += $"{space}\n"; dataAdded = false; }
            if (Elements.Count != 0) { result += "\nElements\n\n"; }
            foreach (T element in Elements) { result += $"'{element}'\n"; dataAdded = true; }

            if (dataAdded) { result += $"{space}\n"; dataAdded = false; }
            if (Layers.Count != 0) { result += "\nLayers\n\n"; }
            for (int i = 0; i < Layers.Count; i++) { foreach (T element in Layers[i]) { result += $"{new string('\t', i)}{element}\n"; } dataAdded = true; }

            if (dataAdded) { result += $"{space}\n"; dataAdded = false; }
            if (Pairs.Count != 0) { result += "\nPairs\n\n"; }
            foreach (KeyValuePair<T, T> pair in Pairs) { result += $"{pair.Key} - {pair.Value}\n"; dataAdded = true; }

            if (dataAdded) { result += $"{space}\n"; dataAdded = false; }
            if (_unitDictionary.Count != 0) { result += "\nUnits\n\n"; }
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
            if (Pairs.Count != 0) { result += "\nEnd elements\n\n"; }
            foreach (T endElement in EndElements) { result += $"{endElement}\n"; dataAdded = true; }

            return result;
        }
    }
}
