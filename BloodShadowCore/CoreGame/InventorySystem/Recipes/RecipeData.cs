using BloodShadow.CoreGame.InventorySystem.Inventory;

namespace BloodShadow.CoreGame.InventorySystem.Recipes
{
    public class RecipeData(List<InventoryData> input, List<InventoryData> output, float craftTime) : IReadOnlyRecipeData
    {
        IEnumerable<InventoryData> IReadOnlyRecipeData.Input => Input;
        IEnumerable<InventoryData> IReadOnlyRecipeData.Output => Output;

        public List<InventoryData> Input { get; set; } = input;
        public List<InventoryData> Output { get; set; } = output;
        public float CraftTime { get; set; } = craftTime;
    }
}
