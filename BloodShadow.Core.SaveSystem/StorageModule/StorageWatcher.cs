using R3;

namespace BloodShadow.Core.SaveSystem.StorageModule
{
    public abstract class StorageWatcher : IDisposable
    {
        public virtual Observable<string> OnCreated => OnCreatedInternal;
        public virtual Observable<string> OnDeleted => OnDeletedInternal;
        public virtual Observable<string> OnChanged => OnChangedInternal;
        public virtual Observable<(string oldName, string newName)> OnRenamed => OnRenamedInternal;

        public abstract string Filter { get; set; }

        protected Subject<string> OnCreatedInternal;
        protected Subject<string> OnDeletedInternal;
        protected Subject<string> OnChangedInternal;
        protected Subject<(string oldName, string newName)> OnRenamedInternal;
        protected readonly CompositeDisposable CD;

        public StorageWatcher()
        {
            OnCreatedInternal = new();
            OnDeletedInternal = new();
            OnChangedInternal = new();
            OnRenamedInternal = new();
            CD = new(OnCreatedInternal, OnDeletedInternal, OnChangedInternal, OnRenamedInternal);
        }

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
            CD.Dispose();
        }
    }
}
