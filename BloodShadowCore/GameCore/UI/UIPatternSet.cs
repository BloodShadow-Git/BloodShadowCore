namespace BloodShadow.GameCore.UI
{
    public class UIPatternSet<TScreen>
    {
        public UIPattern<TScreen>[] Panels { get; private set; }
        public UIPattern<TScreen>[] Loads { get; private set; }

        public UIPatternSet(UIPattern<TScreen>[] panels, UIPattern<TScreen>[] loads)
        {
            Panels = panels;
            Loads = loads;
        }

        public UIPatternSet(TScreen[] panels, TScreen[] loads)
        {
            if (panels != null) { Panels = [.. panels]; }
            else { Panels = []; }

            if (loads != null) { Loads = [.. loads]; }
            else { Loads = []; }
        }
        public UIPatternSet(TScreen[] panels, UIViewModel<TScreen>[] panelsVM, TScreen[] loads, UIViewModel<TScreen>[] loadsVM)
        {
            if (panels != null)
            {
                Panels = new UIPattern<TScreen>[panels.Length];
                for (int panel = 0; panels.Length < panel; panel++)
                {
                    if (panel < panelsVM.Length) { Panels[panel] = new(panels[panel], panelsVM[panel]); }
                    else { Panels[panel] = panels[panel]; }
                }
            }
            else { Panels = []; }
            if (loads != null)
            {
                Loads = new UIPattern<TScreen>[loads.Length];
                for (int load = 0; loads.Length < load; load++)
                {
                    if (load < loadsVM.Length) { Panels[load] = new(loads[load], loadsVM[load]); }
                    else { Panels[load] = loads[load]; }
                }
            }
            else { Loads = []; }
        }
        public UIPatternSet(TScreen[] panels, UIViewModel<TScreen> panelVM, TScreen[] loads, UIViewModel<TScreen>[] loadsVM)
        {
            if (panels != null)
            {
                Panels = new UIPattern<TScreen>[panels.Length];
                for (int panel = 0; panels.Length < panel; panel++) { Panels[panel] = new(panels[panel], panelVM); }
            }
            else { Panels = []; }
            if (loads != null)
            {
                Loads = new UIPattern<TScreen>[loads.Length];
                for (int load = 0; loads.Length < load; load++)
                {
                    if (load < loadsVM.Length) { Panels[load] = new(loads[load], loadsVM[load]); }
                    else { Panels[load] = loads[load]; }
                }
            }
            else { Loads = []; }
        }
        public UIPatternSet(TScreen[] panels, UIViewModel<TScreen>[] panelsVM, TScreen[] loads, UIViewModel<TScreen> loadVM)
        {
            if (panels != null)
            {
                Panels = new UIPattern<TScreen>[panels.Length];
                for (int panel = 0; panels.Length < panel; panel++)
                {
                    if (panel < panelsVM.Length) { Panels[panel] = new(panels[panel], panelsVM[panel]); }
                    else { Panels[panel] = panels[panel]; }
                }
            }
            else { Panels = []; }
            if (loads != null)
            {
                Loads = new UIPattern<TScreen>[loads.Length];
                for (int load = 0; loads.Length < load; load++) { Panels[load] = new(loads[load], loadVM); }
            }
            else { Loads = []; }
        }
        public UIPatternSet(TScreen[] panels, UIViewModel<TScreen> panelVM, TScreen[] loads, UIViewModel<TScreen> loadVM)
        {
            if (panels != null)
            {
                Panels = new UIPattern<TScreen>[panels.Length];
                for (int panel = 0; panels.Length < panel; panel++) { Panels[panel] = new(panels[panel], panelVM); }
            }
            else { Panels = []; }
            if (loads != null)
            {
                Loads = new UIPattern<TScreen>[loads.Length];
                for (int load = 0; loads.Length < load; load++) { Panels[load] = new(loads[load], loadVM); }
            }
            else { Loads = []; }
        }

        public UIPatternSet(UISetData<TScreen> set)
        {
            if (set == null)
            {
                Panels = [];
                Loads = [];
                return;
            }

            if (set.UIPanels != null) { Panels = [.. set.UIPanels]; }
            else { Panels = []; }

            if (set.LoadScreens != null) { Loads = [.. set.LoadScreens]; }
            else { Loads = []; }
        }
        public UIPatternSet(UISetData<TScreen> set, UIViewModel<TScreen>[] panelsVM, UIViewModel<TScreen>[] loadsVM) : this(set)
        {
            if (set.UIPanels != null)
            {
                List<UIPattern<TScreen>> panels = [];
                for (int i = 0; i < set.UIPanels.Length; i++)
                {
                    if (i < panelsVM.Length) { panels.Add(new(set.UIPanels[i], panelsVM[i])); }
                    else { panels.Add(set.LoadScreens[i]); }
                }
                Panels = [.. panels];
            }
            else { Panels = []; }

            if (set.LoadScreens != null)
            {
                List<UIPattern<TScreen>> loads = [];
                for (int i = 0; i < set.LoadScreens.Length; i++)
                {
                    if (i < loadsVM.Length) { loads.Add(new(set.LoadScreens[i], loadsVM[i])); }
                    else { loads.Add(set.LoadScreens[i]); }
                }
                Loads = [.. loads];
            }
            else { Loads = []; }
        }
        public UIPatternSet(UISetData<TScreen> set, UIViewModel<TScreen>[] panelsVM, UIViewModel<TScreen> loadVM) : this(set)
        {
            if (set.UIPanels != null)
            {
                List<UIPattern<TScreen>> panels = [];
                for (int i = 0; i < set.UIPanels.Length; i++)
                {
                    if (i < panelsVM.Length) { panels.Add(new(set.UIPanels[i], panelsVM[i])); }
                    else { panels.Add(set.LoadScreens[i]); }
                }
                Panels = [.. panels];
            }
            else { Panels = []; }

            if (set.LoadScreens != null)
            {
                List<UIPattern<TScreen>> loads = [];
                foreach (TScreen ui in set.LoadScreens) { loads.Add(new(ui, loadVM)); }
                Loads = [.. loads];
            }
            else { Loads = []; }
        }
        public UIPatternSet(UISetData<TScreen> set, UIViewModel<TScreen> panelVM, UIViewModel<TScreen>[] loadsVM) : this(set)
        {
            if (set.UIPanels != null)
            {
                List<UIPattern<TScreen>> panels = [];
                foreach (TScreen ui in set.UIPanels) { panels.Add(new(ui, panelVM)); }
                Panels = [.. panels];
            }
            else { Panels = []; }

            if (set.LoadScreens != null)
            {
                List<UIPattern<TScreen>> loads = [];
                for (int i = 0; i < set.LoadScreens.Length; i++)
                {
                    if (i < loadsVM.Length) { loads.Add(new(set.LoadScreens[i], loadsVM[i])); }
                    else { loads.Add(set.LoadScreens[i]); }
                }
                Loads = [.. loads];
            }
            else { Loads = []; }
        }
        public UIPatternSet(UISetData<TScreen> set, UIViewModel<TScreen> panelVM, UIViewModel<TScreen> loadVM) : this(set)
        {
            if (set.UIPanels != null)
            {
                List<UIPattern<TScreen>> panels = [];
                foreach (TScreen ui in set.UIPanels) { panels.Add(new(ui, panelVM)); }
                Panels = [.. panels];
            }
            else { Panels = []; }

            if (set.LoadScreens != null)
            {
                List<UIPattern<TScreen>> loads = [];
                foreach (TScreen ui in set.LoadScreens) { loads.Add(new(ui, loadVM)); }
                Loads = [.. loads];
            }
            else { Loads = []; }
        }
    }
}
