using BloodShadow.CoreGame.InventorySystem.Items;

namespace BloodShadow.CoreGame.InventorySystem.Inventory
{
    public interface IReadOnlyInventoryData
    {
        Item Item { get; }
        int Count { get; }
    }
}
