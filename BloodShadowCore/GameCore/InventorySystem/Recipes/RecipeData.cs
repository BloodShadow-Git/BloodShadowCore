using BloodShadow.GameCore.InventorySystem.Inventory;

namespace BloodShadow.GameCore.InventorySystem.Recipes
{
    public class RecipeData(string[] targetStations, List<InventoryData> input, InventoryData output, float craftTime) : IReadOnlyRecipeData
    {
        IEnumerable<IReadOnlyInventoryData> IReadOnlyRecipeData.Input => Input;
        IReadOnlyInventoryData IReadOnlyRecipeData.Output => Output;

        public string[] TargetStations { get; } = targetStations;
        public List<InventoryData> Input { get; set; } = input;
        public InventoryData Output { get; set; } = output;
        public float CraftTime { get; set; } = craftTime;
    }
}
