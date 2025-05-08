namespace BloodShadow.GameCore.UI
{
    public interface IUI<TScreen>
    {
        TScreen Screen { get; }
        IUIBinder<TScreen> GetBinder();
        IUILoadBinder<TScreen> GetLoadBinder();
    }
}
