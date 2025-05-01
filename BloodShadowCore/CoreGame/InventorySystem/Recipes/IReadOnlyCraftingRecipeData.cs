namespace BloodShadow.CoreGame.InventorySystem.Recipes
{
    using BloodShadow.CoreGame.InventorySystem.Inventory;

    public interface IReadOnlyCraftingRecipeData
    {
        RecipeData Data { get; }
        Inventory Target { get; }
        float ElapsedTime { get; }
    }
}
