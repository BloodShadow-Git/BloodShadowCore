namespace BloodShadow.Core.SaveSystem.StorageModule
{
    public class FileStorageModule : StorageModule
    {
        protected override bool WriteInternal(StorageKey location, byte[] content)
        {
            File.WriteAllBytes(Path.Combine(location.Path), content);
            return true;
        }
        protected override byte[] ReadInternal(StorageKey location) => File.ReadAllBytes(Path.Combine(location.Path));

        protected override async Task<bool> WriteAsyncInternal(StorageKey location, byte[] content)
        {
            await File.WriteAllBytesAsync(Path.Combine(location.Path), content);
            return true;
        }
        protected override Task<byte[]> ReadAsyncInternal(StorageKey location) => File.ReadAllBytesAsync(Path.Combine(location.Path));

        protected override bool VerifyCollectionInternal(StorageKey location, bool fix)
        {
            string path = Path.Combine(location.Path);
            if (!Directory.Exists(path))
            {
                if (!fix) { return false; }
                Directory.CreateDirectory(path);
            }
            return true;
        }

        protected override Task<bool> VerifyCollectionAsyncInternal(StorageKey collectionLocation, bool createIfNotExists) => Task.Run(() => VerifyCollectionInternal(collectionLocation, createIfNotExists));

        protected override bool VerifyResourceInternal(StorageKey location, bool fix)
        {
            string path = Path.Combine(location.Path);
            if (!File.Exists(path))
            {
                if (!fix) { return false; }
                VerifyCollectionInternal(new StorageKey(Path.GetDirectoryName(path)!), true);
                File.Create(path).Dispose();
            }
            return true;
        }
        protected override Task<bool> VerifyResourceAsyncInternal(StorageKey collectionLocation, bool createIfNotExists) => Task.Run(() => VerifyResourceInternal(collectionLocation, createIfNotExists));

        protected override bool IsResourceInternal(StorageKey location) => File.Exists(Path.Combine(location.Path));
        protected override Task<bool> IsResourceAsyncInternal(StorageKey location) => Task.Run(() => File.Exists(Path.Combine(location.Path)));

        protected override bool IsCollectionInternal(StorageKey location) => Directory.Exists(Path.Combine(location.Path));
        protected override Task<bool> IsCollectionAsyncInternal(StorageKey location) => Task.Run(() => Directory.Exists(Path.Combine(location.Path)));

        protected override IEnumerable<Resource> EnumerateCollectionInternal(StorageKey location)
        {
            string[] fsEntries = [.. Directory.EnumerateFileSystemEntries(Path.Combine(location.Path))];
            Resource[] result = new Resource[fsEntries.Length];
            for (int i = 0; i < fsEntries.Length; i++)
            {
                ResourceType resourceType = ExistsInternal(new(fsEntries[i]));
                result[i] = new(fsEntries[i], resourceType);
            }
            return result;
        }
        protected override async IAsyncEnumerable<Resource> EnumerateCollectionAsyncInternal(StorageKey location)
        {
            string[] fsEntries = [.. Directory.EnumerateFileSystemEntries(Path.Combine(location.Path))];
            for (int i = 0; i < fsEntries.Length; i++)
            {
                ResourceType resourceType = await ExistsAsyncInternal(new(fsEntries[i]));
                yield return new(fsEntries[i], resourceType);
            }
        }

        protected override StorageWatcher CreateWatcherInternal(StorageKey location) => new FileStorageWatcher(Path.Combine(location.Path));

        protected override bool CreateResourceInternal(StorageKey location)
        {
            File.Create(Path.Combine(location.Path)).Close();
            return true;
        }
        protected override async Task<bool> CreateResourceAsyncInternal(StorageKey location)
        {
            File.Create(Path.Combine(location.Path)).Close();
            return true;
        }

        protected override bool RemoveResourceInternal(StorageKey location)
        {
            File.Delete(Path.Combine(location.Path));
            return true;
        }
        protected override async Task<bool> RemoveResourceAsyncInternal(StorageKey location)
        {
            File.Delete(Path.Combine(location.Path));
            return true;
        }

        protected override bool CreateCollectionInternal(StorageKey location)
        {
            Directory.CreateDirectory(Path.Combine(location.Path));
            return true;
        }
        protected override async Task<bool> CreateCollectionAsyncInternal(StorageKey location)
        {
            Directory.CreateDirectory(Path.Combine(location.Path));
            return true;
        }

        protected override bool RemoveCollectionInternal(StorageKey location)
        {
            Directory.Delete(Path.Combine(location.Path));
            return true;
        }
        protected override async Task<bool> RemoveCollectionAsyncInternal(StorageKey location)
        {
            Directory.Delete(Path.Combine(location.Path));
            return true;
        }

        protected override ResourceType ExistsInternal(StorageKey location)
        {
            string path = Path.Combine(location.Path);
            ResourceType resourceType = ResourceType.Unknown;
            if (File.Exists(path)) { resourceType |= ResourceType.Resource; }
            if (Directory.Exists(path)) { resourceType |= ResourceType.Collection; }
            return resourceType;
        }
        protected override async Task<ResourceType> ExistsAsyncInternal(StorageKey location)
        {
            string path = Path.Combine(location.Path);
            ResourceType resourceType = ResourceType.Unknown;
            if (File.Exists(path)) { resourceType |= ResourceType.Resource; }
            if (Directory.Exists(path)) { resourceType |= ResourceType.Collection; }
            return resourceType;
        }
    }
}
