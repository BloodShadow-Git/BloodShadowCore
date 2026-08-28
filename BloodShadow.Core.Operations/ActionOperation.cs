using R3;
using System.Runtime.CompilerServices;

namespace BloodShadow.Core.Operations
{
    public class ActionOperation : Operation
    {
        public override ReadOnlyReactiveProperty<float> Progress => _progress;
        public override ReactiveProperty<bool> AllowLevelActivation => _allowLevelActivation;
        public override ReadOnlyReactiveProperty<bool> IsDone => _isDone;
        public override ReactiveProperty<int> Priority => _priority;
        public override ReadOnlyReactiveProperty<string> Description => _description;

        protected readonly ReactiveProperty<float> _progress;
        protected readonly ReactiveProperty<bool> _allowLevelActivation;
        protected readonly ReactiveProperty<bool> _isDone;
        protected readonly ReactiveProperty<int> _priority;
        protected readonly ReactiveProperty<string> _description;
        protected readonly CancellationTokenSource _tokenSource;
        protected ActionOperationProgress? _aop;

        protected readonly CompositeDisposable _compositeDisposable;

        private readonly Task? _task;

        protected ActionOperation()
        {
            _progress = new();
            _allowLevelActivation = new();
            _isDone = new();
            _priority = new();
            _description = new();
            _tokenSource = new();
            _compositeDisposable = [];
        }
        public ActionOperation(Func<ActionOperationProgress, Task> func) : this()
        {
            _aop = new();
            SetupAOP();
            _task = func.Invoke(_aop);
        }
        public ActionOperation(Action<ActionOperationProgress> action) : this()
        {
            _aop = new();
            SetupAOP();
            _task = new(() => action?.Invoke(_aop), _tokenSource.Token);
        }
        public ActionOperation(Func<Task> func) : this() { _task = func.Invoke(); }
        public ActionOperation(Action action) : this() { _task = new Task(action, _tokenSource.Token); }
        public ActionOperation(Task task) : this() { _task = task; }
        private ActionOperation(ActionOperation ao)
        {
            _progress = new();
            _allowLevelActivation = new();
            _isDone = new();
            _priority = new();
            _description = new();
            _tokenSource = new();
            _compositeDisposable = [];

            _task = ao._task;
            if (ao._aop != null)
            {
                _aop = ao._aop;
                SetupAOP();
            }
        }

        protected void SetupAOP()
        {
            if (_aop != null)
            {
                _compositeDisposable.Add(_aop.Progress.Subscribe(_ => _progress.Value = _aop.Progress.CurrentValue));
                _compositeDisposable.Add(_allowLevelActivation.Subscribe(_ => _aop.AllowSceneActivation.Value = _allowLevelActivation.CurrentValue));
                _compositeDisposable.Add(_aop.IsDone.Subscribe(_ => _isDone.Value = _aop.IsDone.CurrentValue));
                _compositeDisposable.Add(_priority.Subscribe(_ => _aop.Priority.Value = _priority.CurrentValue));
                _compositeDisposable.Add(_aop.Description.Subscribe(_ => _description.Value = _aop.Description.CurrentValue));
            }
            else { throw new NullReferenceException("AOP is null"); }
        }

        public override OperationAwaiter GetAwaiter()
        {
            if (_awaiter == null)
            {
                _awaiter = new(this);
                _progress.Value = 0f;

                Task.Run(() =>
                {
                    _isDone.Value = false;
                    _task?.Wait();
                    Dispose();
                }, _tokenSource.Token);
            }
            return _awaiter;
        }

        public override Operation Start()
        {
            if ((_task?.Status ?? TaskStatus.Faulted) == TaskStatus.Created) { _task?.Start(); }
            return this;
        }
        public override object Clone() => new ActionOperation(this);
        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            _tokenSource?.Cancel();
            _task?.Dispose();
            _compositeDisposable?.Dispose();

            _progress.Value = 1f;
            _isDone.Value = true;
            OnCompletedAction?.Invoke();
        }

        public override void Wait() { _task?.Wait(); }
    }

    public class ActionOperation<T> : ActionOperation
    {
        public T? Result { get; private set; }
        new protected OperationAwaiter? _awaiter;

        private readonly Task<T?> _task;

        public ActionOperation(Func<ActionOperationProgress, Task<T?>> func) : base()
        {
            _aop = new();
            SetupAOP();
            _task = func.Invoke(_aop);
            SetupAwaiter();
        }
        public ActionOperation(Func<ActionOperationProgress, T?> action) : base()
        {
            _aop = new ActionOperationProgress();
            SetupAOP();
            _task = new(() => action.Invoke(_aop), _tokenSource.Token);
            SetupAwaiter();
        }
        public ActionOperation(Func<Task<T?>> func) : base()
        {
            _task = func.Invoke();
            SetupAwaiter();
        }
        public ActionOperation(Func<T?> func) : base()
        {
            _task = new(func, _tokenSource.Token);
            SetupAwaiter();
        }
        public ActionOperation(Task<T?> task) : base()
        {
            _task = task;
            SetupAwaiter();
        }
        private ActionOperation(ActionOperation<T> ao)
        {
            _task = ao._task;
            if (ao._aop != null)
            {
                _aop = ao._aop;
                SetupAOP();
            }
            SetupAwaiter();
        }
        private void SetupAwaiter()
        {
            _awaiter = new(this);
            _progress.Value = 0f;

            Task.Run(() =>
            {
                while (!_tokenSource.IsCancellationRequested && !(_task?.IsCompleted ?? false)) { _isDone.Value = false; }
                Dispose();
            });
        }
        new public virtual OperationAwaiter GetAwaiter()
        {
            Start();
            _awaiter ??= new(this);
            return _awaiter;
        }

        public override Operation Start()
        {
            if (_task.Status == TaskStatus.Created) { _task.Start(); }
            return this;
        }
        public override object Clone() => new ActionOperation<T>(this);
        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            _tokenSource?.Cancel();
            _task?.Dispose();
            _compositeDisposable?.Dispose();

            _progress.Value = 1f;
            Result = _task == null ? default : _task.Result;
            _isDone.Value = true;
            OnCompletedAction?.Invoke();
        }
        public override void Wait() { _task.Wait(); }

        new public class OperationAwaiter(ActionOperation<T> operation) : INotifyCompletion, IDisposable
        {
            protected virtual ActionOperation<T> Operation { get; set; } = operation;
            public virtual bool IsCompleted => Operation?.IsDone.CurrentValue ?? false;
            public virtual void OnCompleted(Action continuation)
            {
                if (IsCompleted) { continuation?.Invoke(); }
                else { Operation?.AddCompleted(continuation); }
            }
            public virtual T? GetResult()
            {
                Operation.Wait();
                return Operation.Result;
            }
            public virtual void Dispose()
            {
                GC.SuppressFinalize(this);
                try { Operation?.Dispose(); } catch { }
            }
        }
    }

    public class ActionOperationProgress : IDisposable
    {
        public readonly ReactiveProperty<float> Progress;
        public readonly ReactiveProperty<bool> AllowSceneActivation;
        public readonly ReactiveProperty<bool> IsDone;
        public readonly ReactiveProperty<int> Priority;
        public readonly ReactiveProperty<string> Description;

        public ActionOperationProgress()
        {
            Progress = new();
            AllowSceneActivation = new();
            IsDone = new();
            Priority = new();
            Description = new();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Progress.Dispose();
            AllowSceneActivation.Dispose();
            IsDone.Dispose();
            Priority.Dispose();
            Description.Dispose();
        }
    }
}
