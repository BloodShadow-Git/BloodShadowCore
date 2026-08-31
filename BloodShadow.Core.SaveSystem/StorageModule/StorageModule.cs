using BloodShadow.Core.Extensions;
using BloodShadow.Core.Logger;

namespace BloodShadow.Core.SaveSystem.StorageModule
{
    public abstract class StorageModule
    {
        protected LoggerLabel Logger;
        public StorageModule() { Logger = new(GetType().Name); }

        #region Write Methods
        public bool Write(StorageKey location, byte[] content)
        {
            if (!VerifyResource(location))
            {
                Logger.WriteLine(MessageChanel.WARN, "Empty location. Location: {0}", null, null, location);
                return false;
            }
            Logger.WriteLine(MessageChanel.INFO, "Writing to {0}", null, null, location);
            if (!content.Valid()) { Logger.WriteLine(MessageChanel.WARN, "Empty content", null, null); }
            bool res = false;
            try { res = WriteInternal(location, content); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while writing at {0}.", new(1), exception, location); }
            finally { if (!res) { Logger.WriteLine(MessageChanel.WARN, "Failed to write to {0}", null, null, location); } }
            return res;
        }
        protected abstract bool WriteInternal(StorageKey location, byte[] content);
        public async Task<bool> WriteAsync(StorageKey location, byte[] content)
        {
            if (!await VerifyResourceAsync(location))
            {
                Logger.WriteLine(MessageChanel.WARN, "Empty location. Location: {0}", null, null, location);
                return false;
            }
            Logger.WriteLine(MessageChanel.INFO, "Writing to {0}", null, null, location);
            if (!content.Valid()) { Logger.WriteLine(MessageChanel.WARN, "Empty content", null, null); }
            bool result = false;
            try { result = await WriteAsyncInternal(location, content); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while writing at {0}.", new(1), exception, location); }
            finally { if (!result) { Logger.WriteLine(MessageChanel.WARN, "Failed to write to {0}", null, null, location); } }
            return result;
        }
        protected abstract Task<bool> WriteAsyncInternal(StorageKey location, byte[] content);
        #endregion

        #region Read Methods
        public byte[] Read(StorageKey location)
        {
            if (!VerifyResource(location))
            {
                Logger.WriteLine(MessageChanel.WARN, "Empty location. Location: {0}", null, null, location);
                return [];
            }
            Logger.WriteLine(MessageChanel.INFO, "Reading from {0}", null, null, location);
            byte[] result = [];
            try { result = ReadInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while reading from {0}.", new(1), exception, location); }
            finally { if (!result.Valid()) { Logger.WriteLine(MessageChanel.WARN, "Empty file content. If it normal you can ignore this warning", null, null); } }
            return result;
        }
        protected abstract byte[] ReadInternal(StorageKey location);
        public async Task<byte[]> ReadAsync(StorageKey location)
        {
            if (!await VerifyResourceAsync(location))
            {
                Logger.WriteLine(MessageChanel.WARN, "Empty location. Location: {0}", null, null, location);
                return [];
            }
            Logger.WriteLine(MessageChanel.INFO, "Reading from {0}", null, null, location);
            byte[] result = [];
            try { result = await ReadAsyncInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while reading at {0}.", new(1), exception); }
            finally { if (!result.Valid()) { Logger.WriteLine(MessageChanel.WARN, "Empty file content. If it normal you can ignore this warning", null, null); } }
            return result;
        }
        protected abstract Task<byte[]> ReadAsyncInternal(StorageKey location);
        #endregion

        #region VerifyCollection Methods
        public bool VerifyCollection(StorageKey collectionLocation, bool createIfNotExists)
        {
            Logger.WriteLine(MessageChanel.INFO, "Verifying collection at {0}", null, null, collectionLocation);
            bool res = false;
            try { res = VerifyCollectionInternal(collectionLocation, createIfNotExists); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while verifying collection at {0}.", new(1), exception, collectionLocation); }
            finally { if (createIfNotExists && !res) { Logger.WriteLine(MessageChanel.WARN, "Collection at {0} cannot be fixed", null, null, collectionLocation); } }
            return res;

        }
        public bool VerifyCollection(StorageKey collectionLocation) => VerifyCollection(collectionLocation, false);
        protected abstract bool VerifyCollectionInternal(StorageKey collectionLocation, bool createIfNotExists);
        public async Task<bool> VerifyCollectionAsync(StorageKey collectionLocation, bool createIfNotExists)
        {
            Logger.WriteLine(MessageChanel.INFO, "Verifying collection at {0}", null, null, collectionLocation);
            bool res = false;
            try { res = await VerifyCollectionAsyncInternal(collectionLocation, createIfNotExists); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while verifying collection at {0}.", new(1), exception, collectionLocation); }
            finally { if (createIfNotExists && !res) { Logger.WriteLine(MessageChanel.WARN, "Collection at {0} cannot be fixed", null, null, collectionLocation); } }
            return res;

        }
        public Task<bool> VerifyCollectionAsync(StorageKey collectionLocation) => VerifyCollectionAsync(collectionLocation, false);
        protected abstract Task<bool> VerifyCollectionAsyncInternal(StorageKey collectionLocation, bool createIfNotExists);
        #endregion

        #region VerifyResource Methods
        public bool VerifyResource(StorageKey resourceLocation, bool createIfNotExists)
        {
            Logger.WriteLine(MessageChanel.INFO, "Verifying resource at {0}", null, null, resourceLocation);
            bool res = false;
            try { res = VerifyResourceInternal(resourceLocation, createIfNotExists); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while verifying resource at {0}.", new(1), exception, resourceLocation); }
            finally { if (createIfNotExists && !res) { Logger.WriteLine(MessageChanel.WARN, "Resource at {0} cannot be fixed", null, null, resourceLocation); } }
            return res;
        }
        public bool VerifyResource(StorageKey resourceLocation) => VerifyResource(resourceLocation, false);
        protected abstract bool VerifyResourceInternal(StorageKey resourceLocation, bool createIfNotExists);
        public async Task<bool> VerifyResourceAsync(StorageKey resourceLocation, bool createIfNotExists)
        {
            Logger.WriteLine(MessageChanel.INFO, "Verifying resource at {0}", null, null, resourceLocation);
            bool res = false;
            try { res = await VerifyResourceAsyncInternal(resourceLocation, createIfNotExists); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while verifying resource at {0}.", new(1), exception, resourceLocation); }
            finally { if (createIfNotExists && !res) { Logger.WriteLine(MessageChanel.WARN, "Resource at {0} cannot be fixed", null, null, resourceLocation); } }
            return res;
        }
        public Task<bool> VerifyResourceAsync(StorageKey resourceLocation) => VerifyResourceAsync(resourceLocation, false);
        protected abstract Task<bool> VerifyResourceAsyncInternal(StorageKey resourceLocation, bool createIfNotExists);
        #endregion

        #region IsResource Methods
        public bool IsResource(StorageKey location)
        {
            Logger.WriteLine(MessageChanel.INFO, "Checking resource at {0}", null, null, location);
            try { return IsResourceInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while checking resource", new(1), exception); }
            return false;
        }
        protected abstract bool IsResourceInternal(StorageKey location);
        public async Task<bool> IsResourceAsync(StorageKey location)
        {
            Logger.WriteLine(MessageChanel.INFO, "Checking resource at {0}", null, null, location);
            try { return await IsResourceAsyncInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while checking resource", new(1), exception); }
            return false;
        }
        protected abstract Task<bool> IsResourceAsyncInternal(StorageKey location);
        #endregion

        #region IsCollection Methods
        public bool IsCollection(StorageKey location)
        {
            Logger.WriteLine(MessageChanel.INFO, "Checking collection at {0}", null, null, location);
            try { return IsCollectionInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while checking collection", new(1), exception); }
            return false;
        }
        protected abstract bool IsCollectionInternal(StorageKey location);
        public async Task<bool> IsCollectionAsync(StorageKey location)
        {
            Logger.WriteLine(MessageChanel.INFO, "Checking collection at {0}", null, null, location);
            try { return await IsCollectionAsyncInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while checking collection", new(1), exception); }
            return false;
        }
        protected abstract Task<bool> IsCollectionAsyncInternal(StorageKey location);
        #endregion

        #region EnumerateCollection Methods
        public IEnumerable<Resource> EnumerateCollection(StorageKey location)
        {
            Logger.WriteLine(MessageChanel.INFO, "Enumerating collection at {0}", null, null, location);
            if (!IsCollection(location))
            {
                Logger.WriteLine(MessageChanel.ERROR, "Location is not collection. Location: {0}", new(1), new ArgumentException("Location is not collection", nameof(location)), location);
                return [];
            }
            try { return EnumerateCollectionInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while enumerating {0}.", new(1), exception, location); }
            return [];
        }
        protected abstract IEnumerable<Resource> EnumerateCollectionInternal(StorageKey location);
        public IAsyncEnumerable<Resource> EnumerateCollectionAsync(StorageKey location)
        {
            Logger.WriteLine(MessageChanel.INFO, "Enumerating collection at {location}", null, null, location);
            if (!IsCollection(location))
            {
                Logger.WriteLine(MessageChanel.ERROR, "Location is not collection. Location: {0}", new(1), new ArgumentException("Location is not collection", nameof(location)), location);
                return Array.Empty<Resource>().ToAsyncEnumerable();
            }
            try { return EnumerateCollectionAsyncInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while enumerating {0}.", new(1), exception, location); }
            return Array.Empty<Resource>().ToAsyncEnumerable();
        }
        protected abstract IAsyncEnumerable<Resource> EnumerateCollectionAsyncInternal(StorageKey location);
        #endregion

        #region CreateWatcher Methods
        public StorageWatcher CreateWatcher(StorageKey location)
        {
            if (!IsCollection(location))
            {
                Logger.WriteLine(MessageChanel.ERROR, "Location is not collection. Location: {0}", new(1), new ArgumentException("Location is not collection", nameof(location)), location);
                return null!;
            }
            StorageWatcher sw = null!;
            try { sw = CreateWatcherInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while creating watcher for {0}.", new(1), exception, location); }
            finally { if (sw == null) { Logger.WriteLine(MessageChanel.WARN, "Watcher was created but result is null", null, null); } }
            return sw;
        }
        protected abstract StorageWatcher CreateWatcherInternal(StorageKey location);
        #endregion

        #region Resource Management
        #region CreateResource Methods
        public bool CreateResource(StorageKey location)
        {
            Logger.WriteLine(MessageChanel.INFO, "Creating resource at {0}", null, null, location);
            bool result;
            try { result = CreateResourceInternal(location); }
            catch (Exception ex)
            {
                Logger.WriteLine(MessageChanel.ERROR, "Failed to create resource at {0}", new(1), ex, location);
                return false;
            }
            if (!result) { Logger.WriteLine(MessageChanel.DEBUG, "Cannot create resource at {0}", null, null, location); }
            return result;
        }
        protected abstract bool CreateResourceInternal(StorageKey location);
        public async Task<bool> CreateResourceAsync(StorageKey location)
        {
            Logger.WriteLine(MessageChanel.INFO, "Creating resource at {0}", null, null, location);
            bool result;
            try { result = await CreateResourceAsyncInternal(location); }
            catch (Exception ex)
            {
                Logger.WriteLine(MessageChanel.ERROR, "Failed to create resource at {0}", new(1), ex, location);
                return false;
            }
            if (!result) { Logger.WriteLine(MessageChanel.DEBUG, "Cannot create resource at {0}", null, null, location); }
            return result;
        }
        protected abstract Task<bool> CreateResourceAsyncInternal(StorageKey location);
        #endregion

        #region RemoveResource Methods
        public bool RemoveResource(StorageKey location)
        {
            Logger.WriteLine(MessageChanel.INFO, "Removing resource at {0}", null, null, location);
            bool result;
            try { result = RemoveResourceInternal(location); }
            catch (Exception ex)
            {
                Logger.WriteLine(MessageChanel.ERROR, "Failed to remove resource at {0}", new(1), ex, location);
                return false;
            }
            if (!result) { Logger.WriteLine(MessageChanel.DEBUG, "Cannot remove resource at {0}", null, null, location); }
            return result;
        }
        protected abstract bool RemoveResourceInternal(StorageKey location);
        public async Task<bool> RemoveResourceAsync(StorageKey location)
        {
            Logger.WriteLine(MessageChanel.INFO, "Removing resource at {0}", null, null, location);
            bool result;
            try { result = await RemoveResourceAsyncInternal(location); }
            catch (Exception ex)
            {
                Logger.WriteLine(MessageChanel.ERROR, "Failed to remove resource at {0}", new(1), ex, location);
                return false;
            }
            if (!result) { Logger.WriteLine(MessageChanel.DEBUG, "Cannot remove resource at {0}", null, null, location); }
            return result;
        }
        protected abstract Task<bool> RemoveResourceAsyncInternal(StorageKey location);
        #endregion
        #endregion

        #region Collection Management
        #region CreateCollection Methods
        public bool CreateCollection(StorageKey location)
        {
            Logger.WriteLine(MessageChanel.INFO, "Creating collection at {0}", null, null, location);
            bool result;
            try { result = CreateCollectionInternal(location); }
            catch (Exception ex)
            {
                Logger.WriteLine(MessageChanel.ERROR, "Failed to create collection at {0}", new(1), ex, location);
                return false;
            }
            if (!result) { Logger.WriteLine(MessageChanel.DEBUG, "Cannot create collection at {0}", null, null, location); }
            return result;
        }
        protected abstract bool CreateCollectionInternal(StorageKey location);
        public async Task<bool> CreateCollectionAsync(StorageKey location)
        {
            Logger.WriteLine(MessageChanel.INFO, "Creating collection at {0}", null, null, location);
            bool result;
            try { result = await CreateCollectionAsyncInternal(location); }
            catch (Exception ex)
            {
                Logger.WriteLine(MessageChanel.ERROR, "Failed to create collection at {0}", new(1), ex, location);
                return false;
            }
            if (!result) { Logger.WriteLine(MessageChanel.DEBUG, "Cannot create collection at {0}", null, null, location); }
            return result;
        }
        protected abstract Task<bool> CreateCollectionAsyncInternal(StorageKey location);
        #endregion

        #region RemoveCollection Methods
        public bool RemoveCollection(StorageKey location)
        {
            Logger.WriteLine(MessageChanel.INFO, "Removing collection at {0}", null, null, location);
            bool result;
            try { result = RemoveCollectionInternal(location); }
            catch (Exception ex)
            {
                Logger.WriteLine(MessageChanel.ERROR, "Failed to remove collection at {0}", new(1), ex, location);
                return false;
            }
            if (!result) { Logger.WriteLine(MessageChanel.DEBUG, "Cannot remove collection at {0}", null, null, location); }
            return result;
        }
        protected abstract bool RemoveCollectionInternal(StorageKey location);
        public async Task<bool> RemoveCollectionAsync(StorageKey location)
        {
            Logger.WriteLine(MessageChanel.INFO, "Removing collection at {0}", null, null, location);
            bool result;
            try { result = await RemoveCollectionAsyncInternal(location); }
            catch (Exception ex)
            {
                Logger.WriteLine(MessageChanel.ERROR, "Failed to remove collection at {0}", new(1), ex, location);
                return false;
            }
            if (!result) { Logger.WriteLine(MessageChanel.DEBUG, "Cannot remove collection at {0}", null, null, location); }
            return result;
        }
        protected abstract Task<bool> RemoveCollectionAsyncInternal(StorageKey location);
        #endregion
        #endregion

        #region Exists Methods
        public ResourceType Exists(StorageKey location)
        {
            Logger.WriteLine(MessageChanel.INFO, "Resource classification at {0}", null, null, location);
            ResourceType result;
            try { result = ExistsInternal(location); }
            catch (Exception ex)
            {
                Logger.WriteLine(MessageChanel.ERROR, "Failed to resource classification at {0}", new(1), ex, location);
                return ResourceType.Unknown;
            }
            return result;
        }
        protected abstract ResourceType ExistsInternal(StorageKey location);
        public async Task<ResourceType> ExistsAsync(StorageKey location)
        {
            Logger.WriteLine(MessageChanel.INFO, "Resource classification at {0}", null, null, location);
            ResourceType result;
            try { result = await ExistsAsyncInternal(location); }
            catch (Exception ex)
            {
                Logger.WriteLine(MessageChanel.ERROR, "Failed to resource classification at {0}", new(1), ex, location);
                return ResourceType.Unknown;
            }
            return result;
        }
        protected abstract Task<ResourceType> ExistsAsyncInternal(StorageKey location);
        #endregion
    }
}
