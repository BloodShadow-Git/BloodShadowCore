namespace BloodShadow.GameCore.InventorySystem.Recipes
{
    using BloodShadow.GameCore.InventorySystem.Inventory;

    public interface IReadOnlyCraftingRecipeData
    {
        RecipeData Data { get; }
        Inventory Target { get; }
        float ElapsedTime { get; }
    }
}
