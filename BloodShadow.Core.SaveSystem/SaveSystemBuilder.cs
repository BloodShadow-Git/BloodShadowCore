namespace BloodShadow.Core.SaveSystem
{
    public class SaveSystemBuilder : IBuilder<SaveSystem>
    {
        public bool CloneAvailable => true;
        public bool BuildAvailable => SerializeSystem != null && IOSystem != null;
        private SerializeModule.SerializeModule? SerializeSystem;
        private StorageModule.StorageModule? IOSystem;
        private EncryptModule.EncryptModule? EncryptModule = null;

        public SaveSystemBuilder SetSerializeSystem(SerializeModule.SerializeModule? serializeSystem)
        {
            if (serializeSystem != null) { SerializeSystem = serializeSystem; }
            return this;
        }
        public SaveSystemBuilder SetIOSystem(StorageModule.StorageModule? ioSystem)
        {
            if (ioSystem != null) { IOSystem = ioSystem; }
            return this;
        }
        public SaveSystemBuilder AddEncryption(EncryptModule.EncryptModule? encryptModule)
        {
            if (encryptModule != null)
            {
                if (EncryptModule != null) { EncryptModule = new EncryptModule.EncryptModuleDecorator(EncryptModule, encryptModule); }
                else { EncryptModule = encryptModule; }
            }
            return this;
        }

        public SaveSystem Build()
        {
            if (SerializeSystem == null) { throw new OperationCanceledException("Serialize system not set"); }
            if (IOSystem == null) { throw new OperationCanceledException("IOSystem system not set"); }
            return new SaveSystem(IOSystem, SerializeSystem, EncryptModule);
        }

        public IBuilder<SaveSystem> Clone()
        {
            return new SaveSystemBuilder()
                .SetSerializeSystem(SerializeSystem)
                .SetIOSystem(IOSystem)
                .AddEncryption(EncryptModule);
        }
    }
}
