using BloodShadow.GameCore.InventorySystem.Inventory;
using System.Collections.Generic;

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
