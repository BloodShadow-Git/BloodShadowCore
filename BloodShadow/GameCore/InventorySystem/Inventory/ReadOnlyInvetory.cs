using BloodShadow.GameCore.InventorySystem.Items;
using ObservableCollections;

namespace BloodShadow.GameCore.InventorySystem.Inventory
{
    public abstract class ReadOnlyInvetory
    {
        public abstract string LocalizationKey { get; }
        public abstract IObservableCollection<IReadOnlyInventoryData> ReadOnlyItems { get; }
        public bool ContainsItem(Item item) => ContainsItem(item, 1);
        public abstract bool ContainsItem(Item item, int count);
        public abstract int GetItemsCount(Item item);
    }
}
