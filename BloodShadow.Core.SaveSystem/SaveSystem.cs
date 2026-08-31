using BloodShadow.Core.Extensions;
using BloodShadow.Core.Logger;
using BloodShadow.Core.SaveSystem.StorageModule;

namespace BloodShadow.Core.SaveSystem
{
    public class SaveSystem
    {
        protected StorageModule.StorageModule IOModule { get; private set; }
        protected SerializeModule.SerializeModule SerializeModule { get; private set; }
        protected EncryptModule.EncryptModule? EncryptModule { get; private set; }
        protected LoggerLabel Label { get; private set; }

        public SaveSystem(StorageModule.StorageModule iOSystem, SerializeModule.SerializeModule serializeSystem, EncryptModule.EncryptModule? encryptModule)
        {
            IOModule = iOSystem;
            SerializeModule = serializeSystem;
            EncryptModule = encryptModule;
            Label = new(GetType().Name);
        }

        #region Save Methods
        public virtual bool Save(StorageKey location, object data, bool createIfNotExists = true)
        {
            try
            {
                byte[] dataToWrite = SerializeModule.Serialize(data);
                if (!dataToWrite.Valid())
                {
                    Label.WriteLine(MessageChanel.WARN, "Serialized data not valid. Location: {0}", null, null, location);
                    return false;
                }
                if (EncryptModule != null)
                {
                    dataToWrite = EncryptModule.Encrypt(dataToWrite);
                    if (!dataToWrite.Valid())
                    {
                        Label.WriteLine(MessageChanel.WARN, "Data after encryption is not valid. Location: {0}", null, null, location);
                        return false;
                    }
                }
                bool resourceAvailable = VerifyResource(location, createIfNotExists);
                if (!resourceAvailable)
                {
                    Label.WriteLine(MessageChanel.WARN, "No resource at {0}", null, null, location);
                    return false;
                }
                bool result = IOModule.Write(location, dataToWrite);
                if (!result) { Label.WriteLine(MessageChanel.WARN, "Failed to write into {0}", null, null, location); }
                return result;
            }
            catch (Exception exception) { Label.WriteLine(MessageChanel.ERROR, "Exception while save to {0}.", new(1), exception, location); }
            return false;
        }
        public virtual async Task<bool> SaveAsync(StorageKey location, object data, bool createIfNotExists = true)
        {
            try
            {
                byte[] dataToWrite = await SerializeModule.SerializeAsync(data);
                if (!dataToWrite.Valid())
                {
                    Label.WriteLine(MessageChanel.WARN, "Serialized data not valid. Location: {0}", null, null, location);
                    return false;
                }
                if (EncryptModule != null)
                {
                    dataToWrite = await EncryptModule.EncryptAsync(dataToWrite);
                    if (!dataToWrite.Valid())
                    {
                        Label.WriteLine(MessageChanel.WARN, "Data after encryption is not valid. Location: {0}", null, null, location);
                        return false;
                    }
                }
                bool resourceAvailable = await VerifyResourceAsync(location, createIfNotExists);
                if (!resourceAvailable)
                {
                    Label.WriteLine(MessageChanel.WARN, "No resource at {0}", null, null, location);
                    return false;
                }
                bool result = await IOModule.WriteAsync(location, dataToWrite);
                if (!result) { Label.WriteLine(MessageChanel.WARN, "Failed to write into {0}", null, null, location); }
                return result;
            }
            catch (Exception exception) { Label.WriteLine(MessageChanel.ERROR, "Exception while save to {0}.", new(1), exception, location); }
            return false;
        }
        #endregion

        #region Load Methods 
        public virtual T? Load<T>(StorageKey location, bool createIfNotExists = true)
        {
            try
            {
                bool resourceAvailable = VerifyResource(location, createIfNotExists);
                if (!resourceAvailable)
                {
                    Label.WriteLine(MessageChanel.WARN, "No resource at {0}", null, null, location);
                    return default;
                }
                byte[] data = IOModule.Read(location);
                if (data.Valid())
                {
                    Label.WriteLine(MessageChanel.WARN, "Data from {0} is not valid", null, null, location);
                    return default;
                }
                if (EncryptModule != null)
                {
                    data = EncryptModule.Decrypt(data);
                    if (!data.Valid())
                    {
                        Label.WriteLine(MessageChanel.WARN, "Data after decrypt is not valid. Location: {0}", null, null, location);
                        return default;
                    }
                }
                T? result = SerializeModule.Deserialize<T>(data);
                if (result == null) { Label.WriteLine(MessageChanel.WARN, "Failed to deserialize data from {0}", null, null, location); }
                return result;
            }
            catch (Exception exception) { Label.WriteLine(MessageChanel.ERROR, "Exception while load from {0}.", new(1), exception, location); }
            return default;
        }
        public virtual async Task<T?> LoadAsync<T>(StorageKey location, bool createIfNotExists = true)
        {
            try
            {
                bool resourceAvailable = await VerifyResourceAsync(location, createIfNotExists);
                if (!resourceAvailable)
                {
                    Label.WriteLine(MessageChanel.WARN, "No resource at {0}", null, null, location);
                    return default;
                }
                byte[] data = await IOModule.ReadAsync(location);
                if (data.Valid())
                {
                    Label.WriteLine(MessageChanel.WARN, "Data from {0} is not valid", null, null, location);
                    return default;
                }
                if (EncryptModule != null)
                {
                    data = await EncryptModule.DecryptAsync(data);
                    if (!data.Valid())
                    {
                        Label.WriteLine(MessageChanel.WARN, "Data after decrypt is not valid. Location: {0}", null, null, location);
                        return default;
                    }
                }
                T? result = await SerializeModule.DeserializeAsync<T>(data);
                if (result == null) { Label.WriteLine(MessageChanel.WARN, "Failed to deserialize data from {0}", null, null, location); }
                return result;
            }
            catch (Exception exception) { Label.WriteLine(MessageChanel.ERROR, "Exception while load from {0}.", new(1), exception, location); }
            return default;
        }
        #endregion

        #region VerifyCollection Methods
        public virtual bool VerifyCollection(StorageKey location) => IOModule.VerifyCollection(location);
        public virtual bool VerifyCollection(StorageKey location, bool createIfNotExists) => IOModule.VerifyCollection(location, createIfNotExists);
        public virtual Task<bool> VerifyCollectionAsync(StorageKey location) => IOModule.VerifyCollectionAsync(location);
        public virtual Task<bool> VerifyCollectionAsync(StorageKey location, bool createIfNotExists) => IOModule.VerifyCollectionAsync(location, createIfNotExists);
        #endregion

        #region VerifyResource Methods
        public virtual bool VerifyResource(StorageKey location) => IOModule.VerifyResource(location);
        public virtual bool VerifyResource(StorageKey location, bool createIfNotExists) => IOModule.VerifyResource(location, createIfNotExists);
        public virtual Task<bool> VerifyResourceAsync(StorageKey location) => IOModule.VerifyResourceAsync(location);
        public virtual Task<bool> VerifyResourceAsync(StorageKey location, bool createIfNotExists) => IOModule.VerifyResourceAsync(location, createIfNotExists);
        #endregion

        #region IsResource Methods
        public virtual bool IsResource(StorageKey location) => IOModule.IsResource(location);
        public virtual Task<bool> IsResourceAsync(StorageKey location) => IOModule.IsResourceAsync(location);
        #endregion

        #region IsCollection Methods
        public virtual bool IsCollection(StorageKey location) => IOModule.IsCollection(location);
        public virtual Task<bool> IsCollectionAsync(StorageKey location) => IOModule.IsCollectionAsync(location);
        #endregion

        #region EnumerateCollection Methods
        public virtual IEnumerable<Resource> EnumerateCollection(StorageKey location) => IOModule.EnumerateCollection(location);
        public virtual IAsyncEnumerable<Resource> EnumerateCollectionAsync(StorageKey location) => IOModule.EnumerateCollectionAsync(location);
        #endregion

        public virtual StorageWatcher CreateWatcher(StorageKey location) => IOModule.CreateWatcher(location);

        #region Resource Management
        public virtual bool CreateResource(StorageKey location) => IOModule.CreateResource(location);
        public virtual Task<bool> CreateResourceAsync(StorageKey location) => IOModule.CreateResourceAsync(location);
        public virtual bool RemoveResource(StorageKey location) => IOModule.RemoveResource(location);
        public virtual Task<bool> RemoveResourceAsync(StorageKey location) => IOModule.RemoveResourceAsync(location);
        #endregion

        #region Collection Management
        public virtual bool CreateCollection(StorageKey location) => IOModule.CreateCollection(location);
        public virtual Task<bool> CreateCollectionAsync(StorageKey location) => IOModule.CreateCollectionAsync(location);
        public virtual bool RemoveCollection(StorageKey location) => IOModule.RemoveCollection(location);
        public virtual Task<bool> RemoveCollectionAsync(StorageKey location) => IOModule.RemoveCollectionAsync(location);
        #endregion

        #region Exists Methods
        public virtual ResourceType Exists(StorageKey location) => IOModule.Exists(location);
        public virtual Task<ResourceType> ExistsAsync(StorageKey location) => IOModule.ExistsAsync(location);
        #endregion
    }
}