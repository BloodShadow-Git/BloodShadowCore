using BloodShadow.GameCore.InventorySystem.Recipes;

namespace BloodShadow.GameCore.InventorySystem.Managers
{
    using System.Collections.Generic;
    using System.Linq;

    public class RecipeManager
    {
        public IEnumerable<IReadOnlyRecipeData> this[string station] => _data[station];
        public IEnumerable<IReadOnlyRecipeData> this[CraftStation station] => _data[station.LocalizationKey];
        public IDictionary<string, IEnumerable<IReadOnlyRecipeData>> Data => new Dictionary<string, IEnumerable<IReadOnlyRecipeData>>
            (_data.Select(input => new KeyValuePair<string, IEnumerable<IReadOnlyRecipeData>>(input.Key, input.Value)));

        private readonly Dictionary<string, List<IReadOnlyRecipeData>> _data;

        public RecipeManager() { _data = new Dictionary<string, List<IReadOnlyRecipeData>>(); }

        public void AddRecipe(RecipeData recipeData)
        {
            foreach (string station in recipeData.TargetStations)
            {
                if (!_data.TryGetValue(station, out List<IReadOnlyRecipeData> recipeList))
                {
                    recipeList = new List<IReadOnlyRecipeData>() { recipeData };
                    _data[station] = recipeList;
                }
                if (!recipeList.Contains(recipeData)) { recipeList.Add(recipeData); }
            }
        }
        public void RemoveRecipe(RecipeData recipeData)
        { foreach (string station in recipeData.TargetStations) { if (_data.TryGetValue(station, out List<IReadOnlyRecipeData> recipes)) { recipes.Remove(recipeData); } } }
    }
}
