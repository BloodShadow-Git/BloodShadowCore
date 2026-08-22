using R3;

namespace BloodShadow.Core.SaveSystem.StorageModule
{
    public abstract class StorageWatcher : IDisposable
    {
        public virtual Observable<string> OnCreated => OnCreatedInternal;
        protected Subject<string> OnCreatedInternal;
        public virtual Observable<string> OnDeleted => OnDeletedInternal;
        protected Subject<string> OnDeletedInternal;
        public virtual Observable<string> OnChanged => OnChangedInternal;
        protected Subject<string> OnChangedInternal;
        public virtual Observable<(string oldName, string newName)> OnRenamed => OnRenamedInternal;
        protected Subject<(string oldName, string newName)> OnRenamedInternal;
        public abstract void Dispose();
        public StorageWatcher()
        {
            OnCreatedInternal = new();
            OnDeletedInternal = new();
            OnChangedInternal = new();
            OnRenamedInternal = new();
        }
    }
}
