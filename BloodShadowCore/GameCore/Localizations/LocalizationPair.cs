namespace BloodShadow.GameCore.Localizations
{
    public class LocalizationPair<T>(string key, T value) : LocalizationPair(key, value)
    {
        public override string Key { get; set; }
        public override object Value { get; set; }
    }

    public class LocalizationPair(string key, object value)
    {
        public virtual string Key { get; set; } = key;
        public virtual object Value { get; set; } = value;
    }
}
