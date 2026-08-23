using System.Diagnostics;
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
                Logger.WriteLine(MessageChanel.WARN, $"Empty location. Location: {location}", null, null);
                return false;
            }
            Logger.WriteLine(MessageChanel.INFO, $"Writing to {location}", null, null);
            if (content.Valid()) { Logger.WriteLine(MessageChanel.WARN, "Empty content", null, null); }
            bool res = false;
            try { res = WriteInternal(location, content); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, $"Exception while writing at {location}.", new StackTrace(1), exception); }
            finally { if (!res) { Logger.WriteLine(MessageChanel.WARN, $"Failed to write to {location}", null, null); } }
            return res;
        }
        protected abstract bool WriteInternal(string location, byte[] content);
        public async Task<bool> WriteAsync(string location, byte[] content)
        {
            if (!location.Valid())
            {
                Logger.WriteLine(MessageChanel.WARN, $"Empty location. Location: {location}", null, null);
                return false;
            }
            Logger.WriteLine(MessageChanel.INFO, $"Writing to {location}", null, null);
            if (content.Valid()) { Logger.WriteLine(MessageChanel.WARN, "Empty content", null, null); }
            bool res = false;
            try { res = await WriteAsyncInternal(location, content); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, $"Exception while writing at {location}.", new StackTrace(1), exception); }
            finally { if (!res) { Logger.WriteLine(MessageChanel.WARN, $"Failed to write to {location}", null, null); } }
            return res;
        }
        protected abstract Task<bool> WriteAsyncInternal(string location, byte[] content);
        public byte[] Read(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                Logger.WriteLine(MessageChanel.WARN, $"Empty location. Location: {location}", null, null);
                return [];
            }
            Logger.WriteLine(MessageChanel.INFO, $"Reading from {location}", null, null);
            byte[] result = [];
            try { result = ReadInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, $"Exception while reading from {location}.", new StackTrace(1), exception); }
            finally { if (!result.Valid()) { Logger.WriteLine(MessageChanel.WARN, $"Empty file content. If it normal you can ignore this warning", null, null); } }
            return result;
        }
        protected abstract byte[] ReadInternal(string location);
        public async Task<byte[]> ReadAsync(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                Logger.WriteLine(MessageChanel.WARN, $"Empty location. Location: {location}", null, null);
                return [];
            }
            Logger.WriteLine(MessageChanel.INFO, $"Reading from {location}", null, null);
            byte[] result = [];
            try { result = await ReadAsyncInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, $"Exception while reading at {location}.", new StackTrace(1), exception); }
            finally { if (!result.Valid()) { Logger.WriteLine(MessageChanel.WARN, $"Empty file content. If it normal you can ignore this warning", null, null); } }
            return result;
        }
        protected abstract Task<byte[]> ReadAsyncInternal(string location);
        public bool VerifyCollection(string collectionLocation, bool createIfNotExists)
        {
            if (!collectionLocation.Valid())
            {
                Logger.WriteLine(MessageChanel.WARN, $"Empty collection location. Location: {collectionLocation}", null, null);
                return false;
            }
            Logger.WriteLine(MessageChanel.INFO, $"Verifying collection at {collectionLocation}", null, null);
            bool res = false;
            try { res = VerifyCollectionInternal(collectionLocation, createIfNotExists); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, $"Exception while verifying collection at {collectionLocation}.", new StackTrace(1), exception); }
            finally { if (createIfNotExists && !res) { Logger.WriteLine(MessageChanel.WARN, $"Collection at {collectionLocation} cannot be fixed", null, null); } }
            return res;

        }
        public bool VerifyCollection(string collectionLocation) => VerifyCollection(collectionLocation, false);
        protected abstract bool VerifyCollectionInternal(string collectionLocation, bool createIfNotExists);
        public async Task<bool> VerifyCollectionAsync(string collectionLocation, bool createIfNotExists)
        {
            if (!collectionLocation.Valid())
            {
                Logger.WriteLine(MessageChanel.WARN, $"Empty collection location. Location: {collectionLocation}", null, null);
                return false;
            }
            Logger.WriteLine(MessageChanel.INFO, $"Verifying collection at {collectionLocation}", null, null);
            bool res = false;
            try { res = await VerifyCollectionAsyncInternal(collectionLocation, createIfNotExists); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, $"Exception while verifying collection at {collectionLocation}.", new StackTrace(1), exception); }
            finally { if (createIfNotExists && !res) { Logger.WriteLine(MessageChanel.WARN, $"Collection at {collectionLocation} cannot be fixed", null, null); } }
            return res;

        }
        public Task<bool> VerifyCollectionAsync(string collectionLocation) => VerifyCollectionAsync(collectionLocation, false);
        protected abstract Task<bool> VerifyCollectionAsyncInternal(string collectionLocation, bool createIfNotExists);
        public bool VerifyResource(string resourceLocation, bool createIfNotExists)
        {
            if (!resourceLocation.Valid())
            {
                Logger.WriteLine(MessageChanel.WARN, $"Empty resource location. Location: {resourceLocation}", null, null);
                return false;
            }
            Logger.WriteLine(MessageChanel.INFO, $"Verifying resource at {resourceLocation}", null, null);
            bool res = false;
            try { res = VerifyResourceInternal(resourceLocation, createIfNotExists); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, $"Exception while verifying resource at {resourceLocation}.", new StackTrace(1), exception); }
            finally { if (createIfNotExists && !res) { Logger.WriteLine(MessageChanel.WARN, $"Resource at {resourceLocation} cannot be fixed", null, null); } }
            return res;
        }
        public bool VerifyResource(string resourceLocation) => VerifyResource(resourceLocation, false);
        protected abstract bool VerifyResourceInternal(string resourceLocation, bool createIfNotExists);
        public async Task<bool> VerifyResourceAsync(string resourceLocation, bool createIfNotExists)
        {
            if (!resourceLocation.Valid())
            {
                Logger.WriteLine(MessageChanel.WARN, $"Empty resource location. Location: {resourceLocation}", null, null);
                return false;
            }
            Logger.WriteLine(MessageChanel.INFO, $"Verifying resource at {resourceLocation}", null, null);
            bool res = false;
            try { res = await VerifyResourceAsyncInternal(resourceLocation, createIfNotExists); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, $"Exception while verifying resource at {resourceLocation}.", new StackTrace(1), exception); }
            finally { if (createIfNotExists && !res) { Logger.WriteLine(MessageChanel.WARN, $"Resource at {resourceLocation} cannot be fixed", null, null); } }
            return res;
        }
        public Task<bool> VerifyResourceAsync(string resourceLocation) => VerifyResourceAsync(resourceLocation, false);
        protected abstract Task<bool> VerifyResourceAsyncInternal(string resourceLocation, bool createIfNotExists);
        public bool IsResource(string location)
        {
            Logger.WriteLine(MessageChanel.INFO, $"Checking resource at {location}", null, null);
            try { return IsResourceInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, $"Exception while checking resource", new StackTrace(1), exception); }
            return false;
        }
        protected abstract bool IsResourceInternal(string location);
        public async Task<bool> IsResourceAsync(string location)
        {
            Logger.WriteLine(MessageChanel.INFO, $"Checking resource at {location}", null, null);
            try { return await IsResourceAsyncInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, $"Exception while checking resource", new StackTrace(1), exception); }
            return false;
        }
        protected abstract Task<bool> IsResourceAsyncInternal(string location);
        public bool IsCollection(string location)
        {
            Logger.WriteLine(MessageChanel.INFO, $"Checking collection at {location}", null, null);
            try { return IsCollectionInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, $"Exception while checking collection", new StackTrace(1), exception); }
            return false;
        }
        protected abstract bool IsCollectionInternal(string location);
        public async Task<bool> IsCollectionAsync(string location)
        {
            Logger.WriteLine(MessageChanel.INFO, $"Checking collection at {location}", null, null);
            try { return await IsCollectionAsyncInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, $"Exception while checking collection", new StackTrace(1), exception); }
            return false;
        }
        protected abstract Task<bool> IsCollectionAsyncInternal(string location);
        public IEnumerable<string> EnumerateCollection(string location)
        {
            Logger.WriteLine(MessageChanel.INFO, $"Enumerating collection at {location}", null, null);
            if (!location.Valid())
            {
                Logger.WriteLine(MessageChanel.ERROR, $"Invalid location. Location: {location}", new StackTrace(1), new ArgumentNullException(nameof(location)));
                return [];
            }
            if (!IsCollection(location))
            {
                Logger.WriteLine(MessageChanel.ERROR, $"Location is not collection. Location: {location}", new StackTrace(1), new ArgumentException(nameof(location)));
                return [];
            }
            try { return EnumerateCollectionInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, $"Exception while enumerating {location}.", new StackTrace(1), exception); }
            return [];
        }
        protected abstract IEnumerable<string> EnumerateCollectionInternal(string location);
        public IAsyncEnumerable<string> EnumerateCollectionAsync(string location)
        {
            Logger.WriteLine(MessageChanel.INFO, $"Enumerating collection at {location}", null, null);
            if (!location.Valid())
            {
                Logger.WriteLine(MessageChanel.ERROR, $"Invalid location. Location: {location}", new StackTrace(1), new ArgumentNullException(nameof(location)));
                return new string[0].ToAsyncEnumerable();
            }
            if (!IsCollection(location))
            {
                Logger.WriteLine(MessageChanel.ERROR, $"Location is not collection. Location: {location}", new StackTrace(1), new ArgumentException(nameof(location)));
                return new string[0].ToAsyncEnumerable();
            }
            try { return EnumerateCollectionAsyncInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, $"Exception while enumerating {location}.", new StackTrace(1), exception); }
            return new string[0].ToAsyncEnumerable();
        }
        protected abstract IAsyncEnumerable<string> EnumerateCollectionAsyncInternal(string location);
        public StorageWatcher CreateWatcher(string location)
        {
            if (!location.Valid())
            {
                Logger.WriteLine(MessageChanel.ERROR, "Empty watcher location", new StackTrace(1), new ArgumentNullException(nameof(location)));
                return null!;
            }
            if (!IsCollection(location))
            {
                Logger.WriteLine(MessageChanel.ERROR, $"Location is not collection. Location: {location}", new StackTrace(1), new ArgumentException($"Location is not collection. Location: {location}", nameof(location)));
                return null!;
            }
            StorageWatcher sw = null!;
            try { sw = CreateWatcherInternal(location); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, $"Exception while creating watcher for {location}.", new StackTrace(1), exception); }
            finally { if (sw == null) { Logger.WriteLine(MessageChanel.WARN, "Watcher was created but result is null", null, null); } }
            return sw;
        }
        protected abstract StorageWatcher CreateWatcherInternal(string location);
    }
}
