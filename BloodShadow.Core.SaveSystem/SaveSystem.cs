using BloodShadow.Core.Extensions;
using BloodShadow.Core.Logger;

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

        public virtual bool Save(string location, object data, bool createIfNotExists = true)
        {
            try
            {
                byte[] dataToWrite = SerializeModule.Serialize(data);
                if (!dataToWrite.Valid())
                {
                    Label.WriteLineWarning($"Serialized data not valid. Location: {location}");
                    return false;
                }
                if (EncryptModule != null)
                {
                    dataToWrite = EncryptModule.Encrypt(dataToWrite);
                    if (!dataToWrite.Valid())
                    {
                        Label.WriteLineWarning($"Data after encryption is not valid. Location: {location}");
                        return false;
                    }
                }
                bool resourceAvailable = VerifyResource(location, createIfNotExists);
                if (!resourceAvailable)
                {
                    Label.WriteLineWarning($"No resource at {location}");
                    return false;
                }
                bool result = IOModule.Write(location, dataToWrite);
                if (!result) { Label.WriteLineWarning($"Failed to write into {location}"); }
                return result;
            }
            catch (Exception exception) { Label.WriteLineException($"Exception while save to {location}.", exception); }
            return false;
        }
        public virtual async Task<bool> SaveAsync(string location, object data, bool createIfNotExists = true)
        {
            try
            {
                byte[] dataToWrite = await SerializeModule.SerializeAsync(data);
                if (!dataToWrite.Valid())
                {
                    Label.WriteLineWarning($"Serialized data not valid. Location: {location}");
                    return false;
                }
                if (EncryptModule != null)
                {
                    dataToWrite = await EncryptModule.EncryptAsync(dataToWrite);
                    if (!dataToWrite.Valid())
                    {
                        Label.WriteLineWarning($"Data after encryption is not valid. Location: {location}");
                        return false;
                    }
                }
                bool resourceAvailable = await VerifyResourceAsync(location, createIfNotExists);
                if (!resourceAvailable)
                {
                    Label.WriteLineWarning($"No resource at {location}");
                    return false;
                }
                bool result = await IOModule.WriteAsync(location, dataToWrite);
                if (!result) { Label.WriteLineWarning($"Failed to write into {location}"); }
                return result;
            }
            catch (Exception exception) { Label.WriteLineException($"Exception while save to {location}.", exception); }
            return false;
        }

        public virtual T? Load<T>(string location, bool createIfNotExists = true)
        {
            try
            {
                bool resourceAvailable = VerifyResource(location, createIfNotExists);
                if (!resourceAvailable)
                {
                    Label.WriteLineWarning($"No resource at {location}");
                    return default;
                }
                byte[] data = IOModule.Read(location);
                if (data.Valid())
                {
                    Label.WriteLineWarning($"Data from {location} is not valid");
                    return default;
                }
                if (EncryptModule != null)
                {
                    data = EncryptModule.Decrypt(data);
                    if (!data.Valid())
                    {
                        Label.WriteLineWarning($"Data after decrypt is not valid. Location: {location}");
                        return default;
                    }
                }
                T? result = SerializeModule.Deserialize<T>(data);
                if (result == null) { Label.WriteLineWarning($"Failed to deserialize data from {location}"); }
                return result;
            }
            catch (Exception exception) { Label.WriteLineException($"Exception while load from {location}.", exception); }
            return default;
        }
        public virtual async Task<T?> LoadAsync<T>(string location, bool createIfNotExists = true)
        {
            try
            {
                bool resourceAvailable = await VerifyResourceAsync(location, createIfNotExists);
                if (!resourceAvailable)
                {
                    Label.WriteLineWarning($"No resource at {location}");
                    return default;
                }
                byte[] data = await IOModule.ReadAsync(location);
                if (data.Valid())
                {
                    Label.WriteLineWarning($"Data from {location} is not valid");
                    return default;
                }
                if (EncryptModule != null)
                {
                    data = await EncryptModule.DecryptAsync(data);
                    if (!data.Valid())
                    {
                        Label.WriteLineWarning($"Data after decrypt is not valid. Location: {location}");
                        return default;
                    }
                }
                T? result = await SerializeModule.DeserializeAsync<T>(data);
                if (result == null) { Label.WriteLineWarning($"Failed to deserialize data from {location}"); }
                return result;
            }
            catch (Exception exception) { Label.WriteLineException($"Exception while load from {location}.", exception); }
            return default;
        }

        public virtual bool VerifyCollection(string collectionLocation, bool createIfNotExists) => IOModule.VerifyCollection(collectionLocation, createIfNotExists);
        public virtual Task<bool> VerifyCollectionAsync(string collectionLocation, bool createIfNotExists) => IOModule.VerifyCollectionAsync(collectionLocation, createIfNotExists);
        public virtual bool VerifyResource(string resourceLocation, bool createIfNotExists) => IOModule.VerifyResource(resourceLocation, createIfNotExists);
        public virtual Task<bool> VerifyResourceAsync(string resourceLocation, bool createIfNotExists) => IOModule.VerifyResourceAsync(resourceLocation, createIfNotExists);
    }
}