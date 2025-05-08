using System;

namespace BloodShadow.GameCore.UI
{
    public abstract class UIViewModel<TScreen> : IDisposable
    {
        public UIController<TScreen> Controller;
        public virtual void Dispose() { }

        public UIViewModel(UIController<TScreen> controller) { Controller = controller; }
    }
}
