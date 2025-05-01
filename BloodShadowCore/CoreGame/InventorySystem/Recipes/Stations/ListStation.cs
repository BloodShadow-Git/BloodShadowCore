namespace BloodShadow.CoreGame.InventorySystem.Recipes.Stations
{
    using Inventory;

    public class ListStation(string localizationKey) : CraftStation
    {
        public override string LocalizationKey => _localizationKey;
        public override IEnumerable<IReadOnlyCraftingRecipeData> ActiveRecipes => _activeRecipes;
        public override List<IReadOnlyCraftingRecipeData> DoneRecipes => [.. _doneRecipes.Cast<IReadOnlyCraftingRecipeData>()];

        private readonly List<CraftingRecipeData> _activeRecipes = [];
        private readonly List<CraftingRecipeData> _doneRecipes = [];
        private readonly string _localizationKey = localizationKey;

        public override void Update(in float delta)
        {
            foreach (CraftingRecipeData activeRecipe in _activeRecipes)
            {
                activeRecipe.ElapsedTime = Math.Clamp(activeRecipe.ElapsedTime + delta, 0, activeRecipe.Data.CraftTime);
                if (activeRecipe.ElapsedTime >= activeRecipe.Data.CraftTime)
                {
                    _activeRecipes.Remove(activeRecipe);
                    _doneRecipes.Add(activeRecipe);
                }
            }
        }

        public override IReadOnlyCraftingRecipeData Add(RecipeData data, Inventory target)
        {
            CraftingRecipeData craft = new(data, target);
            _activeRecipes.Add(craft);
            return craft;
        }

        public override void Remove(IReadOnlyCraftingRecipeData data) { _activeRecipes.Remove((CraftingRecipeData)data); }
    }
}
