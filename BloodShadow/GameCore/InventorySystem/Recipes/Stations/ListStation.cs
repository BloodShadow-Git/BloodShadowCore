namespace BloodShadow.GameCore.InventorySystem.Recipes.Stations
{
    using BloodShadow.GameCore.InventorySystem.Inventory;
    using BloodShadow.GameCore.InventorySystem.Recipes;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class ListStation : CraftStation
    {
        public override string LocalizationKey => _localizationKey;
        public override IEnumerable<IReadOnlyCraftingRecipeData> ActiveRecipes => _activeRecipes;
        public override List<IReadOnlyCraftingRecipeData> DoneRecipes => _doneRecipes.Cast<IReadOnlyCraftingRecipeData>().ToList();

        private readonly List<CraftingRecipeData> _activeRecipes = new List<CraftingRecipeData>();
        private readonly List<CraftingRecipeData> _doneRecipes = new List<CraftingRecipeData>();
        private readonly string _localizationKey;

        public ListStation(string localizationKey)
        {
            _localizationKey = localizationKey;
        }
        public override IReadOnlyCraftingRecipeData Add(RecipeData data, Inventory target)
        {
            CraftingRecipeData craft = new CraftingRecipeData(data, target);
            _activeRecipes.Add(craft);
            return craft;
        }

        public override void Remove(IReadOnlyCraftingRecipeData data) { _activeRecipes.Remove((CraftingRecipeData)data); }
    }
}
