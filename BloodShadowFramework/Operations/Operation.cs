using R3;
using System.Runtime.CompilerServices;

namespace BloodShadowFramework.Operations
{
    public class Operation : IDisposable, ICloneable
    {
        public virtual float Progress { get; protected set; }
        public virtual bool AllowSceneActivation { get; set; }
        public virtual bool IsDone { get; protected set; }
        public virtual int Priority { get; set; }
        public event Action Completed;

        public CancellationTokenSource CancellationTokenSource { get; protected set; } = new();
        private readonly Task _task;
        private readonly Observable<(float progress, OperationTaskProgress taskProgress)> _subject;
        private OperationAwaiter _awaiter;

        public Operation(Func<Task> action, Observable<(float progress, OperationTaskProgress taskProgress)> subject = null)
        {
            _task = action?.Invoke();
            _subject = subject;
            SetupAwaiter();
        }
        public Operation(Action action, Observable<(float progress, OperationTaskProgress taskProgress)> subject = null)
        {
            _task = Task.Factory.StartNew(action, CancellationTokenSource.Token);
            _subject = subject;
            SetupAwaiter();
        }
        public Operation(Task task, Observable<(float progress, OperationTaskProgress taskProgress)> subject = null)
        {
            _task = task;
            _subject = subject;
            SetupAwaiter();
        }
        public Operation() { }
        private void SetupAwaiter()
        {
            _awaiter = new(this);
            Progress = 0f;
            IDisposable disposable = _subject?.Subscribe(data =>
            {
                switch (data.taskProgress)
                {
                    case OperationTaskProgress.Add: Progress += data.progress; break;
                    case OperationTaskProgress.Set: Progress = data.progress; break;
                }
                Progress = Math.Clamp(Progress, 0f, 1f);
            });
            Task.Run(() =>
            {
                while (!CancellationTokenSource.IsCancellationRequested && !_task.IsCompleted)
                {
                    IsDone = _task.IsCompleted;
                    Task.Delay(1);
                }
                disposable?.Dispose();
                Progress = 1f;
                Completed?.Invoke();
                IsDone = true;
            });
        }
        public void Dispose() { GC.SuppressFinalize(this); }
        public virtual OperationAwaiter GetAwaiter() => _awaiter;
        public virtual object Clone() => new Operation(_task, _subject);
        public virtual void Wait() { while (!IsDone) { } }
        public virtual void AddCompleted(Action action) { Completed += action; }


        public class OperationAwaiter : INotifyCompletion
        {
            protected virtual Operation Operation { get; set; }
            public OperationAwaiter() { }
            public OperationAwaiter(Operation operation) { Operation = operation; }
            public virtual bool IsCompleted => Operation.IsDone;
            public virtual void OnCompleted(Action continuation)
            {
                if (IsCompleted) { continuation?.Invoke(); }
                else { Operation.AddCompleted(continuation); }
            }
            public virtual void GetResult() { }
        }

        public static implicit operator Task(Operation operation) => new TaskCompletionSource<int>(operation).Task;

    }

    public enum OperationTaskProgress : byte
    {
        Add = 0,
        Set = 1
    }
}
