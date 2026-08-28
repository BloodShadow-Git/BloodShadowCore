using ObservableCollections;
using R3;

namespace BloodShadow.Core.Operations
{
    public class OperationQueue : Operation
    {
        public override ReadOnlyReactiveProperty<float> Progress => _progress;
        public override ReactiveProperty<bool> AllowLevelActivation => _allowLevelActivation;
        public override ReadOnlyReactiveProperty<bool> IsDone => _isDone;
        public override ReactiveProperty<int> Priority => _priority;
        public override ReadOnlyReactiveProperty<string> Description => _description;
        public ObservableQueue<Operation> Operations { get; private set; }
        public ReactiveProperty<int> OperationsCount { get; private set; }

        private readonly ReactiveProperty<float> _progress;
        private readonly ReactiveProperty<bool> _allowLevelActivation;
        private readonly ReactiveProperty<bool> _isDone;
        private readonly ReactiveProperty<int> _priority;
        private readonly ReactiveProperty<string> _description;

        private Operation? _current;
        private List<Operation> _completed;
        private bool _isRunning;

        public OperationQueue(int operationsCount) : this()
        {
            Operations = new(operationsCount);
            OperationsCount = new(operationsCount);
        }
        public OperationQueue(params Operation[] operations) : this() { Operations = new(operations.Where(op => op != null)); }

        private OperationQueue()
        {
            _isRunning = false;
            _progress = new();
            _allowLevelActivation = new();
            _isDone = new(false);
            _priority = new();
            _description = new();
            _completed = [];
            OperationsCount = new();
            Operations = new();

            _progress.Subscribe(_ => UpdateDescription());
            OperationsCount.Subscribe(_ => UpdateProgress());
        }
        public override Operation Start()
        {
            Proceed();
            return base.Start();
        }
        private async void Proceed()
        {
            if (_isRunning) { return; }
            _isRunning = true;
            _isDone.Value = false;

            OnCompletedAction = null;
            while (Operations == null) { await Task.Yield(); }
            while (Operations.Count > 0)
            {
                _current = Operations.Dequeue();
                if (_current == null) { continue; }
                _completed.Add(_current);
                CompositeDisposable cd =
                [
                    _current.Progress.Subscribe(_ => UpdateProgress()),
                    _current.Description.Subscribe(_ => UpdateDescription()),
                    _allowLevelActivation.Subscribe(_ =>
                    { foreach(Operation operation in Operations) { operation.AllowLevelActivation.Value = _allowLevelActivation.CurrentValue; } }),
                    _priority.Subscribe(_ =>
                    { foreach(Operation operation in Operations) { operation.Priority.Value = _priority.CurrentValue; } })
                ];
                UpdateDescription();
                await _current;
                cd.Dispose();
            }
            while (_completed.Count < OperationsCount.CurrentValue) { await Task.Yield(); }
            Dispose();
            _isRunning = false;
        }

        private void UpdateProgress()
        {
            if (_completed == null) { _progress.Value = 1f; return; }
            _progress.Value = _completed.Count > 0 ? (_completed.Sum(op => op.Progress.CurrentValue) / Math.Max(_completed.Count, OperationsCount.CurrentValue)) : 1f;
        }

        private void UpdateDescription()
        {
            if (Operations == null || _current == null) { return; }
            _description.Value = $"({_progress.CurrentValue:P2}) {_current.Description.CurrentValue}";
        }

        public override object Clone() => new OperationQueue(Operations.ToArray());
        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            _progress.Value = 1f;
            _isDone.Value = true;
            OnCompletedAction?.Invoke();
        }
    }
}
