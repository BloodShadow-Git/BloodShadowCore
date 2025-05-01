namespace BloodShadow.Core.StateMachine
{
    public interface IState
    {
        public void Enter();
        public void Exit();
        public void Update();

        public void OnAnimationEnter();
        public void OnAnimationTransition();
        public void OnAnimationExit();
    }
}
