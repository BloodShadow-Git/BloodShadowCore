using System.Diagnostics;
using BloodShadow.Core.Extensions;
using BloodShadow.Core.Logger;

namespace BloodShadow.Core.SaveSystem.StorageModule
{
    public abstract class StorageModule
    {
        protected LoggerLabel Logger;
        public StorageModule() { Logger = new(GetType().Name); }
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
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while writing at {0}.", new StackTrace(1), exception, location); }
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
            bool res = false;
            try { res = await WriteAsyncInternal(location, content); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while writing at {0}.", new StackTrace(1), exception, location); }
            finally { if (!res) { Logger.WriteLine(MessageChanel.WARN, "Failed to write to {0}", null, null, location); } }
            return res;
        }
        protected abstract Task<bool> WriteAsyncInternal(StorageKey location, byte[] content);
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
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while reading from {0}.", new StackTrace(1), exception, location); }
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
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while reading at {0}.", new StackTrace(1), exception); }
            finally { if (!result.Valid()) { Logger.WriteLine(MessageChanel.WARN, "Empty file content. If it normal you can ignore this warning", null, null); } }
            return result;
        }
        protected abstract Task<byte[]> ReadAsyncInternal(StorageKey location);
        public bool VerifyCollection(StorageKey collectionLocation, bool createIfNotExists)
        {
            Logger.WriteLine(MessageChanel.INFO, "Verifying collection at {0}", null, null, collectionLocation);
            bool res = false;
            try { res = VerifyCollectionInternal(collectionLocation, createIfNotExists); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while verifying collection at {0}.", new StackTrace(1), exception, collectionLocation); }
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
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while verifying collection at {0}.", new StackTrace(1), exception, collectionLocation); }
            finally { if (createIfNotExists && !res) { Logger.WriteLine(MessageChanel.WARN, "Collection at {0} cannot be fixed", null, null, collectionLocation); } }
            return res;

        }
        public Task<bool> VerifyCollectionAsync(StorageKey collectionLocation) => VerifyCollectionAsync(collectionLocation, false);
        protected abstract Task<bool> VerifyCollectionAsyncInternal(StorageKey collectionLocation, bool createIfNotExists);
        public bool VerifyResource(StorageKey resourceLocation, bool createIfNotExists)
        {
            Logger.WriteLine(MessageChanel.INFO, "Verifying resource at {0}", null, null, resourceLocation);
            bool res = false;
            try { res = VerifyResourceInternal(resourceLocation, createIfNotExists); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while verifying resource at {0}.", new StackTrace(1), exception, resourceLocation); }
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
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while verifying resource at {0}.", new StackTrace(1), exception, resourceLocation); }
            finally { if (createIfNotExists && !res) { Logger.WriteLine(MessageChanel.WARN, "Resource at {0} cannot be fixed", null, null, resourceLocation); } }
            return res;
        }
        public Task<bool> VerifyResourceAsync(StorageKey resourceLocation) => VerifyResourceAsync(resourceLocation, false);
        protected abstract Task<bool> VerifyResourceAsyncInternal(StorageKey resourceLocation, bool createIfNotExists);
        public bool IsResource(StorageKey location)
        {
            Logger.WriteLine(MessageChanel.INFO, "Checking resource at {0}", null, null, location);
            try { return IsResourceInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while checking resource", new StackTrace(1), exception); }
            return false;
        }
        protected abstract bool IsResourceInternal(StorageKey location);
        public async Task<bool> IsResourceAsync(StorageKey location)
        {
            Logger.WriteLine(MessageChanel.INFO, "Checking resource at {0}", null, null, location);
            try { return await IsResourceAsyncInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while checking resource", new StackTrace(1), exception); }
            return false;
        }
        protected abstract Task<bool> IsResourceAsyncInternal(StorageKey location);
        public bool IsCollection(StorageKey location)
        {
            Logger.WriteLine(MessageChanel.INFO, "Checking collection at {0}", null, null, location);
            try { return IsCollectionInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while checking collection", new StackTrace(1), exception); }
            return false;
        }
        protected abstract bool IsCollectionInternal(StorageKey location);
        public async Task<bool> IsCollectionAsync(StorageKey location)
        {
            Logger.WriteLine(MessageChanel.INFO, "Checking collection at {0}", null, null, location);
            try { return await IsCollectionAsyncInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while checking collection", new StackTrace(1), exception); }
            return false;
        }
        protected abstract Task<bool> IsCollectionAsyncInternal(StorageKey location);
        public IEnumerable<string> EnumerateCollection(StorageKey location)
        {
            Logger.WriteLine(MessageChanel.INFO, "Enumerating collection at {0}", null, null, location);
            if (!IsCollection(location))
            {
                Logger.WriteLine(MessageChanel.ERROR, "Location is not collection. Location: {0}", new StackTrace(1), new ArgumentException("Location is not collection", nameof(location)), location);
                return [];
            }
            try { return EnumerateCollectionInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while enumerating {0}.", new StackTrace(1), exception, location); }
            return [];
        }
        protected abstract IEnumerable<string> EnumerateCollectionInternal(StorageKey location);
        public IAsyncEnumerable<string> EnumerateCollectionAsync(StorageKey location)
        {
            Logger.WriteLine(MessageChanel.INFO, "Enumerating collection at {location}", null, null, location);
            if (!IsCollection(location))
            {
                Logger.WriteLine(MessageChanel.ERROR, "Location is not collection. Location: {0}", new StackTrace(1), new ArgumentException("Location is not collection", nameof(location)), location);
                return Array.Empty<string>().ToAsyncEnumerable();
            }
            try { return EnumerateCollectionAsyncInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while enumerating {0}.", new StackTrace(1), exception, location); }
            return Array.Empty<string>().ToAsyncEnumerable();
        }
        protected abstract IAsyncEnumerable<string> EnumerateCollectionAsyncInternal(StorageKey location);
        public StorageWatcher CreateWatcher(StorageKey location)
        {
            if (!IsCollection(location))
            {
                Logger.WriteLine(MessageChanel.ERROR, "Location is not collection. Location: {0}", new StackTrace(1), new ArgumentException("Location is not collection", nameof(location)), location);
                return null!;
            }
            StorageWatcher sw = null!;
            try { sw = CreateWatcherInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Exception while creating watcher for {0}.", new StackTrace(1), exception, location); }
            finally { if (sw == null) { Logger.WriteLine(MessageChanel.WARN, "Watcher was created but result is null", null, null); } }
            return sw;
        }
        protected abstract StorageWatcher CreateWatcherInternal(StorageKey location);
    }
}
