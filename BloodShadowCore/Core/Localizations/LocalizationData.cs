namespace BloodShadow.Core.Localizations
{
    [Serializable]
    public class LocalizationData
    {
        public string LocalizationKey { get; set; }
        public LocalizationPair[] Pairs { get; set; }
    }
}
