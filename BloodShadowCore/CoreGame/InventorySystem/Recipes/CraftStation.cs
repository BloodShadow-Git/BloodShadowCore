namespace BloodShadow.CoreGame.InventorySystem.Recipes
{
    using BloodShadow.CoreGame.InventorySystem.Inventory;
    using BloodShadow.CoreGame.Systems;

    public abstract class CraftStation : IUpdateSystem
    {
        public abstract string LocalizationKey { get; }
        public abstract IEnumerable<IReadOnlyCraftingRecipeData> ActiveRecipes { get; }
        public abstract List<IReadOnlyCraftingRecipeData> DoneRecipes { get; }
        public abstract IReadOnlyCraftingRecipeData Add(RecipeData data, Inventory target);
        public abstract void Remove(IReadOnlyCraftingRecipeData data);
        public abstract void Update(in float delta);

        protected class CraftingRecipeData(RecipeData data, Inventory target) : IReadOnlyCraftingRecipeData
        {
            public RecipeData Data { get; set; } = data;
            public Inventory Target { get; set; } = target;
            public float ElapsedTime { get; set; }
        }
    }
}
