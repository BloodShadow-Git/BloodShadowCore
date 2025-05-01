using BloodShadow.CoreGame.InventorySystem.Items;
using ObservableCollections;

namespace BloodShadow.CoreGame.InventorySystem.Inventory
{
    public abstract class Inventory : ReadOnlyInvetory
    {
        public abstract IObservableCollection<InventoryData> Items { get; }
        public bool Add(Item item) => Add(item, 1);
        public bool Remove(Item item) => Remove(item, 1);
        public abstract bool Add(Item item, int count);
        public abstract bool Remove(Item item, int count);
    }
}
