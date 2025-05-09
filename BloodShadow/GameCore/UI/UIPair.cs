using System;

namespace BloodShadow.GameCore.UI
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

        public readonly void Dispose() { UIBase?.Dispose(); }
    }
}