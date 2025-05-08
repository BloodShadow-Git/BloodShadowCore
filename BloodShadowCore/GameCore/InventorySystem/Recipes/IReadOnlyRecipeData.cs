using BloodShadow.GameCore.InventorySystem.Inventory;

namespace BloodShadow.GameCore.InventorySystem.Recipes
{
    public interface IReadOnlyRecipeData
    {
        string[] TargetStations { get; }
        IEnumerable<IReadOnlyInventoryData> Input { get; }
        IReadOnlyInventoryData Output { get; }
        float CraftTime { get; }
    }
}
