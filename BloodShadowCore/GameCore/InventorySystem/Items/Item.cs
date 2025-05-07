namespace BloodShadow.GameCore.InventorySystem.Items
{
    public abstract class Item
    {
        public abstract string LocalizationKey { get; }
        public abstract string Description { get; }
        public abstract byte[] Icon { get; }
        public abstract bool Stackable { get; }
    }
}
