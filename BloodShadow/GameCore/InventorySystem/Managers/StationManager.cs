using BloodShadow.GameCore.InventorySystem.Recipes;

namespace BloodShadow.GameCore.InventorySystem.Managers
{
    using BloodShadow.Core.SaveSystem;
    using System.Collections.Generic;

    public class StationManager
    {
        public IEnumerable<IReadOnlyCraftingRecipeData> this[string key] => _data[key];
        public IEnumerable<IReadOnlyCraftingRecipeData> this[CraftStation station] => _data[station.LocalizationKey];
        public IDictionary<string, IEnumerable<IReadOnlyCraftingRecipeData>> Data => _data;

        private Dictionary<string, IEnumerable<IReadOnlyCraftingRecipeData>> _data;
        private readonly SaveSystem _saveSystem;
        private readonly string _savePath;

        public StationManager(string inventoriesPath, SaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
            _savePath = inventoriesPath;
            _data = new Dictionary<string, IEnumerable<IReadOnlyCraftingRecipeData>>();
        }
        public StationManager(string inventoriesPath) : this(inventoriesPath, new JsonSaveSystem())
        {
            _saveSystem.Load<Dictionary<string, IEnumerable<IReadOnlyCraftingRecipeData>>>(_savePath,
            data => { _data = data ?? new Dictionary<string, IEnumerable<IReadOnlyCraftingRecipeData>>(); });
        }

        public void Save() { _saveSystem.Save(_savePath, _data); }

        public bool AddInventory(CraftStation station) { return _data.TryAdd(station.LocalizationKey, station.ActiveRecipes); }
        public bool RemoveInventory(CraftStation station) { return _data.Remove(station.LocalizationKey); }
        public bool HasInventory(CraftStation station) { return _data.ContainsKey(station.LocalizationKey); }
    }
}
