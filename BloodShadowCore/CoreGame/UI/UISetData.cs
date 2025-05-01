namespace BloodShadow.CoreGame.UI
{
    public class UISetData<TScreen>
    {
        public TScreen[] UIPanels { get; private set; }
        public TScreen[] LoadScreens { get; private set; }

        public static implicit operator UIPatternSet<TScreen>(UISetData<TScreen> data) => new(data);
    }
}
