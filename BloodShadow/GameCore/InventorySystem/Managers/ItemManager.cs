using BloodShadow.GameCore.InventorySystem.Items;
using System.Collections.Generic;

namespace BloodShadow.GameCore.InventorySystem.Managers
{
    public class ItemManager<T> where T : Item
    {
        public T this[string key] => _items[key];

        public IDictionary<string, T> Items => _items;
        private readonly Dictionary<string, T> _items;

        public ItemManager() { _items = new Dictionary<string, T>(); }

        public bool Add(T item)
        {
            if (_items.ContainsKey(item.LocalizationKey)) { return false; }
            _items.Add(item.LocalizationKey, item);
            return true;
        }

        public bool Remove(T item)
        {
            if (!_items.ContainsKey(item.LocalizationKey)) { return false; }
            _items.Remove(item.LocalizationKey);
            return true;
        }

        public bool Has(T item) => _items.ContainsKey(item.LocalizationKey);
    }
}
