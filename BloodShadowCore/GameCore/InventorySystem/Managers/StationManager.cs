using BloodShadow.GameCore.InventorySystem.Recipes;

namespace BloodShadow.GameCore.InventorySystem.Managers
{
    using BloodShadow.Core.SaveSystem;

    public class StationManager(string inventoriesPath, SaveSystem saveSystem)
    {
        public IEnumerable<IReadOnlyCraftingRecipeData> this[string key] => _data[key];
        public IEnumerable<IReadOnlyCraftingRecipeData> this[CraftStation station] => _data[station.LocalizationKey];
        public IDictionary<string, IEnumerable<IReadOnlyCraftingRecipeData>> Data => _data;

        private Dictionary<string, IEnumerable<IReadOnlyCraftingRecipeData>> _data;
        private readonly SaveSystem _saveSystem = saveSystem;
        private readonly string _savePath = inventoriesPath;

        public StationManager(string inventoriesPath) : this(inventoriesPath, new JsonSaveSystem())
        { _saveSystem.Load<Dictionary<string, IEnumerable<IReadOnlyCraftingRecipeData>>>(_savePath, data => { _data = data ?? []; }); }

        public void Save() { _saveSystem.Save(_savePath, _data); }

        public bool AddInventory(CraftStation station) { return _data.TryAdd(station.LocalizationKey, station.ActiveRecipes); }
        public bool RemoveInventory(CraftStation station) { return _data.Remove(station.LocalizationKey); }
        public bool HasInventory(CraftStation station) { return _data.ContainsKey(station.LocalizationKey); }
    }
}
