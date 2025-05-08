namespace BloodShadow.GameCore.Localizations
{
    public class LocalizationPair<T> : LocalizationPair
    {
        public override string Key { get; set; }
        public override object Value { get; set; }
        public T TValue => (T)Value;

        public LocalizationPair(string key, T value) : base(key, value ?? new object())
        {
            Key = key;
            Value = value ?? new object();
        }
    }

    public class LocalizationPair
    {
        public virtual string Key { get; set; }
        public virtual object Value { get; set; }
        public LocalizationPair(string key, object value)
        {
            Key = key;
            Value = value;
        }
    }
}
