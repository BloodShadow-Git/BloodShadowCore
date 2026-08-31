namespace BloodShadow.Core.SaveSystem.StorageModule
{
    public struct Resource
    {
        public string Name { get; private set; }
        public ResourceType ResourceType { get; private set; }

        public Resource(string name, ResourceType resourceType)
        {
            if (resourceType == ResourceType.Unknown) { throw new Exception("Invalid resource type"); }
            Name = name;
            ResourceType = resourceType;
        }
    }
}
