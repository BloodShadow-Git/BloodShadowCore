using BloodShadow.CoreGame.InventorySystem.Inventory;

namespace BloodShadow.CoreGame.InventorySystem.Recipes
{
    public interface IReadOnlyRecipeData
    {
        IEnumerable<InventoryData> Input { get; }
        IEnumerable<InventoryData> Output { get; }
        float CraftTime { get; }
    }
}
