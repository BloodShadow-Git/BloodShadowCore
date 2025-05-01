namespace BloodShadow.CoreGame.DayNightCycle
{
    public abstract class DayNightCycle
    {
        public abstract TimeSpan HumanTime { get; }

        protected void UpdateSunSkyFogClouds()
        {
            UpdateDecimalTime(out float time);
            UpdateSunAngle(out float sunAngle);
            RotateSun(time);
            MoveClouds(time);
            SetSunBrightness(sunAngle);
            SetSunColor(sunAngle);
            SetSkyColor(sunAngle);
            MoveStars(sunAngle);
            SetFogColor(sunAngle);
            SetCloudsColor(sunAngle);
        }

        protected abstract void UpdateDecimalTime(out float time);
        protected abstract void UpdateSunAngle(out float sunAngle);
        protected abstract void RotateSun(in float time);
        protected abstract void MoveClouds(in float time);
        protected abstract void SetSunBrightness(in float sunAngle);
        protected abstract void SetSunColor(in float sunAngle);
        protected abstract void SetSkyColor(in float sunAngle);
        protected abstract void MoveStars(in float sunAngle);
        protected abstract void SetFogColor(in float sunAngle);
        protected abstract void SetCloudsColor(in float sunAngle);
    }
}