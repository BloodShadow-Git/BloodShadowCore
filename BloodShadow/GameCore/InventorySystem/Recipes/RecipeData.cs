using BloodShadow.GameCore.InventorySystem.Inventory;
using System.Collections.Generic;

namespace BloodShadow.GameCore.InventorySystem.Recipes
{
    public class RecipeData : IReadOnlyRecipeData
    {
        IEnumerable<IReadOnlyInventoryData> IReadOnlyRecipeData.Input => Input;
        IReadOnlyInventoryData IReadOnlyRecipeData.Output => Output;

        public string[] TargetStations { get; }
        public List<InventoryData> Input { get; set; }
        public InventoryData Output { get; set; }
        public float CraftTime { get; set; }

        public RecipeData(string[] targetStations, List<InventoryData> input, InventoryData output, float craftTime)
        {
            TargetStations = targetStations;
            Input = input;
            Output = output;
            CraftTime = craftTime;
        }
    }
}
