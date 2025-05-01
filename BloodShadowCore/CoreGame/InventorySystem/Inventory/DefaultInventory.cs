using BloodShadow.CoreGame.InventorySystem.Items;
using Newtonsoft.Json;
using ObservableCollections;

namespace BloodShadow.CoreGame.InventorySystem.Inventory
{
    public class DefaultInventory(string key) : Inventory
    {
        [JsonIgnore] public override string LocalizationKey => _localizationKey;
        [JsonIgnore]
        public override IObservableCollection<IReadOnlyInventoryData> ReadOnlyItems
            => new ObservableList<IReadOnlyInventoryData>(_items.Cast<IReadOnlyInventoryData>());
        [JsonIgnore] public override IObservableCollection<InventoryData> Items => _items;

        private readonly string _localizationKey = key;
        private readonly ObservableList<InventoryData> _items = [];

        [JsonConstructor]
        public DefaultInventory(string key, IEnumerable<Item> items) : this(key) { _items = [.. items]; }

        public override bool Add(Item item, int count)
        {
            if (item == null) { return false; }
            if (count <= 0) { return false; }
            if (item.Stackable) { AddStackable(item, count); }
            else { AddUnStackable(item, count); }
            return true;
        }
        private void AddUnStackable(Item item, int count) { for (int i = 0; i < count; i++) { _items.Add(item); } }
        private void AddStackable(Item item, int count)
        {
            int itemIndex = _items.IndexOf((from search in _items
                                            where search.Item == item
                                            select search).FirstOrDefault());
            if (itemIndex == -1) { _items.Add(new(item, count)); }
            else { _items[itemIndex].Count += count; }
        }

        public override bool Remove(Item item, int count)
        {
            if (item == null) { return false; }
            if (count <= 0) { return false; }
            IEnumerable<InventoryData> items = from search in _items
                                               where search.Item == item
                                               select search;

            if (item.Stackable) { return RemoveStackable(_items.IndexOf(items.FirstOrDefault()), count); }
            else
            {
                if (items.Count() < count) { return false; }
                RemoveUnStackable(item, count);
                return true;
            }
        }
        private void RemoveUnStackable(Item item, int count) { for (int i = 0; i < count; i++) { _items.Remove(item); } }
        private bool RemoveStackable(int itemIndex, int count)
        {
            if (itemIndex == -1) { return false; }
            else
            {
                if (_items[itemIndex].Count < count) { return false; }
                else if (_items[itemIndex].Count == count) { _items.RemoveAt(itemIndex); }
                else { _items[itemIndex].Count -= count; }
                return true;
            }
        }

        public override bool ContainsItem(Item item, int count)
        {
            if (item == null) { return false; }
            return GetItemsCount(item) >= count;
        }

        public override int GetItemsCount(Item item)
        {
            if (item == null) { return 0; }
            if (item.Stackable)
            {
                return (from search in _items
                        where search.Item == item
                        select search)?.FirstOrDefault()?.Count ?? 0;
            }
            else
            {
                return (from search in _items
                        where search.Item == item
                        select search)?.Count() ?? 0;
            }
        }
    }
}
