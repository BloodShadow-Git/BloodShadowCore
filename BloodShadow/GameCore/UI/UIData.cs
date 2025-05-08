using System;
using System.Collections.Generic;

namespace BloodShadow.GameCore.UI
{
    public class UIData<TScreen> : IDisposable
    {
        public UIPair<TScreen> Pair;
        public List<UIPair<TScreen>> Panels;

        public UIData(IUI<TScreen> ui)
        {
            Pair = new UIPair<TScreen>(ui.Screen, ui.GetBinder());
            Panels = new List<UIPair<TScreen>>();
        }

        public void Dispose()
        {
            Pair.Dispose();
            foreach (UIPair<TScreen> panel in Panels) { panel.Dispose(); }
        }
    }
}
