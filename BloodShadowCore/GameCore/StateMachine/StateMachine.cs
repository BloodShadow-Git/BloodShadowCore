namespace BloodShadow.GameCore.StateMachine
{
    public abstract class StateMachine<TCollision>
    {
        public IState<TCollision> CurrentState { get; private set; }

        public void ChangeState(IState<TCollision> newState)
        {
            if (!newState.Enabled) { return; }
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }

        public void Update() { CurrentState?.Update(); }

        public void OnCollisionEnter(TCollision collision) => CurrentState?.OnCollisionEnter(collision);
        public void OnCollisionExit(TCollision collision) => CurrentState?.OnCollisionExit(collision);

        public void OnAnimationEnter() => CurrentState?.OnAnimationEnter();
        public void OnAnimationTransition() => CurrentState?.OnAnimationTransition();
        public void OnAnimationExit() => CurrentState?.OnAnimationExit();
    }
}
