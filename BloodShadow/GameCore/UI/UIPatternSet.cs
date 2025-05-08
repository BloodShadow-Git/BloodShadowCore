using System;
using System.Collections.Generic;

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
            if (panels != null) { Panels = Array.ConvertAll(panels, panel => new UIPattern<TScreen>(panel)); }
            else { Panels = Array.Empty<UIPattern<TScreen>>(); }

            if (loads != null) { Loads = Array.ConvertAll(loads, load => new UIPattern<TScreen>(load)); ; }
            else { Loads = Array.Empty<UIPattern<TScreen>>(); }
        }
        public UIPatternSet(TScreen[] panels, UIViewModel<TScreen>[] panelsVM, TScreen[] loads, UIViewModel<TScreen>[] loadsVM)
        {
            if (panels != null)
            {
                Panels = new UIPattern<TScreen>[panels.Length];
                for (int panel = 0; panels.Length < panel; panel++)
                {
                    if (panel < panelsVM.Length) { Panels[panel] = new UIPattern<TScreen>(panels[panel], panelsVM[panel]); }
                    else { Panels[panel] = panels[panel]; }
                }
            }
            else { Panels = Array.Empty<UIPattern<TScreen>>(); }
            if (loads != null)
            {
                Loads = new UIPattern<TScreen>[loads.Length];
                for (int load = 0; loads.Length < load; load++)
                {
                    if (load < loadsVM.Length) { Panels[load] = new UIPattern<TScreen>(loads[load], loadsVM[load]); }
                    else { Panels[load] = loads[load]; }
                }
            }
            else { Loads = Array.Empty<UIPattern<TScreen>>(); }
        }
        public UIPatternSet(TScreen[] panels, UIViewModel<TScreen> panelVM, TScreen[] loads, UIViewModel<TScreen>[] loadsVM)
        {
            if (panels != null)
            {
                Panels = new UIPattern<TScreen>[panels.Length];
                for (int panel = 0; panels.Length < panel; panel++) { Panels[panel] = new UIPattern<TScreen>(panels[panel], panelVM); }
            }
            else { Panels = Array.Empty<UIPattern<TScreen>>(); }
            if (loads != null)
            {
                Loads = new UIPattern<TScreen>[loads.Length];
                for (int load = 0; loads.Length < load; load++)
                {
                    if (load < loadsVM.Length) { Panels[load] = new UIPattern<TScreen>(loads[load], loadsVM[load]); }
                    else { Panels[load] = loads[load]; }
                }
            }
            else { Loads = Array.Empty<UIPattern<TScreen>>(); }
        }
        public UIPatternSet(TScreen[] panels, UIViewModel<TScreen>[] panelsVM, TScreen[] loads, UIViewModel<TScreen> loadVM)
        {
            if (panels != null)
            {
                Panels = new UIPattern<TScreen>[panels.Length];
                for (int panel = 0; panels.Length < panel; panel++)
                {
                    if (panel < panelsVM.Length) { Panels[panel] = new UIPattern<TScreen>(panels[panel], panelsVM[panel]); }
                    else { Panels[panel] = panels[panel]; }
                }
            }
            else { Panels = Array.Empty<UIPattern<TScreen>>(); }
            if (loads != null)
            {
                Loads = new UIPattern<TScreen>[loads.Length];
                for (int load = 0; loads.Length < load; load++) { Panels[load] = new UIPattern<TScreen>(loads[load], loadVM); }
            }
            else { Loads = Array.Empty<UIPattern<TScreen>>(); }
        }
        public UIPatternSet(TScreen[] panels, UIViewModel<TScreen> panelVM, TScreen[] loads, UIViewModel<TScreen> loadVM)
        {
            if (panels != null)
            {
                Panels = new UIPattern<TScreen>[panels.Length];
                for (int panel = 0; panels.Length < panel; panel++) { Panels[panel] = new UIPattern<TScreen>(panels[panel], panelVM); }
            }
            else { Panels = Array.Empty<UIPattern<TScreen>>(); }
            if (loads != null)
            {
                Loads = new UIPattern<TScreen>[loads.Length];
                for (int load = 0; loads.Length < load; load++) { Panels[load] = new UIPattern<TScreen>(loads[load], loadVM); }
            }
            else { Loads = Array.Empty<UIPattern<TScreen>>(); }
        }

        public UIPatternSet(UISetData<TScreen> set)
        {
            if (set == null)
            {
                Panels = Array.Empty<UIPattern<TScreen>>();
                Loads = Array.Empty<UIPattern<TScreen>>();
                return;
            }

            if (set.UIPanels != null) { Panels = Array.ConvertAll(set.UIPanels, panel => new UIPattern<TScreen>(panel)); ; }
            else { Panels = Array.Empty<UIPattern<TScreen>>(); }

            if (set.LoadScreens != null) { Loads = Array.ConvertAll(set.LoadScreens, load => new UIPattern<TScreen>(load)); ; }
            else { Loads = Array.Empty<UIPattern<TScreen>>(); }
        }
        public UIPatternSet(UISetData<TScreen> set, UIViewModel<TScreen>[] panelsVM, UIViewModel<TScreen>[] loadsVM) : this(set)
        {
            if (set.UIPanels != null)
            {
                List<UIPattern<TScreen>> panels = new List<UIPattern<TScreen>>();
                for (int i = 0; i < set.UIPanels.Length; i++)
                {
                    if (i < panelsVM.Length) { panels.Add(new UIPattern<TScreen>(set.UIPanels[i], panelsVM[i])); }
                    else { panels.Add(set.LoadScreens[i]); }
                }
                Panels = panels.ToArray();
            }
            else { Panels = Array.Empty<UIPattern<TScreen>>(); }

            if (set.LoadScreens != null)
            {
                List<UIPattern<TScreen>> loads = new List<UIPattern<TScreen>>();
                for (int i = 0; i < set.LoadScreens.Length; i++)
                {
                    if (i < loadsVM.Length) { loads.Add(new UIPattern<TScreen>(set.LoadScreens[i], loadsVM[i])); }
                    else { loads.Add(set.LoadScreens[i]); }
                }
                Loads = loads.ToArray();
            }
            else { Loads = Array.Empty<UIPattern<TScreen>>(); }
        }
        public UIPatternSet(UISetData<TScreen> set, UIViewModel<TScreen>[] panelsVM, UIViewModel<TScreen> loadVM) : this(set)
        {
            if (set.UIPanels != null)
            {
                List<UIPattern<TScreen>> panels = new List<UIPattern<TScreen>>();
                for (int i = 0; i < set.UIPanels.Length; i++)
                {
                    if (i < panelsVM.Length) { panels.Add(new UIPattern<TScreen>(set.UIPanels[i], panelsVM[i])); }
                    else { panels.Add(set.LoadScreens[i]); }
                }
                Panels = panels.ToArray();
            }
            else { Panels = Array.Empty<UIPattern<TScreen>>(); }

            if (set.LoadScreens != null)
            {
                List<UIPattern<TScreen>> loads = new List<UIPattern<TScreen>>();
                foreach (TScreen ui in set.LoadScreens) { loads.Add(new UIPattern<TScreen>(ui, loadVM)); }
                Loads = loads.ToArray();
            }
            else { Loads = Array.Empty<UIPattern<TScreen>>(); }
        }
        public UIPatternSet(UISetData<TScreen> set, UIViewModel<TScreen> panelVM, UIViewModel<TScreen>[] loadsVM) : this(set)
        {
            if (set.UIPanels != null)
            {
                List<UIPattern<TScreen>> panels = new List<UIPattern<TScreen>>();
                foreach (TScreen ui in set.UIPanels) { panels.Add(new UIPattern<TScreen>(ui, panelVM)); }
                Panels = panels.ToArray();
            }
            else { Panels = Array.Empty<UIPattern<TScreen>>(); }

            if (set.LoadScreens != null)
            {
                List<UIPattern<TScreen>> loads = new List<UIPattern<TScreen>>();
                for (int i = 0; i < set.LoadScreens.Length; i++)
                {
                    if (i < loadsVM.Length) { loads.Add(new UIPattern<TScreen>(set.LoadScreens[i], loadsVM[i])); }
                    else { loads.Add(set.LoadScreens[i]); }
                }
                Loads = loads.ToArray();
            }
            else { Loads = Array.Empty<UIPattern<TScreen>>(); }
        }
        public UIPatternSet(UISetData<TScreen> set, UIViewModel<TScreen> panelVM, UIViewModel<TScreen> loadVM) : this(set)
        {
            if (set.UIPanels != null)
            {
                List<UIPattern<TScreen>> panels = new List<UIPattern<TScreen>>();
                foreach (TScreen ui in set.UIPanels) { panels.Add(new UIPattern<TScreen>(ui, panelVM)); }
                Panels = panels.ToArray();
            }
            else { Panels = Array.Empty<UIPattern<TScreen>>(); }

            if (set.LoadScreens != null)
            {
                List<UIPattern<TScreen>> loads = new List<UIPattern<TScreen>>();
                foreach (TScreen ui in set.LoadScreens) { loads.Add(new UIPattern<TScreen>(ui, loadVM)); }
                Loads = loads.ToArray();
            }
            else { Loads = Array.Empty<UIPattern<TScreen>>(); }
        }
    }
}
