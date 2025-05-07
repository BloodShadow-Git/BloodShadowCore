using BloodShadow.GameCore.InventorySystem.Items;

namespace BloodShadow.GameCore.InventorySystem.Inventory
{
    public class InventoryData(Item item, int count) : IReadOnlyInventoryData
    {
        public Item Item { get; set; } = item;
        public int Count { get; set; } = count;

        public InventoryData(Item item) : this(item, 1) { }

        public static implicit operator Item(InventoryData data) => data.Item;
        public static implicit operator InventoryData(Item item) => new(item);
    }
}
