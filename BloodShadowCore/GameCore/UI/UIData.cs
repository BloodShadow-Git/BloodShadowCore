namespace BloodShadow.GameCore.UI
{
    public class UIData<TScreen> : IDisposable
    {
        public UIPair<TScreen> Pair;
        public List<UIPair<TScreen>> Panels;

        public UIData(IUI<TScreen> ui)
        {
            Pair = new(ui.Screen, ui.GetBinder());
            Panels = [];
        }

        public void Dispose()
        {
            Pair.Dispose();
            foreach (UIPair<TScreen> panel in Panels) { panel.Dispose(); }
        }
    }
}
