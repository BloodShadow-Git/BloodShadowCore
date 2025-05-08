using BloodShadow.GameCore.InventorySystem.Items;

namespace BloodShadow.GameCore.InventorySystem.Inventory
{
    public class InventoryData : IReadOnlyInventoryData
    {
        public Item Item { get; set; }
        public int Count { get; set; }


        public InventoryData(Item item, int count)
        {
            Item = item;
            Count = count;
        }
        public InventoryData(Item item) : this(item, 1) { }

        public static implicit operator Item(InventoryData data) => data.Item;
        public static implicit operator InventoryData(Item item) => new InventoryData(item);
    }
}
