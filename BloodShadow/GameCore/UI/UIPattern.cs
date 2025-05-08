namespace BloodShadow.GameCore.UI
{
    public class UIPattern<TScreen>
    {
        public TScreen ViewScreen { get; private set; }
        public UIViewModel<TScreen>? ViewModel { get; private set; }

        public UIPattern(TScreen viewScreen, UIViewModel<TScreen> viewModel)
        {
            ViewScreen = viewScreen;
            ViewModel = viewModel;
        }
        public UIPattern(TScreen viewScreen)
        {
            ViewScreen = viewScreen;
            ViewModel = null;
        }

        public static implicit operator UIPattern<TScreen>(TScreen screen) => new UIPattern<TScreen>(screen);
    }
}
