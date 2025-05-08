using BloodShadow.GameCore.InventorySystem.Items;

namespace BloodShadow.GameCore.InventorySystem.Inventory
{
    public interface IReadOnlyInventoryData
    {
        Item Item { get; }
        int Count { get; }
    }
}
