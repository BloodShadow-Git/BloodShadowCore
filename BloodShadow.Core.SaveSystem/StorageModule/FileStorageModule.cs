using R3;

namespace BloodShadow.Core.SaveSystem.StorageModule
{
    public class FileStorageModule : StorageModule
    {
        protected override bool WriteInternal(string path, byte[] content)
        {
            File.WriteAllBytes(path, content);
            return true;
        }
        protected override byte[] ReadInternal(string path) => File.ReadAllBytes(path);

        protected override async Task<bool> WriteAsyncInternal(string path, byte[] content)
        {
            await File.WriteAllBytesAsync(path, content);
            return true;
        }
        protected override Task<byte[]> ReadAsyncInternal(string path) => File.ReadAllBytesAsync(path);

        protected string BuildAndValidPath(string path)
        {
            path = Path.GetFullPath(path);
            Directory.CreateDirectory(path);
            File.Create(path).Dispose();
            return path;
        }

        protected override bool VerifyCollectionInternal(string collectionLocation, bool createIfNotExists)
        {
            if (collectionLocation != null && !Directory.Exists(collectionLocation)) { Directory.CreateDirectory(collectionLocation); }
            return true;
        }

        protected override Task<bool> VerifyCollectionAsyncInternal(string collectionLocation, bool createIfNotExists) => Task.Run(() => VerifyCollectionInternal(collectionLocation, createIfNotExists));

        protected override bool VerifyResourceInternal(string? filePath, bool fix)
        {
            if (filePath != null && !File.Exists(filePath))
            {
                VerifyCollection(Path.GetDirectoryName(filePath)!);
                File.Create(filePath).Dispose();
            }
            return true;
        }
        protected override Task<bool> VerifyResourceAsyncInternal(string collectionLocation, bool createIfNotExists) => Task.Run(() => VerifyResourceInternal(collectionLocation, createIfNotExists));

        protected override bool IsResourceInternal(string location) => File.Exists(location);
        protected override Task<bool> IsResourceAsyncInternal(string location) => Task.Run(() => File.Exists(location));
        protected override bool IsCollectionInternal(string location) => Directory.Exists(location);
        protected override Task<bool> IsCollectionAsyncInternal(string location) => Task.Run(() => Directory.Exists(location));
        protected override IEnumerable<string> EnumerateCollectionInternal(string location) => Directory.EnumerateFileSystemEntries(location);
        protected override IAsyncEnumerable<string> EnumerateCollectionAsyncInternal(string location) => Directory.EnumerateFileSystemEntries(location).ToAsyncEnumerable();
        protected override StorageWatcher CreateWatcherInternal(string location) => new FileStorageWatcher(location);
    }
}
