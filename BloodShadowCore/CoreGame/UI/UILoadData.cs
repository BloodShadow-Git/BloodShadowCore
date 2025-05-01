namespace BloodShadow.CoreGame.UI
{
    public class UILoadData<TScreen> : UIData<TScreen>
    {
        public IUILoadBinder<TScreen> UILoad;
        public UILoadData(IUI<TScreen> screen) : base(screen) { UILoad = screen.GetLoadBinder(); }
    }
}
