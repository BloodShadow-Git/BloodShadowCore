namespace BloodShadowFramework.Localizations
{
    [Serializable]
    public class LocalizationPair<T>(string key, T value) : LocalizationPair
    {
        public override string Key { get; set; } = key;
        public override object Value { get; set; } = value;
    }

    [Serializable]
    public class LocalizationPair
    {
        public virtual string Key { get; set; }
        public virtual object Value { get; set; }
    }
}
