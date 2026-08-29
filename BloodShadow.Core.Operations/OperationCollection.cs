using ObservableCollections;
using R3;

namespace BloodShadow.Core.Operations
{
    public class OperationCollection : Operation
    {
        public override ReadOnlyReactiveProperty<float> Progress => _progress;
        public override ReactiveProperty<bool> AllowLevelActivation => _allowLevelActivation;
        public override ReadOnlyReactiveProperty<bool> IsDone => _isDone;
        public override ReactiveProperty<int> Priority => _priority;
        public override ReadOnlyReactiveProperty<string> Description => _description;

        private readonly ReactiveProperty<float> _progress;
        private readonly ReactiveProperty<bool> _allowLevelActivation;
        private readonly ReactiveProperty<bool> _isDone;
        private readonly ReactiveProperty<int> _priority;
        private readonly ReactiveProperty<string> _description;

        public IObservableCollection<Operation> Operations => _operations;

        private readonly ObservableList<Operation> _operations;
        private readonly Dictionary<Operation, IDisposable> _disposeables;
        private Task? _waitTask;
        private CancellationTokenSource? _waitTaskSource;

        public OperationCollection() : base()
        {
            _operations = [];
            _disposeables = [];
            _awaiter = new(this);

            _progress = new();
            _allowLevelActivation = new();
            _isDone = new();
            _priority = new();
            _description = new();

            _allowLevelActivation.Subscribe(_ => _operations.Where((op) =>
            { return op != null; }).ToList().ForEach((op) => { op.AllowLevelActivation.Value = AllowLevelActivation.Value; }));

            _priority.Subscribe(_ => _operations.Where((op) => { return op != null; }).ToList().ForEach((op) => { op.Priority.Value = _priority.Value; }));

            _operations.ObserveAdd().Subscribe(operation =>
            {
                if (_disposeables.ContainsKey(operation.Value)) { return; }
                CompositeDisposable disposables =
                [
                    operation.Value.Progress.Subscribe(_ => UpdateOperation()),
                    operation.Value.AllowLevelActivation.Subscribe(_ => UpdateOperation()),
                    operation.Value.IsDone.Subscribe(_ => UpdateOperation()),
                    operation.Value.Priority.Subscribe(_ => UpdateOperation())
                ];
                _disposeables.Add(operation.Value, disposables);
                operation.Value.Start();
                UpdateOperation();
            });
            _operations.ObserveRemove().Subscribe(operation =>
            {
                if (_disposeables.TryGetValue(operation.Value, out IDisposable? disposable))
                {
                    disposable?.Dispose();
                    _disposeables.Remove(operation.Value);
                    UpdateOperation();
                }
            });
        }

        private void UpdateOperation()
        {
            _progress.Value = _operations.Where((op) => { return op != null; }).Average((op) => { return op.Progress.CurrentValue; });
            _allowLevelActivation.Value = _operations.Where((op) => { return op != null; }).All((op) => { return op.AllowLevelActivation.Value; });
            _isDone.Value = _operations.Where((op) => { return op != null; }).All((op) => { return op.IsDone.CurrentValue; });
            _priority.Value = (int)_operations.Where((op) => { return op != null; }).Average((op) => { return op.Priority.Value; });
            _description.Value = $"{_operations.Count(input => input.IsDone.CurrentValue)}/{_operations.Count}";
        }

        public OperationCollection(Operation operation) : this() { Add(operation); }
        public OperationCollection(IEnumerable<Operation> operations) : this() { Add(operations); }
        public OperationCollection(params Operation[] operations) : this() { Add(operations); }
        public OperationCollection(Operation operation, IEnumerable<Operation> operations) : this(operations.ToArray()) { Add(operation); }

        public OperationCollection Merge(OperationCollection operation) { return new(_operations.Concat(operation._operations)); }
        public OperationCollection Merge(IEnumerable<OperationCollection> operations)
        {
            OperationCollection result = new(_operations);
            foreach (OperationCollection operation in operations) { result = result.Merge(operation); }
            return result;
        }

        public async void UpdateWait()
        {
            _waitTaskSource?.Cancel();
            if (_waitTask != null) { await _waitTask; }
            _waitTaskSource?.Dispose();

            _waitTaskSource = new();
            _waitTask = Await();
        }

        private async Task Await()
        {
            try
            {
                List<Operation> ops = [.. _operations.Where((op) => { return op != null; })];
                foreach (Operation operation in ops)
                {
                    if (!_waitTaskSource?.IsCancellationRequested ?? false) { await operation; }
                    else { break; }
                }
                _isDone.Value = true;
                OnCompletedAction?.Invoke();
                return;
            }
            catch { }
        }

        public void Add(Operation operation)
        {
            if (operation == null) { return; }
            _operations.Add(operation);
        }

        public void Add(IEnumerable<Operation> operations) { foreach (Operation operation in operations) { Add(operation); } }

        protected override async void DisposeInternal()
        {
            foreach (Operation operation in _operations) { operation?.Dispose(); }
            _waitTaskSource?.Cancel();
            if (_waitTask != null) { await _waitTask; }
            _waitTask?.Dispose();
            _waitTaskSource?.Dispose();
        }
        public override OperationAwaiter GetAwaiter()
        {
            UpdateWait();
            return base.GetAwaiter();
        }
        public override object Clone() => new OperationCollection(_operations);
    }
}
