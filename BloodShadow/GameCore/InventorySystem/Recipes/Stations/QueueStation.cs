namespace BloodShadow.GameCore.InventorySystem.Recipes.Stations
{
    using BloodShadow.GameCore.InventorySystem.Inventory;
    using BloodShadow.GameCore.InventorySystem.Recipes;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class QueueStation : CraftStation
    {
        public override string LocalizationKey => _localizationKey;
        public override IEnumerable<IReadOnlyCraftingRecipeData> ActiveRecipes => _activeRecipes;
        public override List<IReadOnlyCraftingRecipeData> DoneRecipes => _doneRecipes.Cast<IReadOnlyCraftingRecipeData>().ToList();

        private Queue<CraftingRecipeData> _activeRecipes = new Queue<CraftingRecipeData>();
        private readonly List<CraftingRecipeData> _doneRecipes = new List<CraftingRecipeData>();
        private readonly string _localizationKey;

        public QueueStation(string localizationKey)
        {
            _localizationKey = localizationKey;
        }
        public override IReadOnlyCraftingRecipeData Add(RecipeData data, Inventory target)
        {
            CraftingRecipeData craft = new CraftingRecipeData(data, target);
            _activeRecipes.Enqueue(craft);
            return craft;
        }

        public override void Remove(IReadOnlyCraftingRecipeData data)
        {
            List<CraftingRecipeData> list = new List<CraftingRecipeData>(_activeRecipes);
            list.Remove((CraftingRecipeData)data);
            _activeRecipes = new Queue<CraftingRecipeData>(list);
        }
    }
}
