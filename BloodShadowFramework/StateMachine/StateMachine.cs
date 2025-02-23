namespace BloodShadowFramework.StateMachine
{
    public abstract class StateMachine
    {
        public IState CurrentState { get; private set; }

        public void ChangeState(IState newState)
        {
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }

        public void Update() { CurrentState?.Update(); }

        public void OnAnimationEnter() { CurrentState?.OnAnimationEnter(); }
        public void OnAnimationTransition() { CurrentState?.OnAnimationTransition(); }
        public void OnAnimationExit() { CurrentState?.OnAnimationExit(); }
    }
}
