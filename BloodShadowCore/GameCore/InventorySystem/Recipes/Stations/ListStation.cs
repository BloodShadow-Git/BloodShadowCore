namespace BloodShadow.GameCore.InventorySystem.Recipes.Stations
{
    using BloodShadow.GameCore.InventorySystem.Inventory;
    using BloodShadow.GameCore.InventorySystem.Recipes;

    public abstract class ListStation(string localizationKey) : CraftStation
    {
        public override string LocalizationKey => _localizationKey;
        public override IEnumerable<IReadOnlyCraftingRecipeData> ActiveRecipes => _activeRecipes;
        public override List<IReadOnlyCraftingRecipeData> DoneRecipes => [.. _doneRecipes.Cast<IReadOnlyCraftingRecipeData>()];

        private readonly List<CraftingRecipeData> _activeRecipes = [];
        private readonly List<CraftingRecipeData> _doneRecipes = [];
        private readonly string _localizationKey = localizationKey;

        public override IReadOnlyCraftingRecipeData Add(RecipeData data, Inventory target)
        {
            CraftingRecipeData craft = new(data, target);
            _activeRecipes.Add(craft);
            return craft;
        }

        public override void Remove(IReadOnlyCraftingRecipeData data) { _activeRecipes.Remove((CraftingRecipeData)data); }
    }
}
