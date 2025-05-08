namespace BloodShadow.GameCore.UI
{
    public class UISetData<TScreen>
    {
        public TScreen[] UIPanels { get; private set; }
        public TScreen[] LoadScreens { get; private set; }

        public UISetData(TScreen[] panels, TScreen[] loadScreens)
        {
            UIPanels = panels;
            LoadScreens = loadScreens;
        }

        public static implicit operator UIPatternSet<TScreen>(UISetData<TScreen> data) => new UIPatternSet<TScreen>(data);
    }
}
