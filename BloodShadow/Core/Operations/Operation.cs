using R3;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace BloodShadow.Core.Operations
{
    public class Operation : IDisposable, ICloneable
    {
        public virtual float Progress { get; protected set; }
        public virtual bool AllowSceneActivation { get; set; }
        public virtual bool IsDone { get; protected set; }
        public virtual int Priority { get; set; }
        public event Action OnCompleted
        {
            add => OnCompletedAction += value;
            remove => OnCompletedAction -= value;
        }
        protected Action? OnCompletedAction;

        public CancellationTokenSource CancellationTokenSource { get; protected set; } = new CancellationTokenSource();
        private readonly Task? _task;
        private readonly Observable<(float progress, OperationTaskProgress taskProgress)>? _subject;
        private OperationAwaiter? _awaiter;

        public Operation(Func<Task>? action, Observable<(float progress, OperationTaskProgress taskProgress)>? subject = null)
        {
            _task = action?.Invoke();
            _subject = subject;
            SetupAwaiter();
        }
        public Operation(Action? action, Observable<(float progress, OperationTaskProgress taskProgress)>? subject = null)
        {
            _task = Task.Factory.StartNew(action, CancellationTokenSource.Token);
            _subject = subject;
            SetupAwaiter();
        }
        public Operation(Task? task, Observable<(float progress, OperationTaskProgress taskProgress)>? subject = null)
        {
            _task = task;
            _subject = subject;
            SetupAwaiter();
        }
        public Operation() { }
        private void SetupAwaiter()
        {
            _awaiter = new OperationAwaiter(this);
            Progress = 0f;
            IDisposable? disposable = _subject?.Subscribe(data =>
            {
                switch (data.taskProgress)
                {
                    case OperationTaskProgress.Add: Progress += data.progress; break;
                    case OperationTaskProgress.Set: Progress = data.progress; break;
                }
                Progress = Math.Clamp(Progress, 0, 1);
            });
            Task.Run(() =>
            {
                while (!CancellationTokenSource.IsCancellationRequested && (!_task?.IsCompleted ?? true)) { IsDone = _task?.IsCompleted ?? false; }
                disposable?.Dispose();
                Progress = 1f;
                OnCompletedAction?.Invoke();
                IsDone = true;
            });
        }
        public virtual void Dispose()
        {
            _task?.Dispose();
            CancellationTokenSource.Cancel();
        }
        public virtual OperationAwaiter GetAwaiter() => _awaiter ?? new OperationAwaiter();
        public virtual object Clone() => new Operation(_task, _subject);
        public virtual void Wait() { while (!IsDone) { } }
        public virtual void AddCompleted(Action action) { OnCompletedAction += action; }


        public class OperationAwaiter : INotifyCompletion
        {
            protected virtual Operation? Operation { get; set; }
            public OperationAwaiter() { }
            public OperationAwaiter(Operation operation) { Operation = operation; }
            public virtual bool IsCompleted => Operation?.IsDone ?? true;
            public virtual void OnCompleted(Action continuation)
            {
                if (IsCompleted) { continuation?.Invoke(); }
                else { Operation?.AddCompleted(continuation); }
            }
            public virtual void GetResult() { }
        }

        public static implicit operator Task(Operation operation) => new TaskCompletionSource<int>(operation).Task;

    }
}
