namespace BloodShadow.Core.Operations
{
    using R3;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class OperationCollection : Operation
    {
        public override bool AllowSceneActivation
        {
            get => _operations.Where((op) => { return op != null; }).All((op) => { return op.AllowSceneActivation; });
            set => _operations.Where((op) => { return op != null; }).ToList().ForEach((op) => { op.AllowSceneActivation = value; });
        }
        public override bool IsDone => _operations.Where((op) => { return op != null; }).All((op) => { return op.IsDone; });
        public override int Priority
        {
            get => (int)_operations.Where((op) => { return op != null; }).Average((op) => { return op.Priority; });
            set => _operations.Where((op) => { return op != null; }).ToList().ForEach((op) => { op.Priority = value; });
        }
        public override float Progress => _operations.Where((op) => { return op != null; }).Average((op) => { return op.Progress; });
        public IEnumerable<Operation> Operations => _operations;

        private List<Operation> _operations;
        private readonly OperationAwaiter _awaiter;
        private readonly CancellationTokenSource _tokenSource;
        private bool _needUpdate;

        public OperationCollection()
        {
            _operations = new List<Operation>();
            _awaiter = new OperationAwaiter(this);
            _tokenSource = new CancellationTokenSource();
            Task.Run(Await, _tokenSource.Token);
        }

        private async void Await()
        {
            while (true)
            {
                try
                {
                    if (_needUpdate)
                    {
                        foreach (Operation operation in _operations) { await operation; }
                        OnCompletedAction?.Invoke();
                        _needUpdate = false;
                    }
                    await Task.Yield();
                }
                catch { }
            }
        }

        public OperationCollection(Operation operation) : this() { Add(operation); }
        public OperationCollection(IEnumerable<Operation> operations) : this() { Add(operations); }
        public OperationCollection(params Operation[] operations) : this() { Add(operations); }
        public OperationCollection(Operation operation, IEnumerable<Operation> operations) : this(operations) { Add(operation); }

        public OperationCollection Merge(OperationCollection operation) { return new OperationCollection(_operations.Concat(operation._operations)); }
        public OperationCollection Merge(IEnumerable<OperationCollection> operations)
        {
            OperationCollection result = new OperationCollection() { _operations = _operations };
            foreach (OperationCollection operation in operations) { result = result.Merge(operation); }
            return result;
        }

        public void Add(Operation operation)
        {
            if (operation == null) { return; }
            _operations.Add(operation);
            _needUpdate = true;
        }
        public void Add(IEnumerable<Operation> operations)
        {
            if (operations == null) { return; }
            _operations.AddRange(operations);
            _needUpdate = true;
        }

        public override void Dispose()
        {
            foreach (Operation operation in _operations) { if (operation is IDisposable disposable) { disposable?.Dispose(); } }
            _tokenSource.Cancel();
        }
        public override OperationAwaiter GetAwaiter() => _awaiter;
        public override object Clone() => new OperationCollection(_operations);
        public override void Wait() { Task.WaitAll(_operations.ConvertAll<Task>(input => input).ToArray()); }
    }

}
