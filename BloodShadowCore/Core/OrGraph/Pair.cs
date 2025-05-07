namespace BloodShadow.Core.OrGraph
{
    public struct Pair<TKey, TValue>(TKey key, TValue value) : IReadOnlyPair<TKey, TValue> where TKey : notnull
    {
        public TKey Key { get; set; } = key;
        public TValue Value { get; set; } = value;

        public Pair(KeyValuePair<TKey, TValue> keyValuePair) : this(keyValuePair.Key, keyValuePair.Value) { }

        public static implicit operator Pair<TKey, TValue>(KeyValuePair<TKey, TValue> keyValuePair) => new(keyValuePair);
        public static implicit operator KeyValuePair<TKey, TValue>(Pair<TKey, TValue> pair) => new(pair.Key, pair.Value);
    }
}
