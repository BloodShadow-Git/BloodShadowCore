namespace BloodShadow.GameCore.InventorySystem.Recipes.Stations
{
    using BloodShadow.Core.Extensions;
    using BloodShadow.GameCore.InventorySystem.Inventory;
    using BloodShadow.GameCore.InventorySystem.Recipes;

    public abstract class QueueStation(string localizationKey) : CraftStation
    {
        public override string LocalizationKey => _localizationKey;
        public override IEnumerable<IReadOnlyCraftingRecipeData> ActiveRecipes => _activeRecipes;
        public override List<IReadOnlyCraftingRecipeData> DoneRecipes => [.. _doneRecipes.Cast<IReadOnlyCraftingRecipeData>()];

        private Queue<CraftingRecipeData> _activeRecipes = [];
        private readonly List<CraftingRecipeData> _doneRecipes = [];
        private readonly string _localizationKey = localizationKey;

        public override IReadOnlyCraftingRecipeData Add(RecipeData data, Inventory target)
        {
            CraftingRecipeData craft = new(data, target);
            _activeRecipes.Enqueue(craft);
            return craft;
        }

        public override void Remove(IReadOnlyCraftingRecipeData data)
        {
            List<CraftingRecipeData> list = [.. _activeRecipes];
            list.Remove((CraftingRecipeData)data);
            _activeRecipes = [.. list];
        }
    }
}
