namespace BloodShadow.GameCore.StateMachine
{
    public interface IState<TCollision>
    {
        bool Enabled { get; }

        void Enter();
        void Exit();

        void OnCollisionEnter(TCollision collision);
        void OnCollisionExit(TCollision collision);
        void Update();

        void OnAnimationEnter();
        void OnAnimationTransition();
        void OnAnimationExit();
    }
}
