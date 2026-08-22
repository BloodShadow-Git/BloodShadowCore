using BloodShadow.Core.Extensions;
using BloodShadow.Core.Logger;

namespace BloodShadow.Core.SaveSystem.StorageModule
{
    public abstract class StorageModule
    {
        protected LoggerLabel Logger;
        public StorageModule() { Logger = new(GetType().Name); }
        public bool Write(string location, byte[] content)
        {
            if (!location.Valid())
            {
                Logger.WriteLineWarning($"Empty location. Location: {location}");
                return false;
            }
            Logger.WriteLineInfo($"Writing to {location}");
            if (content.Valid()) { Logger.WriteLineWarning("Empty content"); }
            bool res = false;
            try { res = WriteInternal(location, content); }
            catch (Exception exception) { Logger.WriteLineException($"Exception while writing at {location}.", exception); }
            finally { if (!res) { Logger.WriteLineWarning($"Failed to write to {location}"); } }
            return res;
        }
        protected abstract bool WriteInternal(string location, byte[] content);
        public async Task<bool> WriteAsync(string location, byte[] content)
        {
            if (!location.Valid())
            {
                Logger.WriteLineWarning($"Empty location. Location: {location}");
                return false;
            }
            Logger.WriteLineInfo($"Writing to {location}");
            if (content.Valid()) { Logger.WriteLineWarning("Empty content"); }
            bool res = false;
            try { res = await WriteAsyncInternal(location, content); }
            catch (Exception exception) { Logger.WriteLineException($"Exception while writing at {location}.", exception); }
            finally { if (!res) { Logger.WriteLineWarning($"Failed to write to {location}"); } }
            return res;
        }
        protected abstract Task<bool> WriteAsyncInternal(string location, byte[] content);
        public byte[] Read(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                Logger.WriteLineWarning($"Empty location. Location: {location}");
                return [];
            }
            Logger.WriteLineInfo($"Reading from {location}");
            byte[] result = [];
            try { result = ReadInternal(location); }
            catch (Exception exception) { Logger.WriteLineException($"Exception while reading from {location}.", exception); }
            finally { if (!result.Valid()) { Logger.WriteLineWarning($"Empty file content. If it normal you can ignore this warning"); } }
            return result;
        }
        protected abstract byte[] ReadInternal(string location);
        public async Task<byte[]> ReadAsync(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                Logger.WriteLineWarning($"Empty location. Location: {location}");
                return [];
            }
            Logger.WriteLineInfo($"Reading from {location}");
            byte[] result = [];
            try { result = await ReadAsyncInternal(location); }
            catch (Exception exception) { Logger.WriteLineException($"Exception while reading at {location}.", exception); }
            finally { if (!result.Valid()) { Logger.WriteLineWarning($"Empty file content. If it normal you can ignore this warning"); } }
            return result;
        }
        protected abstract Task<byte[]> ReadAsyncInternal(string location);
        public bool VerifyCollection(string collectionLocation, bool createIfNotExists)
        {
            if (!collectionLocation.Valid())
            {
                Logger.WriteLineWarning($"Empty collection location. Location: {collectionLocation}");
                return false;
            }
            Logger.WriteLineInfo($"Verifying collection at {collectionLocation}");
            bool res = false;
            try { res = VerifyCollectionInternal(collectionLocation, createIfNotExists); }
            catch (Exception exception) { Logger.WriteLineException($"Exception while verifying collection at {collectionLocation}.", exception); }
            finally { if (createIfNotExists && !res) { Logger.WriteLineWarning($"Collection at {collectionLocation} cannot be fixed"); } }
            return res;

        }
        public bool VerifyCollection(string collectionLocation) => VerifyCollection(collectionLocation, false);
        protected abstract bool VerifyCollectionInternal(string collectionLocation, bool createIfNotExists);
        public async Task<bool> VerifyCollectionAsync(string collectionLocation, bool createIfNotExists)
        {
            if (!collectionLocation.Valid())
            {
                Logger.WriteLineWarning($"Empty collection location. Location: {collectionLocation}");
                return false;
            }
            Logger.WriteLineInfo($"Verifying collection at {collectionLocation}");
            bool res = false;
            try { res = await VerifyCollectionAsyncInternal(collectionLocation, createIfNotExists); }
            catch (Exception exception) { Logger.WriteLineException($"Exception while verifying collection at {collectionLocation}.", exception); }
            finally { if (createIfNotExists && !res) { Logger.WriteLineWarning($"Collection at {collectionLocation} cannot be fixed"); } }
            return res;

        }
        public Task<bool> VerifyCollectionAsync(string collectionLocation) => VerifyCollectionAsync(collectionLocation, false);
        protected abstract Task<bool> VerifyCollectionAsyncInternal(string collectionLocation, bool createIfNotExists);
        public bool VerifyResource(string resourceLocation, bool createIfNotExists)
        {
            if (!resourceLocation.Valid())
            {
                Logger.WriteLineWarning($"Empty resource location. Location: {resourceLocation}");
                return false;
            }
            Logger.WriteLineInfo($"Verifying resource at {resourceLocation}");
            bool res = false;
            try { res = VerifyResourceInternal(resourceLocation, createIfNotExists); }
            catch (Exception exception) { Logger.WriteLineException($"Exception while verifying resource at {resourceLocation}.", exception); }
            finally { if (createIfNotExists && !res) { Logger.WriteLineWarning($"Resource at {resourceLocation} cannot be fixed"); } }
            return res;
        }
        public bool VerifyResource(string resourceLocation) => VerifyResource(resourceLocation, false);
        protected abstract bool VerifyResourceInternal(string resourceLocation, bool createIfNotExists);
        public async Task<bool> VerifyResourceAsync(string resourceLocation, bool createIfNotExists)
        {
            if (!resourceLocation.Valid())
            {
                Logger.WriteLineWarning($"Empty resource location. Location: {resourceLocation}");
                return false;
            }
            Logger.WriteLineInfo($"Verifying resource at {resourceLocation}");
            bool res = false;
            try { res = await VerifyResourceAsyncInternal(resourceLocation, createIfNotExists); }
            catch (Exception exception) { Logger.WriteLineException($"Exception while verifying resource at {resourceLocation}.", exception); }
            finally { if (createIfNotExists && !res) { Logger.WriteLineWarning($"Resource at {resourceLocation} cannot be fixed"); } }
            return res;
        }
        public Task<bool> VerifyResourceAsync(string resourceLocation) => VerifyResourceAsync(resourceLocation, false);
        protected abstract Task<bool> VerifyResourceAsyncInternal(string resourceLocation, bool createIfNotExists);
        public bool IsResource(string location)
        {
            Logger.WriteLineInfo($"Checking resource at {location}");
            try { return IsResourceInternal(location); }
            catch (Exception exception) { Logger.WriteLineException($"Exception while checking resource", exception); }
            return false;
        }
        protected abstract bool IsResourceInternal(string location);
        public async Task<bool> IsResourceAsync(string location)
        {
            Logger.WriteLineInfo($"Checking resource at {location}");
            try { return await IsResourceAsyncInternal(location); }
            catch (Exception exception) { Logger.WriteLineException($"Exception while checking resource", exception); }
            return false;
        }
        protected abstract Task<bool> IsResourceAsyncInternal(string location);
        public bool IsCollection(string location)
        {
            Logger.WriteLineInfo($"Checking collection at {location}");
            try { return IsCollectionInternal(location); }
            catch (Exception exception) { Logger.WriteLineException($"Exception while checking collection", exception); }
            return false;
        }
        protected abstract bool IsCollectionInternal(string location);
        public async Task<bool> IsCollectionAsync(string location)
        {
            Logger.WriteLineInfo($"Checking collection at {location}");
            try { return await IsCollectionAsyncInternal(location); }
            catch (Exception exception) { Logger.WriteLineException($"Exception while checking collection", exception); }
            return false;
        }
        protected abstract Task<bool> IsCollectionAsyncInternal(string location);
        public IEnumerable<string> EnumerateCollection(string location)
        {
            Logger.WriteLineInfo($"Enumerating collection at {location}");
            if (!location.Valid())
            {
                Logger.WriteLineException($"Invalid location. Location: {location}", new ArgumentNullException(nameof(location)));
                return [];
            }
            if (!IsCollection(location))
            {
                Logger.WriteLineException($"Location is not collection. Location: {location}", new ArgumentException(nameof(location)));
                return [];
            }
            try { return EnumerateCollectionInternal(location); }
            catch (Exception exception) { Logger.WriteLineException($"Exception while enumerating {location}.", exception); }
            return [];
        }
        protected abstract IEnumerable<string> EnumerateCollectionInternal(string location);
        public IAsyncEnumerable<string> EnumerateCollectionAsync(string location)
        {
            Logger.WriteLineInfo($"Enumerating collection at {location}");
            if (!location.Valid())
            {
                Logger.WriteLineException($"Invalid location. Location: {location}", new ArgumentNullException(nameof(location)));
                return new string[0].ToAsyncEnumerable();
            }
            if (!IsCollection(location))
            {
                Logger.WriteLineException($"Location is not collection. Location: {location}", new ArgumentException(nameof(location)));
                return new string[0].ToAsyncEnumerable();
            }
            try { return EnumerateCollectionAsyncInternal(location); }
            catch (Exception exception) { Logger.WriteLineException($"Exception while enumerating {location}.", exception); }
            return new string[0].ToAsyncEnumerable();
        }
        protected abstract IAsyncEnumerable<string> EnumerateCollectionAsyncInternal(string location);
        public StorageWatcher CreateWatcher(string location)
        {
            if (!location.Valid())
            {
                Logger.WriteLineException("Empty watcher location", new ArgumentNullException(nameof(location)));
                return null!;
            }
            if (!IsCollection(location))
            {
                Logger.WriteLineException($"Location is not collection. Location: {location}", new ArgumentException(nameof(location)));
                return null!;
            }
            StorageWatcher sw = null!;
            try { sw = CreateWatcherInternal(location); }
            catch (Exception exception) { Logger.WriteLineException($"Exception while creating watcher for {location}.", exception); }
            finally { if (sw == null) { Logger.WriteLineWarning("Watcher was created but result is null"); } }
            return sw;
        }
        protected abstract StorageWatcher CreateWatcherInternal(string location);
    }
}
