namespace BloodShadowFramework.Operations
{
    using R3;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class OperationCollection : Operation
    {
        public override bool AllowSceneActivation
        {
            get => Operations.Where((op) => { return op != null; }).All((op) => { return op.AllowSceneActivation; });
            set => Operations.Where((op) => { return op != null; }).ToList().ForEach((op) => { op.AllowSceneActivation = value; });
        }
        public override bool IsDone => Operations.Where((op) => { return op != null; }).All((op) => { return op.IsDone; });
        public override int Priority
        {
            get => (int)Operations.Where((op) => { return op != null; }).Average((op) => { return op.Priority; });
            set => Operations.Where((op) => { return op != null; }).ToList().ForEach((op) => { op.Priority = value; });
        }
        public override float Progress => Operations.Where((op) => { return op != null; }).Average((op) => { return op.Progress; });
        new public event Action Completed;

        public readonly List<Operation> Operations;
        private readonly OperationAwaiter _awaiter;
        private readonly CancellationTokenSource _tokenSource;
        private bool _needUpdate;

        public OperationCollection()
        {
            Operations = [];
            _awaiter = new(this);
            _tokenSource = new();
            Task.Run(Await, _tokenSource.Token);
        }

        private void Await()
        {
            while (true)
            {
                try
                {
                    if (IsDone || Progress >= 1f && _needUpdate)
                    {
                        _needUpdate = false;
                        IsDone = true;
                        Completed?.Invoke();
                    }
                }
                catch { }
            }
        }

        public OperationCollection(Operation operation) : this() { Add(operation); }
        public OperationCollection(IEnumerable<Operation> operations) : this() { Add(operations); }
        public OperationCollection(Operation operation, IEnumerable<Operation> operations) : this(operations) { Add(operation); }

        public OperationCollection Merge(OperationCollection operation) { return new([.. Operations, .. operation.Operations]); }
        public OperationCollection Merge(IEnumerable<OperationCollection> operations)
        {
            OperationCollection result = this;
            foreach (OperationCollection operation in operations) { result = result.Merge(operation); }
            return result;
        }

        public void Add(Operation operation)
        {
            if (operation == null) { return; }
            Operations.Add(operation);
            _needUpdate = true;
        }
        public void Add(IEnumerable<Operation> operations)
        {
            if (operations == null) { return; }
            Operations.AddRange(operations);
            _needUpdate = true;
        }

        //public void Dispose()
        //{
        //    foreach (Operation operation in Operations) { if (operation is IDisposable disposable) { disposable?.Dispose(); } }
        //    _tokenSource.Cancel();
        //}
        public override OperationAwaiter GetAwaiter() => _awaiter;
        public override object Clone() => new OperationCollection(Operations);
        public override void Wait() { Task.WaitAll([.. Operations.ConvertAll<Task>(input => input)]); }
    }

}
