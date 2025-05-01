using BloodShadow.Core.SaveSystem;

namespace BloodShadow.CoreGame.InventorySystem.Inventory
{
    public class InventoryManager(string savePath, string runtimeData, string inventoriesFile, SaveSystem saveSystem)
    {
        public IEnumerable<InventoryData> this[string key] => _data[key];
        public IEnumerable<InventoryData> this[Inventory inventory] => _data[inventory.LocalizationKey];
        public IDictionary<string, IEnumerable<InventoryData>> Data => _data;

        private Dictionary<string, IEnumerable<InventoryData>> _data;
        private readonly SaveSystem _saveSystem = saveSystem;
        private readonly string _savePath = Path.Combine(savePath, runtimeData, inventoriesFile);

        public InventoryManager(string savePath, string runtimeData, string inventoriesFile) : this(savePath, runtimeData, inventoriesFile, new JsonSaveSystem())
        { _saveSystem.Load<Dictionary<string, IEnumerable<InventoryData>>>(_savePath, data => { _data = data ?? []; }); }

        public void Save() { _saveSystem.Save(_savePath, _data); }

        public bool AddInventory(Inventory inventory) { return _data.TryAdd(inventory.LocalizationKey, inventory.Items); }
        public bool RemoveInventory(Inventory inventory) { return _data.Remove(inventory.LocalizationKey); }
        public bool HasInventory(Inventory inventory) { return _data.ContainsKey(inventory.LocalizationKey); }
    }
}
