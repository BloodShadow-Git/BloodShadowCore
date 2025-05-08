using System.Collections.Generic;

namespace BloodShadow.Core.OrGraph
{
    public struct Pair<TKey, TValue> : IReadOnlyPair<TKey, TValue> where TKey : notnull
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public Pair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public Pair(KeyValuePair<TKey, TValue> keyValuePair) : this(keyValuePair.Key, keyValuePair.Value) { }

        public static implicit operator Pair<TKey, TValue>(KeyValuePair<TKey, TValue> keyValuePair) => new Pair<TKey, TValue>(keyValuePair);
        public static implicit operator KeyValuePair<TKey, TValue>(Pair<TKey, TValue> pair) => new KeyValuePair<TKey, TValue>(pair.Key, pair.Value);
    }
}
