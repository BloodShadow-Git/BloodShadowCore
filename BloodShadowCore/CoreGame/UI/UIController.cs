using BloodShadow.Core.Operations;

namespace BloodShadow.CoreGame.UI
{
    public abstract class UIController<TScreen, TOrigin> : UIController<TScreen>
    {
        public abstract void Resetup(int newActiveIndex, TOrigin newOrigin, UIPatternSet<TScreen> newSet);
        public abstract void Resetup(int newActiveIndex, TOrigin newOrigin);
        public abstract void Resetup(TOrigin newOrigin, UIPatternSet<TScreen> newSet);
        public abstract void Resetup(TOrigin newOrigin);
    }
    public abstract class UIController<TScreen> : IDisposable
    {
        public abstract void SwitchRandom(Operation operation);
        public abstract void Switch(int index);
        public abstract void Switch(int index, Operation operation);
        public abstract void Switch(int UIIndex, int panelIndex);
        public abstract void Switch(int UIIndex, int panelIndex, UIViewModel<TScreen> VM);
        public abstract void Disable();
        public abstract void Enable();
        public abstract int AddPanel(int UIIndex, IUI<TScreen> UIScreen, UIViewModel<TScreen> VM);
        public abstract int AddPanel(int UIIndex, UIPattern<TScreen> Pattern);
        public abstract void RemovePanel(int UIIndex, int panelIndex);
        public abstract void Prev();
        public abstract void Resetup(int newActiveIndex, UIPatternSet<TScreen> newSet);
        public abstract void Resetup(int newActiveIndex);
        public abstract void Resetup(UIPatternSet<TScreen> newSet);
        public abstract void Reset();
        public abstract int GetLoadsCount();
        public abstract int GetUIsCount();
        public abstract int GetScreensCount();
        public abstract void Dispose();
    }
}
