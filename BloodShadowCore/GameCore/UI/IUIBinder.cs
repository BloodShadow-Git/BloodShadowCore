namespace BloodShadow.GameCore.UI
{
    public interface IUIBinder<TScreen> : IDisposable
    {
        void Bind(UIController<TScreen> controller, int index, UIViewModel<TScreen> viewModel);
        void InternalReset();
        void Enable();
        void Disable();
    }
}
