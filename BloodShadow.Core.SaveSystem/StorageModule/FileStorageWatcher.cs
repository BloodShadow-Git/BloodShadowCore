namespace BloodShadow.Core.SaveSystem.StorageModule
{
    public class FileStorageWatcher : StorageWatcher
    {
        private FileSystemWatcher _fsw;

        public FileStorageWatcher(string location) : base()
        {
            _fsw = new(location, "*");
            _fsw.Created += (obj, e) => { OnCreatedInternal.OnNext(e.FullPath); };
            _fsw.Deleted += (obj, e) => { OnDeletedInternal.OnNext(e.FullPath); };
            _fsw.Changed += (obj, e) => { OnChangedInternal.OnNext(e.FullPath); };
            _fsw.Renamed += (obj, e) => { OnRenamedInternal.OnNext((e.OldFullPath, e.FullPath)); };
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            _fsw.Dispose();
        }
    }
}
