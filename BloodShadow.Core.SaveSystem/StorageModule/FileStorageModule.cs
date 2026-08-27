using R3;

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
        protected override IEnumerable<string> EnumerateCollectionInternal(StorageKey location) => Directory.EnumerateFileSystemEntries(Path.Combine(location.Path));
        protected override IAsyncEnumerable<string> EnumerateCollectionAsyncInternal(StorageKey location) => Directory.EnumerateFileSystemEntries(Path.Combine(location.Path)).ToAsyncEnumerable();
        protected override StorageWatcher CreateWatcherInternal(StorageKey location) => new FileStorageWatcher(Path.Combine(location.Path));
    }
}
