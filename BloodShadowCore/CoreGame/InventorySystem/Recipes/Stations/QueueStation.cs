namespace BloodShadow.CoreGame.InventorySystem.Recipes.Stations
{
    using Inventory;
    using BloodShadow.Core.Extensions;

    public class QueueStation(string localizationKey) : CraftStation
    {
        public override string LocalizationKey => _localizationKey;
        public override IEnumerable<IReadOnlyCraftingRecipeData> ActiveRecipes => _activeRecipes;
        public override List<IReadOnlyCraftingRecipeData> DoneRecipes => [.. _doneRecipes.Cast<IReadOnlyCraftingRecipeData>()];

        private Queue<CraftingRecipeData> _activeRecipes = [];
        private readonly List<CraftingRecipeData> _doneRecipes = [];
        private readonly string _localizationKey = localizationKey;

        public override void Update(in float delta)
        {
            if (_activeRecipes.TryPeek(out CraftingRecipeData recipe))
            {
                recipe.ElapsedTime = Math.Clamp(recipe.ElapsedTime + delta, 0, recipe.Data.CraftTime);
                if (recipe.ElapsedTime >= recipe.Data.CraftTime)
                {
                    _activeRecipes.Dequeue();
                    _doneRecipes.Add(recipe);
                }
            }
        }

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
