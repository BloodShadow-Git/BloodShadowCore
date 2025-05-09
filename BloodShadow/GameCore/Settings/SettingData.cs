﻿namespace BloodShadow.GameCore.Settings
{
    public abstract class SettingData<TScreen>
    {
        public abstract void UpdateData();
        public abstract void CreateSettingScreen(TScreen screen);
    }
}
