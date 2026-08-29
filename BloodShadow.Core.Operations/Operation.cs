using BloodShadow.Core.Logger;
using R3;
using System.Runtime.CompilerServices;

namespace BloodShadow.Core.Operations
{
    public abstract class Operation : IDisposable, ICloneable
    {
        public abstract ReadOnlyReactiveProperty<float> Progress { get; }
        public abstract ReactiveProperty<bool> AllowLevelActivation { get; }
        public abstract ReadOnlyReactiveProperty<bool> IsDone { get; }
        public abstract ReactiveProperty<int> Priority { get; }
        public abstract ReadOnlyReactiveProperty<string> Description { get; }
        protected OperationAwaiter? _awaiter;

        protected Guid _operationGUID { private set; get; }
        protected LoggerLabel _label { private set; get; }

        public event Action OnCompleted
        {
            add => OnCompletedAction += value;
            remove => OnCompleted -= value;
        }
        protected Action? OnCompletedAction;

        public Operation()
        {
            _operationGUID = Guid.NewGuid();
            _label = new($"[{GetType().Name} ({_operationGUID})]");
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        protected abstract void DisposeInternal();
        public abstract object Clone();
        public virtual void Wait() { while (!IsDone.CurrentValue) { } }
        public virtual void AddCompleted(Action action) { OnCompleted += action; }
        public virtual Operation Start() { return this; }
        public virtual OperationAwaiter GetAwaiter()
        {
            _awaiter ??= new OperationAwaiter(this);
            return _awaiter;
        }

        public class OperationAwaiter : INotifyCompletion, IDisposable
        {
            protected virtual Operation Operation { get; set; }
            public OperationAwaiter(Operation operation)
            {
                Operation = operation;
                Operation.Start();
            }
            public virtual bool IsCompleted => Operation?.IsDone.CurrentValue ?? true;
            public virtual void OnCompleted(Action continuation)
            {
                if (IsCompleted) { continuation?.Invoke(); }
                else { Operation?.AddCompleted(continuation); }
            }
            public virtual void GetResult() { Operation.Wait(); }
            public virtual void Dispose()
            {
                GC.SuppressFinalize(this);
                try { Operation?.Dispose(); } catch { }
            }
        }

        public static implicit operator Task(Operation operation) => Task.Factory.StartNew(() => { operation.Wait(); });
    }
}
