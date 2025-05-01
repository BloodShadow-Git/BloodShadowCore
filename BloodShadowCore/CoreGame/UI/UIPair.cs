namespace BloodShadow.CoreGame.UI
{
    public struct UIPair<TScreen> : IDisposable
    {
        public TScreen Screen;
        public IUIBinder<TScreen> UIBase;

        public UIPair(TScreen screen, IUIBinder<TScreen> UIbinder)
        {
            Screen = screen;
            UIBase = UIbinder;
        }

        public void Dispose() { UIBase?.Dispose(); }
    }
}